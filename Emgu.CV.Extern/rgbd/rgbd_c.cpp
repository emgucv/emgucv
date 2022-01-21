//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "rgbd_c.h"

cv::rgbd::Odometry* cveOdometryCreate(
	cv::String* odometryType,
	cv::Algorithm** algorithm,
	cv::Ptr<cv::rgbd::Odometry>** sharedPtr
)
{
#ifdef HAVE_OPENCV_RGBD
	cv::Ptr<cv::rgbd::Odometry> odometry = cv::rgbd::Odometry::create(*odometryType);
	*sharedPtr = new cv::Ptr<cv::rgbd::Odometry>(odometry);
	*algorithm = dynamic_cast<cv::Algorithm*>((*sharedPtr)->get());
	return (*sharedPtr)->get();
#else
	throw_no_rgbd();
#endif
}
void cveOdometryRelease(cv::Ptr<cv::rgbd::Odometry>** sharedPtr)
{
#ifdef HAVE_OPENCV_RGBD
	delete* sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_rgbd();
#endif
}

bool cveOdometryCompute(
	cv::rgbd::Odometry* odometry,
	cv::Mat* srcImage,
	cv::Mat* srcDepth,
	cv::Mat* srcMask,
	cv::Mat* dstImage,
	cv::Mat* dstDepth,
	cv::Mat* dstMask,
	cv::_OutputArray* rt,
	cv::Mat* initRt)
{
#ifdef HAVE_OPENCV_RGBD
	return odometry->compute(
		*srcImage,
		*srcDepth,
		*srcMask,
		*dstImage,
		*dstDepth,
		*dstMask,
		*rt,
		initRt ? *initRt : cv::Mat()
	);
#else
	throw_no_rgbd();
#endif
}

cv::rgbd::RgbdNormals* cveRgbdNormalsCreate(
	int rows,
	int cols,
	int depth,
	cv::_InputArray* K,
	int window_size,
	int method,
	cv::Algorithm** algorithm,
	cv::Ptr<cv::rgbd::RgbdNormals>** sharedPtr)
{
#ifdef HAVE_OPENCV_RGBD
	cv::Ptr<cv::rgbd::RgbdNormals> odometry = cv::rgbd::RgbdNormals::create(
		rows,
		cols,
		depth,
		*K,
		window_size,
		method);
	*sharedPtr = new cv::Ptr<cv::rgbd::RgbdNormals>(odometry);
	*algorithm = dynamic_cast<cv::Algorithm*>((*sharedPtr)->get());
	return (*sharedPtr)->get();
#else
	throw_no_rgbd();
#endif
}
void cveRgbdNormalsRelease(cv::Ptr<cv::rgbd::RgbdNormals>** sharedPtr)
{
#ifdef HAVE_OPENCV_RGBD
	delete* sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_rgbd();
#endif	
}
void cveRgbdNormalsApply(
	cv::rgbd::RgbdNormals* rgbdNormals,
	cv::_InputArray* points,
	cv::_OutputArray* normals)
{
#ifdef HAVE_OPENCV_RGBD
	(*rgbdNormals)(*points, *normals);
#else
	throw_no_rgbd();
#endif	
}

cv::linemod::Detector* cveLinemodLineDetectorCreate(cv::Ptr<cv::linemod::Detector>** sharedPtr)
{
#ifdef HAVE_OPENCV_RGBD
	cv::Ptr<cv::linemod::Detector> detector = cv::linemod::getDefaultLINE();
	*sharedPtr = new cv::Ptr<cv::linemod::Detector>(detector);
	return (*sharedPtr)->get();
#else
	throw_no_rgbd();
#endif	
}

cv::linemod::Detector* cveLinemodLinemodDetectorCreate(cv::Ptr<cv::linemod::Detector>** sharedPtr)
{
#ifdef HAVE_OPENCV_RGBD
	cv::Ptr<cv::linemod::Detector> detector = cv::linemod::getDefaultLINEMOD();
	*sharedPtr = new cv::Ptr<cv::linemod::Detector>(detector);
	return (*sharedPtr)->get();
#else
	throw_no_rgbd();
#endif	
}

void cveLinemodDetectorRead(cv::linemod::Detector* detector, cv::FileNode* fn)
{
#ifdef HAVE_OPENCV_RGBD
	detector->read(*fn);
#else
	throw_no_rgbd();
#endif	
}

void cveLinemodDetectorWrite(cv::linemod::Detector* detector, cv::FileStorage* fs)
{
#ifdef HAVE_OPENCV_RGBD
	detector->write(*fs);
#else
	throw_no_rgbd();
#endif	
}

int cveLinemodDetectorAddTemplate(
	cv::linemod::Detector* detector,
	std::vector< cv::Mat >* sources,
	cv::String* classId,
	cv::Mat* objectMask,
	CvRect* boundingBox)
{
#ifdef HAVE_OPENCV_RGBD
	cv::Rect r;
	int result = detector->addTemplate(*sources, *classId, *objectMask, &r);
	if (boundingBox)
	{
		boundingBox->x = r.x;
		boundingBox->y = r.y;
		boundingBox->width = r.width;
		boundingBox->height = r.height;
	}
	return result;
#else
	throw_no_rgbd();
#endif	

}

void cveLinemodDetectorGetClassIds(cv::linemod::Detector* detector, std::vector< cv::String >* classIds)
{
#ifdef HAVE_OPENCV_RGBD
	std::vector< cv::String > ids = detector->classIds();
	for (std::vector< cv::String >::iterator it = ids.begin(); it != ids.end(); ++it)
	{
		classIds->push_back(*it);
	}
#else
	throw_no_rgbd();
#endif	
}

void cveLinemodDetectorMatch(
	cv::linemod::Detector* detector,
	std::vector< cv::Mat >* sources,
	float threshold,
	std::vector< cv::linemod::Match >* matches,
	std::vector< cv::String >* classIds,
	cv::_OutputArray* quantizedImages,
	std::vector< cv::Mat >* masks)
{
#ifdef HAVE_OPENCV_RGBD
	detector->match(
		*sources,
		threshold,
		*matches,
		classIds ? *classIds : std::vector< cv::String >(),
		quantizedImages ? *quantizedImages : static_cast<cv::OutputArray>(cv::noArray()),
		masks ? *masks : std::vector< cv::Mat >());
#else
	throw_no_rgbd();
#endif	
}

int cveLinemodDetectorGetT(cv::linemod::Detector* detector, int pyramidLevel)
{
#ifdef HAVE_OPENCV_RGBD
	return detector->getT(pyramidLevel);
#else
	throw_no_rgbd();
#endif	
}

void cveLinemodDetectorGetModalities(cv::linemod::Detector* detector, std::vector< void* >* vectorOfPtrs)
{
#ifdef HAVE_OPENCV_RGBD
	vectorOfPtrs->clear();
	const std::vector< cv::Ptr< cv::linemod::Modality > > modalities = detector->getModalities();
	for (auto it = modalities.begin(); it != modalities.end(); ++it)
	{
		cv::linemod::Modality* p = it->get();
		vectorOfPtrs->push_back(p);
	}
#else
	throw_no_rgbd();
#endif	
}

void cveLinemodDetectorRelease(cv::Ptr<cv::linemod::Detector>** sharedPtr)
{
#ifdef HAVE_OPENCV_RGBD
	delete* sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_rgbd();
#endif	

}

cv::linemod::Match* cveLinemodMatchCreate()
{
#ifdef HAVE_OPENCV_RGBD
	return new cv::linemod::Match();
#else
	throw_no_rgbd();
#endif	

}
void cveLinemodMatchRelease(cv::linemod::Match** match)
{
#ifdef HAVE_OPENCV_RGBD
	delete* match;
	*match = 0;
#else
	throw_no_rgbd();
#endif	

}

cv::linemod::Modality* cveLinemodModalityCreate(cv::String* modalityType, cv::Ptr<cv::linemod::Modality>** sharedPtr)
{
#ifdef HAVE_OPENCV_RGBD
	cv::Ptr<cv::linemod::Modality> modality = cv::linemod::Modality::create(*modalityType);
	*sharedPtr = new cv::Ptr<cv::linemod::Modality>(modality);
	return (*sharedPtr)->get();
#else
	throw_no_rgbd();
#endif	

}
void cveLinemodModalityRelease(cv::Ptr<cv::linemod::Modality>** sharedPtr)
{
#ifdef HAVE_OPENCV_RGBD
	delete* sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_rgbd();
#endif	

}