# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------
# Copies the built MAUI demo package(s) from SRC_DIR into DEST_DIR (created if
# it doesn't exist yet). Run as a POST_BUILD step rather than a configure-time
# FILE(GLOB) in the caller, since the package doesn't exist until the dotnet
# build of the MAUI demo app actually finishes.
#
# PATTERN (optional) names the files to copy. Without it, the Android .apk is
# copied: the demo's csproj copies the signed package to
# Emgu.CV.Maui.Demo.android.<version>.apk; when that file is there, only it is
# copied. Otherwise every .apk is, as before.

IF(PATTERN)
  FILE(GLOB EMGU_MAUI_APK_FILES "${SRC_DIR}/${PATTERN}")
ELSE()
  FILE(GLOB EMGU_MAUI_APK_FILES "${SRC_DIR}/Emgu.CV.Maui.Demo.android.*.apk")
  IF(NOT EMGU_MAUI_APK_FILES)
    FILE(GLOB EMGU_MAUI_APK_FILES "${SRC_DIR}/*.apk")
  ENDIF()
ENDIF()
IF(EMGU_MAUI_APK_FILES)
  FILE(MAKE_DIRECTORY "${DEST_DIR}")
  FILE(COPY ${EMGU_MAUI_APK_FILES} DESTINATION "${DEST_DIR}")
ENDIF()
