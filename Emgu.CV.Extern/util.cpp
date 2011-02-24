#pragma warning( disable: 4251 )

#include "opencv2/core/core_c.h"
#include "opencv2/contrib/contrib.hpp"
#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/calib3d/calib3d.hpp"

CVAPI(IplImage*) cvGetImageSubRect(IplImage* image, CvRect* rect) 
{ 
	IplImage* res = cvCreateImageHeader(cvSize(rect->width, rect->height), image->depth, image->nChannels);
	CvMat mat;
	cvGetSubRect(image, &mat, *rect);
	cvGetImage(&mat, res);
	return res;
}

//CvAdaptiveSkinDetector
CVAPI(CvAdaptiveSkinDetector*) CvAdaptiveSkinDetectorCreate(int samplingDivider, int morphingMethod) { return new CvAdaptiveSkinDetector(samplingDivider, morphingMethod); }
CVAPI(void) CvAdaptiveSkinDetectorRelease(CvAdaptiveSkinDetector* detector) { delete detector; }
CVAPI(void) CvAdaptiveSkinDetectorProcess(CvAdaptiveSkinDetector* detector, IplImage *inputBGRImage, IplImage *outputHueMask) { detector->process(inputBGRImage, outputHueMask); }

//GrabCut
CVAPI(void) CvGrabCut(IplImage* img, IplImage* mask, cv::Rect* rect, IplImage* bgdModel, IplImage* fgdModel, int iterCount, int flag)
{
cv::Mat imgMat = cv::cvarrToMat(img);
cv::Mat maskMat = cv::cvarrToMat(mask);
cv::Mat bgdModelMat = cv::cvarrToMat(bgdModel);
cv::Mat fgdModelMat = cv::cvarrToMat(fgdModel);
cv::grabCut(imgMat, maskMat, *rect, bgdModelMat, fgdModelMat, iterCount, flag);
}

//StereoSGBM
CVAPI(cv::StereoSGBM*) CvStereoSGBMCreate(
  int minDisparity, int numDisparities, int SADWindowSize,
  int P1, int P2, int disp12MaxDiff,
  int preFilterCap, int uniquenessRatio,
  int speckleWindowSize, int speckleRange,
  bool fullDP)
{
   return new cv::StereoSGBM(minDisparity, numDisparities, SADWindowSize, P1, P2, disp12MaxDiff, preFilterCap,uniquenessRatio,speckleWindowSize,speckleRange, fullDP);
}
CVAPI(void) CvStereoSGBMRelease(cv::StereoSGBM* obj) { delete obj;}
CVAPI(void) CvStereoSGBMFindCorrespondence(cv::StereoSGBM* disparitySolver, IplImage* left, IplImage* right, IplImage* disparity)
{
   cv::Mat leftMat = cv::cvarrToMat(left);
   cv::Mat rightMat = cv::cvarrToMat(right);
   cv::Mat dispMat = cv::cvarrToMat(disparity);
   (*disparitySolver)(leftMat, rightMat, dispMat);
}

CVAPI(bool) cvCheckRange(CvArr* arr, bool quiet, CvPoint* index, double minVal, double maxVal)
{
   cv::Mat mat = cv::cvarrToMat(arr);
   cv::Point p;
   bool result = cv::checkRange(mat, quiet, &p , minVal, maxVal);
   index->x = p.x;
   index->y = p.y;
   return result;
}

CVAPI(void) cvArrSqrt(CvArr* src, CvArr* dst)
{
   cv::Mat srcMat = cv::cvarrToMat(src);
   cv::Mat dstMat = cv::cvarrToMat(dst);
   cv::sqrt(srcMat, dstMat);
}

CVAPI(bool) getHomographyMatrixFromMatchedFeatures(std::vector<cv::KeyPoint>* model, std::vector<cv::KeyPoint>* observed, CvArr* indices, CvArr* mask, CvMat* homography)
{
   cv::Mat_<int> indMat = (cv::Mat_<int>) cv::cvarrToMat(indices);

   cv::Mat_<uchar> maskMat = mask ? (cv::Mat_<uchar>) cv::cvarrToMat(mask) : cv::Mat_<uchar>(indMat.rows, 1, 255);
   int nonZero = mask? cv::countNonZero(maskMat): indMat.rows;
   if (nonZero < 4) return false;

   cv::Mat_<float> srcPtMat(nonZero, 2);
   cv::Mat_<float> dstPtMat(nonZero, 2);

   int idx = 0;
   for(int i = 0; i < maskMat.rows; i++)
   {
      if ( *maskMat.ptr(i) != 0)
      {
         memcpy(srcPtMat.ptr(idx), &(*model)[*indMat.ptr(i)].pt, sizeof(float) * 2);
         memcpy(dstPtMat.ptr(idx), &(*observed)[i].pt, sizeof(float) * 2);
         idx++;
      }
   }
   cv::Mat result = cv::findHomography(srcPtMat, dstPtMat, CV_RANSAC, 3);
   cv::Mat hMat = cv::cvarrToMat(homography);
   result.copyTo(hMat);
   return true;

}

CVAPI(int) voteForSizeAndOrientation(std::vector<cv::KeyPoint>* modelKeyPoints, std::vector<cv::KeyPoint>* observedKeyPoints, CvArr* indices, CvArr* mask, double scaleIncrement, int rotationBins)
{
   cv::Mat_<int> indiciesMat = (cv::Mat_<int>) cv::cvarrToMat(indices);
   cv::Mat_<uchar> maskMat = (cv::Mat_<uchar>) cv::cvarrToMat(mask);
   std::vector<float> scale;
   std::vector<float> rotations;
   float s, maxS, minS, r;
   maxS = -1.0e-10f; minS = 1.0e10f;

   for (int i = 0; i < maskMat.rows; i++)
   {
      if ( maskMat(i, 0)) 
      {
         cv::KeyPoint observedKeyPoint = observedKeyPoints->at(i);
         cv::KeyPoint modelKeyPoint = modelKeyPoints->at( indiciesMat(i, 0));
         s = log10( observedKeyPoint.size / modelKeyPoint.size );
         scale.push_back(s);
         maxS = s > maxS ? s : maxS;
         minS = s < minS ? s : minS;

         r = observedKeyPoint.angle - modelKeyPoint.angle;
         r = r < 0.0f? r + 360.0f : r;
         rotations.push_back(r);
      }    
   }

   int scaleBinSize = (int)((maxS - minS) / log10(scaleIncrement));
   scaleBinSize = scaleBinSize < 1? 1 : scaleBinSize;

   cv::Mat_<float> scalesMat(scale);
   cv::Mat_<float> rotationsMat(rotations);
   std::vector<float> flags(scale.size());
   cv::Mat flagsMat(flags);
   if (scaleBinSize == 1)
   {
      int histSize[] = {rotationBins};
      float rotationRanges[] = {0, 360};
      int channels[] = {0};
      const float* ranges[] = {rotationRanges};
      double minVal, maxVal;
      const cv::Mat_<float> arrs[] = {rotationsMat}; 

      cv::MatND hist; //CV_32S
      cv::calcHist(arrs, 1, channels, cv::Mat(), hist, 1, histSize, ranges);
      cv::minMaxLoc(hist, &minVal, &maxVal);
      cv::threshold(hist, hist, maxVal * 0.5, 0, cv::THRESH_TOZERO);
      cv::calcBackProject(arrs, 1, channels, hist, flagsMat, ranges);
   } else
   {
      int histSize[] = {scaleBinSize, rotationBins};
      float scaleRanges[] = {minS, maxS};
      float rotationRanges[] = {0, 360};
      int channels[] = {0, 1};
      const float* ranges[] = {scaleRanges, rotationRanges};
      double minVal, maxVal;

      const cv::Mat_<float> arrs[] = {scalesMat, rotationsMat}; 

      cv::MatND hist; //CV_32S
      cv::calcHist(arrs, 2, channels, cv::Mat(), hist, 2, histSize, ranges, true);
      cv::minMaxLoc(hist, &minVal, &maxVal);

      cv::threshold(hist, hist, maxVal * 0.5, 0, cv::THRESH_TOZERO);
      cv::calcBackProject(arrs, 2, channels, hist, flagsMat, ranges);
   }

   int idx =0;
   int nonZeroCount = 0;
   for (int i = 0; i < maskMat.rows; i++)
   {
      if (maskMat(i, 0))
      {
         if (flags[idx++] != 0.0f)
            nonZeroCount++;
         else 
            maskMat(i, 0) = 0;
      }
   }
   return nonZeroCount;
}