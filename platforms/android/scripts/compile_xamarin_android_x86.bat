REM @echo off
pushd %~p0

call compile_xamarin_android.bat x86

:END
popd