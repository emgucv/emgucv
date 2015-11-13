REM @echo off
pushd %~p0

IF "%1%"=="64" ECHO "Running 64bit tests" 
IF NOT "%1%"=="64" ECHO "Running 32 bit tests"

IF "%1%"=="64" SET PLATFORM=x64
IF NOT "%1%"=="64" SET PLATFORM=x86

REM Find MSbuild

SET MSBUILD40="%windir%\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe"

SET TEST2013="%VS120COMNTOOLS%..\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"
SET TEST2015="%VS140COMNTOOLS%..\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"


IF EXIST %TEST2013% SET MSTEST=%TEST2013%
IF EXIST %TEST2015% SET MSTEST=%TEST2015%


:SET_BUILD_TYPE
SET BUILD_TYPE=/property:Configuration=Release

cd ..\..

call %MSBUILD40% %BUILD_TYPE% Emgu.CV.Test\Emgu.CV.Test.Windows.Store\Emgu.CV.Test.Windows.Store.csproj  /p:Platform="%PLATFORM%"
call %MSTEST% Emgu.CV.Test\Emgu.CV.Test.Windows.Store\AppPackages\Emgu.CV.Test.Windows.Store_1.1.0.1_%PLATFORM%_Test\Emgu.CV.Test.Windows.Store_1.1.0.1_%PLATFORM%.appx /InIsolation
:END
popd