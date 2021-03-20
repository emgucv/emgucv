#!/bin/bash -v

sudo apt-get install build-essential libgtk-3-dev libgstreamer1.0-dev libavcodec-dev libswscale-dev libavformat-dev libdc1394-22-dev libv4l-dev cmake-curses-gui ocl-icd-dev freeglut3-dev libgeotiff-dev libusb-1.0-0-dev

if [ "$1" == "cuda" ]; then
    #sudo apt-get install nvidia-cuda-dev nvidia-cuda-toolkit
    echo Please download and install CUDA 11.2 from nvidia: https://developer.nvidia.com/cuda-downloads?target_os=Linux&target_arch=x86_64&target_distro=Ubuntu&target_version=2004&target_type=runfilelocal
fi
