:: Script from Open CV project.
:: Modified by Canming Huang, 03/03/2012 for building Emgu CV on android.

@ECHO OFF

PUSHD %~dp0..
CALL .\scripts\build.cmd %*
POPD