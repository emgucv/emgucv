//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "dpm_c.h"

using cv::dpm::DPMDetector;

DPMDetector* cveDPMDetectorCreate(std::vector<cv::String>* filenames, std::vector<cv::String>* classNames)
{
	std::vector< std::basic_string<char> > files = std::vector<std::string>(filenames->size());
	std::vector< std::basic_string<char> > classes = std::vector<std::string>(classNames->size());

	for (std::vector<cv::String>::iterator it = filenames->begin(); it != filenames->end(); ++it)
		files.push_back(std::string(it->c_str(), it->size()));

	for (std::vector<cv::String>::iterator it = classNames->begin(); it != classNames->end(); ++it)
		classes.push_back(std::string(it->c_str(), it->size()));

	cv::Ptr<DPMDetector> dpm = DPMDetector::create(files, classes);
	dpm.addref();
	return dpm.get();
}

void cveDPMDetectorDetect(DPMDetector* dpm, cv::Mat* image, std::vector<CvRect>* rects, std::vector<float>* scores, std::vector<int>* classIds)
{
	std::vector<DPMDetector::ObjectDetection> dobjects = std::vector<DPMDetector::ObjectDetection>();
	dpm->detect(*image, dobjects);

	for (std::vector<DPMDetector::ObjectDetection>::iterator it = dobjects.begin(); it != dobjects.end(); ++it)
	{
		rects->push_back(it->rect);
		scores->push_back(it->score);
		classIds->push_back(it->classID);
	}
}

size_t cveDPMDetectorGetClassCount(DPMDetector* dpm)
{
	return dpm->getClassCount();
}

void cveDPMDetectorGetClassNames(DPMDetector* dpm, std::vector<cv::String>* names)
{
	std::vector<std::string> classnames = dpm->getClassNames();

	for (std::vector<std::string>::iterator it = classnames.begin(); it != classnames.end(); ++it)
		names->push_back(cv::String(*it));
}

bool cveDPMDetectorIsEmpty(DPMDetector* dpm)
{
	return dpm->isEmpty();
}

void cveDPMDetectorRelease(DPMDetector** dpm)
{
	delete *dpm;
	*dpm = 0;
}