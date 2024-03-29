rm -rf package/*.nupkg

docker build -t emgu/emgucv-ubi-build-nuget .

FOR /F "tokens=* USEBACKQ" %%F IN (`git rev-parse HEAD`) DO (
SET GIT_HEAD_HASH=%%F
)
ECHO %GIT_HEAD_HASH%

docker run --mount type=bind,source=%cd%\package,target=/package emgu/emgucv-ubi-build-nuget:latest bash -c "cd emgucv;git fetch;git checkout %GIT_HEAD_HASH%;git submodule update --init --recursive;cd platforms/rhel/9;./cmake_configure;cd build;make;cp /emgucv/platforms/nuget/*.nupkg /package/"

cp package/*.nupkg ../../../nuget/
