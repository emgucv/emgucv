# --------------------------------------------------------
#  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

ADD_SUBDIRECTORY("Emgu.CV.runtime.windows.cuda.cufft")
ADD_SUBDIRECTORY("Emgu.CV.runtime.windows.cuda.dnn")
ADD_SUBDIRECTORY("Emgu.CV.runtime.windows.cuda.npp")

ADD_SUBDIRECTORY("Emgu.CV")

ADD_SUBDIRECTORY("Emgu.CV.runtime.android")
ADD_SUBDIRECTORY("Emgu.CV.runtime.windows")
ADD_SUBDIRECTORY("Emgu.CV.runtime.macos")
ADD_SUBDIRECTORY("Emgu.CV.runtime.uwp")

IF(HAVE_IOS AND WIN32)
  ADD_SUBDIRECTORY("iOS")
ENDIF()
