//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "ml_c.h"


bool cveStatModelTrain(cv::ml::StatModel* model, cv::_InputArray* samples, int layout, cv::_InputArray* responses)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		return model->train(*samples, layout, *responses);
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}
bool cveStatModelTrainWithData(cv::ml::StatModel* model, cv::ml::TrainData* data, int flags)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		cv::Ptr<cv::ml::TrainData> p(data, [](cv::ml::TrainData*) {});
		return model->train(p, flags);
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}
float cveStatModelPredict(cv::ml::StatModel* model, cv::_InputArray* samples, cv::_OutputArray* results, int flags)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		return model->predict(*samples, results ? *results : static_cast<cv::OutputArray>(cv::noArray()), flags);
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

cv::ml::TrainData* cveTrainDataCreate(
	cv::_InputArray* samples, int layout, cv::_InputArray* responses,
	cv::_InputArray* varIdx, cv::_InputArray* sampleIdx,
	cv::_InputArray* sampleWeights, cv::_InputArray* varType,
	cv::Ptr<cv::ml::TrainData>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		cv::Ptr<cv::ml::TrainData> ptr = cv::ml::TrainData::create(
			*samples, layout, *responses,
			varIdx ? *varIdx : static_cast<cv::InputArray>(cv::noArray()),
			sampleIdx ? *sampleIdx : static_cast<cv::InputArray>(cv::noArray()),
			sampleWeights ? *sampleWeights : static_cast<cv::InputArray>(cv::noArray()),
			varType ? *varType : static_cast<cv::InputArray>(cv::noArray()));
		*sharedPtr = new cv::Ptr<cv::ml::TrainData>(ptr);
		return ptr.get();
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveTrainDataRelease(cv::Ptr<cv::ml::TrainData>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		delete *sharedPtr;
		*sharedPtr = 0;
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

//CvNormalBayesClassifier
cv::ml::NormalBayesClassifier* cveNormalBayesClassifierDefaultCreate(cv::ml::StatModel** statModel, cv::Algorithm** algorithm, cv::Ptr<cv::ml::NormalBayesClassifier>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		cv::Ptr<cv::ml::NormalBayesClassifier> ptr = cv::ml::NormalBayesClassifier::create();
		*sharedPtr = new cv::Ptr<cv::ml::NormalBayesClassifier>(ptr);
		cv::ml::NormalBayesClassifier* p = ptr.get();
		*statModel = dynamic_cast<cv::ml::StatModel*>(p);
		*algorithm = dynamic_cast<cv::Algorithm*> (p);
		return p;
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveNormalBayesClassifierRelease(cv::ml::NormalBayesClassifier** classifier, cv::Ptr<cv::ml::NormalBayesClassifier>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		delete *sharedPtr;
		*classifier = 0;
		*sharedPtr = 0;
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

//KNearest
cv::ml::KNearest* cveKNearestCreate(cv::ml::StatModel** statModel, cv::Algorithm** algorithm, cv::Ptr<cv::ml::KNearest>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		//cv::ml::KNearest::Params p(defaultK, isClassifier);
		cv::Ptr<cv::ml::KNearest> ptr = cv::ml::KNearest::create();
		*sharedPtr = new cv::Ptr<cv::ml::KNearest>(ptr);
		cv::ml::KNearest* r = ptr.get();
		*statModel = dynamic_cast<cv::ml::StatModel*>(r);
		*algorithm = dynamic_cast<cv::Algorithm*>(r);
		return r;
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveKNearestRelease(cv::Ptr<cv::ml::KNearest>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		delete *sharedPtr;
		*sharedPtr = 0;
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
float cveKNearestFindNearest(
	cv::ml::KNearest* classifier,
	cv::_InputArray* samples,
	int k,
	cv::_OutputArray* results,
	cv::_OutputArray* neighborResponses,
	cv::_OutputArray* dist)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		return classifier->findNearest(
			*samples,
			k,
			*results,
			neighborResponses ? *neighborResponses : static_cast<cv::OutputArray>(cv::noArray()),
			dist ? *dist : static_cast<cv::OutputArray>(cv::noArray()));
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

//EM

cv::ml::EM* cveEMDefaultCreate(cv::ml::StatModel** statModel, cv::Algorithm** algorithm, cv::Ptr<cv::ml::EM>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		cv::Ptr<cv::ml::EM> ptr = cv::ml::EM::create();
		*sharedPtr = new cv::Ptr<cv::ml::EM>(ptr);
		cv::ml::EM* em = ptr.get();
		*statModel = dynamic_cast<cv::ml::StatModel*>(em);
		*algorithm = dynamic_cast<cv::Algorithm*>(em);
		return em;
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveEMTrainE(
	cv::ml::EM* model,
	cv::_InputArray* samples,
	cv::_InputArray* means0,
	cv::_InputArray* covs0,
	cv::_InputArray* weights0,
	cv::_OutputArray* logLikelihoods,
	cv::_OutputArray* labels,
	cv::_OutputArray* probs,
	cv::ml::StatModel** statModel, cv::Algorithm** algorithm)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		model->trainE(
			*samples,
			*means0,
			covs0 ? *covs0 : static_cast<cv::InputArray>(cv::noArray()),
			weights0 ? *weights0 : static_cast<cv::InputArray>(cv::noArray()),
			logLikelihoods ? *logLikelihoods : static_cast<cv::OutputArray>(cv::noArray()),
			labels ? *labels : static_cast<cv::OutputArray>(cv::noArray()),
			probs ? *probs : static_cast<cv::OutputArray>(cv::noArray()));
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveEMTrainM(
	cv::ml::EM* model,
	cv::_InputArray* samples,
	cv::_InputArray* probs0,
	cv::_OutputArray* logLikelihoods,
	cv::_OutputArray* labels,
	cv::_OutputArray* probs,
	cv::ml::StatModel** statModel, cv::Algorithm** algorithm)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		model->trainM(
			*samples,
			*probs,
			logLikelihoods ? *logLikelihoods : static_cast<cv::OutputArray>(cv::noArray()),
			labels ? *labels : static_cast<cv::OutputArray>(cv::noArray()),
			probs ? *probs : static_cast<cv::OutputArray>(cv::noArray()));
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveEMPredict(cv::ml::EM* model, cv::_InputArray* sample, cv::Point2d* result, cv::_OutputArray* probs)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		cv::Vec2d vec = model->predict(*sample, probs ? *probs : static_cast<cv::OutputArray>(cv::noArray()));
		result->x = vec(0);
		result->y = vec(1);
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveEMRelease(cv::ml::EM** model, cv::Ptr<cv::ml::EM>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		delete *sharedPtr;
		*model = 0;
		*sharedPtr = 0;
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
//SVM
cv::ml::SVM* cveSVMDefaultCreate(cv::ml::StatModel** model, cv::Algorithm** algorithm, cv::Ptr<cv::ml::SVM>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		cv::Ptr<cv::ml::SVM> ptr = cv::ml::SVM::create();
		*sharedPtr = new cv::Ptr<cv::ml::SVM>(ptr);
		cv::ml::SVM* svm = ptr.get();
		*model = dynamic_cast<cv::ml::StatModel*>(svm);
		*algorithm = dynamic_cast<cv::Algorithm*>(svm);
		return svm;
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

bool cveSVMTrainAuto(
	cv::ml::SVM* model, cv::ml::TrainData* trainData, int kFold,
	cv::ml::ParamGrid* CGrid,
	cv::ml::ParamGrid* gammaGrid,
	cv::ml::ParamGrid* pGrid,
	cv::ml::ParamGrid* nuGrid,
	cv::ml::ParamGrid* coefGrid,
	cv::ml::ParamGrid* degreeGrid,
	bool balanced)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		cv::Ptr<cv::ml::TrainData> td(trainData, [](cv::ml::TrainData*){});
		return model->trainAuto(
			td, kFold,
			*CGrid, *gammaGrid, *pGrid, *nuGrid, *coefGrid, *degreeGrid,
			balanced);
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

void cveSVMGetDefaultGrid(int gridType, cv::ml::ParamGrid* grid)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		cv::ml::ParamGrid defaultGrid = cv::ml::SVM::getDefaultGrid(gridType);
		memcpy(grid, &defaultGrid, sizeof(cv::ml::ParamGrid));
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveSVMRelease(cv::ml::SVM** model, cv::Ptr<cv::ml::SVM>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		delete *sharedPtr;
		*model = 0;
		*sharedPtr = 0;
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

CVAPI(void) cveSVMGetSupportVectors(cv::ml::SVM* model, cv::Mat* supportVectors)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		model->getSupportVectors().copyTo(*supportVectors);
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

//ANN_MLP
//cv::ml::ANN_MLP::Params* CvANN_MLPParamsCreate(
//   cv::Mat* layerSizes, int activateFunc, double fparam1, double fparam2,
//   CvTermCriteria* termCrit, int trainMethod, double param1, double param2)
//{
//   return new cv::ml::ANN_MLP::Params(*layerSizes, activateFunc, fparam1, fparam2, *termCrit, trainMethod, param1, param2);
//}
//void CvANN_MLPParamsRelease(cv::ml::ANN_MLP::Params** p)
//{
//   delete *p;
//   *p = 0;
//}
cv::ml::ANN_MLP* cveANN_MLPCreate(cv::ml::StatModel** model, cv::Algorithm** algorithm, cv::Ptr<cv::ml::ANN_MLP>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		cv::Ptr<cv::ml::ANN_MLP> ptr = cv::ml::ANN_MLP::create();
		*sharedPtr = new cv::Ptr<cv::ml::ANN_MLP>(ptr);
		cv::ml::ANN_MLP* r = ptr.get();
		*model = dynamic_cast<cv::ml::StatModel*>(r);
		*algorithm = dynamic_cast<cv::Algorithm*>(r);
		return r;
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveANN_MLPSetLayerSizes(cv::ml::ANN_MLP* model, cv::_InputArray* layerSizes)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		model->setLayerSizes(*layerSizes);
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveANN_MLPSetTrainMethod(cv::ml::ANN_MLP* model, int method, double param1, double param2)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		model->setTrainMethod(method, param1, param2);
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveANN_MLPSetActivationFunction(cv::ml::ANN_MLP* model, int type, double param1, double param2)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		model->setActivationFunction(type, param1, param2);
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveANN_MLPRelease(cv::ml::ANN_MLP** model, cv::Ptr<cv::ml::ANN_MLP>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		delete *sharedPtr;
		*model = 0;
		*sharedPtr = 0;
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

//Decision Tree
cv::ml::DTrees* cveDTreesCreate(cv::ml::StatModel** statModel, cv::Algorithm** algorithm, cv::Ptr<cv::ml::DTrees>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		cv::Ptr<cv::ml::DTrees> ptr = cv::ml::DTrees::create();
		*sharedPtr = new cv::Ptr<cv::ml::DTrees>(ptr);
		cv::ml::DTrees* dtree = ptr.get();
		*statModel = dynamic_cast<cv::ml::StatModel*>(dtree);
		*algorithm = dynamic_cast<cv::Algorithm*>(dtree);
		return dtree;
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveDTreesRelease(cv::ml::DTrees** model, cv::Ptr<cv::ml::DTrees>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		delete *sharedPtr;
		*model = 0;
		*sharedPtr = 0;
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

//Random Tree

cv::ml::RTrees* cveRTreesCreate(cv::ml::StatModel** statModel, cv::Algorithm** algorithm, cv::Ptr<cv::ml::RTrees>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		cv::Ptr<cv::ml::RTrees> ptr = cv::ml::RTrees::create();
		*sharedPtr = new cv::Ptr<cv::ml::RTrees>(ptr);
		cv::ml::RTrees* rtrees = ptr.get();
		*statModel = dynamic_cast<cv::ml::StatModel*>(rtrees);
		*algorithm = dynamic_cast<cv::Algorithm*>(rtrees);
		return rtrees;
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveRTreesGetVotes(cv::ml::RTrees* model, cv::_InputArray* samples, cv::_OutputArray* results, int flags)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		model->getVotes(*samples, *results, flags);
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveRTreesRelease(cv::ml::RTrees** model, cv::Ptr<cv::ml::RTrees>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		delete *sharedPtr;
		*model = 0;
		*sharedPtr = 0;
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::ml::Boost* cveBoostCreate(cv::ml::StatModel** statModel, cv::Algorithm** algorithm, cv::Ptr<cv::ml::Boost>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		cv::Ptr<cv::ml::Boost> ptr = cv::ml::Boost::create();
		*sharedPtr = new cv::Ptr<cv::ml::Boost>(ptr);
		cv::ml::Boost* boost = ptr.get();
		*statModel = dynamic_cast<cv::ml::StatModel*>(boost);
		*algorithm = dynamic_cast<cv::Algorithm*>(boost);
		return boost;
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveBoostRelease(cv::ml::Boost** model, cv::Ptr<cv::ml::Boost>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		delete *model;
		*model = 0;
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
//LogisticRegression
cv::ml::LogisticRegression* cveLogisticRegressionCreate(cv::ml::StatModel** statModel, cv::Algorithm** algorithm, cv::Ptr<cv::ml::LogisticRegression>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		cv::Ptr<cv::ml::LogisticRegression> ptr = cv::ml::LogisticRegression::create();
		*sharedPtr = new cv::Ptr<cv::ml::LogisticRegression>(ptr);
		cv::ml::LogisticRegression* model = ptr.get();
		*statModel = dynamic_cast<cv::ml::StatModel*>(model);
		*algorithm = dynamic_cast<cv::Algorithm*>(model);
		return model;
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveLogisticRegressionRelease(cv::ml::LogisticRegression** model, cv::Ptr<cv::ml::LogisticRegression>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		delete *sharedPtr;
		*model = 0;
		*sharedPtr = 0;
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

//SVMSGD
cv::ml::SVMSGD* cveSVMSGDDefaultCreate(cv::ml::StatModel** statModel, cv::Algorithm** algorithm, cv::Ptr<cv::ml::SVMSGD>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		cv::Ptr<cv::ml::SVMSGD> ptr = cv::ml::SVMSGD::create();
		*sharedPtr = new cv::Ptr<cv::ml::SVMSGD>(ptr);
		cv::ml::SVMSGD* model = ptr.get();
		*statModel = dynamic_cast<cv::ml::StatModel*>(model);
		*algorithm = dynamic_cast<cv::Algorithm*>(model);
		return model;
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveSVMSGDRelease(cv::ml::SVMSGD** model, cv::Ptr<cv::ml::SVMSGD>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		delete *sharedPtr;
		*model = 0;
		*sharedPtr = 0;
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveSVMSGDSetOptimalParameters(cv::ml::SVMSGD* model, int svmsgdType, int marginType)
{
	try
	{
	#ifdef HAVE_OPENCV_ML
		model->setOptimalParameters(svmsgdType, marginType);
	#else
		throw_no_ml();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
