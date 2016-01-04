SET ZIP_EXEC="C:\Program Files\7-Zip\7z.exe"
rm -rf zip2exe_tmp
mkdir zip2exe_tmp
%ZIP_EXEC% x "%1%" -ozip2exe_tmp -y
cd zip2exe_tmp
%ZIP_EXEC% a -sfx7z.sfx %1%.selfextract.exe * -mx=9 -m0=LZMA2 -mmt=off -md=512m