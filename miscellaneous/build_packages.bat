pushd %~p0
cd ..
rm -rf ..\release
mkdir ..\release

git checkout Emgu.CV.License.txt

call miscellaneous\git-clean.bat
cd android
cd scripts
call build_all.bat
cd ..
cd ..
mv android\build\libemgucv-android.zip ..\release

REM call git clean -f -d
REM build 32 bit package with openni using visual studio, with document
REM call Build_Binary_x86.bat 32 gpu vc openni doc package
REM mv libemgucv*.zip ..\release
REM mv libemgucv*.exe ..\release

call miscellaneous\git-clean.bat
REM build 32 bit package without openni using visual studio, with document
call Build_Binary_x86.bat 32 gpu vc noopenni doc package
mv libemgucv*.zip ..\release
mv libemgucv*.exe ..\release

REM call git clean -f -d
REM build 64 bit package with openni using visual studio, with document
REM call Build_Binary_x86.bat 64 gpu vc openni doc package
REM mv libemgucv*.zip ..\release
REM mv libemgucv*.exe ..\release

call miscellaneous\git-clean.bat
REM build 64 bit package without openni using visual studio, with document
call Build_Binary_x86.bat 64 gpu vc noopenni doc package
mv libemgucv*.zip ..\release
mv libemgucv*.exe ..\release

REM download commercial license
wget http://www.emgu.com/wiki/files/CommercialLicense.txt
cp CommercialLicense.txt Emgu.CV.License.txt

call miscellaneous\git-clean.bat
REM build 32 bit package without gpu, with document
call Build_Binary_x86.bat 32 nogpu intel noopenni doc package
mv libemgucv*.zip ..\release
mv libemgucv*.exe ..\release

call miscellaneous\git-clean.bat
REM build 32 bit package with gpu, using intel compiler, with document
call Build_Binary_x86.bat 32 gpu intel noopenni doc package
mv libemgucv*.zip ..\release
mv libemgucv*.exe ..\release

call miscellaneous\git-clean.bat
REM build 64 bit package without gpu, using intel compiler, with document
call Build_Binary_x86.bat 64 nogpu intel noopenni doc package
mv libemgucv*.zip ..\release
mv libemgucv*.exe ..\release

call miscellaneous\git-clean.bat
REM build 64 bit package with gpu using intel compiler, with document
call Build_Binary_x86.bat 64 gpu intel noopenni doc package
mv libemgucv*.zip ..\release
mv libemgucv*.exe ..\release

popd