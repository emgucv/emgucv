# --------------------------------------------------------
#  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF ((TARGET Emgu.CV.Platform.NetStandard) AND (HAVE_WINDESKTOP_X86 OR HAVE_WINDESKTOP_X64))
  PROJECT(Emgu.CV.runtime.windows.nuget)

  ADD_CUSTOM_TARGET(
    ${PROJECT_NAME} ALL
    COMMAND ${DOTNET_EXECUTABLE} pack -c ${DEFAULT_CS_CONFIG} "${CMAKE_CURRENT_SOURCE_DIR}/../../../Emgu.CV.Platform/NetStandard/Emgu.CV.Platform.NetStandard.csproj" -p:NuspecFile="${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec" -o "${CMAKE_CURRENT_SOURCE_DIR}/../"
    WORKING_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}"
    )
  ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.nuget cvextern)
  
  if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "nuget")
  endif() 
  
  IF (WIN32 AND HAVE_WINDESKTOP_X64 AND WITH_CUDA)
  	ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.runtime.windows.cuda.fft.nuget Emgu.runtime.windows.cuda.dnn.cnn.nuget Emgu.runtime.windows.cuda.dnn.ops.nuget Emgu.runtime.windows.cuda.dnn.adv.nuget Emgu.runtime.windows.cuda.blas.nuget Emgu.runtime.windows.cuda.npp.nuget)
  ENDIF()

  IF (HAVE_WINDESKTOP_X64)
  	ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.runtime.windows.msvc.rt.x64.nuget)
  ENDIF()

  IF (HAVE_WINDESKTOP_X86)
  	ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.runtime.windows.msvc.rt.x86.nuget)
  ENDIF()

  IF (TARGET Emgu.CV.Platform.NetFramework)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Platform.NetFramework)
  ENDIF()

  IF (TARGET Emgu.CV.Platform.NetCore)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Platform.NetCore)
  ENDIF()

ENDIF()
