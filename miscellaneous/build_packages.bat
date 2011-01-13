cd ..
rm -rf ..\release
mkdir ..\release

svn revert Emgu.CV.License.txt

call perl miscellaneous\svn-clean.pl .
REM build 32 bit package with gpu using visual studio, with document
call Build_Binary_x86.bat 32 gpu vc doc package
mv libemgucv*.zip ..\release
mv libemgucv*.exe ..\release

call perl miscellaneous\svn-clean.pl .
REM build 32 bit package without gpu using visual studio, with document
call Build_Binary_x86.bat 32 nogpu vc doc package
mv libemgucv*.zip ..\release
mv libemgucv*.exe ..\release

call perl miscellaneous\svn-clean.pl .
REM build 64 bit package with gpu using visual studio, with document
call Build_Binary_x86.bat 64 gpu vc doc package
mv libemgucv*.zip ..\release
mv libemgucv*.exe ..\release

call perl miscellaneous\svn-clean.pl .
REM build 64 bit package without gpu using visual studio, with document
call Build_Binary_x86.bat 64 nogpu vc doc package
mv libemgucv*.zip ..\release
mv libemgucv*.exe ..\release

REM download commercial license
wget http://www.emgu.com/wiki/files/CommercialLicense.txt
cp CommercialLicense.txt Emgu.CV.License.txt

call perl miscellaneous\svn-clean.pl .
REM build 32 bit package with gpu using intel compiler, with document
call Build_Binary_x86.bat 32 gpu intel doc package
mv libemgucv*.zip ..\release
mv libemgucv*.exe ..\release

call perl miscellaneous\svn-clean.pl .
REM build 32 bit package without gpu using intel compiler, with document
call Build_Binary_x86.bat 32 nogpu intel doc package
mv libemgucv*.zip ..\release
mv libemgucv*.exe ..\release

call perl miscellaneous\svn-clean.pl .
REM build 64 bit package with gpu using intel compiler, with document
call Build_Binary_x86.bat 64 gpu intel doc package
mv libemgucv*.zip ..\release
mv libemgucv*.exe ..\release

call perl miscellaneous\svn-clean.pl .
REM build 64 bit package without gpu using intel compiler, with document
call Build_Binary_x86.bat 64 nogpu intel doc package
mv libemgucv*.zip ..\release
mv libemgucv*.exe ..\release
