# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------
# Copies the built .apk file(s) from SRC_DIR into DEST_DIR (created if it
# doesn't exist yet). Run as a POST_BUILD step rather than a configure-time
# FILE(GLOB) in the caller, since the .apk doesn't exist until the dotnet
# build of the MAUI Android demo app actually finishes.

FILE(GLOB EMGU_MAUI_APK_FILES "${SRC_DIR}/*.apk")
IF(EMGU_MAUI_APK_FILES)
  FILE(MAKE_DIRECTORY "${DEST_DIR}")
  FILE(COPY ${EMGU_MAUI_APK_FILES} DESTINATION "${DEST_DIR}")
ENDIF()
