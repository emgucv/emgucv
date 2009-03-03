set RELEASE-NAME=Emgu.CV.Binary-2.0.0.0
set RELEASE-FOLDER=release_zip
set RELEASE-ZIP=%RELEASE-NAME%.zip

call BinaryBuild.bat

IF EXIST "C:\Program Files\Microsoft Visual Studio 8\Common7\IDE\devenv.exe" ("C:\Program Files\Microsoft Visual Studio 8\Common7\IDE\devenv.exe" /Build Release Solution\VS2005_MonoDevelop\Emgu.CV.sln) ELSE (C:\WINDOWS\Microsoft.NET\Framework\v3.5\MSBuild.exe Solution\VS2008\Emgu.CV.sln /property:Configuration=Release ) 

if exist %RELEASE-FOLDER% rm -rf %RELEASE-FOLDER%
if exist %RELEASE-NAME% rm -rf %RELEASE-NAME%

mkdir %RELEASE-FOLDER%
xcopy lib\3rdParty\* %RELEASE-NAME%\
cp Emgu.CV\README.txt Emgu.CV\Emgu.CV.License.txt bin\Emgu.CV.dll bin\Emgu.CV.dll.config bin\cvextern.dll bin\Emgu.CV.ML.XML bin\Emgu.CV.ML.dll bin\Emgu.CV.ML.dll.config bin\Emgu.CV.XML bin\Emgu.CV.UI.dll bin\Emgu.CV.UI.XML bin\Emgu.Util.dll bin\Emgu.Util.XML %RELEASE-NAME%\

zip -r %RELEASE-FOLDER%/%RELEASE-ZIP% %RELEASE-NAME% 
set RELEASE-NAME=
set RELEASE-FOLDER=
set RELEASE-ZIP=
