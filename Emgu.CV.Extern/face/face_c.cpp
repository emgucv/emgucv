//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "face_c.h"

//FaceRecognizer
cv::face::EigenFaceRecognizer* cveEigenFaceRecognizerCreate(
	int numComponents, 
	double threshold, 
	cv::face::FaceRecognizer** faceRecognizerPtr,
	cv::face::FaceRecognizer** basicFaceRecognizerPtr,
	cv::Ptr<cv::face::EigenFaceRecognizer>** sharedPtr)
{
   cv::Ptr<cv::face::EigenFaceRecognizer> ptr = cv::face::EigenFaceRecognizer::create(numComponents, threshold);
   *faceRecognizerPtr = dynamic_cast<cv::face::FaceRecognizer*>(ptr.get());
   *basicFaceRecognizerPtr = dynamic_cast<cv::face::BasicFaceRecognizer*>(ptr.get());
   *sharedPtr = new cv::Ptr<cv::face::EigenFaceRecognizer>(ptr);
   return ptr.get();
}
void cveEigenFaceRecognizerRelease(cv::Ptr<cv::face::EigenFaceRecognizer>** sharedPtr)
{
	delete *sharedPtr;
	*sharedPtr = 0;
}

cv::face::FisherFaceRecognizer* cveFisherFaceRecognizerCreate(
	int numComponents,
	double threshold,
	cv::face::FaceRecognizer** faceRecognizerPtr,
	cv::face::FaceRecognizer** basicFaceRecognizerPtr,
	cv::Ptr<cv::face::FisherFaceRecognizer>** sharedPtr)
{
   cv::Ptr<cv::face::FisherFaceRecognizer> ptr = cv::face::FisherFaceRecognizer::create(numComponents, threshold);
   *faceRecognizerPtr = dynamic_cast<cv::face::FaceRecognizer*>(ptr.get());
   *basicFaceRecognizerPtr = dynamic_cast<cv::face::BasicFaceRecognizer*>(ptr.get());
   *sharedPtr = new cv::Ptr<cv::face::FisherFaceRecognizer>(ptr);
   return ptr.get();
}

void cveFisherFaceRecognizerRelease(cv::Ptr<cv::face::FisherFaceRecognizer>** sharedPtr)
{
	delete *sharedPtr;
	*sharedPtr = 0;
}
    
cv::face::LBPHFaceRecognizer* cveLBPHFaceRecognizerCreate(
	int radius,
	int neighbors,
	int gridX,
	int gridY,
	double threshold,
	cv::face::FaceRecognizer** faceRecognizerPtr,
	cv::Ptr<cv::face::LBPHFaceRecognizer>** sharedPtr) 
{
   cv::Ptr<cv::face::LBPHFaceRecognizer> ptr = cv::face::LBPHFaceRecognizer::create(radius, neighbors, gridX, gridY, threshold);
   *faceRecognizerPtr = dynamic_cast<cv::face::FaceRecognizer*>(ptr.get());
   *sharedPtr = new cv::Ptr<cv::face::LBPHFaceRecognizer>(ptr);
   return ptr.get();
}
void cveLBPHFaceRecognizerRelease(cv::Ptr<cv::face::LBPHFaceRecognizer>** sharedPtr)
{
	delete *sharedPtr;
	*sharedPtr = 0;
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

cv::face::BIF* cveBIFCreate(int numBands, int numRotations, cv::Ptr<cv::face::BIF>** sharedPtr)
{
	cv::Ptr<cv::face::BIF> ptr = cv::face::BIF::create(numBands, numRotations);
	*sharedPtr = new cv::Ptr<cv::face::BIF>(ptr);
	return ptr.get();
}
void cveBIFCompute(cv::face::BIF* bif, cv::_InputArray* image, cv::_OutputArray* features)
{
	bif->compute(*image, *features);
}
void cveBIFRelease(cv::Ptr<cv::face::BIF>** sharedPtr)
{
	delete *sharedPtr;
	*sharedPtr = 0;
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

cv::face::FacemarkAAM* cveFacemarkAAMCreate(cv::face::FacemarkAAM::Params* parameters, cv::face::Facemark** facemark, cv::Algorithm** algorithm, cv::Ptr<cv::face::FacemarkAAM>** sharedPtr)
{
	cv::Ptr<cv::face::FacemarkAAM> ptr = cv::face::FacemarkAAM::create(*parameters);
	*sharedPtr = new cv::Ptr<cv::face::FacemarkAAM>(ptr);
	*facemark = dynamic_cast<cv::face::Facemark*>(ptr.get());
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr.get());
	return ptr.get();
}
void cveFacemarkAAMRelease(cv::face::FacemarkAAM** facemark, cv::Ptr<cv::face::FacemarkAAM>** sharedPtr)
{
	delete *sharedPtr;
	*facemark = 0;
	*sharedPtr = 0;
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

cv::face::FacemarkLBF* cveFacemarkLBFCreate(cv::face::FacemarkLBF::Params* parameters, cv::face::Facemark** facemark, cv::Algorithm** algorithm, cv::Ptr<cv::face::FacemarkLBF>** sharedPtr)
{
	cv::Ptr<cv::face::FacemarkLBF> ptr = cv::face::FacemarkLBF::create(*parameters);
	*sharedPtr = new cv::Ptr<cv::face::FacemarkLBF>(ptr);
	*facemark = dynamic_cast<cv::face::Facemark*>(ptr.get());
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr.get());
	return ptr.get();
}
void cveFacemarkLBFRelease(cv::face::FacemarkLBF** facemark, cv::Ptr<cv::face::FacemarkLBF>** sharedPtr)
{
	delete *sharedPtr;
	*facemark = 0;
	*sharedPtr = 0;
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
*/

void cveFacemarkLoadModel(cv::face::Facemark* facemark, cv::String* model)
{
	facemark->loadModel(*model);
}

/*
bool cveFacemarkGetFaces(cv::face::Facemark* facemark, cv::_InputArray* image, cv::_OutputArray* faces)
{
	return facemark->getFaces(*image, *faces);
}
*/
bool cveFacemarkFit(cv::face::Facemark* facemark, cv::_InputArray* image, cv::_InputArray* faces, cv::_InputOutputArray* landmarks)
{
	return facemark->fit(*image, *faces, *landmarks);
}

/*
bool cveFacemarkAddTrainingSample(cv::face::Facemark* facemark, cv::_InputArray* image, cv::_InputArray* landmarks)
{
	return facemark->addTrainingSample(*image, *landmarks);
}
void cveFacemarkTraining(cv::face::Facemark* facemark)
{
	facemark->training();
}
*/

void cveDrawFacemarks(cv::_InputOutputArray* image, cv::_InputArray* points, CvScalar* color)
{
	cv::face::drawFacemarks(*image, *points, *color);
}

cv::face::MACE* cveMaceCreate(int imgSize, cv::Ptr<cv::face::MACE>** sharedPtr)
{
	cv::Ptr<cv::face::MACE> mace = cv::face::MACE::create(imgSize);
	*sharedPtr = new cv::Ptr<cv::face::MACE>(mace);
	return (*sharedPtr)->get();
}
void cveMaceSalt(cv::face::MACE* mace, cv::String* passphrase)
{
	mace->salt(*passphrase);
}
void cveMaceTrain(cv::face::MACE* mace, cv::_InputArray* images)
{
	mace->train(*images);
}
bool cveMaceSame(cv::face::MACE* mace, cv::_InputArray* query)
{
	return mace->same(*query);
}
void cveMaceRelease(cv::Ptr<cv::face::MACE>** sharedPtr)
{
	delete *sharedPtr;
	*sharedPtr = 0;
}