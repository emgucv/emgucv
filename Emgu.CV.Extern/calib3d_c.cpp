#include "calib3d_c.h"

int CvEstimateAffine3D(CvMat* src, CvMat* dst,
                               cv::Mat* out, std::vector<unsigned char>* inliers,
                             double ransacThreshold, double confidence)
{
   cv::Mat mSrc = cv::cvarrToMat(src);
   cv::Mat mDst = cv::cvarrToMat(dst);
   return cv::estimateAffine3D(mSrc, mDst, *out, *inliers, ransacThreshold, confidence);
}