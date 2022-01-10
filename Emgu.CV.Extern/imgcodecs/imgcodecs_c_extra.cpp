//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "imgcodecs_c_extra.h"

bool cveHaveImageReader(cv::String* filename)
{
	return cv::haveImageReader(*filename);
}
bool cveHaveImageWriter(cv::String* filename)
{
	return cv::haveImageWriter(*filename);
}

bool cveImwrite(cv::String* filename, cv::_InputArray* img, std::vector<int>* params)
{
   return cv::imwrite(*filename, *img, params ? *params : std::vector<int>());
}

bool cveImwritemulti(cv::String* filename, cv::_InputArray* img, std::vector<int>* params)
{
	return cv::imwritemulti(*filename, *img, params ? *params : std::vector<int>());
}

void cveImread(cv::String* fileName, int flags, cv::Mat* result)
{
   cv::Mat m = cv::imread(*fileName, flags);
   cv::swap(*result, m);
}

bool cveImreadmulti(const cv::String* filename, std::vector<cv::Mat>* mats, int flags)
{
   return cv::imreadmulti(*filename, *mats, flags);
}

void cveImdecode(cv::_InputArray* buf, int flags, cv::Mat* dst)
{
   cv::imdecode(*buf, flags, dst);
}
bool cveImencode(cv::String* ext, cv::_InputArray* img, std::vector< unsigned char >* buf, std::vector< int >* params)
{
   return cv::imencode(*ext, *img, *buf, params ? *params : std::vector<int>());
}
