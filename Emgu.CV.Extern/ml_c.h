//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_ML_C_H
#define EMGU_ML_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/ml/ml.hpp"

//StatModel
CVAPI(void) StatModelSave(CvStatModel* model, char* filename, char* name = 0);
CVAPI(void) StatModelLoad(CvStatModel* model, char* filename, char* name = 0);
CVAPI(void) StatModelClear(CvStatModel* model);

//CvNormalBayesClassifier
CVAPI(CvNormalBayesClassifier*) CvNormalBayesClassifierDefaultCreate();
CVAPI(CvNormalBayesClassifier*) CvNormalBayesClassifierCreate( CvMat* _train_data, CvMat* _responses, CvMat* _var_idx, CvMat* _sample_idx );
CVAPI(void) CvNormalBayesClassifierRelease(CvNormalBayesClassifier* classifier);
CVAPI(bool) CvNormalBayesClassifierTrain(CvNormalBayesClassifier* classifier, CvMat* _train_data, CvMat* _responses,
                                         CvMat* _var_idx, CvMat* _sample_idx, bool update );
CVAPI(float) CvNormalBayesClassifierPredict(CvNormalBayesClassifier* classifier, CvMat* _samples, CvMat* results );
//KNearest
CVAPI(CvKNearest*) CvKNearestDefaultCreate();
CVAPI(void) CvKNearestRelease(CvKNearest* classifier);
CVAPI(bool) CvKNearestTrain(CvKNearest* classifier, CvMat* _train_data, CvMat* _responses,
                            CvMat* _sample_idx, bool is_regression,
                            int _max_k, bool _update_base);
CVAPI(CvKNearest*) CvKNearestCreate(CvMat* _train_data, CvMat* _responses,
                                    CvMat* _sample_idx, bool _is_regression, int max_k );
CVAPI(float) CvKNearestFindNearest(CvKNearest* classifier, CvMat* _samples, int k, CvMat* results,
                                   float** neighbors, CvMat* neighbor_responses, CvMat* dist );

//EM
CVAPI(CvEM*) CvEMDefaultCreate();
CVAPI(void) CvEMRelease(CvEM* model);
CVAPI(bool) CvEMTrain(CvEM* model, CvMat* samples, CvMat* sample_idx,
                      CvEMParams params, CvMat* labels );
CVAPI(float) CvEMPredict(CvEM* model, CvMat* sample, CvMat* probs );
CVAPI(int) CvEMGetNclusters(CvEM* model);
CVAPI(CvMat*) CvEMGetMeans(CvEM* model);
CVAPI(CvMat**) CvEMGetCovs(CvEM* model);
CVAPI(CvMat*) CvEMGetWeights(CvEM* model);
CVAPI(CvMat*) CvEMGetProbs(CvEM* model);

//SVM
CVAPI(CvSVM*) CvSVMDefaultCreate();
CVAPI(bool) CvSVMTrain(CvSVM* model, CvMat* _train_data, CvMat* _responses,
                       CvMat* _var_idx, CvMat* _sample_idx,
                       CvSVMParams _params);
CVAPI(bool) CvSVMTrainAuto(CvSVM* model, CvMat* _train_data, CvMat* _responses,
                           CvMat* _var_idx, CvMat* _sample_idx, CvSVMParams _params,
                           int k_fold,
                           CvParamGrid C_grid,
                           CvParamGrid gamma_grid,
                           CvParamGrid p_grid,
                           CvParamGrid nu_grid,
                           CvParamGrid coef_grid,
                           CvParamGrid degree_grid);
CVAPI(void) CvSVMGetDefaultGrid(int gridType, CvParamGrid* grid);
CVAPI(void) CvSVMRelease(CvSVM* model);
CVAPI(float) CvSVMPredict(CvSVM* model,  CvMat* _sample );
CVAPI(float*) CvSVMGetSupportVector(CvSVM* model, int i);
CVAPI(int) CvSVMGetSupportVectorCount(CvSVM* model);
CVAPI(int) CvSVMGetVarCount(CvSVM* model);
CVAPI(void) CvSVMGetParameters(CvSVM* model, CvSVMParams* param);

//ANN_MLP
CVAPI(CvANN_MLP*) CvANN_MLPCreate(CvMat* _layer_sizes,
                                  int _activ_func,
                                  double _f_param1, double _f_param2 );
CVAPI(void) CvANN_MLPRelease(CvANN_MLP* model);
CVAPI(int) CvANN_MLPTrain(CvANN_MLP* model, CvMat* _inputs, CvMat* _outputs,
                          CvMat* _sample_weights, CvMat* _sample_idx,
                          CvANN_MLP_TrainParams* _params,
                          int flags);
CVAPI(float) CvANN_MLPPredict(CvANN_MLP* model, CvMat* _inputs,
                              CvMat* _outputs );
CVAPI(int) CvANN_MLPGetLayerCount(CvANN_MLP* model);

//Decision Tree
CVAPI(CvDTreeParams*) CvDTreeParamsCreate();
CVAPI(void) CvDTreeParamsRelease(CvDTreeParams* params);
CVAPI(CvDTree*) CvDTreeCreate();
CVAPI(void) CvDTreeRelease(CvDTree* model);
CVAPI(bool) CvDTreeTrain(CvDTree* model, CvMat* _train_data, int _tflag,
                         CvMat* _responses, CvMat* _var_idx,
                         CvMat* _sample_idx, CvMat* _var_type,
                         CvMat* _missing_mask,
                         CvDTreeParams params );
CVAPI(CvDTreeNode*) CvDTreePredict(CvDTree* model, CvMat* _sample, CvMat* _missing_data_mask, bool raw_mode );

//Random Tree
CVAPI(CvRTParams*) CvRTParamsCreate();
CVAPI(void) CvRTParamsRelease(CvRTParams* params);

CVAPI(CvRTrees*) CvRTreesCreate();
CVAPI(void) CvRTreesRelease(CvRTrees* model);
CVAPI(bool) CvRTreesTrain( CvRTrees* model, CvMat* _train_data, int _tflag,
                          CvMat* _responses, CvMat* _var_idx,
                          CvMat* _sample_idx, CvMat* _var_type,
                          CvMat* _missing_mask,
                          CvRTParams params );
/*
CVAPI(bool) CvRTreesTrain( CvRTrees* model, CvMat* _train_data, int _tflag,
                          CvMat* _responses, CvMat* _var_idx=0,
                          CvMat* _sample_idx=0, CvMat* _var_type=0,
                          CvMat* _missing_mask=0,
                          CvRTParams params=CvRTParams() )
{ return model->train(_train_data, _tflag, _responses, _var_idx, _sample_idx, _var_type, _missing_mask, params); }
*/
CVAPI(float) CvRTreesPredict(CvRTrees* model, CvMat* sample, CvMat* missing );
CVAPI(int) CvRTreesGetTreeCount(CvRTrees* model);
CVAPI(CvMat*) CvRTreesGetVarImportance(CvRTrees* model);

//Extreme Random Tree
CVAPI(CvERTrees*) CvERTreesCreate();
CVAPI(void) CvERTreesRelease(CvERTrees* model);

//CvBoost
CVAPI(CvBoostParams*) CvBoostParamsCreate();
CVAPI(void) CvBoostParamsRelease(CvBoostParams* params);

CVAPI(CvBoost*) CvBoostCreate();
CVAPI(void) CvBoostRelease(CvBoost* model);
CVAPI(bool) CvBoostTrain(CvBoost* model, CvMat* _train_data, int _tflag,
                         CvMat* _responses, CvMat* _var_idx,
                         CvMat* _sample_idx, CvMat* _var_type,
                         CvMat* _missing_mask,
                         CvBoostParams params,
                         bool update );

CVAPI(float) CvBoostPredict(CvBoost* model, CvMat* _sample, CvMat* _missing=0,
                            CvMat* weak_responses=0, CvSlice slice=CV_WHOLE_SEQ,
                            bool raw_mode=false );

#endif