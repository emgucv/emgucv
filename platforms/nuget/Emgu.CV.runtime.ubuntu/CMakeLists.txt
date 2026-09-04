# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(HAVE_UBUNTU_X64 OR HAVE_UBUNTU_X86 OR HAVE_UBUNTU_ARM64)
  PROJECT(Emgu.CV.runtime.ubuntu.nuget)

  IF(IS_UBUNTU)
    #MESSAGE(STATUS "TARGET_ARCH_64: ${TARGET_ARCH_64}")
    #SET(NUGET_NATIVE_PLATFORM_ID_UBUNTU "${LSB_RELEASE_ID_SHORT}.${LSB_RELEASE_RELEASE_SHORT}-${NUGET_NATIVE_PLATFORM_ID_ARCH}")
    IF(NOT ("" STREQUAL "${LSB_RELEASE_ID_SHORT}"))
      SET(NUGET_NATIVE_PLATFORM_ID_UBUNTU "${LSB_RELEASE_ID_SHORT}-${NUGET_NATIVE_PLATFORM_ID_ARCH}")
    ELSEIF(NOT ("" STREQUAL "${DISTRO_ID}"))
      SET(NUGET_NATIVE_PLATFORM_ID_UBUNTU "${DISTRO_ID}-${UBUNTU_VERSION}-${NUGET_NATIVE_PLATFORM_ID_ARCH}")
    ENDIF()
    MESSAGE(STATUS "NUGET_NATIVE_PLATFORM_ID_UBUNTU: ${NUGET_NATIVE_PLATFORM_ID_UBUNTU}")
  ELSE()
    SET(UBUNTU_DEFAULT_PLATFORM_ID "ubuntu")
    IF(HAVE_UBUNTU_X86 OR HAVE_UBUNTU_X64 OR HAVE_UBUNTU_ARM64)
      # Attempt to get the ubuntu version from version_string.inc file. This is to make sure the ubuntu version is included in the nuget package id, so that the correct ubuntu version can be installed by users.
      FILE(GLOB VERSION_FILES
        "${CMAKE_CURRENT_SOURCE_DIR}/../../../libs/runtimes/ubuntu.*/native/*version_string.inc"
        "${CMAKE_CURRENT_SOURCE_DIR}/../../../libs/runtimes/ubuntu-arm64/native/*version_string.inc"
        "${CMAKE_CURRENT_SOURCE_DIR}/../../../libs/runtimes/ubuntu-x86/native/*version_string.inc"
        "${CMAKE_CURRENT_SOURCE_DIR}/../../../libs/runtimes/ubuntu-x64/native/*version_string.inc"
      )
      LIST(GET VERSION_FILES 0 VERSION_FILE)
      MESSAGE(STATUS "Attempting to get ubuntu version from file: ${VERSION_FILE}")
      GET_FILENAME_COMPONENT(VERSION_FILE_NAME ${VERSION_FILE} NAME)
      STRING(REGEX MATCH "ubuntu_(.*)_(.*)_version_string.inc" _ ${VERSION_FILE_NAME})
      #ubuntu_${ARCH}_${UBUNTU_VERSION}_version_string.inc
      SET(UBUNTU_VERSION ${CMAKE_MATCH_2})
      # Compute UBUNTU_RID_PREFIX from the detected version
      STRING(REGEX MATCH "^([0-9]+)" _ "${UBUNTU_VERSION}")
      IF(CMAKE_MATCH_1 GREATER 22)
        SET(UBUNTU_RID_PREFIX "ubuntu")
      ELSE()
        SET(UBUNTU_RID_PREFIX "ubuntu.${UBUNTU_VERSION}")
      ENDIF()
    ENDIF()

    IF(HAVE_UBUNTU_X86)
      SET(NUGET_NATIVE_PLATFORM_ID_UBUNTU "${UBUNTU_DEFAULT_PLATFORM_ID}-${UBUNTU_VERSION}-x86")
    ENDIF()
    IF(HAVE_UBUNTU_X64)
      SET(NUGET_NATIVE_PLATFORM_ID_UBUNTU "${UBUNTU_DEFAULT_PLATFORM_ID}-${UBUNTU_VERSION}-x64")
    ENDIF()
    IF(HAVE_UBUNTU_ARM64)
      SET(NUGET_NATIVE_PLATFORM_ID_UBUNTU "${UBUNTU_DEFAULT_PLATFORM_ID}-${UBUNTU_VERSION}-arm64")
    ENDIF()
  ENDIF()
  STRING(TOLOWER "${NUGET_NATIVE_PLATFORM_ID_UBUNTU}" NUGET_NATIVE_PLATFORM_ID_UBUNTU)

  SET(EMGUCV_UBUNTU_NUGET_ID "${EMGUCV_NUGET_ID}.runtime${EMGUCV_RUNTIME_EXTRA_TAG}.${NUGET_NATIVE_PLATFORM_ID_UBUNTU}")

  SET(EMGUCV_UBUNTU_NUGET_TITLE "Emgu CV Native Runtime for Ubuntu")

  SET(EMGUCV_UBUNTU_NUGET_FILE_LIST "")

  IF(HAVE_UBUNTU_X86)
    SET(EMGUCV_UBUNTU_NUGET_FILE_LIST "${EMGUCV_UBUNTU_NUGET_FILE_LIST} <file src=\"..\\..\\..\\libs\\runtimes\\${UBUNTU_RID_PREFIX}-x86\\native\\*.so\" target=\"runtimes\\${UBUNTU_RID_PREFIX}-x86\\native\" />")
    SET(EMGUCV_UBUNTU_NUGET_FILE_LIST "${EMGUCV_UBUNTU_NUGET_FILE_LIST} <file src=\"..\\..\\..\\libs\\runtimes\\${UBUNTU_RID_PREFIX}-x86\\native\\*_version_string.inc\" target=\"docs\" /> ")
  ENDIF()
  IF(HAVE_UBUNTU_X64)
    SET(EMGUCV_UBUNTU_NUGET_FILE_LIST "${EMGUCV_UBUNTU_NUGET_FILE_LIST} <file src=\"..\\..\\..\\libs\\runtimes\\${UBUNTU_RID_PREFIX}-x64\\native\\*.so\" target=\"runtimes\\${UBUNTU_RID_PREFIX}-x64\\native\" />")
    SET(EMGUCV_UBUNTU_NUGET_FILE_LIST "${EMGUCV_UBUNTU_NUGET_FILE_LIST} <file src=\"..\\..\\..\\libs\\runtimes\\${UBUNTU_RID_PREFIX}-x64\\native\\*_version_string.inc\" target=\"docs\" /> ")
  ENDIF()
  IF(HAVE_UBUNTU_ARM64)
    SET(EMGUCV_UBUNTU_NUGET_FILE_LIST "${EMGUCV_UBUNTU_NUGET_FILE_LIST} <file src=\"..\\..\\..\\libs\\runtimes\\${UBUNTU_RID_PREFIX}-arm64\\native\\*.so\" target=\"runtimes\\${UBUNTU_RID_PREFIX}-arm64\\native\" />")
    SET(EMGUCV_UBUNTU_NUGET_FILE_LIST "${EMGUCV_UBUNTU_NUGET_FILE_LIST} <file src=\"..\\..\\..\\libs\\runtimes\\${UBUNTU_RID_PREFIX}-arm64\\native\\*_version_string.inc\" target=\"docs\" /> ")
  ENDIF()

  #Build the RID list actually included in this package, for the README's publish section.
  #HAVE_UBUNTU_X86/X64/ARM64 already account for "already built OR currently
  #building this arch" (see CHECK_BINARY_EXIST in the root CMakeLists.txt),
  #so no extra fallback is needed here, unlike the macOS package.
  SET(EMGUCV_UBUNTU_RID_LIST "")
  IF(HAVE_UBUNTU_X64)
    LIST(APPEND EMGUCV_UBUNTU_RID_LIST "${UBUNTU_RID_PREFIX}-x64")
  ENDIF()
  IF(HAVE_UBUNTU_X86)
    LIST(APPEND EMGUCV_UBUNTU_RID_LIST "${UBUNTU_RID_PREFIX}-x86")
  ENDIF()
  IF(HAVE_UBUNTU_ARM64)
    LIST(APPEND EMGUCV_UBUNTU_RID_LIST "${UBUNTU_RID_PREFIX}-arm64")
  ENDIF()
  LIST(LENGTH EMGUCV_UBUNTU_RID_LIST EMGUCV_UBUNTU_RID_COUNT)
  IF(EMGUCV_UBUNTU_RID_COUNT GREATER 0)
    LIST(GET EMGUCV_UBUNTU_RID_LIST 0 EMGUCV_UBUNTU_PRIMARY_RID)
  ELSE()
    SET(EMGUCV_UBUNTU_PRIMARY_RID "${UBUNTU_RID_PREFIX}-x64")
  ENDIF()
  IF(EMGUCV_UBUNTU_RID_COUNT GREATER 1)
    SET(EMGUCV_UBUNTU_RID_PLURAL_S "s")
  ELSE()
    SET(EMGUCV_UBUNTU_RID_PLURAL_S "")
  ENDIF()
  SET(EMGUCV_UBUNTU_RID_LIST_MD "")
  FOREACH(EMGUCV_UBUNTU_RID_ITEM ${EMGUCV_UBUNTU_RID_LIST})
    LIST(APPEND EMGUCV_UBUNTU_RID_LIST_MD "`${EMGUCV_UBUNTU_RID_ITEM}`")
  ENDFOREACH()
  LIST(JOIN EMGUCV_UBUNTU_RID_LIST_MD " and " EMGUCV_UBUNTU_RID_LIST_TEXT)

  CONFIGURE_FILE(${EMGU_CV_SOURCE_DIR}/cmake/UseRidGraph.props.in ${CMAKE_CURRENT_SOURCE_DIR}/${EMGUCV_UBUNTU_NUGET_ID}.props)
  CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec.in ${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec)
  CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/README.md.in ${CMAKE_CURRENT_SOURCE_DIR}/README.md)

  GET_FILENAME_COMPONENT(NUGET_OUTPUT_DIR ${CMAKE_CURRENT_SOURCE_DIR} DIRECTORY)

  BUILD_NUGET_PACKAGE(
    ${PROJECT_NAME}
    "${EMGU_CV_SOURCE_DIR}/Emgu.CV/NetStandard/Emgu.CV.csproj"  #csproj_file
    "${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec" #nuspec_file
    "${NUGET_OUTPUT_DIR}" #output_dir
    "${CMAKE_CURRENT_SOURCE_DIR}" #working_dir
    "${EMGUCV_UBUNTU_NUGET_ID}" #package_id, see BUILD_NUGET_PACKAGE's comment
    )

  TARGET_SOURCES(${PROJECT_NAME} PRIVATE
    "${CMAKE_CURRENT_SOURCE_DIR}/${EMGUCV_UBUNTU_NUGET_ID}.props"
    "${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec.in"
    "${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec"
    "${CMAKE_CURRENT_SOURCE_DIR}/README.md.in"
    "${CMAKE_CURRENT_SOURCE_DIR}/README.md"
  )

  SOURCE_GROUP("Template" FILES
    "${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec.in"
    "${CMAKE_CURRENT_SOURCE_DIR}/README.md.in")

  SOURCE_GROUP("Nuget" FILES
    "${CMAKE_CURRENT_SOURCE_DIR}/${EMGUCV_UBUNTU_NUGET_ID}.props"
    "${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec"
    "${CMAKE_CURRENT_SOURCE_DIR}/README.md"
  )

  IF(TARGET Emgu.CV)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV)
  ENDIF()

  IF(EMGU_NUGET_SIGN_FOUND)
    EMGU_SIGN_NUGET(${PROJECT_NAME} "${NUGET_OUTPUT_DIR}/${EMGUCV_UBUNTU_NUGET_ID}.${CPACK_PACKAGE_VERSION}.nupkg")
  ENDIF()

  IF(ENABLE_SOLUTION_FOLDERS)
    SET_TARGET_PROPERTIES(${PROJECT_NAME} PROPERTIES FOLDER "nuget")
  ENDIF()
ENDIF()
