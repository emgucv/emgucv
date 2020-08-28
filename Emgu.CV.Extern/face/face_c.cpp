//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "face_c.h"

//FaceRecognizer
cv::face::EigenFaceRecognizer* cveEigenFaceRecognizerCreate(
	int numComponents,
	double threshold,
	cv::face::FaceRecognizer** faceRecognizerPtr,
	cv::face::BasicFaceRecognizer** basicFaceRecognizerPtr,
	cv::Ptr<cv::face::EigenFaceRecognizer>** sharedPtr)
{
#ifdef HAVE_OPENCV_FACE
	cv::Ptr<cv::face::EigenFaceRecognizer> ptr = cv::face::EigenFaceRecognizer::create(numComponents, threshold);
	*faceRecognizerPtr = dynamic_cast<cv::face::FaceRecognizer*>(ptr.get());
	*basicFaceRecognizerPtr = dynamic_cast<cv::face::BasicFaceRecognizer*>(ptr.get());
	*sharedPtr = new cv::Ptr<cv::face::EigenFaceRecognizer>(ptr);
	return ptr.get();
#else
	throw_no_face();
#endif
}
void cveEigenFaceRecognizerRelease(cv::Ptr<cv::face::EigenFaceRecognizer>** sharedPtr)
{
#ifdef HAVE_OPENCV_FACE
	delete* sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_face();
#endif
}

cv::face::FisherFaceRecognizer* cveFisherFaceRecognizerCreate(
	int numComponents,
	double threshold,
	cv::face::FaceRecognizer** faceRecognizerPtr,
	cv::face::FaceRecognizer** basicFaceRecognizerPtr,
	cv::Ptr<cv::face::FisherFaceRecognizer>** sharedPtr)
{
#ifdef HAVE_OPENCV_FACE
	cv::Ptr<cv::face::FisherFaceRecognizer> ptr = cv::face::FisherFaceRecognizer::create(numComponents, threshold);
	*faceRecognizerPtr = dynamic_cast<cv::face::FaceRecognizer*>(ptr.get());
	*basicFaceRecognizerPtr = dynamic_cast<cv::face::BasicFaceRecognizer*>(ptr.get());
	*sharedPtr = new cv::Ptr<cv::face::FisherFaceRecognizer>(ptr);
	return ptr.get();
#else
	throw_no_face();
#endif
}

void cveFisherFaceRecognizerRelease(cv::Ptr<cv::face::FisherFaceRecognizer>** sharedPtr)
{
#ifdef HAVE_OPENCV_FACE
	delete* sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_face();
#endif
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
#ifdef HAVE_OPENCV_FACE
	cv::Ptr<cv::face::LBPHFaceRecognizer> ptr = cv::face::LBPHFaceRecognizer::create(radius, neighbors, gridX, gridY, threshold);
	*faceRecognizerPtr = dynamic_cast<cv::face::FaceRecognizer*>(ptr.get());
	*sharedPtr = new cv::Ptr<cv::face::LBPHFaceRecognizer>(ptr);
	return ptr.get();
#else
	throw_no_face();
#endif
}
void cveLBPHFaceRecognizerRelease(cv::Ptr<cv::face::LBPHFaceRecognizer>** sharedPtr)
{
#ifdef HAVE_OPENCV_FACE
	delete* sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_face();
#endif
}
void cveLBPHFaceRecognizerGetHistograms(cv::face::LBPHFaceRecognizer* recognizer, std::vector<cv::Mat>* histograms)
{
#ifdef HAVE_OPENCV_FACE
	std::vector<cv::Mat> h = recognizer->getHistograms();
	histograms->clear();
	std::copy(h.begin(), h.end(), std::back_inserter(*histograms));
#else
	throw_no_face();
#endif
}

void cveFaceRecognizerTrain(cv::face::FaceRecognizer* recognizer, cv::_InputArray* images, cv::_InputArray* labels)
{
#ifdef HAVE_OPENCV_FACE
	recognizer->train(*images, *labels);
#else
	throw_no_face();
#endif
}

void cveFaceRecognizerUpdate(cv::face::FaceRecognizer* recognizer, cv::_InputArray* images, cv::_InputArray* labels)
{
#ifdef HAVE_OPENCV_FACE
	recognizer->update(*images, *labels);
#else
	throw_no_face();
#endif
}

void cveFaceRecognizerWrite(cv::face::FaceRecognizer* recognizer, cv::String* fileName)
{
#ifdef HAVE_OPENCV_FACE
	recognizer->write(*fileName);
#else
	throw_no_face();
#endif
}

void cveFaceRecognizerRead(cv::face::FaceRecognizer* recognizer, cv::String* fileName)
{
#ifdef HAVE_OPENCV_FACE
	recognizer->read(*fileName);
#else
	throw_no_face();
#endif
}

void cveFaceRecognizerPredict(cv::face::FaceRecognizer* recognizer, cv::_InputArray* image, int* label, double* dist)
{
#ifdef HAVE_OPENCV_FACE
	int l = -1;
	double d = -1;
	recognizer->predict(*image, l, d);
	*label = l;
	*dist = d;
#else
	throw_no_face();
#endif
}

cv::face::BIF* cveBIFCreate(int numBands, int numRotations, cv::Ptr<cv::face::BIF>** sharedPtr)
{
#ifdef HAVE_OPENCV_FACE
	cv::Ptr<cv::face::BIF> ptr = cv::face::BIF::create(numBands, numRotations);
	*sharedPtr = new cv::Ptr<cv::face::BIF>(ptr);
	return ptr.get();
#else
	throw_no_face();
#endif
}
void cveBIFCompute(cv::face::BIF* bif, cv::_InputArray* image, cv::_OutputArray* features)
{
#ifdef HAVE_OPENCV_FACE
	bif->compute(*image, *features);
#else
	throw_no_face();
#endif
}
void cveBIFRelease(cv::Ptr<cv::face::BIF>** sharedPtr)
{
#ifdef HAVE_OPENCV_FACE
	delete* sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_face();
#endif
}

cv::face::FacemarkAAM::Params* cveFacemarkAAMParamsCreate()
{
#ifdef HAVE_OPENCV_FACE
	return new cv::face::FacemarkAAM::Params();
#else
	throw_no_face();
#endif
}
void cveFacemarkAAMParamsRelease(cv::face::FacemarkAAM::Params** params)
{
#ifdef HAVE_OPENCV_FACE
	delete* params;
	*params = 0;
#else
	throw_no_face();
#endif
}

cv::face::FacemarkAAM* cveFacemarkAAMCreate(cv::face::FacemarkAAM::Params* parameters, cv::face::Facemark** facemark, cv::Algorithm** algorithm, cv::Ptr<cv::face::FacemarkAAM>** sharedPtr)
{
#ifdef HAVE_OPENCV_FACE
	cv::Ptr<cv::face::FacemarkAAM> ptr = cv::face::FacemarkAAM::create(*parameters);
	*sharedPtr = new cv::Ptr<cv::face::FacemarkAAM>(ptr);
	*facemark = dynamic_cast<cv::face::Facemark*>(ptr.get());
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr.get());
	return ptr.get();
#else
	throw_no_face();
#endif
}
void cveFacemarkAAMRelease(cv::face::FacemarkAAM** facemark, cv::Ptr<cv::face::FacemarkAAM>** sharedPtr)
{
#ifdef HAVE_OPENCV_FACE
	delete* sharedPtr;
	*facemark = 0;
	*sharedPtr = 0;
#else
	throw_no_face();
#endif
}


cv::face::FacemarkLBF::Params* cveFacemarkLBFParamsCreate()
{
#ifdef HAVE_OPENCV_FACE
	return new cv::face::FacemarkLBF::Params();
#else
	throw_no_face();
#endif
}
void cveFacemarkLBFParamsRelease(cv::face::FacemarkLBF::Params** params)
{
#ifdef HAVE_OPENCV_FACE
	delete* params;
	*params = 0;
#else
	throw_no_face();
#endif
}

cv::face::FacemarkLBF* cveFacemarkLBFCreate(cv::face::FacemarkLBF::Params* parameters, cv::face::Facemark** facemark, cv::Algorithm** algorithm, cv::Ptr<cv::face::FacemarkLBF>** sharedPtr)
{
#ifdef HAVE_OPENCV_FACE
	cv::Ptr<cv::face::FacemarkLBF> ptr = cv::face::FacemarkLBF::create(*parameters);
	*sharedPtr = new cv::Ptr<cv::face::FacemarkLBF>(ptr);
	*facemark = dynamic_cast<cv::face::Facemark*>(ptr.get());
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr.get());
	return ptr.get();
#else
	throw_no_face();
#endif
}
void cveFacemarkLBFRelease(cv::face::FacemarkLBF** facemark, cv::Ptr<cv::face::FacemarkLBF>** sharedPtr)
{
#ifdef HAVE_OPENCV_FACE
	delete* sharedPtr;
	*facemark = 0;
	*sharedPtr = 0;
#else
	throw_no_face();
#endif
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
#ifdef HAVE_OPENCV_FACE
	facemark->loadModel(*model);
#else
	throw_no_face();
#endif
}

/*
bool cveFacemarkGetFaces(cv::face::Facemark* facemark, cv::_InputArray* image, cv::_OutputArray* faces)
{
	return facemark->getFaces(*image, *faces);
}
*/
bool cveFacemarkFit(cv::face::Facemark* facemark, cv::_InputArray* image, cv::_InputArray* faces, cv::_InputOutputArray* landmarks)
{
#ifdef HAVE_OPENCV_FACE
	return facemark->fit(*image, *faces, *landmarks);
#else
	throw_no_face();
#endif
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
#ifdef HAVE_OPENCV_FACE
	cv::face::drawFacemarks(*image, *points, *color);
#else
	throw_no_face();
#endif
}

cv::face::MACE* cveMaceCreate(int imgSize, cv::Ptr<cv::face::MACE>** sharedPtr)
{
#ifdef HAVE_OPENCV_FACE
	cv::Ptr<cv::face::MACE> mace = cv::face::MACE::create(imgSize);
	*sharedPtr = new cv::Ptr<cv::face::MACE>(mace);
	return (*sharedPtr)->get();
#else
	throw_no_face();
#endif
}
void cveMaceSalt(cv::face::MACE* mace, cv::String* passphrase)
{
#ifdef HAVE_OPENCV_FACE
	mace->salt(*passphrase);
#else
	throw_no_face();
#endif
}
void cveMaceTrain(cv::face::MACE* mace, cv::_InputArray* images)
{
#ifdef HAVE_OPENCV_FACE
	mace->train(*images);
#else
	throw_no_face();
#endif
}
bool cveMaceSame(cv::face::MACE* mace, cv::_InputArray* query)
{
#ifdef HAVE_OPENCV_FACE
	return mace->same(*query);
#else
	throw_no_face();
#endif
}
void cveMaceRelease(cv::Ptr<cv::face::MACE>** sharedPtr)
{
#ifdef HAVE_OPENCV_FACE
	delete* sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_face();
#endif
}