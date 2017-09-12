//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "ml_c.h"

//StatModel
void StatModelSave(cv::ml::StatModel* model, cv::String* filename) 
{ 
   model->save(*filename); 
}
/*
void StatModelLoad(cv::ml::StatModel* model, cv::String* filename) 
{ 
   model->load(*filename); 
}*/
void StatModelClear(cv::ml::StatModel* model) 
{ 
   model->clear(); 
}
bool StatModelTrain(cv::ml::StatModel* model, cv::_InputArray* samples, int layout, cv::_InputArray* responses )
{
   return model->train(*samples, layout, *responses);
}
bool StatModelTrainWithData(cv::ml::StatModel* model, cv::ml::TrainData* data, int flags)
{
   cv::Ptr<cv::ml::TrainData> p(data);
   p.addref();
   return model->train(p, flags);
}
float StatModelPredict(cv::ml::StatModel* model, cv::_InputArray* samples, cv::_OutputArray* results, int flags)
{
   return model->predict(*samples, results ? *results : (cv::OutputArray) cv::noArray(), flags);
}

cv::ml::TrainData* cveTrainDataCreate(
   cv::_InputArray* samples, int layout, cv::_InputArray* responses,
   cv::_InputArray* varIdx, cv::_InputArray* sampleIdx,
   cv::_InputArray* sampleWeights, cv::_InputArray* varType)
{
   cv::Ptr<cv::ml::TrainData> ptr = cv::ml::TrainData::create(
      *samples, layout, *responses, 
      varIdx ? *varIdx : (cv::InputArray) cv::noArray(),
      sampleIdx ? *sampleIdx : (cv::InputArray) cv::noArray(),
      sampleWeights ? *sampleWeights : (cv::InputArray) cv::noArray(),
      varType ? *varType : (cv::InputArray) cv::noArray());
   ptr.addref();
   return ptr.get();
}

void cveTrainDataRelease(cv::ml::TrainData** data)
{
   delete *data;
   *data = 0;
}

//CvNormalBayesClassifier
cv::ml::NormalBayesClassifier* cveNormalBayesClassifierDefaultCreate(cv::ml::StatModel** statModel, cv::Algorithm** algorithm) 
{ 
   cv::Ptr<cv::ml::NormalBayesClassifier> ptr = cv::ml::NormalBayesClassifier::create();
   ptr.addref();
   cv::ml::NormalBayesClassifier* p = ptr.get();
   *statModel = dynamic_cast< cv::ml::StatModel* >( p );
   *algorithm = dynamic_cast< cv::Algorithm* > (p);
   return p;
}
/*
cv::ml::NormalBayesClassifier* CvNormalBayesClassifierCreate( CvMat* _train_data, CvMat* _responses, CvMat* _var_idx, CvMat* _sample_idx )
{ 
   return new cv::ml::NormalBayesClassifier(_train_data, _responses, _var_idx, _sample_idx); 
}*/
void cveNormalBayesClassifierRelease(cv::ml::NormalBayesClassifier** classifier) 
{ 
   delete *classifier; 
   *classifier = 0; 
}
/*
bool CvNormalBayesClassifierTrain(cv::ml::NormalBayesClassifier* classifier, CvMat* _train_data, CvMat* _responses,
   CvMat* _var_idx, CvMat* _sample_idx, bool update )
{ return classifier->train(_train_data, _responses, _var_idx, _sample_idx, update); }
float CvNormalBayesClassifierPredict(cv::ml::NormalBayesClassifier* classifier, CvMat* _samples, CvMat* results )
{ return classifier->predict(_samples, results); }
*/
//KNearest
cv::ml::KNearest* cveKNearestCreate(cv::ml::StatModel** statModel, cv::Algorithm** algorithm) 
{ 
   //cv::ml::KNearest::Params p(defaultK, isClassifier);
   cv::Ptr<cv::ml::KNearest> ptr = cv::ml::KNearest::create();
   ptr.addref();
   cv::ml::KNearest* r = ptr.get();
   *statModel = dynamic_cast<cv::ml::StatModel*>( r );
   *algorithm = dynamic_cast< cv::Algorithm* >( r );
   return r;
}
void cveKNearestRelease(cv::ml::KNearest** classifier) 
{ 
   delete *classifier; 
   *classifier = 0; 
}
/*
bool CvKNearestTrain(CvKNearest* classifier, CvMat* _train_data, CvMat* _responses,
   CvMat* _sample_idx, bool is_regression,
   int _max_k, bool _update_base)
{ return classifier->train(_train_data, _responses, _sample_idx, is_regression, _max_k, _update_base); }
CvKNearest* CvKNearestCreate(CvMat* _train_data, CvMat* _responses,
   CvMat* _sample_idx, bool _is_regression, int max_k )
{ return new CvKNearest(_train_data, _responses, _sample_idx, _is_regression, max_k); }
float CvKNearestFindNearest(CvKNearest* classifier, CvMat* _samples, int k, CvMat* results,
   float** neighbors, CvMat* neighbor_responses, CvMat* dist )
{ return classifier->find_nearest(_samples, k, results, (const float**) neighbors, neighbor_responses, dist); }
*/
//EM

cv::ml::EM* cveEMDefaultCreate(cv::ml::StatModel** statModel, cv::Algorithm** algorithm)
{ 
   cv::Ptr<cv::ml::EM> ptr = cv::ml::EM::create();
   ptr.addref();
   cv::ml::EM* em = ptr.get();
   *statModel = dynamic_cast<cv::ml::StatModel*>( em );
   *algorithm = dynamic_cast<cv::Algorithm*>( em );
   return em;
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
   model->trainE(
      *samples, 
      *means0, 
      covs0 ? *covs0 : (cv::InputArray) cv::noArray(),
      weights0 ? *weights0 : (cv::InputArray) cv::noArray(),
      logLikelihoods ? *logLikelihoods : (cv::OutputArray) cv::noArray(),
      labels ? *labels : (cv::OutputArray) cv::noArray(),
      probs ? *probs : (cv::OutputArray) cv::noArray());
   
   
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
   model->trainM(
      *samples, 
      *probs,
      logLikelihoods ? *logLikelihoods : (cv::OutputArray) cv::noArray(),
      labels ? *labels : (cv::OutputArray) cv::noArray(),
      probs ? *probs : (cv::OutputArray) cv::noArray());
}
void cveEMPredict(cv::ml::EM* model, cv::_InputArray* sample, CvPoint2D64f* result, cv::_OutputArray* probs)  
{ 
   cv::Vec2d vec = model->predict(*sample, probs ? *probs : (cv::OutputArray) cv::noArray());
   result->x = vec(0);
   result->y = vec(1);
}
void cveEMRelease(cv::ml::EM** model) 
{ 
   delete *model;  
   *model = 0;
}
/*
bool CvEMTrain(cv::EM* model, cv::_InputArray* samples, cv::_OutputArray* logLikelihoods, cv::_OutputArray* labels, cv::_OutputArray* probs)
{
   return model->train(
      *samples, 
      logLikelihoods? *logLikelihoods : (cv::OutputArray) cv::noArray(),
      labels ? *labels : (cv::OutputArray) cv::noArray(), 
      probs ? *probs : (cv::OutputArray) cv::noArray() );
}

*/

//SVM
/*
cv::ml::SVM::Params* CvSVMParamsCreate(
   int svmType, int kernelType, double degree, double gamma, double coef0,
   double con, double nu, double p, cv::Mat* classWeights, CvTermCriteria* termCrit)
{
   return new cv::ml::SVM::Params(svmType, kernelType, degree, gamma, coef0, con, nu, p, classWeights ? *classWeights : cv::Mat(), *termCrit );
}
void CvSVMParamsRelease(cv::ml::SVM::Params** p)
{
   delete *p;
   *p = 0;
}*/

cv::ml::SVM* cveSVMDefaultCreate(cv::ml::StatModel** model, cv::Algorithm** algorithm) 
{ 
   cv::Ptr<cv::ml::SVM> ptr = cv::ml::SVM::create();
   ptr.addref();
   cv::ml::SVM* svm = ptr.get();
   *model = dynamic_cast<cv::ml::StatModel*>( svm );
   *algorithm = dynamic_cast<cv::Algorithm*>( svm );
   return svm;
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
   cv::Ptr<cv::ml::TrainData> td(trainData);
   td.addref();
   return model->trainAuto(
      td, kFold,
      *CGrid, *gammaGrid, *pGrid, *nuGrid, *coefGrid, *degreeGrid,
      balanced); 
}

void cveSVMGetDefaultGrid(int gridType, cv::ml::ParamGrid* grid)
{  
   cv::ml::ParamGrid defaultGrid = cv::ml::SVM::getDefaultGrid(gridType);
   memcpy(grid, &defaultGrid, sizeof(cv::ml::ParamGrid));
}
void cveSVMRelease(cv::ml::SVM** model) 
{ 
   delete *model; 
   *model = 0; 
}

CVAPI(void) cveSVMGetSupportVectors(cv::ml::SVM* model, cv::Mat* supportVectors)
{
   model->getSupportVectors().copyTo(*supportVectors);
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
cv::ml::ANN_MLP* cveANN_MLPCreate(cv::ml::StatModel** model, cv::Algorithm** algorithm)
{ 
   cv::Ptr<cv::ml::ANN_MLP> ptr = cv::ml::ANN_MLP::create();
   ptr.addref();
   cv::ml::ANN_MLP* r = ptr.get();
   *model = dynamic_cast<cv::ml::StatModel*>( r );
   *algorithm = dynamic_cast<cv::Algorithm*>( r );
   return r;
}


void cveANN_MLPSetLayerSizes(cv::ml::ANN_MLP* model, cv::_InputArray* layerSizes)
{
   model->setLayerSizes(*layerSizes);
}

void cveANN_MLPSetTrainMethod(cv::ml::ANN_MLP* model, int method, double param1, double param2)
{
   model->setTrainMethod(method, param1, param2);
}

void cveANN_MLPSetActivationFunction(cv::ml::ANN_MLP* model, int type, double param1, double param2)
{
   model->setActivationFunction(type, param1, param2);
}
void cveANN_MLPRelease(cv::ml::ANN_MLP** model)
{
   delete *model;
   *model = 0;
}

//Decision Tree
cv::ml::DTrees* cveDTreesCreate(cv::ml::StatModel** statModel, cv::Algorithm** algorithm) 
{ 
   cv::Ptr<cv::ml::DTrees> ptr = cv::ml::DTrees::create();
   ptr.addref();
   cv::ml::DTrees* dtree = ptr.get();
   *statModel = dynamic_cast<cv::ml::StatModel*>( dtree );
   *algorithm = dynamic_cast<cv::Algorithm*>( dtree );
   return dtree;
}
void cveDTreesRelease(cv::ml::DTrees** model) 
{ 
   delete *model; 
   *model = 0;
}

//Random Tree

cv::ml::RTrees* cveRTreesCreate(cv::ml::StatModel** statModel, cv::Algorithm** algorithm) 
{ 
   cv::Ptr<cv::ml::RTrees> ptr = cv::ml::RTrees::create();
   ptr.addref();
   cv::ml::RTrees* rtrees = ptr.get();
   *statModel = dynamic_cast<cv::ml::StatModel*>( rtrees );
   *algorithm = dynamic_cast<cv::Algorithm*>( rtrees );
   return rtrees;
}
void cveRTreesRelease(cv::ml::RTrees** model) 
{ 
   delete *model; 
   *model = 0; 
}

/*
int CvRTreesGetTreeCount(CvRTrees* model) { return model->get_tree_count(); }
CvMat* CvRTreesGetVarImportance(CvRTrees* model) { return (CvMat*) model->get_var_importance(); }
*/
//Extreme Random Tree
//CvERTrees* CvERTreesCreate() { return new CvERTrees(); }
//void CvERTreesRelease(CvERTrees** model) { delete *model; *model = 0; }

//CvBoost
//cv::ml::Boost::Params* CvBoostParamsCreate(
//   int boostType, int weakCount, double weightTrimRate,
//   int maxDepth, bool useSurrogates, cv::Mat* priors)
//{ 
//   return new cv::ml::Boost::Params(
//      boostType, weakCount, weightTrimRate,
//      maxDepth, useSurrogates, 
//      priors ? *priors : cv::Mat()); 
//}
//void CvBoostParamsRelease(cv::ml::Boost::Params** params) 
//{ 
//   delete *params; 
//   *params = 0; 
//}

cv::ml::Boost* cveBoostCreate(cv::ml::StatModel** statModel, cv::Algorithm** algorithm) 
{ 
   cv::Ptr<cv::ml::Boost> ptr = cv::ml::Boost::create();
   ptr.addref();
   cv::ml::Boost* boost = ptr.get();
   *statModel = dynamic_cast<cv::ml::StatModel*>( boost );
   *algorithm = dynamic_cast<cv::Algorithm*>( boost );
   return boost;
}
void cveBoostRelease(cv::ml::Boost** model) 
{ 
   delete *model; 
   *model = 0; 
}
/*
bool CvBoostTrain(CvBoost* model, CvMat* _train_data, int _tflag,
   CvMat* _responses, CvMat* _var_idx,
   CvMat* _sample_idx, CvMat* _var_type,
   CvMat* _missing_mask,
   CvBoostParams* params,
   bool update )
{ return model->train(_train_data, _tflag, _responses, _var_idx,
_sample_idx, _var_type, _missing_mask, *params, update); }

float CvBoostPredict(CvBoost* model, CvMat* _sample, CvMat* _missing,
   CvMat* weak_responses, CvSlice* slice,
   bool raw_mode)
{ return model->predict(_sample, _missing, weak_responses, *slice, raw_mode); }
*/

//CvGBTrees
/*
void CvGBTreesParamsGetDefault(CvGBTreesParams* params)
{
   CvGBTreesParams p;
   memcpy(params, &p, sizeof(CvGBTreesParams));
}
CvGBTrees* CvGBTreesCreate()
{
   return new CvGBTrees();
}
void CvGBTreesRelease( CvGBTrees** model)
{
   delete * model;
   *model = 0;
}

bool CvGBTreesTrain(CvGBTrees* model, const CvMat* trainData, int tflag,
   const CvMat* responses, const CvMat* varIdx,
   const CvMat* sampleIdx, const CvMat* varType,
   const CvMat* missingDataMask,
   CvGBTreesParams* params,
   bool update)
{
   return model->train(trainData, tflag, responses, varIdx, sampleIdx, varType, missingDataMask, *params, update);
}
float CvGBTreesPredict(CvGBTrees* model, CvMat* _sample, CvMat* _missing,
   CvMat* weak_responses, CvSlice* slice,
   bool raw_mode)
{
   return model->predict(_sample, _missing, weak_responses, *slice, raw_mode);
}
*/



//LogisticRegression
/*
cv::ml::LogisticRegression::Params* cveLogisticRegressionParamsCreate(
   double learning_rate,
   int iters,
   int method,
   int normalization,
   int reg,
   int batch_size)
{
   return new cv::ml::LogisticRegression::Params(learning_rate, iters, method, normalization, reg, batch_size);
}
void cveLogisticRegressionParamsRelease(cv::ml::LogisticRegression::Params** params)
{
   delete *params;
   *params = 0;
}
*/
cv::ml::LogisticRegression* cveLogisticRegressionCreate(cv::ml::StatModel** statModel, cv::Algorithm** algorithm)
{
   cv::Ptr<cv::ml::LogisticRegression> ptr = cv::ml::LogisticRegression::create();
   ptr.addref();
   cv::ml::LogisticRegression* model = ptr.get();
   *statModel = dynamic_cast<cv::ml::StatModel*>(model);
   *algorithm = dynamic_cast<cv::Algorithm*>(model);
   return model;

}

void cveLogisticRegressionRelease(cv::ml::LogisticRegression** model)
{
   delete *model;
   *model = 0;
}

//SVMSGD
cv::ml::SVMSGD* cveSVMSGDDefaultCreate(cv::ml::StatModel** statModel, cv::Algorithm** algorithm)
{
	cv::Ptr<cv::ml::SVMSGD> ptr = cv::ml::SVMSGD::create();
	ptr.addref();
	cv::ml::SVMSGD* model = ptr.get();
	*statModel = dynamic_cast<cv::ml::StatModel*>(model);
	*algorithm = dynamic_cast<cv::Algorithm*>(model);
	return model;
}

void cveSVMSGDRelease(cv::ml::SVMSGD** model)
{
	delete *model;
	*model = 0;
}

void cveSVMSGDSetOptimalParameters(cv::ml::SVMSGD* model, int svmsgdType, int marginType)
{
	model->setOptimalParameters(svmsgdType, marginType);
}