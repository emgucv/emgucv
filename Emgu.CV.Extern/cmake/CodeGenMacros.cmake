# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
#
#  Code generation macros used by Emgu.CV.Extern/CMakeLists.txt to produce
#  the P/Invoke C++ (.g.h/.g.cpp) and C# (.g.cs) source files:
#    - CREATE_VECTOR_CS: generates VectorOf* wrapper types.
#    - WRITE_IF_DIFFERENT: writes a file only if its content changed.
#    - CREATE_OCV_CLASS_PROPERTY: generates per-class property getters/
#      setters and simple methods.
#
# ----------------------------------------------------------------------------

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

CREATE_VECTOR_CS("IntPtr" "void*" "IntPtr" "struct_not_array" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")

IF(EMGU_CV_WITH_TESSERACT)
  SET(EMGU_CV_WITH_TESSERACT_FLAG "1")
ELSE()
  SET(EMGU_CV_WITH_TESSERACT_FLAG "0")
ENDIF()
CREATE_VECTOR_CS("TesseractResult" "TesseractResult" "TesseractResult" "struct" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.OCR/" Emgu.CV.OCR "" "#include \"tesseract_c.h\"" "" "defined(HAVE_EMGUCV_TESSERACT)")

CREATE_VECTOR_CS("Mat" "cv::Mat" "Mat" "object" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")
CREATE_VECTOR_CS("UMat" "cv::UMat" "UMat" "object" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")
CREATE_VECTOR_CS("GMat" "cv::GMat" "GMat" "object_not_array" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/Gapi" Emgu.CV.Util "" "#include \"gapi_c.h\"" "" "defined(HAVE_OPENCV_GAPI)")
CREATE_VECTOR_CS("CvString" "cv::String" "CvString" "object" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")

CREATE_VECTOR_CS("VectorOfPoint" "std::vector< cv::Point >" "Point" "vector" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")
CREATE_VECTOR_CS("VectorOfPointF" "std::vector< cv::Point2f >" "PointF" "vector" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")
CREATE_VECTOR_CS("VectorOfPoint3D32F" "std::vector< cv::Point3f >" "MCvPoint3D32f" "vector" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")
CREATE_VECTOR_CS("VectorOfInt" "std::vector< int >" "int" "vector" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")
CREATE_VECTOR_CS("VectorOfByte" "std::vector< unsigned char >" "Byte" "vector" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")
CREATE_VECTOR_CS("VectorOfDMatch" "std::vector< cv::DMatch >" "MDMatch" "vector" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")
CREATE_VECTOR_CS("VectorOfRect" "std::vector< cv::Rect >" "Rectangle" "vector" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")

CREATE_VECTOR_CS("VectorOfVectorOfPointF" "std::vector< std::vector< cv::Point2f > >" "VectorOfVectorOfPointF" "object_not_array" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")

CREATE_VECTOR_CS("OclPlatformInfo" "cv::ocl::PlatformInfo" "Ocl.PlatformInfo" "object" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "#include \"opencv2/core/ocl.hpp\"" "" "1")
CREATE_VECTOR_CS("GpuMat" "cv::cuda::GpuMat" "GpuMat" "object" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Cuda" Emgu.CV.Cuda "" "#include \"opencv2/core/cuda.hpp\"" "" "1")

CREATE_VECTOR_CS("VideoCapture" "cv::VideoCapture" "VideoCapture" "object_not_array" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "#include \"videoio_c_extra.h\"" "" "defined(HAVE_OPENCV_VIDEOIO)")
CREATE_VECTOR_CS("VectorOfMat" "std::vector< cv::Mat >" "VectorOfMat" "object_not_array" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" Emgu.CV.Util "" "" "" "1")

CREATE_VECTOR_CS("LinemodMatch" "cv::linemod::Match" "Emgu.CV.Linemod.Match" "object_not_array" "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/Rgbd" Emgu.CV.Linemod "" "#include \"rgbd_c.h\"" "" "defined(HAVE_OPENCV_RGBD)")

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
  #IF (("${ocv_module_enabled}" STREQUAL "ON") OR ("${ocv_module_enabled}" STREQUAL "TRUE"))
  IF (${ocv_module_enabled})
    SET(IS_DUMMY OFF)
  ELSE()
    SET(IS_DUMMY ON)
  ENDIF()
  #MESSAGE(STATUS ">>>>>>>>>>>>>>>> fname: ${fname}; is_dummy: ${IS_DUMMY}")

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
	SET(C_HEADER_SOURCE "#include \"emgu_error.h\"
${header_additional_include}")
#  ENDIF()

  SET(C_SOURCE "${source_additional_code} #include \"${fname}.g.h\"")
  
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

    # Fallback expression used by CVAPI_CATCH_CV_ERRORS() for functions that return
    # ${PROPERTY_TYPE} by value: pointer types can't use the "Type()" value-initialization
    # syntax (it's not valid C++ grammar for a pointer type-id), so those fall back to a
    # plain 0; everything else (primitives and default-constructible structs/classes) uses
    # "Type()".
    STRING(REGEX MATCH "\\*$" PROPERTY_TYPE_IS_POINTER "${PROPERTY_TYPE}")
    IF(PROPERTY_TYPE_IS_POINTER)
      SET(PROPERTY_TYPE_FALLBACK "0")
    ELSE()
      SET(PROPERTY_TYPE_FALLBACK "${PROPERTY_TYPE}()")
    ENDIF()
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
void cve${CLASS_NAME}${CS_FUNCTION_NAME}(void* obj, cv::String* str) { try { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} } CVAPI_CATCH_CV_ERRORS_VOID }   
     ")
	    ELSE()
	      SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, cv::String* str);  
     ")
	      SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, cv::String* str) { try { *str = obj->${PROPERTY_NAME}(); } CVAPI_CATCH_CV_ERRORS_VOID }   
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
              ${cs_invoke_class}.cve${CLASS_NAME}${CS_FUNCTION_NAME}(_ptr, s);  CvInvoke.CheckError();
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
${PROPERTY_TYPE} cve${CLASS_NAME}${CS_FUNCTION_NAME}(void* obj) { try { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} } CVAPI_CATCH_CV_ERRORS(${PROPERTY_TYPE_FALLBACK}) }   
     ")
		ELSE()
	      SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(${PROPERTY_TYPE}) cve${CLASS_NAME}${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj);  
     ")
	      SET(C_SOURCE "${C_SOURCE}
${PROPERTY_TYPE} cve${CLASS_NAME}${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj) { try { return obj->${PROPERTY_NAME}(); } CVAPI_CATCH_CV_ERRORS(${PROPERTY_TYPE_FALLBACK}) }   
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
        get { ${CS_PROPERTY_TYPE} result = ${cs_invoke_class}.cve${CLASS_NAME}${CS_FUNCTION_NAME}(_ptr); CvInvoke.CheckError(); return result; }
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
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* value) { try { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} } CVAPI_CATCH_CV_ERRORS_VOID }
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* value) { try { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} } CVAPI_CATCH_CV_ERRORS_VOID }     
     ")
	    ELSE()
	    SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value);
CVAPI(void) cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value);     
     ")
        SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value) { try { ${PROPERTY_TYPE} p = cvTermCriteria(obj->get${PROPERTY_NAME}()); memcpy(value, &p, sizeof(${PROPERTY_TYPE})); } CVAPI_CATCH_CV_ERRORS_VOID }
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value) { try { obj->set${PROPERTY_NAME}( *value ); } CVAPI_CATCH_CV_ERRORS_VOID }     
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
        get { ${CS_PROPERTY_TYPE} v = new ${CS_PROPERTY_TYPE}(); ${cs_invoke_class}.cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(_ptr, ref v); CvInvoke.CheckError(); return v; } 
        set { ${cs_invoke_class}.cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(_ptr, ref value); CvInvoke.CheckError(); }
     }
     ")
      ELSEIF("${PROPERTY_TYPE}" STREQUAL "CvSize") #special handling for functions that returns CvSize
	    IF(IS_DUMMY)
	  	SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* value);
CVAPI(void) cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* value);     
     ")
	    SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* value) { try { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} } CVAPI_CATCH_CV_ERRORS_VOID }
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* value) { try { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} } CVAPI_CATCH_CV_ERRORS_VOID }     
     ")
	    ELSE()
	    SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value);
CVAPI(void) cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value);     
     ")
        SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value) { try { ${PROPERTY_TYPE} p = cvSize(obj->get${PROPERTY_NAME}()); memcpy(value, &p, sizeof(${PROPERTY_TYPE})); } CVAPI_CATCH_CV_ERRORS_VOID }
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value) { try { obj->set${PROPERTY_NAME}( *value ); } CVAPI_CATCH_CV_ERRORS_VOID }     
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
        get { ${CS_PROPERTY_TYPE} v = new ${CS_PROPERTY_TYPE}(); ${cs_invoke_class}.cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(_ptr, ref v); CvInvoke.CheckError(); return v; } 
        set { ${cs_invoke_class}.cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(_ptr, ref value); CvInvoke.CheckError(); }
     }
     ")
     ELSE()
	  IF(IS_DUMMY)
	  	SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* value);
CVAPI(void) cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* value);     
     ")
	    SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* value) { try { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} } CVAPI_CATCH_CV_ERRORS_VOID }
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* value) { try { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} } CVAPI_CATCH_CV_ERRORS_VOID }     
     ")
	  ELSE()
	    SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value);
CVAPI(void) cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value);     
     ")
        SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value) { try { ${PROPERTY_TYPE} p = obj->get${PROPERTY_NAME}(); memcpy(value, &p, sizeof(${PROPERTY_TYPE})); } CVAPI_CATCH_CV_ERRORS_VOID }
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value) { try { obj->set${PROPERTY_NAME}( *value ); } CVAPI_CATCH_CV_ERRORS_VOID }     
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
        get { ${CS_PROPERTY_TYPE} v = new ${CS_PROPERTY_TYPE}(); ${cs_invoke_class}.cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(_ptr, ref v); CvInvoke.CheckError(); return v; } 
        set { ${cs_invoke_class}.cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(_ptr, ref value); CvInvoke.CheckError(); }
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
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* value) { try { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} } CVAPI_CATCH_CV_ERRORS_VOID }
     ")
	   ELSE()
         SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value);
     ")
         SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value) { try { ${PROPERTY_TYPE} p = cvSize(obj->get${PROPERTY_NAME}()); memcpy(value, &p, sizeof(${PROPERTY_TYPE})); } CVAPI_CATCH_CV_ERRORS_VOID }
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
        get { ${CS_PROPERTY_TYPE} v = new ${CS_PROPERTY_TYPE}(); ${cs_invoke_class}.cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(_ptr, ref v); CvInvoke.CheckError(); return v; } 
     }
     ")
     ELSE()
	   IF(IS_DUMMY)
	     SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* value);
     ")
         SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* value) { try { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} } CVAPI_CATCH_CV_ERRORS_VOID }
     ")
	   ELSE()
         SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value);
     ")
         SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value) { try { ${PROPERTY_TYPE} p = obj->get${PROPERTY_NAME}(); memcpy(value, &p, sizeof(${PROPERTY_TYPE})); } CVAPI_CATCH_CV_ERRORS_VOID }
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
        get { ${CS_PROPERTY_TYPE} v = new ${CS_PROPERTY_TYPE}(); ${cs_invoke_class}.cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(_ptr, ref v); CvInvoke.CheckError(); return v; } 
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
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(void* obj, cv::String* str) { try { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} } CVAPI_CATCH_CV_ERRORS_VOID }   
     ")
		ELSE()
	      SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, cv::String* str);  
     ")
	      SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, cv::String* str) { try { obj->set${PROPERTY_NAME}(*str); } CVAPI_CATCH_CV_ERRORS_VOID }   
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
              ${cs_invoke_class}.cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(_ptr, cvs);  CvInvoke.CheckError();
           }   
     }
     ")
      ELSE()
	    IF(IS_DUMMY)
		  SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE} value);     
     ")
          SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE} value) { try { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} } CVAPI_CATCH_CV_ERRORS_VOID }     
     ")
		ELSE()
          SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE} value);     
     ")
          SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE} value) { try { obj->set${PROPERTY_NAME}( value ); } CVAPI_CATCH_CV_ERRORS_VOID }     
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
        ${cs_invoke_class}.cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(_ptr, value);  CvInvoke.CheckError();
     }
     ")
	 ENDIF()
	ELSEIF(${MARSHAL_TYPE} STREQUAL "act")
	  IF(IS_DUMMY)
	  	SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(${PROPERTY_TYPE}) cve${CLASS_NAME}${CS_FUNCTION_NAME}(void* obj);     
     ")
        SET(C_SOURCE "${C_SOURCE}
${PROPERTY_TYPE} cve${CLASS_NAME}${CS_FUNCTION_NAME}(void* obj) { try { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} } CVAPI_CATCH_CV_ERRORS(${PROPERTY_TYPE_FALLBACK}) }     
     ")
	  ELSE()
	    SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(${PROPERTY_TYPE}) cve${CLASS_NAME}${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj);     
     ")
        SET(C_SOURCE "${C_SOURCE}
${PROPERTY_TYPE} cve${CLASS_NAME}${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj) { try { return obj->${PROPERTY_NAME}(); } CVAPI_CATCH_CV_ERRORS(${PROPERTY_TYPE_FALLBACK}) }     
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
        ${CS_PROPERTY_TYPE} result = ${cs_invoke_class}.cve${CLASS_NAME}${CS_FUNCTION_NAME}(_ptr);
        CvInvoke.CheckError();
        return result;
     }
     ")
	ELSEIF(${MARSHAL_TYPE} STREQUAL "act0")
	  IF (IS_DUMMY)
	  	SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}${CS_FUNCTION_NAME}(void* obj);     
     ")
        SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}${CS_FUNCTION_NAME}(void* obj) { try { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} } CVAPI_CATCH_CV_ERRORS_VOID }     
     ")
	  ELSE()
	    SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj);     
     ")
        SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj) { try { obj->${PROPERTY_NAME}(); } CVAPI_CATCH_CV_ERRORS_VOID }     
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
        ${cs_invoke_class}.cve${CLASS_NAME}${CS_FUNCTION_NAME}(_ptr);  CvInvoke.CheckError();
     }
     ")
    ELSEIF(${MARSHAL_TYPE} STREQUAL "act1")
	  IF(IS_DUMMY)
        SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE} value);     
     ")
        SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE} value) { try { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} } CVAPI_CATCH_CV_ERRORS_VOID }     
     ")
	  ELSE()
        SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE} value);     
     ")
        SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE} value) { try { obj->${PROPERTY_NAME}( value ); } CVAPI_CATCH_CV_ERRORS_VOID }     
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
        ${cs_invoke_class}.cve${CLASS_NAME}${CS_FUNCTION_NAME}(_ptr, value);  CvInvoke.CheckError();
     }
     ")
    ELSEIF(${MARSHAL_TYPE} STREQUAL "act1obj")
	  IF(IS_DUMMY)
        SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* value);     
     ")
        SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* value) { try { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} } CVAPI_CATCH_CV_ERRORS_VOID }     
     ")
	  ELSE()
        SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value);     
     ")
        SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value) { try { obj->${PROPERTY_NAME}( *value ); } CVAPI_CATCH_CV_ERRORS_VOID }     
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
        ${cs_invoke_class}.cve${CLASS_NAME}${CS_FUNCTION_NAME}(_ptr, value);  CvInvoke.CheckError();
     }
     ")
    ELSEIF(${MARSHAL_TYPE} STREQUAL "propR")
      IF("${PROPERTY_TYPE}" STREQUAL "cv::String") #special handling for functions that returns strings
        IF (IS_DUMMY)
          SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* val);  
     ")
          SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE}* val) { try { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} } CVAPI_CATCH_CV_ERRORS_VOID }   
     ")
	    ELSE()
          SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* val);  
     ")
          SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* val) { try { *val = obj->get${PROPERTY_NAME}(); } CVAPI_CATCH_CV_ERRORS_VOID }   
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
            ${cs_invoke_class}.cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(_ptr, s); CvInvoke.CheckError();
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
${PROPERTY_TYPE} cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj) { try { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} } CVAPI_CATCH_CV_ERRORS(${PROPERTY_TYPE_FALLBACK}) }   
     ")
	  ELSE()
        SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(${PROPERTY_TYPE}) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj);  
     ")
        SET(C_SOURCE "${C_SOURCE}
${PROPERTY_TYPE} cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj) { try { return obj->get${PROPERTY_NAME}(); } CVAPI_CATCH_CV_ERRORS(${PROPERTY_TYPE_FALLBACK}) }   
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
        get { ${CS_PROPERTY_TYPE} result = ${cs_invoke_class}.cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(_ptr); CvInvoke.CheckError(); return result; }
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
${PROPERTY_TYPE}* cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj) { try { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} } CVAPI_CATCH_CV_ERRORS(0) }
     ")
		ELSE()
	      SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(${PROPERTY_TYPE}*) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj);
     ")
	      SET(C_SOURCE "${C_SOURCE}
${PROPERTY_TYPE}* cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj) { try { return &(obj->${PROPERTY_NAME}); } CVAPI_CATCH_CV_ERRORS(0) }
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
        get { IntPtr ptr = ${cs_invoke_class}.cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(_ptr); CvInvoke.CheckError(); return new ${CS_PROPERTY_TYPE}( ptr, false); } 
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
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj, cv::String* str) { try { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} } CVAPI_CATCH_CV_ERRORS_VOID }   
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(void* obj, cv::String* str) { try { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} } CVAPI_CATCH_CV_ERRORS_VOID }   
     ")
		ELSE()
	      SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, cv::String* str);  
CVAPI(void) cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, cv::String* str);  
     ")
	      SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, cv::String* str) { try { *str = obj->${PROPERTY_NAME}; } CVAPI_CATCH_CV_ERRORS_VOID }   
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, cv::String* str) { try { obj->${PROPERTY_NAME} = *str; } CVAPI_CATCH_CV_ERRORS_VOID }   
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
              ${cs_invoke_class}.cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(_ptr, s);  CvInvoke.CheckError();
              return s.ToString();
           }
        } 
		set
		{
		   using (CvString s = new CvString(value))
           {  
              ${cs_invoke_class}.cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(_ptr, s);  CvInvoke.CheckError();
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
${PROPERTY_TYPE} cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj) { try { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} } CVAPI_CATCH_CV_ERRORS(${PROPERTY_TYPE_FALLBACK}) }
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE} value) { try { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} } CVAPI_CATCH_CV_ERRORS_VOID }     
     ")
		ELSE()
	      SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(${PROPERTY_TYPE}) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj);
CVAPI(void) cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE} value);     
     ")
	      SET(C_SOURCE "${C_SOURCE}
${PROPERTY_TYPE} cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj) { try { return obj->${PROPERTY_NAME}; } CVAPI_CATCH_CV_ERRORS(${PROPERTY_TYPE_FALLBACK}) }
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE} value) { try { obj->${PROPERTY_NAME} = value; } CVAPI_CATCH_CV_ERRORS_VOID }     
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
        get { ${CS_PROPERTY_TYPE} result = ${cs_invoke_class}.cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(_ptr); CvInvoke.CheckError(); return result; }
        set { ${cs_invoke_class}.cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(_ptr, value); CvInvoke.CheckError(); }
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
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj, cv::String* str) { try { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} } CVAPI_CATCH_CV_ERRORS_VOID }   
     ")
		ELSE()
	      SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, cv::String* str);   
     ")
	      SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, cv::String* str) { try { *str = obj->${PROPERTY_NAME}; } CVAPI_CATCH_CV_ERRORS_VOID }   
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
              ${cs_invoke_class}.cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(_ptr, s);  CvInvoke.CheckError();
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
${PROPERTY_TYPE} cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj) { try { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} } CVAPI_CATCH_CV_ERRORS(${PROPERTY_TYPE_FALLBACK}) }
     ")
	  ELSE()
        SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(${PROPERTY_TYPE}) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj);
     ")
        SET(C_SOURCE "${C_SOURCE}
${PROPERTY_TYPE} cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj) { try { return obj->${PROPERTY_NAME}; } CVAPI_CATCH_CV_ERRORS(${PROPERTY_TYPE_FALLBACK}) }
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
        get { ${CS_PROPERTY_TYPE} result = ${cs_invoke_class}.cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(_ptr); CvInvoke.CheckError(); return result; }
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
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj, cv::String* str) { try { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} } CVAPI_CATCH_CV_ERRORS_VOID }   
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(void* obj, cv::String* str) { try { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} } CVAPI_CATCH_CV_ERRORS_VOID }   
     ")
	    ELSE()
          SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, cv::String* str);  
CVAPI(void) cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, cv::String* str);  
     ")
	      SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, cv::String* str) { try { *str = obj->get${PROPERTY_NAME}(); } CVAPI_CATCH_CV_ERRORS_VOID }   
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, cv::String* str) { try { obj->set${PROPERTY_NAME}(*str); } CVAPI_CATCH_CV_ERRORS_VOID }   
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
              ${cs_invoke_class}.cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(_ptr, s);  CvInvoke.CheckError();
              return s.ToString();
           }
        } 
        set
        {
           using (CvString s = new CvString(value))
           {  
              ${cs_invoke_class}.cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(_ptr, s);  CvInvoke.CheckError();
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
${PROPERTY_TYPE} cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(void* obj) { try { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} } CVAPI_CATCH_CV_ERRORS(${PROPERTY_TYPE_FALLBACK}) }
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(void* obj, ${PROPERTY_TYPE} value) { try { ${RAISE_FUNCTION_NOT_SUPPORT_CPP} } CVAPI_CATCH_CV_ERRORS_VOID }     
     ")
	  ELSE()
        SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(${PROPERTY_TYPE}) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj);
CVAPI(void) cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE} value);     
     ")
        SET(C_SOURCE "${C_SOURCE}
${PROPERTY_TYPE} cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj) { try { return obj->get${PROPERTY_NAME}(); } CVAPI_CATCH_CV_ERRORS(${PROPERTY_TYPE_FALLBACK}) }
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE} value) { try { obj->set${PROPERTY_NAME}( value ); } CVAPI_CATCH_CV_ERRORS_VOID }     
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
        get { ${CS_PROPERTY_TYPE} result = ${cs_invoke_class}.cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(_ptr); CvInvoke.CheckError(); return result; }
        set { ${cs_invoke_class}.cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(_ptr, value); CvInvoke.CheckError(); }
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
  IF (NOT ("${c_compilation_condition}" STREQUAL ""))
    SET(C_HEADER_SOURCE "${C_COMPILATION_CONDITION_OPEN}
${C_HEADER_SOURCE}
${C_COMPILATION_CONDITION_CLOSE}")
  ENDIF()
  SET(C_SOURCE "${C_COMPILATION_CONDITION_OPEN}
  ${C_SOURCE}
  ${C_COMPILATION_CONDITION_CLOSE}")
  #MESSAGE(STATUS "-------------------- CS_CLASS_SOURCE: ${CS_CLASS_SOURCE}")
  WRITE_IF_DIFFERENT("${csfname}" "${CS_SOURCE}")
  WRITE_IF_DIFFERENT("${PROJECT_SOURCE_DIR}/${fname}.g.h" "${C_HEADER_SOURCE}")
  WRITE_IF_DIFFERENT("${PROJECT_SOURCE_DIR}/${fname}.g.cpp" "${C_SOURCE}")

ENDMACRO()
