# escape=`

# Use the latest vs2019 buildtools with cuda and openvino
FROM emgu/vs2019_buildtools_cuda_openvino:openvino_2021.3.394

#Create a new folder for our project
RUN mkdir emgu

#Change work dir
WORKDIR "c:\emgu"

#use powershell
SHELL ["powershell", "-command"]

#Add nuget.org as a source:
RUN dotnet nuget add source https://api.nuget.org/v3/index.json -n nuget.org

#Create a new console program
RUN dotnet new console

#Add Emgu.CV.runtime package
RUN dotnet add package Emgu.CV.runtime.windows --version 4.5.1.4349

#COPY the source code to the docker image
COPY Program.cs "c:\emgu\Program.cs"

#Compile the program
RUN dotnet build

#COPY dependency walker
#COPY depends.exe "c:\emgu\depends.exe"
#COPY depends.dll "c:\emgu\depends.dll"

#run the program
ENTRYPOINT ["dotnet", "run"]
#ENTRYPOINT ["cmd.exe"]