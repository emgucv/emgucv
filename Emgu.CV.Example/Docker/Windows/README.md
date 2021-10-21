==================================================================


**Instructions**

Run "Build_Docker.bat" script to build the windows x86-64 docker image with Emgu CV

Run "run_Docker.bat" script to run the windows x86-64 docker image with Emgu CV

**Note:**

_The base docker image is about 45GB, which contains all the tools needed to build Emgu CV. Docker's windows default maximum image size is 20GB, you may need to increase the maximum Docker image size to something bigger (e.g. 60GB or more)._

_If you want a smaller docker image size, you may build an image base on mcr.microsoft.com/windows:10.0.18363.1500-amd64 and install dotnet SDK in it. You can't use image based on mcr.microsoft.com/dotnet/aspnet:5.0 which is missing the windows media foundation dependency for Open CV._
