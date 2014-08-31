//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_ML_C_H
#define EMGU_ML_C_H

#include "opencv2/core/core_c.h"
//#include "opencv2/legacy/legacy.hpp"
#include "opencv2/ml/ml.hpp"

//StatModel
CVAPI(void) StatModelSave(cv::ml::StatModel* model, cv::String* filename);
/*
CVAPI(void) StatModelLoad(cv::ml::StatModel* model, cv::String* filename);*/
CVAPI(void) StatModelClear(cv::ml::StatModel* model);
CVAPI(bool) StatModelTrain(cv::ml::StatModel* model, cv::_InputArray* samples, int layout, cv::_InputArray* responses );
CVAPI(bool) StatModelTrainWithData(cv::ml::StatModel* model, cv::ml::TrainData* data, int flags);
CVAPI(float) StatModelPredict(cv::ml::StatModel* model, cv::_InputArray* samples, cv::_OutputArray* results, int flags); 

CVAPI(cv::ml::TrainData*) cveTrainDataCreate(
   cv::_InputArray* samples, int layout, cv::_InputArray* responses,
   cv::_InputArray* varIdx, cv::_InputArray* sampleIdx,
   cv::_InputArray* sampleWeights, cv::_InputArray* varType);

CVAPI(void) cveTrainDataRelease(cv::ml::TrainData** data);

//CvNormalBayesClassifier
CVAPI(cv::ml::NormalBayesClassifier*) CvNormalBayesClassifierDefaultCreate(cv::ml::StatModel** statModel, cv::Algorithm** algorithm);

//CVAPI(cv::ml::NormalBayesClassifier*) CvNormalBayesClassifierCreate( CvMat* _train_data, CvMat* _responses, CvMat* _var_idx, CvMat* _sample_idx );
CVAPI(void) CvNormalBayesClassifierRelease(cv::ml::NormalBayesClassifier** classifier);
/*CVAPI(bool) CvNormalBayesClassifierTrain(cv::ml::NormalBayesClassifier* classifier, CvMat* _train_data, CvMat* _responses,
                                         CvMat* _var_idx, CvMat* _sample_idx, bool update );
CVAPI(float) CvNormalBayesClassifierPredict(cv::ml::NormalBayesClassifier* classifier, CvMat* _samples, CvMat* results );*/
//KNearest
CVAPI(cv::ml::KNearest*) CvKNearestCreate(int defaultK, bool isClassifier, cv::ml::StatModel** statModel, cv::Algorithm** algorithm);
CVAPI(void) CvKNearestRelease(cv::ml::KNearest** classifier);
/*CVAPI(bool) CvKNearestTrain(CvKNearest* classifier, CvMat* _train_data, CvMat* _responses,
                            CvMat* _sample_idx, bool is_regression,
                            int _max_k, bool _update_base);
CVAPI(CvKNearest*) CvKNearestCreate(CvMat* _train_data, CvMat* _responses,
                                    CvMat* _sample_idx, bool _is_regression, int max_k );
CVAPI(float) CvKNearestFindNearest(CvKNearest* classifier, CvMat* _samples, int k, CvMat* results,
                                   float** neighbors, CvMat* neighbor_responses, CvMat* dist );*/

//EM
CVAPI(cv::ml::EM::Params*) cveEmParamsCreate(int nclusters, int covMatType, CvTermCriteria* termcrit);
CVAPI(void) cveEmParamsRelease(cv::ml::EM::Params** p);
CVAPI(cv::ml::EM*) CvEMDefaultCreate(cv::ml::EM::Params* p, cv::ml::StatModel** statModel, cv::Algorithm** algorithm);
CVAPI(cv::ml::EM*) CvEMTrainStartWithE(
   cv::_InputArray* samples,
   cv::_InputArray* means0,
   cv::_InputArray* covs0,
   cv::_InputArray* weights0,
   cv::_OutputArray* logLikelihoods,
   cv::_OutputArray* labels,
   cv::_OutputArray* probs,
   cv::ml::EM::Params* p, 
   cv::ml::StatModel** statModel, cv::Algorithm** algorithm);
CVAPI(cv::ml::EM*) CvEMTrainStartWithM(
   cv::_InputArray* samples,
   cv::_InputArray* probs0,
   cv::_OutputArray* logLikelihoods,
   cv::_OutputArray* labels,
   cv::_OutputArray* probs,
   cv::ml::EM::Params* p, 
   cv::ml::StatModel** statModel, cv::Algorithm** algorithm);
CVAPI(void) CvEMPredict(cv::ml::EM* model, cv::_InputArray* sample, CvPoint2D64f* result, cv::_OutputArray* probs);

CVAPI(void) CvEMRelease(cv::ml::EM** model);

/*
CVAPI(bool) CvEMTrain(cv::EM* model, cv::_InputArray* samples, cv::_OutputArray* logLikelihoods, cv::_OutputArray* labels, cv::_OutputArray* probs);
*/


//SVM
CVAPI(cv::ml::SVM::Params*) CvSVMParamsCreate(
   int svmType, int kernelType, double degree, double gamma, double coef0,
   double con, double nu, double p, cv::Mat* classWeights, CvTermCriteria* termCrit);
CVAPI(void) CvSVMParamsRelease(cv::ml::SVM::Params** p);
CVAPI(cv::ml::SVM*) CvSVMDefaultCreate(cv::ml::SVM::Params* p, cv::ml::StatModel** model, cv::Algorithm** algorithm);

CVAPI(bool) CvSVMTrainAuto(
   cv::ml::SVM* model, cv::ml::TrainData* trainData, int kFold,
   cv::ml::ParamGrid* CGrid,
   cv::ml::ParamGrid* gammaGrid,
   cv::ml::ParamGrid* pGrid,
   cv::ml::ParamGrid* nuGrid,
   cv::ml::ParamGrid* coefGrid,
   cv::ml::ParamGrid* degreeGrid,
   bool balanced);

CVAPI(void) CvSVMGetDefaultGrid(int gridType, cv::ml::ParamGrid* grid);
CVAPI(void) CvSVMRelease(cv::ml::SVM** model);
CVAPI(void) CvSVMGetSupportVectors(cv::ml::SVM* model, cv::Mat* supportVectors);

//ANN_MLP
CVAPI(cv::ml::ANN_MLP::Params*) CvANN_MLPParamsCreate(
   cv::Mat* layerSizes, int activateFunc, double fparam1, double fparam2,
   CvTermCriteria* termCrit, int trainMethod, double param1, double param2);
CVAPI(void) CvANN_MLPParamsRelease(cv::ml::ANN_MLP::Params** p);
CVAPI(cv::ml::ANN_MLP*) CvANN_MLPCreate(cv::ml::ANN_MLP::Params* p, cv::ml::StatModel** model, cv::Algorithm** algorithm);
CVAPI(void) CvANN_MLPRelease(cv::ml::ANN_MLP** model);

//Decision Tree
CVAPI(cv::ml::DTrees::Params*) CvDTreeParamsCreate(
   int maxDepth, int minSampleCount,
   double regressionAccuracy, bool useSurrogates,
   int maxCategories, int CVFolds,
   bool use1SERule, bool truncatePrunedTree,
   cv::Mat* priors);
CVAPI(void) CvDTreeParamsRelease(cv::ml::DTrees::Params** params);
CVAPI(cv::ml::DTrees*) CvDTreeCreate(cv::ml::DTrees::Params* p, cv::ml::StatModel** statModel, cv::Algorithm** algorithm);
CVAPI(void) CvDTreeRelease(cv::ml::DTrees** model);

//Random Tree
CVAPI(cv::ml::RTrees::Params*) CvRTParamsCreate(
   int maxDepth, int minSampleCount,
   double regressionAccuracy, bool useSurrogates,
   int maxCategories, cv::Mat* priors,
   bool calcVarImportance, int nactiveVars,
   CvTermCriteria* termCrit);
CVAPI(void) CvRTParamsRelease(cv::ml::RTrees::Params** params);

CVAPI(cv::ml::RTrees*) CvRTreesCreate(cv::ml::RTrees::Params* p, cv::ml::StatModel** statModel, cv::Algorithm** algorithm);
CVAPI(void) CvRTreesRelease(cv::ml::RTrees** model);
/*
CVAPI(int) CvRTreesGetTreeCount(CvRTrees* model);
CVAPI(CvMat*) CvRTreesGetVarImportance(CvRTrees* model);
*/

//Extreme Random Tree
//CVAPI(CvERTrees*) CvERTreesCreate();
//CVAPI(void) CvERTreesRelease(CvERTrees** model);

//CvBoost
CVAPI(cv::ml::Boost::Params*) CvBoostParamsCreate(
   int boostType, int weakCount, double weightTrimRate,
   int maxDepth, bool useSurrogates, cv::Mat* priors);
CVAPI(void) CvBoostParamsRelease(cv::ml::Boost::Params** params);

CVAPI(cv::ml::Boost*) CvBoostCreate(cv::ml::Boost::Params* p, cv::ml::StatModel** statModel, cv::Algorithm** algorithm);
CVAPI(void) CvBoostRelease(cv::ml::Boost** model);

//CvGBTrees
//CVAPI(void) CvGBTreesParamsGetDefault(CvGBTreesParams* params); 
//CVAPI(CvGBTrees*) CvGBTreesCreate();
//CVAPI(void) CvGBTreesRelease(CvGBTrees** model);
/*
CVAPI(bool) CvGBTreesTrain(CvGBTrees* model, const CvMat* trainData, int tflag,
             const CvMat* responses, const CvMat* varIdx,
             const CvMat* sampleIdx, const CvMat* varType,
             const CvMat* missingDataMask,
             CvGBTreesParams* params,
             bool update);
CVAPI(float) CvGBTreesPredict(CvGBTrees* model, CvMat* _sample, CvMat* _missing,
                            CvMat* weak_responses, CvSlice* slice,
                            bool raw_mode);*/
#endif
