#include "calib3d_c.h"

int cveEstimateAffine3D(
   cv::_InputArray* src, cv::_InputArray* dst,
   cv::_OutputArray* out, cv::_OutputArray* inliers,
   double ransacThreshold, double confidence)
{
   return cv::estimateAffine3D(*src, *dst, *out, *inliers, ransacThreshold, confidence);
}

//StereoSGBM
cv::StereoSGBM* CvStereoSGBMCreate(
  int minDisparity, int numDisparities, int blockSize,
  int P1, int P2, int disp12MaxDiff,
  int preFilterCap, int uniquenessRatio,
  int speckleWindowSize, int speckleRange,
  int mode, cv::StereoMatcher** stereoMatcher)
{
   cv::Ptr<cv::StereoSGBM> ptr =  cv::createStereoSGBM(minDisparity, numDisparities, blockSize, P1, P2, disp12MaxDiff, preFilterCap, uniquenessRatio, speckleWindowSize, speckleRange, mode);
   ptr.addref();
   cv::StereoSGBM* result = ptr.get();
   *stereoMatcher = (cv::StereoMatcher*) result;
   return result;
}
void CvStereoSGBMRelease(cv::StereoSGBM** obj) 
{ 
   delete *obj;
   *obj = 0;
}

//StereoBM
cv::StereoMatcher* CvStereoBMCreate(int numberOfDisparities, int blockSize)
{
   cv::Ptr<cv::StereoMatcher> ptr = cv::createStereoBM(numberOfDisparities, blockSize);
   ptr.addref();
   return ptr.get();
}

//StereoMatcher
void CvStereoMatcherCompute(cv::StereoMatcher*  disparitySolver, cv::_InputArray* left, cv::_InputArray* right, cv::_OutputArray* disparity)
{
   disparitySolver->compute(*left, *right, *disparity);
}
void CvStereoMatcherRelease(cv::StereoMatcher** matcher)
{
   delete *matcher;
   *matcher = 0;
}

//2D tracker
bool getHomographyMatrixFromMatchedFeatures(std::vector<cv::KeyPoint>* model, std::vector<cv::KeyPoint>* observed, CvArr* indices, CvArr* mask, double randsacThreshold, CvMat* homography)
{
   cv::Mat_<int> indMat = (cv::Mat_<int>) cv::cvarrToMat(indices);

   cv::Mat_<uchar> maskMat = mask ? (cv::Mat_<uchar>) cv::cvarrToMat(mask) : cv::Mat_<uchar>(indMat.rows, 1, 255);
   int nonZero = mask? cv::countNonZero(maskMat): indMat.rows;
   if (nonZero < 4) return false;

   std::vector<cv::Point2f> srcPtVec;
   std::vector<cv::Point2f> dstPtVec;

   for(int i = 0; i < maskMat.rows; i++)
   {
      if ( maskMat.at<uchar>(i) )
      {  
         int modelIdx = indMat(i, 0); 
         srcPtVec.push_back((*model)[modelIdx].pt);
         dstPtVec.push_back((*observed)[i].pt);
      }
   }
   
   //cv::Mat_<uchar> ransacMask(srcPtVec.size(), 1);
   std::vector<uchar> ransacMask;
   cv::Mat result = cv::findHomography(cv::Mat(srcPtVec), cv::Mat(dstPtVec), cv::RANSAC, randsacThreshold, ransacMask);
   if (result.empty())
   {
      return false;
   }
   cv::Mat hMat = cv::cvarrToMat(homography);
   result.copyTo(hMat);

   int idx = 0;
   for (int i = 0; i < maskMat.rows; i++)
   {
      uchar* val = maskMat.ptr<uchar>(i);
      if (*val)
         *val = ransacMask[idx++];
   }
   return true;

}

bool cveFindCirclesGrid(cv::_InputArray* image, CvSize* patternSize, cv::_OutputArray* centers, int flags, cv::FeatureDetector* blobDetector)
{
   cv::Ptr<cv::FeatureDetector> ptr(blobDetector);
   ptr.addref();
   return cv::findCirclesGrid(*image, *patternSize, *centers, flags, ptr);
}