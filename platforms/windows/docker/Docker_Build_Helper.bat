cd c:\
xcopy c:\src c:\TEMP /s /e /h 
cd c:\TEMP\platforms\windows 
call powershell.exe -NoProfile -ExecutionPolicy Bypass -File Build_Binary.ps1 -Arch x86 %*
IF NOT EXIST c:\src\libs mkdir c:\src\libs
xcopy c:\TEMP\libs c:\src\libs /s /e /h
xcopy c:\TEMP\build_* c:\src /s /e /h
