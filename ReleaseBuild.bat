C:\WINDOWS\Microsoft.NET\Framework\v3.5\MSBuild.exe Solution\VS2008\Emgu.CV_VS2008.sln /property:Configuration=Release
rm -f Emgu.CV.Windows.Binary.zip
rm -rf Emgu.CV.Windows.Binary
mkdir Emgu.CV.Windows.Binary
cp Emgu.CV\README.txt Emgu.CV\Emgu.CV.License.txt Emgu.CV\bin\Release\Emgu.CV.dll Emgu.Utils\bin\Release\Emgu.Utils.dll lib\zlib.net.dll lib\zlib.net.license.txt Emgu.CV.Windows.Binary
zip -r Emgu.CV.Windows.Binary.zip Emgu.CV.Windows.Binary 
