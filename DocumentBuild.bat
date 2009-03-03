call BinaryBuild.bat

C:\WINDOWS\Microsoft.NET\Framework\v3.5\MSBuild.exe Solution\VS2008\Emgu.CV.sln /property:Configuration=Debug
"C:\Program Files\EWSoftware\Sandcastle Help File Builder\SandcastleBuilderConsole.exe" Document\Emgu.CV.shfb
find Document\Help\html -name "*.htm" -type f -exec sed -i -f C:\Users\canming\Desktop\cruisecontrol\emgucv\Cruisecontrol\adscript.txt  {} ;
zip -r Document-2.0.0.0.zip Document\Help
mv Document-2.0.0.0.zip Document\Help\
