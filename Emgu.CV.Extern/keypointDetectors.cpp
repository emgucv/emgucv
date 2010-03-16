#include "cvaux.h"
#include "vectorOfFloat.h"

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
CVAPI(void) CvPatchGeneratorInit(cv::PatchGenerator* pg) 
{ 
   cv::PatchGenerator defaultPG;
   memcpy(pg, &defaultPG, sizeof(cv::PatchGenerator));
}


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

//SelfSimDescriptor
CVAPI(cv::SelfSimDescriptor*) CvSelfSimDescriptorCreate(int smallSize,int largeSize, int startDistanceBucket, int numberOfDistanceBuckets, int numberOfAngles)
{  return new cv::SelfSimDescriptor(smallSize, largeSize, startDistanceBucket, numberOfDistanceBuckets, numberOfAngles); }
CVAPI(void) CvSelfSimDescriptorRelease(cv::SelfSimDescriptor* descriptor) { delete descriptor; }
CVAPI(void) CvSelfSimDescriptorCompute(cv::SelfSimDescriptor* descriptor, IplImage* image, vectorOfFloat* descriptors, cv::Size* winStride, cv::Point* locations, int numberOfLocation)
{
   std::vector<cv::Point> locationVec = std::vector<cv::Point>(numberOfLocation);
   memcpy(&locationVec[0], locations, sizeof(cv::Point) * numberOfLocation);
   //CV_Assert(numberOfLocation == locationVec.size());
   cv::Mat imageMat = cv::cvarrToMat(image);
   descriptor->compute(imageMat, descriptors->data, *winStride, locationVec);

   //float sumAbs = 0.0f;
   //for (int i = 0; i < descriptors->data.size(); i++)
   //   sumAbs += descriptors->data[i];
   
   //CV_Assert(sumAbs != 0.0f);
   
}
CVAPI(int) CvSelfSimDescriptorGetDescriptorSize(cv::SelfSimDescriptor* descriptor) { return descriptor->getDescriptorSize(); }

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

CVAPI(void) CvSURFDetectorDetectFeature(cv::SURF* detector, IplImage* image, IplImage* mask, CvSeq* keypoints, vectorOfFloat* descriptors)
{
   cv::Mat mat = cv::cvarrToMat(image);
   cv::Mat maskMat;
   if (mask) maskMat = cv::cvarrToMat(mask);
   std::vector<cv::KeyPoint> pts;
   (*detector)(mat, maskMat, pts, descriptors->data, false);

   int count = pts.size();
   if (count > 0)
   {
      cvSeqPushMulti(keypoints, &pts[0], count);
   }
}

CVAPI(void) CvSURFDetectorComputeDescriptors(cv::SURF* detector, IplImage* image, IplImage* mask, cv::KeyPoint* keypoints, int numberOfKeyPoints, vectorOfFloat* descriptors)
{
   cv::Mat mat = cv::cvarrToMat(image);
   cv::Mat maskMat;
   if (mask) maskMat = cv::cvarrToMat(mask);
   std::vector<cv::KeyPoint> pts = std::vector<cv::KeyPoint>(numberOfKeyPoints);
   memcpy(&pts[0], keypoints, sizeof(cv::KeyPoint) * numberOfKeyPoints);
   (*detector)(mat, maskMat, pts, descriptors->data, true);
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
CVAPI(void) CvPlanarObjectDetectorTrain(
   cv::PlanarObjectDetector* objectDetector, 
   IplImage* image, 
   int _npoints,
   int _patchSize,
   int _nstructs,
   int _structSize,
   int _nviews,
   cv::LDetector* detector,
   cv::PatchGenerator* patchGenerator)
{
   std::vector<cv::Mat> pyr;
   cv::Mat imageMat = cv::cvarrToMat(image);
   pyr.push_back(imageMat);
   objectDetector->train(pyr, _npoints, _patchSize, _nstructs, _structSize, _nviews, *detector, *patchGenerator);
}
CVAPI(void) CvPlanarObjectDetectorDetect(cv::PlanarObjectDetector* detector, IplImage* image, CvMat* homography, CvSeq* corners)
{
   std::vector<cv::Point2f> cornerVec;
   cv::Mat imageMat = cv::cvarrToMat(image);
   cv::Mat homographyMat = cv::cvarrToMat(homography);
   (*detector)(imageMat, homographyMat,  cornerVec);
   if (cornerVec.size() > 0)
      cvSeqPushMulti(corners, &cornerVec[0], cornerVec.size());
}
CVAPI(void) CvPlanarObjectDetectorGetModelPoints(cv::PlanarObjectDetector* detector, CvSeq* modelPoints)
{
   std::vector<cv::KeyPoint> modelPtVec = detector->getModelPoints();
   if (modelPtVec.size() > 0)
      cvSeqPushMulti(modelPoints, &modelPtVec[0], modelPtVec.size());
}
