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

void cveTriangulatePoints(cv::_InputArray* projMat1, cv::_InputArray* projMat2, cv::_InputArray* projPoints1, cv::_InputArray* projPoints2, cv::_OutputArray* points4D)
{
   cv::triangulatePoints(*projMat1, *projMat2, *projPoints1, *projPoints2, *points4D);
}

void cveCorrectMatches(cv::_InputArray* f, cv::_InputArray* points1, cv::_InputArray* points2, cv::_OutputArray* newPoints1, cv::_OutputArray* newPoints2)
{
   cv::correctMatches(*f, *points1, *points2, *newPoints1, *newPoints2);
}

void cveDrawChessboardCorners(cv::_InputOutputArray* image, CvSize* patternSize, cv::_InputArray* corners, bool patternWasFound)
{
   cv::drawChessboardCorners(*image, *patternSize, *corners, patternWasFound);
}

bool cveFindChessboardCorners(cv::_InputArray* image, CvSize* patternSize, cv::_OutputArray* corners, int flags)
{
   return cv::findChessboardCorners(*image, *patternSize, *corners, flags);
}

bool cveStereoRectifyUncalibrated(cv::_InputArray* points1, cv::_InputArray* points2, cv::_InputArray* f, CvSize* imgSize, cv::_OutputArray* h1, cv::_OutputArray* h2, double threshold)
{
   return cv::stereoRectifyUncalibrated(*points1, *points2, *f, *imgSize, *h1, *h2, threshold);
}

void cveStereoRectify(
   cv::_InputArray* cameraMatrix1, cv::_InputArray* distCoeffs1,
   cv::_InputArray* cameraMatrix2, cv::_InputArray* distCoeffs2,
   CvSize* imageSize, cv::_InputArray* r, cv::_InputArray* t,
   cv::_OutputArray* r1, cv::_OutputArray* r2,
   cv::_OutputArray* p1, cv::_OutputArray* p2,
   cv::_OutputArray* q, int flags,
   double alpha, CvSize* newImageSize,
   CvRect* validPixROI1, CvRect* validPixROI2)
{
   cv::Rect rect1, rect2;
   cv::stereoRectify(*cameraMatrix1, *distCoeffs1, *cameraMatrix2, *distCoeffs2, *imageSize, *r, *t, *r1, *r2,
      *p1, *p2, *q, flags, alpha, *newImageSize, &rect1, &rect2);
   *validPixROI1 = rect1;
   *validPixROI2 = rect2;
}

void cveRodrigues(cv::_InputArray* src, cv::_OutputArray* dst, cv::_OutputArray* jacobian)
{
   cv::Rodrigues(*src, *dst, jacobian? *jacobian : (cv::OutputArray) cv::noArray());
}

double cveCalibrateCamera(
   cv::_InputArray* objectPoints, cv::_InputArray* imagePoints, CvSize* imageSize, 
   cv::_InputOutputArray* cameraMatrix, cv::_InputOutputArray* distCoeffs, 
   cv::_OutputArray* rvecs, cv::_OutputArray* tvecs, int flags, CvTermCriteria* criteria)
{
   return cv::calibrateCamera(*objectPoints, *imagePoints, *imageSize, *cameraMatrix, *distCoeffs, *rvecs, *tvecs, flags, *criteria); 
}

void cveReprojectImageTo3D(cv::_InputArray* disparity, cv::_OutputArray* threeDImage, cv::_InputArray* q, bool handleMissingValues, int ddepth)
{
   cv::reprojectImageTo3D(*disparity, *threeDImage, *q, handleMissingValues, ddepth);
}

void cveConvertPointsToHomogeneous(cv::_InputArray* src, cv::_OutputArray* dst)
{
   cv::convertPointsToHomogeneous(*src, *dst);
}

void cveConvertPointsFromHomogeneous(cv::_InputArray* src, cv::_OutputArray* dst)
{
   cv::convertPointsFromHomogeneous(*src, *dst);
}

void cveFindFundamentalMat(cv::_InputArray* points1, cv::_InputArray* points2, cv::_OutputArray* dst, int method, double param1, double param2, cv::_OutputArray* mask)
{
   cv::Mat tmp = cv::findFundamentalMat(*points1, *points2, method, param1, param2, mask ? *mask : (cv::OutputArray) cv::noArray());
   tmp.copyTo(*dst);
}

void cveFindHomography(cv::_InputArray* srcPoints, cv::_InputArray* dstPoints, cv::_OutputArray* dst, int method, double ransacReprojThreshold, cv::_OutputArray* mask)
{
   cv::Mat tmp = cv::findHomography(*srcPoints, *dstPoints,method, ransacReprojThreshold, mask ? *mask : (cv::OutputArray) cv::noArray());
   tmp.copyTo(*dst);
}

void cveComputeCorrespondEpilines(cv::_InputArray* points, int whichImage, cv::_InputArray* f, cv::_OutputArray* lines)
{
   cv::computeCorrespondEpilines(*points, whichImage, *f, *lines);
}

void cveProjectPoints(
   cv::_InputArray* objPoints, cv::_InputArray* rvec, cv::_InputArray* tvec, cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs,
   cv::_OutputArray* imagePoints, cv::_OutputArray* jacobian, double aspectRatio)
{
   cv::projectPoints(*objPoints, *rvec, *tvec, *cameraMatrix, distCoeffs ? *distCoeffs : (cv::InputArray) cv::noArray() , *imagePoints, jacobian ? *jacobian : (cv::OutputArray) cv::noArray(), aspectRatio);
}

void cveCalibrationMatrixValues(
   cv::_InputArray* cameraMatrix, CvSize* imageSize, double apertureWidth, double apertureHeight, 
   double* fovx, double* fovy, double* focalLength, CvPoint2D64f* principalPoint, double* aspectRatio)
{
   double _fovx, _fovy, _focalLength, _aspectRatio;
   cv::Point2d _principalPoint;

   cv::calibrationMatrixValues(*cameraMatrix, *imageSize, apertureWidth, apertureHeight, _fovx, _fovy, _focalLength, _principalPoint, _aspectRatio);
   *fovx = _fovx; *fovy = _fovy; *focalLength = _focalLength; *aspectRatio = _aspectRatio; principalPoint->x = _principalPoint.x; principalPoint->y = _principalPoint.y;
}

double cveStereoCalibrate(
   cv::_InputArray* objectPoints, cv::_InputArray* imagePoints1, cv::_InputArray* imagePoints2,
   cv::_InputOutputArray* cameraMatrix1, cv::_InputOutputArray* distCoeffs1, cv::_InputOutputArray* cameraMatrix2, cv::_InputOutputArray* distCoeffs2,
   CvSize* imageSize, cv::_OutputArray* r, cv::_OutputArray* t, cv::_OutputArray* e, cv::_OutputArray* f, int flags, CvTermCriteria* criteria)
{
   return cv::stereoCalibrate(*objectPoints, *imagePoints1, *imagePoints2, *cameraMatrix1, *distCoeffs1, *cameraMatrix2, *distCoeffs2, *imageSize, *r, *t, *e, *f,
      flags, *criteria);
}

bool cveSolvePnP(cv::_InputArray* objectPoints, cv::_InputArray* imagePoints, cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs, cv::_OutputArray* rvec, cv::_OutputArray* tvec, bool useExtrinsicGuess, int flags)
{
   return cv::solvePnP(*objectPoints, *imagePoints, *cameraMatrix, *distCoeffs, *rvec, *tvec, useExtrinsicGuess, flags);
}

void cveSolvePnPRansac(cv::_InputArray* objectPoints, cv::_InputArray* imagePoints, cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs, cv::_OutputArray* rvec, cv::_OutputArray* tvec, bool useExtrinsicGuess, int iterationsCount, float reprojectionError, int minInliersCount, cv::_OutputArray* inliers, int flags )
{
   cv::solvePnPRansac(*objectPoints, *imagePoints, *cameraMatrix, distCoeffs ? *distCoeffs : (cv::InputArray) cv::noArray(), *rvec, *tvec, useExtrinsicGuess, iterationsCount, reprojectionError, minInliersCount, inliers ? *inliers : (cv::OutputArray) cv::noArray(), flags);
}