cd ..
rm -rf ..\release
mkdir ..\release

svn revert Emgu.CV.License.txt

call perl miscellaneous\svn-clean.pl .
REM build 32 bit package with openni using visual studio, with document
call Build_Binary_x86.bat 32 gpu vc openni doc package
mv libemgucv*.zip ..\release
mv libemgucv*.exe ..\release

call perl miscellaneous\svn-clean.pl .
REM build 32 bit package without openni using visual studio, with document
call Build_Binary_x86.bat 32 gpu vc noopenni doc package
mv libemgucv*.zip ..\release
mv libemgucv*.exe ..\release

call perl miscellaneous\svn-clean.pl .
REM build 64 bit package with openni using visual studio, with document
call Build_Binary_x86.bat 64 gpu vc openni doc package
mv libemgucv*.zip ..\release
mv libemgucv*.exe ..\release

call perl miscellaneous\svn-clean.pl .
REM build 64 bit package without openni using visual studio, with document
call Build_Binary_x86.bat 64 gpu vc noopenni doc package
mv libemgucv*.zip ..\release
mv libemgucv*.exe ..\release

REM download commercial license
wget http://www.emgu.com/wiki/files/CommercialLicense.txt
cp CommercialLicense.txt Emgu.CV.License.txt

call perl miscellaneous\svn-clean.pl .
REM build 32 bit package with openni using intel compiler, with document
call Build_Binary_x86.bat 32 gpu intel openni doc package
mv libemgucv*.zip ..\release
mv libemgucv*.exe ..\release

call perl miscellaneous\svn-clean.pl .
REM build 32 bit package without openni using intel compiler, with document
call Build_Binary_x86.bat 32 gpu intel noopenni doc package
mv libemgucv*.zip ..\release
mv libemgucv*.exe ..\release

call perl miscellaneous\svn-clean.pl .
REM build 64 bit package with openni using intel compiler, with document
call Build_Binary_x86.bat 64 gpu intel openni doc package
mv libemgucv*.zip ..\release
mv libemgucv*.exe ..\release

call perl miscellaneous\svn-clean.pl .
REM build 64 bit package without openni using intel compiler, with document
call Build_Binary_x86.bat 64 gpu intel nopenni doc package
mv libemgucv*.zip ..\release
mv libemgucv*.exe ..\release
