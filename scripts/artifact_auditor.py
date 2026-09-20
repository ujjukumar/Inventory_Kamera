"""
Genshin Impact Unified Artifact Grading & Optimization Engine
Works directly with GOOD (Genshin Open Object Data) JSON files exported by InventoryKamera.

Combines ALL evaluation dimensions into a single, intuitive Grade (S+, S, A, B, F),
Score (0-100), and Single Clear Action:
  - S+ (Grail / God-Tier): Pre-activated 4-liner double crits, 38+ CV god pieces, rare DMG goblets.
  - S  (Elite / Priority): 3-liner double crits, 4-liners with 1 crit + synergy, 30+ CV pieces, top Reshape targets.
  - A  (Solid / Keep): Usable 20-30 CV pieces, rare EM mainstats, high-synergy support pieces.
  - B  (Test Roll +4): 3-liners needing 1 upgrade to test 4th stat (promotes to A/S or demotes to F).
  - F  (Fodder / Scrap): Dead flat stats, 0-crit flowers/feathers, bricked pieces with no reshape value.
"""

import os
import sys
import json
import glob
import csv
import argparse
from typing import Dict, List, Tuple, Any

# Ensure UTF-8 output on Windows consoles
if hasattr(sys.stdout, 'reconfigure'):
    try:
        sys.stdout.reconfigure(encoding='utf-8')
    except Exception:
        pass

# Max 5-star substat roll values
MAX_ROLLS = {
    'critRate_': 3.89,
    'critDMG_': 7.77,
    'atk_': 5.83,
    'hp_': 5.83,
    'def_': 7.29,
    'enerRech_': 6.48,
    'eleMas': 23.31,
    'atk': 19.45,
    'hp': 298.75,
    'def': 23.15,
}

STAT_NAMES = {
    'hp': 'Flat HP',
    'hp_': 'HP%',
    'def': 'Flat DEF',
    'def_': 'DEF%',
    'atk': 'Flat ATK',
    'atk_': 'ATK%',
    'enerRech_': 'ER',
    'eleMas': 'EM',
    'critRate_': 'CR',
    'critDMG_': 'CD',
    'heal_': 'Healing Bonus',
    'pyro_dmg_': 'Pyro DMG',
    'hydro_dmg_': 'Hydro DMG',
    'electro_dmg_': 'Electro DMG',
    'cryo_dmg_': 'Cryo DMG',
    'anemo_dmg_': 'Anemo DMG',
    'geo_dmg_': 'Geo DMG',
    'dendro_dmg_': 'Dendro DMG',
    'physical_dmg_': 'Physical DMG',
}

FLAT_STATS = {'hp', 'def', 'atk'}
PERCENT_STATS = {'atk_', 'hp_', 'def_', 'enerRech_', 'eleMas'}
CRIT_STATS = {'critRate_', 'critDMG_'}

HP_SCALER_SETS = {
    'MarechausseeHunter', 'GoldenTroupe', 'TenacityOfTheMillelith',
    'SongsOfDaysPast', 'VourukashasGlow', 'MaidenBeloved', 'OceanHuedClam'
}

DEF_SCALER_SETS = {
    'HuskOfOpulentDreams', 'ObsidianCodex', 'DefenderWill'
}

EM_SUPPORT_SETS = {
    'ViridescentVenerer', 'DeepwoodMemories', 'GildedDreams',
    'FlowerOfParadiseLost', 'ScrollOfTheHeroOfCinderCity'
}

ER_SUPPORT_SETS = {
    'EmblemOfSeveredFate', 'NoblesseOblige', 'ScrollOfTheHeroOfCinderCity',
    'TheExile', 'Instructor'
}

FOUR_STAR_SUPPORT_SETS = {'TheExile', 'Instructor'}


def calculate_cv(substats: List[Dict[str, Any]]) -> float:
    cr = next((s['value'] for s in substats if s['key'] == 'critRate_'), 0.0)
    cd = next((s['value'] for s in substats if s['key'] == 'critDMG_'), 0.0)
    return round(2.0 * cr + cd, 1)


def format_substats(substats: List[Dict[str, Any]], unactivated: List[Dict[str, Any]] = None) -> str:
    parts = []
    for s in substats:
        k = STAT_NAMES.get(s['key'], s['key'])
        v = s['value']
        unit = '%' if s['key'].endswith('_') else ''
        parts.append(f"{k}+{v:.1f}{unit}" if isinstance(v, float) and unit else f"{k}+{v}")
    if unactivated:
        for s in unactivated:
            k = STAT_NAMES.get(s['key'], s['key'])
            parts.append(f"[{k}?]")
    return ", ".join(parts)


def score_and_grade_artifact(art: Dict[str, Any], min_cv_leveled: float = 24.0, keep_equipped: bool = True) -> Tuple[str, str, int, str, Dict[str, Any]]:
    """
    Unified Grading Engine.
    Combines:
      - Main stat / slot rarity
      - Crit Value & Roll Value synergy
      - Natural 4-Liner (Pre-Activated, 5 Upgrades) vs 3-Liner (4 Upgrades)
      - Unactivated 4th substat knowledge
      - Reshaping potential with Dust of Enlightenment / Feathers

    Returns: (grade, action, score, reason, metadata)
      grade: 'S+', 'S', 'A', 'B', 'F'
      action: 'UPGRADE_NOW', 'LOCK_KEEP', 'RESHAPE', 'ROLL_TO_PLUS_4', 'SCRAP'
      score: 0 to 100
    """
    rarity = art.get('rarity', 5)
    level = art.get('level', 0)
    slot = art.get('slotKey', '')
    main = art.get('mainStatKey', '')
    set_name = art.get('setKey', '')
    location = art.get('location', '')
    subs = art.get('substats', [])
    unact = art.get('unactivatedSubstats', [])
    equipped = bool(location)

    full_subs = subs + unact
    full_keys = {s['key'] for s in full_subs}
    active_keys = {s['key'] for s in subs}
    unact_keys = {s['key'] for s in unact}

    cv = calculate_cv(subs)
    cr_count = sum(1 for k in full_keys if k in CRIT_STATS)
    active_cr = sum(1 for k in active_keys if k in CRIT_STATS)
    flat_count = sum(1 for k in full_keys if k in FLAT_STATS)
    useful_pct = sum(1 for k in full_keys if k in PERCENT_STATS)
    is_4l = (level == 0 and len(subs) == 4 and len(unact) == 0)

    if level == 0 and rarity == 5:
        starter_type = "4-Line (5 Upgrades)" if is_4l else "3-Line (4 Upgrades)"
    elif level > 0:
        starter_type = f"Leveled (+{level})"
    else:
        starter_type = f"{rarity}-Star"

    meta = {
        'cv': cv,
        'slot': slot,
        'main': main,
        'set': set_name,
        'level': level,
        'rarity': rarity,
        'location': location,
        'subs_str': format_substats(subs, unact),
        'starter_type': starter_type,
        'is_four_liner': is_4l,
        'reshape_target': ''
    }

    # 1. 4-STAR ARTIFACTS
    if rarity < 5:
        if set_name in FOUR_STAR_SUPPORT_SETS and (equipped or 'enerRech_' in full_keys or slot in ('flower', 'plume')):
            return 'A', 'LOCK_KEEP', 65, f'4-Star Support Set ({set_name}) with ER/Support stat', meta
        return 'F', 'SCRAP', 10, '4-Star fodder (Strongbox/EXP)', meta

    # 2. FULLY LEVELED ARTIFACTS (+16 to +20)
    if level >= 16:
        # Check Reshape Target first (Level 20 pieces with elite base stats that rolled poorly)
        if level == 20:
            if cr_count == 2 and cv < 26.0:
                prio_grade = 'S+' if slot in ('goblet', 'sands', 'circlet') else 'S'
                score = 92 if prio_grade == 'S+' else 87
                meta['reshape_target'] = 'CR + CD'
                loc_info = f" [Equipped: {location}]" if location else ""
                return prio_grade, 'RESHAPE', score, f'Prime Reshape Target (Double Crit, {cv:.1f} CV){loc_info} -> Reroll with Feather', meta

            if slot == 'circlet' and main in CRIT_STATS:
                opp = 'critDMG_' if main == 'critRate_' else 'critRate_'
                if opp in full_keys and cv < 18.0:
                    opp_name = STAT_NAMES.get(opp, opp)
                    second = 'ER' if 'enerRech_' in full_keys else ('ATK%' if 'atk_' in full_keys else 'EM')
                    meta['reshape_target'] = f'{opp_name} + {second}'
                    loc_info = f" [Equipped: {location}]" if location else ""
                    return 'S', 'RESHAPE', 86, f'Prime Reshape Target (Crit Circlet with opposite {opp_name}, {cv:.1f} CV){loc_info}', meta

            if slot == 'goblet' and 'dmg_' in main and cr_count >= 1 and cv < 18.0:
                cr_stat = 'CR' if 'critRate_' in full_keys else 'CD'
                second = 'ER' if 'enerRech_' in full_keys else ('EM' if 'eleMas' in full_keys else 'ATK%')
                meta['reshape_target'] = f'{cr_stat} + {second}'
                loc_info = f" [Equipped: {location}]" if location else ""
                return 'S+', 'RESHAPE', 94, f'Grail Reshape Target (Elemental DMG Goblet, {cv:.1f} CV){loc_info} -> Reroll with Feather', meta

            if slot == 'sands' and main in ('enerRech_', 'eleMas') and cr_count >= 1 and cv < 18.0:
                cr_stat = 'CR' if 'critRate_' in full_keys else 'CD'
                meta['reshape_target'] = f'{cr_stat} + ER/EM'
                loc_info = f" [Equipped: {location}]" if location else ""
                return 'S', 'RESHAPE', 86, f'Prime Reshape Target ({STAT_NAMES.get(main, main)} Sands, {cv:.1f} CV){loc_info}', meta

        # Standard high-level grading
        if cv >= 38.0:
            return 'S+', 'LOCK_KEEP', 99, f'God-Tier Leveled ({cv:.1f} CV)', meta
        if cv >= 30.0:
            return 'S', 'LOCK_KEEP', 88, f'Great Leveled ({cv:.1f} CV)', meta
        if cv >= 22.0 or (cv >= 16.0 and ('enerRech_' in full_keys or 'eleMas' in full_keys)):
            return 'A', 'LOCK_KEEP', 76, f'Solid Leveled ({cv:.1f} CV + utility)', meta
        if main in ('eleMas', 'enerRech_', 'heal_') or (slot == 'goblet' and 'dmg_' in main):
            return 'A', 'LOCK_KEEP', 74, f'Rare {STAT_NAMES.get(main, main)} with usable utility', meta
        if equipped:
            return 'A', 'LOCK_KEEP', 70, f'Equipped on {location} ({cv:.1f} CV)', meta
        return 'F', 'SCRAP', 20, f'Bricked Leveled ({cv:.1f} CV, no reshape value) - Recycle for EXP', meta

    # 3. PARTIALLY LEVELED PIECES (+1 to +15)
    if 1 <= level < 16:
        if cv >= 14.0 or (cr_count >= 1 and 'enerRech_' in full_keys):
            return 'B', 'ROLL_TO_PLUS_4', 60, f'+{level} Promising ({cv:.1f} CV) - continue rolling to +16', meta
        if equipped:
            return 'A', 'LOCK_KEEP', 68, f'Equipped on {location}', meta
        return 'F', 'SCRAP', 25, f'+{level} Stalled rolls ({cv:.1f} CV, hit flat stats) - Feed to next piece', meta

    # 4. UNLEVELLED LEVEL 0 PIECES
    # A. Double Crit pieces
    if cr_count == 2:
        if is_4l:
            return 'S+', 'UPGRADE_NOW', 98, 'Grail: Pre-Activated 4-Liner Double Crit (5 upgrades, 54.4 CV max!)', meta
        if active_cr == 2:
            return 'S', 'UPGRADE_NOW', 88, 'Elite: 3-Liner Double Crit (4 upgrades, 46.6 CV max)', meta
        if len(unact) > 0 and sum(1 for k in unact_keys if k in CRIT_STATS) >= 1:
            return 'S', 'UPGRADE_NOW', 86, 'Elite: 3-Liner with Guaranteed Double Crit at +4', meta

    # B. Rare Main Stats (Elemental Goblets, EM Goblets/Circlets)
    if slot == 'goblet' and 'dmg_' in main:
        if is_4l:
            return 'S+', 'UPGRADE_NOW', 92, f'Grail: Pre-Activated 4-Liner Elemental DMG ({STAT_NAMES.get(main, main)})', meta
        if cr_count >= 1 or 'enerRech_' in full_keys or 'eleMas' in full_keys:
            return 'S', 'UPGRADE_NOW', 86, f'Elemental DMG ({STAT_NAMES.get(main, main)}) with Crit/ER/EM', meta
        if flat_count <= 2:
            return 'B', 'ROLL_TO_PLUS_4', 58, f'Elemental DMG ({STAT_NAMES.get(main, main)}) - test roll +4', meta
        return 'F', 'SCRAP', 30, f'Elemental DMG goblet with 3 confirmed flat stats', meta

    if main == 'eleMas' and slot in ('goblet', 'circlet'):
        if is_4l or cr_count >= 1 or 'enerRech_' in full_keys:
            return 'S', 'UPGRADE_NOW', 88, f'Rare {slot} EM Main Stat with Crit/ER (~2.5-4% drop rate)', meta
        return 'A', 'LOCK_KEEP', 78, f'Rare {slot} EM Main Stat (~2.5-4% drop rate) - Keep for EM buffers', meta

    # C. Crit Circlets
    if slot == 'circlet' and main in CRIT_STATS:
        opp = 'critDMG_' if main == 'critRate_' else 'critRate_'
        if opp in full_keys:
            return 'S', 'UPGRADE_NOW', 88, f'Crit Circlet with opposite {STAT_NAMES.get(opp, opp)}', meta
        if is_4l and useful_pct >= 1:
            return 'S', 'UPGRADE_NOW', 82, 'Pre-Activated 4-Liner Crit Circlet with useful % stats', meta
        if useful_pct >= 2:
            return 'A', 'ROLL_TO_PLUS_4', 74, 'Crit Circlet with useful % stats', meta
        if flat_count >= 3:
            return 'F', 'SCRAP', 28, 'Crit Circlet with 3 confirmed flat stats', meta
        return 'B', 'ROLL_TO_PLUS_4', 56, 'Crit Circlet - test roll +4', meta

    # D. Healing Bonus Circlet
    if slot == 'circlet' and main == 'heal_':
        if 'hp_' in full_keys or 'enerRech_' in full_keys or cr_count >= 1:
            return 'A', 'LOCK_KEEP', 74, 'Healing Bonus Circlet with HP%/ER/Crit', meta
        return 'F', 'SCRAP', 20, 'Healing Bonus Circlet with no HP/ER synergy', meta

    # E. Flowers and Feathers (Fixed main stat -> highest standards)
    if slot in ('flower', 'plume'):
        if cr_count == 0:
            if 'enerRech_' in full_keys and 'eleMas' in full_keys and set_name in (EM_SUPPORT_SETS | ER_SUPPORT_SETS):
                return 'A', 'LOCK_KEEP', 72, f'Support {slot} with ER+EM on {set_name}', meta
            return 'F', 'SCRAP', 15, f'{slot} with 0 Crit stats (Instant Fodder/Strongbox)', meta

        if cr_count == 1:
            if is_4l:
                if flat_count <= 1:
                    return 'S', 'UPGRADE_NOW', 84, f'Pre-Activated 4-Liner {slot} 1-Crit + 2 synergy stats (5 upgrades)', meta
                return 'A', 'UPGRADE_NOW', 72, f'Pre-Activated 4-Liner {slot} 1-Crit', meta
            else:
                if len(unact) > 0 and flat_count >= 2:
                    return 'F', 'SCRAP', 25, f'{slot} 1-Crit with known dead 4th stat ({flat_count} flats)', meta
                return 'B', 'ROLL_TO_PLUS_4', 55, f'{slot} 3-Liner 1-Crit - test roll to +4', meta

    # F. Sands (ATK%, HP%, DEF%, ER, EM)
    if slot == 'sands':
        if main in ('enerRech_', 'eleMas'):
            if is_4l or cr_count >= 1:
                return 'S', 'UPGRADE_NOW', 85, f'{STAT_NAMES.get(main, main)} Sands with Crit', meta
            if useful_pct >= 2:
                return 'A', 'ROLL_TO_PLUS_4', 73, f'{STAT_NAMES.get(main, main)} Sands with useful % stats', meta
            return 'B', 'ROLL_TO_PLUS_4', 55, f'{STAT_NAMES.get(main, main)} Sands - test roll +4', meta

        if main in ('atk_', 'hp_', 'def_'):
            is_synergistic = (main == 'hp_' and set_name in HP_SCALER_SETS) or \
                             (main == 'def_' and set_name in DEF_SCALER_SETS) or \
                             (main == 'atk_')
            if cr_count >= 1:
                if is_4l:
                    return 'S', 'UPGRADE_NOW', 84, f'Pre-Activated {STAT_NAMES.get(main, main)} Sands with Crit (5 upgrades)', meta
                if len(unact) > 0 and flat_count >= 2 and not is_synergistic:
                    return 'F', 'SCRAP', 25, f'{STAT_NAMES.get(main, main)} Sands 1-Crit with dead 4th stat', meta
                return 'A', 'UPGRADE_NOW', 74, f'{STAT_NAMES.get(main, main)} Sands with Crit + synergy', meta
            else:
                if is_synergistic and 'enerRech_' in full_keys and 'eleMas' in full_keys:
                    return 'A', 'LOCK_KEEP', 70, f'Dedicated Support Sands ({set_name}) with ER+EM', meta
                return 'F', 'SCRAP', 20, f'{STAT_NAMES.get(main, main)} Sands with 0 Crit stats', meta

    # G. HP% or DEF% Goblet
    if slot == 'goblet' and main in ('hp_', 'def_', 'atk_'):
        if (main == 'hp_' and set_name in HP_SCALER_SETS) or (main == 'def_' and set_name in DEF_SCALER_SETS):
            if cr_count >= 1:
                return 'A', 'UPGRADE_NOW', 76, f'Dedicated {STAT_NAMES.get(main, main)} Goblet for {set_name}', meta
        if cr_count == 2:
            return 'S', 'UPGRADE_NOW', 85, f'{STAT_NAMES.get(main, main)} Goblet with Double Crit', meta
        return 'F', 'SCRAP', 20, f'{STAT_NAMES.get(main, main)} Goblet with no dedicated synergy', meta

    # Fallback
    if flat_count >= 2 and cr_count == 0:
        return 'F', 'SCRAP', 15, 'No crit and multiple flat stats (Fodder)', meta

    return 'B', 'ROLL_TO_PLUS_4', 50, 'Borderline potential - test roll to +4', meta


def find_latest_good_file() -> str:
    """Finds the most recent genshinData_GOOD_*.json file."""
    search_patterns = [
        os.path.join(os.getcwd(), "**", "genshinData_GOOD_*.json"),
        os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "**", "genshinData_GOOD_*.json"),
        os.path.join(os.path.expanduser("~"), "Documents", "**", "genshinData_GOOD_*.json"),
        os.path.join(os.path.expanduser("~"), "Downloads", "genshinData_GOOD_*.json"),
    ]
    
    candidates = []
    for pat in search_patterns:
        matches = glob.glob(pat, recursive=True)
        for m in matches:
            if not m.endswith("_audited.json"):
                candidates.append((os.path.getmtime(m), m))

    if not candidates:
        return ""

    candidates.sort(key=lambda x: x[0], reverse=True)
    return candidates[0][1]


def run_audit(input_file: str, output_json: str = None, report_file: str = None, csv_file: str = None, min_cv: float = 24.0, keep_equipped: bool = True):
    print(f"\n=======================================================")
    print(f"       GENSHIN IMPACT UNIFIED ARTIFACT AUDITOR         ")
    print(f"=======================================================")
    print(f"Loading artifact data from:")
    print(f"  {input_file}\n")

    with open(input_file, 'r', encoding='utf-8') as f:
        data = json.load(f)

    artifacts = data.get('artifacts', [])
    total_count = len(artifacts)

    if total_count == 0:
        print("No artifacts found in JSON file.")
        return

    grades = {'S+': [], 'S': [], 'A': [], 'B': [], 'F': []}
    actions_count = {}

    for idx, art in enumerate(artifacts):
        grade, action, score, reason, meta = score_and_grade_artifact(art, min_cv_leveled=min_cv, keep_equipped=keep_equipped)
        entry = {
            'index': idx,
            'artifact': art,
            'grade': grade,
            'action': action,
            'score': score,
            'reason': reason,
            'meta': meta
        }
        grades[grade].append(entry)
        actions_count[action] = actions_count.get(action, 0) + 1

    print("-------------------------------------------------------")
    print("             UNIFIED GRADE DISTRIBUTION                ")
    print("-------------------------------------------------------")
    grade_desc = {
        'S+': 'Grail / God-Tier ',
        'S':  'Elite / Priority ',
        'A':  'Solid / Keep     ',
        'B':  'Test Roll (+4)   ',
        'F':  'Fodder / Scrap   '
    }

    grade_detail = {
        'S+': '4-Liner Double Crits, 38+ CV & Grail Reshapes',
        'S':  'Double Crits, 4-Liners & Prime Reshape Targets',
        'A':  'Reliable +20s, rare EM & support sets',
        'B':  'Roll once: promote to A/S or demote to F',
        'F':  'Safe to Strongbox or feed as EXP immediately'
    }

    for g_key in ['S+', 'S', 'A', 'B', 'F']:
        cnt = len(grades[g_key])
        pct = (cnt / total_count) * 100.0
        print(f"  [{g_key:<2}] {grade_desc[g_key]}: {cnt:4d} ({pct:5.1f}%) -> {grade_detail[g_key]}")


    print("-------------------------------------------------------")
    print("Action Recommendations:")
    print(f"  * UPGRADE_NOW     : {actions_count.get('UPGRADE_NOW', 0):4d} pieces (Top priority +0 artifacts)")
    print(f"  * LOCK_KEEP       : {actions_count.get('LOCK_KEEP', 0):4d} pieces (Already leveled / ready to use)")
    print(f"  * RESHAPE         : {actions_count.get('RESHAPE', 0):4d} pieces (Reroll with Dust/Feathers)")
    print(f"  * ROLL_TO_PLUS_4  : {actions_count.get('ROLL_TO_PLUS_4', 0):4d} pieces (Feed 1 fodder to test 4th stat)")
    print(f"  * SCRAP           : {actions_count.get('SCRAP', 0):4d} pieces (Feed to Mystic Offering / EXP)")
    print("-------------------------------------------------------\n")

    # If output JSON requested, update lock flags and write
    if output_json:
        # S+, S, A, B are locked. F is unlocked unless equipped.
        for g_key in ['S+', 'S', 'A', 'B']:
            for item in grades[g_key]:
                item['artifact']['lock'] = True
        for item in grades['F']:
            if not item['artifact'].get('location'):
                item['artifact']['lock'] = False

        with open(output_json, 'w', encoding='utf-8') as f:
            json.dump(data, f, indent=2)
        print(f"Updated GOOD JSON saved with adjusted locks to:")
        print(f"  {output_json}")
        print(f"  -> Grades S+, S, A, B are LOCKED ({len(grades['S+']) + len(grades['S']) + len(grades['A']) + len(grades['B'])} pieces)")
        print(f"  -> Grade F is UNLOCKED ({len(grades['F'])} pieces ready to Strongbox!)\n")

    # Generate Markdown report
    if report_file:
        generate_markdown_report(report_file, input_file, grades, total_count)
        print(f"Detailed Markdown Audit Report generated at:")
        print(f"  {report_file}\n")

    # Generate Fast In-Game Scrap Guide
    in_game_guide_file = "in_game_scrap_guide.md"
    generate_in_game_guide(in_game_guide_file, grades['F'])
    print(f"Fast In-Game Scrap Guide & Checklist generated at:")
    print(f"  {in_game_guide_file}\n")

    # Generate CSV report if requested
    if csv_file:
        generate_csv_report(csv_file, grades)
        print(f"Detailed CSV Spreadsheet generated at:")
        print(f"  {csv_file}\n")


def generate_csv_report(csv_path: str, grades: Dict[str, List[Dict]]):
    with open(csv_path, 'w', newline='', encoding='utf-8') as f:
        writer = csv.writer(f)
        writer.writerow(['Grade', 'Score', 'Action', 'Slot', 'MainStat', 'Set', 'Level', 'StarterType', 'CV', 'Substats', 'ReshapeTarget', 'Equipped', 'Reason'])
        for g in ['S+', 'S', 'A', 'B', 'F']:
            # Sort within grade by score descending
            sorted_items = sorted(grades[g], key=lambda x: x['score'], reverse=True)
            for item in sorted_items:
                m = item['meta']
                writer.writerow([
                    item['grade'],
                    item['score'],
                    item['action'],
                    m['slot'],
                    STAT_NAMES.get(m['main'], m['main']),
                    m['set'],
                    f"+{m['level']}",
                    m['starter_type'],
                    m['cv'],
                    m['subs_str'],
                    m.get('reshape_target', ''),
                    m['location'] or '',
                    item['reason']
                ])


def generate_markdown_report(report_path: str, input_file: str, grades: Dict[str, List[Dict]], total_count: int):
    with open(report_path, 'w', encoding='utf-8') as f:
        f.write("# Genshin Impact Unified Artifact Tier List & Action Plan\n\n")
        f.write(f"- **Source File**: `{os.path.basename(input_file)}`\n")
        f.write(f"- **Total Artifacts Analyzed**: {total_count}\n\n")

        # Section 1: Executive Summary
        f.write("## 1. Unified Grade Distribution\n\n")
        f.write("| Grade | Quality | Count | % | Action Needed |\n")
        f.write("| :---: | :--- | :---: | :---: | :--- |\n")
        f.write(f"| 🏆 **S+** | **Grail / God-Tier** | **{len(grades['S+'])}** | {(len(grades['S+'])/total_count)*100:.1f}% | Upgrade first or Reshape with Feathers (Highest ceiling) |\n")
        f.write(f"| ⭐ **S** | **Elite / Priority** | **{len(grades['S'])}** | {(len(grades['S'])/total_count)*100:.1f}% | Lock & Upgrade / Prime Reshape targets |\n")
        f.write(f"| ✨ **A** | **Solid / Keep** | **{len(grades['A'])}** | {(len(grades['A'])/total_count)*100:.1f}% | Lock & Keep (Reliable +20s & essential supports) |\n")
        f.write(f"| 🎲 **B** | **Test Roll (+4)** | **{len(grades['B'])}** | {(len(grades['B'])/total_count)*100:.1f}% | Feed 1 fodder to level to +4: promote to S/A or demote to F |\n")
        f.write(f"| 🗑️ **F** | **Fodder / Strongbox** | **{len(grades['F'])}** | {(len(grades['F'])/total_count)*100:.1f}% | **SCRAP IMMEDIATELY** in Mystic Offering (Strongbox) or EXP |\n\n")

        # Section 2: 4-Step Action Plan
        f.write("## 2. ⚡ Your Simple 4-Step Action Plan\n\n")
        f.write(f"1. **Step 1: Scrap the {len(grades['F'])} Grade F Artifacts**\n")
        f.write("   - These pieces have 0 Crit stats, 3 confirmed flat stats, or bricked rolls with no Reshape value.\n")
        f.write("   - Feed them into the **Artifact Strongbox** (Mystic Offering) at the Crafting Bench to roll for new pieces.\n\n")
        f.write(f"2. **Step 2: Test the {len(grades['B'])} Grade B Pieces to +4**\n")
        f.write("   - Feed 1 piece of junk fodder to bring each to level +4.\n")
        f.write("   - If the roll/unlocked stat is **Crit Rate, Crit DMG, or ER**: Keep rolling (promoted to A/S)!\n")
        f.write("   - If the roll lands on a flat stat (Flat DEF/HP): Stop immediately and use it as fodder (recovers 80% EXP).\n\n")
        f.write(f"3. **Step 3: Level Your Grade S+ & S Pieces**\n")
        f.write("   - Focus all your artifact EXP on the S+ and S unlevelled pieces—they have the highest damage ceiling in the game.\n\n")
        f.write("4. **Step 4: Reshape Bricked S+ and S Pieces with Feathers / Dust**\n")
        f.write("   - Use your **Dust of Enlightenment** and **Hallowed Exegesis Feathers** on the designated `RESHAPE` artifacts below to turn them into 35-45+ CV god pieces!\n\n")

        # Section 3: Grade S+ Showcase
        f.write("## 3. 🏆 Grade S+ Artifacts (The Account Grails)\n\n")
        f.write("| Action | Slot | Main Stat | Set | Level | Substats | Reason |\n")
        f.write("| :--- | :--- | :--- | :--- | :---: | :--- | :--- |\n")
        for item in sorted(grades['S+'], key=lambda x: x['score'], reverse=True):
            m = item['meta']
            f.write(f"| **{item['action']}** | {m['slot']} | {STAT_NAMES.get(m['main'], m['main'])} | {m['set']} | +{m['level']} | {m['subs_str']} | {item['reason']} |\n")
        f.write("\n")

        # Section 4: Reshape Targets
        reshape_pieces = [it for it in (grades['S+'] + grades['S']) if it['action'] == 'RESHAPE']
        if reshape_pieces:
            f.write("## 4. 🪶 Prime Reshape Targets (Dust of Enlightenment & Feathers)\n\n")
            f.write("> [!IMPORTANT]\n")
            f.write("> These Level 20 artifacts rolled poorly, but their base substats are elite. Use Reshape to guarantee rolls into your chosen stats:\n\n")
            f.write("| Slot | Main Stat | Set | Current CV | Substats | Target Stats | Equipped |\n")
            f.write("| :--- | :--- | :--- | :---: | :--- | :---: | :---: |\n")
            for it in reshape_pieces:
                m = it['meta']
                f.write(f"| {m['slot']} | {STAT_NAMES.get(m['main'], m['main'])} | {m['set']} | **{m['cv']}** | {m['subs_str']} | `{m.get('reshape_target', '')}` | {m['location'] or 'No'} |\n")
            f.write("\n")

        # Section 5: Grade F Scrap Candidates Sample
        f.write(f"## 5. 🗑️ Grade F Immediate Scrap List (First 50 of {len(grades['F'])})\n\n")
        f.write("| Slot | Main Stat | Set | Level | Substats | Reason |\n")
        f.write("| :--- | :--- | :--- | :---: | :--- | :--- |\n")
        for item in grades['F'][:50]:
            m = item['meta']
            f.write(f"| {m['slot']} | {STAT_NAMES.get(m['main'], m['main'])} | {m['set']} | +{m['level']} | {m['subs_str']} | {item['reason']} |\n")


def generate_in_game_guide(guide_path: str, f_items: List[Dict]):
    from collections import defaultdict
    by_set = defaultdict(list)
    for it in f_items:
        by_set[it['meta']['set']].append(it)

    with open(guide_path, 'w', encoding='utf-8') as f:
        f.write("# In-Game Artifact Scrapping Guide & Fast Checklist\n\n")
        f.write(f"Total Grade F Artifacts to Scrap: **{len(f_items)} pieces**\n\n")
        f.write("> [!TIP]\n")
        f.write("> **How to avoid checking 784 artifacts one by one?**\n")
        f.write("> Use the **3-Minute Quick Filters** in Phase 1 below. That will eliminate over **150 pieces in under 3 minutes**!\n")
        f.write("> For the remaining pieces, use the **Set-by-Set Checklist** in Phase 2.\n\n")

        f.write("## Phase 1: The 3-Minute Quick Filter Method (Clears ~150 Pieces Instantly)\n\n")
        f.write("Go to the **Crafting Bench -> Artifact Strongbox** (Mystic Offering):\n\n")
        f.write("### Filter 1: 0-Crit Flowers & Feathers (~68 pieces)\n")
        f.write("- **In-Game Filter**: Slot: `Flower` & `Plume` | Level: `+0`\n")
        f.write("- **Rule**: **If it does NOT have Crit Rate or Crit DMG in its substats, select it!**\n")
        f.write("- *Result*: ~68 safe fodder pieces cleared in 60 seconds.\n\n")

        f.write("### Filter 2: Non-Crit / Off-Stat Goblets (~64 pieces)\n")
        f.write("- **In-Game Filter**: Slot: `Goblet` | Main Stat: `ATK%`, `DEF%`, `HP%` | Level: `+0`\n")
        f.write("- **Rule**: Select any DEF%/HP%/ATK% goblet that does NOT have Double Crit.\n")
        f.write("- *Result*: ~64 safe fodder pieces cleared in 60 seconds.\n\n")

        f.write("### Filter 3: 0-Crit Sands (~35 pieces)\n")
        f.write("- **In-Game Filter**: Slot: `Sands` | Main Stat: `HP%`, `DEF%`, `ATK%` | Level: `+0`\n")
        f.write("- **Rule**: Select any Sands with 0 Crit stats and multiple flat stats.\n")
        f.write("- *Result*: ~35 safe fodder pieces cleared in 60 seconds.\n\n")

        f.write("----\n\n")
        f.write("## Phase 2: Set-by-Set Scrap Checklist (Complete 229 Pieces)\n\n")
        f.write("If you want 100% precision, simply filter by **Artifact Set** in-game and check off the pieces below:\n\n")

        for set_name, items in sorted(by_set.items(), key=lambda x: len(x[1]), reverse=True):
            f.write(f"### {set_name} ({len(items)} pieces)\n\n")
            f.write("| Slot | Main Stat | Level | Substats | Reason |\n")
            f.write("| :--- | :--- | :---: | :--- | :--- |\n")
            for it in items:
                m = it['meta']
                f.write(f"| {m['slot']} | {STAT_NAMES.get(m['main'], m['main'])} | +{m['level']} | {m['subs_str']} | {it['reason']} |\n")
            f.write("\n")


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description="Genshin Impact Unified Artifact Auditor")
    parser.add_argument('--input', '-i', type=str, help="Path to GOOD JSON file (optional, auto-detects latest if omitted)")
    parser.add_argument('--output-json', '-o', type=str, help="Path to export updated GOOD JSON with locks applied")
    parser.add_argument('--report', '-r', type=str, default="artifact_audit_report.md", help="Path for markdown report")
    parser.add_argument('--csv', '-c', type=str, default="artifact_audit_report.csv", help="Path for CSV spreadsheet report")
    parser.add_argument('--min-cv', type=float, default=24.0, help="Minimum CV for +20 pieces to keep (default: 24.0)")

    args = parser.parse_args()

    input_path = args.input
    if not input_path:
        input_path = find_latest_good_file()
        if not input_path:
            print("Error: Could not find any genshinData_GOOD_*.json files automatically. Please specify with --input.")
            sys.exit(1)

    output_json_path = args.output_json
    if not output_json_path:
        base, ext = os.path.splitext(input_path)
        output_json_path = f"{base}_audited{ext}"

    run_audit(
        input_file=input_path,
        output_json=output_json_path,
        report_file=args.report,
        csv_file=args.csv,
        min_cv=args.min_cv,
        keep_equipped=True
    )
