//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "dpm_c.h"

DPMDetector* cveDPMDetectorCreate(std::vector<cv::String>* filenames, std::vector<cv::String>* classNames, cv::Ptr<cv::dpm::DPMDetector>** sharedPtr)
{
#ifdef HAVE_OPENCV_DPM
	std::vector< std::string > files;
	for (std::vector<cv::String>::iterator it = filenames->begin(); it != filenames->end(); ++it)
		files.push_back(std::string(it->c_str(), it->size()));

	std::vector< std::string > classes;
	if (classNames)
	{
		for (std::vector<cv::String>::iterator it = classNames->begin(); it != classNames->end(); ++it)
			classes.push_back(std::string(it->c_str(), it->size()));
	}

	cv::Ptr<DPMDetector> dpm = DPMDetector::create(files, classes);
	*sharedPtr = new cv::Ptr<DPMDetector>(dpm);
	return (*sharedPtr)->get();
#else
	throw_no_dpm();
#endif
}

void cveDPMDetectorDetect(DPMDetector* dpm, cv::Mat* image, std::vector<CvRect>* rects, std::vector<float>* scores, std::vector<int>* classIds)
{
#ifdef HAVE_OPENCV_DPM
	std::vector<DPMDetector::ObjectDetection> dobjects = std::vector<DPMDetector::ObjectDetection>();
	dpm->detect(*image, dobjects);

	for (std::vector<DPMDetector::ObjectDetection>::iterator it = dobjects.begin(); it != dobjects.end(); ++it)
	{
		rects->push_back(cvRect(it->rect));
		scores->push_back(it->score);
		classIds->push_back(it->classID);
	}
#else
	throw_no_dpm();
#endif
}

size_t cveDPMDetectorGetClassCount(DPMDetector* dpm)
{
#ifdef HAVE_OPENCV_DPM
	return dpm->getClassCount();
#else
	throw_no_dpm();
#endif
}

void cveDPMDetectorGetClassNames(DPMDetector* dpm, std::vector<cv::String>* names)
{
#ifdef HAVE_OPENCV_DPM
	std::vector<std::string> classnames = dpm->getClassNames();

	for (std::vector<std::string>::iterator it = classnames.begin(); it != classnames.end(); ++it)
		names->push_back(cv::String(*it));
#else
	throw_no_dpm();
#endif
}

bool cveDPMDetectorIsEmpty(DPMDetector* dpm)
{
#ifdef HAVE_OPENCV_DPM
	return dpm->isEmpty();
#else
	throw_no_dpm();
#endif
}

void cveDPMDetectorRelease(cv::Ptr<cv::dpm::DPMDetector>** sharedPtr)
{
#ifdef HAVE_OPENCV_DPM
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_dpm();
#endif
}