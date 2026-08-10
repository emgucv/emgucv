# - Shared CMake build logic for the Debian Linux runtime nuget packages
# It defines the following macro:
#
# BUILD_DEBIAN_LINUX_RUNTIME_PACKAGE (<arch_lower> <arch_upper> <have_flag>)
#
# Used by Emgu.CV.runtime.debian-arm and Emgu.CV.runtime.debian-arm64, which
# are nearly identical aside from their target architecture. Each package
# keeps its own directory/CMakeLists.txt/Package.nuspec.in/README.md.in (same
# convention as every other runtime package), but the CMake logic itself
# lives here once instead of being duplicated per architecture. Must be
# called with CMAKE_CURRENT_SOURCE_DIR already set to the calling package's
# own directory (i.e. from within that package's CMakeLists.txt).

MACRO(BUILD_DEBIAN_LINUX_RUNTIME_PACKAGE arch_lower arch_upper have_flag)
  IF(${have_flag})
    PROJECT(Emgu.CV.runtime.debian-${arch_lower}.nuget)

    IF(NOT DEFINED DEBIAN_VERSION)
      # Attempt to get the debian version from version_string.inc file. This is to make sure the debian version is included in the nuget package id, so that the correct debian version can be installed by users.
      file(GLOB VERSION_FILES
        "${CMAKE_CURRENT_SOURCE_DIR}/../../../libs/runtimes/linux-${arch_lower}/native/*version_string.inc"
      )
      LIST(GET VERSION_FILES 0 VERSION_FILE)
      MESSAGE(STATUS "Attempting to get debian version from file: ${VERSION_FILE}")
      get_filename_component(VERSION_FILE_NAME ${VERSION_FILE} NAME)
      string(REGEX MATCH "debian_${arch_lower}_(.*)_version_string.inc" _ ${VERSION_FILE_NAME})
      set(DEBIAN_VERSION ${CMAKE_MATCH_1})
    ENDIF()

    SET(EMGUCV_DEBIAN_RUNTIME_NUGET_ID "${EMGUCV_NUGET_ID}.runtime${EMGUCV_RUNTIME_EXTRA_TAG}.debian-${DEBIAN_VERSION}-${arch_lower}")
    SET(EMGUCV_DEBIAN_RUNTIME_NUGET_TITLE "Emgu CV Native ${arch_upper} Runtime for Debian (including Raspberry Pi OS)")

    SET(EMGUCV_DEBIAN_RUNTIME_NUGET_FILE_LIST "")
    SET(EMGUCV_DEBIAN_RUNTIME_NUGET_FILE_LIST "${EMGUCV_DEBIAN_RUNTIME_NUGET_FILE_LIST}
        <file src=\"..\\..\\..\\libs\\runtimes\\linux-${arch_lower}\\native\\*.so\" target=\"runtimes\\linux-${arch_lower}\\native\" />")
    SET(EMGUCV_DEBIAN_RUNTIME_NUGET_FILE_LIST "${EMGUCV_DEBIAN_RUNTIME_NUGET_FILE_LIST}
        <file src=\"..\\..\\..\\libs\\runtimes\\linux-${arch_lower}\\native\\*_version_string.inc\" target=\"docs\" />")

    CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec.in ${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec)
    CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/README.md.in ${CMAKE_CURRENT_SOURCE_DIR}/README.md)

    get_filename_component(NUGET_OUTPUT_DIR ${CMAKE_CURRENT_SOURCE_DIR} DIRECTORY)

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
      "${CMAKE_CURRENT_SOURCE_DIR}/README.md"
    )

    source_group("Template" FILES
      "${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec.in"
      "${CMAKE_CURRENT_SOURCE_DIR}/README.md.in")

    source_group("Nuget" FILES
      "${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec"
      "${CMAKE_CURRENT_SOURCE_DIR}/README.md"
    )

    IF (EMGU_NUGET_SIGN_FOUND)
      EMGU_SIGN_NUGET(${PROJECT_NAME} "${NUGET_OUTPUT_DIR}/${EMGUCV_DEBIAN_RUNTIME_NUGET_ID}.${CPACK_PACKAGE_VERSION}.nupkg")
    ENDIF()

    ADD_DEPENDENCIES(${PROJECT_NAME} cvextern)

    IF(TARGET Emgu.CV)
      ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV)
    ENDIF()

    if(ENABLE_SOLUTION_FOLDERS)
      set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "nuget")
    endif()
  ENDIF()
ENDMACRO()
