//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "imgcodecs_c_extra.h"

bool cveImwrite(cv::String* filename, cv::_InputArray* img, const std::vector<int>* params)
{
   return cv::imwrite(*filename, *img, params ? *params : std::vector<int>());
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
void cveImencode(cv::String* ext, cv::_InputArray* img, std::vector< unsigned char >* buf, std::vector< int >* params)
{
   cv::imencode(*ext, *img, *buf, params ? *params : std::vector<int>());
}
