# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

INSTALL_EMGU_CV_RUNTIME_SHARED_PROJECT(Mac)
INSTALL_EMGU_CV_RUNTIME_SHARED_PROJECT(Windows)
INSTALL_EMGU_CV_RUNTIME_SHARED_PROJECT(UWP)
INSTALL_EMGU_CV_RUNTIME_SHARED_PROJECT(Ubuntu)
INSTALL_EMGU_CV_RUNTIME_SHARED_PROJECT(Debian)

ADD_SUBDIRECTORY(Maui)
ADD_SUBDIRECTORY(WebAssembly)
