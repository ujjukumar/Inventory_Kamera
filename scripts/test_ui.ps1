using namespace System.IO;
using namespace System.Text.RegularExpressions;

$file = "D:\Coding\repos\InventoryKamera\InventoryKamera.WinForms\UI\main\MainForm.Designer.cs"
$content = Get-Content $file -Raw

# Replace font to Segoe UI
$content = $content -replace '"Microsoft Sans Serif"', '"Segoe UI"'

Set-Content -Path $file -Value $content
