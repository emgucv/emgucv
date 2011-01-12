cd ..
mkdir ..\release

call perl miscellaneous\svn-clean.pl .
REM build 32 bit package with gpu using visual studio, with document
call Build_Binary_x86.bat 32 gpu vc doc package
mv emgucv*.zip ..\release
mv emgucv*.exe ..\release

call perl miscellaneous\svn-clean.pl .
REM build 32 bit package with gpu using visual studio, with document
call Build_Binary_x86.bat 32 nogpu vc doc package
mv emgucv*.zip ..\release
mv emgucv*.exe ..\release