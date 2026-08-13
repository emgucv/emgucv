# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
#
#  DepthAI (OAK-1/OAK-D) property invocations, source gathering, and the
#  Windows libusb runtime dependency, gated by EMGU_CV_WITH_DEPTHAI.
#  Included from Emgu.CV.Extern/CMakeLists.txt; CMAKE_CURRENT_SOURCE_DIR still
#  refers to Emgu.CV.Extern/ here (include() does not change it).
# ----------------------------------------------------------------------------

############################### DEPTHAI START ##############################

  CREATE_OCV_CLASS_PROPERTY( 
    "depthai/color_camera" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/DepthAI/ColorCamera.g.cs"
    "dai::node::ColorCamera" 
    "ColorCamera" 
    "Interleaved" 
    "bool"
    "prop"
    "Interleaved" 
    "bool"
    "True if the image pixels are interleaved"
    "Emgu.CV.Dai"
    "DaiInvoke"
    "ColorCamera"
	""
    "#include \"depthai_c.h\""
	""
	"" 
	${EMGU_CV_WITH_DEPTHAI})

  CREATE_OCV_CLASS_PROPERTY( 
    "depthai/mono_camera" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/DepthAI/MonoCamera.g.cs"
    "dai::node::MonoCamera" 
    "MonoCamera" 
    "Fps" 
    "float"
    "prop"
    "Fps" 
    "float"
    "The rate at which camera should produce frames"
    "Emgu.CV.Dai"
    "DaiInvoke"
    "MonoCamera"
	""
    "#include \"depthai_c.h\""
	""
	"" 
	${EMGU_CV_WITH_DEPTHAI})

  CREATE_OCV_CLASS_PROPERTY( 
    "depthai/img_frame" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/DepthAI/ImgFrame.g.cs"
    "dai::ImgFrame" 
    "ImgFrame" 
    "Width;Height;InstanceNum;Category;SequenceNum" 
    "uint32_t;uint32_t;uint32_t;uint32_t;int64_t"
    "prop;prop;prop;prop;prop"
    "Width;Height;InstanceNum;Category;SequenceNum" 
    "UInt32;UInt32;UInt32;UInt32;Int64"
    "Image width;
    Image height;
    Instance number;
    Image category;
    Image sequence number"
    "Emgu.CV.Dai"
    "DaiInvoke"
    "ImgFrame"
	""
    "#include \"depthai_c.h\""
	""
	"" 
	${EMGU_CV_WITH_DEPTHAI})

  CREATE_OCV_CLASS_PROPERTY( 
    "depthai/xlinkout" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/DepthAI/XLinkOut.g.cs"
    "dai::node::XLinkOut" 
    "XLinkOut" 
    "StreamName;FpsLimit;MetadataOnly" 
    "cv::String;float;bool"
    "prop;prop;prop"
    "StreamName;FpsLimit;MetadataOnly" 
    "String;float;bool"
    "The Stream Name;
    A message sending limit. It's approximated from specified rate;
    Specify whether to transfer only messages attributes and not buffer data"
    "Emgu.CV.Dai"
    "DaiInvoke"
    "XLinkOut"
	""
    "#include \"depthai_c.h\""
	""
	"" 
	${EMGU_CV_WITH_DEPTHAI})

  CREATE_OCV_CLASS_PROPERTY( 
    "depthai/data_output_queue" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/DepthAI/DataOutputQueue.g.cs"
    "dai::DataOutputQueue" 
    "DataOutputQueue" 
    "Blocking;MaxSize" 
    "bool;uint32_t"
    "prop;prop"
    "Blocking;MaxSize" 
    "bool;UInt32"
    "Specifies if block or overwrite the oldest message in the queue;
    Specifies maximum number of messages in the queue"
    "Emgu.CV.Dai"
    "DaiInvoke"
    "DataOutputQueue"
	""
    "#include \"depthai_c.h\""
	""
	"" 
	${EMGU_CV_WITH_DEPTHAI})

    CREATE_OCV_CLASS_PROPERTY( 
    "depthai/neural_network" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/DepthAI/NeuralNetwork.g.cs"
    "dai::node::NeuralNetwork" 
    "NeuralNetwork" 
    "NumInferenceThreads" 
    "int"
    "prop"
    "NumInferenceThreads" 
    "int"
    "How many inference threads will be used to run the network, Zero means AUTO"
    "Emgu.CV.Dai"
    "DaiInvoke"
    "NeuralNetwork"
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

IF(WIN32)
  #MESSAGE(STATUS "++++++++++++++++++++  LIBUSB_BINARY_FILE_DIR: ${LIBUSB_BINARY_FILE_DIR}")
  IF (EMGU_CV_WITH_DEPTHAI)
    SET(LIBUSB_BINARY_FILE_DIR "${CMAKE_CURRENT_BINARY_DIR}/depthai-core/${CMAKE_BUILD_TYPE}")
    LIST(APPEND CVEXTERN_DEPENDENCY_DLLS "${LIBUSB_BINARY_FILE_DIR}/libusb-1.0.dll")
  ENDIF()
ENDIF()

############################### DEPTHAI END ################################
