REM == FORCE OVERWRITE ==
SET COPYCMD=/Y

"C:\Program Files\CMake 2.6\bin\cmake.exe" -DVARIABLE:OPENCV_ENABLE_OPENMP=ON .
IF EXIST "C:\Program Files\Microsoft Visual Studio 8\Common7\IDE\devenv.exe" (SET DEVENV="C:\Program Files\Microsoft Visual Studio 8\Common7\IDE\devenv.exe") ELSE (IF EXIST "C:\Program Files\Microsoft Visual Studio 9.0\Common7\IDE\devenv.exe" (SET DEVENV="C:\Program Files\Microsoft Visual Studio 9.0\Common7\IDE\devenv.exe"))
%DEVENV% /Build Release emgucv.sln
xcopy bin\release\*.dll bin\