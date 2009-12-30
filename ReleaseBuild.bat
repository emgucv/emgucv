@echo off

REM =============================================
REM FIND VISUAL STUDIO
REM =============================================
set devenv=""
set devenv_version=""

echo searching for VS2010
start /w regedit /e %Temp%.\vs.reg "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\VisualStudio\10.0"
FOR /F "tokens=1* delims==" %%A IN ('TYPE %Temp%.\vs.reg ^| FIND "InstallDir"') DO (
 SET devenv=%%B
 SET devenv_version="VS2010"
 GOTO devenv_found
)
echo not found

echo searching for VS2008
start /w regedit /e %Temp%.\vs.reg "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\VisualStudio\9.0"
FOR /F "tokens=1* delims==" %%A IN ('TYPE %Temp%.\vs.reg ^| FIND "InstallDir"') DO (
 SET devenv=%%B
 SET devenv_version="VS2008"
 GOTO devenv_found
)
echo not found

echo searching for VS2005
start /w regedit /e %Temp%.\vs.reg "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\VisualStudio\8.0"
FOR /F "tokens=1* delims==" %%A IN ('TYPE %Temp%.\vs.reg ^| FIND "InstallDir"') DO (
 SET devenv=%%B
 SET devenv_version="VS2005"
 GOTO devenv_found
)
echo not found

:devenv_not_found
GOTO:EOF

:devenv_found

REM =============================================
REM FIND CMAKE
REM =============================================

set cmake=""
echo searching for CMake
start /w regedit /e %Temp%.\cmake.reg "HKEY_LOCAL_MACHINE\SOFTWARE\Kitware"
FOR /F "tokens=1* delims==" %%A IN ('TYPE %Temp%.\cmake.reg ^| FIND "@"') DO (
 SET cmake=%%B
 GOTO cmake_found
)

:cmake_not_found
echo not found
GOTO:EOFread

:cmake_found

REM =============================================
REM Make build script
REM =============================================

set cmake="%cmake:~1,-1%\\bin\\cmake.exe"

set CMAKE_CONF=""
REM IF %devenv_version%=="VS2005" SET CMAKE_CONF=-G "Visual Studio 8 2005"
REM IF %devenv_version%=="VS2008" SET CMAKE_CONF=-G "Visual Studio 9 2008"
REM IF %devenv_version%=="VS2010" SET CMAKE_CONF=-G "Visual Studio 10"

set CMAKE_COMMAND=%cmake% %CMAKE_CONF% -DBUILD_TESTS:BOOL=FALSE -DBUILD_NEW_PYTHON_SUPPORT=FALSE -DEMGU_CV_DOCUMENTATION_BUILD:BOOL=TRUE .  

SET devenv="%devenv:~1,-1%devenv.exe"
set BUILD_TYPE=/Build Release /Project Package
set BUILD_COMMAND=%devenv% %BUILD_TYPE% emgucv.sln

@echo on
%CMAKE_COMMAND%
%BUILD_COMMAND%

