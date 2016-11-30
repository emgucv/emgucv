REM @echo off
pushd %~p0

IF "%1%"=="64" ECHO "Running 64bit tests" 
IF NOT "%1%"=="64" ECHO "Running 32 bit tests"

SET PLATFORM=x86
IF "%1%"=="64" SET PLATFORM=x64

REM Find Visual Studio or Msbuild
SET VS2012="%VS110COMNTOOLS%..\IDE\devenv.com"
SET VS2013="%VS120COMNTOOLS%..\IDE\devenv.com"
SET VS2015="%VS140COMNTOOLS%..\IDE\devenv.com"
SET MSBUILD35="%windir%\Microsoft.NET\Framework\v3.5\MSBuild.exe"
SET MSBUILD40="%windir%\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe"

IF EXIST %MSBUILD35% SET DEVENV=%MSBUILD35%
IF EXIST %MSBUILD40% SET DEVENV=%MSBUILD40%
IF EXIST %VS2012% SET DEVENV=%VS2012%
IF EXIST %VS2013% SET DEVENV=%VS2013%
IF EXIST %VS2015% SET DEVENV=%VS2015%


SET TEST2012="%VS110COMNTOOLS%..\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"
SET TEST2013="%VS120COMNTOOLS%..\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"
SET TEST2015="%VS140COMNTOOLS%..\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"

IF EXIST %TEST2012% SET MSTEST=%TEST2012%
IF EXIST %TEST2013% SET MSTEST=%TEST2013%
IF EXIST %TEST2015% SET MSTEST=%TEST2015%

:SET_BUILD_TYPE
IF %DEVENV%==%MSBUILD35% SET BUILD_TYPE=/property:Configuration=Debug
IF %DEVENV%==%MSBUILD40% SET BUILD_TYPE=/property:Configuration=Debug
IF %DEVENV%==%VS2012% SET BUILD_TYPE=/Build Debug
IF %DEVENV%==%VS2013% SET BUILD_TYPE=/Build Debug
IF %DEVENV%==%VS2015% SET BUILD_TYPE=/Build Debug

cd ..\..

IF "%2"=="" GOTO TEST_INPLACE

:TEST_PACKAGE
rm -rf tmp
mkdir tmp
unzip "%2" -d tmp
call %DEVENV% %BUILD_TYPE% tmp\Solution\Windows.Desktop\Emgu.CV.Example.sln

call %DEVENV% %BUILD_TYPE% Solution\Windows.Desktop\Emgu.CV.Test.sln
cp bin/Emgu.CV.Test.dll tmp/bin
call %MSTEST% tmp\bin\Emgu.CV.Test.dll /Platform:%PLATFORM%
GOTO END

:TEST_INPLACE
call %DEVENV% %BUILD_TYPE% Solution\Windows.Desktop\Emgu.CV.Example.sln
call %DEVENV% %BUILD_TYPE% Solution\Windows.Desktop\Emgu.CV.Test.sln
call %MSTEST% bin\Emgu.CV.Test.dll /Platform:%PLATFORM%

:END
popd