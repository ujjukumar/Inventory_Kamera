# Inventory-Kamera

A desktop application designed to automatically scan and export data from Genshin Impact's inventory.

## Project Status & Architecture
The application is currently undergoing a modernization phase to migrate the UI from WinForms to WinUI 3 while reorganizing the core scanning logic into a separate class library for reusability.

*   **`InventoryKamera.Services`**: Contains all the modernized core logic for reading the screen, identifying items, parsing text, and exporting the final data. This project leverages `.NET 10` and `Microsoft.Extensions.Hosting` with Dependency Injection (DI) and `ILogger`.
*   **`InventoryKamera.WinUI`**: The new modern frontend utilizing WinUI 3 and the Windows App SDK. It incorporates a fresh redesign using Mica backdrops and a modern command center interface.
*   **`InventoryKamera.WinForms`**: The legacy frontend application. It remains in the repository as a fallback and reference.

## Requirements
To build and run this application, you will need:
*   [Visual Studio 2022 (v17.10+)](https://visualstudio.microsoft.com/)
*   [.NET 10.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
*   [Windows App SDK workload](https://learn.microsoft.com/en-us/windows/apps/windows-app-sdk/set-up-your-development-environment) in Visual Studio.

## Building the Project
1. Clone the repository.
2. Open `InventoryKamera.slnx` in Visual Studio 2022.
3. Set **`InventoryKamera.WinUI`** as your startup project.
4. Build the solution. The core `Services` library will be built alongside the frontend applications.

## Contribution
(WIP)

## License
(WIP)
