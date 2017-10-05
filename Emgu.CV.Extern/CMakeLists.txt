# --------------------------------------------------------
#  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.
# 
#  CMake file for cvextern. See root CMakeLists.txt
#
# ----------------------------------------------------------------------------
project(cvextern)

INCLUDE_DIRECTORIES("${OPENCV_SUBFOLDER}/include/opencv")
INCLUDE_DIRECTORIES("${OPENCV_SUBFOLDER}/include")
INCLUDE_DIRECTORIES("${CMAKE_BINARY_DIR}/opencv/3rdparty/zlib")

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

CREATE_VECTOR_CS("DMatch" "cv::DMatch" "MDMatch" "struct" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util Emgu.CV.Util)
CREATE_VECTOR_CS("Triangle2DF" "cv::Vec6f" "Triangle2DF" "struct" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util Emgu.CV.Util)

IF(HAVE_opencv_latentsvm)
  CREATE_VECTOR_CS("ObjectDetection" "cv::lsvm::LSVMDetector::ObjectDetection" "MCvObjectDetection" "struct" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util Emgu.CV.Util "#include \"opencv2/latentsvm.hpp\"")
ENDIF()

IF(HAVE_opencv_text)
  CREATE_VECTOR_CS("ERStat" "cv::text::ERStat" "MCvERStat" "struct" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/Text Emgu.CV.Text "#include \"opencv2/text/erfilter.hpp\"")
  CREATE_VECTOR_CS("VectorOfERStat" "std::vector< cv::text::ERStat >" "MCvERStat" "vector" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/Text Emgu.CV.Text "#include \"opencv2/text/erfilter.hpp\"")
ENDIF()

IF(HAVE_opencv_line_descriptor)
  CREATE_VECTOR_CS("KeyLine" "cv::line_descriptor::KeyLine" "MKeyLine" "struct" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/LineDescriptor Emgu.CV.LineDescriptor "#include \"opencv2/line_descriptor.hpp\"")
ENDIF()

IF(HAVE_opencv_videoio)
  CREATE_VECTOR_CS("ColorPoint" "ColorPoint" "ColorPoint" "struct" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util Emgu.CV.Util "#include \"videoio_c_extra.h\"")
ENDIF()

IF(EMGU_CV_WITH_TESSERACT)
  CREATE_VECTOR_CS("TesseractResult" "TesseractResult" "TesseractResult" "struct" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.OCR/ Emgu.CV.OCR "#include \"tesseract_c.h\"")
ENDIF()

CREATE_VECTOR_CS("Mat" "cv::Mat" "Mat" "object" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util Emgu.CV.Util)
CREATE_VECTOR_CS("UMat" "cv::UMat" "UMat" "object" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util Emgu.CV.Util)
CREATE_VECTOR_CS("CvString" "cv::String" "CvString" "object" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util Emgu.CV.Util)

CREATE_VECTOR_CS("VectorOfPoint" "std::vector< cv::Point >" "Point" "vector" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util Emgu.CV.Util)
CREATE_VECTOR_CS("VectorOfPointF" "std::vector< cv::Point2f >" "PointF" "vector" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util Emgu.CV.Util)
CREATE_VECTOR_CS("VectorOfPoint3D32F" "std::vector< cv::Point3f >" "MCvPoint3D32f" "vector" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util Emgu.CV.Util)
CREATE_VECTOR_CS("VectorOfInt" "std::vector< int >" "int" "vector" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util Emgu.CV.Util)
CREATE_VECTOR_CS("VectorOfDMatch" "std::vector< cv::DMatch >" "MDMatch" "vector" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util Emgu.CV.Util)

CREATE_VECTOR_CS("OclPlatformInfo" "cv::ocl::PlatformInfo" "Ocl.PlatformInfo" "object" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util Emgu.CV.Util "#include \"opencv2/core/ocl.hpp\"")
IF(NOT IOS)
  CREATE_VECTOR_CS("GpuMat" "cv::cuda::GpuMat" "GpuMat" "object" ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Cuda Emgu.CV.Cuda "#include \"opencv2/core/cuda.hpp\"")
ENDIF()

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
# * act1: [c++] {obj}.${PROPERTY_NAME}(val); [C#] {obj}.${CS_FUNCTION_NAME}(val); where val is a value / structure
# * act1obj: [c++] {obj}.${PROPERTY_NAME}(val); [C#] {obj}.${CS_FUNCTION_NAME}(val); where val is an object
# * element: [c++] {obj}.${PROPERTY_NAME}; [C#] {obj}.${CS_FUNCTION_NAME} {get; set;}
# * elementR: [c++] {obj}.${PROPERTY_NAME}; [C#] {obj}.${CS_FUNCTION_NAME} {get;}
#################################################################################
MACRO(CREATE_OCV_CLASS_PROPERTY fname csfname cname_full cname pnames ptypes mtypes cs_func_names csptypes csp_docs cs_namespace cs_invoke_class cs_class_name)
  SET(FILE_NAME ${fname})
  SET(CLASS_NAME_FULL ${cname_full})
  SET(CLASS_NAME ${cname})
  SET(PROPERTY_NAMES ${pnames})
  SET(PROPERTY_TYPES ${ptypes})
  SET(CS_FUNCTION_NAMES ${cs_func_names})
  SET(MARSHAL_TYPES ${mtypes})
  SET(CS_PROPERTY_TYPES ${csptypes})
  SET(CS_PROPERTY_DOCS ${csp_docs})
  SET(HEADER_ADDITIONAL_INCLUDE "")
  SET(SOURCE_ADDITIONAL_CODE "")
  
  SET(extra_macro_args ${ARGN})
  # Did we get any optional args?
  LIST(LENGTH extra_macro_args num_extra_args)
  IF (${num_extra_args} GREATER 0)
    list(GET extra_macro_args 0 additional_c_header)
    SET(HEADER_ADDITIONAL_INCLUDE ${additional_c_header})
  ENDIF()
  IF (${num_extra_args} GREATER 1)
    list(GET extra_macro_args 1 additional_c_code)
    SET(SOURCE_ADDITIONAL_CODE ${additional_c_code})
  ENDIF()
  #MESSAGE(STATUS "-------------------- PROPERTY_NAMES: ${PROPERTY_NAMES}")
  SET(C_HEADER_SOURCE "${HEADER_ADDITIONAL_INCLUDE}")
  SET(C_SOURCE "${SOURCE_ADDITIONAL_CODE} #include \"${fname}.h\"")
  SET(CS_SOURCE "//----------------------------------------------------------------------------
//  This file is automatically generated, do not modify.      
//----------------------------------------------------------------------------

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
	SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, cv::String* str);  
     ")
	SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, cv::String* str) { *str = obj->${PROPERTY_NAME}(); }   
     ")
	
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
	SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(${PROPERTY_TYPE}) cve${CLASS_NAME}${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj);  
     ")
	SET(C_SOURCE "${C_SOURCE}
${PROPERTY_TYPE} cve${CLASS_NAME}${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj) { return obj->${PROPERTY_NAME}(); }   
     ")
	
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
      SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value);
CVAPI(void) cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value);     
     ")
      SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value) { ${PROPERTY_TYPE} p = obj->get${PROPERTY_NAME}(); memcpy(value, &p, sizeof(${PROPERTY_TYPE})); }
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value) { obj->set${PROPERTY_NAME}( *value ); }     
     ")
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
     ELSEIF("${MARSHAL_TYPE}" STREQUAL "structR")
      SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value);
     ")
      SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value) { ${PROPERTY_TYPE} p = obj->get${PROPERTY_NAME}(); memcpy(value, &p, sizeof(${PROPERTY_TYPE})); }
     ")
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
    ELSEIF(${MARSHAL_TYPE} STREQUAL "propW")
      SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE} value);     
     ")
      SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE} value) { obj->set${PROPERTY_NAME}( value ); }     
     ")
      
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
     public void Set${CS_FUNCTION_NAME}(${CS_PROPERTY_TYPE} value)
     {
        ${cs_invoke_class}.cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(_ptr, value); 
     }
     ")
    ELSEIF(${MARSHAL_TYPE} STREQUAL "act1")
      SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE} value);     
     ")
      SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE} value) { obj->${PROPERTY_NAME}( value ); }     
     ")
      
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
     public void ${CS_FUNCTION_NAME}(${CS_PROPERTY_TYPE} value)
     {
        ${cs_invoke_class}.cve${CLASS_NAME}${CS_FUNCTION_NAME}(_ptr, value); 
     }
     ")
    ELSEIF(${MARSHAL_TYPE} STREQUAL "act1obj")
      SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(void) cve${CLASS_NAME}${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value);     
     ")
      SET(C_SOURCE "${C_SOURCE}
void cve${CLASS_NAME}${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE}* value) { obj->${PROPERTY_NAME}( *value ); }     
     ")
      
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
     public void ${CS_FUNCTION_NAME}(${CS_PROPERTY_TYPE} value)
     {
        ${cs_invoke_class}.cve${CLASS_NAME}${CS_FUNCTION_NAME}(_ptr, value); 
     }
     ")
    ELSEIF(${MARSHAL_TYPE} STREQUAL "propR")
      
      SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(${PROPERTY_TYPE}) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj);  
     ")
      SET(C_SOURCE "${C_SOURCE}
${PROPERTY_TYPE} cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj) { return obj->get${PROPERTY_NAME}(); }   
     ")
      
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
    ELSEIF("${MARSHAL_TYPE}" STREQUAL "element")
      IF (${PROPERTY_TYPE} STREQUAL "cv::Mat") #special handling for functions that returns Mats
	SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(${PROPERTY_TYPE}*) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj);
     ")
	SET(C_SOURCE "${C_SOURCE}
${PROPERTY_TYPE}* cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj) { return &(obj->${PROPERTY_NAME}); }
     ")
	
	SET(CS_SOURCE "${CS_SOURCE}
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] ${MARSHAL_RETURN}
     internal static extern IntPtr cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(IntPtr obj);
     ")
	SET(CS_CLASS_SOURCE "${CS_CLASS_SOURCE}
     /// <summary>
     /// ${CS_DOCUMENTATION}
     /// </summary>
     public ${CS_PROPERTY_TYPE} ${CS_FUNCTION_NAME}
     {
        get { return new ${CS_PROPERTY_TYPE}( ${cs_invoke_class}.cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(_ptr), false); } 
     }
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
      SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(${PROPERTY_TYPE}) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj);
     ")
      SET(C_SOURCE "${C_SOURCE}
${PROPERTY_TYPE} cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj) { return obj->${PROPERTY_NAME}; }
     ")
      
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
    ELSE() # for "prop" type
      SET(C_HEADER_SOURCE "${C_HEADER_SOURCE}
CVAPI(${PROPERTY_TYPE}) cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj);
CVAPI(void) cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE} value);     
     ")
      SET(C_SOURCE "${C_SOURCE}
${PROPERTY_TYPE} cve${CLASS_NAME}Get${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj) { return obj->get${PROPERTY_NAME}(); }
void cve${CLASS_NAME}Set${CS_FUNCTION_NAME}(${CLASS_NAME_FULL}* obj, ${PROPERTY_TYPE} value) { obj->set${PROPERTY_NAME}( value ); }     
     ")
      
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
  ENDFOREACH()
  SET(CS_CLASS_SOURCE "${CS_CLASS_SOURCE}
   }")
  SET(CS_SOURCE "${CS_SOURCE}
   }

   ${CS_CLASS_SOURCE}
}")
  #MESSAGE(STATUS "-------------------- CS_CLASS_SOURCE: ${CS_CLASS_SOURCE}")
  WRITE_IF_DIFFERENT("${csfname}" "${CS_SOURCE}")
  WRITE_IF_DIFFERENT("${PROJECT_SOURCE_DIR}/${fname}.h" "${C_HEADER_SOURCE}")
  WRITE_IF_DIFFERENT("${PROJECT_SOURCE_DIR}/${fname}.cpp" "${C_SOURCE}")
  
ENDMACRO()

############################### core code gen START ##############################
IF(HAVE_opencv_core)
  CREATE_OCV_CLASS_PROPERTY( 
    "core/mat_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/MatGenerated.cs"
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
    "#include \"mat_c.h\"")
  
  CREATE_OCV_CLASS_PROPERTY( 
    "core/umat_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/UMatGenerated.cs"
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
    "#include \"umat_c.h\"")
  
  CREATE_OCV_CLASS_PROPERTY( 
    "core/input_array_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util/InputArrayGenerated.cs"
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
    "#include \"core_c_extra.h\"")
  
  CREATE_OCV_CLASS_PROPERTY( 
    "core/output_array_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util/OutputArrayGenerated.cs"
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
    "#include \"core_c_extra.h\"")
  
  CREATE_OCV_CLASS_PROPERTY( 
    "core/ocl_device_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Ocl/DeviceGenerated.cs"
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
    "#include \"ocl_c.h\"")
  
  CREATE_OCV_CLASS_PROPERTY( 
    "core/ocl_platform_info_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Ocl/PlatformInfoGenerated.cs"
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
    "#include \"ocl_c.h\"")
  
  CREATE_OCV_CLASS_PROPERTY( 
    "core/ocl_kernel_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Ocl/KernelGenerated.cs"
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
    "#include \"ocl_c.h\"")
  
  CREATE_OCV_CLASS_PROPERTY( 
    "core/gpumat_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Cuda/GpuMatGenerated.cs"
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
    "#include \"opencv2/core/cuda.hpp\"")
ENDIF()
############################### core code gen END ################################

############################### features2d code gen START ##############################
IF(HAVE_opencv_features2d)
  CREATE_OCV_CLASS_PROPERTY( 
    "features2d/SimpleBlobDetector_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Features2D/SimpleBlobDetectorParamsGenerated.cs"
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
    "#include \"features2d_c.h\"")
ENDIF()
############################### features2d code gen END ################################

############################### ml code gen START ##############################
IF(HAVE_opencv_ml)
  CREATE_OCV_CLASS_PROPERTY( 
    "ml/em_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Ml/EMGenerated.cs"
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
    "#include \"ml_c.h\"")
  
  CREATE_OCV_CLASS_PROPERTY( 
    "ml/svm_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Ml/SVMGenerated.cs"
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
    "#include \"ml_c.h\"")
	
  CREATE_OCV_CLASS_PROPERTY( 
    "ml/svmsgd_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Ml/SVMSGDGenerated.cs"
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
    "#include \"ml_c.h\"")	
  
  CREATE_OCV_CLASS_PROPERTY( 
    "ml/knearest_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Ml/KNearestGenerated.cs"
    "cv::ml::KNearest" 
    "KNearest" 
    "DefaultK;IsClassifier;Emax;AlgorithmType" 
    "int;bool;int;int" 
    "prop;prop;prop;prop"
    "DefaultK;IsClassifier;Emax;AlgorithmType" 
    "int;bool;int;int"
    "Default number of neighbors to use in predict method;
  Whether classification or regression model should be trained;
  Parameter for KDTree implementation;
  Algorithm type"
    "Emgu.CV.ML"
    "MlInvoke"
    "KNearest"
    "#include \"ml_c.h\"")
  
  CREATE_OCV_CLASS_PROPERTY( 
    "ml/ann_mlp_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Ml/ANN_MLPGenerated.cs"
    "cv::ml::ANN_MLP" 
    "ANN_MLP" 
    "TermCriteria;BackpropWeightScale;BackpropMomentumScale;RpropDW0;RpropDWPlus;RpropDWMinus;RpropDWMin;RpropDWMax" 
    "CvTermCriteria;double;double;double;double;double;double;double" 
    "struct;prop;prop;prop;prop;prop;prop;prop"
    "TermCriteria;BackpropWeightScale;BackpropMomentumScale;RpropDW0;RpropDWPlus;RpropDWMinus;RpropDWMin;RpropDWMax" 
    "MCvTermCriteria;double;double;double;double;double;double;double"
    "Termination criteria of the training algorithm;
  BPROP: Strength of the weight gradient term;
  BPROP: Strength of the momentum term (the difference between weights on the 2 previous iterations);
  RPROP: Initial value Delta_0 of update-values Delta_{ij};
  RPROP: Increase factor;
  RPROP: Decrease factor;
  RPROP: Update-values lower limit;
  RPROP: Update-values upper limit"
    "Emgu.CV.ML"
    "MlInvoke"
    "ANN_MLP"
    "#include \"ml_c.h\"")
  
  CREATE_OCV_CLASS_PROPERTY( 
    "ml/logistic_regression_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Ml/LogisticRegressionGenerated.cs"
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
    "#include \"ml_c.h\"")
  
  CREATE_OCV_CLASS_PROPERTY( 
    "ml/rtrees_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Ml/RTreesGenerated.cs"
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
    "#include \"ml_c.h\"")
  
  CREATE_OCV_CLASS_PROPERTY( 
    "ml/dtree_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Ml/DTreesGenerated.cs"
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
    "#include \"ml_c.h\"")
  
  CREATE_OCV_CLASS_PROPERTY( 
    "ml/boost_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Ml/BoostGenerated.cs"
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
    "#include \"ml_c.h\"")
ENDIF()
############################### ml code gen END ################################

############################### video code gen START ##############################
IF(HAVE_opencv_video)
  CREATE_OCV_CLASS_PROPERTY( 
    "video/dual_tvl1_opticalflow_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Video/DualTVL1OpticalFlowGenerated.cs"
    "cv::DualTVL1OpticalFlow" 
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
    "#include \"video_c.h\"")
  
  CREATE_OCV_CLASS_PROPERTY( 
    "video/kalmanfilter_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Video/KalmanFilterGenerated.cs"
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
    "#include \"video_c.h\"")
ENDIF()
############################### video code gen END ################################

############################### videoio code gen START ##############################
IF(HAVE_opencv_videoio)
  CREATE_OCV_CLASS_PROPERTY( 
    "videoio/video_capture_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Capture/VideoCaptureGenerated.cs"
    "cv::VideoCapture" 
    "VideoCapture" 
    "isOpened" 
    "bool" 
    "val"
    "IsOpened" 
    "bool"
    "True if the camera is opened"
    "Emgu.CV"
    "CvInvoke"
    "VideoCapture"
    "#include \"videoio_c_extra.h\"")
  
ENDIF()
############################### videoio code gen END ################################

############################### optflow code gen START ##############################
IF(HAVE_opencv_optflow)
  CREATE_OCV_CLASS_PROPERTY( 
    "optflow/disopticalflow_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Optflow/DISOpticalFlowGenerated.cs"
    "cv::optflow::DISOpticalFlow" 
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
    "#include \"optflow_c.h\"")
  
  CREATE_OCV_CLASS_PROPERTY( 
    "optflow/variational_refinement_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Optflow/VariationalRefinementGenerated.cs"
    "cv::optflow::VariationalRefinement" 
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
    "#include \"optflow_c.h\"")
  
ENDIF()
############################### optflow code gen END ################################

############################### photo code gen START ##############################
IF(HAVE_opencv_photo)
  CREATE_OCV_CLASS_PROPERTY( 
    "photo/Tonemap_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Photo/TonemapGenerated.cs"
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
    "#include \"opencv2/photo.hpp\"")
		
  CREATE_OCV_CLASS_PROPERTY( 
    "photo/TonemapDurand_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Photo/TonemapDragoGenerated.cs"
    "cv::TonemapDurand" 
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
    "Emgu.CV"
    "CvInvoke"
    "TonemapDurand"
    "#include \"opencv2/photo.hpp\"")

  CREATE_OCV_CLASS_PROPERTY( 
    "photo/TonemapReinhard_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Photo/TonemapReinhardGenerated.cs"
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
    "#include \"opencv2/photo.hpp\"")	
	
  CREATE_OCV_CLASS_PROPERTY( 
    "photo/TonemapDrago_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Photo/TonemapDurandGenerated.cs"
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
    "#include \"opencv2/photo.hpp\"")		
	
  CREATE_OCV_CLASS_PROPERTY( 
    "photo/TonemapMantiuk_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Photo/TonemapMantiukGenerated.cs"
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
    "#include \"opencv2/photo.hpp\"")	
ENDIF()
############################### photo code gen END ##############################


############################### xfeatures2d code gen START ##############################
IF(HAVE_opencv_xfeatures2d)
   CREATE_OCV_CLASS_PROPERTY( 
    "xfeatures2d/pct_compute_signature_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/XFeatures2D/PCTComputeSignatureGenerated.cs"
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
    "#include \"xfeatures2d_c.h\"")
  
ENDIF()
############################### xfeatures2d code gen END ################################

############################### stitching code gen START ##############################
IF(HAVE_opencv_stitching)
   CREATE_OCV_CLASS_PROPERTY( 
    "stitching/stitching_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Stitching/StitcherGenerated.cs"
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
    "#include \"stitching_c.h\"")
  
ENDIF()
############################### stitching code gen END ################################

############################### shape code gen START ##############################
IF(HAVE_opencv_shape)
   CREATE_OCV_CLASS_PROPERTY( 
    "shape/ShapeContextDistanceExtractor_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Shape/ShapeContextDistanceExtractorGenerated.cs"
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
    "#include \"shape_c.h\"")
  
ENDIF()
############################### shape code gen END ################################

############################### cudaimgproc code gen START ##############################
IF(HAVE_opencv_cudaimgproc)
  CREATE_OCV_CLASS_PROPERTY( 
    "cudaimgproc/cuda_hough_lines_detector_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Cuda/Imgproc/CudaHoughLinesDetectorGenerated.cs"
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
    "#include \"opencv2/cudaimgproc.hpp\"")
ENDIF()
############################### cudaimgproc code gen END ##############################

############################### cudaobjdetect code gen START ##############################
#IF(HAVE_opencv_cudaobjdetect)
  CREATE_OCV_CLASS_PROPERTY( 
    "cudaobjdetect/cuda_hog_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Cuda/Objdetect/CudaHOGGenerated.cs"
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
    "#include \"opencv2/cudaobjdetect.hpp\"")
  CREATE_OCV_CLASS_PROPERTY( 
    "cudaobjdetect/cuda_cascade_classifier_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Cuda/Objdetect/CudaCascadeClassifierGenerated.cs"
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
    "#include \"opencv2/cudaobjdetect.hpp\"")
#ENDIF()
############################### cudaobjdetect code gen END ##############################

############################### plot code gen START ##############################
IF(HAVE_opencv_plot)
  CREATE_OCV_CLASS_PROPERTY( 
    "plot/plot2d_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/Plot/Plot2dGenerated.cs"
    "cv::plot::Plot2d" 
    "Plot2d" 
    "MinX;MinY;MaxX;MaxY;PlotLineWidth" 
    "double;double;double;double;int" 
    "propW;propW;propW;propW;propW"
    "MinX;MinY;MaxX;MaxY;PlotLineWidth" 
    "double;double;double;double;int"
    "Min X;
    Min Y;
    Max X;
    Max Y;
    Plot line width"
    "Emgu.CV.Plot"
    "PlotInvoke"
    "Plot2d"
    "#include \"opencv2/plot.hpp\"")
ENDIF()
############################### plot code gen END ##############################

############################### xphoto code gen START ##############################
IF(HAVE_opencv_xphoto)
  CREATE_OCV_CLASS_PROPERTY( 
    "xphoto/simplewb_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/XPhoto/SimpleWBGenerated.cs"
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
    "#include \"opencv2/xphoto.hpp\"")
	
  CREATE_OCV_CLASS_PROPERTY( 
    "xphoto/grayworldwb_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/XPhoto/GrayworldWBGenerated.cs"
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
    "#include \"opencv2/xphoto.hpp\"")

	CREATE_OCV_CLASS_PROPERTY( 
    "xphoto/learningbasedwb_property" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/XPhoto/LearningBasedWBGenerated.cs"
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
    "#include \"opencv2/xphoto.hpp\"")
ENDIF()
############################### xphoto code gen END ##############################

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

FOREACH(CVEXTERN_OPTIONAL_DEP ${OPENCV_MODULE_NAMES})
  #MESSAGE("Retriveing source folder for project :  opencv_${CVEXTERN_OPTIONAL_DEP}")
  #GET_TARGET_PROPERTY(OPENCV_MODULE_SOURCE_DIR opencv_${CVEXTERN_OPTIONAL_DEP} PROJECT_SOURCE_DIR)
  IF (HAVE_opencv_${CVEXTERN_OPTIONAL_DEP})
    INCLUDE_DIRECTORIES("${opencv_${CVEXTERN_OPTIONAL_DEP}_SOURCE_DIR}/include")
  ENDIF()
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
    SET(INTEL_ICC_REDIST_PATH "$ENV{ICPP_COMPILER17}redist/intel64_win/compiler/")
  ELSE()
    SET(INTEL_ICC_REDIST_PATH "$ENV{ICPP_COMPILER17}redist/ia32_win/compiler/")
  ENDIF()
  STRING(REGEX REPLACE "\\\\" "/" INTEL_ICC_REDIST_PATH ${INTEL_ICC_REDIST_PATH})
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
SET(LEPT_ROOT_DIR "${PROJECT_SOURCE_DIR}/tesseract/libtesseract/leptonica/leptonica-1.74")

IF(ANDROID)
  IF(EMGU_CV_WITH_TESSERACT)
    ADD_SUBDIRECTORY(tesseract/libtesseract)
    IF(TESSERACT_FOUND)
    ELSE()
      SET(TESSERACT_INCLUDE_DIRS 
        "${TESSERACT_OCR_ROOT_DIR}/ccutil" 
        "${TESSERACT_OCR_ROOT_DIR}/api" 
        "${TESSERACT_OCR_ROOT_DIR}/ccmain" 
        "${TESSERACT_OCR_ROOT_DIR}/ccstruct" 
        "${LEPT_ROOT_DIR}/src" 
        "${PROJECT_SOURCE_DIR}/tesseract")
      #SET(TESSERACT_PROJECTS libleptonica tesseract_ccutil tesseract_viewer tesseract_cutil tesseract_opencl tesseract_ccstruct tesseract_dict tesseract_classify tesseract_wordrec tesseract_arch tesseract_textord tesseract_lstm tesseract_api tesseract_ccmain)
      SET(TESSERACT_PROJECTS tesseract_api tesseract_ccmain tesseract_lstm tesseract_textord tesseract_arch tesseract_wordrec tesseract_classify tesseract_dict tesseract_ccstruct tesseract_opencl tesseract_cutil tesseract_viewer tesseract_ccutil libleptonica)
    ENDIF()
  ENDIF()
ELSE() #ELSEIF(WIN32 OR APPLE)
  IF(EMGU_CV_WITH_TESSERACT)
    ADD_SUBDIRECTORY(tesseract/libtesseract)
    IF(TESSERACT_FOUND)
    ELSE()
      SET(TESSERACT_INCLUDE_DIRS 
        "${TESSERACT_OCR_ROOT_DIR}/ccutil" 
        "${TESSERACT_OCR_ROOT_DIR}/api" 
        "${TESSERACT_OCR_ROOT_DIR}/ccmain" 
        "${TESSERACT_OCR_ROOT_DIR}/ccstruct" 
        "${LEPT_ROOT_DIR}/src" 
        "${PROJECT_SOURCE_DIR}/tesseract")
      #SET(TESSERACT_PROJECTS libleptonica tesseract_ccutil tesseract_viewer tesseract_cutil tesseract_opencl tesseract_ccstruct tesseract_dict tesseract_classify tesseract_wordrec tesseract_arch tesseract_textord tesseract_lstm tesseract_api tesseract_ccmain)
      SET(TESSERACT_PROJECTS tesseract_api tesseract_ccmain tesseract_lstm tesseract_textord tesseract_arch tesseract_wordrec tesseract_classify tesseract_dict tesseract_ccstruct tesseract_opencl tesseract_cutil tesseract_viewer tesseract_ccutil libleptonica)
    ENDIF()
    ADD_DEFINITIONS(-D__MSW32__)
  ENDIF()
ENDIF()
#ELSE()
#  SET(CMAKE_MODULE_PATH "${PROJECT_SOURCE_DIR}/../cmake/modules")
#  FIND_PACKAGE(TESSERACT)
#  IF(TESSERACT_FOUND)
#    SET(EMGU_CV_WITH_TESSERACT ON CACHE BOOL "BUILD OCR")
#  ELSE()
#    SET(EMGU_CV_WITH_TESSERACT OFF CACHE BOOL "BUILD OCR")
#    SET(TESSERACT_INCLUDE_DIRS "")
#    SET(TESSERACT_PROJECTS "")
#  ENDIF()
#ENDIF()

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
  IF(TARGET_ARCH_64)
    LINK_DIRECTORIES(${CUDA_TOOLKIT_ROOT_DIR}/lib/x64)
  ELSE()
    LINK_DIRECTORIES(${CUDA_TOOLKIT_ROOT_DIR}/lib/Win32)
  ENDIF()
  INCLUDE_DIRECTORIES(${CUDA_TOOLKIT_ROOT_DIR}/include)
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
	SET(CUDA_NPP_DLLS 
	  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/nppc${CUDA_POSTFIX}_${npp_major}${npp_minor}.dll"
      "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/nppi${CUDA_POSTFIX}_${npp_major}${npp_minor}.dll"
	  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/nppial${CUDA_POSTFIX}_${npp_major}${npp_minor}.dll"
	  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/nppicc${CUDA_POSTFIX}_${npp_major}${npp_minor}.dll"
	  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/nppicom${CUDA_POSTFIX}_${npp_major}${npp_minor}.dll"
	  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/nppidei${CUDA_POSTFIX}_${npp_major}${npp_minor}.dll"
	  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/nppif${CUDA_POSTFIX}_${npp_major}${npp_minor}.dll"
	  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/nppig${CUDA_POSTFIX}_${npp_major}${npp_minor}.dll"
	  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/nppim${CUDA_POSTFIX}_${npp_major}${npp_minor}.dll"
	  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/nppist${CUDA_POSTFIX}_${npp_major}${npp_minor}.dll"
	  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/nppisu${CUDA_POSTFIX}_${npp_major}${npp_minor}.dll"
	  "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/nppitc${CUDA_POSTFIX}_${npp_major}${npp_minor}.dll"
      "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/npps${CUDA_POSTFIX}_${npp_major}${npp_minor}.dll")
	FOREACH(CUDA_NPP_DLL ${CUDA_NPP_DLLS})
	  IF(EXISTS "${CUDA_NPP_DLL}")
		LIST(APPEND CVEXTERN_DEPENDENCY_DLLS "${CUDA_NPP_DLL}")
	  ENDIF()
    ENDFOREACH()
	
	LIST(APPEND CVEXTERN_DEPENDENCY_DLLS
      "${CUDA_TOOLKIT_ROOT_DIR}/bin/cudart${CUDA_POSTFIX}_${npp_major}${npp_minor}.dll"
      )
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
IF(HAVE_opencv_xfeatures2d)
  IF( (NOT WITH_CUDA) OR (NOT (TARGET opencv_cudaimgproc)) )
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
    DESTINATION libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}
    COMPONENT libs)
ENDIF()

IF(ANDROID OR APPLE)
  FILE(GLOB extern_srcs_excludes "${PROJECT_SOURCE_DIR}/tiffio.cpp")
  LIST(REMOVE_ITEM extern_srcs ${extern_srcs_excludes})
  #MESSAGE(STATUS "extern_srcs: ${extern_srcs}")
  FILE(GLOB extern_hdrs_excludes "${PROJECT_SOURCE_DIR}/tiffio_c.h")
  LIST(REMOVE_ITEM extern_hdrs ${extern_hdrs_excludes})
ENDIF()

set(the_target ${PROJECT_NAME})
IF(IOS)
  add_library(${the_target} STATIC ${extern_srcs} ${extern_hdrs} )
ELSE()
  add_library(${the_target} SHARED ${extern_srcs} ${extern_hdrs} )
  IF (APPLE)
    set_target_properties(${the_target} PROPERTIES MACOSX_RPATH ON)    
    #SET(DYLIBBUNDLER_PATH "${PROJECT_SOURCE_DIR}/../platform/osx/dylibbundler")
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

# For dynamic link numbering convenions
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
  IF (NETFX_CORE OR WIN32)
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

IF(WIN32 OR APPLE)
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
  # ${ZLIB_LIBRARY} 
  #  xtiff
  ${TIFF_LIBRARY} ${GEOTIFF_LIBRARY} ${CVBLOB_LIBRARY} )

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
  # For Android, tell the linker to transform all the symbols in the static libraries to hidden.
  # This can significantlt reduce the binary size
  set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -Wl,--exclude-libs=ALL")
  set(CMAKE_C_FLAGS "${CMAKE_CXX_FLAGS} -Wl,--exclude-libs=ALL")
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

if(BUILD_PNG)
  target_link_libraries(${the_target} libpng)
endif()

if(BUILD_TIFF)
  target_link_libraries(${the_target} libtiff)
endif()

if(BUILD_JPEG)
  target_link_libraries(${the_target} libjpeg)
endif()

# Add the required libraries for linking:
target_link_libraries(${the_target} ${OPENCV_LINKER_LIBS} 
  ${ZLIB_LIBRARY} 
  #  xtiff 
  ${CVBLOB_LIBRARY})

#IF(EMGU_CV_WITH_TESSERACT)
#  ADD_DEPENDENCIES(${TESSERACT_PROJECTS})
#ENDIF()

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
  INSTALL(TARGETS ${the_target} 
    RUNTIME DESTINATION libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT libs
    LIBRARY DESTINATION libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT libs
    ARCHIVE DESTINATION libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT libs)
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
    RUNTIME DESTINATION libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT libs
    LIBRARY DESTINATION libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT libs
    ARCHIVE DESTINATION libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT libs)
ENDIF()

#add_subdirectory(gpu)
