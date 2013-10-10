#include "calib3d_c.h"

int CvEstimateAffine3D(CvMat* src, CvMat* dst,
                               cv::Mat* out, std::vector<unsigned char>* inliers,
                             double ransacThreshold, double confidence)
{
   cv::Mat mSrc = cv::cvarrToMat(src);
   cv::Mat mDst = cv::cvarrToMat(dst);
   return cv::estimateAffine3D(mSrc, mDst, *out, *inliers, ransacThreshold, confidence);
}

//StereoSGBM
cv::StereoSGBM* CvStereoSGBMCreate(
  int minDisparity, int numDisparities, int blockSize,
  int P1, int P2, int disp12MaxDiff,
  int preFilterCap, int uniquenessRatio,
  int speckleWindowSize, int speckleRange,
  int mode)
{
   cv::Ptr<cv::StereoSGBM> ptr =  cv::createStereoSGBM(minDisparity, numDisparities, blockSize, P1, P2, disp12MaxDiff, preFilterCap, uniquenessRatio, speckleWindowSize, speckleRange, mode);
   ptr.addref();
   return ptr.obj;
}
void CvStereoSGBMRelease(cv::StereoSGBM* obj) { delete obj;}
void CvStereoSGBMFindCorrespondence(cv::StereoSGBM* disparitySolver, IplImage* left, IplImage* right, IplImage* disparity)
{
   cv::Mat leftMat = cv::cvarrToMat(left);
   cv::Mat rightMat = cv::cvarrToMat(right);
   cv::Mat dispMat = cv::cvarrToMat(disparity);
   disparitySolver->compute(leftMat, rightMat, dispMat);
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

bool cvFindCirclesGrid(IplImage* image, CvSize* patternSize, std::vector<cv::Point2f>* centers, int flags, cv::FeatureDetector* blobDetector)
{
   cv::Mat mat = cv::cvarrToMat(image);
   cv::Size size(patternSize->width, patternSize->height);
   cv::Ptr<cv::FeatureDetector> ptr(blobDetector);
   ptr.addref();
   return cv::findCirclesGrid(mat, size, *centers, flags, ptr);
}