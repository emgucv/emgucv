//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "optim_c.h"

int cvSolveLP(const CvMat* Func, const CvMat* Constr, CvMat* z)
{
   cv::Mat funcMat = cv::cvarrToMat(Func);
   cv::Mat constrMat = cv::cvarrToMat(Constr);
   cv::Mat zMat = cv::cvarrToMat(z);
   return cv::optim::solveLP(funcMat, constrMat, zMat);
}
