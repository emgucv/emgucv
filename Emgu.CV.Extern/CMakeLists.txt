# --------------------------------------------------------
#  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.
# 
#  CMake file for cvextern. See root CMakeLists.txt
#
# ----------------------------------------------------------------------------
project(cvextern)

INCLUDE_DIRECTORIES("${OPENCV_SUBFOLDER}/include/opencv")
INCLUDE_DIRECTORIES("${OPENCV_SUBFOLDER}/include")

MACRO(CREATE_VECTOR_CS vname velement velement_cs element_type cs_source_folder namespace_cs)
   SET(VECTOR_NAME ${vname})
   SET(VECTOR_ELEMENT ${velement})
   SET(VECTOR_ELEMENT_CS ${velement_cs})
   SET(NAMESPACE_CS ${namespace_cs})
   SET(IS_VECTOR_OF_VECTOR false)

   SET(VECTOR_ADDITIONAL_INCLUDE "")
   SET(VECTOR_ADDITIONAL_CODE "")

   SET(extra_macro_args ${ARGN})
   # Did we get any optional args?
   LIST(LENGTH extra_macro_args num_extra_args)
   IF (${num_extra_args} GREATER 0)
      list(GET extra_macro_args 0 additional_c_header)
      SET(VECTOR_ADDITIONAL_INCLUDE ${additional_c_header})
   ENDIF()
   IF (${num_extra_args} GREATER 1)
      list(GET extra_macro_args 1 additional_c_code)
      SET(VECTOR_ADDITIONAL_CODE ${additional_c_code})
   ENDIF()

   if (${element_type} STREQUAL "struct")
     CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/cmake/vectorOfStruct_c.h.in ${CMAKE_CURRENT_SOURCE_DIR}/vector_${VECTOR_NAME}.h)
     CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/cmake/vectorOfStruct_c.cpp.in ${CMAKE_CURRENT_SOURCE_DIR}/vector_${VECTOR_NAME}.cpp)
     CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/cmake/VectorOfStruct.cs.in ${cs_source_folder}/VectorOf${VECTOR_NAME}.cs)
   ELSEIF(${element_type} STREQUAL "vector")
     SET(VECTOR_ELEMENT_CS ${vname})
     SET(ELEMENT_OF_ELEMENT ${velement_cs})
     SET(IS_VECTOR_OF_VECTOR true)
     CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/cmake/vectorOfObject_c.h.in ${CMAKE_CURRENT_SOURCE_DIR}/vector_${VECTOR_NAME}.h)
     CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/cmake/vectorOfObject_c.cpp.in ${CMAKE_CURRENT_SOURCE_DIR}/vector_${VECTOR_NAME}.cpp)
     CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/cmake/VectorOfObject.cs.in ${cs_source_folder}/VectorOf${VECTOR_NAME}.cs)
     #CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/cmake/VectorOfVector.cs.in ${cs_source_folder}/VectorOf${VECTOR_NAME}Extra.cs)
   ELSE()
     CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/cmake/vectorOfObject_c.h.in ${CMAKE_CURRENT_SOURCE_DIR}/vector_${VECTOR_NAME}.h)
     CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/cmake/vectorOfObject_c.cpp.in ${CMAKE_CURRENT_SOURCE_DIR}/vector_${VECTOR_NAME}.cpp)
     CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/cmake/VectorOfObject.cs.in ${cs_source_folder}/VectorOf${VECTOR_NAME}.cs)
   ENDIF()
ENDMACRO()

CREATE_VECTOR_CS("Byte" "unsigned char" "byte" "struct" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util Emgu.CV.Util)
CREATE_VECTOR_CS("Int" "int" "int" "struct" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util Emgu.CV.Util)
CREATE_VECTOR_CS("Float" "float" "float" "struct" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util Emgu.CV.Util)
CREATE_VECTOR_CS("Double" "double" "double" "struct" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util Emgu.CV.Util)
CREATE_VECTOR_CS("Point" "cv::Point" "Point" "struct" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util Emgu.CV.Util)
CREATE_VECTOR_CS("PointF" "cv::Point2f" "PointF" "struct" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util Emgu.CV.Util)
CREATE_VECTOR_CS("Point3D32F" "cv::Point3f" "MCvPoint3D32f" "struct" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util Emgu.CV.Util)
CREATE_VECTOR_CS("Rect" "cv::Rect" "Rectangle" "struct" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util Emgu.CV.Util)
CREATE_VECTOR_CS("KeyPoint" "cv::KeyPoint" "MKeyPoint" "struct" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util Emgu.CV.Util)
CREATE_VECTOR_CS("ERStat" "cv::text::ERStat" "MCvERStat" "struct" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/Text Emgu.CV.Text "#include \"opencv2/text/erfilter.hpp\"")
CREATE_VECTOR_CS("DMatch" "cv::DMatch" "MDMatch" "struct" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util Emgu.CV.Util)
CREATE_VECTOR_CS("Triangle2DF" "cv::Vec6f" "Triangle2DF" "struct" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util Emgu.CV.Util)

CREATE_VECTOR_CS("Mat" "cv::Mat" "Mat" "object" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util Emgu.CV.Util)
CREATE_VECTOR_CS("UMat" "cv::UMat" "UMat" "object" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util Emgu.CV.Util)
CREATE_VECTOR_CS("CvString" "cv::String" "CvString" "object" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util Emgu.CV.Util)

CREATE_VECTOR_CS("VectorOfPoint" "std::vector< cv::Point >" "Point" "vector" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util Emgu.CV.Util)
CREATE_VECTOR_CS("VectorOfPointF" "std::vector< cv::Point2f >" "PointF" "vector" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util Emgu.CV.Util)
CREATE_VECTOR_CS("VectorOfPoint3D32F" "std::vector< cv::Point3f >" "MCvPoint3D32f" "vector" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util Emgu.CV.Util)
CREATE_VECTOR_CS("VectorOfInt" "std::vector< int >" "int" "vector" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util Emgu.CV.Util)
CREATE_VECTOR_CS("VectorOfDMatch" "std::vector< cv::DMatch >" "MDMatch" "vector" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util Emgu.CV.Util)

CREATE_VECTOR_CS("OclPlatformInfo" "cv::ocl::PlatformInfo" "OclPlatformInfo" "object" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util Emgu.CV.Util "#include \"opencv2/core/ocl.hpp\"")
IF(NOT IOS)
CREATE_VECTOR_CS("GpuMat" "cv::cuda::GpuMat" "GpuMat" "object" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Cuda Emgu.CV.Cuda "#include \"opencv2/core/cuda.hpp\"")
ENDIF()

SET(OPENCV_MODULES_DIR "${OPENCV_SUBFOLDER}/modules")
INCLUDE_DIRECTORIES("${CMAKE_BINARY_DIR}")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}")

file(GLOB extern_srcs "${PROJECT_SOURCE_DIR}/*.cpp")
file(GLOB extern_hdrs "${PROJECT_SOURCE_DIR}/*.h*")
source_group("Src_extern" FILES ${extern_srcs})
source_group("Include_extern" FILES ${extern_hdrs})

IF(IOS)
  LIST(APPEND extern_srcs "ios/divmodsi4.c")
ENDIF()

FOREACH(CVEXTERN_OPTIONAL_DEP ${OPENCV_MODULE_NAMES})

  #MESSAGE("Retriveing source folder for project :  opencv_${CVEXTERN_OPTIONAL_DEP}")
  #GET_TARGET_PROPERTY(OPENCV_MODULE_SOURCE_DIR opencv_${CVEXTERN_OPTIONAL_DEP} PROJECT_SOURCE_DIR)
  INCLUDE_DIRECTORIES("${opencv_${CVEXTERN_OPTIONAL_DEP}_SOURCE_DIR}/include")
  #MESSAGE("Included:  ${opencv_${CVEXTERN_OPTIONAL_DEP}_SOURCE_DIR}/include")

  SET(CVEXTERN_OPTIONAL_DIR "${PROJECT_SOURCE_DIR}/${CVEXTERN_OPTIONAL_DEP}")
  IF(EXISTS "${CVEXTERN_OPTIONAL_DIR}" AND IS_DIRECTORY "${CVEXTERN_OPTIONAL_DIR}")
    
    INCLUDE_DIRECTORIES("${CVEXTERN_OPTIONAL_DIR}")
    file(GLOB_RECURSE ${CVEXTERN_OPTIONAL_DEP}_extern_srcs "${CVEXTERN_OPTIONAL_DEP}/*.cpp")
	 #MESSAGE("srcs: ${${CVEXTERN_OPTIONAL_DEP}_extern_srcs}")
    file(GLOB_RECURSE ${CVEXTERN_OPTIONAL_DEP}_extern_hdrs "${CVEXTERN_OPTIONAL_DEP}/*.h*")
    source_group("Src_${CVEXTERN_OPTIONAL_DEP}" FILES ${${CVEXTERN_OPTIONAL_DEP}_extern_srcs})
    source_group("Include_${CVEXTERN_OPTIONAL_DEP}" FILES ${${CVEXTERN_OPTIONAL_DEP}_extern_hdrs})
    LIST(APPEND extern_srcs ${${CVEXTERN_OPTIONAL_DEP}_extern_srcs})
    LIST(APPEND extern_hdrs ${${CVEXTERN_OPTIONAL_DEP}_extern_hdrs})
  ENDIF()
ENDFOREACH()

IF(WIN32 AND CV_ICC)
  STRING(REGEX REPLACE "/Qipo" "" CMAKE_C_FLAGS_RELEASE ${CMAKE_C_FLAGS_RELEASE})
  STRING(REGEX REPLACE "/Qipo" "" CMAKE_CXX_FLAGS_RELEASE ${CMAKE_CXX_FLAGS_RELEASE})
ENDIF()

############################### IPP START ##############################
#IF(WITH_IPP)
#  include(${OPENCV_SUBFOLDER}/cmake/OpenCVFindIPP.cmake)
#  IF (IPP_FOUND)
#    message(STATUS "CVEXTERN: USING IPP: ${IPP_LIBRARY_DIRS} ")
#    add_definitions(-DHAVE_IPP)
#    include_directories(${IPP_INCLUDE_DIRS})
#    LINK_DIRECTORIES(${IPP_LIBRARY_DIRS})
#  ENDIF()
#ENDIF()
############################### IPP END ################################

############################### TBB START ##############################
if (WITH_TBB AND TBB_INCLUDE_DIRS AND TBB_LIB_DIR)
  MESSAGE(STATUS "CVEXTERN: using TBB")
  add_definitions(-DHAVE_TBB)
  include_directories(${TBB_INCLUDE_DIRS})
  link_directories(${TBB_LIB_DIR})
  
  IF(WIN32)
  SET(TBB_BINARY_FILE_DIR "${TBB_INCLUDE_DIRS}/../../redist")
  IF(TARGET_ARCH_64)
    SET(TBB_BINARY_FILE_DIR "${TBB_BINARY_FILE_DIR}/intel64/tbb")
  ELSE()
    SET(TBB_BINARY_FILE_DIR "${TBB_BINARY_FILE_DIR}/ia32/tbb")
  ENDIF()
  if (MSVC80)
	SET(TBB_BINARY_FILE_DIR "${TBB_BINARY_FILE_DIR}/vc8")
  elseif(MSVC90)
	SET(TBB_BINARY_FILE_DIR "${TBB_BINARY_FILE_DIR}/vc9")
  elseif(MSVC10)
	SET(TBB_BINARY_FILE_DIR "${TBB_BINARY_FILE_DIR}/vc10")
  elseif(MSVC11)
	SET(TBB_BINARY_FILE_DIR "${TBB_BINARY_FILE_DIR}/vc11")
  endif()

  #LIST(APPEND CVEXTERN_DEPENDENCY_DLL_NAMES tbb)
  LIST(APPEND CVEXTERN_DEPENDENCY_DLLS "${TBB_BINARY_FILE_DIR}/tbb.dll")
   
  ENDIF()
endif()

IF(WITH_TBB)
  IF (NOT TBB_INCLUDE_DIRS)
    MESSAGE(STATUS "Cannot find TBB_INCLUDE_DIRS")
  ELSEIF (NOT TBB_LIB_DIR)
    MESSAGE(STATUS "Cannot find TBB_LIB_DIR")
  ELSE()
    MESSAGE(STATUS "tbb dll: ${TBB_BINARY_FILE_DIR}/tbb.dll")
  ENDIF()
ENDIF()
############################### TBB END ################################

############################### INTEL ICL START ##############################
IF(WIN32 AND CV_ICC)
  IF(TARGET_ARCH_64)
    SET(INTEL_ICC_REDIST_PATH "$ENV{ICPP_COMPILER13}redist/intel64/compiler/")
  ELSE()
    SET(INTEL_ICC_REDIST_PATH "$ENV{ICPP_COMPILER13}redist/ia32/compiler/")
  ENDIF()
  STRING(REGEX REPLACE "\\\\" "/" INTEL_ICC_REDIST_PATH ${INTEL_ICC_REDIST_PATH})
  #LIST(APPEND CVEXTERN_DEPENDENCY_DLL_NAMES libomp5md libmmd svml_dispmd)
  LIST(APPEND CVEXTERN_DEPENDENCY_DLLS ${INTEL_ICC_REDIST_PATH}libiomp5md.dll ${INTEL_ICC_REDIST_PATH}libmmd.dll ${INTEL_ICC_REDIST_PATH}svml_dispmd.dll)
ENDIF()
############################### INTEL ICL START ##############################

############################### OPENNI START ##############################
IF(WIN32 AND WITH_OPENNI)
  IF(TARGET_ARCH_64)
    SET(OPENNI_BINARY_FILE "${OPENNI_LIB_DIR}/../Bin64/OpenNI64.dll")
  ELSE()
    SET(OPENNI_BINARY_FILE "${OPENNI_LIB_DIR}/../Bin/OpenNI.dll")
  ENDIF()
  
LIST(APPEND CVEXTERN_DEPENDENCY_DLLS ${OPENNI_BINARY_FILE})   
ENDIF()
############################### OPENNI END ################################

############################### TESSERACT START ########################
SET(TESSERACT_OCR_ROOT_DIR "${PROJECT_SOURCE_DIR}/tesseract/libtesseract/tesseract-ocr.git")
IF(ANDROID)
  SET(EMGU_CV_WITH_TESSERACT ON CACHE BOOL "BUILD OCR")
  IF(EMGU_CV_WITH_TESSERACT)
    ADD_SUBDIRECTORY(tesseract/libtesseract)
    IF(TESSERACT_FOUND)
    ELSE()
      SET(TESSERACT_INCLUDE_DIRS "${TESSERACT_OCR_ROOT_DIR}/ccutil" "${TESSERACT_OCR_ROOT_DIR}/api" "${TESSERACT_OCR_ROOT_DIR}/ccmain" "${TESSERACT_OCR_ROOT_DIR}/ccstruct" "${PROJECT_SOURCE_DIR}/tesseract")
      #SET(TESSERACT_PROJECTS libleptonica tesseract_ccutil tesseract_viewer tesseract_cutil tesseract_opencl tesseract_ccstruct tesseract_dict tesseract_classify tesseract_wordrec tesseract_neural_networks tesseract_textord tesseract_cube tesseract_api tesseract_ccmain)
	  SET(TESSERACT_PROJECTS tesseract_api tesseract_ccmain tesseract_cube tesseract_textord tesseract_neural_networks tesseract_wordrec tesseract_classify tesseract_dict tesseract_ccstruct tesseract_opencl tesseract_cutil tesseract_viewer tesseract_ccutil libleptonica)
    ENDIF()
  ENDIF()
ELSEIF(WIN32 OR APPLE)
  IF(NETFX_CORE)
    SET(EMGU_CV_WITH_TESSERACT OFF CACHE BOOL "BUILD OCR")
  ELSE()
    SET(EMGU_CV_WITH_TESSERACT ON CACHE BOOL "BUILD OCR")
  ENDIF()
  IF(EMGU_CV_WITH_TESSERACT)
    ADD_SUBDIRECTORY(tesseract/libtesseract)
    IF(TESSERACT_FOUND)
    ELSE()
      SET(TESSERACT_INCLUDE_DIRS "${TESSERACT_OCR_ROOT_DIR}/ccutil" "${TESSERACT_OCR_ROOT_DIR}/api" "${TESSERACT_OCR_ROOT_DIR}/ccmain" "${TESSERACT_OCR_ROOT_DIR}/ccstruct" "${PROJECT_SOURCE_DIR}/tesseract")
      #SET(TESSERACT_PROJECTS libleptonica tesseract_ccutil tesseract_viewer tesseract_cutil tesseract_opencl tesseract_ccstruct tesseract_dict tesseract_classify tesseract_wordrec tesseract_neural_networks tesseract_textord tesseract_cube tesseract_api tesseract_ccmain)
	  SET(TESSERACT_PROJECTS tesseract_api tesseract_ccmain tesseract_cube tesseract_textord tesseract_neural_networks tesseract_wordrec tesseract_classify tesseract_dict tesseract_ccstruct tesseract_opencl tesseract_cutil tesseract_viewer tesseract_ccutil libleptonica)
    ENDIF()
    ADD_DEFINITIONS(-D__MSW32__)
  ENDIF()
ELSE()
  SET(CMAKE_MODULE_PATH "${PROJECT_SOURCE_DIR}/../cmake/modules")
  FIND_PACKAGE(TESSERACT)
  IF(TESSERACT_FOUND)
    SET(EMGU_CV_WITH_TESSERACT ON CACHE BOOL "BUILD OCR")
  ELSE()
    SET(EMGU_CV_WITH_TESSERACT OFF CACHE BOOL "BUILD OCR")
    SET(TESSERACT_INCLUDE_DIRS "")
    SET(TESSERACT_PROJECTS "")
  ENDIF()
ENDIF()

IF(EMGU_CV_WITH_TESSERACT)
  INCLUDE_DIRECTORIES(${TESSERACT_INCLUDE_DIRS})
  LIST(APPEND extern_hdrs "${PROJECT_SOURCE_DIR}/tesseract/tesseract_c.h")
  LIST(APPEND extern_srcs "${PROJECT_SOURCE_DIR}/tesseract/tesseract.cpp")
ENDIF()
############################### TESSERACT END ##########################

############################### CVBLOB START ########################
SET(EMGU_CV_WITH_CVBLOB ON CACHE BOOL "BUILD with CVBLOB")
MESSAGE(STATUS "WITH CVBLOB: ${EMGU_CV_WITH_CVBLOB}")
IF(EMGU_CV_WITH_CVBLOB)
  
  ADD_SUBDIRECTORY(cvblob)
  INCLUDE_DIRECTORIES(${PROJECT_SOURCE_DIR}/cvblob/libcvblob/cvBlob)
  SET(CVBLOB_LIBRARY cvblob)
ELSE()
  FILE(GLOB extern_srcs_excludes "${PROJECT_SOURCE_DIR}/cvblob_c.cpp")
  LIST(REMOVE_ITEM extern_srcs ${extern_srcs_excludes})
  FILE(GLOB extern_hdrs_excludes "${PROJECT_SOURCE_DIR}/cvblob_c.h")
  LIST(REMOVE_ITEM extern_hdrs ${extern_hdrs_excludes})
ENDIF()
############################### CVBLOB END ##########################

############################### GPU START ##############################
IF(WIN32 AND WITH_CUDA)
  SET(CUDA_NPP_INCLUDES ${CUDA_TOOLKIT_ROOT_DIR}/include)
  #MESSAGE("npp includes: ${CUDA_NPP_INCLUDES}")
  if(EXISTS ${CUDA_NPP_INCLUDES}/nppversion.h)
    #MESSAGE("npp version header: ${CUDA_NPP_INCLUDES}/nppversion.h")
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

  IF ((${npp_major} GREATER 5) OR ((${npp_major} STREQUAL "5") AND (${npp_minor} GREATER 0)))
  LIST(APPEND CVEXTERN_DEPENDENCY_DLLS 
    "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/nppc${CUDA_POSTFIX}_${npp_major}${npp_minor}.dll"
    "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/nppi${CUDA_POSTFIX}_${npp_major}${npp_minor}.dll"
    "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/npps${CUDA_POSTFIX}_${npp_major}${npp_minor}.dll"
    "${CUDA_TOOLKIT_ROOT_DIR}/bin/cudart${CUDA_POSTFIX}_${npp_major}${npp_minor}.dll"
    )
  #LIST(APPEND CVEXTERN_DEPENDENCY_DLL_NAMES 
  #  npp${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build} 
  #  cudart${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build})

  IF(WITH_CUFFT)
    LIST(APPEND CVEXTERN_DEPENDENCY_DLLS "${CUDA_TOOLKIT_ROOT_DIR}/bin/cufft${CUDA_POSTFIX}_${npp_major}${npp_minor}.dll")
    #LIST(APPEND CVEXTERN_DEPENDENCY_DLL_NAMES cufft${CUDA_POSTFIX}_${npp_major}${npp_minor}) 
  ENDIF()

  IF(WITH_CUBLAS)
    LIST(APPEND CVEXTERN_DEPENDENCY_DLLS "${CUDA_TOOLKIT_ROOT_DIR}/bin/cublas${CUDA_POSTFIX}_${npp_major}${npp_minor}.dll")
    #LIST(APPEND CVEXTERN_DEPENDENCY_DLL_NAMES cublas${CUDA_POSTFIX}_${npp_major}${npp_minor})
  ENDIF()  
  ELSE()  
  LIST(APPEND CVEXTERN_DEPENDENCY_DLLS 
    "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/npp${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build}.dll"
    "${CUDA_TOOLKIT_ROOT_DIR}/bin/cudart${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build}.dll"
    )
  #LIST(APPEND CVEXTERN_DEPENDENCY_DLL_NAMES 
  #  npp${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build} 
  #  cudart${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build})

  IF(WITH_CUFFT)
    LIST(APPEND CVEXTERN_DEPENDENCY_DLLS "${CUDA_TOOLKIT_ROOT_DIR}/bin/cufft${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build}.dll")
    #LIST(APPEND CVEXTERN_DEPENDENCY_DLL_NAMES cufft${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build}) 
  ENDIF()

  IF(WITH_CUBLAS)
    LIST(APPEND CVEXTERN_DEPENDENCY_DLLS "${CUDA_TOOLKIT_ROOT_DIR}/bin/cublas${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build}.dll")
    #LIST(APPEND CVEXTERN_DEPENDENCY_DLL_NAMES cublas${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build})
  ENDIF()
  ENDIF()
ENDIF()

#IF(ANDROID OR IOS)
#  file(GLOB_RECURSE gpu_extern_srcs "gpu/stitching_c.cpp" "gpu/videostab_c.cpp")
#  file(GLOB_RECURSE gpu_extern_hdrs "gpu/stitching_c.h" "gpu/videostab_c.h")
#ELSE()
#  file(GLOB_RECURSE gpu_extern_srcs "gpu/*.cpp")
#  file(GLOB_RECURSE gpu_extern_hdrs "gpu/*.h*")
#ENDIF()
############################### GPU END ################################

############################### OpenCL START ##############################
IF(WITH_OPENCL)
  MESSAGE(STATUS "Building cvextern with OPENCL")

  IF(WIN32 AND WITH_OPENCL AND NOT (NETFX_CORE))
    SET(OPENCL_PROJ OpenCL)
  ENDIF()
#  IF(WIN32)
#    IF(TARGET_ARCH_64) 
#      MESSAGE("Including OpenCL dll: ${PROJECT_SOURCE_DIR}/../lib/3rdParty/x64/OpenCL.dll")
#      LIST(APPEND CVEXTERN_DEPENDENCY_DLLS "${PROJECT_SOURCE_DIR}/../lib/3rdParty/x64/OpenCL.dll")
#    else()
#      MESSAGE("Including OpenCL dll: ${PROJECT_SOURCE_DIR}/../lib/3rdParty/x64/OpenCL.dll")
#      LIST(APPEND CVEXTERN_DEPENDENCY_DLLS "${PROJECT_SOURCE_DIR}/../lib/3rdParty/x64/OpenCL.dll")
#    ENDIF()
#  ENDIF()
ENDIF()
############################### OpenCL END ################################

############################### xfeatures2d START ##############################
IF(BUILD_opencv_xfeatures2d)
  IF( (NOT WITH_CUDA) OR (NOT (TARGET opencv_cuda)) )
    FILE(GLOB xfeatures2d_srcs_excludes "xfeatures2d/nonfree_gpu_c.cpp")
    LIST(REMOVE_ITEM extern_srcs ${xfeatures2d_srcs_excludes})
    FILE(GLOB xfeatures2d_hdrs_excludes "xfeatures2d/nonfree_gpu_c.h")
    LIST(REMOVE_ITEM extern_hdrs ${xfeatures2d_hdrs_excludes})
  ENDIF()
ENDIF()
############################### xfeatures2d END ################################

IF(DEFINED CVEXTERN_DEPENDENCY_DLLS)
  INSTALL(
    FILES 
    ${CVEXTERN_DEPENDENCY_DLLS}
    DESTINATION bin${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}
    COMPONENT main)
ENDIF()

IF(ANDROID OR APPLE)
  FILE(GLOB extern_srcs_excludes "${PROJECT_SOURCE_DIR}/tiffio.cpp")
  LIST(REMOVE_ITEM extern_srcs ${extern_srcs_excludes})
  #MESSAGE(STATUS "extern_srcs: ${extern_srcs}")
  FILE(GLOB extern_hdrs_excludes "${PROJECT_SOURCE_DIR}/tiffio_c.h")
  LIST(REMOVE_ITEM extern_hdrs ${extern_hdrs_excludes})
ENDIF()


#source_group("Src_cuda" FILES ${gpu_extern_srcs})
#source_group("Include_cuda" FILES ${gpu_extern_hdrs})
#source_group("Src_nonfree" FILES ${nonfree_srcs})
#source_group("Include_nonfree" FILES ${nonfree_extern_hdrs})

set(the_target ${PROJECT_NAME})
add_library(${the_target} ${extern_srcs} ${extern_hdrs} )

IF(DEFINED CVEXTERN_DEPENDENCY_DLLS)
  FOREACH(CVEXTERN_DEPENDENCY_DLL ${CVEXTERN_DEPENDENCY_DLLS})
    LIST(APPEND CVEXTERN_DEPENDENCY_DLL_DEPLOY_COMMAND COMMAND ${CMAKE_COMMAND} -E copy "${CVEXTERN_DEPENDENCY_DLL}" "${CMAKE_RUNTIME_OUTPUT_DIRECTORY}")
	GET_FILENAME_COMPONENT(CVEXTERN_DEPENDENCT_DLL_NAME ${CVEXTERN_DEPENDENCY_DLL} NAME_WE)
	LIST(APPEND CVEXTERN_DEPENDENCY_DLL_NAMES ${CVEXTERN_DEPENDENCT_DLL_NAME})
  ENDFOREACH()
  
  #Promote this to parent scope such that cpack will know what dlls to be included in the package
  #MESSAGE("**************CVEXTERN_DEPENDENCY_DLL_NAMES: ${CVEXTERN_DEPENDENCY_DLL_NAMES}")
  SET(CVEXTERN_DEPENDENCY_DLL_NAMES ${CVEXTERN_DEPENDENCY_DLL_NAMES} PARENT_SCOPE)

  ADD_CUSTOM_COMMAND(
    TARGET ${the_target}
    POST_BUILD
    ${CVEXTERN_DEPENDENCY_DLL_DEPLOY_COMMAND}
    COMMENT "Copying ${CVEXTERN_DEPENDENCY_DLLS} to ${CMAKE_RUNTIME_OUTPUT_DIRECTORY}")
ENDIF()

# For dynamic link numbering convenions
set_target_properties(${the_target} PROPERTIES
    OUTPUT_NAME "${the_target}"
    )

# Additional target properties
set_target_properties(${the_target} PROPERTIES
	DEBUG_POSTFIX "${OPENCV_DEBUG_POSTFIX}"
	ARCHIVE_OUTPUT_DIRECTORY "${CMAKE_BINARY_DIR}/bin${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}"
	RUNTIME_OUTPUT_DIRECTORY "${CMAKE_BINARY_DIR}/bin${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}"
	LIBRARY_OUTPUT_DIRECTORY "${CMAKE_BINARY_DIR}/bin${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}"
	DEFINE_SYMBOL "CVAPI_EXPORTS"
	)

if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${the_target} PROPERTIES FOLDER "emgu")
endif() 

IF (WITH_TIFF)
  FIND_PACKAGE(TIFF)
  IF(NOT TIFF_FOUND)
    ADD_DEPENDENCIES(${the_target} libtiff)
    SET(TIFF_INCLUDE_DIR "${OPENCV_SUBFOLDER}/3rdparty/libtiff")
    SET(TIFF_LIBRARY "libtiff")
  ENDIF()
  INCLUDE_DIRECTORIES(${TIFF_INCLUDE_DIR})
ELSE()
  SET(TIFF_LIBRARY "")
ENDIF()

FIND_PACKAGE(ZLIB)
IF(ZLIB_FOUND)
  MESSAGE(STATUS "CVEXTERN: ZLIB found.")
ENDIF()
IF(NOT ZLIB_FOUND)
  MESSAGE(STATUS "CVEXTERN: ZLIB not found, building from source")
  ADD_DEPENDENCIES(${the_target} zlib)
  SET(ZLIB_INCLUDE_DIR "${OPENCV_SUBFOLDER}/3rdparty/zlib")
  SET(ZLIB_LIBRARY "zlib")
ENDIF()
INCLUDE_DIRECTORIES(${ZLIB_INCLUDE_DIR})

IF (ANDROID OR APPLE)
  SET(EMGU_CV_WITH_TIFF OFF CACHE BOOL "BUILD TIFF wrapper")
  MESSAGE(STATUS "Skipping tiff")
  SET(GEOTIFF_LIBRARY "")
ELSE()
  SET(EMGU_CV_WITH_TIFF ON CACHE BOOL "BUILD TIFF wrapper")
ENDIF()

IF(EMGU_CV_WITH_TIFF)
  SET(CMAKE_MODULE_PATH "${PROJECT_SOURCE_DIR}/libgeotiff/cmake")
  FIND_PACKAGE(GeoTIFF)
  IF(GEOTIFF_FOUND)
    MESSAGE(STATUS "CVEXTERN: GEOTIFF found. INCLUDE DIR: ${GEOTIFF_INCLUDE_DIR}")
  ENDIF()
  IF (NOT GEOTIFF_FOUND)
    MESSAGE(STATUS "CVEXTERN: GEOTIFF not found. Building from source")
    # ----------------------------------------------------------------------------
    #  Build libgeotiff 
    # ----------------------------------------------------------------------------
    SET(LIBGEOTIFF_DIR "${CMAKE_CURRENT_SOURCE_DIR}/libgeotiff/libgeotiff-1.4.0")
    ADD_SUBDIRECTORY(libgeotiff)
	#MESSAGE(STATUS "CURRENT Binary DIR: ${CMAKE_CURRENT_BINARY_DIR}")
    SET(GEOTIFF_INCLUDE_DIR ${TIFF_INCLUDE_DIR} ${LIBGEOTIFF_DIR} "${LIBGEOTIFF_DIR}/libxtiff")
    SET(GEOTIFF_LIBRARY geotiff_archive xtiff)
  ENDIF()
  ADD_DEFINITIONS(-DEMGU_CV_WITH_TIFF)
  INCLUDE_DIRECTORIES(${GEOTIFF_INCLUDE_DIR})
ENDIF()

ADD_DEPENDENCIES(${the_target} 
  ${OPENCL_PROJ}
  ${ZLIB_LIBRARY} 
#  xtiff
  ${TIFF_LIBRARY} ${GEOTIFF_LIBRARY} ${CVBLOB_LIBRARY} )

IF(EMGU_CV_WITH_TESSERACT)
  IF(TESSERACT_FOUND)
    #MESSAGE("TESSERACT found, no need to add dependency")
    #ADD_DEPENDENCIES(${TESSERACT_LIBRARIES})
  ELSE()  
    #MESSAGE("TESSERACT not found, add dependency to ${TESSERACT_PROJECTS}")
    ADD_DEPENDENCIES(${TESSERACT_PROJECTS})
  ENDIF()
ENDIF()
  
FOREACH(CVEXTERN_OPTIONAL_DEP ${OPENCV_MODULE_NAMES})
#  INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/${CVEXTERN_OPTIONAL_DEP}/include")
#  MESSAGE("Included:  ${OPENCV_MODULES_DIR}/${CVEXTERN_OPTIONAL_DEP}/include")
  ADD_DEPENDENCIES(${the_target} opencv_${CVEXTERN_OPTIONAL_DEP})
  target_link_libraries(${the_target} opencv_${CVEXTERN_OPTIONAL_DEP})
ENDFOREACH()

IF(EMGU_ENABLE_SSE)
  ADD_DEFINITIONS(-D__EMGU_ENABLE_SSE__)
  IF(MSVC AND (NOT CV_ICC) AND (NOT TARGET_ARCH_64)) 
	SET(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /arch:SSE2")        ## Optimization
  ENDIF()
ENDIF()

IF(ANDROID)
  ADD_DEFINITIONS(-DANDROID)
ENDIF()

#disable warnings
IF(MSVC)
  ADD_DEFINITIONS(-wd4251 -D_CRT_SECURE_NO_WARNINGS)
ENDIF()

# Add the required libraries for linking:
target_link_libraries(${the_target} ${OPENCV_LINKER_LIBS} 
  ${ZLIB_LIBRARY} 
#  xtiff 
  ${CVBLOB_LIBRARY})

#IF(EMGU_CV_WITH_TESSERACT)
#  ADD_DEPENDENCIES(${TESSERACT_PROJECTS})
#ENDIF()
  
IF(DEFINED EMGUCV_PLATFORM_TOOLSET)
  set_target_properties(${the_target} PROPERTIES PLATFORM_TOOLSET ${EMGUCV_PLATFORM_TOOLSET})
ENDIF()

#IF(WITH_IPP)
#  target_link_libraries(${the_target} ippdc_l)
#ENDIF()  

IF(ANDROID)
  IF(EMGU_CV_WITH_TESSERACT)
    #NOT SURE WHY THE FOLLOWING IS NEEDED, BUT SEEMS TO MAKE THE PROBLEM GOES AWAY
    target_link_libraries(${the_target} ${TESSERACT_PROJECTS} tesseract_ccstruct tesseract_dict tesseract_classify tesseract_ccutil)
  ENDIF()
  INSTALL(TARGETS ${the_target} 
    LIBRARY DESTINATION bin${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT main)
ELSEIF(APPLE)
  IF(EMGU_CV_WITH_TESSERACT)
    IF(NOT TESSERACT_FOUND)
      target_link_libraries(${the_target} ${TESSERACT_PROJECTS})
    ENDIF()
  ENDIF()
  INSTALL(TARGETS ${the_target} 
    RUNTIME DESTINATION bin${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT main
    LIBRARY DESTINATION bin${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT main
    ARCHIVE DESTINATION bin${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT main)
ELSE()
  target_link_libraries(${the_target} ${TIFF_LIBRARY} ${GEOTIFF_LIBRARY})
  IF(EMGU_CV_WITH_TESSERACT)
    IF(NOT TESSERACT_FOUND)
      target_link_libraries(${the_target} ${TESSERACT_PROJECTS})
    ENDIF()
    IF(WIN32)
      #additional linkage required for tesseract built on windows
      target_link_libraries(${the_target} Ws2_32)
    ENDIF()
  ENDIF()
  INSTALL(TARGETS ${the_target} 
    RUNTIME DESTINATION bin${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT main
    LIBRARY DESTINATION bin${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT main
    ARCHIVE DESTINATION lib${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT main)
ENDIF()

#add_subdirectory(gpu)
