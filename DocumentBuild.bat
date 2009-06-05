"C:\WINDOWS\Microsoft.NET\Framework\v3.5\MSBuild.exe" Solution\VS2008\Emgu.CV.sln /property:Configuration=Debug
"C:\WINDOWS\Microsoft.NET\Framework\v3.5\MSBuild.exe" Emgu.CV.shfbproj
zip -r Document-2.0.0.0.zip Document\Help
mv Document-2.0.0.0.zip Document\Help\
