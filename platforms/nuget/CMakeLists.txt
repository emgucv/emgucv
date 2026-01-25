# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

ADD_SUBDIRECTORY("Emgu.runtime.windows.msvc.rt.x86")
ADD_SUBDIRECTORY("Emgu.runtime.windows.msvc.rt.x64")
ADD_SUBDIRECTORY("Emgu.runtime.windows.msvc.rt.arm64")

ADD_SUBDIRECTORY("Emgu.runtime.windows.zlibwapi.x64")

ADD_SUBDIRECTORY("Emgu.runtime.windows.cuda.rt")
ADD_SUBDIRECTORY("Emgu.runtime.windows.cuda.fft")
ADD_SUBDIRECTORY("Emgu.runtime.windows.cuda.npp")
ADD_SUBDIRECTORY("Emgu.runtime.windows.cuda.blas")
ADD_SUBDIRECTORY("Emgu.runtime.windows.cuda.blas.lt")
ADD_SUBDIRECTORY("Emgu.runtime.windows.cuda.dnn")
ADD_SUBDIRECTORY("Emgu.runtime.windows.cuda.dnn.cnn")
ADD_SUBDIRECTORY("Emgu.runtime.windows.cuda.dnn.engines")
ADD_SUBDIRECTORY("Emgu.runtime.windows.cuda.dnn.ops")
ADD_SUBDIRECTORY("Emgu.runtime.windows.cuda.dnn.adv")

ADD_SUBDIRECTORY("Emgu.CV.runtime.windows")
ADD_SUBDIRECTORY("Emgu.CV.runtime.uwp")

ADD_SUBDIRECTORY("Emgu.CV.runtime.macos")

add_subdirectory("Emgu.CV.runtime.debian-arm")
add_subdirectory("Emgu.CV.runtime.debian-arm64")

add_subdirectory("Emgu.CV.runtime.ubuntu")

add_subdirectory("Emgu.CV.runtime.rhel")

INSTALL(
      DIRECTORY
      ${CMAKE_CURRENT_SOURCE_DIR}
      DESTINATION "platforms/"
      COMPONENT emgucv_binary
      FILES_MATCHING 
      PATTERN "*.nupkg"
)


