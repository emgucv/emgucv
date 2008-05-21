set RELEASE-NAME=Emgu.CV.Windows-1.2.2.0-Binary
C:\WINDOWS\Microsoft.NET\Framework\v3.5\MSBuild.exe Solution\VS2008\Emgu.CV_VS2008.sln /property:Configuration=Release
rm -f %RELEASE-NAME%.zip
rm -rf %RELEASE-NAME%
mkdir %RELEASE-NAME%
cp Emgu.CV\README.txt Emgu.CV\Emgu.CV.License.txt Emgu.CV\bin\Release\Emgu.CV.dll Emgu.Utils\bin\Release\Emgu.Utils.dll lib\zlib.net.dll lib\zlib.net.license.txt %RELEASE-NAME%
zip -r %RELEASE-NAME%.zip %RELEASE-NAME% 
set RELEASE-NAME=