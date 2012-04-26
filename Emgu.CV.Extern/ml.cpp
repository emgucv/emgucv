//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "ml_c.h"

//StatModel
void StatModelSave(CvStatModel* model, char* filename, char* name) { model->save(filename, name); }
void StatModelLoad(CvStatModel* model, char* filename, char* name) { model->load(filename, name); }
void StatModelClear(CvStatModel* model) { model->clear(); }

//CvNormalBayesClassifier
CvNormalBayesClassifier* CvNormalBayesClassifierDefaultCreate() { return new CvNormalBayesClassifier; }
CvNormalBayesClassifier* CvNormalBayesClassifierCreate( CvMat* _train_data, CvMat* _responses, CvMat* _var_idx, CvMat* _sample_idx )
{ return new CvNormalBayesClassifier(_train_data, _responses, _var_idx, _sample_idx); }
void CvNormalBayesClassifierRelease(CvNormalBayesClassifier** classifier) { delete *classifier; *classifier = 0; }
bool CvNormalBayesClassifierTrain(CvNormalBayesClassifier* classifier, CvMat* _train_data, CvMat* _responses,
   CvMat* _var_idx, CvMat* _sample_idx, bool update )
{ return classifier->train(_train_data, _responses, _var_idx, _sample_idx, update); }
float CvNormalBayesClassifierPredict(CvNormalBayesClassifier* classifier, CvMat* _samples, CvMat* results )
{ return classifier->predict(_samples, results); }

//KNearest
CvKNearest* CvKNearestDefaultCreate() { return new CvKNearest; }
void CvKNearestRelease(CvKNearest** classifier) { delete *classifier; *classifier = 0; }
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

//EM
CVAPI(cv::EM*) CvEMDefaultCreate(int nclusters, int covMatType, const cv::TermCriteria termcrit)
{ 
   return new cv::EM(nclusters, covMatType, termcrit); 
}
void CvEMRelease(cv::EM** model) 
{ 
   delete *model;  
   *model = 0;
}
bool CvEMTrain(cv::EM* model, CvMat* samples, CvMat* labels, CvMat* probs, CvMat* logLikelihoods )
{
   cv::Mat samplesMat = cv::cvarrToMat(samples);
   cv::Mat labelsMat = labels ? cv::cvarrToMat(labels) : cv::Mat();
   cv::Mat probsMat = probs? cv::cvarrToMat(probs) : cv::Mat();
   cv::Mat logLikelihoodsMat = logLikelihoods ? cv::cvarrToMat(logLikelihoods) : cv::Mat();

   cv::Size labelsSize = labels ? labelsMat.size() : cv::Size();
   cv::Size probsSize = probs ? probsMat.size() : cv::Size();
   cv::Size logLikelihoodsSize = logLikelihoods ? logLikelihoodsMat.size() : cv::Size();

   bool result = model->train(
      samplesMat, 
      labels ? labelsMat : cv::noArray(), 
      probs ? probsMat: cv::noArray(), 
      logLikelihoods? logLikelihoodsMat : cv::noArray());

   CV_Assert(!labels || (labelsSize.width == labelsMat.size().width && labelsSize.height == labelsMat.size().height));
   CV_Assert(!probs || (probsSize.width == probsMat.size().width && probsSize.height == probsMat.size().height));
   CV_Assert(!logLikelihoods || (logLikelihoodsSize.width == logLikelihoodsMat.size().width && logLikelihoodsSize.height == logLikelihoodsMat.size().height));
   return result;
}

int CvEMPredict(cv::EM* model, CvMat* sample, CvMat* probs )  
{ 
   cv::Mat sampleMat = cv::cvarrToMat(sample);
   cv::Mat probsMat = probs? cv::cvarrToMat(probs) : cv::Mat(); 
   return model->predict(sampleMat, probs ? probsMat : cv::noArray()); 
}

//EMLegacy
CvEM* CvEMLegacyDefaultCreate() { return new CvEM; }
void CvEMLegacyRelease(CvEM** model) { delete *model; *model = 0;}
bool CvEMLegacyTrain(CvEM* model, CvMat* samples, CvMat* sample_idx,
   CvEMParams params, CvMat* labels )
{ return model->train(samples, sample_idx, params, labels); }
float CvEMLegacyPredict(CvEM* model, CvMat* sample, CvMat* probs )
{ return model->predict(sample, probs); }
int CvEMLegacyGetNclusters(CvEM* model) { return model->get_nclusters(); }
CvMat* CvEMLegacyGetMeans(CvEM* model) { return (CvMat*) model->get_means(); }
CvMat** CvEMLegacyGetCovs(CvEM* model) { return (CvMat**) model->get_covs(); }
CvMat* CvEMLegacyGetWeights(CvEM* model) { return (CvMat*) model->get_weights(); }
CvMat* CvEMLegacyGetProbs(CvEM* model) { return (CvMat*) model->get_probs(); }

//SVM
CvSVM* CvSVMDefaultCreate() { return new CvSVM; }
bool CvSVMTrain(CvSVM* model, CvMat* _train_data, CvMat* _responses,
   CvMat* _var_idx, CvMat* _sample_idx,
   CvSVMParams _params)
{ return model->train(_train_data, _responses, _var_idx, _sample_idx, _params); }
bool CvSVMTrainAuto(CvSVM* model, CvMat* _train_data, CvMat* _responses,
   CvMat* _var_idx, CvMat* _sample_idx, CvSVMParams _params,
   int k_fold,
   CvParamGrid C_grid,
   CvParamGrid gamma_grid,
   CvParamGrid p_grid,
   CvParamGrid nu_grid,
   CvParamGrid coef_grid,
   CvParamGrid degree_grid)
{ return model->train_auto(_train_data, _responses, _var_idx, _sample_idx, _params, k_fold,
C_grid, gamma_grid, p_grid, nu_grid, coef_grid, degree_grid); }
void CvSVMGetDefaultGrid(int gridType, CvParamGrid* grid)
{  CvParamGrid defaultGrid = CvSVM::get_default_grid(gridType);
grid->max_val = defaultGrid.max_val;
grid->min_val = defaultGrid.min_val;
grid->step = defaultGrid.step;
}
void CvSVMRelease(CvSVM** model) { delete *model; *model = 0; }
float CvSVMPredict(CvSVM* model,  CvMat* _sample )
{ return model->predict(_sample); }
float* CvSVMGetSupportVector(CvSVM* model, int i)
{ return (float*) model->get_support_vector(i); }
int CvSVMGetSupportVectorCount(CvSVM* model)
{ return model->get_support_vector_count(); }
int CvSVMGetVarCount(CvSVM* model)
{ return model->get_var_count(); }
void CvSVMGetParameters(CvSVM* model, CvSVMParams* param) { CvSVMParams p = model->get_params(); memcpy(param, &p, sizeof(CvSVMParams)); }

//ANN_MLP
CvANN_MLP* CvANN_MLPCreate(CvMat* _layer_sizes,
   int _activ_func,
   double _f_param1, double _f_param2 )
{ return new CvANN_MLP(_layer_sizes, _activ_func, _f_param1, _f_param2); }
void CvANN_MLPRelease(CvANN_MLP** model) { delete *model; *model = 0; }
int CvANN_MLPTrain(CvANN_MLP* model, CvMat* _inputs, CvMat* _outputs,
   CvMat* _sample_weights, CvMat* _sample_idx,
   CvANN_MLP_TrainParams* _params,
   int flags)
{ return model->train(_inputs, _outputs, _sample_weights, _sample_idx, *_params, flags); }
float CvANN_MLPPredict(CvANN_MLP* model, CvMat* _inputs,
   CvMat* _outputs )
{ return model->predict(_inputs, _outputs); }
int CvANN_MLPGetLayerCount(CvANN_MLP* model) { return model->get_layer_count(); }

//Decision Tree
CvDTreeParams* CvDTreeParamsCreate() { return new CvDTreeParams; }
void CvDTreeParamsRelease(CvDTreeParams** params) { delete *params; *params = 0;}

CvDTree* CvDTreeCreate() { return new CvDTree; }
void CvDTreeRelease(CvDTree** model) { delete *model; *model = 0;}
bool CvDTreeTrain(CvDTree* model, CvMat* _train_data, int _tflag,
   CvMat* _responses, CvMat* _var_idx,
   CvMat* _sample_idx, CvMat* _var_type,
   CvMat* _missing_mask,
   CvDTreeParams params )
{ return model->train(_train_data, _tflag, _responses, _var_idx, _sample_idx, _var_type, _missing_mask, params); }
CvDTreeNode* CvDTreePredict(CvDTree* model, CvMat* _sample, CvMat* _missing_data_mask, bool raw_mode )
{ return model->predict(_sample, _missing_data_mask, raw_mode); }

//Random Tree
CvRTParams* CvRTParamsCreate() { return new CvRTParams(); }
void CvRTParamsRelease(CvRTParams** params) { delete *params; *params = 0; }

CvRTrees* CvRTreesCreate() { return new CvRTrees(); }
void CvRTreesRelease(CvRTrees** model) { delete *model; *model = 0; }
bool CvRTreesTrain( CvRTrees* model, CvMat* _train_data, int _tflag,
   CvMat* _responses, CvMat* _var_idx,
   CvMat* _sample_idx, CvMat* _var_type,
   CvMat* _missing_mask,
   CvRTParams params )
{ return model->train(_train_data, _tflag, _responses, _var_idx, _sample_idx, _var_type, _missing_mask, params); }
/*
bool CvRTreesTrain( CvRTrees* model, CvMat* _train_data, int _tflag,
CvMat* _responses, CvMat* _var_idx=0,
CvMat* _sample_idx=0, CvMat* _var_type=0,
CvMat* _missing_mask=0,
CvRTParams params=CvRTParams() )
{ return model->train(_train_data, _tflag, _responses, _var_idx, _sample_idx, _var_type, _missing_mask, params); }
*/
float CvRTreesPredict(CvRTrees* model, CvMat* sample, CvMat* missing ) 
{ return model->predict(sample, missing); }
int CvRTreesGetTreeCount(CvRTrees* model) { return model->get_tree_count(); }
CvMat* CvRTreesGetVarImportance(CvRTrees* model) { return (CvMat*) model->get_var_importance(); }

//Extreme Random Tree
CvERTrees* CvERTreesCreate() { return new CvERTrees(); }
void CvERTreesRelease(CvERTrees** model) { delete *model; *model = 0; }

//CvBoost
CvBoostParams* CvBoostParamsCreate() { return new CvBoostParams(); }
void CvBoostParamsRelease(CvBoostParams** params) { delete *params; *params = 0; }

CvBoost* CvBoostCreate() { return new CvBoost(); }
void CvBoostRelease(CvBoost** model) { delete *model; *model = 0; }
bool CvBoostTrain(CvBoost* model, CvMat* _train_data, int _tflag,
   CvMat* _responses, CvMat* _var_idx,
   CvMat* _sample_idx, CvMat* _var_type,
   CvMat* _missing_mask,
   CvBoostParams params,
   bool update )
{ return model->train(_train_data, _tflag, _responses, _var_idx,
_sample_idx, _var_type, _missing_mask, params, update); }

float CvBoostPredict(CvBoost* model, CvMat* _sample, CvMat* _missing,
   CvMat* weak_responses, CvSlice slice,
   bool raw_mode)
{ return model->predict(_sample, _missing, weak_responses, slice, raw_mode); }


//CvGBTrees
void CvGBTreesParamsGetDefault(CvGBTreesParams* params)
{
   CvGBTreesParams p;
   memcpy(params, &p, sizeof(CvGBTreesParams));
}
CvGBTrees* CvGBTreesCreate()
{
   return new CvGBTrees();
}
void CvGBTreesRelease(CvGBTrees** model)
{
   delete * model;
   *model = 0;
}
bool CvGBTreesTrain(CvGBTrees* model, const CvMat* trainData, int tflag,
   const CvMat* responses, const CvMat* varIdx,
   const CvMat* sampleIdx, const CvMat* varType,
   const CvMat* missingDataMask,
   CvGBTreesParams params,
   bool update)
{
   return model->train(trainData, tflag, responses, varIdx, sampleIdx, varType, missingDataMask, params, update);
}
float CvGBTreesPredict(CvGBTrees* model, CvMat* _sample, CvMat* _missing,
   CvMat* weak_responses, CvSlice slice,
   bool raw_mode)
{
   return model->predict(_sample, _missing, weak_responses, slice, raw_mode);
}
