# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
#
#  Assembles CPACK_PACKAGE_NAME from the platform labels and enabled features
# (CUDA/OpenNI/ICC/IPP/OpenVINO/ONNXRuntime), plus license file selection.
#  Included from the root CMakeLists.txt; CMAKE_CURRENT_SOURCE_DIR still
#  refers to the repository root here (include() does not change it).
# ----------------------------------------------------------------------------

# ----------------------------------------------------------------------------
#  Setup Package information
# ----------------------------------------------------------------------------
SET(PACKAGE "${PROJECT_NAME}")
SET(GITHUB_REPO_URL "https://github.com/emgucv/emgucv")
SET(CPACK_PACKAGE_CONTACT "Emgu CV SUPPORT <support@emgu.com>")
SET(PACKAGE_BUGREPORT "${CPACK_PACKAGE_CONTACT}")
SET(PACKAGE_NAME "${PROJECT_NAME}")

SET(CPACK_PACKAGE_DESCRIPTION_SUMMARY "Emgu CV is a cross platform .Net wrapper to the OpenCV image processing library.")

SET(CPACK_PACKAGE_NAME "${PACKAGE_NAME}${IOS_LABEL}${ANDROID_LABEL}${MACOS_LABEL}${WINDOWS_LABEL}")


IF(WITH_CUDA)
  SET(CPACK_PACKAGE_NAME "${CPACK_PACKAGE_NAME}-cuda")
ENDIF()

IF(WITH_OPENNI) 
  SET(CPACK_PACKAGE_NAME "${CPACK_PACKAGE_NAME}-openni")
ENDIF()

IF(CV_ICC)
  SET(CPACK_PACKAGE_NAME "${CPACK_PACKAGE_NAME}-icc")
ENDIF()


IF(WITH_IPP)
  SET(CPACK_PACKAGE_NAME "${CPACK_PACKAGE_NAME}-ipp")
ENDIF()

IF(WITH_INF_ENGINE OR WITH_OPENVINO)
  SET(CPACK_PACKAGE_NAME "${CPACK_PACKAGE_NAME}-dldt")
ENDIF()

IF(HAVE_ONNXRUNTIME)
  SET(CPACK_PACKAGE_NAME "${CPACK_PACKAGE_NAME}-ort")
ENDIF()

SET(IS_PRO_BUILD FALSE)
IF("${CPACK_PACKAGE_NAME}" STREQUAL "${PACKAGE_NAME}-ios-android-macos-windows-ipp-ort")
  SET(CPACK_PACKAGE_NAME "${PACKAGE_NAME}-pro")
  SET(IS_PRO_BUILD TRUE)
ELSEIF("${CPACK_PACKAGE_NAME}" STREQUAL "${PACKAGE_NAME}-ios-android-macos-windows")
  SET(CPACK_PACKAGE_NAME "${PACKAGE_NAME}-pro-mini")
  SET(IS_PRO_BUILD TRUE)
ENDIF()


SET(LICENSE_FILE_NAME "LICENSE")

SET(CPACK_RESOURCE_FILE_LICENSE "${CMAKE_CURRENT_SOURCE_DIR}/${LICENSE_FILE_NAME}")

SET(CPACK_PACKAGE_CLI_FOLDER libs)
