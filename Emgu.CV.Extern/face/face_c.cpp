//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
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

cv::face::FacemarkLBF::Params* cveFacemarkLBFParamsCreate()
{
	return new cv::face::FacemarkLBF::Params();
}
void cveFacemarkLBFParamsRelease(cv::face::FacemarkLBF::Params** params)
{
	delete *params;
	*params = 0;
}

cv::face::FacemarkLBF* cveFacemarkLBFCreate(cv::face::FacemarkLBF::Params* parameters, cv::face::Facemark** facemark)
{
	cv::Ptr<cv::face::FacemarkLBF> ptr = cv::face::FacemarkLBF::create(*parameters);
	ptr.addref();
	*facemark = dynamic_cast<cv::face::Facemark*>(ptr.get());
	return ptr.get();
}
void cveFacemarkLBFRelease(cv::face::FacemarkLBF** facemark)
{
	delete *facemark;
	*facemark = 0;
}

bool myDetector(cv::InputArray image, cv::OutputArray faces, CSharp_FaceDetector face_detector)
{
	return (*face_detector)(&image, &faces);
}
bool cveFacemarkSetFaceDetector(cv::face::Facemark* facemark, CSharp_FaceDetector detector)
{
	return facemark->setFaceDetector((cv::face::FN_FaceDetector) myDetector, detector);
}