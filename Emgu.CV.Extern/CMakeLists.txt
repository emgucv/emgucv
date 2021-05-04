# --------------------------------------------------------
#  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
# 
#  CMake file for cvextern. See root CMakeLists.txt
#
# ----------------------------------------------------------------------------


project(cvextern)
set(the_target ${PROJECT_NAME})

INCLUDE_DIRECTORIES("${OPENCV_SUBFOLDER}/include/opencv")
INCLUDE_DIRECTORIES("${OPENCV_SUBFOLDER}/modules/core/src/parallel")
INCLUDE_DIRECTORIES("${OPENCV_SUBFOLDER}/include")
INCLUDE_DIRECTORIES("${CMAKE_BINARY_DIR}/opencv/3rdparty/zlib")

IF(EMGU_CV_WITH_DEPTHAI)
  INCLUDE_DIRECTORIES("${CMAKE_CURRENT_SOURCE_DIR}/depthai-core/include/depthai")
  add_definitions(-DHAVE_DEPTHAI)
ENDIF()

MACRO(CREATE_VECTOR_CS vname velement velement_cs element_type cs_source_folder namespace_cs cs_compilation_condition additional_c_header additional_c_code c_compilation_condition )
  SET(VECTOR_NAME ${vname})
  SET(VECTOR_ELEMENT ${velement})
  SET(VECTOR_ELEMENT_CS ${velement_cs})
  SET(NAMESPACE_CS ${namespace_cs})
  SET(IS_VECTOR_OF_VECTOR false)
  
  SET(VECTOR_ADDITIONAL_INCLUDE "")
  SET(VECTOR_ADDITIONAL_CODE "")
  
  #SET(extra_macro_args ${ARGN})
  # Did we get any optional args?
  #LIST(LENGTH extra_macro_args num_extra_args)
  #IF (${num_extra_args} GREATER 0)
  #  list(GET extra_macro_args 0 additional_c_header)
    SET(VECTOR_ADDITIONAL_INCLUDE ${additional_c_header})
  #ENDIF()
  #IF (${num_extra_args} GREATER 1)
  #  list(GET extra_macro_args 1 additional_c_code)
    SET(VECTOR_ADDITIONAL_CODE ${additional_c_code})
  #ENDIF()

  SET(COMPILATION_CONDITION_CS_OPEN "")
  SET(COMPILATION_CONDITION_CS_CLOSE "")  
#  IF (NOT ("${cs_compilation_condition}" STREQUAL ""))
#	SET(COMPILATION_CONDITION_CS_OPEN "#if ${cs_compilation_condition}")
#	SET(COMPILATION_CONDITION_CS_CLOSE "#endif")
#  ENDIF()

  #SET(COMPILATION_CONDITION_C_OPEN "")
  #SET(COMPILATION_CONDITION_C_CLOSE "")    
  #IF (NOT ("${c_compilation_condition}" STREQUAL ""))
	SET(COMPILATION_CONDITION_C_OPEN "#if ${c_compilation_condition}")
	SET(COMPILATION_CONDITION_C_ELSE "#else")
	SET(COMPILATION_CONDITION_C_CLOSE "#endif")
  #ENDIF()
  
  if (${element_type} STREQUAL "struct")
    SET(IS_INPUT_OUTPUT_ARRAY true)
    CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/cmake/vectorOfStruct_c.h.in ${CMAKE_CURRENT_SOURCE_DIR}/vector_${VECTOR_NAME}.h)
    CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/cmake/vectorOfStruct_c.cpp.in ${CMAKE_CURRENT_SOURCE_DIR}/vector_${VECTOR_NAME}.cpp)
    CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/cmake/VectorOfStruct.cs.in ${cs_source_folder}/VectorOf${VECTOR_NAME}.cs)
  ELSEIF(${element_type} STREQUAL "struct_not_array")
    SET(IS_INPUT_OUTPUT_ARRAY false)
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
  ELSEIF(${element_type} STREQUAL "object") 
    SET(IS_INPUT_OUTPUT_ARRAY true)
    CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/cmake/vectorOfObject_c.h.in ${CMAKE_CURRENT_SOURCE_DIR}/vector_${VECTOR_NAME}.h)
    CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/cmake/vectorOfObject_c.cpp.in ${CMAKE_CURRENT_SOURCE_DIR}/vector_${VECTOR_NAME}.cpp)
    CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/cmake/VectorOfObject.cs.in ${cs_source_folder}/VectorOf${VECTOR_NAME}.cs)
  ELSEIF(${element_type} STREQUAL "object_not_array") 
    SET(IS_INPUT_OUTPUT_ARRAY false)
    CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/cmake/vectorOfObject_c.h.in ${CMAKE_CURRENT_SOURCE_DIR}/vector_${VECTOR_NAME}.h)
    CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/cmake/vectorOfObject_c.cpp.in ${CMAKE_CURRENT_SOURCE_DIR}/vector_${VECTOR_NAME}.cpp)
    CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/cmake/VectorOfObject.cs.in ${cs_source_folder}/VectorOf${VECTOR_NAME}.cs)
  ENDIF()
ENDMACRO()

CREATE_VECTOR_CS("Byte" "unsigned char" "byte" "struct" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")
#CREATE_VECTOR_CS("IntPtr" "void*" "IntPtr" "struct" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")
CREATE_VECTOR_CS("Int" "int" "int" "struct" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")
CREATE_VECTOR_CS("Float" "float" "float" "struct" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")
CREATE_VECTOR_CS("Double" "double" "double" "struct" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")
CREATE_VECTOR_CS("Point" "cv::Point" "Point" "struct" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")
CREATE_VECTOR_CS("PointF" "cv::Point2f" "PointF" "struct" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")
CREATE_VECTOR_CS("Point3D32F" "cv::Point3f" "MCvPoint3D32f" "struct" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")
CREATE_VECTOR_CS("Rect" "cv::Rect" "Rectangle" "struct" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")
CREATE_VECTOR_CS("RotatedRect" "cv::RotatedRect" "RotatedRect" "struct" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")
CREATE_VECTOR_CS("KeyPoint" "cv::KeyPoint" "MKeyPoint" "struct" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")
CREATE_VECTOR_CS("DMatch" "cv::DMatch" "MDMatch" "struct" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")
CREATE_VECTOR_CS("Triangle2DF" "cv::Vec6f" "Triangle2DF" "struct" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")
CREATE_VECTOR_CS("Size" "cv::Size" "Size" "struct" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")
#IF(HAVE_opencv_latentsvm)
#  CREATE_VECTOR_CS("ObjectDetection" "cv::lsvm::LSVMDetector::ObjectDetection" "MCvObjectDetection" "struct" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "#include \"opencv2/latentsvm.hpp\"" "" "1")
#ENDIF()

CREATE_VECTOR_CS("ERStat" "cv::text::ERStat" "MCvERStat" "struct" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/Text" Emgu.CV.Text "" "#include \"text_c.h\"" "" "defined(HAVE_OPENCV_TEXT)")
CREATE_VECTOR_CS("VectorOfERStat" "std::vector< cv::text::ERStat >" "MCvERStat" "vector" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/Text" Emgu.CV.Text "" "#include \"text_c.h\"" "" "defined(HAVE_OPENCV_TEXT)")

#IF(HAVE_opencv_line_descriptor)
CREATE_VECTOR_CS("KeyLine" "cv::line_descriptor::KeyLine" "MKeyLine" "struct_not_array" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/LineDescriptor" Emgu.CV.LineDescriptor "" "#include \"line_descriptor_c.h\"" "" "defined(HAVE_OPENCV_LINE_DESCRIPTOR)")
#ENDIF()

CREATE_VECTOR_CS("ColorPoint" "ColorPoint" "ColorPoint" "struct" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "#include \"videoio_c_extra.h\"" "" "defined(HAVE_OPENCV_VIDEOIO)")

IF(EMGU_CV_WITH_TESSERACT)
  SET(EMGU_CV_WITH_TESSERACT_FLAG "1")
ELSE()
  SET(EMGU_CV_WITH_TESSERACT_FLAG "0")
ENDIF()
CREATE_VECTOR_CS("TesseractResult" "TesseractResult" "TesseractResult" "struct" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.OCR/" Emgu.CV.OCR "" "#include \"tesseract_c.h\"" "" "defined(HAVE_EMGUCV_TESSERACT)")

CREATE_VECTOR_CS("Mat" "cv::Mat" "Mat" "object" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")
CREATE_VECTOR_CS("UMat" "cv::UMat" "UMat" "object" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")
CREATE_VECTOR_CS("GMat" "cv::GMat" "GMat" "object_not_array" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "#include \"gapi_c.h\"" "" "defined(HAVE_OPENCV_GAPI)")
CREATE_VECTOR_CS("CvString" "cv::String" "CvString" "object" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")

CREATE_VECTOR_CS("VectorOfPoint" "std::vector< cv::Point >" "Point" "vector" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")
CREATE_VECTOR_CS("VectorOfPointF" "std::vector< cv::Point2f >" "PointF" "vector" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")
CREATE_VECTOR_CS("VectorOfPoint3D32F" "std::vector< cv::Point3f >" "MCvPoint3D32f" "vector" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")
CREATE_VECTOR_CS("VectorOfInt" "std::vector< int >" "int" "vector" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")
CREATE_VECTOR_CS("VectorOfByte" "std::vector< unsigned char >" "Byte" "vector" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")
CREATE_VECTOR_CS("VectorOfDMatch" "std::vector< cv::DMatch >" "MDMatch" "vector" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")
CREATE_VECTOR_CS("VectorOfRect" "std::vector< cv::Rect >" "Rectangle" "vector" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")


CREATE_VECTOR_CS("OclPlatformInfo" "cv::ocl::PlatformInfo" "Ocl.PlatformInfo" "object" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "#include \"opencv2/core/ocl.hpp\"" "" "1")

CREATE_VECTOR_CS("GpuMat" "cv::cuda::GpuMat" "GpuMat" "object" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Cuda" Emgu.CV.Cuda "" "#include \"opencv2/core/cuda.hpp\"" "" "1")

CREATE_VECTOR_CS("VideoCapture" "cv::VideoCapture" "VideoCapture" "object_not_array" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "#include \"videoio_c_extra.h\"" "" "defined(HAVE_OPENCV_VIDEOIO)")


MACRO(WRITE_IF_DIFFERENT fname content)
  IF (EXISTS "${fname}")
    FILE(READ "${fname}" EXISTING_FILE_CONTENT)
    IF(NOT ("${EXISTING_FILE_CONTENT}" STREQUAL "${content}") )
      FILE(WRITE "${fname}" "${content}")
    ENDIF()
  ELSE()
    FILE(WRITE "${fname}" "${content}")
  ENDIF()
ENDMACRO()

#################################################################################
# Code Generation
# Types:
#
# * val: [c++] {obj}.${PROPERTY_NAME}() [C#] {obj}.${CS_FUNCTION_NAME} {get;}
# * struct: [c++] {obj}.get${PROPERTY_NAME}(); {obj}.set${PROPERTY_NAME}(val); [C#] {obj}.${CS_FUNCTION_NAME} {get; set;}
# * propW: [c++] {obj}.set${PROPERTY_NAME}(val); [C#] {obj}.Set${CS_FUNCTION_NAME}(val)
# * propR: [c++] {obj}.get${PROPERTY_NAME}(); [C#] {obj}.${CS_FUNCTION_NAME} {get;} 
# * prop: [c++] {obj}.set${PROPERTY_NAME}(val); & {obj}.get${PROPERTY_NAME}(); [C#] {obj}.${CS_FUNCTION_NAME} {get; set;}
# * act: [c++] {obj}.${PROPERTY_NAME}(); [C#] {obj}.${CS_FUNCTION_NAME}(); where val is a value / structure, a single value is returned
# * act0: [c++] {obj}.${PROPERTY_NAME}(); [C#] {obj}.${CS_FUNCTION_NAME}(); where val is a value / structure, no value is returned
# * act1: [c++] {obj}.${PROPERTY_NAME}(val); [C#] {obj}.${CS_FUNCTION_NAME}(val); where val is a value / structure, no value is returned
# * act1obj: [c++] {obj}.${PROPERTY_NAME}(val); [C#] {obj}.${CS_FUNCTION_NAME}(val); where val is an object, no value is returned
# * element: [c++] {obj}.${PROPERTY_NAME}; [C#] {obj}.${CS_FUNCTION_NAME} {get; set;}
# * elementR: [c++] {obj}.${PROPERTY_NAME}; [C#] {obj}.${CS_FUNCTION_NAME} {get;}
#################################################################################
MACRO(CREATE_OCV_CLASS_PROPERTY fname csfname cname_full cname pnames ptypes mtypes cs_func_names csptypes csp_docs cs_namespace cs_invoke_class cs_class_name cs_compilation_condition header_additional_include source_additional_code c_compilation_condition ocv_module_enabled)
  #MESSAGE(STATUS ">>>>>>>>>>>>>>>> ocv_module_enabled: ${ocv_module_enabled}")
  IF ("${ocv_module_enabled}" STREQUAL "ON")
	SET(IS_DUMMY OFF)
  ELSE()
    SET(IS_DUMMY ON)
  ENDIF()
  #MESSAGE(STATUS ">>>>>>>>>>>>>>>> is_dummy: ${IS_DUMMY}")

  SET(FILE_NAME ${fname})
  SET(CLASS_NAME_FULL ${cname_full})
  SET(CLASS_NAME ${cname})
  SET(PROPERTY_NAMES ${pnames})
  SET(PROPERTY_TYPES ${ptypes})
  SET(CS_FUNCTION_NAMES ${cs_func_names})
  SET(MARSHAL_TYPES ${mtypes})
  SET(CS_PROPERTY_TYPES ${csptypes})
  SET(CS_PROPERTY_DOCS ${csp_docs})
  #SET(HEADER_ADDITIONAL_INCLUDE "")
  #SET(SOURCE_ADDITIONAL_CODE "")
  SET(DEFAULT_FUNCTION_NOT_SUPPORT_MESSAGE "This function is not implemented in the current platform")
  SET(RAISE_FUNCTION_NOT_SUPPORT_CPP "CV_Error(cv::Error::StsBadFunc, \"${DEFAULT_FUNCTION_NOT_SUPPORT_MESSAGE}\");")
  
  SET(CS_COMPILATION_CONDITION_OPEN "")
  SET(CS_COMPILATION_CONDITION_CLOSE "")
  
  IF(NOT ("${cs_compilation_condition}" STREQUAL ""))
    SET(CS_COMPILATION_CONDITION_OPEN "#if ${cs_compilation_condition}")
    SET(CS_COMPILATION_CONDITION_CLOSE "#endif")
  ENDIF()

  #SET(extra_macro_args ${ARGN})
  # Did we get any optional args?
  #LIST(LENGTH extra_macro_args num_extra_args)
  #IF (${num_extra_args} GREATER 0)
  #  list(GET extra_macro_args 0 additional_c_header)
  #  SET(HEADER_ADDITIONAL_INCLUDE ${additional_c_header})
  #ENDIF()
  #IF (${num_extra_args} GREATER 1)
  #  list(GET extra_macro_args 1 additional_c_code)
  #  SET(SOURCE_ADDITIONAL_CODE ${additional_c_code})
  #ENDIF()
  #MESSAGE(STATUS "-------------------- PROPERTY_NAMES: ${PROPERTY_NAMES}")
  
  SET(C_COMPILATION_CONDITION_OPEN "")
  SET(C_COMPILATION_CONDITION_CLOSE "")  
  IF (NOT ("${c_compilation_condition}" STREQUAL ""))
	SET(C_COMPILATION_CONDITION_OPEN "#if ${c_compilation_condition}")
	SET(C_COMPILATION_CONDITION_CLOSE "#endif")
  ENDIF()
#  IF(IS_DUMMY)
#	SET(C_HEADER_SOURCE "#include \"opencv2/core/core_c.h\"")
#  ELSE()
	SET(C_HEADER_SOURCE "${header_additional_include}")
#  ENDIF()

  SET(C_SOURCE "${source_additional_code} #include \"${fname}.h\"")
  
  SET(CS_SOURCE "//----------------------------------------------------------------------------
//  This file is automatically generated, do not modify.      
//----------------------------------------------------------------------------

${CS_COMPILATION_CONDITION_OPEN}

using System;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace ${cs_namespace}
{
   public static partial class ${cs_invoke_class}
   {
")
  SET(CS_CLASS_SOURCE "public partial class ${cs_class_name}
   {
")
  LIST(LENGTH PROPERTY_NAMES PROPERTY_COUNT)
  math(EXPR idx "${PROPERTY_COUNT} - 1")
  FOREACH(ival RANGE ${idx})
    #MESSAGE(STATUS "-------------------- PROPERTY_NAMES: ${PROPERTY_NAMES}")
    #MESSAGE(STATUS "-------------------- PROPERTY_TYPES: ${PROPERTY_TYPES}")
    #MESSAGE(STATUS "-------------------- val: ${ival}")
    LIST(GET PROPERTY_NAMES ${ival} PROPERTY_NAME)
    
    #STRING(SUBSTRING "${PROPERTY_NAME}" 1 -1 PROPERTY_NAME_PART2)
    #STRING(SUBSTRING "${PROPERTY_NAME}" 0 1 PROPERTY_NAME_PART1)
    #STRING(TOUPPER "${PROPERTY_NAME_PART1}" PROPERTY_NAME_PART1)
    #SET(CS_PROPERTY_NAME "${PROPERTY_NAME_PART1}${PROPERTY_NAME_PART2}")
    
    LIST(GET PROPERTY_TYPES ${ival} PROPERTY_TYPE)
    LIST(GET MARSHAL_TYPES ${ival} MARSHAL_TYPE)
    LIST(GET CS_PROPERTY_TYPES ${ival} CS_PROPERTY_TYPE)
    LIST(GET CS_PROPERTY_DOCS ${ival} CS_DOCUMENTATION )
    STRING(STRIP "${CS_DOCUMENTATION}" CS_DOCUMENTATION )
    
    LIST(GET CS_FUNCTION_NAMES ${ival} CS_FUNCTION_NAME)
    
    SET(MARSHAL_IN "")
    SET(MARSHAL_RETURN "")
    IF("${CS_PROPERTY_TYPE}" STREQUAL "bool")
      SET(MARSHAL_IN "
        [MarshalAs(CvInvoke.BoolMarshalType)]")
      SET(MARSHAL_RETURN "
     [return: MarshalAs(CvInvoke.BoolMarshalType)]")
    ENDIF()
    
    IF("${MARSHAL_TYPE}" STREQUAL "val")
      IF("${PROPERTY_TYPE}" STREQUAL "cv::String") #special handling for functions that returns strings
	    IF(IS_DUMMY)
	  	  SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}${CS_FUNCTION_NAME}(void* obj, cv::String* str);  
     ")
	      SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}${CS_FUNCTION_NAME}(void* obj, cv::String* str) { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} }   
     ")
	    ELSE()
	      SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, cv::String* str);  
     ")
	      SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, cv::String* str) { *str = obj->${PROPERTY_NAME}(); }   
     ")
	    ENDIF()
	
	    SET(CS_SOURCE "${CS_SOURCE}
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] ${MARSHAL_RETURN}
     internal static extern void cve${CLASS_NAME}${CS_FUNCTION_NAME}(IntPtr obj, IntPtr str);
     ")
	    SET(CS_CLASS_SOURCE "${CS_CLASS_SOURCE}
     /// <summary>
     /// ${CS_DOCUMENTATION}
     /// </summary>
     public ${CS_PROPERTY_TYPE} ${CS_FUNCTION_NAME}
     {
        get 
        { 
           using (CvString s = new CvString())
           {  
              ${cs_invoke_class}.cve${CLASS_NAME}${CS_FUNCTION_NAME}(_ptr, s); 
              return s.ToString();
           }
        } 
     }
     ")
      ELSE()
	    IF (IS_DUMMY)
		  SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(${PROPERTY_TYPE}) cve${CLASS_NAME}${CS_FUNCTION_NAME}(void* obj);  
     ")
	      SET(C_SOURCE "${C_SOURCE}
${PROPERTY_TYPE} cve${CLASS_NAME}${CS_FUNCTION_NAME}(void* obj) { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} }   
     ")
		ELSE()
	      SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(${PROPERTY_TYPE}) cve${CLASS_NAME}${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj);  
     ")
	      SET(C_SOURCE "${C_SOURCE}
${PROPERTY_TYPE} cve${CLASS_NAME}${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj) { return obj->${PROPERTY_NAME}(); }   
     ")
		ENDIF()
	
	SET(CS_SOURCE "${CS_SOURCE}
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] ${MARSHAL_RETURN}
     internal static extern ${CS_PROPERTY_TYPE} cve${CLASS_NAME}${CS_FUNCTION_NAME}(IntPtr obj);
     ")
	SET(CS_CLASS_SOURCE "${CS_CLASS_SOURCE}
     /// <summary>
     /// ${CS_DOCUMENTATION}
     /// </summary>
     public ${CS_PROPERTY_TYPE} ${CS_FUNCTION_NAME}
     {
        get { return ${cs_invoke_class}.cve${CLASS_NAME}${CS_FUNCTION_NAME}(_ptr); } 
     }
     ")
      ENDIF()
    ELSEIF("${MARSHAL_TYPE}" STREQUAL "struct")
      IF("${PROPERTY_TYPE}" STREQUAL "CvTermCriteria") #special handling for functions that returns CvTermCriteria
	    IF(IS_DUMMY)
	  	SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* value);
CVAPI(void) cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* value);     
     ")
	    SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* value) { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} }
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* value) { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} }     
     ")
	    ELSE()
	    SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value);
CVAPI(void) cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value);     
     ")
        SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value) { ${PROPERTY_TYPE} p = cvTermCriteria(obj->get${PROPERTY_NAME}()); memcpy(value, &p, sizeof(${PROPERTY_TYPE})); }
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value) { obj->set${PROPERTY_NAME}( *value ); }     
     ")
	    ENDIF()
	
      SET(CS_SOURCE "${CS_SOURCE}
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(IntPtr obj, ref ${CS_PROPERTY_TYPE} val);
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(IntPtr obj, ref ${CS_PROPERTY_TYPE} val);
     ")
      
      SET(CS_CLASS_SOURCE "${CS_CLASS_SOURCE}
     /// <summary>
     /// ${CS_DOCUMENTATION}
     /// </summary>
     public ${CS_PROPERTY_TYPE} ${CS_FUNCTION_NAME}
     {
        get { ${CS_PROPERTY_TYPE} v = new ${CS_PROPERTY_TYPE}(); ${cs_invoke_class}.cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(_ptr, ref v); return v; } 
        set { ${cs_invoke_class}.cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(_ptr, ref value); }
     }
     ")
      ELSEIF("${PROPERTY_TYPE}" STREQUAL "CvSize") #special handling for functions that returns CvSize
	    IF(IS_DUMMY)
	  	SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* value);
CVAPI(void) cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* value);     
     ")
	    SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* value) { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} }
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* value) { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} }     
     ")
	    ELSE()
	    SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value);
CVAPI(void) cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value);     
     ")
        SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value) { ${PROPERTY_TYPE} p = cvSize(obj->get${PROPERTY_NAME}()); memcpy(value, &p, sizeof(${PROPERTY_TYPE})); }
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value) { obj->set${PROPERTY_NAME}( *value ); }     
     ")
	    ENDIF()
	
      SET(CS_SOURCE "${CS_SOURCE}
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(IntPtr obj, ref ${CS_PROPERTY_TYPE} val);
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(IntPtr obj, ref ${CS_PROPERTY_TYPE} val);
     ")
      
      SET(CS_CLASS_SOURCE "${CS_CLASS_SOURCE}
     /// <summary>
     /// ${CS_DOCUMENTATION}
     /// </summary>
     public ${CS_PROPERTY_TYPE} ${CS_FUNCTION_NAME}
     {
        get { ${CS_PROPERTY_TYPE} v = new ${CS_PROPERTY_TYPE}(); ${cs_invoke_class}.cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(_ptr, ref v); return v; } 
        set { ${cs_invoke_class}.cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(_ptr, ref value); }
     }
     ")
     ELSE()
	  IF(IS_DUMMY)
	  	SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* value);
CVAPI(void) cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* value);     
     ")
	    SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* value) { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} }
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* value) { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} }     
     ")
	  ELSE()
	    SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value);
CVAPI(void) cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value);     
     ")
        SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value) { ${PROPERTY_TYPE} p = obj->get${PROPERTY_NAME}(); memcpy(value, &p, sizeof(${PROPERTY_TYPE})); }
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value) { obj->set${PROPERTY_NAME}( *value ); }     
     ")
	    
	  ENDIF()
      SET(CS_SOURCE "${CS_SOURCE}
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(IntPtr obj, ref ${CS_PROPERTY_TYPE} val);
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(IntPtr obj, ref ${CS_PROPERTY_TYPE} val);
     ")
      
      SET(CS_CLASS_SOURCE "${CS_CLASS_SOURCE}
     /// <summary>
     /// ${CS_DOCUMENTATION}
     /// </summary>
     public ${CS_PROPERTY_TYPE} ${CS_FUNCTION_NAME}
     {
        get { ${CS_PROPERTY_TYPE} v = new ${CS_PROPERTY_TYPE}(); ${cs_invoke_class}.cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(_ptr, ref v); return v; } 
        set { ${cs_invoke_class}.cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(_ptr, ref value); }
     }
     ")
     ENDIF()
     ELSEIF("${MARSHAL_TYPE}" STREQUAL "structR")
     IF("${PROPERTY_TYPE}" STREQUAL "CvSize") #special handling for functions that returns CvSize
     IF(IS_DUMMY)
	     SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* value);
     ")
         SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* value) { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} }
     ")
	   ELSE()
         SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value);
     ")
         SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value) { ${PROPERTY_TYPE} p = cvSize(obj->get${PROPERTY_NAME}()); memcpy(value, &p, sizeof(${PROPERTY_TYPE})); }
     ")
	   ENDIF()
      SET(CS_SOURCE "${CS_SOURCE}
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(IntPtr obj, ref ${CS_PROPERTY_TYPE} val);     
     ")
      
      SET(CS_CLASS_SOURCE "${CS_CLASS_SOURCE}
     /// <summary>
     /// ${CS_DOCUMENTATION}
     /// </summary>
     public ${CS_PROPERTY_TYPE} ${CS_FUNCTION_NAME}
     {
        get { ${CS_PROPERTY_TYPE} v = new ${CS_PROPERTY_TYPE}(); ${cs_invoke_class}.cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(_ptr, ref v); return v; } 
     }
     ")
     ELSE()
	   IF(IS_DUMMY)
	     SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* value);
     ")
         SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* value) { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} }
     ")
	   ELSE()
         SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value);
     ")
         SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value) { ${PROPERTY_TYPE} p = obj->get${PROPERTY_NAME}(); memcpy(value, &p, sizeof(${PROPERTY_TYPE})); }
     ")
	   ENDIF()
      SET(CS_SOURCE "${CS_SOURCE}
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(IntPtr obj, ref ${CS_PROPERTY_TYPE} val);     
     ")
      
      SET(CS_CLASS_SOURCE "${CS_CLASS_SOURCE}
     /// <summary>
     /// ${CS_DOCUMENTATION}
     /// </summary>
     public ${CS_PROPERTY_TYPE} ${CS_FUNCTION_NAME}
     {
        get { ${CS_PROPERTY_TYPE} v = new ${CS_PROPERTY_TYPE}(); ${cs_invoke_class}.cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(_ptr, ref v); return v; } 
     }
     ")
    ENDIF()
    ELSEIF(${MARSHAL_TYPE} STREQUAL "propW")
	  IF("${PROPERTY_TYPE}" STREQUAL "cv::String") #special handling for functions that returns strings
	    IF(IS_DUMMY)
	      SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(void* obj, cv::String* str);  
     ")
	      SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(void* obj, cv::String* str) { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} }   
     ")
		ELSE()
	      SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, cv::String* str);  
     ")
	      SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, cv::String* str) { obj->set${PROPERTY_NAME}(*str); }   
     ")
	    ENDIF()
	
	    SET(CS_SOURCE "${CS_SOURCE}
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] ${MARSHAL_RETURN}
     internal static extern void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(IntPtr obj, IntPtr str);
     ")
	    SET(CS_CLASS_SOURCE "${CS_CLASS_SOURCE}
     /// <summary>
     /// ${CS_DOCUMENTATION}
     /// </summary>
	 /// <param name=\"s\">The value</param>
     public void Set${CS_FUNCTION_NAME}(${CS_PROPERTY_TYPE} s)
     { 
           using (CvString cvs = new CvString(s))
           {  
              ${cs_invoke_class}.cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(_ptr, cvs); 
           }   
     }
     ")
      ELSE()
	    IF(IS_DUMMY)
		  SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE} value);     
     ")
          SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE} value) { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} }     
     ")
		ELSE()
          SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE} value);     
     ")
          SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE} value) { obj->set${PROPERTY_NAME}( value ); }     
     ")
	    ENDIF()
      
      SET(CS_SOURCE "${CS_SOURCE}
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(
        IntPtr obj, ${MARSHAL_IN} 
        ${CS_PROPERTY_TYPE} val);
     ")
      SET(CS_CLASS_SOURCE "${CS_CLASS_SOURCE}
     /// <summary>
     /// ${CS_DOCUMENTATION}
     /// </summary>
	 /// <param name=\"value\">The value</param>
     public void Set${CS_FUNCTION_NAME}(${CS_PROPERTY_TYPE} value)
     {
        ${cs_invoke_class}.cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(_ptr, value); 
     }
     ")
	 ENDIF()
	ELSEIF(${MARSHAL_TYPE} STREQUAL "act")
	  IF(IS_DUMMY)
	  	SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(${PROPERTY_TYPE}) cve${CLASS_NAME}${CS_FUNCTION_NAME}(void* obj);     
     ")
        SET(C_SOURCE "${C_SOURCE}
${PROPERTY_TYPE} cve${CLASS_NAME}${CS_FUNCTION_NAME}(void* obj) { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} }     
     ")
	  ELSE()
	    SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(${PROPERTY_TYPE}) cve${CLASS_NAME}${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj);     
     ")
        SET(C_SOURCE "${C_SOURCE}
${PROPERTY_TYPE} cve${CLASS_NAME}${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj) { return obj->${PROPERTY_NAME}(); }     
     ")
	 ENDIF()
      
      SET(CS_SOURCE "${CS_SOURCE}
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
	 ${MARSHAL_RETURN} 
     internal static extern ${CS_PROPERTY_TYPE} cve${CLASS_NAME}${CS_FUNCTION_NAME}(
        IntPtr obj);
     ")
      SET(CS_CLASS_SOURCE "${CS_CLASS_SOURCE}
     /// <summary>
     /// ${CS_DOCUMENTATION}
     /// </summary>
     /// <returns>The result</returns>
     public ${CS_PROPERTY_TYPE} ${CS_FUNCTION_NAME}( )
     {
        return ${cs_invoke_class}.cve${CLASS_NAME}${CS_FUNCTION_NAME}(_ptr); 
     }
     ")
	ELSEIF(${MARSHAL_TYPE} STREQUAL "act0")
	  IF (IS_DUMMY)
	  	SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}${CS_FUNCTION_NAME}(void* obj);     
     ")
        SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}${CS_FUNCTION_NAME}(void* obj) { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} }     
     ")
	  ELSE()
	    SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj);     
     ")
        SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj) { obj->${PROPERTY_NAME}(); }     
     ")
	 ENDIF()
      
      SET(CS_SOURCE "${CS_SOURCE}
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
	 ${MARSHAL_RETURN} 
     internal static extern void cve${CLASS_NAME}${CS_FUNCTION_NAME}(
        IntPtr obj);
     ")
      SET(CS_CLASS_SOURCE "${CS_CLASS_SOURCE}
     /// <summary>
     /// ${CS_DOCUMENTATION}
     /// </summary>
     public void ${CS_FUNCTION_NAME}( )
     {
        ${cs_invoke_class}.cve${CLASS_NAME}${CS_FUNCTION_NAME}(_ptr); 
     }
     ")
    ELSEIF(${MARSHAL_TYPE} STREQUAL "act1")
	  IF(IS_DUMMY)
        SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE} value);     
     ")
        SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE} value) { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} }     
     ")
	  ELSE()
        SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE} value);     
     ")
        SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE} value) { obj->${PROPERTY_NAME}( value ); }     
     ")
	 ENDIF()
      
      SET(CS_SOURCE "${CS_SOURCE}
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cve${CLASS_NAME}${CS_FUNCTION_NAME}(
        IntPtr obj, ${MARSHAL_IN} 
        ${CS_PROPERTY_TYPE} val);
     ")
      SET(CS_CLASS_SOURCE "${CS_CLASS_SOURCE}
     /// <summary>
     /// ${CS_DOCUMENTATION}
     /// </summary>
	 /// <param name=\"value\">The value</param>
     public void ${CS_FUNCTION_NAME}(${CS_PROPERTY_TYPE} value)
     {
        ${cs_invoke_class}.cve${CLASS_NAME}${CS_FUNCTION_NAME}(_ptr, value); 
     }
     ")
    ELSEIF(${MARSHAL_TYPE} STREQUAL "act1obj")
	  IF(IS_DUMMY)
        SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* value);     
     ")
        SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* value) { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} }     
     ")
	  ELSE()
        SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value);     
     ")
        SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value) { obj->${PROPERTY_NAME}( *value ); }     
     ")
	 ENDIF()
      
      SET(CS_SOURCE "${CS_SOURCE}
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cve${CLASS_NAME}${CS_FUNCTION_NAME}(
        IntPtr obj, ${MARSHAL_IN} 
        IntPtr val);
     ")
      SET(CS_CLASS_SOURCE "${CS_CLASS_SOURCE}
     /// <summary>
     /// ${CS_DOCUMENTATION}
     /// </summary>
     /// <param name=\"value\">The value</param>
     public void ${CS_FUNCTION_NAME}(${CS_PROPERTY_TYPE} value)
     {
        ${cs_invoke_class}.cve${CLASS_NAME}${CS_FUNCTION_NAME}(_ptr, value); 
     }
     ")
    ELSEIF(${MARSHAL_TYPE} STREQUAL "propR")
      IF("${PROPERTY_TYPE}" STREQUAL "cv::String") #special handling for functions that returns strings
        IF (IS_DUMMY)
          SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* val);  
     ")
          SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* val) { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} }   
     ")
	    ELSE()
          SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* val);  
     ")
          SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* val) { *val = obj->get${PROPERTY_NAME}(); }   
     ")
	   ENDIF()
      
        SET(CS_SOURCE "${CS_SOURCE}
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] ${MARSHAL_RETURN}
     internal static extern void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(IntPtr obj, IntPtr val);
     ")
        SET(CS_CLASS_SOURCE "${CS_CLASS_SOURCE}
     /// <summary>
    /// ${CS_DOCUMENTATION}
     /// </summary>
     public ${CS_PROPERTY_TYPE} ${CS_FUNCTION_NAME}
     {
        get 
        { 
          using (CvString s = new CvString())
          {
            ${cs_invoke_class}.cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(_ptr, s);
            return s.ToString();
          }
        } 
     }
     ")

     ELSE()
      IF (IS_DUMMY)
        SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(${PROPERTY_TYPE}) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj);  
     ")
        SET(C_SOURCE "${C_SOURCE}
${PROPERTY_TYPE} cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj) { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} }   
     ")
	  ELSE()
        SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(${PROPERTY_TYPE}) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj);  
     ")
        SET(C_SOURCE "${C_SOURCE}
${PROPERTY_TYPE} cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj) { return obj->get${PROPERTY_NAME}(); }   
     ")
	 ENDIF()
      
      SET(CS_SOURCE "${CS_SOURCE}
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] ${MARSHAL_RETURN}
     internal static extern ${CS_PROPERTY_TYPE} cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(IntPtr obj);
     ")
      SET(CS_CLASS_SOURCE "${CS_CLASS_SOURCE}
     /// <summary>
    /// ${CS_DOCUMENTATION}
     /// </summary>
     public ${CS_PROPERTY_TYPE} ${CS_FUNCTION_NAME}
     {
        get { return ${cs_invoke_class}.cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(_ptr); } 
     }
     ")
     ENDIF()
    ELSEIF("${MARSHAL_TYPE}" STREQUAL "element")
      IF (${PROPERTY_TYPE} STREQUAL "cv::Mat") #special handling for functions that returns Mats
	    IF(IS_DUMMY)
	      SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(${PROPERTY_TYPE}*) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj);
     ")
	      SET(C_SOURCE "${C_SOURCE}
${PROPERTY_TYPE}* cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj) { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} }
     ")
		ELSE()
	      SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(${PROPERTY_TYPE}*) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj);
     ")
	      SET(C_SOURCE "${C_SOURCE}
${PROPERTY_TYPE}* cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj) { return &(obj->${PROPERTY_NAME}); }
     ")
	    ENDIF()
	
	SET(CS_SOURCE "${CS_SOURCE}
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] ${MARSHAL_RETURN}
     internal static extern IntPtr cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(IntPtr obj);
     ")
	SET(CS_CLASS_SOURCE "${CS_CLASS_SOURCE}
     /// <summary>
     /// ${CS_DOCUMENTATION}
     /// </summary>
	 /// <returns>The result</returns>
     public ${CS_PROPERTY_TYPE} ${CS_FUNCTION_NAME}
     {
        get { return new ${CS_PROPERTY_TYPE}( ${cs_invoke_class}.cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(_ptr), false); } 
     }
     ")

	 ###################
	  ELSEIF("${PROPERTY_TYPE}" STREQUAL "cv::String") #special handling for functions that returns strings
	    IF(IS_DUMMY)
	      SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj, cv::String* str);  
CVAPI(void) cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(void* obj, cv::String* str);  
     ")
	      SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj, cv::String* str) { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} }   
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(void* obj, cv::String* str) { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} }   
     ")
		ELSE()
	      SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, cv::String* str);  
CVAPI(void) cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, cv::String* str);  
     ")
	      SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, cv::String* str) { *str = obj->${PROPERTY_NAME}; }   
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, cv::String* str) { obj->${PROPERTY_NAME} = *str; }   
     ")
		ENDIF()
	
	SET(CS_SOURCE "${CS_SOURCE}
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] ${MARSHAL_RETURN}
     internal static extern void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(IntPtr obj, IntPtr str);
	 [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] ${MARSHAL_RETURN}
     internal static extern void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(IntPtr obj, IntPtr str);
     ")
	SET(CS_CLASS_SOURCE "${CS_CLASS_SOURCE}
     /// <summary>
     /// ${CS_DOCUMENTATION}
     /// </summary>
     public ${CS_PROPERTY_TYPE} ${CS_FUNCTION_NAME}
     {
        get 
        { 
           using (CvString s = new CvString())
           {  
              ${cs_invoke_class}.cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(_ptr, s); 
              return s.ToString();
           }
        } 
		set
		{
		   using (CvString s = new CvString(value))
           {  
              ${cs_invoke_class}.cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(_ptr, s); 
           }
		}
     }
     ")
	 ###################
      ELSE()
	    IF(IS_DUMMY)
	      SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(${PROPERTY_TYPE}) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj);
CVAPI(void) cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE} value);     
     ")
	      SET(C_SOURCE "${C_SOURCE}
${PROPERTY_TYPE} cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj) { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} }
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE} value) { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} }     
     ")
		ELSE()
	      SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(${PROPERTY_TYPE}) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj);
CVAPI(void) cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE} value);     
     ")
	      SET(C_SOURCE "${C_SOURCE}
${PROPERTY_TYPE} cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj) { return obj->${PROPERTY_NAME}; }
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE} value) { obj->${PROPERTY_NAME} = value; }     
     ")
	    ENDIF()
	
	SET(CS_SOURCE "${CS_SOURCE}
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] ${MARSHAL_RETURN}
     internal static extern ${CS_PROPERTY_TYPE} cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(IntPtr obj);
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(
        IntPtr obj, ${MARSHAL_IN} 
        ${CS_PROPERTY_TYPE} val);
     ")
	SET(CS_CLASS_SOURCE "${CS_CLASS_SOURCE}
     /// <summary>
     /// ${CS_DOCUMENTATION}
     /// </summary>
     public ${CS_PROPERTY_TYPE} ${CS_FUNCTION_NAME}
     {
        get { return ${cs_invoke_class}.cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(_ptr); } 
        set { ${cs_invoke_class}.cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(_ptr, value); }
     }
     ")
      ENDIF()
    ELSEIF("${MARSHAL_TYPE}" STREQUAL "elementR")
	  IF("${PROPERTY_TYPE}" STREQUAL "cv::String") #special handling for functions that returns strings
	    IF(IS_DUMMY)
	      SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj, cv::String* str);   
     ")
	      SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj, cv::String* str) { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} }   
     ")
		ELSE()
	      SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, cv::String* str);   
     ")
	      SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, cv::String* str) { *str = obj->${PROPERTY_NAME}; }   
     ")
	    ENDIF()
	
	SET(CS_SOURCE "${CS_SOURCE}
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] ${MARSHAL_RETURN}
     internal static extern void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(IntPtr obj, IntPtr str);
     ")
	SET(CS_CLASS_SOURCE "${CS_CLASS_SOURCE}
     /// <summary>
     /// ${CS_DOCUMENTATION}
     /// </summary>
     public ${CS_PROPERTY_TYPE} ${CS_FUNCTION_NAME}
     {
        get 
        { 
           using (CvString s = new CvString())
           {  
              ${cs_invoke_class}.cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(_ptr, s); 
              return s.ToString();
           }
        }
     }
     ")
	 ELSE()
	  IF(IS_DUMMY)
        SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(${PROPERTY_TYPE}) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj);
     ")
        SET(C_SOURCE "${C_SOURCE}
${PROPERTY_TYPE} cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj) { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} }
     ")
	  ELSE()
        SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(${PROPERTY_TYPE}) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj);
     ")
        SET(C_SOURCE "${C_SOURCE}
${PROPERTY_TYPE} cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj) { return obj->${PROPERTY_NAME}; }
     ")
	  ENDIF()
      
      SET(CS_SOURCE "${CS_SOURCE}
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] ${MARSHAL_RETURN}
     internal static extern ${CS_PROPERTY_TYPE} cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(IntPtr obj);
     ")
      SET(CS_CLASS_SOURCE "${CS_CLASS_SOURCE}
     /// <summary>
     /// ${CS_DOCUMENTATION}
     /// </summary>
     public ${CS_PROPERTY_TYPE} ${CS_FUNCTION_NAME}
     {
        get { return ${cs_invoke_class}.cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(_ptr); } 
     }
     ")
	 ENDIF()
    ELSE() # for "prop" type
      IF("${PROPERTY_TYPE}" STREQUAL "cv::String") #special handling for functions that returns strings

	    IF(IS_DUMMY)
          SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj, cv::String* str);  
CVAPI(void) cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(void* obj, cv::String* str);  
     ")
	      SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj, cv::String* str) { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} }   
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(void* obj, cv::String* str) { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} }   
     ")
	    ELSE()
          SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, cv::String* str);  
CVAPI(void) cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, cv::String* str);  
     ")
	      SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, cv::String* str) { *str = obj->get${PROPERTY_NAME}(); }   
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, cv::String* str) { obj->set${PROPERTY_NAME}(*str); }   
     ")
	    ENDIF()
	
	    SET(CS_SOURCE "${CS_SOURCE}
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] ${MARSHAL_RETURN}
     internal static extern void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(IntPtr obj, IntPtr str);
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] ${MARSHAL_RETURN}
     internal static extern void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(IntPtr obj, IntPtr str);
     ")
	    SET(CS_CLASS_SOURCE "${CS_CLASS_SOURCE}
     /// <summary>
     /// ${CS_DOCUMENTATION}
     /// </summary>
     public ${CS_PROPERTY_TYPE} ${CS_FUNCTION_NAME}
     {
        get 
        { 
           using (CvString s = new CvString())
           {  
              ${cs_invoke_class}.cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(_ptr, s); 
              return s.ToString();
           }
        } 
        set
        {
           using (CvString s = new CvString(value))
           {  
              ${cs_invoke_class}.cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(_ptr, s); 
           }
        }
     }
     ")
      ELSE()

	  IF(IS_DUMMY)
        SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(${PROPERTY_TYPE}) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj);
CVAPI(void) cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE} value);     
     ")
        SET(C_SOURCE "${C_SOURCE}
${PROPERTY_TYPE} cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj) { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} }
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE} value) { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} }     
     ")
	  ELSE()
        SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(${PROPERTY_TYPE}) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj);
CVAPI(void) cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE} value);     
     ")
        SET(C_SOURCE "${C_SOURCE}
${PROPERTY_TYPE} cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj) { return obj->get${PROPERTY_NAME}(); }
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE} value) { obj->set${PROPERTY_NAME}( value ); }     
     ")
	 ENDIF()
      
      SET(CS_SOURCE "${CS_SOURCE}
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] ${MARSHAL_RETURN}
     internal static extern ${CS_PROPERTY_TYPE} cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(IntPtr obj);
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(
        IntPtr obj, ${MARSHAL_IN} 
        ${CS_PROPERTY_TYPE} val);
     ")
      SET(CS_CLASS_SOURCE "${CS_CLASS_SOURCE}
     /// <summary>
     /// ${CS_DOCUMENTATION}
     /// </summary>
     public ${CS_PROPERTY_TYPE} ${CS_FUNCTION_NAME}
     {
        get { return ${cs_invoke_class}.cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(_ptr); } 
        set { ${cs_invoke_class}.cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(_ptr, value); }
     }
     ")
     ENDIF()
    ENDIF()
  ENDFOREACH()
  SET(CS_CLASS_SOURCE "${CS_CLASS_SOURCE}
   }")
  SET(CS_SOURCE "${CS_SOURCE}
   }

   ${CS_CLASS_SOURCE}
}
${CS_COMPILATION_CONDITION_CLOSE}")
  SET(C_SOURCE "${C_COMPILATION_CONDITION_OPEN} 
  ${C_SOURCE} 
  ${C_COMPILATION_CONDITION_OPEN}")
  #MESSAGE(STATUS "-------------------- CS_CLASS_SOURCE: ${CS_CLASS_SOURCE}")
  WRITE_IF_DIFFERENT("${csfname}" "${CS_SOURCE}")
  WRITE_IF_DIFFERENT("${PROJECT_SOURCE_DIR}/${fname}.h" "${C_HEADER_SOURCE}")
  WRITE_IF_DIFFERENT("${PROJECT_SOURCE_DIR}/${fname}.cpp" "${C_SOURCE}")

ENDMACRO()

############################### core code gen START ##############################

  CREATE_OCV_CLASS_PROPERTY( 
    "core/mat_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Core/Mat.g.cs"
    "cv::Mat" 
    "Mat" 
    "isContinuous;isSubmatrix;depth;empty;channels;pop_back;push_back;total;dims" 
    "bool;bool;int;bool;int;int;cv::Mat;size_t;int" 
    "val;val;val;val;val;act1;act1obj;val;elementR"
    "IsContinuous;IsSubmatrix;Depth;IsEmpty;NumberOfChannels;PopBack;PushBack;Total;Dims" 
    "bool;bool;CvEnum.DepthType;bool;int;int;Mat;IntPtr;int"
    "True if the data is continues;
  True if the matrix is a submatrix of another matrix;
  Depth type;
  True if the Mat is empty;
  Number of channels;
  The method removes one or more rows from the bottom of the matrix;
  Adds elements to the bottom of the matrix;
  The method returns the number of array elements (a number of pixels if the array represents an image);
  The matrix dimensionality"
    "Emgu.CV"
    "CvInvoke"
    "Mat"
	""
    "#include \"mat_c.h\""
	""
	"" 
	${HAVE_opencv_core})
  
  CREATE_OCV_CLASS_PROPERTY( 
    "core/umat_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Core/UMat.g.cs"
    "cv::UMat" 
    "UMat" 
    "isContinuous;isSubmatrix;depth;empty;channels;total;dims" 
    "bool;bool;int;bool;int;size_t;int" 
    "val;val;val;val;val;val;elementR"
    "IsContinuous;IsSubmatrix;Depth;IsEmpty;NumberOfChannels;Total;Dims" 
    "bool;bool;CvEnum.DepthType;bool;int;IntPtr;int"
    "True if the data is continues;
  True if the matrix is a submatrix of another matrix;
  Depth type;
  True if the matrix is empty;
  Number of channels;
  The method returns the number of array elements (a number of pixels if the array represents an image);
  The matrix dimensionality"
    "Emgu.CV"
    "CvInvoke"
    "UMat"
	""
    "#include \"umat_c.h\""
	""
	""
	${HAVE_opencv_core})
  
  CREATE_OCV_CLASS_PROPERTY( 
    "core/input_array_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Core/InputArray.g.cs"
    "cv::_InputArray" 
    "InputArray" 
    "isMat;isUMat;isMatVector;isUMatVector;isMatx;kind" 
    "bool;bool;bool;bool;bool;int" 
    "val;val;val;val;val;val"
    "IsMat;IsUMat;IsMatVector;IsUMatVector;IsMatx;Kind" 
    "bool;bool;bool;bool;bool;InputArray.Type"
    "True if the input array is a Mat;
  True if the input array is an UMat;
  True if the input array is a vector of Mat;
  True if the input array is a vector of UMat;
  True if the input array is a Matx;
  The type of the input array"
    "Emgu.CV"
    "CvInvoke"
    "InputArray"
	""
    "#include \"core_c_extra.h\""
	""
	""
	${HAVE_opencv_core})
  
  CREATE_OCV_CLASS_PROPERTY( 
    "core/output_array_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Core/OutputArray.g.cs"
    "cv::_OutputArray" 
    "OutputArray" 
    "fixedSize;fixedType;needed" 
    "bool;bool;bool" 
    "val;val;val"
    "FixedSize;FixedType;Needed" 
    "bool;bool;bool"
    "True if the output array is fixed size;
  True if the output array is fixed type;
  True if the output array is needed"
    "Emgu.CV"
    "CvInvoke"
    "OutputArray"
	""
    "#include \"core_c_extra.h\""
	""
	""
	${HAVE_opencv_core})
  
  CREATE_OCV_CLASS_PROPERTY( 
    "core/ocl_device_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Ocl/Device.g.cs"
    "cv::ocl::Device" 
    "Device" 
    "isNVidia;isIntel;isAMD;addressBits;linkerAvailable;compilerAvailable;available;maxWorkGroupSize;maxComputeUnits;localMemSize;maxMemAllocSize;deviceVersionMajor;deviceVersionMinor;halfFPConfig;singleFPConfig;doubleFPConfig;hostUnifiedMemory;globalMemSize;image2DMaxWidth;image2DMaxHeight;type;name;version;vendorName;driverVersion;extensions;OpenCLVersion;OpenCL_C_Version" 
    "bool;bool;bool;int;bool;bool;bool;int;int;int;int;int;int;int;int;int;bool;size_t;int;int;int;cv::String;cv::String;cv::String;cv::String;cv::String;cv::String;cv::String" 
    "val;val;val;val;val;val;val;val;val;val;val;val;val;val;val;val;val;val;val;val;val;val;val;val;val;val;val;val"
    "IsNVidia;IsIntel;IsAMD;AddressBits;LinkerAvailable;CompilerAvailable;Available;MaxWorkGroupSize;MaxComputeUnits;LocalMemSize;MaxMemAllocSize;DeviceVersionMajor;DeviceVersionMinor;HalfFPConfig;SingleFPConfig;DoubleFPConfig;HostUnifiedMemory;GlobalMemSize;Image2DMaxWidth;Image2DMaxHeight;Type;Name;Version;VendorName;DriverVersion;Extensions;OpenCLVersion;OpenCLCVersion" 
    "bool;bool;bool;int;bool;bool;bool;int;int;int;int;int;int;FpConfig;FpConfig;FpConfig;bool;IntPtr;int;int;DeviceType;String;String;String;String;String;String;String"
    "Indicates if this is an NVidia device;
  Indicates if this is an Intel device;
  Indicates if this is an AMD device;
  The AddressBits;
  Indicates if the linker is available;
  Indicates if the compiler is available;
  Indicates if the device is available;
  The maximum work group size;
  The max compute unit;
  The local memory size;
  The maximum memory allocation size;
  The device major version number;
  The device minor version number;
  The device half floating point configuration;
  The device single floating point configuration;
  The device double floating point configuration;
  True if the device use unified memory;
  The global memory size;
  The image 2d max width;
  The image2d max height;
  The ocl device type;
  The device name;
  The device version;
  The device vendor name;
  The device driver version;
  The device extensions;
  The device OpenCL version;
  The device OpenCL C version"
    "Emgu.CV.Ocl"
    "OclInvoke"
    "Device"
	""
    "#include \"ocl_c.h\""
	""
	""
	${HAVE_opencv_core})
  
  CREATE_OCV_CLASS_PROPERTY( 
    "core/ocl_platform_info_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Ocl/PlatformInfo.g.cs"
    "cv::ocl::PlatformInfo" 
    "PlatformInfo" 
    "name;version;vendor;deviceNumber" 
    "cv::String;cv::String;cv::String;int" 
    "val;val;val;val"
    "Name;Version;Vendor;DeviceNumber" 
    "String;String;String;int"
    "The platform name;
  The platform version;
  The platform vendor;
  The number of devices"
    "Emgu.CV.Ocl"
    "OclInvoke"
    "PlatformInfo"
	""
    "#include \"ocl_c.h\""
	""
	""
	${HAVE_opencv_core})
  
  CREATE_OCV_CLASS_PROPERTY( 
    "core/ocl_kernel_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Ocl/Kernel.g.cs"
    "cv::ocl::Kernel" 
    "OclKernel"
    "empty;ptr"
    "bool;void*"
    "val;val"
    "Empty;NativeKernelPtr"
    "bool;IntPtr"
    "Indicates if the kernel is empty;
  The pointer to the native kernel"
    "Emgu.CV.Ocl"
    "OclInvoke"
    "Kernel" 
	""
    "#include \"ocl_c.h\""
	""
	""
	${HAVE_opencv_core})
  
  CREATE_OCV_CLASS_PROPERTY( 
    "core/gpumat_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Cuda/GpuMat.g.cs"
    "cv::cuda::GpuMat" 
    "GpuMat" 
    "isContinuous;depth;empty;channels" 
    "bool;int;bool;int" 
    "val;val;val;val"
    "IsContinuous;Depth;IsEmpty;NumberOfChannels" 
    "bool;CvEnum.DepthType;bool;int"
    "True if the data is continues;
  Depth type;
  True if the matrix is empty;
  Number of channels"
    "Emgu.CV.Cuda"
    "CudaInvoke"
    "GpuMat"
	""
    "#include \"opencv2/core/cuda.hpp\"
	#include \"opencv2/core/types_c.h\""
	""
	""
	${HAVE_opencv_core})

  CREATE_OCV_CLASS_PROPERTY( 
    "core/file_node_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Core/FileNode.g.cs"
    "cv::FileNode" 
    "FileNode" 
    "isNamed;empty;isNone;isSeq;isMap;isInt;isReal;isString" 
    "bool;bool;bool;bool;bool;bool;bool;bool" 
    "val;val;val;val;val;val;val;val"
    "IsNamed;IsEmpty;IsNone;IsSeq;IsMap;IsInt;IsReal;IsString" 
    "bool;bool;bool;bool;bool;bool;bool;bool"
    "Returns true if the node has a name;
	Returns true if the node is empty;
	Returns true if the node is a \"none\" object;
	Returns true if the node is a sequence;
	Returns true if the node is a mapping;
	Returns true if the node is an integer;
	Returns true if the node is a floating-point number;
	Returns true if the node is a text string"
    "Emgu.CV"
    "CvInvoke"
    "FileNode"
	""
    "#include \"core_c_extra.h\""
	""
	""
	${HAVE_opencv_core})

  CREATE_OCV_CLASS_PROPERTY( 
    "core/moments_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Core/Moments.g.cs"
    "cv::Moments" 
    "Moments" 
    "m00;m10;m01;m20;m11;m02;m30;m21;m12;m03;mu20;mu11;mu02;mu30;mu21;mu12;mu03;nu20;nu11;nu02;nu30;nu21;nu12;nu03" 
    "double;double;double;double;double;double;double;double;double;double;double;double;double;double;double;double;double;double;double;double;double;double;double;double" 
    "element;element;element;element;element;element;element;element;element;element;element;element;element;element;element;element;element;element;element;element;element;element;element;element"
    "M00;M10;M01;M20;M11;M02;M30;M21;M12;M03;Mu20;Mu11;Mu02;Mu30;Mu21;Mu12;Mu03;Nu20;Nu11;Nu02;Nu30;Nu21;Nu12;Nu03" 
    "double;double;double;double;double;double;double;double;double;double;double;double;double;double;double;double;double;double;double;double;double;double;double;double" 
    "Spatial Moment M00;
    Spatial Moment M10;
    Spatial Moment M01;
    Spatial Moment M20;
    Spatial Moment M11;
    Spatial Moment M02;
    Spatial Moment M30;
    Spatial Moment M21;
    Spatial Moment M12;
    Spatial Moment M03;
    Central Moment Mu20;
    Central Moment Mu11;
    Central Moment Mu02;
    Central Moment Mu30;
    Central Moment Mu21;
    Central Moment Mu12;
    Central Moment Mu03;
    Central Normalized Moment Nu20;
    Central Normalized Moment Nu11;
    Central Normalized Moment Nu02;
    Central Normalized Moment Nu30;
    Central Normalized Moment Nu21;
    Central Normalized Moment Nu12;
    Central Normalized Moment Nu03"
    "Emgu.CV"
    "CvInvoke"
    "Moments"
	""
    "#include \"core_c_extra.h\""
	""
	""
	${HAVE_opencv_core})

############################### core code gen END ################################

############################### objdetect code gen START ##############################
IF (NOT HAVE_opencv_objdetect)
  SET(HAVE_opencv_objdetect FALSE)
ENDIF()
  CREATE_OCV_CLASS_PROPERTY( 
    "objdetect/QRCodeDetector_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Objdetect/QRCodeDetector.g.cs"
    "cv::QRCodeDetector" 
    "QRCodeDetector" 
    "EpsX;EpsY" 
    "double;double" 
    "propW;propW"
    "EpsX;EpsY" 
    "double;double"
    "EpsX;
     EpsY"
    "Emgu.CV"
    "CvInvoke"
    "QRCodeDetector"
	""
    "#include \"objdetect_c.h\""
	""
	""
	${HAVE_opencv_objdetect})

############################### objdetect code gen END ################################

############################### imgproc code gen START ##############################
IF (NOT HAVE_opencv_imgproc)
  SET(HAVE_opencv_imgproc FALSE)
ENDIF()
  CREATE_OCV_CLASS_PROPERTY( 
    "imgproc/LineIterator_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Imgproc/LineIterator.g.cs"
    "cv::LineIterator" 
    "LineIterator" 
    "count" 
    "int" 
    "element"
    "Count" 
    "int"
    "The total number of pixels in the line"
    "Emgu.CV"
    "CvInvoke"
    "LineIterator"
	""
    "#include \"imgproc_c.h\""
	""
	""
	${HAVE_opencv_imgproc})
  CREATE_OCV_CLASS_PROPERTY( 
    "imgproc/IntelligentScissorsMB_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Imgproc/IntelligentScissorsMB.g.cs"
    "cv::segmentation::IntelligentScissorsMB" 
    "IntelligentScissorsMB" 
    "EdgeFeatureZeroCrossingParameters;GradientMagnitudeMaxLimit" 
    "float;float" 
    "propW;propW"
    "EdgeFeatureZeroCrossingParameters;GradientMagnitudeMaxLimit" 
    "float;float"
    "Switch to Laplacian Zero-Crossing edge feature extractor and specify its parameters. This feature extractor is used by default according to article. Implementation has additional filtering for regions with low-amplitude noise. This filtering is enabled through parameter of minimal gradient amplitude (use some small value 4, 8, 16).;
    Specify gradient magnitude max value threshold. Zero limit value is used to disable gradient magnitude thresholding (default behavior, as described in original article). Otherwize pixels with gradient magnitude greater than threshold have zero cost."
    "Emgu.CV"
    "CvInvoke"
    "IntelligentScissorsMB"
	""
    "#include \"imgproc_c.h\""
	""
	""
	${HAVE_opencv_imgproc})
    


############################### imgproc code gen END ################################

############################### features2d code gen START ##############################
IF (NOT HAVE_opencv_features2d)
  SET(HAVE_opencv_features2d FALSE)
ENDIF()
  CREATE_OCV_CLASS_PROPERTY( 
    "features2d/SimpleBlobDetector_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Features2D/SimpleBlobDetectorParams.g.cs"
    "cv::SimpleBlobDetector::Params" 
    "SimpleBlobDetectorParams" 
    "thresholdStep;minThreshold;maxThreshold;minDistBetweenBlobs;filterByColor;blobColor;filterByArea;minArea;maxArea;filterByCircularity;minCircularity;maxCircularity;filterByInertia;minInertiaRatio;maxInertiaRatio;filterByConvexity;minConvexity;maxConvexity;minRepeatability" 
    "float;float;float;float;bool;uchar;bool;float;float;bool;float;float;bool;float;float;bool;float;float;size_t" 
    "element;element;element;element;element;element;element;element;element;element;element;element;element;element;element;element;element;element;element"
    "ThresholdStep;MinThreshold;MaxThreshold;MinDistBetweenBlobs;FilterByColor;blobColor;FilterByArea;MinArea;MaxArea;FilterByCircularity;MinCircularity;MaxCircularity;FilterByInertia;MinInertiaRatio;MaxInertiaRatio;FilterByConvexity;MinConvexity;MaxConvexity;MinRepeatability" 
    "float;float;float;float;bool;Byte;bool;float;float;bool;float;float;bool;float;float;bool;float;float;IntPtr"
    "Threshold step;
  Min threshold;
  Max threshold;
  Min dist between blobs;
  Filter by color;
  Blob color;
  Filter by area;
  Min area;
  Max area;
  Filter by circularity;
  Min circularity;
  Max circularity;
  Filter by inertia;
  Min inertia ratio;
  Max inertia ratio;
  Filter by convexity;
  Min Convexity;
  Max Convexity;
  Min Repeatability"
    "Emgu.CV.Features2D"
    "Features2DInvoke"
    "SimpleBlobDetectorParams"
	""
    "#include \"features2d_c.h\""
	""
	""
	${HAVE_opencv_features2d})

############################### features2d code gen END ################################

############################### mcc code gen START ##############################
IF (NOT HAVE_opencv_mcc)
  SET(HAVE_opencv_mcc FALSE)
ENDIF()
  CREATE_OCV_CLASS_PROPERTY( 
    "mcc/CChecker_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/Mcc/CChecker.g.cs"
    "cv::mcc::CChecker" 
    "CChecker" 
    "Target;Cost" 
    "cv::mcc::TYPECHART;float" 
    "prop;prop"
    "Target;Cost" 
    "CChecker.TypeChart;float"
    "Target;Cost"
    "Emgu.CV.Mcc"
    "MccInvoke"
    "CChecker"
	""
    "#include \"mcc_c.h\""
	""
	""
	${HAVE_opencv_mcc})

      CREATE_OCV_CLASS_PROPERTY( 
    "mcc/DetectorParameters_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/Mcc/DetectorParameters.g.cs"
    "cv::mcc::DetectorParameters" 
    "DetectorParameters" 
    "adaptiveThreshWinSizeMin;adaptiveThreshWinSizeMax;adaptiveThreshWinSizeStep;adaptiveThreshConstant;minContoursAreaRate;minContoursArea;confidenceThreshold;minContourSolidity;findCandidatesApproxPolyDPEpsMultiplier;borderWidth;B0factor;maxError;minContourPointsAllowed;minContourLengthAllowed;minInterContourDistance;minInterCheckerDistance;minImageSize;minGroupSize" 
    "int;int;int;double;double;double;double;double;double;int;float;float;int;int;int;int;int;unsigned" 
    "element;element;element;element;element;element;element;element;element;element;element;element;element;element;element;element;element;element"
    "AdaptiveThreshWinSizeMin;AdaptiveThreshWinSizeMax;AdaptiveThreshWinSizeStep;AdaptiveThreshConstant;MinContoursAreaRate;MinContoursArea;ConfidenceThreshold;MinContourSolidity;FindCandidatesApproxPolyDPEpsMultiplier;BorderWidth;B0factor;MaxError;MinContourPointsAllowed;MinContourLengthAllowed;MinInterContourDistance;MinInterCheckerDistance;MinImageSize;MinGroupSize" 
    "int;int;int;double;double;double;double;double;double;int;float;float;int;int;int;int;int;uint"
    "AdaptiveThreshold minimum window size;
    AdaptiveThreshold maximum window size;
    AdaptiveThreshold window size step;
    AdaptiveThreshold constant;
    Minimum Contours Area Rate;
    Minimum Contours Area;
    Confidence Threshold;
    Minimum Contour Solidity;
    Find Candidates Approx Poly DP Eps Multiplier;
    Border Width;
    B0factor;
    Max Error;
    Minimum Contour Points Allowed;
    Minimum Contour Length Allowed;
    Minimum InterContour Distance;
    Minimum InterChecker Distance;
    Minimum Image Size;
    Minimum Group Size
    "  # documentation
    "Emgu.CV.Mcc"
    "MccInvoke"
    "DetectorParameters"
	""
    "#include \"mcc_c.h\""
	""
	""
	${HAVE_opencv_mcc})

############################### features2d code gen END ################################

############################### ml code gen START ##############################
IF (NOT HAVE_opencv_ml)
  SET(HAVE_opencv_ml FALSE)
ENDIF()
  CREATE_OCV_CLASS_PROPERTY( 
    "ml/em_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Ml/EM.g.cs"
    "cv::ml::EM" 
    "EM" 
    "ClustersNumber;CovarianceMatrixType;TermCriteria" 
    "int;int;CvTermCriteria" 
    "prop;prop;struct"
    "ClustersNumber;CovarianceMatrixType;TermCriteria" 
    "int;EM.CovarianMatrixType;MCvTermCriteria"
    "The number of mixtures;
  The type of the mixture covariation matrices;
  Termination criteria of the procedure. EM algorithm stops either after a certain number of iterations (term_crit.num_iter), or when the parameters change too little (no more than term_crit.epsilon) from iteration to iteration"
    "Emgu.CV.ML"
    "MlInvoke"
    "EM"
	""
    "#include \"ml_c.h\""
	""
	""
	${HAVE_opencv_ml})
  
  CREATE_OCV_CLASS_PROPERTY( 
    "ml/svm_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Ml/SVM.g.cs"
    "cv::ml::SVM" 
    "SVM" 
    "Type;Gamma;Coef0;Degree;C;Nu;P;Kernel;TermCriteria;KernelType" 
    "int;double;double;double;double;double;double;int;CvTermCriteria;int" 
    "prop;prop;prop;prop;prop;prop;prop;propW;struct;propR"
    "Type;Gamma;Coef0;Degree;C;Nu;P;Kernel;TermCriteria;KernelType" 
    "SVM.SvmType;double;double;double;double;double;double;SVM.SvmKernelType;MCvTermCriteria;SVM.SvmKernelType"
    "Type of a SVM formulation;
  Parameter gamma of a kernel function;
  Parameter coef0 of a kernel function;
  Parameter degree of a kernel function;
  Parameter C of a SVM optimization problem;
  Parameter nu of a SVM optimization problem;
  Parameter epsilon of a SVM optimization problem;
  Initialize with one of predefined kernels;
  Termination criteria of the iterative SVM training procedure which solves a partial case of constrained quadratic optimization problem;
  Type of a SVM kernel"
    "Emgu.CV.ML"
    "MlInvoke"
    "SVM"
	""
    "#include \"ml_c.h\""
	""
	""
	${HAVE_opencv_ml})
	
  CREATE_OCV_CLASS_PROPERTY( 
    "ml/svmsgd_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Ml/SVMSGD.g.cs"
    "cv::ml::SVMSGD" 
    "SVMSGD" 
    "SvmsgdType;MarginType;MarginRegularization;InitialStepSize;StepDecreasingPower;TermCriteria" 
    "int;int;float;float;float;CvTermCriteria" 
    "prop;prop;prop;prop;prop;struct"
    "Type;Margin;MarginRegularization;InitialStepSize;StepDecreasingPower;TermCriteria" 
    "SVMSGD.SvmsgdType;SVMSGD.MarginType;float;float;float;MCvTermCriteria"
    "Algorithm type;
	Margin type;
	marginRegularization of a SVMSGD optimization problem;
	initialStepSize of a SVMSGD optimization problem;
	stepDecreasingPower of a SVMSGD optimization problem;
	Termination criteria of the training algorithm."
    "Emgu.CV.ML"
    "MlInvoke"
    "SVMSGD"
	""
    "#include \"ml_c.h\""
	""
	""
	${HAVE_opencv_ml})	
  
  CREATE_OCV_CLASS_PROPERTY( 
    "ml/knearest_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Ml/KNearest.g.cs"
    "cv::ml::KNearest" 
    "KNearest" 
    "DefaultK;IsClassifier;Emax;AlgorithmType" 
    "int;bool;int;int" 
    "prop;prop;prop;prop"
    "DefaultK;IsClassifier;Emax;AlgorithmType" 
    "int;bool;int;KNearest.Types"
    "Default number of neighbors to use in predict method;
  Whether classification or regression model should be trained;
  Parameter for KDTree implementation;
  Algorithm type"
    "Emgu.CV.ML"
    "MlInvoke"
    "KNearest"
	""
    "#include \"ml_c.h\""
	""
	""
	${HAVE_opencv_ml})
  
  CREATE_OCV_CLASS_PROPERTY( 
    "ml/ann_mlp_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Ml/ANN_MLP.g.cs"
    "cv::ml::ANN_MLP" 
    "ANN_MLP" 
    "TermCriteria;BackpropWeightScale;BackpropMomentumScale;RpropDW0;RpropDWPlus;RpropDWMinus;RpropDWMin;RpropDWMax;AnnealInitialT;AnnealFinalT;AnnealCoolingRatio;AnnealItePerStep" 
    "CvTermCriteria;double;double;double;double;double;double;double;double;double;double;int" 
    "struct;prop;prop;prop;prop;prop;prop;prop;prop;prop;prop;prop"
    "TermCriteria;BackpropWeightScale;BackpropMomentumScale;RpropDW0;RpropDWPlus;RpropDWMinus;RpropDWMin;RpropDWMax;AnnealInitialT;AnnealFinalT;AnnealCoolingRatio;AnnealItePerStep" 
    "MCvTermCriteria;double;double;double;double;double;double;double;double;double;double;int"
    "Termination criteria of the training algorithm;
  BPROP: Strength of the weight gradient term;
  BPROP: Strength of the momentum term (the difference between weights on the 2 previous iterations);
  RPROP: Initial value Delta_0 of update-values Delta_{ij};
  RPROP: Increase factor;
  RPROP: Decrease factor;
  RPROP: Update-values lower limit;
  RPROP: Update-values upper limit;
  ANNEAL: Update initial temperature.;
  ANNEAL: Update final temperature.;
  ANNEAL: Update cooling ratio.;
  ANNEAL: Update iteration per step."
    "Emgu.CV.ML"
    "MlInvoke"
    "ANN_MLP"
	""
    "#include \"ml_c.h\""
	""
	""
	${HAVE_opencv_ml})
  
  CREATE_OCV_CLASS_PROPERTY( 
    "ml/logistic_regression_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Ml/LogisticRegression.g.cs"
    "cv::ml::LogisticRegression" 
    "LogisticRegression" 
    "LearningRate;Iterations;Regularization;TrainMethod;MiniBatchSize;TermCriteria" 
    "double;int;int;int;int;CvTermCriteria" 
    "prop;prop;prop;prop;prop;struct"
    "LearningRate;Iterations;Regularization;TrainMethod;MiniBatchSize;TermCriteria" 
    "double;int;LogisticRegression.RegularizationMethod;LogisticRegression.TrainType;int;MCvTermCriteria;"
    "Learning rate;
  Number of iterations;
  Kind of regularization to be applied;
  Kind of training method to be applied;
  Specifies the number of training samples taken in each step of Mini-Batch Gradient Descent;
  Termination criteria of the algorithm"
    "Emgu.CV.ML"
    "MlInvoke"
    "LogisticRegression"
	""
    "#include \"ml_c.h\""
	""
	""
	${HAVE_opencv_ml})
  
  CREATE_OCV_CLASS_PROPERTY( 
    "ml/rtrees_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Ml/RTrees.g.cs"
    "cv::ml::RTrees" 
    "RTrees" 
    "MaxCategories;MaxDepth;MinSampleCount;CVFolds;UseSurrogates;Use1SERule;TruncatePrunedTree;RegressionAccuracy;CalculateVarImportance;ActiveVarCount;TermCriteria" 
    "int;int;int;int;bool;bool;bool;float;bool;int;CvTermCriteria" 
    "prop;prop;prop;prop;prop;prop;prop;prop;prop;prop;struct"
    "MaxCategories;MaxDepth;MinSampleCount;CVFolds;UseSurrogates;Use1SERule;TruncatePrunedTree;RegressionAccuracy;CalculateVarImportance;ActiveVarCount;TermCriteria" 
    "int;int;int;int;bool;bool;bool;float;bool;int;MCvTermCriteria"
    "Cluster possible values of a categorical variable into K less than or equals maxCategories clusters to find a suboptimal split;
  The maximum possible depth of the tree;
  If the number of samples in a node is less than this parameter then the node will not be split;
  If CVFolds greater than 1 then algorithms prunes the built decision tree using K-fold;
  If true then surrogate splits will be built;
  If true then a pruning will be harsher;
  If true then pruned branches are physically removed from the tree;
  Termination criteria for regression trees;
  If true then variable importance will be calculated;
  The size of the randomly selected subset of features at each tree node and that are used to find the best split(s);
  The termination criteria that specifies when the training algorithm stops"
    "Emgu.CV.ML"
    "MlInvoke"
    "RTrees"
	""
    "#include \"ml_c.h\""
	""
	""
	${HAVE_opencv_ml})
  
  CREATE_OCV_CLASS_PROPERTY( 
    "ml/dtree_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Ml/DTrees.g.cs"
    "cv::ml::DTrees" 
    "DTrees" 
    "MaxCategories;MaxDepth;MinSampleCount;CVFolds;UseSurrogates;Use1SERule;TruncatePrunedTree;RegressionAccuracy" 
    "int;int;int;int;bool;bool;bool;float" 
    "prop;prop;prop;prop;prop;prop;prop;prop"
    "MaxCategories;MaxDepth;MinSampleCount;CVFolds;UseSurrogates;Use1SERule;TruncatePrunedTree;RegressionAccuracy" 
    "int;int;int;int;bool;bool;bool;float"
    "Cluster possible values of a categorical variable into K less than or equals maxCategories clusters to find a suboptimal split;
  The maximum possible depth of the tree;
  If the number of samples in a node is less than this parameter then the node will not be split;
  If CVFolds greater than 1 then algorithms prunes the built decision tree using K-fold;
  If true then surrogate splits will be built;
  If true then a pruning will be harsher;
  If true then pruned branches are physically removed from the tree;
  Termination criteria for regression trees"
    "Emgu.CV.ML"
    "MlInvoke"
    "DTrees"
	""
    "#include \"ml_c.h\""
	""
	""
	${HAVE_opencv_ml})
  
  CREATE_OCV_CLASS_PROPERTY( 
    "ml/boost_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Ml/Boost.g.cs"
    "cv::ml::Boost" 
    "Boost" 
    "MaxCategories;MaxDepth;MinSampleCount;CVFolds;UseSurrogates;Use1SERule;TruncatePrunedTree;RegressionAccuracy" 
    "int;int;int;int;bool;bool;bool;float" 
    "prop;prop;prop;prop;prop;prop;prop;prop"
    "MaxCategories;MaxDepth;MinSampleCount;CVFolds;UseSurrogates;Use1SERule;TruncatePrunedTree;RegressionAccuracy" 
    "int;int;int;int;bool;bool;bool;float"
    "Cluster possible values of a categorical variable into K less than or equals maxCategories clusters to find a suboptimal split;
  The maximum possible depth of the tree;
  If the number of samples in a node is less than this parameter then the node will not be split;
  If CVFolds greater than 1 then algorithms prunes the built decision tree using K-fold;
  If true then surrogate splits will be built;
  If true then a pruning will be harsher;
  If true then pruned branches are physically removed from the tree;
  Termination criteria for regression trees"
    "Emgu.CV.ML"
    "MlInvoke"
    "Boost"
	""
    "#include \"ml_c.h\""
	""
	""
	${HAVE_opencv_ml})

############################### ml code gen END ################################

############################### video code gen START ##############################
IF (NOT HAVE_opencv_video)
  SET(HAVE_opencv_video FALSE)
ENDIF()
  CREATE_OCV_CLASS_PROPERTY( 
    "video/kalmanfilter_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Video/KalmanFilter.g.cs"
    "cv::KalmanFilter" 
    "KalmanFilter"  "statePre;statePost;transitionMatrix;controlMatrix;measurementMatrix;processNoiseCov;measurementNoiseCov;errorCovPre;gain;errorCovPost" 
    "cv::Mat;cv::Mat;cv::Mat;cv::Mat;cv::Mat;cv::Mat;cv::Mat;cv::Mat;cv::Mat;cv::Mat" 
    "element;element;element;element;element;element;element;element;element;element"
    "StatePre;StatePost;TransitionMatrix;ControlMatrix;MeasurementMatrix;ProcessNoiseCov;MeasurementNoiseCov;ErrorCovPre;Gain;ErrorCovPost" 
    "Mat;Mat;Mat;Mat;Mat;Mat;Mat;Mat;Mat;Mat"
    "Predicted state (x'(k)): x(k)=A*x(k-1)+B*u(k);
  Corrected state (x(k)): x(k)=x'(k)+K(k)*(z(k)-H*x'(k));
  State transition matrix (A);
  Control matrix (B) (not used if there is no control);
  Measurement matrix (H);
  Process noise covariance matrix (Q);
  Measurement noise covariance matrix (R);
  priori error estimate covariance matrix (P'(k)): P'(k)=A*P(k-1)*At + Q);
  Kalman gain matrix (K(k)): K(k)=P'(k)*Ht*inv(H*P'(k)*Ht+R);
  posteriori error estimate covariance matrix (P(k)): P(k)=(I-K(k)*H)*P'(k)"
    "Emgu.CV"
    "CvInvoke"
    "KalmanFilter"
	""
    "#include \"video_c.h\""
	""
	""
	${HAVE_opencv_video})

  CREATE_OCV_CLASS_PROPERTY( 
    "video/variational_refinement_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Video/VariationalRefinement.g.cs"
    "cv::VariationalRefinement" 
    "VariationalRefinement" 
    "FixedPointIterations;SorIterations;Omega;Alpha;Delta;Gamma" 
    "int;int;float;float;float;float" 
    "prop;prop;prop;prop;prop;prop"
    "FixedPointIterations;SorIterations;Omega;Alpha;Delta;Gamma" 
    "int;int;float;float;float;float"
    "Number of outer (fixed-point) iterations in the minimization procedure.;
	Number of inner successive over-relaxation (SOR) iterations in the minimization procedure to solve the respective linear system.;
	Relaxation factor in SOR;
	Weight of the smoothness term;
	Weight of the color constancy term;
	Weight of the gradient constancy term"
    "Emgu.CV"
    "CvInvoke"
    "VariationalRefinement"
	""
    "#include \"video_c.h\""
	""
	""
	${HAVE_opencv_video})

############################### video code gen END ################################

############################### videoio code gen START ##############################

  CREATE_OCV_CLASS_PROPERTY( 
    "videoio/video_capture_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Videoio/VideoCapture.g.cs"
    "cv::VideoCapture" 
    "VideoCapture" 
    "isOpened;ExceptionMode" 
    "bool;bool" 
    "val;prop"
    "IsOpened;ExceptionMode" 
    "bool;bool"
    "True if the camera is opened;
    If True, methods raise exceptions if not successful instead of returning an error code"
    "Emgu.CV"
    "CvInvoke"
    "VideoCapture"
	""
    "#include \"videoio_c_extra.h\""
	""
	""
	${HAVE_opencv_videoio})

############################### videoio code gen END ################################

############################### optflow code gen START ##############################
IF (NOT HAVE_opencv_optflow)
  SET(HAVE_opencv_optflow FALSE)
ENDIF()

 CREATE_OCV_CLASS_PROPERTY( 
    "optflow/dual_tvl1_opticalflow_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Optflow/DualTVL1OpticalFlow.g.cs"
    "cv::optflow::DualTVL1OpticalFlow" 
    "DualTVL1OpticalFlow" 
    "Tau;Lambda;Theta;Gamma;ScalesNumber;WarpingsNumber;Epsilon;InnerIterations;OuterIterations;UseInitialFlow;ScaleStep;MedianFiltering" 
    "double;double;double;double;int;int;double;int;int;bool;double;int" 
    "prop;prop;prop;prop;prop;prop;prop;prop;prop;prop;prop;prop"
    "Tau;Lambda;Theta;Gamma;ScalesNumber;WarpingsNumber;Epsilon;InnerIterations;OuterIterations;UseInitialFlow;ScaleStep;MedianFiltering" 
    "double;double;double;double;int;int;double;int;int;bool;double;int"
    "Time step of the numerical scheme;
  Weight parameter for the data term, attachment parameter;
  Weight parameter for (u - v)^2, tightness parameter;
  Coefficient for additional illumination variation term;
  Number of scales used to create the pyramid of images;
  Number of warpings per scale;
  Stopping criterion threshold used in the numerical scheme, which is a trade-off between precision and running time;
  Inner iterations (between outlier filtering) used in the numerical scheme;
  Outer iterations (number of inner loops) used in the numerical scheme;
  Use initial flow;
  Step between scales (less than 1);
  Median filter kernel size (1 = no filter) (3 or 5)"
    "Emgu.CV"
    "CvInvoke"
    "DualTVL1OpticalFlow"
	""
    "#include \"optflow_c.h\""
	""
	""
	${HAVE_opencv_optflow})

CREATE_OCV_CLASS_PROPERTY( 
    "optflow/rlof_opticalflow_parameter_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Optflow/RLOFOpticalFlowParameter.g.cs"
    "cv::optflow::RLOFOpticalFlowParameter" 
    "RLOFOpticalFlowParameter" 
    "NormSigma0;NormSigma1;SolverType;SupportRegionType;SmallWinSize;LargeWinSize;CrossSegmentationThreshold;MaxLevel;UseInitialFlow;UseIlluminationModel;UseGlobalMotionPrior;MaxIteration;MinEigenValue;GlobalMotionRansacThreshold" 
    "float;float;cv::optflow::SolverType;cv::optflow::SupportRegionType;int;int;int;int;bool;bool;bool;int;float;float" 
    "prop;prop;prop;prop;prop;prop;prop;prop;prop;prop;prop;prop;prop;prop"
    "NormSigma0;NormSigma1;Solver;SupportRegion;SmallWinSize;LargeWinSize;CrossSegmentationThreshold;MaxLevel;UseInitialFlow;UseIlluminationModel;UseGlobalMotionPrior;MaxIteration;MinEigenValue;GlobalMotionRansacThreshold" 
    "float;float;Emgu.CV.RLOFOpticalFlowParameter.SolverType;Emgu.CV.RLOFOpticalFlowParameter.SupportRegionType;int;int;int;int;bool;bool;bool;int;float;float"
    "parameter of the shrinked Hampel norm;
	parameter of the shrinked Hampel norm;
	Variable specifies the iterative refinement strategy;
	Variable specifies the support region shape extraction or shrinking strategy;
	Minimal window size of the support region. This parameter is only used if supportRegionType is Cross;
	Maximal window size of the support region. If supportRegionType is Fixed this gives the exact support region size. The speed of the RLOF is related to the applied win sizes. The smaller the window size the lower is the runtime, but the more sensitive to noise is the method.;
	Color similarity threshold used by cross-based segmentation. Only used  if supportRegionType is Cross. With the cross-bassed segmentation motion boundaries can be computed more accurately;
	Maximal number of pyramid level used. The large this value is the more likely it is to obtain accurate solutions for long-range motions. The runtime is linear related to this parameter;
	Use next point list as initial values. A good initialization can improve the algorithm accuracy and reduce the runtime by a faster convergence of the iteration refinement;
	Use the Gennert and Negahdaripour illumination model instead of the intensity brightness constraint.;
	Use global motion prior initialisation. It allows to be more accurate for long-range motion. The computational complexity is slightly increased by enabling the global motion prior initialisation.;
	Number of maximal iterations used for the iterative refinement. Lower values can reduce the runtime but also the accuracy.;
	Threshold for the minimal eigenvalue of the gradient matrix defines when to abort the iterative refinement.;
	To apply the global motion prior motion vectors will be computed on a regularly sampled which are the basis for Homography estimation using RANSAC. The reprojection threshold is based on n-th percentil (given by this value [0 ... 100]) of the motion vectors magnitude. "
    "Emgu.CV"
    "CvInvoke"
    "RLOFOpticalFlowParameter"
	""
    "#include \"optflow_c.h\""
	""
	""
	${HAVE_opencv_optflow})  

############################### optflow code gen END ################################

############################### photo code gen START ##############################
IF (NOT HAVE_opencv_photo)
  SET(HAVE_opencv_photo FALSE)
ENDIF()

  CREATE_OCV_CLASS_PROPERTY( 
    "photo/Tonemap_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Photo/Tonemap.g.cs"
    "cv::Tonemap" 
    "Tonemap" 
    "Gamma" 
    "float" 
    "prop"
    "Gamma" 
    "float"
    "Positive value for gamma correction. Gamma value of 1.0 implies no correction, gamma equal to 2.2f is suitable for most displays."
    "Emgu.CV"
    "CvInvoke"
    "Tonemap"
	""
    "#include \"photo_c.h\""
	""
	""
	${HAVE_opencv_photo})
		
  CREATE_OCV_CLASS_PROPERTY( 
    "photo/TonemapReinhard_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Photo/TonemapReinhard.g.cs"
    "cv::TonemapReinhard" 
    "TonemapReinhard" 
    "Intensity;LightAdaptation;ColorAdaptation" 
    "float;float;float" 
    "prop;prop;prop"
    "Intensity;LightAdaptation;ColorAdaptation" 
    "float;float;float"
    "Result intensity in [-8, 8] range. Greater intensity produces brighter results.;
	Light adaptation in [0, 1] range. If 1 adaptation is based only on pixel value, if 0 it is global, otherwise it is a weighted mean of this two cases.;
	chromatic adaptation in [0, 1] range. If 1 channels are treated independently, if 0 adaptation level is the same for each channel."
    "Emgu.CV"
    "CvInvoke"
    "TonemapReinhard"
	""
    "#include \"photo_c.h\""
	""
	""
	${HAVE_opencv_photo})	
	
  CREATE_OCV_CLASS_PROPERTY( 
    "photo/TonemapDrago_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Photo/TonemapDrago.g.cs"
    "cv::TonemapDrago" 
    "TonemapDrago" 
    "Saturation;Bias" 
    "float;float" 
    "prop;prop"
    "Saturation;Bias" 
    "float;float"
    "Positive saturation enhancement value. 1.0 preserves saturation, values greater than 1 increase saturation and values less than 1 decrease it.;
	Value for bias function in [0, 1] range. Values from 0.7 to 0.9 usually give best results, default value is 0.85."
    "Emgu.CV"
    "CvInvoke"
    "TonemapDrago"
	""
    "#include \"photo_c.h\""
	""
	""
	${HAVE_opencv_photo})		
	
  CREATE_OCV_CLASS_PROPERTY( 
    "photo/TonemapMantiuk_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Photo/TonemapMantiuk.g.cs"
    "cv::TonemapMantiuk" 
    "TonemapMantiuk" 
    "Saturation;Scale" 
    "float;float" 
    "prop;prop"
    "Saturation;Scale" 
    "float;float"
    "Saturation enhancement value.;
	Contrast scale factor. HVS response is multiplied by this parameter, thus compressing dynamic range. Values from 0.6 to 0.9 produce best results."
    "Emgu.CV"
    "CvInvoke"
    "TonemapMantiuk"
	""
    "#include \"photo_c.h\""
	""
	""
	${HAVE_opencv_photo})	

############################### photo code gen END ##############################


############################### xfeatures2d code gen START ##############################
IF (NOT HAVE_opencv_xfeatures2d)
  SET(HAVE_opencv_xfeatures2d FALSE)
ENDIF()

   CREATE_OCV_CLASS_PROPERTY( 
    "xfeatures2d/pct_compute_signature_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/XFeatures2D/PCTComputeSignature.g.cs"
    "cv::xfeatures2d::PCTSignatures" 
    "PCTSignatures" 
    "GrayscaleBits;WindowRadius;WeightX;WeightY;WeightL;WeightA;WeightB;WeightEntropy;IterationCount;MaxClustersCount;ClusterMinSize;JoiningDistance;DropThreshold;DistanceFunction" 
    "int;int;float;float;float;float;float;float;int;int;int;float;float;int" 
    "prop;prop;prop;prop;prop;prop;prop;prop;prop;prop;prop;prop;prop;prop;prop"
    "GrayscaleBits;WindowRadius;WeightX;WeightY;WeightL;WeightA;WeightB;WeightEntropy;IterationCount;MaxClustersCount;ClusterMinSize;JoiningDistance;DropThreshold;DistanceFunction" 
    "int;int;float;float;float;float;float;float;int;int;int;float;float;PCTSignatures.PointDistributionType"
    "Color resolution of the greyscale bitmap represented in allocated bits (i.e., value 4 means that 16 shades of grey are used). The greyscale bitmap is used for computing contrast and entropy values.;
	Size of the texture sampling window used to compute contrast and entropy. (center of the window is always in the pixel selected by x,y coordinates of the corresponding feature sample).;
	Weights (multiplicative constants) that linearly stretch individual axes of the feature space. (x,y = position. L,a,b = color in CIE Lab space. c = contrast. e = entropy);
	Weights (multiplicative constants) that linearly stretch individual axes of the feature space. (x,y = position. L,a,b = color in CIE Lab space. c = contrast. e = entropy);
	Weights (multiplicative constants) that linearly stretch individual axes of the feature space. (x,y = position. L,a,b = color in CIE Lab space. c = contrast. e = entropy);
	Weights (multiplicative constants) that linearly stretch individual axes of the feature space. (x,y = position. L,a,b = color in CIE Lab space. c = contrast. e = entropy);
	Weights (multiplicative constants) that linearly stretch individual axes of the feature space. (x,y = position. L,a,b = color in CIE Lab space. c = contrast. e = entropy);
	Weights (multiplicative constants) that linearly stretch individual axes of the feature space. (x,y = position. L,a,b = color in CIE Lab space. c = contrast. e = entropy);
	Number of iterations of the k-means clustering. We use fixed number of iterations, since the modified clustering is pruning clusters (not iteratively refining k clusters).;
	Maximal number of generated clusters. If the number is exceeded, the clusters are sorted by their weights and the smallest clusters are cropped.;
	This parameter multiplied by the index of iteration gives lower limit for cluster size. Clusters containing fewer points than specified by the limit have their centroid dismissed and points are reassigned.;
	Threshold euclidean distance between two centroids. If two cluster centers are closer than this distance, one of the centroid is dismissed and points are reassigned.;
	Remove centroids in k-means whose weight is lesser or equal to given threshold.;
	Distance function selector used for measuring distance between two points in k-means."
    "Emgu.CV.XFeatures2D"
    "XFeatures2DInvoke"
    "PCTSignatures"
	""
    "#include \"xfeatures2d_c.h\""
	""
	""
	${HAVE_opencv_xfeatures2d})

############################### xfeatures2d code gen END ################################

############################### stitching code gen START ##############################
IF (NOT HAVE_opencv_stitching)
  SET(HAVE_opencv_stitching FALSE)
ENDIF()

   CREATE_OCV_CLASS_PROPERTY( 
    "stitching/stitching_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Stitching/Stitcher.g.cs"
    "cv::Stitcher" 
    "Stitcher" 
    "workScale" 
    "double" 
    "val"
    "WorkScale" 
    "double"
    "The work scale"
    "Emgu.CV.Stitching"
    "StitchingInvoke"
    "Stitcher"
	""
    "#include \"stitching_c.h\""
	""
	""
	${HAVE_opencv_stitching})  

############################### stitching code gen END ################################

############################### video code gen START ##############################
IF (NOT HAVE_opencv_video)
  SET(HAVE_opencv_video FALSE)
ENDIF()

  CREATE_OCV_CLASS_PROPERTY( 
    "video/disopticalflow_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Video/DISOpticalFlow.g.cs"
    "cv::DISOpticalFlow" 
    "DISOpticalFlow" 
    "FinestScale;PatchSize;PatchStride;GradientDescentIterations;VariationalRefinementIterations;VariationalRefinementAlpha;VariationalRefinementDelta;VariationalRefinementGamma;UseMeanNormalization;UseSpatialPropagation" 
    "int;int;int;int;int;float;float;float;bool;bool" 
    "prop;prop;prop;prop;prop;prop;prop;prop;prop;prop"
    "FinestScale;PatchSize;PatchStride;GradientDescentIterations;VariationalRefinementIterations;VariationalRefinementAlpha;VariationalRefinementDelta;VariationalRefinementGamma;UseMeanNormalization;UseSpatialPropagation" 
    "int;int;int;int;int;float;float;float;bool;bool"
    "Finest level of the Gaussian pyramid on which the flow is computed (zero level corresponds to the original image resolution). The final flow is obtained by bilinear upscaling.;
	Size of an image patch for matching (in pixels). Normally, default 8x8 patches work well enough in most cases.;
	Stride between neighbor patches. Must be less than patch size. Lower values correspond to higher flow quality.;
	Maximum number of gradient descent iterations in the patch inverse search stage. Higher values may improve quality in some cases.;
	Number of fixed point iterations of variational refinement per scale. Set to zero to disable variational refinement completely. Higher values will typically result in more smooth and high-quality flow.;
	Weight of the smoothness term;
	Weight of the color constancy term;
	Weight of the gradient constancy term;
	Whether to use mean-normalization of patches when computing patch distance. It is turned on by default as it typically provides a noticeable quality boost because of increased robustness to illumination variations. Turn it off if you are certain that your sequence doesn't contain any changes in illumination.;
	Whether to use spatial propagation of good optical flow vectors. This option is turned on by default, as it tends to work better on average and can sometimes help recover from major errors introduced by the coarse-to-fine scheme employed by the DIS optical flow algorithm. Turning this option off can make the output flow field a bit smoother, however."
    "Emgu.CV"
    "CvInvoke"
    "DISOpticalFlow"
	""
    "#include \"video_c.h\""
	""
	""
	${HAVE_opencv_video})

   CREATE_OCV_CLASS_PROPERTY( 
    "video/BackgroundSubtractorKNN_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Video/BackgroundSubtractorKNN.g.cs"
    "cv::BackgroundSubtractorKNN" 
    "BackgroundSubtractorKNN" 
    "History;NSamples;Dist2Threshold;kNNSamples;DetectShadows;ShadowValue;ShadowThreshold" 
    "int;int;double;int;bool;int;double" 
    "prop;prop;prop;prop;prop;prop;prop"
    "History;NSamples;Dist2Threshold;KNNSamples;DetectShadows;ShadowValue;ShadowThreshold" 
    "int;int;double;int;bool;int;double"
    "The number of last frames that affect the background model;
	The number of data samples in the background model;
	The threshold on the squared distance between the pixel and the sample to decide whether a pixel is close to a data sample.;
	The number of neighbours, the k in the kNN. K is the number of samples that need to be within dist2Threshold in order to decide that pixel is matching the kNN background model.;
	If true, the algorithm detects shadows and marks them.;
	Shadow value is the value used to mark shadows in the foreground mask. Default value is 127. Value 0 in the mask always means background, 255 means foreground.;
	A shadow is detected if pixel is a darker version of the background. The shadow threshold (Tau in the paper) is a threshold defining how much darker the shadow can be. Tau= 0.5 means that if a pixel is more than twice darker then it is not shadow."
    "Emgu.CV"
    "CvInvoke"
    "BackgroundSubtractorKNN"
	""
    "#include \"video_c.h\""
	""
	""
	${HAVE_opencv_video})
  
   CREATE_OCV_CLASS_PROPERTY( 
    "video/BackgroundSubtractorMOG2_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Video/BackgroundSubtractorMOG2.g.cs"
    "cv::BackgroundSubtractorMOG2" 
    "BackgroundSubtractorMOG2" 
    "History;DetectShadows;ShadowValue;ShadowThreshold;NMixtures;BackgroundRatio;VarThreshold;VarThresholdGen;VarInit;VarMin;VarMax;ComplexityReductionThreshold" 
    "int;bool;int;double;int;double;double;double;double;double;double;double" 
    "prop;prop;prop;prop;prop;prop;prop;prop;prop;prop;prop;prop"
    "History;DetectShadows;ShadowValue;ShadowThreshold;NMixtures;BackgroundRatio;VarThreshold;VarThresholdGen;VarInit;VarMin;VarMax;ComplexityReductionThreshold" 
    "int;bool;int;double;int;double;double;double;double;double;double;double"
    "The number of last frames that affect the background model;
	If true, the algorithm detects shadows and marks them.;
	Shadow value is the value used to mark shadows in the foreground mask. Default value is 127. Value 0 in the mask always means background, 255 means foreground.;
	A shadow is detected if pixel is a darker version of the background. The shadow threshold (Tau in the paper) is a threshold defining how much darker the shadow can be. Tau= 0.5 means that if a pixel is more than twice darker then it is not shadow.;
	The number of gaussian components in the background model;
	If a foreground pixel keeps semi-constant value for about backgroundRatio * history frames, it's considered background and added to the model as a center of a new component. It corresponds to TB parameter in the paper.;
	The main threshold on the squared Mahalanobis distance to decide if the sample is well described by the background model or not. Related to Cthr from the paper.;
	The variance threshold for the pixel-model match used for new mixture component generation. Threshold for the squared Mahalanobis distance that helps decide when a sample is close to the existing components (corresponds to Tg in the paper). If a pixel is not close to any component, it is considered foreground or added as a new component. 3 sigma =%gt; Tg=3*3=9 is default. A smaller Tg value generates more components. A higher Tg value may result in a small number of components but they can grow too large.;
	The initial variance of each gaussian component;
	The minimum variance;
	The maximum variance;
	the complexity reduction threshold. This parameter defines the number of samples needed to accept to prove the component exists. CT=0.05 is a default value for all the samples. By setting CT=0 you get an algorithm very similar to the standard Stauffer &amp; Grimson algorithm."
    "Emgu.CV"
    "CvInvoke"
    "BackgroundSubtractorMOG2"
	""
    "#include \"video_c.h\""
	""
	""
	${HAVE_opencv_video})

############################### video code gen END ################################

############################### shape code gen START ##############################
IF (NOT HAVE_opencv_shape)
  SET(HAVE_opencv_shape FALSE)
ENDIF()
   CREATE_OCV_CLASS_PROPERTY( 
    "shape/ShapeContextDistanceExtractor_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Shape/ShapeContextDistanceExtractor.g.cs"
    "cv::ShapeContextDistanceExtractor" 
    "ShapeContextDistanceExtractor" 
    "Iterations;AngularBins;RadialBins;InnerRadius;OuterRadius;RotationInvariant;ShapeContextWeight;ImageAppearanceWeight;BendingEnergyWeight;StdDev" 
    "int;int;int;float;float;bool;float;float;float;float" 
    "prop;prop;prop;prop;prop;prop;prop;prop;prop;prop"
    "Iterations;AngularBins;RadialBins;InnerRadius;OuterRadius;RotationInvariant;ShapeContextWeight;ImageAppearanceWeight;BendingEnergyWeight;StdDev" 
    "int;int;int;float;float;bool;float;float;float;float"
    "The number of iterations;
    The number of angular bins in the shape context descriptor.;
    The number of radial bins in the shape context descriptor.;
    The value of the inner radius.;
    The value of the outer radius.;
    Rotation Invariant;
    The weight of the shape context distance in the final distance value.;
    The weight of the appearance cost in the final distance value.;
    The weight of the Bending Energy in the final distance value.;
    Standard Deviation."
    "Emgu.CV.Shape"
    "ShapeInvoke"
    "ShapeContextDistanceExtractor"
	""
    "#include \"shape_c.h\""
	""
	""
	${HAVE_opencv_shape})

############################### shape code gen END ################################

############################### cudaimgproc code gen START ##############################
IF (NOT HAVE_opencv_cudaimgproc)
  SET(HAVE_opencv_cudaimgproc FALSE)
ENDIF()
  CREATE_OCV_CLASS_PROPERTY( 
    "cudaimgproc/cuda_hough_lines_detector_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Cuda/Imgproc/CudaHoughLinesDetector.g.cs"
    "cv::cuda::HoughLinesDetector" 
    "CudaHoughLinesDetector" 
    "Rho;Theta;Threshold;DoSort;MaxLines" 
    "float;float;int;bool;int" 
    "prop;prop;prop;Prop;Prop"
    "Rho;Theta;Threshold;DoSort;MaxLines" 
    "float;float;int;bool;int"
    "Distance resolution of the accumulator in pixels;
  Angle resolution of the accumulator in radians;
  Accumulator threshold parameter. Only those lines are returned that get enough;
  Performs lines sort by votes;
  Maximum number of output lines"
    "Emgu.CV.Cuda"
    "CudaInvoke"
    "CudaHoughLinesDetector"
	""
    "#include \"cudaimgproc_c.h\""
	""
	""
	${HAVE_opencv_cudaimgproc})

############################### cudaimgproc code gen END ##############################

############################### cudaobjdetect code gen START ##############################
IF (NOT HAVE_opencv_cudaobjdetect)
  SET(HAVE_opencv_cudaobjdetect FALSE)
ENDIF()

  CREATE_OCV_CLASS_PROPERTY( 
    "cudaobjdetect/cuda_hog_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Cuda/Objdetect/CudaHOG.g.cs"
    "cv::cuda::HOG" 
    "CudaHOG"
    "GammaCorrection;WinSigma;NumLevels;GroupThreshold;HitThreshold;ScaleFactor;L2HysThreshold;DescriptorFormat;DescriptorSize;WinStride;BlockHistogramSize" 
    "bool;double;int;int;double;double;double;int;size_t;CvSize;size_t" 
    "prop;prop;prop;prop;prop;prop;prop;propR;propR;struct;propR"
    "GammaCorrection;WinSigma;NumLevels;GroupThreshold;HitThreshold;ScaleFactor;L2HysThreshold;DescriptorFormat;DescriptorSize;WinStride;BlockHistogramSize" 
    "bool;double;int;int;double;double;double;CudaHOG.DescrFormat;IntPtr;System.Drawing.Size;IntPtr"
    "Flag to specify whether the gamma correction preprocessing is required or not;
    Gaussian smoothing window parameter;
    Maximum number of detection window increases;
    Coefficient to regulate the similarity threshold. When detected, some objects can be covered by many rectangles. 0 means not to perform grouping. See groupRectangles.;
    Threshold for the distance between features and SVM classifying plane. Usually it is 0 and should be specfied in the detector coefficients (as the last free coefficient). But if the free coefficient is omitted (which is allowed), you can specify it manually here.;
    Coefficient of the detection window increase.;
    L2-Hys normalization method shrinkage.;
    The descriptor format;
    Returns the number of coefficients required for the classification.;
    Window stride. It must be a multiple of block stride.;
    Returns the block histogram size."
    "Emgu.CV.Cuda"
    "CudaInvoke"
    "CudaHOG"
	""
    "#include \"cudaobjdetect_c.h\""
	""
	""
	${HAVE_opencv_cudaobjdetect})

  CREATE_OCV_CLASS_PROPERTY( 
    "cudaobjdetect/cuda_cascade_classifier_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Cuda/Objdetect/CudaCascadeClassifier.g.cs"
    "cv::cuda::CascadeClassifier"
    "CudaCascadeClassifier"
    "ScaleFactor;MinNeighbors;MaxNumObjects;FindLargestObject;MaxObjectSize;MinObjectSize;ClassifierSize" 
    "double;int;int;bool;CvSize;CvSize;CvSize" 
    "prop;prop;prop;prop;struct;struct;structR"
    "ScaleFactor;MinNeighbors;MaxNumObjects;FindLargestObject;MaxObjectSize;MinObjectSize;ClassifierSize" 
    "double;int;int;bool;System.Drawing.Size;System.Drawing.Size;System.Drawing.Size"
    "Parameter specifying how much the image size is reduced at each image scale;
  Parameter specifying how many neighbors each candidate rectangle should have to retain it;
  The maximum number of objects;
  If true, only return the largest object;
  The maximum object size;
  The minimum object size;
  The classifier size"
    "Emgu.CV.Cuda"
    "CudaInvoke"
    "CudaCascadeClassifier"
	""
    "#include \"cudaobjdetect_c.h\""
	""
	""
	${HAVE_opencv_cudaobjdetect})

############################### cudaobjdetect code gen END ##############################

############################### plot code gen START ##############################
IF (NOT HAVE_opencv_plot)
  SET(HAVE_opencv_plot FALSE)
ENDIF()
  CREATE_OCV_CLASS_PROPERTY( 
    "plot/plot2d_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/Plot/Plot2d.g.cs"
    "cv::plot::Plot2d" 
    "Plot2d" 
    "MinX;MinY;MaxX;MaxY;PlotLineWidth;GridLinesNumber;PointIdxToPrint;InvertOrientation;ShowText;ShowGrid;NeedPlotLine" 
    "double;double;double;double;int;int;int;bool;bool;bool;bool" 
    "propW;propW;propW;propW;propW;propW;propW;propW;propW;propW;propW"
    "MinX;MinY;MaxX;MaxY;PlotLineWidth;GridLinesNumber;PointIdxToPrint;InvertOrientation;ShowText;ShowGrid;NeedPlotLine" 
    "double;double;double;double;int;int;int;bool;bool;bool;bool"
    "Min X;
    Min Y;
    Max X;
    Max Y;
    Plot line width;
    Grid Lines Number;
    Sets the index of a point which coordinates will be printed on the top left corner of the plot (if ShowText flag is true);
    Invert Orientation;
    Show Text;
    Show Grid;
    Need Plot Line"
    "Emgu.CV.Plot"
    "PlotInvoke"
    "Plot2d"
	""
    "#include \"plot_c.h\""
	""
	""
	${HAVE_opencv_plot})

############################### plot code gen END ##############################

############################### saliency code gen START ##############################
IF (NOT HAVE_opencv_saliency)
  SET(HAVE_opencv_saliency FALSE)
ENDIF()
  CREATE_OCV_CLASS_PROPERTY( 
    "saliency/MotionSaliencyBinWangApr2014_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/Saliency/MotionSaliencyBinWangApr2014.g.cs"
    "cv::saliency::MotionSaliencyBinWangApr2014" 
    "MotionSaliencyBinWangApr2014" 
    "ImageWidth;ImageHeight;init" 
    "int;int;bool" 
    "prop;prop;act"
    "ImageWidth;ImageHeight;Init" 
    "int;int;bool"
    "Image width;
    Image height;
	This function allows the correct initialization of all data structures that will be used by the algorithm.
	"
    "Emgu.CV.Saliency"
    "SaliencyInvoke"
    "MotionSaliencyBinWangApr2014"
	""
    "#include \"saliency_c.h\""
	""
	""
	${HAVE_opencv_saliency})
  CREATE_OCV_CLASS_PROPERTY( 
    "saliency/ObjectnessBING_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/Saliency/ObjectnessBING.g.cs"
    "cv::saliency::ObjectnessBING" 
    "ObjectnessBING" 
    "W;NSS" 
    "int;int" 
    "prop;prop"
    "W;NSS" 
    "int;int"
    "W;
    NSS"
    "Emgu.CV.Saliency"
    "SaliencyInvoke"
    "ObjectnessBING"
	""
    "#include \"saliency_c.h\""
	""
	""
	${HAVE_opencv_saliency})

############################### plot code gen END ##############################

############################### xphoto code gen START ##############################
IF (NOT HAVE_opencv_xphoto)
  SET(HAVE_opencv_xphoto FALSE)
ENDIF()
  CREATE_OCV_CLASS_PROPERTY( 
    "xphoto/simplewb_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/XPhoto/SimpleWB.g.cs"
    "cv::xphoto::SimpleWB" 
    "SimpleWB" 
    "InputMin;InputMax;OutputMin;OutputMax;P" 
    "float;float;float;float;float" 
    "prop;prop;prop;prop;prop"
    "InputMin;InputMax;OutputMin;OutputMax;P" 
    "float;float;float;float;float"
    "Input image range minimum value;
	Input image range maximum value;
	Output image range minimum value;
	Output image range maximum value;
	Percent of top/bottom values to ignore"
    "Emgu.CV.XPhoto"
    "XPhotoInvoke"
    "SimpleWB"
	""
    "#include \"xphoto_c.h\""
	""
	""
	${HAVE_opencv_xphoto})
	
  CREATE_OCV_CLASS_PROPERTY( 
    "xphoto/grayworldwb_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/XPhoto/GrayworldWB.g.cs"
    "cv::xphoto::GrayworldWB" 
    "GrayworldWB" 
    "SaturationThreshold" 
    "float" 
    "prop"
    "SaturationThreshold" 
    "float"
    "Maximum saturation for a pixel to be included in the gray-world assumption"
    "Emgu.CV.XPhoto"
    "XPhotoInvoke"
    "GrayworldWB"
	""
    "#include \"xphoto_c.h\""
	""
	""
	${HAVE_opencv_xphoto})

	CREATE_OCV_CLASS_PROPERTY( 
    "xphoto/learningbasedwb_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/XPhoto/LearningBasedWB.g.cs"
    "cv::xphoto::LearningBasedWB" 
    "LearningBasedWB" 
    "RangeMaxVal;SaturationThreshold;HistBinNum" 
    "int;float;int" 
    "prop;prop;prop"
    "RangeMaxVal;SaturationThreshold;HistBinNum" 
    "int;float;int"
    "Maximum possible value of the input image (e.g. 255 for 8 bit images, 4095 for 12 bit images);
	Threshold that is used to determine saturated pixels, i.e. pixels where at least one of the channels exceeds saturation_threshold x range_max_val are ignored.;
	Defines the size of one dimension of a three-dimensional RGB histogram that is used internally by the algorithm. It often makes sense to increase the number of bins for images with higher bit depth (e.g. 256 bins for a 12 bit image).
	"
    "Emgu.CV.XPhoto"
    "XPhotoInvoke"
    "LearningBasedWB"
	""
    "#include \"xphoto_c.h\""
	""
	""
	${HAVE_opencv_xphoto})

  CREATE_OCV_CLASS_PROPERTY( 
    "xphoto/TonemapDurand_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/XPhoto/TonemapDurand.g.cs"
    "cv::xphoto::TonemapDurand" 
    "TonemapDurand" 
    "Saturation;Contrast;SigmaSpace;SigmaColor" 
    "float;float;float;float" 
    "prop;prop;prop;prop"
    "Saturation;Contrast;SigmaSpace;SigmaColor" 
    "float;float;float;float"
    "Positive saturation enhancement value. 1.0 preserves saturation, values greater than 1 increase saturation and values less than 1 decrease it.;
	Resulting contrast on logarithmic scale, i. e. log(max / min), where max and min are maximum and minimum luminance values of the resulting image.;
	Bilateral filter sigma in color space;
	bilateral filter sigma in coordinate space"
    "Emgu.CV.XPhoto"
    "XPhotoInvoke"
    "TonemapDurand"
	""
    "#include \"xphoto_c.h\""
	""
	""
	${HAVE_opencv_xphoto})

############################### xphoto code gen END ##############################

############################### face code gen START ##############################
IF (NOT HAVE_opencv_face)
  SET(HAVE_opencv_face FALSE)
ENDIF()
  CREATE_OCV_CLASS_PROPERTY( 
    "face/facemarklbf_params_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/Face/FacemarkLBFParams.g.cs"
    "cv::face::FacemarkLBF::Params" 
    "FacemarkLBFParams" 
    "shape_offset;verbose;n_landmarks;initShape_n;stages_n;tree_n;tree_depth;bagging_overlap;save_model;cascade_face;model_filename" 
    "double;bool;int;int;int;int;int;double;bool;cv::String;cv::String" 
    "element;element;element;element;element;element;element;element;element;element;element"
    "ShapeOffset;Verbose;NLandmarks;InitShapeN;StagesN;TreeN;TreeDepth;BaggingOverlap;SaveModel;CascadeFace;ModelFile" 
    "double;bool;int;int;int;int;int;double;bool;String;String"
    "offset for the loaded face landmark points;
	show the training print-out;
	number of landmark points;
	multiplier for augment the training data;
	number of refinement stages;
	number of tree in the model for each landmark point refinement;
	the depth of decision tree, defines the size of feature;
	overlap ratio for training the LBF feature;
	flag to save the trained model or not;
	filename of the face detector model;
	filename where the trained model will be saved"
    "Emgu.CV.Face"
    "FaceInvoke"
    "FacemarkLBFParams"
	""
    "#include \"face_c.h\""
	""
	""
	${HAVE_opencv_face})

  CREATE_OCV_CLASS_PROPERTY( 
    "face/facemarkaam_params_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/Face/FacemarkAAMParams.g.cs"
    "cv::face::FacemarkAAM::Params" 
    "FacemarkAAMParams" 
    "model_filename;m;n;n_iter;verbose;save_model;max_m;max_n" 
    "cv::String;int;int;int;bool;bool;int;int" 
    "element;element;element;element;element;element;element;element"
    "ModelFile;M;N;NIter;Verbose;SaveModel;MaxM;MaxN" 
    "String;int;int;int;bool;bool;int;int"
    "filename where the trained model will be saved;
	M;
	N;
	Number of iteration;
	show the training print-out;
	flag to save the trained model or not;
	The maximum value of M;
	The maximum value of N"
    "Emgu.CV.Face"
    "FaceInvoke"
    "FacemarkAAMParams"
	""
    "#include \"face_c.h\""
	""
	""
	${HAVE_opencv_face})

############################### face code gen END ##############################

############################### dnn code gen START ##############################
IF (NOT HAVE_opencv_dnn)
  SET(HAVE_opencv_dnn FALSE)
ENDIF()
  CREATE_OCV_CLASS_PROPERTY( 
    "dnn/net_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Dnn/Net.g.cs"
    "cv::dnn::Net" 
    "Net" 
    "PreferableBackend;PreferableTarget;enableFusion;empty;HalideScheduler" 
    "int;int;bool;bool;cv::String" 
    "propW;propW;act1;val;propW"
    "PreferableBackend;PreferableTarget;EnableFusion;Empty;HalideScheduler" 
    "Backend;Target;bool;bool;String"
    "Ask network to use specific computation backend where it supported.;
	Ask network to make computations on specific target device.;
	Enables or disables layer fusion in the network.;
	Returns true if there are no layers in the network.;
	Schedule layers that support Halide backend. Then compile them for specific target. For layers that not represented in scheduling file or if no manual scheduling used at all, automatic scheduling will be applied."
    "Emgu.CV.Dnn"
    "DnnInvoke"
    "Net"
	""
	"#include \"dnn_c.h\""
	""
	""
	${HAVE_opencv_dnn})
  CREATE_OCV_CLASS_PROPERTY( 
    "dnn/layer_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Dnn/Layer.g.cs"
    "cv::dnn::Layer" 
    "Layer" 
    "name;type;preferableTarget" 
    "cv::String;cv::String;int" 
    "elementR;elementR;elementR"
    "Name;Type;PreferableTarget" 
    "String;String;Target"
    "The name of the layer;
	The layer type;
	The preferable target"
    "Emgu.CV.Dnn"
    "DnnInvoke"
    "Layer"
	""
	"#include \"dnn_c.h\""
	""
	""
	${HAVE_opencv_dnn})
  CREATE_OCV_CLASS_PROPERTY( 
    "dnn/TextDetectionModel_EAST_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Dnn/TextDetectionModel_EAST.g.cs"
    "cv::dnn::TextDetectionModel_EAST" 
    "TextDetectionModel_EAST" 
    "ConfidenceThreshold;NMSThreshold" 
    "float;float" 
    "prop;prop"
    "ConfidenceThreshold;NMSThreshold" 
    "float;float"
    "Confidence threshold;
    Non-maximum suppression threshold"
    "Emgu.CV.Dnn"
    "DnnInvoke"
    "TextDetectionModel_EAST"
	""
	"#include \"dnn_c.h\""
	""
	""
	${HAVE_opencv_dnn})
  CREATE_OCV_CLASS_PROPERTY( 
    "dnn/TextDetectionModel_DB_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Dnn/TextDetectionModel_DB.g.cs"
    "cv::dnn::TextDetectionModel_DB" 
    "TextDetectionModel_DB" 
    "BinaryThreshold;PolygonThreshold;UnclipRatio;MaxCandidates" 
    "float;float;double;int" 
    "prop;prop;prop;prop"
    "BinaryThreshold;PolygonThreshold;UnclipRatio;MaxCandidates" 
    "float;float;double;int"
    "Binary threshold;
    Polygon threshold;
    Unclip ratio;
    Max candidates"
    "Emgu.CV.Dnn"
    "DnnInvoke"
    "TextDetectionModel_DB"
	""
	"#include \"dnn_c.h\""
	""
	""
	${HAVE_opencv_dnn})
  CREATE_OCV_CLASS_PROPERTY( 
    "dnn/TextRecognitionModel_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Dnn/TextRecognitionModel.g.cs"
    "cv::dnn::TextRecognitionModel" 
    "TextRecognitionModel" 
    "DecodeType" 
    "cv::String" 
    "prop"
    "DecodeType" 
    "String"
    "Decode type"
    "Emgu.CV.Dnn"
    "DnnInvoke"
    "TextRecognitionModel"
	""
	"#include \"dnn_c.h\""
	""
	""
	${HAVE_opencv_dnn})
  CREATE_OCV_CLASS_PROPERTY( 
    "dnn/DetectionModel_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Dnn/DetectionModel.g.cs"
    "cv::dnn::DetectionModel" 
    "DetectionModel" 
    "NmsAcrossClasses" 
    "bool" 
    "prop"
    "NmsAcrossClasses" 
    "bool"
    "It true, will perform non-maximum suppression across classes"
    "Emgu.CV.Dnn"
    "DnnInvoke"
    "DetectionModel"
	""
	"#include \"dnn_c.h\""
	""
	""
	${HAVE_opencv_dnn})
############################### dnn code gen END ##############################

############################### stereo code gen START ##############################
IF (NOT HAVE_opencv_stereo)
  SET(HAVE_opencv_stereo FALSE)
ENDIF()
  CREATE_OCV_CLASS_PROPERTY( 
    "stereo/quasi_dense_stereo_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/Stereo/QuasiDenseStereo.g.cs"
    "cv::stereo::QuasiDenseStereo" 
    "QuasiDenseStereo" 
    "Param" 
    "cv::stereo::PropagationParameters" 
    "element"
    "Param" 
    "QuasiDenseStereo.PropagationParameters"
    "Parameters for the QuasiDenseStereo class"
    "Emgu.CV.Stereo"
    "StereoInvoke"
    "QuasiDenseStereo"
	""
	"#include \"stereo_c.h\""
	""
	""
	${HAVE_opencv_stereo})

############################### stereo code gen END ##############################

############################### OPENMP START ##############################
IF(WITH_OPENMP)
  FIND_PACKAGE( OpenMP REQUIRED)
  if(OPENMP_FOUND)
    MESSAGE(STATUS " -- Compiling cvextern with OPENMP")
    set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} ${OpenMP_C_FLAGS}")
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} ${OpenMP_CXX_FLAGS}")
    set(CMAKE_EXE_LINKER_FLAGS "${CMAKE_EXE_LINKER_FLAGS} ${OpenMP_EXE_LINKER_FLAGS}")
  endif()
ENDIF()
############################### OPENMP END ################################


SET(OPENCV_MODULES_DIR "${OPENCV_SUBFOLDER}/modules")
INCLUDE_DIRECTORIES("${CMAKE_BINARY_DIR}")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}")

file(GLOB extern_srcs "${PROJECT_SOURCE_DIR}/*.cpp")
file(GLOB extern_hdrs "${PROJECT_SOURCE_DIR}/*.h*")
source_group("Src_extern" FILES ${extern_srcs})
source_group("Include_extern" FILES ${extern_hdrs})

FOREACH(CVEXTERN_OPTIONAL_DEP ${OPENCV_MODULE_NAME_CANDIDATES})
  #MESSAGE("Retriveing source folder for project :  opencv_${CVEXTERN_OPTIONAL_DEP}")
  #GET_TARGET_PROPERTY(OPENCV_MODULE_SOURCE_DIR opencv_${CVEXTERN_OPTIONAL_DEP} PROJECT_SOURCE_DIR)
  #IF (HAVE_opencv_${CVEXTERN_OPTIONAL_DEP})
  INCLUDE_DIRECTORIES("${opencv_${CVEXTERN_OPTIONAL_DEP}_SOURCE_DIR}/include")
  #ENDIF()
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

############################### DEPTHAI START ##############################


  CREATE_OCV_CLASS_PROPERTY( 
    "depthai/device_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/DepthAI/Device.g.cs"
    "Device" 
    "Device" 
    "is_usb3;is_eeprom_loaded;is_rgb_connected;is_left_connected;is_right_connected" 
    "bool;bool;bool;bool;bool" 
    "val;val;val;val;val"
    "IsUsb3;IsEepromLoaded;IsRgbConnected;IsLeftConnected;IsRightConnected" 
    "bool;bool;bool;bool;bool"
    "True if the connection is USB3;
     True if eeprom is loaded;
     True if rgb connected;
     True if left connected;
     True if right connected"
    "Emgu.CV.DepthAI"
    "DepthAIInvoke"
    "Device"
	""
    "#include \"depthai_c.h\""
	""
	"" 
	${EMGU_CV_WITH_DEPTHAI})

      CREATE_OCV_CLASS_PROPERTY( 
    "depthai/hostDataPacket_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/DepthAI/HostDataPacket.g.cs"
    "HostDataPacket" 
    "HostDataPacket" 
    "stream_name;size;Data;elem_size" 
    "cv::String;int;const unsigned char*;int" 
    "elementR;val;propR;elementR"
    "StreamName;Size;Data;ElemSize" 
    "String;int;IntPtr;int"
    "Stream Name;
    Size;
    Pointer to the native data;
    Element size"
    "Emgu.CV.DepthAI"
    "DepthAIInvoke"
    "HostDataPacket"
	""
    "#include \"depthai_c.h\""
	""
	"" 
	${EMGU_CV_WITH_DEPTHAI})

    CREATE_OCV_CLASS_PROPERTY( 
    "depthai/FrameMetadata_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/DepthAI/FrameMetadata.g.cs"
    "FrameMetadata" 
    "FrameMetadata" 
    "CameraName;SequenceNum;InstanceNum;Category;Stride;FrameBytesPP;FrameHeight;FrameWidth;FrameType;Timestamp;isValid" 
    "cv::String;int;int;int;unsigned int;unsigned int;unsigned int;unsigned int;int;double;bool" 
    "propR;propR;propR;propR;propR;propR;propR;propR;propR;propR;val"
    "CameraName;SequenceNum;InstanceNum;Category;Stride;FrameBytesPP;FrameHeight;FrameWidth;FrameType;Timestamp;IsValid" 
    "String;int;int;int;UInt32;UInt32;UInt32;UInt32;int;double;bool"
    "Camera Name; 
    Sequence Number;
    Instance Number;
    Category;
    Stride;
    FrameBytesPP;
    FrameHeight;
    FrameWidth;
    FrameType;
    Timestamp;
    True if the value is valid"
    "Emgu.CV.DepthAI"
    "DepthAIInvoke"
    "FrameMetadata"
	""
    "#include \"depthai_c.h\""
	""
	"" 
	${EMGU_CV_WITH_DEPTHAI})
	
file(GLOB_RECURSE depthai_srcs "${PROJECT_SOURCE_DIR}/depthai/*.cpp")
file(GLOB_RECURSE depthai_hdrs "${PROJECT_SOURCE_DIR}/depthai/*.h*")
#message(STATUS ">>>>>>>>>>>>>>>>>>>>>>>>>>>>>  depthai_srcs: ${depthai_srcs}")
#message(STATUS ">>>>>>>>>>>>>>>>>>>>>>>>>>>>>  depthai_hdrs: ${depthai_hdrs}")
source_group("Src_depthai" FILES ${depthai_srcs})
source_group("Include_depthai" FILES ${depthai_hdrs})
LIST(APPEND extern_srcs ${depthai_srcs})
LIST(APPEND extern_hdrs ${depthai_hdrs})
############################### DEPTHAI END ################################

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
        "${TESSERACT_OCR_ROOT_DIR}/src/ccutil" 
        "${TESSERACT_OCR_ROOT_DIR}/src/api" 
        "${TESSERACT_OCR_ROOT_DIR}/src/ccmain" 
        "${TESSERACT_OCR_ROOT_DIR}/src/ccstruct" 
        "${LEPT_ROOT_DIR}/src" 
        "${PROJECT_SOURCE_DIR}/tesseract")
      SET(TESSERACT_PROJECTS tesseract_api tesseract_ccmain tesseract_lstm tesseract_textord tesseract_arch tesseract_wordrec tesseract_classify tesseract_dict tesseract_ccstruct tesseract_opencl tesseract_cutil tesseract_viewer tesseract_ccutil libleptonica)
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

IF(WITH_INF_ENGINE)
	#SET(INF_ENGINE_LIB_DIRS )
	#link_directories("${INF_ENGINE_LIB_DIRS}/Release")
	link_directories("${IE_INCLUDE_DIR}/../lib/intel64/")
	
	SET(INF_ENGINE_DLL_DIRS "${IE_INCLUDE_DIR}/../bin/intel64/Release")
	FILE(GLOB INF_DLL_NAMES "${INF_ENGINE_DLL_DIRS}/*.dll" "${INF_ENGINE_DLL_DIRS}/*.xml" "${INF_ENGINE_DLL_DIRS}/*.mvcmd" "${INF_ENGINE_DLL_DIRS}/*.json" "${INF_ENGINE_DLL_DIRS}/*.elf")
    LIST(APPEND CVEXTERN_DEPENDENCY_DLLS ${INF_DLL_NAMES})

    SET(INF_TBB_ENGINE_DLL_DIRS "${IE_INCLUDE_DIR}/../external/tbb/bin")
	#MESSAGE(STATUS "^^^^^^^^^^^^^^  INF_TBB_ENGINE_DLL_DIRS: ${INF_TBB_ENGINE_DLL_DIRS}")
    FILE(GLOB INF_TBB_DLL_NAMES "${INF_TBB_ENGINE_DLL_DIRS}/*.dll")
	#MESSAGE(STATUS "^^^^^^^^^^^^^^  INF_TBB_DLL_NAMES: ${INF_TBB_DLL_NAMES}")
    FILE(GLOB INF_TBB_DEBUG_DLL_NAMES "${INF_TBB_ENGINE_DLL_DIRS}/*_debug.dll")
	#MESSAGE(STATUS "^^^^^^^^^^^^^^  INF_TBB_DEBUG_DLL_NAMES: ${INF_TBB_DEBUG_DLL_NAMES}")
    IF (INF_TBB_DEBUG_DLL_NAMES)
      LIST(REMOVE_ITEM INF_TBB_DLL_NAMES ${INF_TBB_DEBUG_DLL_NAMES})
    ENDIF()
	LIST(APPEND CVEXTERN_DEPENDENCY_DLLS ${INF_TBB_DLL_NAMES})
    
    SET(INF_NGRAPH_DLL_DIRS "${IE_INCLUDE_DIR}/../../ngraph/lib")
    FILE(GLOB INF_NGRAPH_DLL_NAMES "${INF_NGRAPH_DLL_DIRS}/*ngraph.dll" "${INF_NGRAPH_DLL_DIRS}/*_importer.dll")
    #MESSAGE(STATUS "^^^^^^^^^^^^^^  INF_NGRAPH_DLL_NAMES: ${INF_NGRAPH_DLL_NAMES}")
    LIST(APPEND CVEXTERN_DEPENDENCY_DLLS ${INF_NGRAPH_DLL_NAMES})
	
	SET(INF_HDDL_DLL_DIRS "${IE_INCLUDE_DIR}/../external/hddl/bin")
    FILE(GLOB INF_HDDL_DLL_NAMES "${INF_HDDL_DLL_DIRS}/*.dll")
    #MESSAGE(STATUS "^^^^^^^^^^^^^^  INF_HDDL_DLL_NAMES: ${INF_HDDL_DLL_NAMES}")
    LIST(APPEND CVEXTERN_DEPENDENCY_DLLS ${INF_HDDL_DLL_NAMES})
	
	SET(PROGRAMFILES_X86_ENV "ProgramFiles(x86)")
    SET(ICC_RUNTIME_DIRS "$ENV{${PROGRAMFILES_X86_ENV}}/Common Files/Intel/Shared Libraries/redist/intel64_win/compiler")
    STRING(REGEX REPLACE "\\\\" "/" ICC_RUNTIME_DIRS ${ICC_RUNTIME_DIRS})
    FILE(GLOB ICC_RUNTIME_DLL_NAMES "${ICC_RUNTIME_DIRS}/libmmd.dll" "${ICC_RUNTIME_DIRS}/svml_dispmd.dll")
    #MESSAGE(STATUS "^^^^^^^^^^^^^^  ICC_RUNTIME_DLL_NAMES: ${ICC_RUNTIME_DLL_NAMES}")
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
  
  IF(TARGET_ARCH_64) 
    SET(CUDA_POSTFIX 64)
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
	  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/nppc${CUDA_POSTFIX}_${NPP_POSTFIX}.dll"
      "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/nppi${CUDA_POSTFIX}_${NPP_POSTFIX}.dll"
	  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/nppial${CUDA_POSTFIX}_${NPP_POSTFIX}.dll"
	  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/nppicc${CUDA_POSTFIX}_${NPP_POSTFIX}.dll"
	  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/nppicom${CUDA_POSTFIX}_${NPP_POSTFIX}.dll"
	  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/nppidei${CUDA_POSTFIX}_${NPP_POSTFIX}.dll"
	  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/nppif${CUDA_POSTFIX}_${NPP_POSTFIX}.dll"
	  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/nppig${CUDA_POSTFIX}_${NPP_POSTFIX}.dll"
	  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/nppim${CUDA_POSTFIX}_${NPP_POSTFIX}.dll"
	  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/nppist${CUDA_POSTFIX}_${NPP_POSTFIX}.dll"
	  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/nppisu${CUDA_POSTFIX}_${NPP_POSTFIX}.dll"
	  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/nppitc${CUDA_POSTFIX}_${NPP_POSTFIX}.dll"
      "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/npps${CUDA_POSTFIX}_${NPP_POSTFIX}.dll")
	FOREACH(CUDA_NPP_DLL ${CUDA_NPP_DLLS})
	  IF(EXISTS "${CUDA_NPP_DLL}")
		LIST(APPEND CVEXTERN_CUDA_DEPENDENCY_DLLS "${CUDA_NPP_DLL}")
	  ENDIF()
    ENDFOREACH()

	FILE(GLOB CUDART_DLL_FULL_NAME "${CUDA_TOOLKIT_ROOT_DIR}/bin/cudart${CUDA_POSTFIX}_*.dll")
	
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
      FILE(GLOB CVEXTERN_CUFFT_DEPENDENCY_DLLS "${CUDA_TOOLKIT_ROOT_DIR}/bin/cufft*.dll")
      MESSAGE(STATUS "CVEXTERN_CUFFT_DEPENDENCY_DLLS: ${CVEXTERN_CUFFT_DEPENDENCY_DLLS}")
      LIST(APPEND CVEXTERN_CUDA_DEPENDENCY_DLLS ${CVEXTERN_CUFFT_DEPENDENCY_DLLS})
    ENDIF()
    
    IF(WITH_CUBLAS)
      FILE(GLOB CVEXTERN_CUBLAS_DEPENDENCY_DLLS "${CUDA_TOOLKIT_ROOT_DIR}/bin/cublas*.dll")
      MESSAGE(STATUS "CVEXTERN_CUBLAS_DEPENDENCY_DLLS: ${CVEXTERN_CUBLAS_DEPENDENCY_DLLS}")
      LIST(APPEND CVEXTERN_CUDA_DEPENDENCY_DLLS ${CVEXTERN_CUBLAS_DEPENDENCY_DLLS})
	  #LIST(APPEND CVEXTERN_CUDA_DEPENDENCY_DLLS "${CUDA_TOOLKIT_ROOT_DIR}/bin/cublas${CUDA_POSTFIX}_${NPP_POSTFIX}.dll")
	  #IF ((${npp_major} GREATER 10) OR (("${npp_major}" STREQUAL "10") AND (${npp_minor} GREATER 0)))
	  #  LIST(APPEND CVEXTERN_CUDA_DEPENDENCY_DLLS "${CUDA_TOOLKIT_ROOT_DIR}/bin/cublasLt${CUDA_POSTFIX}_${NPP_POSTFIX}.dll")
	  #ENDIF()
    ENDIF()  
  ELSE()  
    LIST(APPEND CVEXTERN_CUDA_DEPENDENCY_DLLS 
      "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/npp${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build}.dll"
      "${CUDA_TOOLKIT_ROOT_DIR}/bin/cudart${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build}.dll"
      )
    #LIST(APPEND CVEXTERN_DEPENDENCY_DLL_NAMES 
    #  npp${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build} 
    #  cudart${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build})
    
    IF(WITH_CUFFT)
      LIST(APPEND CVEXTERN_CUDA_DEPENDENCY_DLLS "${CUDA_TOOLKIT_ROOT_DIR}/bin/cufft${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build}.dll")
      #LIST(APPEND CVEXTERN_DEPENDENCY_DLL_NAMES cufft${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build}) 
    ENDIF()
    
    IF(WITH_CUBLAS)
      LIST(APPEND CVEXTERN_CUDA_DEPENDENCY_DLLS "${CUDA_TOOLKIT_ROOT_DIR}/bin/cublas${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build}.dll")
      #LIST(APPEND CVEXTERN_DEPENDENCY_DLL_NAMES cublas${CUDA_POSTFIX}_${npp_major}${npp_minor}_${npp_build})
    ENDIF()
  ENDIF()

  IF(WITH_CUDNN)
    FILE(GLOB CVEXTERN_CUDNN_DEPENDENCY_DLLS "${CUDA_TOOLKIT_ROOT_DIR}/bin/cudnn*.dll")
    MESSAGE(STATUS "CVEXTERN_CUDNN_DEPENDENCY_DLLS: ${CVEXTERN_CUDNN_DEPENDENCY_DLLS}")
    LIST(APPEND CVEXTERN_CUDA_DEPENDENCY_DLLS ${CVEXTERN_CUDNN_DEPENDENCY_DLLS})
	#MESSAGE("WITH_CUDNN: ${WITH_CUDNN}")
	#MESSAGE("APPENDING: ${CUDA_TOOLKIT_ROOT_DIR}/bin/cudnn${CUDA_POSTFIX}_${CUDNN_MAJOR_VERSION}.dll")
    #LIST(APPEND CVEXTERN_CUDA_DEPENDENCY_DLLS "${CUDA_TOOLKIT_ROOT_DIR}/bin/cudnn${CUDA_POSTFIX}_${CUDNN_MAJOR_VERSION}.dll") 
  ENDIF()

  FILE(GLOB CVEXTERN_CURAND_DEPENDENCY_DLLS "${CUDA_TOOLKIT_ROOT_DIR}/bin/curand*.dll")
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

  IF( (NOT WITH_CUDA) OR (NOT (TARGET opencv_cudaimgproc)) )
    FILE(GLOB xfeatures2d_srcs_excludes "xfeatures2d/nonfree_gpu_c.cpp")
    LIST(REMOVE_ITEM extern_srcs ${xfeatures2d_srcs_excludes})
    FILE(GLOB xfeatures2d_hdrs_excludes "xfeatures2d/nonfree_gpu_c.h")
    LIST(REMOVE_ITEM extern_hdrs ${xfeatures2d_hdrs_excludes})
  ENDIF()

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



IF(IOS)
  add_library(${the_target} STATIC ${extern_srcs} ${extern_hdrs} )
ELSEIF(WIN32)
  #add the version information for windows binary
  add_library(${the_target} SHARED ${extern_srcs} ${extern_hdrs} ${PROJECT_SOURCE_DIR}/version.rc )
ELSEIF("${CMAKE_SYSTEM_NAME}" STREQUAL "Emscripten")
  add_library(${the_target} STATIC ${extern_srcs} ${extern_hdrs} )
  #set_target_properties(${the_target} PROPERTIES SUFFIX ".bc")    
ELSE()
  add_library(${the_target} SHARED ${extern_srcs} ${extern_hdrs} )
  IF (APPLE)
    set_target_properties(${the_target} PROPERTIES MACOSX_RPATH ON)    
    #SET(DYLIBBUNDLER_PATH "${PROJECT_SOURCE_DIR}/../platform/macos/dylibbundler")
    #MESSAGE(STATUS "DYLIBBUNDLER_PATH: ${DYLIBBUNDLER_PATH}")
  ENDIF()
ENDIF()  



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

IF(DEFINED CVEXTERN_CUDA_DEPENDENCY_DLLS)
  #Promote this to parent scope such that cpack will know what cuda dlls to be included in the package
  #MESSAGE("**************CVEXTERN_CUDA_DEPENDENCY_DLL_NAMES: ${CVEXTERN_CUDA_DEPENDENCY_DLL_NAMES}")
  SET(CVEXTERN_CUDA_DEPENDENCY_DLL_NAMES ${CVEXTERN_CUDA_DEPENDENCY_DLL_NAMES} PARENT_SCOPE)
ENDIF()

# For dynamic link numbering conventions
set_target_properties(${the_target} PROPERTIES
  OUTPUT_NAME "${the_target}"
  )

# Additional target properties
set_target_properties(${the_target} PROPERTIES
  DEBUG_POSTFIX "${OPENCV_DEBUG_POSTFIX}"
  ARCHIVE_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}"
  RUNTIME_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}"
  LIBRARY_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}"
  DEFINE_SYMBOL "CVAPI_EXPORTS"
  )

#if (NOT (WIN32 OR ANDROID OR IOS))
#  set_target_properties(${the_target} PROPERTIES LINK_FLAGS "-fPIC")
#endif()  

if(ENABLE_SOLUTION_FOLDERS)
  set_target_properties(${the_target} PROPERTIES FOLDER "emgu")
endif() 

IF (WITH_TIFF)
  IF (BUILD_TIFF)
    ADD_DEPENDENCIES(${the_target} libtiff)
    SET(TIFF_INCLUDE_DIR "${OPENCV_SUBFOLDER}/3rdparty/libtiff")
    SET(TIFF_CONFIG_INCLUDE_DIR "${CMAKE_BINARY_DIR}/opencv/3rdparty/libtiff")
    SET(TIFF_LIBRARY "libtiff")
  ELSE()
    FIND_PACKAGE(TIFF)
    IF(NOT TIFF_FOUND)
      ADD_DEPENDENCIES(${the_target} libtiff)
      SET(TIFF_INCLUDE_DIR "${OPENCV_SUBFOLDER}/3rdparty/libtiff")
      SET(TIFF_CONFIG_INCLUDE_DIR "${CMAKE_BINARY_DIR}/opencv/3rdparty/libtiff")
      SET(TIFF_LIBRARY "libtiff")
    ENDIF()
  ENDIF()
  INCLUDE_DIRECTORIES(${TIFF_INCLUDE_DIR})
  INCLUDE_DIRECTORIES(${TIFF_CONFIG_INCLUDE_DIR})
ELSE()
  SET(TIFF_LIBRARY "")
ENDIF()

IF(WIN32 OR APPLE OR NETFX_CORE OR IS_UBUNTU)
  #always build from source on windows / Mac
  MESSAGE(STATUS "Building zlib from source")
  ADD_DEPENDENCIES(${the_target} zlib)
  SET(ZLIB_INCLUDE_DIR "${OPENCV_SUBFOLDER}/3rdparty/zlib")
  SET(ZLIB_LIBRARY "zlib")
  INCLUDE_DIRECTORIES(${ZLIB_INCLUDE_DIR})
ELSE()
  FIND_PACKAGE(ZLIB)
  IF(ZLIB_FOUND)
    MESSAGE(STATUS "CVEXTERN: ZLIB found.")
  ELSE()
    MESSAGE(STATUS "CVEXTERN: ZLIB not found, building from source")
    ADD_DEPENDENCIES(${the_target} zlib)
    SET(ZLIB_INCLUDE_DIR "${OPENCV_SUBFOLDER}/3rdparty/zlib")
    SET(ZLIB_LIBRARY "zlib")
  ENDIF()
  INCLUDE_DIRECTORIES(${ZLIB_INCLUDE_DIR})
ENDIF()

SET(GEOTIFF_LIBRARY)
IF (ANDROID OR APPLE)
  SET(EMGU_CV_WITH_TIFF OFF CACHE BOOL "BUILD TIFF wrapper")
  MESSAGE(STATUS "Skipping tiff")
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

IF (OPENCL_PROJ)
  ADD_DEPENDENCIES(${the_target} ${OPENCL_PROJ})
ENDIF()

#IF (ZLIB_LIBRARY)
#  ADD_DEPENDENCIES(${the_target} ${ZLIB_LIBRARY})
#ENDIF()

IF (TIFF_LIBRARY)
  IF (TARGET TIFF_LIBRARY)
    ADD_DEPENDENCIES(${the_target} ${TIFF_LIBRARY})
  ENDIF()
ENDIF()

IF (GEOTIFF_LIBRARY)
  IF (TARGET GEOTIFF_LIBRARY)
    ADD_DEPENDENCIES(${the_target} ${GEOTIFF_LIBRARY})
  ENDIF()
ENDIF()

#IF (CVBLOB_LIBRARY)
#  ADD_DEPENDENCIES(${the_target} ${CVBLOB_LIBRARY})
#ENDIF()

#ADD_DEPENDENCIES(${the_target} 
#  ${OPENCL_PROJ}
  # ${ZLIB_LIBRARY} 
  #  xtiff
#  ${TIFF_LIBRARY} ${GEOTIFF_LIBRARY} ${CVBLOB_LIBRARY} )

#IF(WITH_CUDA)
#  FIND_PACKAGE(CUDA)
#  ADD_DEPENDENCIES(${the_target}  ${CUDA_LIBRARIES})
#ENDIF()

IF(EMGU_CV_WITH_TESSERACT)
  IF(TESSERACT_FOUND)
    #MESSAGE("TESSERACT found, no need to add dependency")
    #ADD_DEPENDENCIES(${TESSERACT_LIBRARIES})
  ELSE()  
    #MESSAGE("TESSERACT not found, add dependency to ${TESSERACT_PROJECTS}")
    ADD_DEPENDENCIES(${TESSERACT_PROJECTS})
  ENDIF()
ENDIF()

IF(EMGU_CV_WITH_DEPTHAI)
  target_link_libraries(${the_target} depthai-core) 
ENDIF()

FOREACH(CVEXTERN_OPTIONAL_DEP ${OPENCV_MODULE_NAMES})
  #  INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/${CVEXTERN_OPTIONAL_DEP}/include")
  MESSAGE(STATUS "CVEXTERN dependency added:  opencv_${CVEXTERN_OPTIONAL_DEP}")
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
  # For Android, tell the linker to transform all the symbols in the static libraries to hidden.
  # This can significantly reduce the binary size
  #set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -Wl,--exclude-libs,ALL")
  #set(CMAKE_C_FLAGS "${CMAKE_CXX_FLAGS} -Wl,--exclude-libs,ALL")
ENDIF()

#disable warnings
IF(MSVC)
  ADD_DEFINITIONS(-wd4251 -D_CRT_SECURE_NO_WARNINGS)
ENDIF()

if(NETFX_CORE)
  if((CMAKE_SYSTEM_VERSION MATCHES 10.0) OR (CMAKE_SYSTEM_VERSION MATCHES 8.1))
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /ZW")
  endif()
endif()

if(WITH_PNG AND BUILD_PNG)
  target_link_libraries(${the_target} libpng)
endif()

if(WITH_TIFF AND BUILD_TIFF)
  target_link_libraries(${the_target} libtiff)
endif()

if(WITH_JPEG AND BUILD_JPEG)
  target_link_libraries(${the_target} libjpeg-turbo)
endif()

# Add the required libraries for linking:
target_link_libraries(${the_target} ${OPENCV_LINKER_LIBS} 
  ${ZLIB_LIBRARY} 
  #  xtiff 
  ${CVBLOB_LIBRARY})

set_target_properties(${PROJECT_NAME} PROPERTIES CXX_STANDARD 11)
IF(DEFINED EMGUCV_PLATFORM_TOOLSET)
  set_target_properties(${the_target} PROPERTIES PLATFORM_TOOLSET ${EMGUCV_PLATFORM_TOOLSET})
ENDIF()

#IF(WITH_IPP)
#  target_link_libraries(${the_target} ippdc_l)
#ENDIF()  

IF(EMGU_CV_WITH_TESSERACT AND TESSERACT_OPENCL)
  #SET(CMAKE_MODULE_PATH ${CMAKE_MODULE_PATH} "${OPENCV_SUBFOLDER}/cmake/")
  #include(${OPENCV_SUBFOLDER}/cmake/OpenCVDetectOpenCL.cmake)
  #MESSAGE(STATUS "OPENCVL_LIBRARIES: ${OPENCV_LIBRARIES}")
  target_link_libraries(${the_target} OpenCL)
ENDIF()

IF(WITH_INF_ENGINE)
  target_link_libraries(${the_target} inference_engine)
ENDIF()

#message(STATUS "EMGU_CV_WITH_FREETYPE: ${EMGU_CV_WITH_FREETYPE}")
IF (TARGET opencv_freetype)
	target_link_libraries(${the_target} ${FREETYPE_LIBRARY} ${HARFBUZZ_LIBRARY})
ENDIF()

IF (TARGET opencv_hdf)
    message(STATUS "LINK CVEXTERN with HDF5_LIBRARIES: ${HDF5_LIBRARIES}")
	target_link_libraries(${the_target} ${HDF5_LIBRARIES})
ENDIF()

IF(ANDROID)
  IF(EMGU_CV_WITH_TESSERACT)
    #NOT SURE WHY THE FOLLOWING IS NEEDED, BUT SEEMS TO MAKE THE PROBLEM GOES AWAY
    target_link_libraries(${the_target} ${TESSERACT_PROJECTS} tesseract_ccstruct tesseract_dict tesseract_classify tesseract_ccutil)
  ENDIF()
  INSTALL(TARGETS ${the_target} 
	RUNTIME DESTINATION libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT libs
    LIBRARY DESTINATION libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT libs
	ARCHIVE DESTINATION libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT libs)
ELSEIF(APPLE)
  IF(EMGU_CV_WITH_TESSERACT)
    IF(NOT TESSERACT_FOUND)
      target_link_libraries(${the_target} ${TESSERACT_PROJECTS})
    ENDIF()
  ENDIF()  
ELSE()
  target_link_libraries(${the_target} ${TIFF_LIBRARY})
  IF (EMGU_CV_WITH_TIFF)
    target_link_libraries(${the_target} ${GEOTIFF_LIBRARY})
  ENDIF()
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
    RUNTIME DESTINATION libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT libs
    LIBRARY DESTINATION libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT libs
    ARCHIVE DESTINATION libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT libs)
ENDIF()

#add_subdirectory(gpu)
IF (IOS)
  SET(IOS_RELEASE_FOLDER "")
  IF (IPHONESIMULATOR)
    SET(IOS_RELEASE_FOLDER "Release-iphonesimulator")
    SET(IOS_RELEASE_FILENAME "libcvextern_simulator_${IOS_ARCH}.a")
  ELSEIF(IPHONEOS)
    SET(IOS_RELEASE_FOLDER "Release-iphoneos")
    SET(IOS_RELEASE_FILENAME "libcvextern_iphoneos_${IOS_ARCH}.a")
  ENDIF()
  #MESSAGE(STATUS "<<<<<< IOS_RELEASE_FOLDER: ${IOS_RELEASE_FOLDER} >>>>>>") 
  SET(LIBTOOL_LINKING_COMMAND libtool -static -o ${CMAKE_SOURCE_DIR}/libs/iOS/${IOS_RELEASE_FILENAME} bin${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}/Release/*.a opencv/3rdparty/lib/Release/*.a ${CMAKE_RUNTIME_OUTPUT_DIRECTORY}/Release/*.a)
  IF (EMGU_CV_WITH_TESSERACT)
     LIST(APPEND LIBTOOL_LINKING_COMMAND libs/Release/*.a)
  ENDIF()
  IF (TARGET opencv_freetype) 
	LIST(APPEND LIBTOOL_LINKING_COMMAND ${FREETYPE_LIBRARIES} ${HARFBUZZ_LIBRARIES})
  ENDIF()
  add_custom_command(TARGET ${the_target}
    POST_BUILD
    COMMAND ${LIBTOOL_LINKING_COMMAND}
    WORKING_DIRECTORY "${CMAKE_BINARY_DIR}"
    COMMENT "Linking target for ${IOS_ARCH}")
    
  SET(IPHONEOS_STATIC_LIB_FILE ${CMAKE_SOURCE_DIR}/libs/iOS/libcvextern_iphoneos.a)
  SET(IPHONESIMULATOR_STATIC_LIB_FILE ${CMAKE_SOURCE_DIR}/libs/iOS/libcvextern_simulator.a)
  
  IF (IPHONESIMULATOR)
    IF (EXISTS ${IPHONEOS_STATIC_LIB_FILE})
    ELSE()
      FILE(WRITE ${IPHONEOS_STATIC_LIB_FILE} "")
    ENDIF()
    add_custom_command(TARGET ${the_target}
      POST_BUILD
      COMMAND lipo -create -output ${IPHONESIMULATOR_STATIC_LIB_FILE} ${CMAKE_SOURCE_DIR}/libs/iOS/libcvextern_simulator_*.a
      WORKING_DIRECTORY "${CMAKE_SOURCE_DIR}"
      COMMENT "Linking multiple arch into a single file for iOS simulator") 
  ELSEIF(IPHONEOS)
    IF (EXISTS ${IPHONESIMULATOR_STATIC_LIB_FILE})
    ELSE()
      FILE(WRITE ${IPHONESIMULATOR_STATIC_LIB_FILE} "")
    ENDIF()
    add_custom_command(TARGET ${the_target}
      POST_BUILD
      COMMAND lipo -create -output ${IPHONEOS_STATIC_LIB_FILE} ${CMAKE_SOURCE_DIR}/libs/iOS/libcvextern_iphoneos_*.a
      WORKING_DIRECTORY "${CMAKE_SOURCE_DIR}"
      COMMENT "Linking multiple arch into a single file for iOS device")
  ENDIF()
  INSTALL(FILES
    "${IPHONEOS_STATIC_LIB_FILE}"
    DESTINATION "libs/iOS/"
    COMPONENT emgucv_binary)
  INSTALL(FILES
    "${IPHONESIMULATOR_STATIC_LIB_FILE}"
    DESTINATION "libs/iOS/"
    COMPONENT emgucv_binary)
ELSEIF (ANDROID)
add_custom_command(TARGET ${the_target}
    POST_BUILD
    COMMAND ${CMAKE_STRIP} libcvextern.so 
    WORKING_DIRECTORY "${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}"
    COMMENT "Stripping libcvextern.so from ${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}")
ELSEIF (IS_UBUNTU AND WITH_CUDA)
  add_custom_command(TARGET ${the_target}
    POST_BUILD
    COMMAND cp -fP ${CMAKE_BINARY_DIR}/bin${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}/*.so ${CMAKE_BINARY_DIR}/bin${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}/*.so.* ${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}
    #WORKING_DIRECTORY "${UNMANAGED_LIBRARY_OUTPUT_PATH}"
    COMMENT "Copying file to ${CMAKE_SOURCEDIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}")
ELSEIF (APPLE)
  SET_TARGET_PROPERTIES(${the_target}
    PROPERTIES
    XCODE_ATTRIBUTE_COPY_PHASE_STRIP "YES"
    XCODE_ATTRIBUTE_STRIP_INSTALLED_PRODUCT "YES"
    XCODE_ATTRIBUTE_STRIP_STYLE "non-global"
    XCODE_ATTRIBUTE_STRIPFLAGS "-x -u -r"
    XCODE_ATTRIBUTE_DEAD_CODE_STRIPPING "YES"
    )
  IF ("${${CMAKE_SYSTEM_PROCESSOR}}" STREQUAL "arm64")
    SET(CV_EXTERN_FILE_NAME "libcvextern_arm64.dylib")
    SET(LIBUSB_FILE_NAME "libusb_arm64.dylib")
  ELSE()
    SET(CV_EXTERN_FILE_NAME "libcvextern_x64.dylib")
    SET(LIBUSB_FILE_NAME "libusb_x64.dylib")
  ENDIF()
  add_custom_command(TARGET ${the_target}
    POST_BUILD
    COMMAND mkdir -p ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../arch
    COMMENT "Creating arch folder: ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../arch")
    
  add_custom_command(TARGET ${the_target}
    POST_BUILD
    COMMAND cp -f ${UNMANAGED_LIBRARY_OUTPUT_PATH}/libcvextern.dylib ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../arch/${CV_EXTERN_FILE_NAME}
    COMMENT "Copying file to ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../arch/${CV_EXTERN_FILE_NAME}")
  add_custom_command(TARGET ${the_target}
    POST_BUILD
    COMMAND lipo ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../arch/libcvextern_*.dylib -output ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../libcvextern.dylib -create
    COMMENT "Linking target for all architectures and ouputing to ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../libcvextern.dylib")


  IF(EMGU_CV_WITH_DEPTHAI)
    IF ("${EMGUCV_ARCH}" STREQUAL "arm64")
       # Apple silicon
       SET(LIBUSB_PATH /opt/homebrew/opt/libusb/lib/libusb-1.0.0.dylib)
    ELSE()
       # Intel Mac
       SET(LIBUSB_PATH /usr/local/opt/libusb/lib/libusb-1.0.0.dylib)
    ENDIF()
    add_custom_command(TARGET ${the_target}
      POST_BUILD
      COMMAND cp -f ${LIBUSB_PATH} ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../arch/${LIBUSB_FILE_NAME}
      COMMENT "Copying file to ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../arch/${LIBUSB_FILE_NAME}")
    add_custom_command(TARGET ${the_target}
      POST_BUILD
      COMMAND lipo ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../arch/libusb*.dylib -output ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../libusb-1.0.0.dylib -create
      COMMENT "Linking target for all architectures and ouputing to ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../libusb-1.0.0.dylib")
    INSTALL(FILES
      ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../libusb-1.0.0.dylib
      DESTINATION "libs/macos/"
      COMPONENT emgucv_binary)
#    add_custom_command(TARGET ${the_target}
#      POST_BUILD
#      COMMAND install_name_tool -change /usr/local/opt/libusb/lib/libusb-1.0.0.dylib @rpath/libusb-1.0.0.dylib ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../libusb-1.0.0.dylib 
#      COMMENT "Altering dependency for ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../libusb-1.0.0.dylib")
    add_custom_command(TARGET ${the_target}
      POST_BUILD
      COMMAND install_name_tool -change /usr/local/opt/libusb/lib/libusb-1.0.0.dylib @rpath/libusb-1.0.0.dylib ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../libcvextern.dylib 
      COMMENT "Altering dependency for ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../libcvextern.dylib")
    add_custom_command(TARGET ${the_target}
      POST_BUILD
      COMMAND install_name_tool -id "@rpath/libusb-1.0.0.dylib" ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../libusb-1.0.0.dylib
      COMMAND chmod a+x ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../libusb-1.0.0.dylib
      COMMENT "Altering id for ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../libusb-1.0.0.dylib")
  ENDIF()

  INSTALL(FILES
    ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../libcvextern.dylib
    DESTINATION "libs/macos/"
    COMPONENT emgucv_binary)

  #INSTALL(FILES ${the_target} 
  #  RUNTIME DESTINATION libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT libs
  #  LIBRARY DESTINATION libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT libs
  #  ARCHIVE DESTINATION libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT libs)
  
ENDIF()

#IF("${CMAKE_SYSTEM_NAME}" STREQUAL "Emscripten")
#  add_custom_command(TARGET ${the_target}
#    POST_BUILD
#    COMMAND emcc -o ${CMAKE_SOURCE_DIR}/libs/libcvextern.bc bin${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}/*.a opencv/3rdparty/lib/*.a opencv/lib/*.a ${CMAKE_RUNTIME_OUTPUT_DIRECTORY}/*.a
#    WORKING_DIRECTORY "${CMAKE_BINARY_DIR}"
#    COMMENT "Merging libcvextern.bc")
#ENDIF()
