//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
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
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveEigenFaceRecognizerRelease(cv::Ptr<cv::face::EigenFaceRecognizer>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_FACE
		delete* sharedPtr;
		*sharedPtr = 0;
	#else
		throw_no_face();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::face::FisherFaceRecognizer* cveFisherFaceRecognizerCreate(
	int numComponents,
	double threshold,
	cv::face::FaceRecognizer** faceRecognizerPtr,
	cv::face::FaceRecognizer** basicFaceRecognizerPtr,
	cv::Ptr<cv::face::FisherFaceRecognizer>** sharedPtr)
{
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveFisherFaceRecognizerRelease(cv::Ptr<cv::face::FisherFaceRecognizer>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_FACE
		delete* sharedPtr;
		*sharedPtr = 0;
	#else
		throw_no_face();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
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
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveLBPHFaceRecognizerRelease(cv::Ptr<cv::face::LBPHFaceRecognizer>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_FACE
		delete* sharedPtr;
		*sharedPtr = 0;
	#else
		throw_no_face();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveLBPHFaceRecognizerGetHistograms(cv::face::LBPHFaceRecognizer* recognizer, std::vector<cv::Mat>* histograms)
{
	try
	{
	#ifdef HAVE_OPENCV_FACE
		std::vector<cv::Mat> h = recognizer->getHistograms();
		histograms->clear();
		std::copy(h.begin(), h.end(), std::back_inserter(*histograms));
	#else
		throw_no_face();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveFaceRecognizerTrain(cv::face::FaceRecognizer* recognizer, cv::_InputArray* images, cv::_InputArray* labels)
{
	try
	{
	#ifdef HAVE_OPENCV_FACE
		recognizer->train(*images, *labels);
	#else
		throw_no_face();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveFaceRecognizerUpdate(cv::face::FaceRecognizer* recognizer, cv::_InputArray* images, cv::_InputArray* labels)
{
	try
	{
	#ifdef HAVE_OPENCV_FACE
		recognizer->update(*images, *labels);
	#else
		throw_no_face();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveFaceRecognizerWrite(cv::face::FaceRecognizer* recognizer, cv::String* fileName)
{
	try
	{
	#ifdef HAVE_OPENCV_FACE
		recognizer->write(*fileName);
	#else
		throw_no_face();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveFaceRecognizerRead(cv::face::FaceRecognizer* recognizer, cv::String* fileName)
{
	try
	{
	#ifdef HAVE_OPENCV_FACE
		recognizer->read(*fileName);
	#else
		throw_no_face();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveFaceRecognizerPredict(cv::face::FaceRecognizer* recognizer, cv::_InputArray* image, int* label, double* dist)
{
	try
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
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveFaceRecognizerSetLabelInfo(cv::face::FaceRecognizer* recognizer, int label, cv::String* strInfo)
{
	try
	{
	#ifdef HAVE_OPENCV_FACE
		recognizer->setLabelInfo(label, *strInfo);
	#else
		throw_no_face();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveFaceRecognizerGetLabelInfo(cv::face::FaceRecognizer* recognizer, int label, cv::String* strInfo)
{
	try
	{
	#ifdef HAVE_OPENCV_FACE
		cv::String str = recognizer->getLabelInfo(label);
		*strInfo = str;
	#else
		throw_no_face();
	#endif		
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}


cv::face::BIF* cveBIFCreate(int numBands, int numRotations, cv::Ptr<cv::face::BIF>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_FACE
		cv::Ptr<cv::face::BIF> ptr = cv::face::BIF::create(numBands, numRotations);
		*sharedPtr = new cv::Ptr<cv::face::BIF>(ptr);
		return ptr.get();
	#else
		throw_no_face();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveBIFCompute(cv::face::BIF* bif, cv::_InputArray* image, cv::_OutputArray* features)
{
	try
	{
	#ifdef HAVE_OPENCV_FACE
		bif->compute(*image, *features);
	#else
		throw_no_face();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveBIFRelease(cv::Ptr<cv::face::BIF>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_FACE
		delete* sharedPtr;
		*sharedPtr = 0;
	#else
		throw_no_face();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::face::FacemarkAAM::Params* cveFacemarkAAMParamsCreate()
{
	try
	{
	#ifdef HAVE_OPENCV_FACE
		return new cv::face::FacemarkAAM::Params();
	#else
		throw_no_face();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveFacemarkAAMParamsRelease(cv::face::FacemarkAAM::Params** params)
{
	try
	{
	#ifdef HAVE_OPENCV_FACE
		delete* params;
		*params = 0;
	#else
		throw_no_face();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::face::FacemarkAAM* cveFacemarkAAMCreate(cv::face::FacemarkAAM::Params* parameters, cv::face::Facemark** facemark, cv::Algorithm** algorithm, cv::Ptr<cv::face::FacemarkAAM>** sharedPtr)
{
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveFacemarkAAMRelease(cv::face::FacemarkAAM** facemark, cv::Ptr<cv::face::FacemarkAAM>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_FACE
		delete* sharedPtr;
		*facemark = 0;
		*sharedPtr = 0;
	#else
		throw_no_face();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}


cv::face::FacemarkLBF::Params* cveFacemarkLBFParamsCreate()
{
	try
	{
	#ifdef HAVE_OPENCV_FACE
		return new cv::face::FacemarkLBF::Params();
	#else
		throw_no_face();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveFacemarkLBFParamsRelease(cv::face::FacemarkLBF::Params** params)
{
	try
	{
	#ifdef HAVE_OPENCV_FACE
		delete* params;
		*params = 0;
	#else
		throw_no_face();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::face::FacemarkLBF* cveFacemarkLBFCreate(cv::face::FacemarkLBF::Params* parameters, cv::face::Facemark** facemark, cv::Algorithm** algorithm, cv::Ptr<cv::face::FacemarkLBF>** sharedPtr)
{
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveFacemarkLBFRelease(cv::face::FacemarkLBF** facemark, cv::Ptr<cv::face::FacemarkLBF>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_FACE
		delete* sharedPtr;
		*facemark = 0;
		*sharedPtr = 0;
	#else
		throw_no_face();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::face::FacemarkKazemi::Params* cveFacemarkKazemiParamsCreate()
{
	try
	{
	#ifdef HAVE_OPENCV_FACE
		return new cv::face::FacemarkKazemi::Params();
	#else
		throw_no_face();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveFacemarkKazemiParamsRelease(cv::face::FacemarkKazemi::Params** params)
{
	try
	{
	#ifdef HAVE_OPENCV_FACE
		delete* params;
		*params = 0;
	#else
		throw_no_face();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::face::FacemarkKazemi* cveFacemarkKazemiCreate(cv::face::FacemarkKazemi::Params* parameters, cv::face::Facemark** facemark, cv::Algorithm** algorithm, cv::Ptr<cv::face::FacemarkKazemi>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_FACE
		cv::Ptr<cv::face::FacemarkKazemi> ptr = cv::face::FacemarkKazemi::create(*parameters);
		*sharedPtr = new cv::Ptr<cv::face::FacemarkKazemi>(ptr);
		*facemark = dynamic_cast<cv::face::Facemark*>(ptr.get());
		*algorithm = dynamic_cast<cv::Algorithm*>(ptr.get());
		return ptr.get();
	#else
		throw_no_face();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveFacemarkKazemiRelease(cv::face::FacemarkKazemi** facemark, cv::Ptr<cv::face::FacemarkKazemi>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_FACE
		delete* sharedPtr;
		*facemark = 0;
		*sharedPtr = 0;
	#else
		throw_no_face();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
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
#ifdef HAVE_OPENCV_FACE
// addTrainingSample/training are only declared on FacemarkTrain, not the base Facemark class
// (FacemarkKazemi, for instance, derives from Facemark directly and does not implement them).
static cv::face::FacemarkTrain* toFacemarkTrain(cv::face::Facemark* facemark)
{
	cv::face::FacemarkTrain* t = dynamic_cast<cv::face::FacemarkTrain*>(facemark);
	if (!t)
		CV_Error(cv::Error::StsBadFunc, "This Facemark implementation does not support this operation (not a FacemarkTrain).");
	return t;
}

// getFaces/setFaceDetector are declared separately, with identical signatures, on both
// FacemarkTrain and FacemarkKazemi (they do not share a common virtual base that declares
// them), so both need to be tried before concluding the implementation doesn't support it.
#endif

bool cveFacemarkSetFaceDetector(cv::face::Facemark* facemark, CSharp_FaceDetector detector)
{
	try
	{
#ifdef HAVE_OPENCV_FACE
		// Heap-allocated so it remains valid for the lifetime of the Facemark object;
		// setFaceDetector stores this pointer and invokes it later from getFaces()/fit(),
		// so it must outlive this call, not be a stack-local.
		face_detector_pointer* detector_pointer = new face_detector_pointer();
		detector_pointer->face_detector_func = detector;
		cv::face::FN_FaceDetector fn = (cv::face::FN_FaceDetector) myDetector;
		if (cv::face::FacemarkTrain* t = dynamic_cast<cv::face::FacemarkTrain*>(facemark))
			return t->setFaceDetector(fn, detector_pointer);
		if (cv::face::FacemarkKazemi* k = dynamic_cast<cv::face::FacemarkKazemi*>(facemark))
			return k->setFaceDetector(fn, detector_pointer);
		CV_Error(cv::Error::StsBadFunc, "This Facemark implementation does not support setFaceDetector.");
#else
		throw_no_face();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

void cveFacemarkLoadModel(cv::face::Facemark* facemark, cv::String* model)
{
	try
	{
#ifdef HAVE_OPENCV_FACE
		facemark->loadModel(*model);
#else
		throw_no_face();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

bool cveFacemarkGetFaces(cv::face::Facemark* facemark, cv::_InputArray* image, cv::_OutputArray* faces)
{
	try
	{
#ifdef HAVE_OPENCV_FACE
		if (cv::face::FacemarkTrain* t = dynamic_cast<cv::face::FacemarkTrain*>(facemark))
			return t->getFaces(*image, *faces);
		if (cv::face::FacemarkKazemi* k = dynamic_cast<cv::face::FacemarkKazemi*>(facemark))
			return k->getFaces(*image, *faces);
		CV_Error(cv::Error::StsBadFunc, "This Facemark implementation does not support getFaces.");
#else
		throw_no_face();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

bool cveFacemarkFit(cv::face::Facemark* facemark, cv::_InputArray* image, cv::_InputArray* faces, cv::_OutputArray* landmarks)
{
	try
	{
#ifdef HAVE_OPENCV_FACE
		// Run fit() into a temporary std::vector<cv::Mat> so that _copyVector2Output
		// takes its isMatVector() branch (which uses getMatRef — a true reference —
		// ensuring the write sticks rather than going to a reallocated header copy).
		std::vector<cv::Mat> tmpLandmarks;
		bool result = facemark->fit(*image, *faces, tmpLandmarks);
		if (result)
		{
			auto* pOut = reinterpret_cast<std::vector<std::vector<cv::Point2f>>*>(landmarks->getObj());
			pOut->resize(tmpLandmarks.size());
			for (size_t i = 0; i < tmpLandmarks.size(); i++)
				tmpLandmarks[i].copyTo((*pOut)[i]);
		}
		return result;
#else
		throw_no_face();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

bool cveFacemarkAddTrainingSample(cv::face::Facemark* facemark, cv::_InputArray* image, cv::_InputArray* landmarks)
{
	try
	{
#ifdef HAVE_OPENCV_FACE
		return toFacemarkTrain(facemark)->addTrainingSample(*image, *landmarks);
#else
		throw_no_face();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}
void cveFacemarkTraining(cv::face::Facemark* facemark)
{
	try
	{
#ifdef HAVE_OPENCV_FACE
		toFacemarkTrain(facemark)->training();
#else
		throw_no_face();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveDrawFacemarks(cv::_InputOutputArray* image, cv::_InputArray* points, cv::Scalar* color)
{
	try
	{
	#ifdef HAVE_OPENCV_FACE
		cv::face::drawFacemarks(*image, *points, *color);
	#else
		throw_no_face();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::face::MACE* cveMaceCreate(int imgSize, cv::Ptr<cv::face::MACE>** sharedPtr, cv::Algorithm** algorithm)
{
	try
	{
	#ifdef HAVE_OPENCV_FACE
		cv::Ptr<cv::face::MACE> mace = cv::face::MACE::create(imgSize);
		*sharedPtr = new cv::Ptr<cv::face::MACE>(mace);
		*algorithm = dynamic_cast<cv::Algorithm*>(mace.get());
		return (*sharedPtr)->get();
	#else
		throw_no_face();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

cv::face::MACE* cveMaceCreate2(cv::String* fileName, cv::String* objName, cv::Ptr<cv::face::MACE>** sharedPtr, cv::Algorithm** algorithm)
{
	try
	{
	#ifdef HAVE_OPENCV_FACE
		cv::Ptr<cv::face::MACE> mace = cv::face::MACE::load(*fileName, *objName);
		*sharedPtr = new cv::Ptr<cv::face::MACE>(mace);
		*algorithm = dynamic_cast<cv::Algorithm*>(mace.get());
		return (*sharedPtr)->get();
	#else
		throw_no_face();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveMaceSalt(cv::face::MACE* mace, cv::String* passphrase)
{
	try
	{
	#ifdef HAVE_OPENCV_FACE
		mace->salt(*passphrase);
	#else
		throw_no_face();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveMaceTrain(cv::face::MACE* mace, cv::_InputArray* images)
{
	try
	{
	#ifdef HAVE_OPENCV_FACE
		mace->train(*images);
	#else
		throw_no_face();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
bool cveMaceSame(cv::face::MACE* mace, cv::_InputArray* query)
{
	try
	{
	#ifdef HAVE_OPENCV_FACE
		return mace->same(*query);
	#else
		throw_no_face();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}
void cveMaceRelease(cv::Ptr<cv::face::MACE>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_FACE
		delete* sharedPtr;
		*sharedPtr = 0;
	#else
		throw_no_face();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
