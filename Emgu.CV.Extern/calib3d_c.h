//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_CALIB3D_C_H
#define EMGU_CALIB3D_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/calib3d/calib3d.hpp"

CVAPI(int)  CvEstimateAffine3D(CvMat* src, CvMat* dst,
                               cv::Mat* out, std::vector<unsigned char>* inliers,
                             double ransacThreshold, double confidence);


#endif