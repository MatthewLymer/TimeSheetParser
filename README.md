# TimeSheetParser
Parses custom made timesheet files

## Example timesheet.txt file format

```
JUNE 1
0900 STAND UP
0915 EE-123 - Fix widget
1210 LUNCH
1300 EE-456 - Implement feature foobar
1732 HOME

JUNE 2
0900 STAND UP
0920 GROOMING MEETINGS
0955 FILLING OUT TIME SHEET
1118 BACKLOG REFINEMENT
1209 LUNCH
1320 EE-456 - Implement feature foobar
1410 TOWNHALL
1506 EE-456 - Implement feature foobar
1734 HOME
```

## Usage

`cat timesheet.txt | TimeSheetParser.exe`

## Example output

```
JUNE 1
0.25 STAND UP
2.92 EE-123 - Fix widget
0.83 LUNCH
4.53 EE-456 - Implement feature foobar
Total Time: 8h 32m

JUNE 2
0.33 STAND UP
0.58 GROOMING MEETINGS
1.38 FILLING OUT TIME SHEET
0.85 BACKLOG REFINEMENT
1.18 LUNCH
3.30 EE-456 - Implement feature foobar
0.93 TOWNHALL
Total Time: 8h 34m
```

## Building

Windows

```sh
docker run -it --rm -v $(pwd):/code mcr.microsoft.com/dotnet/sdk:3.1 dotnet publish /code/src/TimeSheetParser/TimeSheetParser.csproj --configuration=Release --self-contained true /p:PublishReadyToRun=false --runtime win-x64 /p:PublishDir=/code/artifacts/win-x64
```

MacOS

```sh
docker run -it --rm -v $(pwd):/code mcr.microsoft.com/dotnet/sdk:3.1 dotnet publish /code/src/TimeSheetParser/TimeSheetParser.csproj --configuration=Release --self-contained true /p:PublishReadyToRun=false --runtime osx-x64 /p:PublishDir=/code/artifacts/osx-x64
```
