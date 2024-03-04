# Use the bazel-android image
FROM emgu/emgucv-ubi:ubi9-dotnet8

#Create a new folder for our project
RUN mkdir -p /emgu

#Create a new folder for nuget package
RUN mkdir -p /package

#Copy the nuget package to the docker image
COPY package/*.nupkg "/package/"

#Change work dir
WORKDIR "/emgu"

#Add nuget.org as a source:
RUN dotnet nuget add source /package -n local_nuget_repo

#Create a new console program
RUN dotnet new console

#Add Emgu.CV.runtime package
RUN dotnet add package Emgu.CV.runtime.mini.rhel-x64 

#Copy the source code to the docker image
COPY Program.cs "/emgu/Program.cs"

#Need to set the UseRidGraph to true during build for .Net 8+. 
#See https://learn.microsoft.com/en-ca/dotnet/core/rid-catalog#using-rids
COPY Directory.Build.props "/emgu/"

#Build and publish the executable
RUN dotnet publish -c Release

#Move the published application to /app folder
RUN mv bin/Release/net8.0/publish /app

#Change work dir
WORKDIR "/app"

#run the program
ENTRYPOINT ["./emgu"]
