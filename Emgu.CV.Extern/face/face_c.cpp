//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "face_c.h"

//FaceRecognizer
cv::face::FaceRecognizer* cveEigenFaceRecognizerCreate(int numComponents, double threshold)
{
   cv::Ptr<cv::face::FaceRecognizer> ptr = cv::face::EigenFaceRecognizer::create(numComponents, threshold);
   ptr.addref();
   return ptr.get();
}
    
cv::face::FaceRecognizer* cveFisherFaceRecognizerCreate(int numComponents, double threshold)
{
   cv::Ptr<cv::face::FaceRecognizer> ptr = cv::face::FisherFaceRecognizer::create(numComponents, threshold);
   ptr.addref();
   return ptr.get();
}
    
cv::face::FaceRecognizer* cveLBPHFaceRecognizerCreate(int radius, int neighbors, int gridX, int gridY, double threshold)
{
   cv::Ptr<cv::face::FaceRecognizer> ptr = cv::face::LBPHFaceRecognizer::create(radius, neighbors, gridX, gridY, threshold);
   ptr.addref();
   return ptr.get();
}

void cveFaceRecognizerTrain(cv::face::FaceRecognizer* recognizer, cv::_InputArray* images, cv::_InputArray* labels)
{
   recognizer->train(*images, *labels);
}

void cveFaceRecognizerUpdate(cv::face::FaceRecognizer* recognizer, cv::_InputArray* images, cv::_InputArray* labels)
{
   recognizer->update(*images, *labels);
}

void cveFaceRecognizerWrite(cv::face::FaceRecognizer* recognizer, cv::String* fileName)
{
   recognizer->write(*fileName);
}

void cveFaceRecognizerRead(cv::face::FaceRecognizer* recognizer, cv::String* fileName)
{
   recognizer->read(*fileName);
}

void cveFaceRecognizerPredict(cv::face::FaceRecognizer* recognizer, cv::_InputArray* image, int* label, double* dist)
{
   int l = -1;
   double d = -1;
   recognizer->predict(*image, l, d);
   *label = l;
   *dist = d;
}

void cveFaceRecognizerRelease(cv::face::FaceRecognizer** recognizer)
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

cv::face::FacemarkAAM::Params* cveFacemarkAAMParamsCreate()
{
	return new cv::face::FacemarkAAM::Params();
}
void cveFacemarkAAMParamsRelease(cv::face::FacemarkAAM::Params** params)
{
	delete *params;
	*params = 0;
}

cv::face::FacemarkAAM* cveFacemarkAAMCreate(cv::face::FacemarkAAM::Params* parameters, cv::face::Facemark** facemark, cv::Algorithm** algorithm)
{
	cv::Ptr<cv::face::FacemarkAAM> ptr = cv::face::FacemarkAAM::create(*parameters);
	ptr.addref();
	*facemark = dynamic_cast<cv::face::Facemark*>(ptr.get());
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr.get());
	return ptr.get();
}
void cveFacemarkAAMRelease(cv::face::FacemarkAAM** facemark)
{
	delete *facemark;
	*facemark = 0;
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

cv::face::FacemarkLBF* cveFacemarkLBFCreate(cv::face::FacemarkLBF::Params* parameters, cv::face::Facemark** facemark, cv::Algorithm** algorithm)
{
	cv::Ptr<cv::face::FacemarkLBF> ptr = cv::face::FacemarkLBF::create(*parameters);
	ptr.addref();
	*facemark = dynamic_cast<cv::face::Facemark*>(ptr.get());
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr.get());
	return ptr.get();
}
void cveFacemarkLBFRelease(cv::face::FacemarkLBF** facemark)
{
	delete *facemark;
	*facemark = 0;
}

/*
cv::face::FacemarkKazemi::Params* cveFacemarkKazemiParamsCreate()
{
	return new cv::face::FacemarkKazemi::Params();
}
void cveFacemarkKazemiParamsRelease(cv::face::FacemarkKazemi::Params** params)
{
	delete *params;
	*params = 0;
}

cv::face::FacemarkKazemi* cveFacemarkKazemiCreate(cv::face::FacemarkKazemi::Params* parameters, cv::face::Facemark** facemark, cv::Algorithm** algorithm)
{
	cv::Ptr<cv::face::FacemarkKazemi> ptr = cv::face::FacemarkKazemi::create(*parameters);
	ptr.addref();
	*facemark = dynamic_cast<cv::face::Facemark*>(ptr.get());
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr.get());
	return ptr.get();
}
void cveFacemarkKazemiRelease(cv::face::FacemarkKazemi** facemark)
{
	delete *facemark;
	*facemark = 0;
}
*/

typedef struct
{
	CSharp_FaceDetector face_detector_func;
} face_detector_pointer;

bool myDetector(cv::InputArray image, cv::OutputArray faces, void* face_detector_struct)
{
	face_detector_pointer* fds = (face_detector_pointer*)face_detector_struct;
	return (*(fds->face_detector_func))(&image, &faces);
}
bool cveFacemarkSetFaceDetector(cv::face::Facemark* facemark, CSharp_FaceDetector detector)
{
	face_detector_pointer detector_pointer;
	detector_pointer.face_detector_func = detector;
	return facemark->setFaceDetector((cv::face::FN_FaceDetector) myDetector, &detector_pointer);
}


void cveFacemarkLoadModel(cv::face::Facemark* facemark, cv::String* model)
{
	facemark->loadModel(*model);
}
bool cveFacemarkGetFaces(cv::face::Facemark* facemark, cv::_InputArray* image, cv::_OutputArray* faces)
{
	return facemark->getFaces(*image, *faces);
}
bool cveFacemarkFit(cv::face::Facemark* facemark, cv::_InputArray* image, cv::_InputArray* faces, cv::_InputOutputArray* landmarks)
{
	return facemark->fit(*image, *faces, *landmarks);
}
bool cveFacemarkAddTrainingSample(cv::face::Facemark* facemark, cv::_InputArray* image, cv::_InputArray* landmarks)
{
	return facemark->addTrainingSample(*image, *landmarks);
}
void cveFacemarkTraining(cv::face::Facemark* facemark)
{
	facemark->training();
}


void cveDrawFacemarks(cv::_InputOutputArray* image, cv::_InputArray* points, CvScalar* color)
{
	cv::face::drawFacemarks(*image, *points, *color);
}