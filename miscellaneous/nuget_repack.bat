SET ZIP_EXEC="C:\Program Files\7-Zip\7z.exe"
SET TMP_FOLDER=nuget_repack_tmp
rm -rf %TMP_FOLDER%
mkdir %TMP_FOLDER%
%ZIP_EXEC% x "%1%" -onuget_repack_tmp -y
cd %TMP_FOLDER%
%ZIP_EXEC% a %1%.zip * -tzip -mx9 -mm=lzma
%ZIP_EXEC% l %1%.zip
mv %1%.zip ../
cd ..