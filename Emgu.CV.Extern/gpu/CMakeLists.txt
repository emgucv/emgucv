# --------------------------------------------------------
#  Copyright (C) 2004-2011 by EMGU. All rights reserved.
# 
#  CMake file for cvextern. See root CMakeLists.txt
#
# ----------------------------------------------------------------------------
project(cvextern_gpu)

#INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/core/include")
#INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/imgproc/include")
#INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/objdetect/include")
#INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/features2d/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../../opencv/modules/flann/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../../opencv/modules/gpu/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../../opencv/include/opencv")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../../opencv/modules/stitching/include")

############################### GPU START ##############################
IF(WIN32 AND WITH_CUDA)
SET(CUDA_NPP_INCLUDES ${CUDA_TOOLKIT_ROOT_DIR}/include)

if(EXISTS ${CUDA_NPP_INCLUDES}/nppversion.h)
    file( STRINGS ${CUDA_NPP_INCLUDES}/nppversion.h npp_major REGEX "#define NPP_VERSION_MAJOR.*")
    file( STRINGS ${CUDA_NPP_INCLUDES}/nppversion.h npp_minor REGEX "#define NPP_VERSION_MINOR.*")
    file( STRINGS ${CUDA_NPP_INCLUDES}/nppversion.h npp_build REGEX "#define NPP_VERSION_BUILD.*")

    string( REGEX REPLACE "#define NPP_VERSION_MAJOR[ \t]+|//.*" "" npp_major ${npp_major})
    string( REGEX REPLACE "#define NPP_VERSION_MINOR[ \t]+|//.*" "" npp_minor ${npp_minor})
    string( REGEX REPLACE "#define NPP_VERSION_BUILD[ \t]+|//.*" "" npp_build ${npp_build})

    string( REGEX MATCH "[0-9]+" npp_major ${npp_major} ) 
    string( REGEX MATCH "[0-9]+" npp_minor ${npp_minor} ) 
    string( REGEX MATCH "[0-9]+" npp_build ${npp_build} ) 	
endif()

SET(CUDA_NPP_LIBRARY_ROOT_DIR ${CUDA_TOOLKIT_ROOT_DIR})
#replace any potential backslash in the path with slash
#STRING(REGEX REPLACE "\\\\" "/" CUDA_NPP_LIBRARY_ROOT_DIR ${CUDA_NPP_LIBRARY_ROOT_DIR}) 

IF(TARGET_ARCH_64) 
  SET(CUDA_POSTFIX 64)
else()
  SET(CUDA_POSTFIX 32)
ENDIF()

LIST(APPEND CVEXTERN_GPU_DEPENDENCY_DLLS 
  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/npp${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build}.dll"
  "${CUDA_SDK_ROOT_DIR}/bin/cudart${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build}.dll"
  )
LIST(APPEND CVEXTERN_GPU_DEPENDENCY_DLL_NAMES 
  npp${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build} 
  cudart${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build})

IF(WITH_CUFFT)
  LIST(APPEND CVEXTERN_GPU_DEPENDENCY_DLLS "${CUDA_SDK_ROOT_DIR}/bin/cufft${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build}.dll")
  LIST(APPEND CVEXTERN_GPU_DEPENDENCY_DLL_NAMES cufft${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build}) 
ENDIF()

IF(WITH_CUBLAS)
  LIST(APPEND CVEXTERN_GPU_DEPENDENCY_DLLS "${CUDA_SDK_ROOT_DIR}/bin/cublas${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build}.dll")
  LIST(APPEND CVEXTERN_GPU_DEPENDENCY_DLL_NAMES cublas${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build})
ENDIF()

ENDIF()
############################### GPU END ################################

IF(WITH_CUDA AND DEFINED CVEXTERN_GPU_DEPENDENCY_DLLS)
  INSTALL(
    FILES 
    ${CVEXTERN_GPU_DEPENDENCY_DLLS}
    DESTINATION bin
    COMPONENT main)
ENDIF()

file(GLOB gpu_extern_srcs "./*.cpp")
file(GLOB gpu_extern_hdrs "./*.h*")

source_group("Src" FILES ${gpu_extern_srcs})
source_group("Include" FILES ${gpu_extern_hdrs})
set(the_target ${PROJECT_NAME})
add_library(${the_target} ${gpu_extern_srcs} ${gpu_extern_hdrs})

IF(WITH_CUDA AND DEFINED CVEXTERN_GPU_DEPENDENCY_DLLS)
  FOREACH(CVEXTERN_GPU_DEPENDENCY_DLL ${CVEXTERN_GPU_DEPENDENCY_DLLS})
    LIST(APPEND CVEXTERN_GPU_DEPENDENCY_DLL_DEPLOY_COMMAND COMMAND ${CMAKE_COMMAND} -E copy "${CVEXTERN_GPU_DEPENDENCY_DLL}" ${LIBRARY_OUTPUT_PATH})
  ENDFOREACH()
  #Promote this to parent scope such that cpack will know what dlls to be included in the package
  SET(CVEXTERN_GPU_DEPENDENCY_DLL_NAMES ${CVEXTERN_GPU_DEPENDENCY_DLL_NAMES} PARENT_SCOPE)

  ADD_CUSTOM_COMMAND(
    TARGET ${the_target}
    POST_BUILD
    ${CVEXTERN_GPU_DEPENDENCY_DLL_DEPLOY_COMMAND}
    COMMENT "Copying ${CVEXTERN_GPU_DEPENDENCY_DLLS} to ${LIBRARY_OUTPUT_PATH}")
ENDIF()

# For dynamic link numbering convenions
set_target_properties(${the_target} PROPERTIES
    OUTPUT_NAME "${the_target}"
    )

if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${the_target} PROPERTIES FOLDER "emgu")
endif()

# Additional target properties
set_target_properties(${the_target} PROPERTIES
	DEBUG_POSTFIX "${OPENCV_DEBUG_POSTFIX}"
	ARCHIVE_OUTPUT_DIRECTORY "${CMAKE_BINARY_DIR}/lib/"
	RUNTIME_OUTPUT_DIRECTORY "${CMAKE_BINARY_DIR}/bin/"
	DEFINE_SYMBOL "CVAPI_EXPORTS"
	)

ADD_DEPENDENCIES(${the_target} 
  opencv_core opencv_features2d opencv_imgproc opencv_objdetect opencv_gpu opencv_stitching)

#disable warnings
IF(MSVC)
  ADD_DEFINITIONS(-wd4251 -D_CRT_SECURE_NO_WARNINGS)
ENDIF()

# Add the required libraries for linking:
target_link_libraries(${the_target} ${OPENCV_LINKER_LIBS} 
  opencv_core opencv_features2d opencv_imgproc opencv_objdetect opencv_gpu opencv_stitching)
  
INSTALL(TARGETS ${the_target} 
    RUNTIME DESTINATION bin COMPONENT main
    LIBRARY DESTINATION lib COMPONENT main
    ARCHIVE DESTINATION lib COMPONENT main)
