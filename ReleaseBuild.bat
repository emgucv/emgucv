set RELEASE-NAME=Emgu.CV.Windows.Binary-1.3.0.0
SET RELEASE-FOLDER=release_zip
set RELEASE-ZIP=%RELEASE-NAME%.zip

C:\WINDOWS\Microsoft.NET\Framework\v3.5\MSBuild.exe Solution\VS2008\Emgu.CV.sln /property:Configuration=Release
if exist %RELEASE-FOLDER% rm -rf %RELEASE-FOLDER%
if exist %RELEASE-ZIP% rm -f %RELEASE-ZIP%

mkdir %RELEASE-FOLDER%
mkdir %RELEASE-NAME%
cp Emgu.CV\README.txt Emgu.CV\Emgu.CV.License.txt Emgu.CV\bin\Release\Emgu.CV.dll Emgu.Utils\bin\Release\Emgu.Util.dll lib\zlib.net.dll lib\zlib.net.license.txt %RELEASE-NAME%
zip -r %RELEASE-FOLDER%/%RELEASE-ZIP% %RELEASE-NAME% 
set RELEASE-NAME=