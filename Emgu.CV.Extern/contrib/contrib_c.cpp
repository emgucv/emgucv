//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "contrib_c.h"

//Octtree
cv::Octree* CvOctreeCreate() { return new cv::Octree(); }
void CvOctreeBuildTree(cv::Octree* tree, cv::Point3f* points, int numberOfPoints, int maxLevels, int minPoints ) 
{ 
   std::vector<cv::Point3f> pts = std::vector<cv::Point3f>(numberOfPoints); 
   memcpy(&pts[0], points, numberOfPoints * sizeof(cv::Point3f));  
   tree->buildTree(pts, maxLevels, minPoints); 
}
void CvOctreeGetPointsWithinSphere(cv::Octree* tree, cv::Point3f* center, float radius, std::vector<cv::Point3f>* points )
{
   tree->getPointsWithinSphere(*center, radius, *points);
}
void CvOctreeRelease(cv::Octree* tree) { delete tree; } 

//CvAdaptiveSkinDetector
CvAdaptiveSkinDetector* CvAdaptiveSkinDetectorCreate(int samplingDivider, int morphingMethod) { return new CvAdaptiveSkinDetector(samplingDivider, morphingMethod); }
void CvAdaptiveSkinDetectorRelease(CvAdaptiveSkinDetector* detector) { delete detector; }
void CvAdaptiveSkinDetectorProcess(CvAdaptiveSkinDetector* detector, IplImage *inputBGRImage, IplImage *outputHueMask) 
{ 
   detector->process(inputBGRImage, outputHueMask); 
}

//FaceRecognizer
cv::FaceRecognizer* CvEigenFaceRecognizerCreate(int numComponents, double threshold)
{
   cv::Ptr<cv::FaceRecognizer> ptr = cv::createEigenFaceRecognizer(numComponents, threshold);
   ptr.addref();
   return ptr.get();
}
    
cv::FaceRecognizer* CvFisherFaceRecognizerCreate(int numComponents, double threshold)
{
   cv::Ptr<cv::FaceRecognizer> ptr = cv::createFisherFaceRecognizer(numComponents, threshold);
   ptr.addref();
   return ptr.get();
}
    
cv::FaceRecognizer* CvLBPHFaceRecognizerCreate(int radius, int neighbors, int gridX, int gridY, double threshold)
{
   cv::Ptr<cv::FaceRecognizer> ptr = cv::createLBPHFaceRecognizer(radius, neighbors, gridX, gridY, threshold);
   ptr.addref();
   return ptr.get();
}

void CvFaceRecognizerTrain(cv::FaceRecognizer* recognizer, cv::_InputArray* images, cv::_InputArray* labels)
{
   recognizer->train(*images, *labels);
}

void CvFaceRecognizerUpdate(cv::FaceRecognizer* recognizer, cv::_InputArray* images, cv::_InputArray* labels)
{
   recognizer->update(*images, *labels);
}

void CvFaceRecognizerSave(cv::FaceRecognizer* recognizer, cv::String* fileName)
{
   recognizer->save(*fileName);
}

void CvFaceRecognizerLoad(cv::FaceRecognizer* recognizer, cv::String* fileName)
{
   recognizer->load(*fileName);
}

void CvFaceRecognizerPredict(cv::FaceRecognizer* recognizer, cv::_InputArray* image, int* label, double* dist)
{
   int l = -1;
   double d = -1;
   recognizer->predict(*image, l, d);
   *label = l;
   *dist = d;
}

void CvFaceRecognizerRelease(cv::FaceRecognizer** recognizer)
{
   delete *recognizer;
   *recognizer = 0;
}

void cveApplyColorMap(cv::_InputArray* src, cv::_OutputArray* dst, int colorMap)
{
   cv::applyColorMap(*src, *dst, colorMap);
}

//LevMarqSparse
cv::LevMarqSparse* CvCreateLevMarqSparse()
{
   return new cv::LevMarqSparse();
}

void CvLevMarqSparseAdjustBundle( int numberOfFrames, int pointCount, CvPoint3D64f* points, CvMat* imagePoints, CvMat*  visibility, std::vector<cv::Mat>* cameraMatrix, std::vector<cv::Mat>* R, std::vector<cv::Mat>* T, std::vector<cv::Mat>* distCoeffs, CvTermCriteria* termCrit)
{
	//Convert 3D points positions
   std::vector<cv::Point3d> pointsInput(0);
   for(int i=0; i<pointCount; i++)
	{
      cv::Point3d point(points[i].x , points[i].y, points[i].z);
		pointsInput.push_back(point);
	}

	//Convert 2xN matrix array of 2D points per view	
	std::vector<std::vector<cv::Point2d> > imagePointsInput;
   std::vector<std::vector<int> > visibilityInput;
   for(int i=0; i< imagePoints->rows; i++)
	{
		std::vector<cv::Point2d> imagePointsTemp;
      std::vector<int> visibilityTemp;
		
      for(int j=0; j<imagePoints->cols; j++)
		{
         double x = imagePoints->data.db[i*imagePoints->cols + j * 2];
         double y = imagePoints->data.db[i*imagePoints->cols + j * 2 + 1];
			cv::Point2d point(x,y);
			imagePointsTemp.push_back(point);
         visibilityTemp.push_back(visibility->data.i[i*imagePoints->cols + j]);
		}
		imagePointsInput.push_back(imagePointsTemp);
      visibilityInput.push_back(visibilityTemp);
	}

	//Adjust bundle
   cv::LevMarqSparse::bundleAdjust(pointsInput,imagePointsInput, visibilityInput, *cameraMatrix, *R, *T, *distCoeffs, *termCrit);

   //copy the data from pointsInput, visibilityInput to the original places
   for (int i = 0; i < pointCount; i++)
   {
      cv::Point3d p = pointsInput[i];
      memcpy(points+i,&p.x, sizeof(CvPoint3D64f)); 
   }
   //image points and visibily are input paremters so we do not need to copy the data back
   //the cameraMatrix, R, T and distortion coefficient has its data modified, no copy is necessary neither.
   
}

void CvReleaseLevMarqSparse(cv::LevMarqSparse** levMarq)
{
   delete *levMarq;
   *levMarq = 0;
}

//ChamferMatching
int cveChamferMatching( 
   cv::Mat* img, cv::Mat* templ,
   std::vector< std::vector<cv::Point> >* results, std::vector<float>* cost,
   double templScale, int maxMatches,
   double minMatchDistance, int padX,
   int padY, int scales, double minScale, double maxScale,
   double orientationWeight, double truncate)
{
   //cv::Mat imgMat = cv::cvarrToMat(img);
   //cv::Mat templMat = cv::cvarrToMat(templ);
   return cv::chamerMatching(*img, *templ, *results, *cost, templScale, maxMatches, minMatchDistance, padX, padY, scales, minScale, maxScale, orientationWeight, truncate);
}

//SelfSimDescriptor
cv::SelfSimDescriptor* CvSelfSimDescriptorCreate(int smallSize,int largeSize, int startDistanceBucket, int numberOfDistanceBuckets, int numberOfAngles)
{  return new cv::SelfSimDescriptor(smallSize, largeSize, startDistanceBucket, numberOfDistanceBuckets, numberOfAngles); }
void CvSelfSimDescriptorRelease(cv::SelfSimDescriptor* descriptor) { delete descriptor; }
void CvSelfSimDescriptorCompute(cv::SelfSimDescriptor* descriptor, cv::Mat* image, std::vector<float>* descriptors, cv::Size* winStride, std::vector<  cv::Point >* locations)
{
   //std::vector<cv::Point> locationVec = std::vector<cv::Point>(numberOfLocation);
   //memcpy(&locationVec[0], locations, sizeof(cv::Point) * numberOfLocation);
   //CV_Assert(numberOfLocation == locationVec.size());
   //cv::Mat imageMat = cv::cvarrToMat(image);
   descriptor->compute(*image, *descriptors, *winStride, *locations);

   //float sumAbs = 0.0f;
   //for (int i = 0; i < descriptors->data.size(); i++)
   //   sumAbs += descriptors->data[i];
   
   //CV_Assert(sumAbs != 0.0f);
   
}
int CvSelfSimDescriptorGetDescriptorSize(cv::SelfSimDescriptor* descriptor) { return static_cast<int>(descriptor->getDescriptorSize()); }
