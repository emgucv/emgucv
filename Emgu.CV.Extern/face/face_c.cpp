//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "face_c.h"

//FaceRecognizer
cv::face::FaceRecognizer* CvEigenFaceRecognizerCreate(int numComponents, double threshold)
{
   cv::Ptr<cv::face::FaceRecognizer> ptr = cv::face::EigenFaceRecognizer::create(numComponents, threshold);
   ptr.addref();
   return ptr.get();
}
    
cv::face::FaceRecognizer* CvFisherFaceRecognizerCreate(int numComponents, double threshold)
{
   cv::Ptr<cv::face::FaceRecognizer> ptr = cv::face::FisherFaceRecognizer::create(numComponents, threshold);
   ptr.addref();
   return ptr.get();
}
    
cv::face::FaceRecognizer* CvLBPHFaceRecognizerCreate(int radius, int neighbors, int gridX, int gridY, double threshold)
{
   cv::Ptr<cv::face::FaceRecognizer> ptr = cv::face::LBPHFaceRecognizer::create(radius, neighbors, gridX, gridY, threshold);
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

void CvFaceRecognizerWrite(cv::face::FaceRecognizer* recognizer, cv::String* fileName)
{
   recognizer->write(*fileName);
}

void CvFaceRecognizerRead(cv::face::FaceRecognizer* recognizer, cv::String* fileName)
{
   recognizer->read(*fileName);
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

cv::face::BIF* cveBIFCreate(int numBands, int numRotations)
{
	cv::Ptr<cv::face::BIF> ptr = cv::face::BIF::create(numBands, numRotations);
	ptr.addref();
	return ptr.get();
}
void cveBIFCompute(cv::face::BIF* bif, cv::_InputArray* image, cv::_OutputArray* features)
{
	bif->compute(*image, *features);
}
void cveBIFRelease(cv::face::BIF** bif)
{
	delete *bif;
	*bif = 0;
}