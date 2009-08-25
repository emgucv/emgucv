#include "cvaux.h"
//FernClassifier
CVAPI(cv::FernClassifier*) CvFernClassifierCreate() { return new cv::FernClassifier; }
CVAPI(void) CvFernClassifierRelease(cv::FernClassifier* classifier) { delete classifier;}

CVAPI(void) CvFernClassifierTrainFromSingleView(
                                  cv::FernClassifier* classifier,
                                  IplImage* image,
                                  cv::KeyPoint* keypoints,
                                  int numberOfKeyPoints,
                                  int _patchSize,
                                  int _signatureSize,
                                  int _nstructs,
                                  int _structSize,
                                  int _nviews,
                                  int _compressionMethod,
                                  cv::PatchGenerator* patchGenerator)
{
   cv::Mat mat = cv::cvarrToMat(image);
   std::vector<cv::KeyPoint> keypointVector = std::vector<cv::KeyPoint>(numberOfKeyPoints); 
   memcpy(&keypointVector[0], keypoints, numberOfKeyPoints * sizeof(cv::KeyPoint));
   classifier->trainFromSingleView(mat, keypointVector, _patchSize, _signatureSize, _nstructs, _structSize, _nviews, _compressionMethod, *patchGenerator);
}

//Patch Genetator
CVAPI(cv::PatchGenerator*) CvPatchGeneratorDefaultCreate() { return new cv::PatchGenerator; }
CVAPI(void) CvPatchGeneratorRelease(cv::PatchGenerator* pg) { delete pg ;}

//LDetector
CVAPI(void) CvLDetectorDetectKeyPoints(cv::LDetector* detector, IplImage* image, CvSeq* keypoints, int maxCount, bool scaleCoords)
{
   cvClearSeq(keypoints);
   cv::Mat mat = cv::cvarrToMat(image);
   std::vector<cv::KeyPoint> pts;

   (*detector)(mat, pts, maxCount, scaleCoords);

   int count = pts.size();
   if (count > 0)
      cvSeqPushMulti(keypoints, &pts[0], count);
}

//StarDetector
CVAPI(void) CvStarDetectorDetectKeyPoints(cv::StarDetector* detector, IplImage* image, CvSeq* keypoints)
{
   cvClearSeq(keypoints);
   cv::Mat mat = cv::cvarrToMat(image);
   std::vector<cv::KeyPoint> pts;

   (*detector)(mat, pts);

   int count = pts.size();
   if (count > 0)
      cvSeqPushMulti(keypoints, &pts[0], count);
}

//SURFDetector
CVAPI(void) CvSURFDetectorDetectKeyPoints(cv::SURF* detector, IplImage* image, IplImage* mask, CvSeq* keypoints)
{
   cvClearSeq(keypoints);

   std::vector<cv::KeyPoint> pts;

   cv::Mat mat = cv::cvarrToMat(image);
   cv::Mat maskMat;
   if (mask) maskMat = cv::cvarrToMat(mask);

   (*detector)(mat, maskMat, pts);

   int count = pts.size();
   if (count > 0)
      cvSeqPushMulti(keypoints, &pts[0], count);
}

CVAPI(void) CvSURFDetectorDetect(cv::SURF* detector, IplImage* image, IplImage* mask, CvSeq* keypoints, CvSeq* descriptors, bool useProvidedKeyPoints)
{
   cvClearSeq(keypoints);
   cvClearSeq(descriptors);
   std::vector<cv::KeyPoint> pts;
   std::vector<float> desc;

   cv::Mat mat = cv::cvarrToMat(image);
   cv::Mat maskMat;
   if (mask) maskMat = cv::cvarrToMat(mask);

   (*detector)(mat, maskMat, pts, desc, useProvidedKeyPoints);

   int count = pts.size();
   if (count > 0)
   {
      cvSeqPushMulti(keypoints, &pts[0], count);
      cvSeqPushMulti(descriptors, &desc[0], desc.size());
   }
}


// detect corners using FAST algorithm
CVAPI(void) CvFASTKeyPoints( IplImage* image, CvSeq* keypoints, int threshold, bool nonmax_supression)
{
   cv::Mat mat = cv::cvarrToMat(image);
   std::vector<cv::KeyPoint> pts;
   cv::FAST(mat, pts, threshold, nonmax_supression);
   
   int count = pts.size();
   if (count > 0)
      cvSeqPushMulti(keypoints, &pts[0], count);
}

//Plannar Object Detector
CVAPI(cv::PlanarObjectDetector*) CvPlanarObjectDetectorDefaultCreate() { return new cv::PlanarObjectDetector; }
CVAPI(void) CvPlanarObjectDetectorRelease(cv::PlanarObjectDetector* detector) { delete detector; }
CVAPI(void) CvPlanarObjectDetectorTrain(cv::PlanarObjectDetector* detector, IplImage* image)
{
   cv::Mat mat = cv::cvarrToMat(image);
   std::vector<cv::Mat> pyr;
   cv::buildPyramid(mat, pyr, 20);
   detector->train(pyr);
}
