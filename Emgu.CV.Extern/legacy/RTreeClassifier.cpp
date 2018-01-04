//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "legacy_c.h"

cv::RTreeClassifier* CvRTreeClassifierCreate() { return new cv::RTreeClassifier(); }
void CvRTreeClassifierRelease(cv::RTreeClassifier* classifier) { delete classifier; }
void CvRTreeClassifierTrain(
      cv::RTreeClassifier* classifier, 
      IplImage* train_image,
      CvPoint* train_points,
      int numberOfPoints,
		cv::RNG* rng, 
      int num_trees, int depth,
		int views, size_t reduced_num_dim,
		int num_quant_bits)
{
   std::vector<cv::BaseKeypoint> base_set;
   for (int i=0;i<numberOfPoints;i++)
	{
      base_set.push_back(cv::BaseKeypoint(train_points[i].x,train_points[i].y,const_cast<IplImage*>(train_image)));
	}

   classifier->train(base_set, *rng, num_trees, depth, views, reduced_num_dim, num_quant_bits);
}

int CvRTreeClassifierGetOriginalNumClasses(cv::RTreeClassifier* classifier) { return classifier->original_num_classes(); }
int CvRTreeClassifierGetNumClasses(cv::RTreeClassifier* classifier) { return classifier->classes(); }

int CvRTreeClassifierGetSigniture(
   cv::RTreeClassifier* classifier, 
   IplImage* image, 
   CvPoint* point,
   int patchSize,
   float* signiture)
{
   CvRect roi = cvRect(point->x - (patchSize >> 1), point->y - (patchSize>>1), patchSize, patchSize);
   CvRect originalRoi = cvGetImageROI(image);
   cvSetImageROI(image, roi);
   CvRect roi2 = cvGetImageROI(image);
   if (roi2.width != patchSize || roi2.height != patchSize)
   {
      cvSetImageROI(image, originalRoi);
      return 0;
   }
   IplImage* patch = cvCreateImage(cvSize(roi.width, roi.height), image->depth, image->nChannels);
   cvCopy(image, patch);
   classifier->getSignature(patch, signiture);
   cvReleaseImage(&patch);
   cvSetImageROI(image, originalRoi);
   return 1;
}
