REM go to the folder of the current script
pushd %~p0
cd ..
call c:\BuildTools\vc\Auxiliary\Build\vcvars32.bat
call Build_Binary_x86.bat 32 nogpu vc no-openni nodoc nopackage build
