# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(HAVE_MACOS)

  PROJECT(Emgu.CV.runtime.macos.nuget)

  SET(EMGUCV_MACOS_NUGET_ID "${EMGUCV_NUGET_ID}.runtime${EMGUCV_RUNTIME_EXTRA_TAG}.macos")

  #Build the RID list actually included in this package, for the README's publish section.
  #Checked directly here (rather than via CHECK_BINARY_EXIST, which resolves paths
  #relative to the root CMakeLists.txt and isn't safe to call from this nested
  #directory) so it stays in sync with whichever architectures were actually built.
  #A dylib glob alone isn't enough: on a fresh configure (before the native build
  #has run) the file wouldn't exist yet even though this invocation is building it,
  #so also fall back to the root CMakeLists.txt's IS_MAC_X86_64_BUILD/IS_MAC_ARM64_BUILD
  #build-intent flags, same "already built OR currently building this arch" logic
  #CHECK_BINARY_EXIST uses for the Windows/Ubuntu/RHEL packages.
  FILE(GLOB EMGUCV_MACOS_X64_DYLIBS "${CMAKE_CURRENT_SOURCE_DIR}/../../../libs/runtimes/osx-x64/native/*.dylib")
  FILE(GLOB EMGUCV_MACOS_ARM64_DYLIBS "${CMAKE_CURRENT_SOURCE_DIR}/../../../libs/runtimes/osx-arm64/native/*.dylib")
  SET(EMGUCV_MACOS_RID_LIST "")
  IF(EMGUCV_MACOS_X64_DYLIBS OR IS_MAC_X86_64_BUILD)
    LIST(APPEND EMGUCV_MACOS_RID_LIST "osx-x64")
  ENDIF()
  IF(EMGUCV_MACOS_ARM64_DYLIBS OR IS_MAC_ARM64_BUILD)
    LIST(APPEND EMGUCV_MACOS_RID_LIST "osx-arm64")
  ENDIF()
  LIST(LENGTH EMGUCV_MACOS_RID_LIST EMGUCV_MACOS_RID_COUNT)
  IF(EMGUCV_MACOS_RID_COUNT GREATER 0)
    LIST(GET EMGUCV_MACOS_RID_LIST 0 EMGUCV_MACOS_PRIMARY_RID)
  ELSE()
    SET(EMGUCV_MACOS_PRIMARY_RID "osx-x64")
  ENDIF()
  IF(EMGUCV_MACOS_RID_COUNT GREATER 1)
    SET(EMGUCV_MACOS_RID_PLURAL_S "s")
  ELSE()
    SET(EMGUCV_MACOS_RID_PLURAL_S "")
  ENDIF()
  SET(EMGUCV_MACOS_RID_LIST_MD "")
  FOREACH(EMGUCV_MACOS_RID_ITEM ${EMGUCV_MACOS_RID_LIST})
    LIST(APPEND EMGUCV_MACOS_RID_LIST_MD "`${EMGUCV_MACOS_RID_ITEM}`")
  ENDFOREACH()
  LIST(JOIN EMGUCV_MACOS_RID_LIST_MD " and " EMGUCV_MACOS_RID_LIST_TEXT)

  CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec.in ${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec)
  CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/README.md.in ${CMAKE_CURRENT_SOURCE_DIR}/docs/README.md)

  GET_FILENAME_COMPONENT(NUGET_OUTPUT_DIR ${CMAKE_CURRENT_SOURCE_DIR} DIRECTORY)

  BUILD_NUGET_PACKAGE(
    ${PROJECT_NAME}
    "${EMGU_CV_SOURCE_DIR}/Emgu.CV/NetStandard/Emgu.CV.csproj"  #csproj_file
    "${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec" #nuspec_file
    "${NUGET_OUTPUT_DIR}" #output_dir
    "${CMAKE_CURRENT_SOURCE_DIR}" #working_dir
    )

  TARGET_SOURCES(${PROJECT_NAME} PRIVATE
    "${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec.in"
    "${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec"
    "${CMAKE_CURRENT_SOURCE_DIR}/README.md.in"
    "${CMAKE_CURRENT_SOURCE_DIR}/docs/README.md"
  )

  SOURCE_GROUP("Template" FILES
    "${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec.in"
    "${CMAKE_CURRENT_SOURCE_DIR}/README.md.in")

  SOURCE_GROUP("Nuget" FILES
    "${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec"
    "${CMAKE_CURRENT_SOURCE_DIR}/docs/README.md"
  )

  IF(EMGU_NUGET_SIGN_FOUND)
    EMGU_SIGN_NUGET(${PROJECT_NAME} "${NUGET_OUTPUT_DIR}/${EMGUCV_MACOS_NUGET_ID}.${CPACK_PACKAGE_VERSION}.nupkg")
  ENDIF()

  IF(ENABLE_SOLUTION_FOLDERS)
    SET_TARGET_PROPERTIES(${PROJECT_NAME} PROPERTIES FOLDER "nuget")
  ENDIF()

  ADD_DEPENDENCIES(${PROJECT_NAME} cvextern)

  IF(TARGET Emgu.CV)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV)
  ENDIF()

ENDIF()
