REM == FORCE OVERWRITE ==
REM SET COPYCMD=/Y

SET CMAKE="C:\Program Files\CMake 2.6\bin\cmake.exe"
SET VS2005="C:\Program Files\Microsoft Visual Studio 8\Common7\IDE\devenv.exe"
SET VS2008="C:\Program Files\Microsoft Visual Studio 9.0\Common7\IDE\devenv.exe"
SET MSBUILD35="C:\WINDOWS\Microsoft.NET\Framework\v3.5\MSBuild.exe"

IF EXIST %MSBUILD35% SET DEVENV=%MSBUILD35%
IF EXIST %VS2005% SET DEVENV=%VS2005% 
IF EXIST %VS2008% SET DEVENV=%VS2008%

IF %DEVENV%==%MSBUILD35% SET BUILD_TYPE=/property:Configuration=Release
IF %DEVENV%==%VS2005% SET BUILD_TYPE=/Build Release
IF %DEVENV%==%VS2008% SET BUILD_TYPE=/Build Release

IF %DEVENV%==%MSBUILD35% SET CMAKE_CONF="Visual Studio 8 2005"
IF %DEVENV%==%VS2005% SET CMAKE_CONF="Visual Studio 8 2005"
IF %DEVENV%==%VS2008% SET CMAKE_CONF="Visual Studio 9 2008"

REM %CMAKE% -G %CMAKE_CONF% -DOPENCV_ENABLE_OPENMP:BOOL=FALSE -DOPENCV_WHOLE_PROGRAM_OPTIMIZATION:BOOL=TRUE -DBUILD_TESTS:BOOL=FALSE . 
%CMAKE% -G %CMAKE_CONF% -DOPENCV_WHOLE_PROGRAM_OPTIMIZATION:BOOL=TRUE -DBUILD_TESTS:BOOL=FALSE . 
%DEVENV% %BUILD_TYPE% emgucv.sln

FOR %%F IN (cxcore111.dll cv111.dll cvaux111.dll cvextern.dll highgui111.dll ml111.dll opencv_ffmpeg111.dll) DO xcopy /D /Y bin\%%F Emgu.CV.Example\WebDynamicImage\Bin 
 
