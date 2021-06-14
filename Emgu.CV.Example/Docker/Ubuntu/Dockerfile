# Use the bazel-android image
FROM emgu/bazel-android:dotnet-5.0-bazel-4.0

#Create a new folder for our project
RUN mkdir -p /emgu

#Change work dir
WORKDIR "/emgu"

#Add nuget.org as a source:
#RUN dotnet nuget add source https://api.nuget.org/v3/index.json -n nuget.org

#Create a new console program
RUN dotnet new console

#Add Emgu.CV.runtime package
RUN dotnet add package Emgu.CV.runtime.ubuntu.20.04-x64 --version 4.5.1.4349

#COPY the source code to the docker image
COPY Program.cs "/emgu/Program.cs"

#Compile the program
RUN dotnet build

#run the program
ENTRYPOINT ["dotnet", "run"]
