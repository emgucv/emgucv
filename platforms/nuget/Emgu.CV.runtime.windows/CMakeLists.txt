# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF( (TARGET Emgu.CV)
  AND (HAVE_WINDESKTOP)
  AND (NOT ("${EMGU_WINDOWS_MSVCRT_VERSION}" STREQUAL "")) )

  PROJECT(Emgu.CV.runtime.windows.nuget)

  SET(EMGUCV_WINDOWS_NUGET_FILE_LIST "")
  IF(HAVE_WINDESKTOP_X86)
    SET(EMGUCV_WINDOWS_NUGET_FILE_LIST "${EMGUCV_WINDOWS_NUGET_FILE_LIST} <file src=\"..\\..\\..\\libs\\runtimes\\win-x86\\native\\*.dll\" exclude=\"..\\..\\..\\libs\\runtimes\\win-x86\\native\\cu*.dll;..\\..\\..\\libs\\runtimes\\win-x86\\native\\npp*.dll;..\\..\\..\\libs\\runtimes\\win-x86\\native\\concrt*.dll;..\\..\\..\\libs\\runtimes\\win-x86\\native\\msvcp*.dll;..\\..\\..\\libs\\runtimes\\win-x86\\native\\vcruntime*.dll;..\\..\\..\\libs\\runtimes\\win-x86\\native\\vccorlib*.dll\" target=\"runtimes\\win-x86\\native\" />")
    SET(EMGUCV_WINDOWS_NUGET_FILE_LIST "${EMGUCV_WINDOWS_NUGET_FILE_LIST} <file src=\"..\\..\\..\\libs\\runtimes\\win-x86\\native\\version_string.inc\" target=\"docs\\build_info_x86.inc\" />")
  ENDIF()
  IF(HAVE_WINDESKTOP_X64)
    SET(EMGUCV_WINDOWS_NUGET_FILE_LIST "${EMGUCV_WINDOWS_NUGET_FILE_LIST} <file src=\"..\\..\\..\\libs\\runtimes\\win-x64\\native\\*.dll\" exclude=\"..\\..\\..\\libs\\runtimes\\win-x64\\native\\cu*.dll;..\\..\\..\\libs\\runtimes\\win-x64\\native\\npp*.dll;..\\..\\..\\libs\\runtimes\\win-x64\\native\\concrt*.dll;..\\..\\..\\libs\\runtimes\\win-x64\\native\\msvcp*.dll;..\\..\\..\\libs\\runtimes\\win-x64\\native\\vcruntime*.dll;..\\..\\..\\libs\\runtimes\\win-x64\\native\\vccorlib*.dll;..\\..\\..\\libs\\runtimes\\win-x64\\native\\zlibwapi.dll\" target=\"runtimes\\win-x64\\native\" />")
    #Also added configuration files from OpenVino
    SET(EMGUCV_WINDOWS_NUGET_FILE_LIST "${EMGUCV_WINDOWS_NUGET_FILE_LIST} <file src=\"..\\..\\..\\libs\\runtimes\\win-x64\\native\\*.xml\" target=\"runtimes\\win-x64\\native\" />")
    SET(EMGUCV_WINDOWS_NUGET_FILE_LIST "${EMGUCV_WINDOWS_NUGET_FILE_LIST} <file src=\"..\\..\\..\\libs\\runtimes\\win-x64\\native\\*.json\" target=\"runtimes\\win-x64\\native\" />")
    SET(EMGUCV_WINDOWS_NUGET_FILE_LIST "${EMGUCV_WINDOWS_NUGET_FILE_LIST} <file src=\"..\\..\\..\\libs\\runtimes\\win-x64\\native\\*.elf\" target=\"runtimes\\win-x64\\native\" />")
    SET(EMGUCV_WINDOWS_NUGET_FILE_LIST "${EMGUCV_WINDOWS_NUGET_FILE_LIST} <file src=\"..\\..\\..\\libs\\runtimes\\win-x64\\native\\*.mvcmd\" target=\"runtimes\\win-x64\\native\" />")
    SET(EMGUCV_WINDOWS_NUGET_FILE_LIST "${EMGUCV_WINDOWS_NUGET_FILE_LIST} <file src=\"..\\..\\..\\libs\\runtimes\\win-x64\\native\\version_string.inc\" target=\"docs\\build_info_x64.inc\" />")
  ENDIF()
  IF(HAVE_WINDESKTOP_ARM64)
    SET(EMGUCV_WINDOWS_NUGET_FILE_LIST "${EMGUCV_WINDOWS_NUGET_FILE_LIST}
       <file src=\"..\\..\\..\\libs\\runtimes\\win-arm64\\native\\*.dll\" exclude=\"..\\..\\..\\libs\\runtimes\\win-arm64\\native\\cu*.dll;..\\..\\..\\libs\\runtimes\\win-arm64\\native\\npp*.dll;..\\..\\..\\libs\\runtimes\\win-arm64\\native\\concrt*.dll;..\\..\\..\\libs\\runtimes\\win-arm64\\native\\msvcp*.dll;..\\..\\..\\libs\\runtimes\\win-arm64\\native\\vcruntime*.dll;..\\..\\..\\libs\\runtimes\\win-arm64\\native\\vccorlib*.dll;..\\..\\..\\libs\\runtimes\\win-arm64\\native\\opencv_videoio_ffmpeg*.dll\" target=\"runtimes\\win-arm64\\native\" />
       <file src=\"..\\..\\..\\libs\\runtimes\\win-arm64\\native\\version_string.inc\" target=\"docs\\build_info_arm64.inc\" />")
  ENDIF()

  #Build the RID list actually included in this package, for the README's publish section.
  #This keeps the documented RIDs in sync with whichever architectures were
  #actually built (HAVE_WINDESKTOP_X86/X64/ARM64), instead of hard-coding all three.
  SET(EMGUCV_WINDOWS_RID_LIST "")
  IF(HAVE_WINDESKTOP_X64)
    LIST(APPEND EMGUCV_WINDOWS_RID_LIST "win-x64")
  ENDIF()
  IF(HAVE_WINDESKTOP_X86)
    LIST(APPEND EMGUCV_WINDOWS_RID_LIST "win-x86")
  ENDIF()
  IF(HAVE_WINDESKTOP_ARM64)
    LIST(APPEND EMGUCV_WINDOWS_RID_LIST "win-arm64")
  ENDIF()
  LIST(GET EMGUCV_WINDOWS_RID_LIST 0 EMGUCV_WINDOWS_PRIMARY_RID)
  LIST(LENGTH EMGUCV_WINDOWS_RID_LIST EMGUCV_WINDOWS_RID_COUNT)
  IF(EMGUCV_WINDOWS_RID_COUNT GREATER 1)
    SET(EMGUCV_WINDOWS_RID_PLURAL_S "s")
  ELSE()
    SET(EMGUCV_WINDOWS_RID_PLURAL_S "")
  ENDIF()
  #Wrap each RID in backticks for the markdown code-span style used elsewhere in this doc
  SET(EMGUCV_WINDOWS_RID_LIST_MD "")
  FOREACH(EMGUCV_WINDOWS_RID_ITEM ${EMGUCV_WINDOWS_RID_LIST})
    LIST(APPEND EMGUCV_WINDOWS_RID_LIST_MD "`${EMGUCV_WINDOWS_RID_ITEM}`")
  ENDFOREACH()
  LIST(JOIN EMGUCV_WINDOWS_RID_LIST_MD ", " EMGUCV_WINDOWS_RID_LIST_TEXT)

  IF(WITH_CUDA)
    SET(EMGUCV_WINDOWS_CUDA_RID_NOTE "\n\nNote: this CUDA-enabled release only includes the `${EMGUCV_WINDOWS_PRIMARY_RID}` native binary; other architectures are not available in this release.")
  ELSE()
    SET(EMGUCV_WINDOWS_CUDA_RID_NOTE "")
  ENDIF()

  CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec.in ${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec)
  CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/README.md.in ${CMAKE_CURRENT_SOURCE_DIR}/README.md)

  GET_FILENAME_COMPONENT(NUGET_OUTPUT_DIR ${CMAKE_CURRENT_SOURCE_DIR} DIRECTORY)

  BUILD_NUGET_PACKAGE(
    ${PROJECT_NAME}
    "${EMGU_CV_SOURCE_DIR}/Emgu.CV/NetStandard/Emgu.CV.csproj"  #csproj_file
    "${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec" #nuspec_file
    "${NUGET_OUTPUT_DIR}" #output_dir
    "${CMAKE_CURRENT_SOURCE_DIR}" #working_dir
    "${EMGUCV_WINDOWS_NUGET_ID}" #package_id, see BUILD_NUGET_PACKAGE's comment
    )

  TARGET_SOURCES(${PROJECT_NAME} PRIVATE
    "${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec.in"
    "${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec"
    "${CMAKE_CURRENT_SOURCE_DIR}/README.md.in"
    "${CMAKE_CURRENT_SOURCE_DIR}/README.md"
  )

  SOURCE_GROUP("Template" FILES
    "${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec.in"
    "${CMAKE_CURRENT_SOURCE_DIR}/README.md.in")

  SOURCE_GROUP("Nuget" FILES
    "${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec"
    "${CMAKE_CURRENT_SOURCE_DIR}/README.md"
  )

  IF(EMGU_NUGET_SIGN_FOUND)
    EMGU_SIGN_NUGET(${PROJECT_NAME} "${NUGET_OUTPUT_DIR}/${EMGUCV_WINDOWS_NUGET_ID}.${CPACK_PACKAGE_VERSION}.nupkg")
  ENDIF()

  IF(ENABLE_SOLUTION_FOLDERS)
    SET_TARGET_PROPERTIES(${PROJECT_NAME} PROPERTIES FOLDER "nuget")
  ENDIF()

  ADD_DEPENDENCIES(${PROJECT_NAME} cvextern)

  IF(TARGET Emgu.CV)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV)
  ENDIF()

  IF(WIN32 AND HAVE_WINDESKTOP_X64 AND WITH_CUDA)
    ADD_DEPENDENCIES(${PROJECT_NAME}
      Emgu.runtime.windows.cuda.fft.nuget
      Emgu.runtime.windows.cuda.dnn.cnn.nuget
      Emgu.runtime.windows.cuda.dnn.engines.nuget
      Emgu.runtime.windows.cuda.dnn.ops.nuget
      Emgu.runtime.windows.cuda.dnn.adv.nuget
      Emgu.runtime.windows.cuda.blas.nuget
      Emgu.runtime.windows.cuda.blas.lt.nuget
      Emgu.runtime.windows.cuda.npp.nuget)

    # Also declared as <dependency> entries in this package's own generated
    # nuspec (cmake/NugetIds.cmake -- EMGU_CUDART_NUGET_ID, guarded the same
    # as here; EMGU_CUDA_NVRTC_NUGET_ID, guarded by WITH_CUDNN there too) but
    # missing from this build-ordering list. Without an explicit dependency
    # edge here, nothing guarantees these two finish packing before a
    # downstream consumer's `dotnet restore` needs them -- reproduced
    # directly: Emgu.runtime.windows.cuda.rt's .nupkg happened to finish
    # early enough to not be noticed, but Emgu.runtime.windows.cuda.nvrtc's
    # took ~22 minutes longer than DebuggerVisualizer's restore step, which
    # failed with NU1101 "Unable to find package Emgu.runtime.windows.cuda.nvrtc"
    # even though the .nupkg existed on disk minutes later in the same run.
    IF(TARGET Emgu.runtime.windows.cuda.rt.nuget)
      ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.runtime.windows.cuda.rt.nuget)
    ENDIF()
    IF(WITH_CUDNN AND TARGET Emgu.runtime.windows.cuda.nvrtc.nuget)
      ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.runtime.windows.cuda.nvrtc.nuget)
    ENDIF()
  ENDIF()

  IF(TARGET Emgu.runtime.windows.msvc.rt.x64.nuget)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.runtime.windows.msvc.rt.x64.nuget)
  ENDIF()

  IF(TARGET Emgu.runtime.windows.msvc.rt.x86.nuget)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.runtime.windows.msvc.rt.x86.nuget)
  ENDIF()

  IF(TARGET Emgu.runtime.windows.msvc.rt.arm64.nuget)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.runtime.windows.msvc.rt.arm64.nuget)
  ENDIF()

#  IF(TARGET Emgu.CV.Platform.NetFramework)
#    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Platform.NetFramework)
#  ENDIF()

#  IF(TARGET Emgu.CV.Platform.NetCore)
#    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Platform.NetCore)
#  ENDIF()

ENDIF()
