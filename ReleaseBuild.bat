set RELEASE-NAME=Emgu.CV.Windows.Binary-1.3.0.0
set RELEASE-ZIP=%RELEASE-NAME%.zip

C:\WINDOWS\Microsoft.NET\Framework\v3.5\MSBuild.exe Solution\VS2008\Emgu.CV.sln /property:Configuration=Release

if exist %RELEASE-ZIP% rm %RELEASE-NAME%.zip
if exist %RELEASE-NAME% rm -rf %RELEASE-NAME%
mkdir %RELEASE-NAME%

cp Emgu.CV\README.txt Emgu.CV\Emgu.CV.License.txt Emgu.CV\bin\Release\Emgu.CV.dll Emgu.Utils\bin\Release\Emgu.Utils.dll lib\zlib.net.dll lib\zlib.net.license.txt %RELEASE-NAME%
zip -r %RELEASE-ZIP% %RELEASE-NAME% 
set RELEASE-NAME=