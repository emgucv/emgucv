//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_FACE_C_H
#define EMGU_FACE_C_H

#include "opencv2/core/core_c.h"

#ifdef HAVE_OPENCV_FACE
#include "opencv2/face/bif.hpp"
#include "opencv2/face.hpp"
#else
static inline CV_NORETURN void throw_no_face() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without face support"); }
namespace cv {
	namespace face {
		class FaceRecognizer {};
		class BasicFaceRecognizer {};
		class EigenFaceRecognizer {};
		class FisherFaceRecognizer {};
		class LBPHFaceRecognizer {};
		class BIF {};
		class Facemark {};
		class FacemarkAAM {
		public:
			struct  Params {};
		};
		class FacemarkLBF {
		public:
			struct  Params {};
		};
		class MACE {};
	}
}
#endif

//EigenFaceRecognizer
CVAPI(cv::face::EigenFaceRecognizer*) cveEigenFaceRecognizerCreate(
	int numComponents,
	double threshold,
	cv::face::FaceRecognizer** faceRecognizerPtr,
	cv::face::BasicFaceRecognizer** basicFaceRecognizerPtr,
	cv::Ptr<cv::face::EigenFaceRecognizer>** sharedPtr);
CVAPI(void) cveEigenFaceRecognizerRelease(cv::Ptr<cv::face::EigenFaceRecognizer>** sharedPtr);

CVAPI(cv::face::FisherFaceRecognizer*) cveFisherFaceRecognizerCreate(
	int numComponents,
	double threshold,
	cv::face::FaceRecognizer** faceRecognizerPtr,
	cv::face::FaceRecognizer** basicFaceRecognizerPtr,
	cv::Ptr<cv::face::FisherFaceRecognizer>** sharedPtr);
CVAPI(void) cveFisherFaceRecognizerRelease(cv::Ptr<cv::face::FisherFaceRecognizer>** sharedPtr);

CVAPI(cv::face::LBPHFaceRecognizer*) cveLBPHFaceRecognizerCreate(
	int radius,
	int neighbors,
	int gridX,
	int gridY,
	double threshold,
	cv::face::FaceRecognizer** faceRecognizerPtr,
	cv::Ptr<cv::face::LBPHFaceRecognizer>** sharedPtr);
CVAPI(void) cveLBPHFaceRecognizerRelease(cv::Ptr<cv::face::LBPHFaceRecognizer>** sharedPtr);
CVAPI(void) cveLBPHFaceRecognizerGetHistograms(cv::face::LBPHFaceRecognizer* recognizer, std::vector<cv::Mat>* histograms);

CVAPI(void) cveFaceRecognizerTrain(cv::face::FaceRecognizer* recognizer, cv::_InputArray* images, cv::_InputArray* labels);
CVAPI(void) cveFaceRecognizerUpdate(cv::face::FaceRecognizer* recognizer, cv::_InputArray* images, cv::_InputArray* labels);
CVAPI(void) cveFaceRecognizerPredict(cv::face::FaceRecognizer* recognizer, cv::_InputArray* image, int* label, double* distance);
CVAPI(void) cveFaceRecognizerWrite(cv::face::FaceRecognizer* recognizer, cv::String* fileName);
CVAPI(void) cveFaceRecognizerRead(cv::face::FaceRecognizer* recognizer, cv::String* fileName);


CVAPI(cv::face::BIF*) cveBIFCreate(int numBands, int numRotations, cv::Ptr<cv::face::BIF>** sharedPtr);
CVAPI(void) cveBIFCompute(cv::face::BIF* bif, cv::_InputArray* image, cv::_OutputArray* features);
CVAPI(void) cveBIFRelease(cv::Ptr<cv::face::BIF>** sharedPtr);


CVAPI(cv::face::FacemarkAAM::Params*) cveFacemarkAAMParamsCreate();
CVAPI(void) cveFacemarkAAMParamsRelease(cv::face::FacemarkAAM::Params** params);

CVAPI(cv::face::FacemarkAAM*) cveFacemarkAAMCreate(cv::face::FacemarkAAM::Params* parameters, cv::face::Facemark** facemark, cv::Algorithm** algorithm, cv::Ptr<cv::face::FacemarkAAM>** sharedPtr);
CVAPI(void) cveFacemarkAAMRelease(cv::face::FacemarkAAM** facemark, cv::Ptr<cv::face::FacemarkAAM>** sharedPtr);

CVAPI(cv::face::FacemarkLBF::Params*) cveFacemarkLBFParamsCreate();
CVAPI(void) cveFacemarkLBFParamsRelease(cv::face::FacemarkLBF::Params** params);

CVAPI(cv::face::FacemarkLBF*) cveFacemarkLBFCreate(cv::face::FacemarkLBF::Params* parameters, cv::face::Facemark** facemark, cv::Algorithm** algorithm, cv::Ptr<cv::face::FacemarkLBF>** sharedPtr);
CVAPI(void) cveFacemarkLBFRelease(cv::face::FacemarkLBF** facemark, cv::Ptr<cv::face::FacemarkLBF>** sharedPtr);

/*
CVAPI(cv::face::FacemarkKazemi::Params*) cveFacemarkKazemiParamsCreate();
CVAPI(void) cveFacemarkKazemiParamsRelease(cv::face::FacemarkKazemi::Params** params);

CVAPI(cv::face::FacemarkKazemi*) cveFacemarkKazemiCreate(cv::face::FacemarkKazemi::Params* parameters, cv::face::Facemark** facemark, cv::Algorithm** algorithm);
CVAPI(void) cveFacemarkKazemiRelease(cv::face::FacemarkKazemi** facemark);
*/

/*
typedef bool(*CSharp_FaceDetector)(const cv::_InputArray*, const cv::_OutputArray*);
CVAPI(bool) cveFacemarkSetFaceDetector(cv::face::Facemark* facemark, CSharp_FaceDetector detector);
*/
CVAPI(void) cveFacemarkLoadModel(cv::face::Facemark* facemark, cv::String* model);
//CVAPI(bool) cveFacemarkGetFaces(cv::face::Facemark* facemark, cv::_InputArray* image, cv::_OutputArray* faces);
CVAPI(bool) cveFacemarkFit(cv::face::Facemark* facemark, cv::_InputArray* image, cv::_InputArray* faces, cv::_InputOutputArray* landmarks);

//CVAPI(bool) cveFacemarkAddTrainingSample(cv::face::Facemark* facemark, cv::_InputArray* image, cv::_InputArray* landmarks);
//CVAPI(void) cveFacemarkTraining(cv::face::Facemark* facemark);

CVAPI(void) cveDrawFacemarks(cv::_InputOutputArray* image, cv::_InputArray* points, CvScalar* color);

CVAPI(cv::face::MACE*) cveMaceCreate(int imgSize, cv::Ptr<cv::face::MACE>** sharedPtr);
CVAPI(void) cveMaceSalt(cv::face::MACE* mace, cv::String* passphrase);
CVAPI(void) cveMaceTrain(cv::face::MACE* mace, cv::_InputArray* images);
CVAPI(bool) cveMaceSame(cv::face::MACE* mace, cv::_InputArray* query);
CVAPI(void) cveMaceRelease(cv::Ptr<cv::face::MACE>** sharedPtr);
#endif