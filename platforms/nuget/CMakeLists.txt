# --------------------------------------------------------
#  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

ADD_SUBDIRECTORY("Emgu.CV.runtime.windows.cuda.cufft")
ADD_SUBDIRECTORY("Emgu.CV.runtime.windows.cuda.dnn.cnn")
ADD_SUBDIRECTORY("Emgu.CV.runtime.windows.cuda.dnn.ops")
ADD_SUBDIRECTORY("Emgu.CV.runtime.windows.cuda.dnn")
ADD_SUBDIRECTORY("Emgu.CV.runtime.windows.cuda.npp")

ADD_SUBDIRECTORY("Emgu.CV")
ADD_SUBDIRECTORY("Emgu.CV.Bitmap")
ADD_SUBDIRECTORY("Emgu.CV.UI")

ADD_SUBDIRECTORY("Emgu.CV.runtime.android")
ADD_SUBDIRECTORY("Emgu.CV.runtime.windows")
ADD_SUBDIRECTORY("Emgu.CV.runtime.macos")
ADD_SUBDIRECTORY("Emgu.CV.runtime.uwp")

ADD_SUBDIRECTORY("Emgu.CV.runtime.ios")

IF (HAVE_LINUX_ARM)
  add_subdirectory("Emgu.CV.runtime.raspbian")
ENDIF()

IF (HAVE_LINUX_X86 OR HAVE_LINUX_X64)
  add_subdirectory("Emgu.CV.runtime.ubuntu")
ENDIF()

