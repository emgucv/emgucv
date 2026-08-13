# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
#
#  CREATE_OCV_CLASS_PROPERTY / CREATE_VECTOR_CS invocations for every core
#  and contrib OpenCV module (core, objdetect, imgproc, features, mcc, ml,
#  video, videoio, optflow, photo, xfeatures2d, stitching, shape, cuda*,
#  plot, saliency, xphoto, face, dnn, xstereo, structured_light, ximgproc,
#  surface_matching, rgbd, etc). Purely declarative macro calls generating
#  the .g.h/.g.cpp/.g.cs property wrappers -- see CLAUDE.md for how to add
#  a new entry.
#  Included from Emgu.CV.Extern/CMakeLists.txt; CMAKE_CURRENT_SOURCE_DIR still
#  refers to Emgu.CV.Extern/ here (include() does not change it).
# ----------------------------------------------------------------------------

############################### core code gen START ##############################

  CREATE_OCV_CLASS_PROPERTY( 
    "core/mat" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Core/Mat.g.cs"
    "cv::Mat" 
    "Mat" 
    "isContinuous;isSubmatrix;depth;empty;channels;pop_back;push_back;total;dims;data"
    "bool;bool;int;bool;int;int;cv::Mat;size_t;int;uchar*"
    "val;val;val;val;val;act1;act1obj;val;elementR;elementR"
    "IsContinuous;IsSubmatrix;Depth;IsEmpty;NumberOfChannels;PopBack;PushBack;Total;Dims;DataPointer"
    "bool;bool;CvEnum.DepthType;bool;int;int;Mat;IntPtr;int;IntPtr"
    "True if the data is continues;
  True if the matrix is a submatrix of another matrix;
  Depth type;
  True if the Mat is empty;
  Number of channels;
  The method removes one or more rows from the bottom of the matrix;
  Adds elements to the bottom of the matrix;
  The method returns the number of array elements (a number of pixels if the array represents an image);
  The matrix dimensionality;
  Pointer to the beginning of the raw data"
    "Emgu.CV"
    "CvInvoke"
    "Mat"
	""
    "#include \"mat_c.h\""
	""
	"" 
	${HAVE_opencv_core})
  
  CREATE_OCV_CLASS_PROPERTY( 
    "core/umat" 
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
    "core/input_array" 
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
    "core/output_array" 
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
    "core/ocl_device" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Ocl/Device.g.cs"
    "cv::ocl::Device" 
    "Device" 
    "isNVidia;isIntel;isAMD;addressBits;linkerAvailable;compilerAvailable;available;maxWorkGroupSize;maxComputeUnits;localMemSize;maxMemAllocSize;deviceVersionMajor;deviceVersionMinor;halfFPConfig;singleFPConfig;doubleFPConfig;hostUnifiedMemory;globalMemSize;image2DMaxWidth;image2DMaxHeight;type;name;version;vendorName;driverVersion;extensions;OpenCLVersion;OpenCL_C_Version;ptr;set"
    "bool;bool;bool;int;bool;bool;bool;int;int;int;int;int;int;int;int;int;bool;size_t;int;int;int;cv::String;cv::String;cv::String;cv::String;cv::String;cv::String;cv::String;void*;void*"
    "val;val;val;val;val;val;val;val;val;val;val;val;val;val;val;val;val;val;val;val;val;val;val;val;val;val;val;val;val;act1"
    "IsNVidia;IsIntel;IsAMD;AddressBits;LinkerAvailable;CompilerAvailable;Available;MaxWorkGroupSize;MaxComputeUnits;LocalMemSize;MaxMemAllocSize;DeviceVersionMajor;DeviceVersionMinor;HalfFPConfig;SingleFPConfig;DoubleFPConfig;HostUnifiedMemory;GlobalMemSize;Image2DMaxWidth;Image2DMaxHeight;Type;Name;Version;VendorName;DriverVersion;Extensions;OpenCLVersion;OpenCLCVersion;NativeDevicePointer;Set"
    "bool;bool;bool;int;bool;bool;bool;int;int;int;int;int;int;FpConfig;FpConfig;FpConfig;bool;IntPtr;int;int;DeviceType;String;String;String;String;String;String;String;IntPtr;IntPtr"
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
  The device OpenCL C version;
  Get the native device pointer;
  Set the native device pointer"
    "Emgu.CV.Ocl"
    "OclInvoke"
    "Device"
	""
    "#include \"ocl_c.h\""
	""
	""
	${HAVE_opencv_core})
  
  CREATE_OCV_CLASS_PROPERTY( 
    "core/ocl_platform_info" 
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
    "core/ocl_kernel" 
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
    "core/gpumat" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Cuda/GpuMat.g.cs"
    "cv::cuda::GpuMat" 
    "GpuMat" 
    "isContinuous;depth;empty;channels;type"
    "bool;int;bool;int;int"
    "val;val;val;val;val"
    "IsContinuous;Depth;IsEmpty;NumberOfChannels;Type"
    "bool;CvEnum.DepthType;bool;int;int"
    "True if the data is continues;
  Depth type;
  True if the matrix is empty;
  Number of channels;
  The type of the GpuMat"
    "Emgu.CV.Cuda"
    "CudaInvoke"
    "GpuMat"
	""
    "#include \"opencv2/core/cuda.hpp\"
    #include \"opencv2/core.hpp\"
    #include \"cvapi_compat.h\""
	""
	""
	${HAVE_opencv_core})

  CREATE_OCV_CLASS_PROPERTY( 
    "core/stream" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Cuda/Stream.g.cs"
    "cv::cuda::Stream" 
    "Stream" 
    "cudaPtr" 
    "void*" 
    "val"
    "CudaPtr" 
    "IntPtr"
    "Get pointer to CUDA stream"
    "Emgu.CV.Cuda"
    "CudaInvoke"
    "Stream"
	""
    "#include \"opencv2/core/cuda.hpp\"
	#include \"opencv2/core.hpp\"
    #include \"cvapi_compat.h\""
	""
	""
	${HAVE_opencv_core})

  CREATE_OCV_CLASS_PROPERTY( 
    "core/file_node" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Core/FileNode.g.cs"
    "cv::FileNode" 
    "FileNode" 
    "isNamed;empty;isNone;isSeq;isMap;isInt;isReal;isString;type"
    "bool;bool;bool;bool;bool;bool;bool;bool;int"
    "val;val;val;val;val;val;val;val;val"
    "IsNamed;IsEmpty;IsNone;IsSeq;IsMap;IsInt;IsReal;IsString;NodeType"
    "bool;bool;bool;bool;bool;bool;bool;bool;FileNode.Type"
    "Returns true if the node has a name;
	Returns true if the node is empty;
	Returns true if the node is a \"none\" object;
	Returns true if the node is a sequence;
	Returns true if the node is a mapping;
	Returns true if the node is an integer;
	Returns true if the node is a floating-point number;
	Returns true if the node is a text string;
	Gets the type of the node"
    "Emgu.CV"
    "CvInvoke"
    "FileNode"
	""
    "#include \"core_c_extra.h\""
	""
	""
	${HAVE_opencv_core})

  CREATE_OCV_CLASS_PROPERTY( 
    "core/moments" 
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
    "objdetect/QRCodeDetector" 
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

  CREATE_OCV_CLASS_PROPERTY( 
    "objdetect/FaceDetectorYN" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Objdetect/FaceDetectorYN.g.cs"
    "cv::FaceDetectorYN" 
    "FaceDetectorYN" 
    "ScoreThreshold;NMSThreshold;TopK;InputSize" 
    "float;float;int;cv::Size" 
    "propW;propW;propW;struct"
    "ScoreThreshold;NMSThreshold;TopK;InputSize" 
    "float;float;int;System.Drawing.Size"
    "The score threshold to filter out bounding boxes of score less than the given value;
     The Non-maximum-suppression threshold to suppress bounding boxes that have IoU greater than the given value;
     The number of bounding boxes to preserve from top rank based on score;
     The size for the network input, which overwrites the input size of creating model."
    "Emgu.CV"
    "CvInvoke"
    "FaceDetectorYN"
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
    "imgproc/LineIterator" 
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
    "photo/IntelligentScissorsMB"
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Photo/IntelligentScissorsMB.g.cs"
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
    "#include \"photo_c.h\""
	""
	""
	${HAVE_opencv_photo})
    


############################### imgproc code gen END ################################

############################### features code gen START ##############################
IF (NOT HAVE_opencv_features)
  SET(HAVE_opencv_features FALSE)
ENDIF()
  CREATE_OCV_CLASS_PROPERTY( 
    "features/SimpleBlobDetector" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Features/SimpleBlobDetectorParams.g.cs"
    "cv::SimpleBlobDetector::Params" 
    "SimpleBlobDetectorParams" 
    "thresholdStep;minThreshold;maxThreshold;minDistBetweenBlobs;filterByColor;blobColor;filterByArea;minArea;maxArea;filterByCircularity;minCircularity;maxCircularity;filterByInertia;minInertiaRatio;maxInertiaRatio;filterByConvexity;minConvexity;maxConvexity;minRepeatability;collectContours" 
    "float;float;float;float;bool;uchar;bool;float;float;bool;float;float;bool;float;float;bool;float;float;size_t;bool" 
    "element;element;element;element;element;element;element;element;element;element;element;element;element;element;element;element;element;element;element;element"
    "ThresholdStep;MinThreshold;MaxThreshold;MinDistBetweenBlobs;FilterByColor;blobColor;FilterByArea;MinArea;MaxArea;FilterByCircularity;MinCircularity;MaxCircularity;FilterByInertia;MinInertiaRatio;MaxInertiaRatio;FilterByConvexity;MinConvexity;MaxConvexity;MinRepeatability;CollectContours" 
    "float;float;float;float;bool;Byte;bool;float;float;bool;float;float;bool;float;float;bool;float;float;IntPtr;bool"
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
  Min Repeatability;
  Collect Contours"
    "Emgu.CV.Features"
    "FeaturesInvoke"
    "SimpleBlobDetectorParams"
	""
    "#include \"features_c.h\""
	""
	""
	${HAVE_opencv_features})
  CREATE_OCV_CLASS_PROPERTY( 
    "features/MSER" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Features/MSER.g.cs"
    "cv::MSER" 
    "MSER" 
    "Pass2Only;Delta;MinArea;MaxArea" 
    "bool;int;int;int" 
    "prop;prop;prop;prop"
    "Pass2Only;Delta;MinArea;MaxArea" 
    "bool;int;int;int"
    "Pass2 only;
    Delta;
    Min Area;
    Max Area"
    "Emgu.CV.Features"
    "FeaturesInvoke"
    "MSER"
	""
    "#include \"features_c.h\""
	""
	""
	${HAVE_opencv_features})
  CREATE_OCV_CLASS_PROPERTY(
    "features/FastFeatureDetector"
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Features/FastFeatureDetector.g.cs"
    "cv::FastFeatureDetector"
    "FastFeatureDetector"
    "Threshold;NonmaxSuppression;Type"
    "int;bool;cv::FastFeatureDetector::DetectorType"
    "prop;prop;prop"
    "Threshold;NonmaxSuppression;Type"
    "int;bool;FastFeatureDetector.DetectorType"
    "Threshold on difference between intensity of the central pixel and pixels of a circle around this pixel;
    If true, non-maximum suppression is applied to detected corners (keypoints);
    The neighborhood type"
    "Emgu.CV.Features"
    "FeaturesInvoke"
    "FastFeatureDetector"
	""
    "#include \"features_c.h\""
	""
	""
	${HAVE_opencv_features})
  CREATE_OCV_CLASS_PROPERTY(
    "features/ORB"
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Features/ORB.g.cs"
    "cv::ORB"
    "ORB"
    "MaxFeatures;ScaleFactor;NLevels;EdgeThreshold;FirstLevel;WTA_K;ScoreType;PatchSize;FastThreshold"
    "int;double;int;int;int;int;cv::ORB::ScoreType;int;int"
    "prop;prop;prop;prop;prop;prop;prop;prop;prop"
    "MaxFeatures;ScaleFactor;NLevels;EdgeThreshold;FirstLevel;WTAK;Score;PatchSize;FastThreshold"
    "int;double;int;int;int;int;ORB.ScoreType;int;int"
    "The maximum number of features to retain;
    Pyramid decimation ratio, greater than 1;
    The number of pyramid levels;
    The size of the border where the features are not detected;
    The level of pyramid to put source image to;
    The number of points that produce each element of the oriented BRIEF descriptor;
    The type of score used to rank features;
    The size of the patch used by the oriented BRIEF descriptor;
    The fast threshold"
    "Emgu.CV.Features"
    "FeaturesInvoke"
    "ORB"
	""
    "#include \"features_c.h\""
	""
	""
	${HAVE_opencv_features})

############################### features code gen END ################################

############################### mcc code gen START ##############################
IF (NOT HAVE_opencv_objdetect)
  SET(HAVE_opencv_objdetect FALSE)
ENDIF()
  CREATE_OCV_CLASS_PROPERTY( 
    "objdetect/CChecker" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Objdetect/CChecker.g.cs"
    "cv::mcc::CChecker" 
    "CChecker" 
    "Target;Cost" 
    "cv::mcc::ColorChart;float" 
    "prop;prop"
    "Target;Cost" 
    "CChecker.ColorChart;float"
    "Target;Cost"
    "Emgu.CV.Mcc"
    "MccInvoke"
    "CChecker"
	""
    "#include \"mcc_c.h\""
	""
	""
	${HAVE_opencv_objdetect})

      CREATE_OCV_CLASS_PROPERTY( 
    "objdetect/DetectorParametersMCC" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Objdetect/DetectorParametersMCC.g.cs"
    "cv::mcc::DetectorParametersMCC" 
    "DetectorParametersMCC" 
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
    "DetectorParametersMCC"
	""
    "#include \"mcc_c.h\""
	""
	""
	${HAVE_opencv_objdetect})

############################### objdetect code gen END ################################

############################### ml code gen START ##############################
IF (NOT HAVE_opencv_ml)
  SET(HAVE_opencv_ml FALSE)
ENDIF()
  CREATE_OCV_CLASS_PROPERTY( 
    "ml/em" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/Ml/EM.g.cs"
    "cv::ml::EM" 
    "EM" 
    "ClustersNumber;CovarianceMatrixType;TermCriteria" 
    "int;int;cv::TermCriteria" 
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
    "ml/svm" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/Ml/SVM.g.cs"
    "cv::ml::SVM" 
    "SVM" 
    "Type;Gamma;Coef0;Degree;C;Nu;P;Kernel;TermCriteria;KernelType" 
    "int;double;double;double;double;double;double;int;cv::TermCriteria;int" 
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
    "ml/svmsgd" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/Ml/SVMSGD.g.cs"
    "cv::ml::SVMSGD" 
    "SVMSGD" 
    "SvmsgdType;MarginType;MarginRegularization;InitialStepSize;StepDecreasingPower;TermCriteria" 
    "int;int;float;float;float;cv::TermCriteria" 
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
    "ml/knearest" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/Ml/KNearest.g.cs"
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
    "ml/ann_mlp" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/Ml/ANN_MLP.g.cs"
    "cv::ml::ANN_MLP" 
    "ANN_MLP" 
    "TermCriteria;BackpropWeightScale;BackpropMomentumScale;RpropDW0;RpropDWPlus;RpropDWMinus;RpropDWMin;RpropDWMax;AnnealInitialT;AnnealFinalT;AnnealCoolingRatio;AnnealItePerStep" 
    "cv::TermCriteria;double;double;double;double;double;double;double;double;double;double;int" 
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
    "ml/logistic_regression" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/Ml/LogisticRegression.g.cs"
    "cv::ml::LogisticRegression" 
    "LogisticRegression" 
    "LearningRate;Iterations;Regularization;TrainMethod;MiniBatchSize;TermCriteria" 
    "double;int;int;int;int;cv::TermCriteria" 
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
    "ml/rtrees" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/Ml/RTrees.g.cs"
    "cv::ml::RTrees" 
    "RTrees" 
    "MaxCategories;MaxDepth;MinSampleCount;CVFolds;UseSurrogates;Use1SERule;TruncatePrunedTree;RegressionAccuracy;CalculateVarImportance;ActiveVarCount;TermCriteria" 
    "int;int;int;int;bool;bool;bool;float;bool;int;cv::TermCriteria" 
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
    "ml/dtree" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/Ml/DTrees.g.cs"
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
    "ml/boost" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/Ml/Boost.g.cs"
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
    "video/kalmanfilter" 
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
    "video/variational_refinement" 
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
    "videoio/video_capture" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Videoio/VideoCapture.g.cs"
    "cv::VideoCapture" 
    "VideoCapture" 
    "isOpened;ExceptionMode;release" 
    "bool;bool;void" 
    "val;prop;act0"
    "IsOpened;ExceptionMode;Release" 
    "bool;bool;void"
    "True if the camera is opened;
    If True, methods raise exceptions if not successful instead of returning an error code;
    The method is automatically called by subsequent VideoCapture.Open and by VideoCapture destructor."
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
    "optflow/dual_tvl1_opticalflow" 
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
    "optflow/rlof_opticalflow_parameter" 
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
    "photo/Tonemap" 
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
    "photo/TonemapReinhard" 
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
    "photo/TonemapDrago" 
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
    "photo/TonemapMantiuk" 
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

  CREATE_OCV_CLASS_PROPERTY( 
    "photo/ColorCorrectionModel" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Photo/ColorCorrectionModel.g.cs"
    "cv::ccm::ColorCorrectionModel" 
    "ColorCorrectionModel" 
    "ColorSpace;CcmType;Loss;Distance;Linearization;LinearizationGamma;LinearizationDegree;WeightCoeff;MaxCount" 
    "cv::ccm::ColorSpace;cv::ccm::CcmType;double;cv::ccm::DistanceType;cv::ccm::LinearizationType;double;int;double;int" 
    "propW;propW;propR;propW;propW;propW;propW;propW;propW"
    "ColorSpace;CcmType;Loss;DistanceType;LinearizationType;LinearGamma;LinearDegree;WeightCoeff;MaxCount" 
    "ColorCorrectionModel.ColorSpace;ColorCorrectionModel.CcmType;double;ColorCorrectionModel.DistanceType;ColorCorrectionModel.LinearizationType;double;int;double;int"
    "Color space;
     Ccm type;
     Loss;
     The type of color distance;
     The method of linearization;
     The gamma value of gamma correction;
     The degree of linearization polynomial;
     The exponent number of L* component of the reference color in CIE Lab color space;
     Used in MinProblemSolver-DownhillSolver, terminal criteria to the algorithm"
    "Emgu.CV.Ccm"
    "CcmInvoke"
    "ColorCorrectionModel"
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
    "xfeatures2d/pct_compute_signature" 
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
    "stitching/stitching" 
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

   CREATE_OCV_CLASS_PROPERTY( 
    "stitching/camera_params" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Stitching/CameraParams.g.cs"
    "cv::detail::CameraParams" 
    "CameraParams" 
    "focal;aspect;ppx;ppy;R;t" 
    "double;double;double;double;cv::Mat;cv::Mat" 
    "element;element;element;element;element;element"
    "Focal;Aspect;Ppx;Ppy;R;T" 
    "double;double;double;double;Mat;Mat"
    "The focal length;
    The aspect ratio;
    The principal point X;
    The principal point Y;
    The rotation Mat;
    The translation Mat"
    "Emgu.CV.Stitching"
    "StitchingInvoke"
    "CameraParams"
	""
    "#include \"stitching_c.h\""
	""
	""
	${HAVE_opencv_stitching})  

CREATE_VECTOR_CS(
    "CameraParams" 
    "cv::detail::CameraParams" 
    "Emgu.CV.Stitching.CameraParams" 
    "object_not_array" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Util" 
    Emgu.CV.Stitching 
    "" 
    "#include \"stitching_c.h\"" 
    "" 
    "defined(HAVE_OPENCV_STITCHING)")
############################### stitching code gen END ################################

############################### video code gen START ##############################
IF (NOT HAVE_opencv_video)
  SET(HAVE_opencv_video FALSE)
ENDIF()

  CREATE_OCV_CLASS_PROPERTY( 
    "video/disopticalflow" 
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
    "video/BackgroundSubtractorKNN" 
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
    "video/BackgroundSubtractorMOG2" 
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

  CREATE_OCV_CLASS_PROPERTY( 
    "video/TrackerDaSiamRPN" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Video/TrackerDaSiamRPN.g.cs"
    "cv::TrackerDaSiamRPN" 
    "TrackerDaSiamRPN" 
    "TrackingScore" 
    "float" 
    "propR"
    "TrackingScore" 
    "float"
    "Tracking score"
    "Emgu.CV"
    "CvInvoke"
    "TrackerDaSiamRPN"
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
    "shape/ShapeContextDistanceExtractor" 
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
    "cudaimgproc/cuda_hough_lines_detector" 
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
    "cudaobjdetect/cuda_hog" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Cuda/Objdetect/CudaHOG.g.cs"
    "cv::cuda::HOG" 
    "CudaHOG"
    "GammaCorrection;WinSigma;NumLevels;GroupThreshold;HitThreshold;ScaleFactor;L2HysThreshold;DescriptorFormat;DescriptorSize;WinStride;BlockHistogramSize" 
    "bool;double;int;int;double;double;double;int;size_t;cv::Size;size_t" 
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
    "cudaobjdetect/cuda_cascade_classifier" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Cuda/Objdetect/CudaCascadeClassifier.g.cs"
    "cv::cuda::CascadeClassifier"
    "CudaCascadeClassifier"
    "ScaleFactor;MinNeighbors;MaxNumObjects;FindLargestObject;MaxObjectSize;MinObjectSize;ClassifierSize" 
    "double;int;int;bool;cv::Size;cv::Size;cv::Size" 
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
    "plot/plot2d" 
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
    "saliency/MotionSaliencyBinWangApr2014" 
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
    "saliency/ObjectnessBING" 
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

############################### saliency code gen END ##############################

############################### xphoto code gen START ##############################
IF (NOT HAVE_opencv_xphoto)
  SET(HAVE_opencv_xphoto FALSE)
ENDIF()
  CREATE_OCV_CLASS_PROPERTY( 
    "xphoto/simplewb" 
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
    "xphoto/grayworldwb" 
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
    "xphoto/learningbasedwb" 
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
    "xphoto/TonemapDurand" 
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
    "face/facemarklbf_params" 
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
    "face/facemarkaam_params" 
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

  CREATE_OCV_CLASS_PROPERTY(
    "face/facemarkkazemi_params"
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/Face/FacemarkKazemiParams.g.cs"
    "cv::face::FacemarkKazemi::Params"
    "FacemarkKazemiParams"
    "cascade_depth;tree_depth;num_trees_per_cascade_level;learning_rate;oversampling_amount;num_test_coordinates;lambda;num_test_splits;configfile"
    "int;int;int;float;int;int;float;int;cv::String"
    "element;element;element;element;element;element;element;element;element"
    "CascadeDepth;TreeDepth;NumTreesPerCascadeLevel;LearningRate;OversamplingAmount;NumTestCoordinates;Lambda;NumTestSplits;ConfigFile"
    "int;int;int;float;int;int;float;int;String"
    "the depth of cascade used for training;
	the max height of the regression tree built;
	number of trees fit per cascade level;
	the learning rate in gradient boosting, also referred as shrinkage;
	number of initialisations used to create training samples;
	number of test coordinates;
	a value to calculate probability of closeness of two coordinates;
	number of random test splits generated;
	the name of the file containing the values of training parameters"
    "Emgu.CV.Face"
    "FaceInvoke"
    "FacemarkKazemiParams"
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
    "dnn/net" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV/Dnn/Net.g.cs"
    "cv::dnn::Net" 
    "Net" 
    "PreferableBackend;PreferableTarget;enableFusion;enableWinograd;empty" 
    "int;int;bool;bool;bool" 
    "propW;propW;act1;act1;val"
    "PreferableBackend;PreferableTarget;EnableFusion;EnableWinograd;Empty" 
    "Backend;Target;bool;bool;bool"
    "Ask network to use specific computation backend where it supported.;
	Ask network to make computations on specific target device.;
	Enables or disables layer fusion in the network.;
    Enables or disables the Winograd compute branch. The Winograd compute branch can speed up 3x3 Convolution at a small loss of accuracy.;
	Returns true if there are no layers in the network."
    "Emgu.CV.Dnn"
    "DnnInvoke"
    "Net"
	""
	"#include \"dnn_c.h\""
	""
	""
	${HAVE_opencv_dnn})
  CREATE_OCV_CLASS_PROPERTY( 
    "dnn/layer" 
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
    "dnn/TextDetectionModel_EAST" 
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
    "dnn/TextDetectionModel_DB" 
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
    "dnn/TextRecognitionModel" 
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
    "dnn/DetectionModel" 
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

############################### xstereo code gen START ##############################
IF (NOT HAVE_opencv_xstereo)
  SET(HAVE_opencv_xstereo FALSE)
ENDIF()
  CREATE_OCV_CLASS_PROPERTY( 
    "xstereo/quasi_dense_stereo" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/XStereo/QuasiDenseStereo.g.cs"
    "cv::stereo::QuasiDenseStereo" 
    "QuasiDenseStereo" 
    "Param" 
    "cv::stereo::PropagationParameters" 
    "element"
    "Param" 
    "QuasiDenseStereo.PropagationParameters"
    "Parameters for the QuasiDenseStereo class"
    "Emgu.CV.Stereo"
    "XStereoInvoke"
    "QuasiDenseStereo"
	""
	"#include \"xstereo_c.h\""
	""
	""
	${HAVE_opencv_xstereo})

############################### xstereo code gen END ##############################

############################### structed_light code gen START ##############################
IF (NOT HAVE_opencv_structured_light)
  SET(HAVE_opencv_structured_light FALSE)
ENDIF()
  CREATE_OCV_CLASS_PROPERTY( 
    "structured_light/graycodepattern" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/StructuredLight/GrayCodePattern.g.cs"
    "cv::structured_light::GrayCodePattern" 
    "GrayCodePattern" 
    "NumberOfPatternImages;WhiteThreshold;BlackThreshold" 
    "int;int;int" 
    "propR;propW;propW"
    "NumberOfPatternImages;WhiteThreshold;BlackThreshold" 
    "int;int;int"
    "Get the number of pattern images needed for the graycode pattern;
    White threshold is a number between 0-255 that represents the minimum brightness difference required for valid pixels, between the graycode pattern and its inverse images, used in getProjPixel method;
    Black threshold is a number between 0-255 that represents the minimum brightness difference required for valid pixels, between the fully illuminated (white) and the not illuminated images (black), used in computeShadowMasks method
    "
    "Emgu.CV.StructuredLight"
    "StructuredLightInvoke"
    "GrayCodePattern"
	""
	"#include \"structured_light_c.h\""
	""
	""
	${HAVE_opencv_structured_light})

############################### structed_light code gen END ##############################

############################### ximgproc code gen START ##############################
IF (NOT HAVE_opencv_ximgproc)
  SET(HAVE_opencv_ximgproc FALSE)
ENDIF()
  CREATE_OCV_CLASS_PROPERTY( 
    "ximgproc/scan_segment" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/XImgproc/ScanSegment.g.cs"
    "cv::ximgproc::ScanSegment" 
    "ScanSegment" 
    "NumberOfSuperpixels" 
    "int" 
    "propR"
    "NumberOfSuperpixels" 
    "int"
    "Returns the actual superpixel segmentation from the last image processed using iterate. Returns zero if no image has been processed."
    "Emgu.CV.XImgproc"
    "XImgprocInvoke"
    "ScanSegment"
	""
	"#include \"ximgproc_c.h\""
	""
	""
	${HAVE_opencv_structured_light})

############################### ximgproc code gen END ##############################

############################### sureface matching code gen START ##############################
IF (NOT HAVE_opencv_surface_matching)
  SET(HAVE_opencv_surface_matching FALSE)
ENDIF()
  CREATE_OCV_CLASS_PROPERTY( 
    "surface_matching/pose3d" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/SurfaceMatching/Pose3D.g.cs"
    "cv::ppf_match_3d::Pose3D" 
    "Pose3D" 
    "alpha;residual;angle;modelIndex;numVotes" 
    "double;double;double;int;int" 
    "element;element;element;element;element"
    "Alpha;Residual;Angle;ModelIndex;NumVotes" 
    "double;double;double;int;int"
    "Alpha value;Residual value;Angle value;Model Index;Number of Votes"
    "Emgu.CV.PpfMatch3d"
    "PpfMatch3dInvoke"
    "Pose3D"
	""
	"#include \"surface_matching_c.h\""
	""
	""
	${HAVE_opencv_surface_matching})

    CREATE_VECTOR_CS(
        "Pose3D" 
        "std::vector< cv::ppf_match_3d::Pose3D >" 
        "Pose3D" "object_not_array" 
        "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/SurfaceMatching" 
        "Emgu.CV.PpfMatch3d"
        "" 
        "#include \"surface_matching_c.h\"" 
        "" 
        "defined(HAVE_OPENCV_SURFACE_MATCHING)")

############################### sureface matching code gen END ##############################


############################### rgbd code gen START ##############################
IF (NOT HAVE_opencv_rgbd)
  SET(HAVE_opencv_rgbd FALSE)
ENDIF()
  CREATE_OCV_CLASS_PROPERTY( 
    "rgbd/match" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/Rgbd/Match.g.cs"
    "cv::linemod::Match" 
    "Match" 
    "x;y;similarity;template_id;class_id" 
    "int;int;float;int;cv::String" 
    "element;element;element;element;element"
    "X;Y;Similarity;TemplateId;class_id" 
    "int;int;float;int;String"
    "X position;
    Y position;
    Similarity;
    TemplateId;
    Class Id"
    "Emgu.CV.Linemod"
    "LinemodInvoke"
    "Match"
	""
	"#include \"rgbd_c.h\""
	""
	""
	${HAVE_opencv_rgbd})

  CREATE_OCV_CLASS_PROPERTY( 
    "rgbd/detector" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/Rgbd/Detector.g.cs"
    "cv::linemod::Detector" 
    "Detector" 
    "pyramidLevels;numTemplates;numClasses" 
    "int;int;int" 
    "val;val;val"
    "PyramidLevels;NumTemplates;NumClasses" 
    "int;int;int"
    "Get number of pyramid levels used by this detector.;
    Get number of templates.;
    Get number of classes."
    "Emgu.CV.Linemod"
    "LinemodInvoke"
    "Detector"
	""
	"#include \"rgbd_c.h\""
	""
	""
	${HAVE_opencv_rgbd})

  CREATE_OCV_CLASS_PROPERTY( 
    "rgbd/modality" 
    "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.Contrib/Rgbd/Modality.g.cs"
    "cv::linemod::Modality" 
    "Modality" 
    "name" 
    "cv::String" 
    "val"
    "Name" 
    "String"
    "The name of modality"
    "Emgu.CV.Linemod"
    "LinemodInvoke"
    "Modality"
	""
	"#include \"rgbd_c.h\""
	""
	""
	${HAVE_opencv_rgbd})

############################### structed_light code gen END ##############################
