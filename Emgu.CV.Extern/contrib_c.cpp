//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
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
void CvOctreeGetPointsWithinSphere(cv::Octree* tree, cv::Point3f* center, float radius, CvSeq* pointSeq )
{
   std::vector<cv::Point3f> points; 
   tree->getPointsWithinSphere(*center, radius, points);
   cvClearSeq(pointSeq);
   if (!points.empty())
      cvSeqPushMulti(pointSeq, &points.front(), (int)points.size());
}
void CvOctreeRelease(cv::Octree* tree) { delete tree; } 

//CvAdaptiveSkinDetector
CvAdaptiveSkinDetector* CvAdaptiveSkinDetectorCreate(int samplingDivider, int morphingMethod) { return new CvAdaptiveSkinDetector(samplingDivider, morphingMethod); }
void CvAdaptiveSkinDetectorRelease(CvAdaptiveSkinDetector* detector) { delete detector; }
void CvAdaptiveSkinDetectorProcess(CvAdaptiveSkinDetector* detector, IplImage *inputBGRImage, IplImage *outputHueMask) { detector->process(inputBGRImage, outputHueMask); }

//FaceRecognizer
cv::FaceRecognizer* CvEigenFaceRecognizerCreate(int numComponents, double threshold)
{
   cv::Ptr<cv::FaceRecognizer> ptr = cv::createEigenFaceRecognizer(numComponents, threshold);
   ptr.addref();
   return ptr.obj;
}
    
cv::FaceRecognizer* CvFisherFaceRecognizerCreate(int numComponents, double threshold)
{
   cv::Ptr<cv::FaceRecognizer> ptr = cv::createFisherFaceRecognizer(numComponents, threshold);
   ptr.addref();
   return ptr.obj;
}
    
cv::FaceRecognizer* CvLBPHFaceRecognizerCreate(int radius, int neighbors, int gridX, int gridY, double threshold)
{
   cv::Ptr<cv::FaceRecognizer> ptr = cv::createLBPHFaceRecognizer(radius, neighbors, gridX, gridY, threshold);
   ptr.addref();
   return ptr.obj;
}

void CvFaceRecognizerTrain(cv::FaceRecognizer* recognizer, IplImage** images, int* labels, int count)
{
   std::vector<cv::Mat> imageVec(count);
   std::vector<int> labelVec(count);
   for (int i = 0; i < count; ++i)
   {
      imageVec[i] = cv::cvarrToMat(images[i]);
      labelVec[i] = labels[i];
   }
   recognizer->train(imageVec, labelVec);
}

void CvFaceRecognizerSave(cv::FaceRecognizer* recognizer, const char* fileName)
{
   std::string file(fileName);
   recognizer->save(file);
}

void CvFaceRecognizerLoad(cv::FaceRecognizer* recognizer, const char* fileName)
{
   std::string file(fileName);
   recognizer->load(file);
}

void CvFaceRecognizerPredict(cv::FaceRecognizer* recognizer, IplImage* image, int* label, double* dist)
{
   cv::Mat mat = cv::cvarrToMat(image);
   int l = -1;
   double d = -1;
   recognizer->predict(mat, l, d);
   *label = l;
   *dist = d;
}

void CvFaceRecognizerRelease(cv::FaceRecognizer** recognizer)
{
   delete *recognizer;
   *recognizer = 0;
}

void CvApplyColorMap(IplImage* src, IplImage* dst, int colorMap)
{
   cv::Mat srcMat = cv::cvarrToMat(src);
   cv::Mat dstMat = cv::cvarrToMat(dst);
   cv::applyColorMap(srcMat, dstMat, colorMap);
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

int cvChamferMatching( 
   IplImage* img, IplImage* templ,
   std::vector< std::vector<cv::Point> >* results, std::vector<float>* cost,
   double templScale, int maxMatches,
   double minMatchDistance, int padX,
   int padY, int scales, double minScale, double maxScale,
   double orientationWeight, double truncate)
{
   cv::Mat imgMat = cv::cvarrToMat(img);
   cv::Mat templMat = cv::cvarrToMat(templ);
   return cv::chamerMatching(imgMat, templMat, *results, *cost, templScale, maxMatches, minMatchDistance, padX, padY, scales, minScale, maxScale, orientationWeight, truncate);
}