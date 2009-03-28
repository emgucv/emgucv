SET VERSION=2.0.0.0
SET VS2005_FOLDER=Solution\VS2005_MonoDevelop\
SET VS2008_FOLDER=Solution\VS2008\

IF exist src rd /s /q src

REM == Build OpenCV ==
call BinaryBuild.bat

xcopy bin\release\*.dll src\bin\
xcopy /E /i opencv\include\opencv src\opencv\include\opencv
xcopy lib\release\*.lib src\lib\release\
xcopy /E /i lib\3rdParty src\lib\3rdParty

REM == Clean up all folders except src folder == 
perl miscellaneous\svn-clean.pl --exclude "^src(/|$)"

REM == Copy other files ==
copy Emgu.CV\README.txt + README.txt src\README.txt
cp -r Emgu.CV Emgu.CV.Extern Emgu.CV.ML Emgu.Util Emgu.CV.UI Emgu.CV.Example src\
install -d src\%VS2005_FOLDER%
install -d src\%VS2008_FOLDER%
cp %VS2005_FOLDER%Emgu.CV.sln %VS2005_FOLDER%Emgu.CV.Example.sln src src\%VS2005_FOLDER%
cp %VS2008_FOLDER%Emgu.CV.sln  %VS2008_FOLDER%Emgu.CV.Example.sln src\%VS2008_FOLDER%
tar --exclude-vcs -cvf Emgu.CV.SourceAndExamples-%VERSION%.tar src
rm -rf src/*
move Emgu.CV.SourceAndExamples-%VERSION%.tar src
REM tar -xvf Emgu.CV.SourceAndExamples-%VERSION%.tar src
