# --------------------------------------------------------
#  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
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
  	ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.runtime.windows.cuda.cufft.nuget Emgu.CV.runtime.windows.cuda.dnn.nuget Emgu.CV.runtime.windows.cuda.npp.nuget)
  ENDIF()

  IF (TARGET Emgu.CV.Platform.NetFramework)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Platform.NetFramework)
  ENDIF()

  IF (TARGET Emgu.CV.Platform.NetCore)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Platform.NetCore)
  ENDIF()

ENDIF()
