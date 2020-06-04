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
