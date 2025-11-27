//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "mcc_c.h"

cv::mcc::CChecker* cveCCheckerCreate(cv::Ptr<cv::mcc::CChecker>** sharedPtr)
{
#ifdef HAVE_OPENCV_OBJDETECT
	cv::Ptr<cv::mcc::CChecker> checker = cv::mcc::CChecker::create();
	*sharedPtr = new cv::Ptr<cv::mcc::CChecker>(checker);
	return (*sharedPtr)->get();
#else
	throw_no_objdetect();
#endif
}

void cveCCheckerGetBox(cv::mcc::CChecker* checker, std::vector< cv::Point2f >* box)
{
#ifdef HAVE_OPENCV_OBJDETECT
	std::vector<cv::Point2f> pts = checker->getBox();
	*box = pts;
#else
	throw_no_objdetect();
#endif
}
void cveCCheckerSetBox(cv::mcc::CChecker* checker, std::vector< cv::Point2f >* box)
{
#ifdef HAVE_OPENCV_OBJDETECT
	checker->setBox(*box);
#else
	throw_no_objdetect();
#endif
}

void cveCCheckerGetCenter(cv::mcc::CChecker* checker, cv::Point2f* center)
{
#ifdef HAVE_OPENCV_OBJDETECT
	cv::Point2f p = checker->getCenter();
	center->x = p.x;
	center->y = p.y;
#else
	throw_no_objdetect();
#endif
}
void cveCCheckerSetCenter(cv::mcc::CChecker* checker, cv::Point2f* center)
{
#ifdef HAVE_OPENCV_OBJDETECT
	cv::Point2f p = *center;
	checker->setCenter(p);
#else
	throw_no_objdetect();
#endif
}

void cveCCheckerRelease(cv::Ptr<cv::mcc::CChecker>** sharedPtr)
{
#ifdef HAVE_OPENCV_OBJDETECT
	delete* sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_objdetect();
#endif
}

void cveCCheckerGetChartsRGB(cv::mcc::CChecker* checker, cv::_OutputArray* chartsRgb)
{
#ifdef HAVE_OPENCV_OBJDETECT
	cv::Mat m = checker->getChartsRGB();
	m.copyTo(*chartsRgb);
#else
	throw_no_objdetect();
#endif
}

void cveCCheckerSetChartsRGB(cv::mcc::CChecker* checker, cv::Mat* chartsRgb)
{
#ifdef HAVE_OPENCV_OBJDETECT
	checker->setChartsRGB(*chartsRgb);
#else
	throw_no_objdetect();
#endif
}

/*
cv::mcc::CCheckerDraw* cveCCheckerDrawCreate(
	cv::mcc::CChecker* pChecker,
	cv::Scalar* color,
	int thickness,
	cv::Ptr<cv::mcc::CCheckerDraw>** sharedPtr)
{
#ifdef HAVE_OPENCV_OBJDETECT
	cv::Ptr<cv::mcc::CChecker> cCheckerPtr(pChecker, [](cv::mcc::CChecker* p) {});
	cv::Ptr<cv::mcc::CCheckerDraw> checkerDraw = cv::mcc::CCheckerDraw::create(
		cCheckerPtr,
		*color,
		thickness);
	*sharedPtr = new cv::Ptr<cv::mcc::CCheckerDraw>(checkerDraw);
	return (*sharedPtr)->get();
#else
	throw_no_objdetect();
#endif
}

void cveCCheckerDrawDraw(cv::mcc::CCheckerDraw* ccheckerDraw, cv::_InputOutputArray* img)
{
#ifdef HAVE_OPENCV_OBJDETECT
	ccheckerDraw->draw(*img);
#else
	throw_no_objdetect();
#endif
}
void cveCCheckerDrawRelease(cv::Ptr<cv::mcc::CCheckerDraw>** sharedPtr)
{
#ifdef HAVE_OPENCV_OBJDETECT
	delete* sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_objdetect();
#endif  
}

*/

cv::mcc::CCheckerDetector* cveCCheckerDetectorCreate(cv::Algorithm** algorithm, cv::Ptr<cv::mcc::CCheckerDetector>** sharedPtr)
{
#ifdef HAVE_OPENCV_OBJDETECT
	cv::Ptr<cv::mcc::CCheckerDetector> checkerDetector = cv::mcc::CCheckerDetector::create();
	*sharedPtr = new cv::Ptr<cv::mcc::CCheckerDetector>(checkerDetector);
	*algorithm = dynamic_cast<cv::Algorithm*>((*sharedPtr)->get());
	return (*sharedPtr)->get();
#else
	throw_no_objdetect();
#endif
}

bool cveCCheckerDetectorProcess(
	cv::mcc::CCheckerDetector* detector,
	cv::_InputArray* image,
	std::vector< cv::Rect >* regionOfInterest,
	int nc)
{
#ifdef HAVE_OPENCV_OBJDETECT
	if (regionOfInterest) {
		return detector->process(*image, *regionOfInterest, nc);
	}
	else
	{
		return detector->process(*image, nc);
	}
#else
	throw_no_objdetect();
#endif
}


void cveCCheckerDetectorDraw(
	cv::mcc::CCheckerDetector* detector,
	cv::mcc::CChecker* pChecker,
	cv::_InputOutputArray* img,
	cv::Scalar* color,
	int thickness)
{
#ifdef HAVE_OPENCV_OBJDETECT
	std::vector< cv::Ptr<cv::mcc::CChecker> > checkers;
	cv::Ptr< cv::mcc::CChecker > checkerPtr(pChecker, [](cv::mcc::CChecker* p) {});
	checkers.push_back(checkerPtr);
	detector->draw(checkers, *img, *color, thickness);
#else
	throw_no_objdetect();
#endif
}


cv::mcc::CChecker* cveCCheckerDetectorGetBestColorChecker(cv::mcc::CCheckerDetector* detector)
{
#ifdef HAVE_OPENCV_OBJDETECT
	cv::Ptr<cv::mcc::CChecker> ptr = detector->getBestColorChecker();
	return ptr.get();
#else
	throw_no_objdetect();
#endif
}
void cveCCheckerDetectorRelease(cv::Ptr<cv::mcc::CCheckerDetector>** sharedPtr)
{
#ifdef HAVE_OPENCV_OBJDETECT
	delete* sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_objdetect();
#endif
}

cv::mcc::DetectorParametersMCC* cveDetectorParametersMCCCreate()
{
#ifdef HAVE_OPENCV_OBJDETECT
	return new cv::mcc::DetectorParametersMCC();
#else
	throw_no_objdetect();
#endif
}
void cveDetectorParametersMCCRelease(cv::mcc::DetectorParametersMCC** parameters)
{
#ifdef HAVE_OPENCV_OBJDETECT
	delete* parameters;
	*parameters = 0;
#else
	throw_no_objdetect();
#endif	
}

