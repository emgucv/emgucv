# --------------------------------------------------------
#  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

ADD_SUBDIRECTORY("Emgu.runtime.windows.cuda.rt")
ADD_SUBDIRECTORY("Emgu.runtime.windows.cuda.fft")
ADD_SUBDIRECTORY("Emgu.runtime.windows.cuda.npp")
ADD_SUBDIRECTORY("Emgu.runtime.windows.cuda.blas")
ADD_SUBDIRECTORY("Emgu.runtime.windows.cuda.dnn")
ADD_SUBDIRECTORY("Emgu.runtime.windows.cuda.dnn.cnn.infer")
ADD_SUBDIRECTORY("Emgu.runtime.windows.cuda.dnn.cnn.train")
ADD_SUBDIRECTORY("Emgu.runtime.windows.cuda.dnn.ops")
ADD_SUBDIRECTORY("Emgu.runtime.windows.cuda.dnn.adv")

ADD_SUBDIRECTORY("Emgu.CV")
ADD_SUBDIRECTORY("Emgu.CV.Bitmap")
ADD_SUBDIRECTORY("Emgu.CV.Models")
ADD_SUBDIRECTORY("Emgu.CV.UI")

ADD_SUBDIRECTORY("Emgu.CV.runtime.android")
ADD_SUBDIRECTORY("Emgu.CV.runtime.windows")
ADD_SUBDIRECTORY("Emgu.CV.runtime.macos")
ADD_SUBDIRECTORY("Emgu.CV.runtime.uwp")

ADD_SUBDIRECTORY("Emgu.CV.runtime.ios")

IF (HAVE_LINUX_ARM)
  add_subdirectory("Emgu.CV.runtime.linux-arm")
ENDIF()

IF (HAVE_LINUX_X86 OR HAVE_LINUX_X64 OR HAVE_LINUX_ARM64)
  add_subdirectory("Emgu.CV.runtime.ubuntu")
ENDIF()

ADD_SUBDIRECTORY("Emgu.runtime.windows.msvc.rt.x86")
ADD_SUBDIRECTORY("Emgu.runtime.windows.msvc.rt.x64")