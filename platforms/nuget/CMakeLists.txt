# --------------------------------------------------------
#  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

ADD_SUBDIRECTORY("Emgu.runtime.windows.zlibwapi.x64")

ADD_SUBDIRECTORY("Emgu.runtime.windows.cuda.rt")
ADD_SUBDIRECTORY("Emgu.runtime.windows.cuda.fft")
ADD_SUBDIRECTORY("Emgu.runtime.windows.cuda.npp")
ADD_SUBDIRECTORY("Emgu.runtime.windows.cuda.blas")
ADD_SUBDIRECTORY("Emgu.runtime.windows.cuda.blas.lt")
ADD_SUBDIRECTORY("Emgu.runtime.windows.cuda.dnn")
ADD_SUBDIRECTORY("Emgu.runtime.windows.cuda.dnn.cnn.infer")
ADD_SUBDIRECTORY("Emgu.runtime.windows.cuda.dnn.cnn.train")
ADD_SUBDIRECTORY("Emgu.runtime.windows.cuda.dnn.ops")
ADD_SUBDIRECTORY("Emgu.runtime.windows.cuda.dnn.adv")

ADD_SUBDIRECTORY("Emgu.CV")
#ADD_SUBDIRECTORY("Emgu.CV.Bitmap")
ADD_SUBDIRECTORY("Emgu.CV.Wpf")
ADD_SUBDIRECTORY("Emgu.CV.Models")
ADD_SUBDIRECTORY("Emgu.CV.UI")

ADD_SUBDIRECTORY("Emgu.CV.runtime.windows")
ADD_SUBDIRECTORY("Emgu.CV.runtime.uwp")

ADD_SUBDIRECTORY("Emgu.CV.runtime.xamarin.macos")
ADD_SUBDIRECTORY("Emgu.CV.runtime.xamarin.ios")
ADD_SUBDIRECTORY("Emgu.CV.runtime.xamarin.android")

#ADD_SUBDIRECTORY("Emgu.CV.runtime.maui.macos")
#ADD_SUBDIRECTORY("Emgu.CV.runtime.maui.ios")
#ADD_SUBDIRECTORY("Emgu.CV.runtime.maui.android")

#IF (HAVE_LINUX_ARM)
add_subdirectory("Emgu.CV.runtime.debian-arm")
add_subdirectory("Emgu.CV.runtime.debian-arm64")
#ENDIF()

#IF (HAVE_LINUX_X86 OR HAVE_LINUX_X64 OR HAVE_LINUX_ARM64)
add_subdirectory("Emgu.CV.runtime.ubuntu")
#ENDIF()

ADD_SUBDIRECTORY("Emgu.runtime.windows.msvc.rt.x86")
ADD_SUBDIRECTORY("Emgu.runtime.windows.msvc.rt.x64")
ADD_SUBDIRECTORY("Emgu.runtime.windows.msvc.rt.arm64")