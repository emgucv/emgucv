//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_ML_C_H
#define EMGU_ML_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/ml/ml.hpp"

//StatModel
CVAPI(void) StatModelSave(cv::ml::StatModel* model, cv::String* filename);

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
CVAPI(cv::ml::NormalBayesClassifier*) cveNormalBayesClassifierDefaultCreate(cv::ml::StatModel** statModel, cv::Algorithm** algorithm);
CVAPI(void) cveNormalBayesClassifierRelease(cv::ml::NormalBayesClassifier** classifier);

//KNearest
CVAPI(cv::ml::KNearest*) cveKNearestCreate(cv::ml::StatModel** statModel, cv::Algorithm** algorithm);
CVAPI(void) cveKNearestRelease(cv::ml::KNearest** classifier);

//EM
CVAPI(cv::ml::EM*) cveEMDefaultCreate(cv::ml::StatModel** statModel, cv::Algorithm** algorithm);
CVAPI(void) cveEMTrainE(
   cv::ml::EM* model,
   cv::_InputArray* samples,
   cv::_InputArray* means0,
   cv::_InputArray* covs0,
   cv::_InputArray* weights0,
   cv::_OutputArray* logLikelihoods,
   cv::_OutputArray* labels,
   cv::_OutputArray* probs,
   cv::ml::StatModel** statModel, cv::Algorithm** algorithm);
CVAPI(void) cveEMTrainM(
   cv::ml::EM* model,
   cv::_InputArray* samples,
   cv::_InputArray* probs0,
   cv::_OutputArray* logLikelihoods,
   cv::_OutputArray* labels,
   cv::_OutputArray* probs,
   cv::ml::StatModel** statModel, cv::Algorithm** algorithm);
CVAPI(void) cveEMPredict(cv::ml::EM* model, cv::_InputArray* sample, CvPoint2D64f* result, cv::_OutputArray* probs);

CVAPI(void) cveEMRelease(cv::ml::EM** model);


//SVM
CVAPI(cv::ml::SVM*) cveSVMDefaultCreate(cv::ml::StatModel** model, cv::Algorithm** algorithm);

CVAPI(bool) cveSVMTrainAuto(
   cv::ml::SVM* model, cv::ml::TrainData* trainData, int kFold,
   cv::ml::ParamGrid* CGrid,
   cv::ml::ParamGrid* gammaGrid,
   cv::ml::ParamGrid* pGrid,
   cv::ml::ParamGrid* nuGrid,
   cv::ml::ParamGrid* coefGrid,
   cv::ml::ParamGrid* degreeGrid,
   bool balanced);

CVAPI(void) cveSVMGetDefaultGrid(int gridType, cv::ml::ParamGrid* grid);
CVAPI(void) cveSVMRelease(cv::ml::SVM** model);
CVAPI(void) cveSVMGetSupportVectors(cv::ml::SVM* model, cv::Mat* supportVectors);

//ANN_MLP
CVAPI(cv::ml::ANN_MLP*) cveANN_MLPCreate(cv::ml::StatModel** model, cv::Algorithm** algorithm);
CVAPI(void) cveANN_MLPSetLayerSizes(cv::ml::ANN_MLP* model, cv::_InputArray* layerSizes); 
CVAPI(void) cveANN_MLPSetActivationFunction(cv::ml::ANN_MLP* model, int type, double param1, double param2);
CVAPI(void) cveANN_MLPSetTrainMethod(cv::ml::ANN_MLP* model, int method, double param1, double param2);
CVAPI(void) cveANN_MLPRelease(cv::ml::ANN_MLP** model);

//Decision Tree
CVAPI(cv::ml::DTrees*) cveDTreesCreate(cv::ml::StatModel** statModel, cv::Algorithm** algorithm);
CVAPI(void) cveDTreesRelease(cv::ml::DTrees** model);

//Random Tree
CVAPI(cv::ml::RTrees*) cveRTreesCreate(cv::ml::StatModel** statModel, cv::Algorithm** algorithm);
CVAPI(void) cveRTreesRelease(cv::ml::RTrees** model);

//CvBoost
CVAPI(cv::ml::Boost*) cveBoostCreate(cv::ml::StatModel** statModel, cv::Algorithm** algorithm);
CVAPI(void) cveBoostRelease(cv::ml::Boost** model);

//LogisticRegression
CVAPI(cv::ml::LogisticRegression*) cveLogisticRegressionCreate(cv::ml::StatModel** statModel, cv::Algorithm** algorithm);
CVAPI(void) cveLogisticRegressionRelease(cv::ml::LogisticRegression** model);

//SVMSGD
CVAPI(cv::ml::SVMSGD*) cveSVMSGDDefaultCreate(cv::ml::StatModel** model, cv::Algorithm** algorithm);
CVAPI(void) cveSVMSGDRelease(cv::ml::SVMSGD** model);
CVAPI(void) cveSVMSGDSetOptimalParameters(cv::ml::SVMSGD* model, int svmsgdType, int marginType);
#endif
