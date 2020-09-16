pushd %~p0
call wsl dos2unix build-unity-doc
call wsl ./build-unity-doc
popd