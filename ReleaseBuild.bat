set RELEASE-NAME=Emgu.CV.Windows.Binary-1.5.0.0
SET RELEASE-FOLDER=release_zip
set RELEASE-ZIP=%RELEASE-NAME%.zip

C:\WINDOWS\Microsoft.NET\Framework\v3.5\MSBuild.exe Solution\VS2008\Emgu.CV.sln /property:Configuration=Release
if exist %RELEASE-FOLDER% rm -rf %RELEASE-FOLDER%
if exist %RELEASE-NAME% rm -rf %RELEASE-NAME%

mkdir %RELEASE-FOLDER%
mkdir %RELEASE-NAME%
cp Emgu.CV\README.txt Emgu.CV\Emgu.CV.License.txt bin\Emgu.CV.dll bin\Emgu.CV.XML bin\Emgu.CV.UI.dll bin\Emgu.CV.UI.XML bin\Emgu.Util.dll bin\Emgu.Util.XML lib\zlib.net.dll lib\zlib.net.license.txt lib\ZedGraph.dll lib\ZedGraph.XML lib\ZedGraph.license.txt %RELEASE-NAME%\
zip -r %RELEASE-FOLDER%/%RELEASE-ZIP% %RELEASE-NAME% 
set RELEASE-NAME=
