# FancyShare

A tool to share parts of a screen in online meeting apps.

FancyShare enables sharing of a section of a screen by creating a transparent overlay window on top of all other applications. This window can then be selected as the application to share by apps supporting single window sharing.
## Getting Started

### Requirements
[.NET Core 3.1 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet-core/thank-you/runtime-desktop-3.1.4-windows-x64-installer)

### Download

See [release page](https://github.com/patrickmichelson/FancyShare/releases).

### Build

To build the project from source, the [.NET SDK 3.1 or higher](https://dotnet.microsoft.com/download) is needed.

Checkout the repository, navigate to the root folder, and run the command:

```
dotnet build
```


## Known Issues

When using [Microsoft PowerToys FancyZone](https://aka.ms/PowerToysOverview_FancyZones): 

Dragging the FancyShare frame window to a zone with the mouse does not work, if the PowerToys option "Make dragged window transparent" is enabled.
Please disable this option or use keyboard shortcuts to move the window to a zone.


## License

This repository is licensed with the [MIT](LICENSE) license.