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

//SelfSimDescriptor
CVAPI(cv::SelfSimDescriptor*) CvSelfSimDescriptorCreate(int smallSize,int largeSize, int startDistanceBucket, int numberOfDistanceBuckets, int numberOfAngles)
{  return new cv::SelfSimDescriptor(smallSize, largeSize, startDistanceBucket, numberOfDistanceBuckets, numberOfAngles); }
CVAPI(void) CvSelfSimDescriptorRelease(cv::SelfSimDescriptor* descriptor) { delete descriptor; }
CVAPI(void) CvSelfSimDescriptorCompute(cv::SelfSimDescriptor* descriptor, IplImage* image, CvSeq* descriptors, cv::Size winStride, cv::Point* locations, int numberOfLocation)
{
   std::vector<float> descriptorVec;
   std::vector<cv::Point> locationVec = std::vector<cv::Point>(numberOfLocation);
   memcpy(&locationVec[0], locations, sizeof(cv::Point) * numberOfLocation);

   descriptor->compute(cv::cvarrToMat(image), descriptorVec, winStride, locationVec);
   
   if (descriptorVec.size() > 0)
      cvSeqPushMulti(descriptors, &descriptorVec[0], descriptorVec.size());
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
   pyr.push_back(cv::cvarrToMat(image));
   objectDetector->train(pyr, _npoints, _patchSize, _nstructs, _structSize, _nviews, *detector, *patchGenerator);
}
CVAPI(void) CvPlanarObjectDetectorDetect(cv::PlanarObjectDetector* detector, IplImage* image, CvMat* homography, CvSeq* corners)
{
   std::vector<cv::Point2f> cornerVec;
   (*detector)(cv::cvarrToMat(image), cv::cvarrToMat(homography), cornerVec);
   if (cornerVec.size() > 0)
      cvSeqPushMulti(corners, &cornerVec[0], cornerVec.size());
}
CVAPI(void) CvPlanarObjectDetectorGetModelPoints(cv::PlanarObjectDetector* detector, CvSeq* modelPoints)
{
   std::vector<cv::KeyPoint> modelPtVec = detector->getModelPoints();
   if (modelPtVec.size() > 0)
      cvSeqPushMulti(modelPoints, &modelPtVec[0], modelPtVec.size());
}