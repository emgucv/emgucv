//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "objdetect_c.h"

cv::QRCodeDetector* cveQRCodeDetectorCreate(cv::GraphicalCodeDetector** graphicalCodeDetector)
{
	try
	{
	#ifdef HAVE_OPENCV_OBJDETECT
		cv::QRCodeDetector* result =  new cv::QRCodeDetector();
		*graphicalCodeDetector = static_cast<cv::GraphicalCodeDetector*>(result);
		return result;
	#else 
		throw_no_objdetect();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveQRCodeDetectorRelease(cv::QRCodeDetector** detector)
{
	try
	{
	#ifdef HAVE_OPENCV_OBJDETECT
		delete *detector;
		*detector = 0;
	#else 
		throw_no_objdetect();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveQRCodeDetectorDecodeCurved(cv::QRCodeDetector* detector, cv::_InputArray* img, cv::_InputArray* points, cv::String* decodedInfo, cv::_OutputArray* straightQrcode)
{
	try
	{
	#ifdef HAVE_OPENCV_OBJDETECT
		std::string s = detector->decodeCurved(*img, *points, straightQrcode ? *straightQrcode : static_cast<cv::OutputArray>(cv::noArray()));
		*decodedInfo = s;
	#else 
		throw_no_objdetect();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

int cveQRCodeDetectorGetEncoding(cv::QRCodeDetector* detector, int codeIdx)
{
	try
	{
	#ifdef HAVE_OPENCV_OBJDETECT
		return detector->getEncoding(codeIdx);
	#else 
		throw_no_objdetect();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

cv::QRCodeDetectorAruco* cveQRCodeDetectorArucoCreate(cv::GraphicalCodeDetector** graphicalCodeDetector)
{
	try
	{
	#ifdef HAVE_OPENCV_OBJDETECT
		cv::QRCodeDetectorAruco* result = new cv::QRCodeDetectorAruco();
		*graphicalCodeDetector = static_cast<cv::GraphicalCodeDetector*>(result);
		return result;
	#else 
		throw_no_objdetect();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveQRCodeDetectorArucoRelease(cv::QRCodeDetectorAruco** detector)
{
	try
	{
	#ifdef HAVE_OPENCV_OBJDETECT
		delete* detector;
		*detector = 0;
	#else 
		throw_no_objdetect();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::QRCodeEncoder* cveQRCodeEncoderCreate(
    cv::Ptr<cv::QRCodeEncoder>** sharedPtr,
    int version,
    int correctionLevel,
    int mode,
    int structureNumber)
{
	try
	{
	#ifdef HAVE_OPENCV_OBJDETECT
		cv::QRCodeEncoder::Params p;
		p.version = version;
		p.correction_level = static_cast<cv::QRCodeEncoder::CorrectionLevel>(correctionLevel);
		p.mode = static_cast<cv::QRCodeEncoder::EncodeMode>(mode);
		p.structure_number = structureNumber;
		cv::Ptr<cv::QRCodeEncoder> ptr = cv::QRCodeEncoder::create();
		*sharedPtr = new cv::Ptr<cv::QRCodeEncoder>(ptr);
		return ptr.get();
	#else 
		throw_no_objdetect();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveQRCodeEncoderRelease(cv::QRCodeEncoder** encoder, cv::Ptr<cv::QRCodeEncoder>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_OBJDETECT
		delete* sharedPtr;
		*encoder = 0;
		*sharedPtr = 0;
	#else
		throw_no_objdetect();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveQRCodeEncoderEncode(cv::QRCodeEncoder* encoder, cv::String* encodedInfo, cv::_OutputArray* qrcode)
{
	try
	{
	#ifdef HAVE_OPENCV_OBJDETECT
		encoder->encode(*encodedInfo, *qrcode);
	#else
		throw_no_objdetect();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::barcode::BarcodeDetector* cveBarcodeDetectorCreate(
	cv::String* superResolutionModelPath,
	cv::GraphicalCodeDetector** graphicalCodeDetector)
{
	try
	{
	#ifdef HAVE_OPENCV_OBJDETECT
		cv::barcode::BarcodeDetector* result = superResolutionModelPath->empty()
			? new cv::barcode::BarcodeDetector()
			: new cv::barcode::BarcodeDetector(*superResolutionModelPath);
		*graphicalCodeDetector = static_cast<cv::GraphicalCodeDetector*>(result);
		return result;
	#else
		throw_no_objdetect();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveBarcodeDetectorRelease(cv::barcode::BarcodeDetector** detector)
{
	try
	{
	#ifdef HAVE_OPENCV_OBJDETECT
		delete* detector;
		detector = 0;
	#else
		throw_no_objdetect();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}



bool cveGraphicalCodeDetectorDetect(cv::GraphicalCodeDetector* detector, cv::_InputArray* img, cv::_OutputArray* points)
{
	try
	{
	#ifdef HAVE_OPENCV_OBJDETECT
		return detector->detect(*img, *points);
	#else 
		throw_no_objdetect();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}
bool cveGraphicalCodeDetectorDetectMulti(cv::GraphicalCodeDetector* detector, cv::_InputArray* img, cv::_OutputArray* points)
{
	try
	{
	#ifdef HAVE_OPENCV_OBJDETECT
		return detector->detectMulti(*img, *points);
	#else 
		throw_no_objdetect();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

void cveGraphicalCodeDetectorDecode(cv::GraphicalCodeDetector* detector, cv::_InputArray* img, cv::_InputArray* points, cv::_OutputArray* straightCode, cv::String* output)
{
	try
	{
	#ifdef HAVE_OPENCV_OBJDETECT
		*output = detector->decode(*img, *points, straightCode ? *straightCode : static_cast<cv::OutputArray>(cv::noArray()));
	#else 
		throw_no_objdetect();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

bool cveGraphicalCodeDetectorDecodeMulti(
	cv::GraphicalCodeDetector* detector,
	cv::_InputArray* img,
	cv::_InputArray* points,
	std::vector< std::string >* decodedInfo,
	cv::_OutputArray* straightCode)
{
	try
	{
	#ifdef HAVE_OPENCV_OBJDETECT
		return detector->decodeMulti(
			*img,
			*points,
			*decodedInfo,
			straightCode ? *straightCode : static_cast<cv::OutputArray>(cv::noArray())
		);
	#else 
		throw_no_objdetect();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

bool cveGraphicalCodeDetectorDetectAndDecodeMulti(
	cv::GraphicalCodeDetector* detector,
	cv::_InputArray* img,
	std::vector< std::string >* decodedInfo,
	cv::_OutputArray* points,
	cv::_OutputArray* straightCode)
{
	try
	{
	#ifdef HAVE_OPENCV_OBJDETECT
		return detector->detectAndDecodeMulti(
			*img,
			*decodedInfo,
			*points,	
			straightCode ? *straightCode : static_cast<cv::OutputArray>(cv::noArray())
		);
	#else 
		throw_no_objdetect();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

