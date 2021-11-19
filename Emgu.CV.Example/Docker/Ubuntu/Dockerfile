# Use the bazel-android image
FROM emgu/bazel-android:dotnet-6.0-bazel-4.2.1

#Create a new folder for our project
RUN mkdir -p /emgu

#Change work dir
WORKDIR "/emgu"

#Add nuget.org as a source:
#RUN dotnet nuget add source https://api.nuget.org/v3/index.json -n nuget.org

#Create a new console program
RUN dotnet new console

#Add Emgu.CV.runtime package
RUN dotnet add package Emgu.CV.runtime.ubuntu.20.04-x64 --version 4.5.4.4788

#Copy the source code to the docker image
COPY Program.cs "/emgu/Program.cs"

#Build and publish the executable
RUN dotnet publish -c Release

#Move the published application to /app folder
RUN mv bin/Release/net6.0/publish /app

#Change work dir
WORKDIR "/app"

#run the program
ENTRYPOINT ["./emgu"]
