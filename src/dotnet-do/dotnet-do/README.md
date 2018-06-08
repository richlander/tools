# A simple approach to .NET Core Local Tools

Local tools are intended to have a variety of characteritics:

* Local (non-global) installation.
* Easy to clean up.
* Can install multiple tools at once, codified in a tools list file.
* Can specify specific versions of tools.
* Multiple tools lists can be consulted, in a hierarchical fashion.

## Challenges with this implementation

There are some challenges with this approach:

* Not integrated into the .NET CLI. That's a temporary challenge, given that this is a prototype.
* Double process hop. `dotnet do` launches `dotnetsay.exe` instead of directly hosting `dotnetsay.dll`.
* It is very incovienient to locate files within a `.store` installation due to being so heavily oriented on NuGet. This looks like a design point that needs to change. I'm sure others will want to find and/or launch files from within the structure, too.
* `dotnet tool install --tool-path` output is oriented around global tool installation. See [dotnet/cli #9400](https://github.com/dotnet/cli/issues/9400).

## Script for using dotnet-do

```console
C:\git\tools\src\dotnet-do\dotnet-do>dotnet pack -c release -o nupkg
Microsoft (R) Build Engine version 15.7.179.6572 for .NET Core
Copyright (C) Microsoft Corporation. All rights reserved.

  Restoring packages for C:\git\tools\src\dotnet-do\dotnet-do\dotnet-do.csproj...
  Restore completed in 157.15 ms for C:\git\tools\src\dotnet-do\dotnet-do\dotnet-do.csproj.
  dotnet-do -> C:\git\tools\src\dotnet-do\dotnet-do\bin\release\netcoreapp2.1\dotnet-do.dll
  dotnet-do -> C:\git\tools\src\dotnet-do\dotnet-do\bin\release\netcoreapp2.1\dotnet-do.dll
  dotnet-do -> C:\git\tools\src\dotnet-do\dotnet-do\bin\release\netcoreapp2.1\publish\
  Successfully created package 'C:\git\tools\src\dotnet-do\dotnet-do\nupkg\dotnet-do.1.0.0.nupkg'.

C:\git\tools\src\dotnet-do\dotnet-do>dotnet tool install -g --add-source nupkg dotnet-do
You can invoke the tool using the following command: dotnet-do
Tool 'dotnet-do' (version '1.0.0') was successfully installed.

C:\git\tools\src\dotnet-do\dotnet-do>dotnet do
This tool exposes two options:
install -- installs tools referenced in dotnet.tool files.
clean -- deletes installed tools in .dotnettools directories
print -- prints set of dotnet.tools files.
[toolname] -- runs a tool (using short name w/no extension).

C:\git\tools\src\dotnet-do\dotnet-do>dotnet do print
C:\git\tools\src\dotnet-do\dotnet-do\dotnet.tools
C:\git\tools\src\dotnet.tools

C:\git\tools\src\dotnet-do\dotnet-do>dotnet do install
Installing tools: 3
Installing to: C:\git\tools\src\dotnet-do\dotnet-do
You can invoke the tool using the following command: dotnetsay
Tool 'dotnetsay' (version '2.0.0') was successfully installed.
You can invoke the tool using the following command: dx
Tool 'dx' (version '0.4.0') was successfully installed.
You can invoke the tool using the following command: dotnet-serve
Tool 'dotnet-serve' (version '1.0.0') was successfully installed.

C:\git\tools\src\dotnet-do\dotnet-do>dotnet do dotnetsay Welcome to using a .NET Core local tool!

        Welcome to using a .NET Core local tool!
    __________________
                      \
                       \
                          ....
                          ....'
                           ....
                        ..........
                    .............'..'..
                 ................'..'.....
               .......'..........'..'..'....
              ........'..........'..'..'.....
             .'....'..'..........'..'.......'.
             .'..................'...   ......
             .  ......'.........         .....
             .                           ......
            ..    .            ..        ......
           ....       .                 .......
           ......  .......          ............
            ................  ......................
            ........................'................
           ......................'..'......    .......
        .........................'..'.....       .......
     ........    ..'.............'..'....      ..........
   ..'..'...      ...............'.......      ..........
  ...'......     ...... ..........  ......         .......
 ...........   .......              ........        ......
.......        '...'.'.              '.'.'.'         ....
.......       .....'..               ..'.....
   ..       ..........               ..'........
          ............               ..............
         .............               '..............
        ...........'..              .'.'............
       ...............              .'.'.............
      .............'..               ..'..'...........
      ...............                 .'..............
       .........                        ..............
        .....



C:\git\tools\src\dotnet-do\dotnet-do>dotnet do print
C:\git\tools\src\dotnet-do\dotnet-do\dotnet.tools
C:\git\tools\src\dotnet.tools

C:\git\tools\src\dotnet-do\dotnet-do>dotnet do clean
Deleting: C:\git\tools\src\dotnet-do\dotnet-do\.dotnettools
```
