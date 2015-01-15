//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "face_c.h"

//FaceRecognizer
cv::face::FaceRecognizer* CvEigenFaceRecognizerCreate(int numComponents, double threshold)
{
   cv::Ptr<cv::face::FaceRecognizer> ptr = cv::face::createEigenFaceRecognizer(numComponents, threshold);
   ptr.addref();
   return ptr.get();
}
    
cv::face::FaceRecognizer* CvFisherFaceRecognizerCreate(int numComponents, double threshold)
{
   cv::Ptr<cv::face::FaceRecognizer> ptr = cv::face::createFisherFaceRecognizer(numComponents, threshold);
   ptr.addref();
   return ptr.get();
}
    
cv::face::FaceRecognizer* CvLBPHFaceRecognizerCreate(int radius, int neighbors, int gridX, int gridY, double threshold)
{
   cv::Ptr<cv::face::FaceRecognizer> ptr = cv::face::createLBPHFaceRecognizer(radius, neighbors, gridX, gridY, threshold);
   ptr.addref();
   return ptr.get();
}

void CvFaceRecognizerTrain(cv::face::FaceRecognizer* recognizer, cv::_InputArray* images, cv::_InputArray* labels)
{
   recognizer->train(*images, *labels);
}

void CvFaceRecognizerUpdate(cv::face::FaceRecognizer* recognizer, cv::_InputArray* images, cv::_InputArray* labels)
{
   recognizer->update(*images, *labels);
}

void CvFaceRecognizerSave(cv::face::FaceRecognizer* recognizer, cv::String* fileName)
{
   recognizer->save(*fileName);
}

void CvFaceRecognizerLoad(cv::face::FaceRecognizer* recognizer, cv::String* fileName)
{
   recognizer->load(*fileName);
}

void CvFaceRecognizerPredict(cv::face::FaceRecognizer* recognizer, cv::_InputArray* image, int* label, double* dist)
{
   int l = -1;
   double d = -1;
   recognizer->predict(*image, l, d);
   *label = l;
   *dist = d;
}

void CvFaceRecognizerRelease(cv::face::FaceRecognizer** recognizer)
{
   delete *recognizer;
   *recognizer = 0;
}

