set RELEASE-NAME=Emgu.CV.Binary-2.0.0.0
set RELEASE-FOLDER=release_zip
set RELEASE-ZIP=%RELEASE-NAME%.zip
set RELEASE-SOLUTION-NAME=Emgu.CV.DebuggerVisualizers.sln

call BinaryBuild.bat

IF %DEVENV%==%MSBUILD35% SET BUILD_CMD=%DEVENV% %BUILD_TYPE% Solution\VS2005_MonoDevelop\%RELEASE-SOLUTION-NAME%
IF %DEVENV%==%VS2005% SET BUILD_CMD=%DEVENV% %BUILD_TYPE% Solution\VS2005_MonoDevelop\%RELEASE-SOLUTION-NAME%
IF %DEVENV%==%VS2008% SET BUILD_CMD=%DEVENV% %BUILD_TYPE% Solution\VS2008\%RELEASE-SOLUTION-NAME%

%BUILD_CMD%

if exist %RELEASE-FOLDER% rd /s /q %RELEASE-FOLDER%
if exist %RELEASE-NAME% rd /s /q %RELEASE-NAME%

mkdir %RELEASE-FOLDER%
xcopy lib\3rdParty\* %RELEASE-NAME%\
xcopy bin\*.dll %RELEASE-NAME%\ /Y
FOR %%F IN (Emgu.CV\README.txt Emgu.CV\Emgu.CV.License.txt bin\Emgu.CV.dll.config bin\Emgu.CV.ML.XML bin\Emgu.CV.ML.dll bin\Emgu.CV.ML.dll.config bin\Emgu.CV.XML bin\Emgu.CV.UI.XML bin\Emgu.Util.dll bin\Emgu.Util.XML) DO COPY %%F %RELEASE-NAME%\

c:\cygwin\bin\zip.exe -r %RELEASE-FOLDER%/%RELEASE-ZIP% %RELEASE-NAME% 
set RELEASE-NAME=
set RELEASE-FOLDER=
set RELEASE-ZIP=
set RELEASE-SOLUTION-NAME=
