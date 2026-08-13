# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
#
#  Discovers and registers the third-party runtime DLLs/shared libraries
#  cvextern needs bundled at install time: ONNX Runtime, TBB, Intel ICL,
#  Windows system runtimes, Android libc++, OpenNI, Tesseract (built from
#  source subdirectory), VTK, OpenVINO/Inference Engine, FFmpeg, and the
#  CUDA/NPP/cuDNN/cuRAND/cuFFT/cuBLAS DLL set.
#  Included from Emgu.CV.Extern/CMakeLists.txt; CMAKE_CURRENT_SOURCE_DIR still
#  refers to Emgu.CV.Extern/ here (include() does not change it).
# ----------------------------------------------------------------------------

IF(WIN32 AND CV_ICC)
  STRING(REGEX REPLACE "/Qipo" "" CMAKE_C_FLAGS_RELEASE ${CMAKE_C_FLAGS_RELEASE})
  STRING(REGEX REPLACE "/Qipo" "" CMAKE_CXX_FLAGS_RELEASE ${CMAKE_CXX_FLAGS_RELEASE})
ENDIF()

############################### ONNX RUNTIME START ##############################
IF(HAVE_ONNXRUNTIME)
  MESSAGE(STATUS "CVEXTERN: using ONNX Runtime")
  add_definitions(-DHAVE_ONNXRUNTIME)
  IF(ONNX_INCLUDE_DIR)
    include_directories(${ONNX_INCLUDE_DIR})
  ENDIF()
ENDIF()
############################### ONNX RUNTIME END ################################

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
if (WITH_TBB AND TBB_ENV_INCLUDE AND TBB_ENV_LIB)
  MESSAGE(STATUS "CVEXTERN: using TBB")
  add_definitions(-DHAVE_TBB)
  include_directories(${TBB_ENV_INCLUDE})
  get_filename_component(TBB_ENV_LIB_DIRECTORY "${TBB_ENV_LIB}" DIRECTORY)
  link_directories(${TBB_ENV_LIB_DIRECTORY})
  
  IF(WIN32)
    SET(TBB_BINARY_FILE_DIR "${TBB_ENV_INCLUDE}/../../redist")
    IF(TARGET_ARCH_64)
      SET(TBB_BINARY_FILE_DIR "${TBB_BINARY_FILE_DIR}/intel64_win/tbb")
    ELSE()
      SET(TBB_BINARY_FILE_DIR "${TBB_BINARY_FILE_DIR}/ia32_win/tbb")
    ENDIF()
    
    if(MSVC10)
      SET(TBB_BINARY_FILE_DIR "${TBB_BINARY_FILE_DIR}/vc10")
    elseif(MSVC11)
      SET(TBB_BINARY_FILE_DIR "${TBB_BINARY_FILE_DIR}/vc11")
    elseif(MSVC12)
      SET(TBB_BINARY_FILE_DIR "${TBB_BINARY_FILE_DIR}/vc12")
    elseif(MSVC14)
      SET(TBB_BINARY_FILE_DIR "${TBB_BINARY_FILE_DIR}/vc14")
    endif()
    
    #LIST(APPEND CVEXTERN_DEPENDENCY_DLL_NAMES tbb)
    LIST(APPEND CVEXTERN_DEPENDENCY_DLLS "${TBB_BINARY_FILE_DIR}/tbb.dll")

  ENDIF()
endif()

IF(WITH_TBB)
  IF (NOT TBB_ENV_INCLUDE)
    MESSAGE(STATUS "Cannot find TBB_INCLUDE_DIRS")
  ELSEIF (NOT TBB_ENV_LIB)
    MESSAGE(STATUS "Cannot find TBB_LIB_DIR")
  ELSE()
    MESSAGE(STATUS "tbb dll: ${TBB_BINARY_FILE_DIR}/tbb.dll")
  ENDIF()
ENDIF()
############################### TBB END ################################

############################### INTEL ICL START ##############################
IF(WIN32 AND CV_ICC)
  IF(TARGET_ARCH_64)
    SET(INTEL_ICC_REDIST_PATH "$ENV{ICPP_COMPILER19}redist/intel64_win/compiler/")
  ELSE()
    SET(INTEL_ICC_REDIST_PATH "$ENV{ICPP_COMPILER19}redist/ia32_win/compiler/")
  ENDIF()
  #MESSAGE(STATUS "OOOOOOOOOOOOOOOOOOOOOOOOOOOO  INTEL_ICC_REDIST_PATH: ${INTEL_ICC_REDIST_PATH}")
  STRING(REGEX REPLACE "\\\\" "/" INTEL_ICC_REDIST_PATH ${INTEL_ICC_REDIST_PATH})
  #MESSAGE(STATUS "OOOOOOOOOOOOOOOOOOOOOOOOOOOO  INTEL_ICC_REDIST_PATH: ${INTEL_ICC_REDIST_PATH}")
  #STRING(REGEX REPLACE "\\" "/" INTEL_ICC_REDIST_PATH ${INTEL_ICC_REDIST_PATH})
  #MESSAGE(STATUS "OOOOOOOOOOOOOOOOOOOOOOOOOOOO  INTEL_ICC_REDIST_PATH: ${INTEL_ICC_REDIST_PATH}")
  #LIST(APPEND CVEXTERN_DEPENDENCY_DLL_NAMES libomp5md libmmd svml_dispmd)
  LIST(APPEND CVEXTERN_DEPENDENCY_DLLS ${INTEL_ICC_REDIST_PATH}libiomp5md.dll ${INTEL_ICC_REDIST_PATH}libmmd.dll ${INTEL_ICC_REDIST_PATH}svml_dispmd.dll)
ENDIF()
############################### INTEL ICL START ##############################

IF(WIN32 AND MSVC AND (NOT NETFX_CORE))
  # Add install rules for required system runtimes such as MSVCRxx.dll
  SET (CMAKE_INSTALL_SYSTEM_RUNTIME_LIBS_SKIP ON)
  INCLUDE(InstallRequiredSystemLibraries)
  LIST(APPEND CVEXTERN_DEPENDENCY_DLLS ${CMAKE_INSTALL_SYSTEM_RUNTIME_LIBS})   
ENDIF()

IF(ANDROID)
  GET_FILENAME_COMPONENT(ANDROID_NATIVE_LIBRARRY_PATH "${ZLIB_LIBRARY_RELEASE}" DIRECTORY)
  SET(CPP_SHARED_LIBRARY_RELEASE "${ANDROID_NATIVE_LIBRARRY_PATH}/../libc++_shared.so") 
  LIST(APPEND CVEXTERN_DEPENDENCY_DLLS ${CPP_SHARED_LIBRARY_RELEASE})   
ENDIF()


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
SET(LEPT_ROOT_DIR "${PROJECT_SOURCE_DIR}/tesseract/libtesseract/leptonica/leptonica.git")


  IF(EMGU_CV_WITH_TESSERACT)
    ADD_SUBDIRECTORY(tesseract/libtesseract)
    IF(TESSERACT_FOUND)
    ELSE()
      SET(TESSERACT_INCLUDE_DIRS 
        "${TESSERACT_OCR_ROOT_DIR}/include" 
        "${TESSERACT_OCR_ROOT_DIR}/src/ccutil" 
        "${TESSERACT_OCR_ROOT_DIR}/src/api" 
        "${TESSERACT_OCR_ROOT_DIR}/src/ccmain" 
        "${TESSERACT_OCR_ROOT_DIR}/src/ccstruct" 
        "${LEPT_ROOT_DIR}/src" 
        "${PROJECT_SOURCE_DIR}/tesseract")
      SET(TESSERACT_PROJECTS tesseract_api tesseract_ccmain tesseract_lstm tesseract_textord tesseract_arch tesseract_wordrec tesseract_classify tesseract_dict tesseract_ccstruct tesseract_cutil tesseract_viewer tesseract_ccutil libleptonica)
    ENDIF()
	
	IF (ANDROID)
	ELSE()
    ADD_DEFINITIONS(-D__MSW32__)
	ENDIF()
  ENDIF()



IF(EMGU_CV_WITH_TESSERACT)
  ADD_DEFINITIONS(-DHAVE_EMGUCV_TESSERACT)
  INCLUDE_DIRECTORIES(${TESSERACT_INCLUDE_DIRS})
ENDIF()

LIST(APPEND extern_hdrs "${PROJECT_SOURCE_DIR}/tesseract/tesseract_c.h")
LIST(APPEND extern_srcs "${PROJECT_SOURCE_DIR}/tesseract/tesseract.cpp")

############################### TESSERACT END ##########################


IF (WITH_VTK)
  IF(WIN32)
    CMAKE_PATH(SET VTK_DLL_DIRS NORMALIZE "${VTK_DIR}/bin/Release")
    FILE(GLOB VTK_DLL_NAMES "${VTK_DLL_DIRS}/vtk*.dll")
    MESSAGE(STATUS "VTK_DLL_NAMES: ${VTK_DLL_NAMES}")
    LIST(APPEND CVEXTERN_DEPENDENCY_DLLS ${VTK_DLL_NAMES})
  ENDIF()
ENDIF()


IF(WITH_OPENVINO)

  IF(WIN32)
    CMAKE_PATH(SET OPENVINO_RUNTIME_DIR NORMALIZE "${OpenVINO_DIR}/..")
    link_directories("${OPENVINO_RUNTIME_DIR}/lib/intel64")	
    SET(OPENVINO_DLL_DIRS "${OPENVINO_RUNTIME_DIR}/bin/intel64/Release")
    FILE(GLOB OPENVINO_DLL_NAMES "${OPENVINO_DLL_DIRS}/*.dll" "${OPENVINO_DLL_DIRS}/*.xml" "${OPENVINO_DLL_DIRS}/*.mvcmd" "${OPENVINO_DLL_DIRS}/*.json" "${OPENVINO_DLL_DIRS}/*.elf")
    MESSAGE(STATUS "OPENVINO_DLL_NAMES: ${OPENVINO_DLL_NAMES}")
    LIST(APPEND CVEXTERN_DEPENDENCY_DLLS ${OPENVINO_DLL_NAMES})
    
    SET(OPENVINO_TBB_DLL_DIRS "${OPENVINO_RUNTIME_DIR}/3rdparty/tbb/bin")
    FILE(GLOB OPENVINO_TBB_DLL_NAMES "${OPENVINO_TBB_DLL_DIRS}/*.dll")
	IF (NOT OPENVINO_TBB_DLL_NAMES)
		SET(OPENVINO_TBB_DLL_DIRS_OLD ${OPENVINO_TBB_DLL_DIRS})
		IF (TARGET_ARCH_64)
			#MESSAGE(STATUS "^^^^^^^^^^^^^^ MSVC_TOOLSET_VERSION: ${MSVC_TOOLSET_VERSION}")
			IF(("${MSVC_TOOLSET_VERSION}" STREQUAL "143") AND (EXISTS "${TBB_DIR}/../../../redist/intel64/vc14"))
				CMAKE_PATH(SET OPENVINO_TBB_DLL_DIRS NORMALIZE "${TBB_DIR}/../../../redist/intel64/vc14")
			ELSE()
				CMAKE_PATH(SET OPENVINO_TBB_DLL_DIRS NORMALIZE "${TBB_DIR}/../../../redist/intel64/vc_mt")
			ENDIF()
		ELSE()
			IF(("${MSVC_TOOLSET_VERSION}" STREQUAL "143") AND (EXISTS "${TBB_DIR}/../../../redist/ia32/vc14"))
				CMAKE_PATH(SET OPENVINO_TBB_DLL_DIRS NORMALIZE "${TBB_DIR}/../../../redist/ia32/vc14")
			ELSE()
				CMAKE_PATH(SET OPENVINO_TBB_DLL_DIRS NORMALIZE "${TBB_DIR}/../../../redist/ia32/vc_mt")
			ENDIF()
		ENDIF()
		MESSAGE(STATUS "Cannot find TBB dll in ${OPENVINO_TBB_DLL_DIRS_OLD}, trying to look into ${OPENVINO_TBB_DLL_DIRS}")
		FILE(GLOB OPENVINO_TBB_DLL_NAMES "${OPENVINO_TBB_DLL_DIRS}/*.dll")
	ENDIF()
	#FILE(TO_CMAKE_PATH "${OPENVINO_TBB_DLL_DIRS}" OPENVINO_TBB_DLL_DIRS)
	MESSAGE(STATUS "OPENVINO_TBB_DLL_DIRS: ${OPENVINO_TBB_DLL_DIRS}")
    FILE(GLOB OPENVINO_TBB_DEBUG_DLL_NAMES "${OPENVINO_TBB_DLL_DIRS}/*_debug.dll")
    MESSAGE(STATUS "OPENVINO_TBB_DEBUG_DLL_NAMES: ${OPENVINO_TBB_DEBUG_DLL_NAMES}")
    IF (OPENVINO_TBB_DEBUG_DLL_NAMES)
      LIST(REMOVE_ITEM OPENVINO_TBB_DLL_NAMES ${OPENVINO_TBB_DEBUG_DLL_NAMES})
    ENDIF()
	
    MESSAGE(STATUS "OPENVINO_TBB_DLL_NAMES: ${OPENVINO_TBB_DLL_NAMES}")
    LIST(APPEND CVEXTERN_DEPENDENCY_DLLS ${OPENVINO_TBB_DLL_NAMES})

    if(TARGET_ARCH_64)
      set(OPENVINO_SUFFIX _64)
    endif()
    set(OPENCV_OPENVINO_OUTPUT_NAME "opencv_dnn_openvino${OPENCV_VERSION_MAJOR}${OPENCV_VERSION_MINOR}${OPENCV_VERSION_PATCH}${OPENVINO_SUFFIX}")
    IF(MSVC_IDE)
	  SET(OPENCV_OPENVINO_OUTPUT_FILE_PATH "${EXECUTABLE_OUTPUT_PATH}/Release/${OPENCV_OPENVINO_OUTPUT_NAME}.dll")
    ELSEIF(MSVC AND (CMAKE_GENERATOR MATCHES "Visual"))
	  SET(OPENCV_OPENVINO_OUTPUT_FILE_PATH "${EXECUTABLE_OUTPUT_PATH}/${CMAKE_BUILD_TYPE}/${OPENCV_OPENVINO_OUTPUT_NAME}.dll")
    ENDIF()
    MESSAGE(STATUS "OPENVINO DNN plugin file path: ${OPENCV_OPENVINO_OUTPUT_FILE_PATH}")
    LIST(APPEND CVEXTERN_DEPENDENCY_DLLS "${OPENCV_OPENVINO_OUTPUT_FILE_PATH}")

  ELSEIF(APPLE)
    SET(OPENVINO_RUNTIME_DIR "${OpenVINO_DIR}/..")
    SET(OPENVINO_DYLIB_DIRS "${OPENVINO_RUNTIME_DIR}/lib/intel64/Release")
    FILE(GLOB OPENVINO_DYLIB_NAMES "${OPENVINO_DYLIB_DIRS}/*.dylib" "${OPENVINO_DYLIB_DIRS}/*.so" "${OPENVINO_DYLIB_DIRS}/*.xml" "${OPENVINO_DYLIB_DIRS}/*.mvcmd" "${OPENVINO_DYLIB_DIRS}/*.json" "${OPENVINO_DYLIB_DIRS}/*.elf")
    MESSAGE(STATUS "^^^^^^^^^^^^^^  OPENVINO_DYLIB_NAMES: ${OPENVINO_DYLIB_NAMES}")
    LIST(APPEND CVEXTERN_DEPENDENCY_DLLS ${OPENVINO_DYLIB_NAMES})

    SET(OPENVINO_TBB_DYLIB_DIRS "${OPENVINO_RUNTIME_DIR}/3rdparty/tbb/lib")
    MESSAGE(STATUS "^^^^^^^^^^^^^^  OPENVINO_TBB_DYLIB_DIRS: ${OPENVINO_TBB_DYLIB_DIRS}")
    FILE(GLOB OPENVINO_TBB_DYLIB_NAMES "${OPENVINO_TBB_DYLIB_DIRS}/*.dylib")
    FILE(GLOB OPENVINO_TBB_DEBUG_DYLIB_NAMES "${OPENVINO_TBB_DYLIB_DIRS}/*_debug.dylib")
    MESSAGE(STATUS "^^^^^^^^^^^^^^  OPENVINO_TBB_DEBUG_DYLIB_NAMES: ${OPENVINO_TBB_DEBUG_DYLIB_NAMES}")
    IF (OPENVINO_TBB_DEBUG_DYLIB_NAMES)
      LIST(REMOVE_ITEM OPENVINO_TBB_DYLIB_NAMES ${OPENVINO_TBB_DEBUG_DYLIB_NAMES})
    ENDIF()
    MESSAGE(STATUS "^^^^^^^^^^^^^^  OPENVINO_TBB_DYLIB_NAMES: ${OPENVINO_TBB_DYLIB_NAMES}")
    LIST(APPEND CVEXTERN_DEPENDENCY_DLLS ${OPENVINO_TBB_DYLIB_NAMES})
  ENDIF()
ELSEIF(WITH_INF_ENGINE)
  IF(WIN32)
	#SET(INF_ENGINE_LIB_DIRS )
	#link_directories("${INF_ENGINE_LIB_DIRS}/Release")
    SET(INF_ENGINE_BASE_DIR "${InferenceEngine_DIR}/../../../inference_engine")
    #MESSAGE(STATUS "^^^^^^^^^^^^^^^^^ IE_INCLUDE_DIR: ${IE_INCLUDE_DIR}")
	link_directories("${INF_ENGINE_BASE_DIR}/lib/intel64/")
	
	#SET(INF_ENGINE_DLL_DIRS "${IE_INCLUDE_DIR}/../../../bin/intel64/Release")
    SET(INF_ENGINE_DLL_DIRS "${INF_ENGINE_BASE_DIR}/bin/intel64/Release")
	FILE(GLOB INF_DLL_NAMES "${INF_ENGINE_DLL_DIRS}/*.dll" "${INF_ENGINE_DLL_DIRS}/*.xml" "${INF_ENGINE_DLL_DIRS}/*.mvcmd" "${INF_ENGINE_DLL_DIRS}/*.json" "${INF_ENGINE_DLL_DIRS}/*.elf")
    MESSAGE(STATUS "^^^^^^^^^^^^^^  INF_DLL_NAMES: ${INF_DLL_NAMES}")
    LIST(APPEND CVEXTERN_DEPENDENCY_DLLS ${INF_DLL_NAMES})

    SET(INF_TBB_ENGINE_DLL_DIRS "${INF_ENGINE_BASE_DIR}/external/tbb/bin")
	MESSAGE(STATUS "^^^^^^^^^^^^^^  INF_TBB_ENGINE_DLL_DIRS: ${INF_TBB_ENGINE_DLL_DIRS}")
    FILE(GLOB INF_TBB_DLL_NAMES "${INF_TBB_ENGINE_DLL_DIRS}/*.dll")
	MESSAGE(STATUS "^^^^^^^^^^^^^^  INF_TBB_DLL_NAMES: ${INF_TBB_DLL_NAMES}")
    FILE(GLOB INF_TBB_DEBUG_DLL_NAMES "${INF_TBB_ENGINE_DLL_DIRS}/*_debug.dll")
	MESSAGE(STATUS "^^^^^^^^^^^^^^  INF_TBB_DEBUG_DLL_NAMES: ${INF_TBB_DEBUG_DLL_NAMES}")
    IF (INF_TBB_DEBUG_DLL_NAMES)
      LIST(REMOVE_ITEM INF_TBB_DLL_NAMES ${INF_TBB_DEBUG_DLL_NAMES})
    ENDIF()
	LIST(APPEND CVEXTERN_DEPENDENCY_DLLS ${INF_TBB_DLL_NAMES})
    
    SET(INF_NGRAPH_DLL_DIRS "${InferenceEngine_DIR}/../../ngraph/lib")
    FILE(GLOB INF_NGRAPH_DLL_NAMES "${INF_NGRAPH_DLL_DIRS}/*ngraph.dll" "${INF_NGRAPH_DLL_DIRS}/*_importer.dll")
    MESSAGE(STATUS "^^^^^^^^^^^^^^  INF_NGRAPH_DLL_NAMES: ${INF_NGRAPH_DLL_NAMES}")
    LIST(APPEND CVEXTERN_DEPENDENCY_DLLS ${INF_NGRAPH_DLL_NAMES})
	
	SET(INF_HDDL_DLL_DIRS "${INF_ENGINE_BASE_DIR}/external/hddl/bin")
    FILE(GLOB INF_HDDL_DLL_NAMES "${INF_HDDL_DLL_DIRS}/*.dll")
    MESSAGE(STATUS "^^^^^^^^^^^^^^  INF_HDDL_DLL_NAMES: ${INF_HDDL_DLL_NAMES}")
    LIST(APPEND CVEXTERN_DEPENDENCY_DLLS ${INF_HDDL_DLL_NAMES})
	
	SET(PROGRAMFILES_X86_ENV "ProgramFiles(x86)")
    SET(ICC_RUNTIME_DIRS "$ENV{${PROGRAMFILES_X86_ENV}}/Common Files/Intel/Shared Libraries/redist/intel64_win/compiler")
    STRING(REGEX REPLACE "\\\\" "/" ICC_RUNTIME_DIRS ${ICC_RUNTIME_DIRS})
    FILE(GLOB ICC_RUNTIME_DLL_NAMES "${ICC_RUNTIME_DIRS}/libmmd.dll" "${ICC_RUNTIME_DIRS}/svml_dispmd.dll")
    MESSAGE(STATUS "^^^^^^^^^^^^^^  ICC_RUNTIME_DLL_NAMES: ${ICC_RUNTIME_DLL_NAMES}")
    LIST(APPEND CVEXTERN_DEPENDENCY_DLLS ${ICC_RUNTIME_DLL_NAMES})
	##Add the inference_engine dll
	#LIST(APPEND CVEXTERN_DEPENDENCY_DLLS "${INF_ENGINE_DLL_DIRS}/inference_engine.dll")
	##Add CPU ingerence plugin and dependency
	#LIST(APPEND CVEXTERN_DEPENDENCY_DLLS "${INF_ENGINE_DLL_DIRS}/MKLDNNPlugin.dll")
	#LIST(APPEND CVEXTERN_DEPENDENCY_DLLS "${INF_ENGINE_DLL_DIRS}/mkl_tiny.dll")
	#LIST(APPEND CVEXTERN_DEPENDENCY_DLLS "${INF_ENGINE_DLL_DIRS}/libiomp5md.dll")
	##Add Intel Integrated Graphics plugin and dependency
	#LIST(APPEND CVEXTERN_DEPENDENCY_DLLS "${INF_ENGINE_DLL_DIRS}/clDNNPlugin.dll")
	#LIST(APPEND CVEXTERN_DEPENDENCY_DLLS "${INF_ENGINE_DLL_DIRS}/clDNN64.dll")
	##Add Heterogeneous plugin and dependency
	#LIST(APPEND CVEXTERN_DEPENDENCY_DLLS "${INF_ENGINE_DLL_DIRS}/HeteroPlugin.dll")
	##Add HDDL plugin and dependency
	#LIST(APPEND CVEXTERN_DEPENDENCY_DLLS "${INF_ENGINE_DLL_DIRS}/clDNN64.dll")
	##Add Myriad plugin and dependency
	#LIST(APPEND CVEXTERN_DEPENDENCY_DLLS "${INF_ENGINE_DLL_DIRS}/myriadPlugin.dll")
	##Add GNA plugin and dependency
	#LIST(APPEND CVEXTERN_DEPENDENCY_DLLS "${INF_ENGINE_DLL_DIRS}/GNAPlugin.dll")
	#LIST(APPEND CVEXTERN_DEPENDENCY_DLLS "${INF_ENGINE_DLL_DIRS}/gna.dll")
  ENDIF()
ENDIF()

############################# FFMPEG START ##############################
if(WIN32 AND WITH_FFMPEG)
  if(TARGET_ARCH_64)
    set(FFMPEG_SUFFIX _64)
  endif()
  set(OPENCV_FFMPEG_OUTPUT_NAME "opencv_videoio_ffmpeg${OPENCV_VERSION_MAJOR}${OPENCV_VERSION_MINOR}${OPENCV_VERSION_PATCH}${FFMPEG_SUFFIX}")
  IF(MSVC_IDE)
	SET(OPENCV_FFMPEG_OUTPUT_FILE_PATH "${EXECUTABLE_OUTPUT_PATH}/Release/${OPENCV_FFMPEG_OUTPUT_NAME}.dll")
  ELSEIF(MSVC AND (CMAKE_GENERATOR MATCHES "Visual"))
	SET(OPENCV_FFMPEG_OUTPUT_FILE_PATH "${EXECUTABLE_OUTPUT_PATH}/${CMAKE_BUILD_TYPE}/${OPENCV_FFMPEG_OUTPUT_NAME}.dll")
  ENDIF()
  #if(EXISTS "${OPENCV_FFMPEG_OUTPUT_FILE_PATH}")
  LIST(APPEND CVEXTERN_DEPENDENCY_DLLS "${OPENCV_FFMPEG_OUTPUT_FILE_PATH}")
  #ELSE()
  #  MESSAGE(FATAL_ERROR "Could not find Open CV FFMPEG dll, please verify if the ffmpeg dll is in '${OPENCV_FFMPEG_OUTPUT_FILE_PATH}'")
  #ENDIF()
endif()

############################### FFMPEG END ##############################

############################### GPU START ##############################
IF(WIN32 AND WITH_CUDA)
  IF(TARGET_ARCH_64)
    LINK_DIRECTORIES(${CUDA_TOOLKIT_ROOT_DIR}/lib/x64)
  ELSE()
    LINK_DIRECTORIES(${CUDA_TOOLKIT_ROOT_DIR}/lib/Win32)
  ENDIF()
  INCLUDE_DIRECTORIES(${CUDA_TOOLKIT_ROOT_DIR}/include)
  SET(CUDA_NPP_INCLUDES ${CUDA_TOOLKIT_ROOT_DIR}/include)
   
  if(EXISTS ${CUDA_NPP_INCLUDES}/cuda.h)
    SET(CUDA_VERSION_FILE ${CUDA_NPP_INCLUDES}/cuda.h)
    
    file( STRINGS ${CUDA_VERSION_FILE} cuda_version REGEX "#define CUDA_VERSION.*")
	string( REGEX REPLACE "#define CUDA_VERSION[ \t]+|//.*" "" cuda_version ${cuda_version})
	string( SUBSTRING ${cuda_version} 0 2 cuda_major)
	string( SUBSTRING ${cuda_version} 2 2 cuda_minor)
	string( REGEX REPLACE "[0]+|//.*" "" cuda_minor ${cuda_minor})
	#MESSAGE(STATUS ">>> cuda version major: ${cuda_major}")
	#MESSAGE(STATUS ">>> cuda version minor: ${cuda_minor}")

	#Also copy the values to the parent scope
	set(cuda_major "${cuda_major}" PARENT_SCOPE)
	set(cuda_minor "${cuda_minor}" PARENT_SCOPE)
  endif()
    
 
  if(EXISTS ${CUDA_NPP_INCLUDES}/nppversion.h)
    SET(NPP_VERSION_FILE ${CUDA_NPP_INCLUDES}/nppversion.h)
    
    file( STRINGS ${NPP_VERSION_FILE} npp_major REGEX "#define NPP_VERSION_MAJOR.*")
    file( STRINGS ${NPP_VERSION_FILE} npp_minor REGEX "#define NPP_VERSION_MINOR.*")
    file( STRINGS ${NPP_VERSION_FILE} npp_build REGEX "#define NPP_VERSION_BUILD.*")
    
    string( REGEX REPLACE "#define NPP_VERSION_MAJOR[ \t]+|//.*" "" npp_major ${npp_major})
    string( REGEX REPLACE "#define NPP_VERSION_MINOR[ \t]+|//.*" "" npp_minor ${npp_minor})
    string( REGEX REPLACE "#define NPP_VERSION_BUILD[ \t]+|//.*" "" npp_build ${npp_build})
  elseif(EXISTS ${CUDA_NPP_INCLUDES}/npp.h)
    SET(NPP_VERSION_FILE ${CUDA_NPP_INCLUDES}/npp.h)
    file( STRINGS ${NPP_VERSION_FILE} npp_major REGEX "#define NPP_VER_MAJOR.*")
    file( STRINGS ${NPP_VERSION_FILE} npp_minor REGEX "#define NPP_VER_MINOR.*")
    file( STRINGS ${NPP_VERSION_FILE} npp_build REGEX "#define NPP_VER_BUILD.*")
    
    string( REGEX REPLACE "#define NPP_VER_MAJOR[ \t]+|//.*" "" npp_major ${npp_major})
    string( REGEX REPLACE "#define NPP_VER_MINOR[ \t]+|//.*" "" npp_minor ${npp_minor})
    string( REGEX REPLACE "#define NPP_VER_BUILD[ \t]+|//.*" "" npp_build ${npp_build})
  endif()
 
  if(EXISTS ${NPP_VERSION_FILE})
    #MESSAGE(STATUS ">>>>> npp version header: ${NPP_VERSION_FILE}")
	
    string( REGEX MATCH "[0-9]+" npp_major ${npp_major} ) 
    string( REGEX MATCH "[0-9]+" npp_minor ${npp_minor} ) 
    string( REGEX MATCH "[0-9]+" npp_build ${npp_build} ) 	
	
	#MESSAGE(STATUS ">>> npp version major: ${npp_major}")
	#MESSAGE(STATUS ">>> npp version minor: ${npp_minor}")
	#MESSAGE(STATUS ">>> npp version build: ${npp_build}")
	
	#Also copy the values to the parent scope
	set(npp_major "${npp_major}" PARENT_SCOPE)
	set(npp_minor "${npp_minor}" PARENT_SCOPE)
	set(npp_build "${npp_build}" PARENT_SCOPE)
  endif()
  
  SET(CUDA_NPP_LIBRARY_ROOT_DIR ${CUDA_TOOLKIT_ROOT_DIR})
  #replace any potential backslash in the path with slash
  #STRING(REGEX REPLACE "\\\\" "/" CUDA_NPP_LIBRARY_ROOT_DIR ${CUDA_NPP_LIBRARY_ROOT_DIR}) 
  
  SET(CUDA_ARCH_SUBFOLDER "")
  IF(TARGET_ARCH_64) 
    SET(CUDA_POSTFIX 64)
    IF(${cuda_major} GREATER 12)
      SET(CUDA_ARCH_SUBFOLDER "/x64")
    ENDIF()
  else()
    SET(CUDA_POSTFIX 32)
  ENDIF()
  
  SET(CVEXTERN_CUDA_DEPENDENCY_DLLS)
  
  IF ((${npp_major} GREATER 5) OR ((${npp_major} STREQUAL "5") AND (${npp_minor} GREATER 0)))
    IF(((${npp_major} GREATER 10) OR (("${npp_major}" STREQUAL "10") AND (${npp_minor} GREATER 0)))) 
	  SET(NPP_POSTFIX "${npp_major}")
	ELSE()
	  SET(NPP_POSTFIX "${npp_major}${npp_minor}")
	ENDIF()
    
	SET(CUDA_NPP_DLLS 
	  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin${CUDA_ARCH_SUBFOLDER}/nppc${CUDA_POSTFIX}_${NPP_POSTFIX}.dll"
      "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin${CUDA_ARCH_SUBFOLDER}/nppi${CUDA_POSTFIX}_${NPP_POSTFIX}.dll"
	  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin${CUDA_ARCH_SUBFOLDER}/nppial${CUDA_POSTFIX}_${NPP_POSTFIX}.dll"
	  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin${CUDA_ARCH_SUBFOLDER}/nppicc${CUDA_POSTFIX}_${NPP_POSTFIX}.dll"
	  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin${CUDA_ARCH_SUBFOLDER}/nppicom${CUDA_POSTFIX}_${NPP_POSTFIX}.dll"
	  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin${CUDA_ARCH_SUBFOLDER}/nppidei${CUDA_POSTFIX}_${NPP_POSTFIX}.dll"
	  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin${CUDA_ARCH_SUBFOLDER}/nppif${CUDA_POSTFIX}_${NPP_POSTFIX}.dll"
	  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin${CUDA_ARCH_SUBFOLDER}/nppig${CUDA_POSTFIX}_${NPP_POSTFIX}.dll"
	  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin${CUDA_ARCH_SUBFOLDER}/nppim${CUDA_POSTFIX}_${NPP_POSTFIX}.dll"
	  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin${CUDA_ARCH_SUBFOLDER}/nppist${CUDA_POSTFIX}_${NPP_POSTFIX}.dll"
	  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin${CUDA_ARCH_SUBFOLDER}/nppisu${CUDA_POSTFIX}_${NPP_POSTFIX}.dll"
	  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin${CUDA_ARCH_SUBFOLDER}/nppitc${CUDA_POSTFIX}_${NPP_POSTFIX}.dll"
      "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin${CUDA_ARCH_SUBFOLDER}/npps${CUDA_POSTFIX}_${NPP_POSTFIX}.dll")
	FOREACH(CUDA_NPP_DLL ${CUDA_NPP_DLLS})
	  IF(EXISTS "${CUDA_NPP_DLL}")
		LIST(APPEND CVEXTERN_CUDA_DEPENDENCY_DLLS "${CUDA_NPP_DLL}")
	  ENDIF()
    ENDFOREACH()
    FILE(GLOB CUDART_DLL_FULL_NAME "${CUDA_TOOLKIT_ROOT_DIR}/bin${CUDA_ARCH_SUBFOLDER}/cudart${CUDA_POSTFIX}_*.dll")
    
	
	LIST(APPEND CVEXTERN_CUDA_DEPENDENCY_DLLS "${CUDART_DLL_FULL_NAME}" )
    #LIST(APPEND CVEXTERN_DEPENDENCY_DLLS
    #  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/nppc${CUDA_POSTFIX}_${npp_major}${npp_minor}.dll"
    #  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/nppi${CUDA_POSTFIX}_${npp_major}${npp_minor}.dll"
    #  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/npps${CUDA_POSTFIX}_${npp_major}${npp_minor}.dll"
    #  "${CUDA_TOOLKIT_ROOT_DIR}/bin/cudart${CUDA_POSTFIX}_${npp_major}${npp_minor}.dll"
    #  )
	
    #LIST(APPEND CVEXTERN_DEPENDENCY_DLL_NAMES 
    #  npp${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build} 
    #  cudart${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build})
    
    #IF(WITH_NVCUVID)
    #  LIST(APPEND CVEXTERN_DEPENDENCY_DLLS "${CUDA_TOOLKIT_ROOT_DIR}/bin/cufft${CUDA_POSTFIX}_${npp_major}${npp_minor}.dll")
    #ENDIF()
    
    IF(WITH_CUFFT)
      FILE(GLOB CVEXTERN_CUFFT_DEPENDENCY_DLLS "${CUDA_TOOLKIT_ROOT_DIR}/bin${CUDA_ARCH_SUBFOLDER}/cufft*.dll")
      MESSAGE(STATUS "CVEXTERN_CUFFT_DEPENDENCY_DLLS: ${CVEXTERN_CUFFT_DEPENDENCY_DLLS}")
      LIST(APPEND CVEXTERN_CUDA_DEPENDENCY_DLLS ${CVEXTERN_CUFFT_DEPENDENCY_DLLS})
    ENDIF()
    
    IF(WITH_CUBLAS)
      FILE(GLOB CVEXTERN_CUBLAS_DEPENDENCY_DLLS "${CUDA_TOOLKIT_ROOT_DIR}/bin${CUDA_ARCH_SUBFOLDER}/cublas*.dll")
      MESSAGE(STATUS "CVEXTERN_CUBLAS_DEPENDENCY_DLLS: ${CVEXTERN_CUBLAS_DEPENDENCY_DLLS}")
      LIST(APPEND CVEXTERN_CUDA_DEPENDENCY_DLLS ${CVEXTERN_CUBLAS_DEPENDENCY_DLLS})
	  #LIST(APPEND CVEXTERN_CUDA_DEPENDENCY_DLLS "${CUDA_TOOLKIT_ROOT_DIR}/bin/cublas${CUDA_POSTFIX}_${NPP_POSTFIX}.dll")
	  #IF ((${npp_major} GREATER 10) OR (("${npp_major}" STREQUAL "10") AND (${npp_minor} GREATER 0)))
	  #  LIST(APPEND CVEXTERN_CUDA_DEPENDENCY_DLLS "${CUDA_TOOLKIT_ROOT_DIR}/bin/cublasLt${CUDA_POSTFIX}_${NPP_POSTFIX}.dll")
	  #ENDIF()
    ENDIF()  
  ELSE()  
    LIST(APPEND CVEXTERN_CUDA_DEPENDENCY_DLLS 
      "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin${CUDA_ARCH_SUBFOLDER}/npp${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build}.dll"
      "${CUDA_TOOLKIT_ROOT_DIR}/bin${CUDA_ARCH_SUBFOLDER}/cudart${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build}.dll"
      )
    #LIST(APPEND CVEXTERN_DEPENDENCY_DLL_NAMES 
    #  npp${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build} 
    #  cudart${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build})
    
    IF(WITH_CUFFT)
      LIST(APPEND CVEXTERN_CUDA_DEPENDENCY_DLLS "${CUDA_TOOLKIT_ROOT_DIR}/bin${CUDA_ARCH_SUBFOLDER}/cufft${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build}.dll")
      #LIST(APPEND CVEXTERN_DEPENDENCY_DLL_NAMES cufft${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build}) 
    ENDIF()
    
    IF(WITH_CUBLAS)
      LIST(APPEND CVEXTERN_CUDA_DEPENDENCY_DLLS "${CUDA_TOOLKIT_ROOT_DIR}/bin${CUDA_ARCH_SUBFOLDER}/cublas${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build}.dll")
      #LIST(APPEND CVEXTERN_DEPENDENCY_DLL_NAMES cublas${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build})
    ENDIF()
  ENDIF()

  IF(WITH_CUDNN)
    FILE(GLOB CVEXTERN_CUDNN_DEPENDENCY_DLLS "${CUDA_TOOLKIT_ROOT_DIR}/bin${CUDA_ARCH_SUBFOLDER}/cudnn*.dll" "${CUDA_TOOLKIT_ROOT_DIR}/bin${CUDA_ARCH_SUBFOLDER}/zlibwapi.dll")
    MESSAGE(STATUS "CVEXTERN_CUDNN_DEPENDENCY_DLLS: ${CVEXTERN_CUDNN_DEPENDENCY_DLLS}")
    LIST(APPEND CVEXTERN_CUDA_DEPENDENCY_DLLS ${CVEXTERN_CUDNN_DEPENDENCY_DLLS})
	#MESSAGE("WITH_CUDNN: ${WITH_CUDNN}")
	#MESSAGE("APPENDING: ${CUDA_TOOLKIT_ROOT_DIR}/bin/cudnn${CUDA_POSTFIX}_${CUDNN_MAJOR_VERSION}.dll")
    #LIST(APPEND CVEXTERN_CUDA_DEPENDENCY_DLLS "${CUDA_TOOLKIT_ROOT_DIR}/bin/cudnn${CUDA_POSTFIX}_${CUDNN_MAJOR_VERSION}.dll")

    # cuDNN's runtime-fusion engine (cudnn_engines_runtime_compiled*.dll) JIT-compiles
    # fused kernels via NVRTC at runtime; without it cuDNN silently falls back to its
    # precompiled engines, but deploy it too so that fusion engine is available.
    FILE(GLOB CVEXTERN_NVRTC_DEPENDENCY_DLLS "${CUDA_TOOLKIT_ROOT_DIR}/bin${CUDA_ARCH_SUBFOLDER}/nvrtc64_*.dll" "${CUDA_TOOLKIT_ROOT_DIR}/bin${CUDA_ARCH_SUBFOLDER}/nvrtc-builtins64_*.dll")
    MESSAGE(STATUS "CVEXTERN_NVRTC_DEPENDENCY_DLLS: ${CVEXTERN_NVRTC_DEPENDENCY_DLLS}")
    LIST(APPEND CVEXTERN_CUDA_DEPENDENCY_DLLS ${CVEXTERN_NVRTC_DEPENDENCY_DLLS})
  ENDIF()

  FILE(GLOB CVEXTERN_CURAND_DEPENDENCY_DLLS "${CUDA_TOOLKIT_ROOT_DIR}/bin${CUDA_ARCH_SUBFOLDER}/curand*.dll")
  MESSAGE(STATUS "CVEXTERN_CURAND_DEPENDENCY_DLLS: ${CVEXTERN_CURAND_DEPENDENCY_DLLS}")
  LIST(APPEND CVEXTERN_CUDA_DEPENDENCY_DLLS ${CVEXTERN_CURAND_DEPENDENCY_DLLS})

  #FILE(GLOB CVEXTERN_CURAND_DEPENDENCY_DLLS "${CUDA_TOOLKIT_ROOT_DIR}/bin/curand*.dll")
  #MESSAGE(STATUS "CVEXTERN_CURAND_DEPENDENCY_DLLS: ${CVEXTERN_CURAND_DEPENDENCY_DLLS}")
  #LIST(APPEND CVEXTERN_CUDA_DEPENDENCY_DLLS ${CVEXTERN_CURAND_DEPENDENCY_DLLS})

  LIST(APPEND CVEXTERN_DEPENDENCY_DLLS ${CVEXTERN_CUDA_DEPENDENCY_DLLS})

ENDIF()

#IF(ANDROID OR IOS)
#  file(GLOB_RECURSE gpu_extern_srcs "gpu/stitching_c.cpp" "gpu/videostab_c.cpp")
#  file(GLOB_RECURSE gpu_extern_hdrs "gpu/stitching_c.h" "gpu/videostab_c.h")
#ELSE()
#  file(GLOB_RECURSE gpu_extern_srcs "gpu/*.cpp")
#  file(GLOB_RECURSE gpu_extern_hdrs "gpu/*.h*")
#ENDIF()
############################### GPU END ################################

############################### ONNX RUNTIME DLL DEPLOY START ##############################
IF(HAVE_ONNXRUNTIME AND ONNXRT_ROOT_DIR)
  FILE(GLOB CVEXTERN_ONNXRUNTIME_DEPENDENCY_DLLS "${ONNXRT_ROOT_DIR}/lib/onnxruntime*.dll")
  MESSAGE(STATUS "CVEXTERN_ONNXRUNTIME_DEPENDENCY_DLLS: ${CVEXTERN_ONNXRUNTIME_DEPENDENCY_DLLS}")
  LIST(APPEND CVEXTERN_DEPENDENCY_DLLS ${CVEXTERN_ONNXRUNTIME_DEPENDENCY_DLLS})
ENDIF()
############################### ONNX RUNTIME DLL DEPLOY END ################################

############################### OpenCL START ##############################
#IF(WITH_OPENCL)
#  MESSAGE(STATUS "Building cvextern with OPENCL")

#  IF(WIN32 AND WITH_OPENCL AND NOT (NETFX_CORE))
#    SET(OPENCL_PROJ OpenCL)
#  ENDIF()
#  IF(WIN32)
#    IF(TARGET_ARCH_64) 
#      MESSAGE("Including OpenCL dll: ${PROJECT_SOURCE_DIR}/../lib/3rdParty/x64/OpenCL.dll")
#      LIST(APPEND CVEXTERN_DEPENDENCY_DLLS "${PROJECT_SOURCE_DIR}/../lib/3rdParty/x64/OpenCL.dll")
#    else()
#      MESSAGE("Including OpenCL dll: ${PROJECT_SOURCE_DIR}/../lib/3rdParty/x64/OpenCL.dll")
#      LIST(APPEND CVEXTERN_DEPENDENCY_DLLS "${PROJECT_SOURCE_DIR}/../lib/3rdParty/x64/OpenCL.dll")
#    ENDIF()
#  ENDIF()
#ENDIF()
############################### OpenCL END ################################

############################### xfeatures2d START ##############################

#  IF( (NOT WITH_CUDA) OR (NOT (TARGET opencv_cudaimgproc)) )
#    FILE(GLOB xfeatures2d_srcs_excludes "xfeatures2d/nonfree_gpu_c.cpp")
#    LIST(REMOVE_ITEM extern_srcs ${xfeatures2d_srcs_excludes})
#    FILE(GLOB xfeatures2d_hdrs_excludes "xfeatures2d/nonfree_gpu_c.h")
#    LIST(REMOVE_ITEM extern_hdrs ${xfeatures2d_hdrs_excludes})
#  ENDIF()

############################### xfeatures2d END ################################

IF(DEFINED CVEXTERN_DEPENDENCY_DLLS)
  FOREACH(CVEXTERN_DEPENDENCY_DLL ${CVEXTERN_DEPENDENCY_DLLS})
	STRING(REPLACE "$(ConfigurationName)" "release" CVEXTERN_DEPENDENCY_DLL ${CVEXTERN_DEPENDENCY_DLL})
    LIST(APPEND CVEXTERN_DEPENDENCY_DLLS_RELEASE ${CVEXTERN_DEPENDENCY_DLL})
  ENDFOREACH()
  #MESSAGE(STATUS "==================> CVEXTERN_DEPENDENCY_DLLS_RELEASE: ${CVEXTERN_DEPENDENCY_DLLS_RELEASE}")
  INSTALL(
    FILES 
    ${CVEXTERN_DEPENDENCY_DLLS_RELEASE}
    DESTINATION libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}
    COMPONENT libs)
ENDIF()

#IF(ANDROID OR APPLE)
#  FILE(GLOB extern_srcs_excludes "${PROJECT_SOURCE_DIR}/tiffio.cpp")
#  LIST(REMOVE_ITEM extern_srcs ${extern_srcs_excludes})
#  #MESSAGE(STATUS "extern_srcs: ${extern_srcs}")
#  FILE(GLOB extern_hdrs_excludes "${PROJECT_SOURCE_DIR}/tiffio_c.h")
#  LIST(REMOVE_ITEM extern_hdrs ${extern_hdrs_excludes})
#ENDIF()



