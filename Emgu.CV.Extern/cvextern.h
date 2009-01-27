#ifndef CVEXTERN_H
#define CVEXTERN_H

#include "ml.h"

//StatModel
CVAPI(void) StatModelSave(CvStatModel* model, const char* filename, const char* name=0 );
CVAPI(void) StatModelLoad(CvStatModel* model, const char* filename, const char* name=0 );
CVAPI(void) StatModelClear(CvStatModel* model);

//CvNormalBayesClassifier
CVAPI(CvNormalBayesClassifier*) CvNormalBayesClassifierDefaultCreate();
CVAPI(CvNormalBayesClassifier*) CvNormalBayesClassifierCreate( const CvMat* _train_data, const CvMat* _responses, const CvMat* _var_idx=0, const CvMat* _sample_idx=0 );
CVAPI(void) CvNormalBayesClassifierRelease(CvNormalBayesClassifier* classifier);
CVAPI(bool) CvNormalBayesClassifierTrain(CvNormalBayesClassifier* classifier, const CvMat* _train_data, const CvMat* _responses,
   const CvMat* _var_idx = 0, const CvMat* _sample_idx=0, bool update=false );
CVAPI(float) CvNormalBayesClassifierPredict(CvNormalBayesClassifier* classifier, const CvMat* _samples, CvMat* results=0 );

//KNearest
CVAPI(CvKNearest*) CvKNearestDefaultCreate();
CVAPI(bool) CvKNearestTrain(CvKNearest* classifier, const CvMat* _train_data, const CvMat* _responses,
   const CvMat* _sample_idx=0, bool is_regression=false,
   int _max_k=32, bool _update_base=false );
CVAPI(CvKNearest*) CvKNearestCreate(const CvMat* _train_data, const CvMat* _responses,
   const CvMat* _sample_idx=0, bool _is_regression=false, int max_k=32 );
CVAPI(void) CvKNearestRelease(CvKNearest* classifier);
CVAPI(float) CvKNearestFindNearest(CvKNearest* classifier, const CvMat* _samples, int k, CvMat* results=0,
   const float** neighbors=0, CvMat* neighbor_responses=0, CvMat* dist=0 );

//EM
CVAPI(CvEM*) CvEMDefaultCreate();
CVAPI(void) CvEMRelease(CvEM* model);
CVAPI(bool) CvEMTrain(CvEM* model, const CvMat* samples, const CvMat* sample_idx=0,
   CvEMParams params=CvEMParams(), CvMat* labels=0 );
CVAPI(float) CvEMPredict(CvEM* model, const CvMat* sample, CvMat* probs );
CVAPI(int) CvEMGetNclusters(CvEM* model);
CVAPI(const CvMat*) CvEMGetMeans(CvEM* model);
CVAPI(const CvMat**) CvEMGetCovs(CvEM* model);
CVAPI(const CvMat*) CvEMGetWeights(CvEM* model);
CVAPI(const CvMat*) CvEMGetProbs(CvEM* model);

//SVM
CVAPI(CvSVM*) CvSVMDefaultCreate();
CVAPI(bool) CvSVMTrain(CvSVM* model, const CvMat* _train_data, const CvMat* _responses,
   const CvMat* _var_idx=0, const CvMat* _sample_idx=0,
   CvSVMParams _params=CvSVMParams() );
CVAPI(bool) CvSVMTrainAuto(CvSVM* model, const CvMat* _train_data, const CvMat* _responses,
   const CvMat* _var_idx, const CvMat* _sample_idx, CvSVMParams _params,
   int k_fold = 10,
   CvParamGrid C_grid      = CvSVM::get_default_grid(CvSVM::C),
   CvParamGrid gamma_grid  = CvSVM::get_default_grid(CvSVM::GAMMA),
   CvParamGrid p_grid      = CvSVM::get_default_grid(CvSVM::P),
   CvParamGrid nu_grid     = CvSVM::get_default_grid(CvSVM::NU),
   CvParamGrid coef_grid   = CvSVM::get_default_grid(CvSVM::COEF),
   CvParamGrid degree_grid = CvSVM::get_default_grid(CvSVM::DEGREE) );
CVAPI(void) CvSVMGetDefaultGrid(int gridType, CvParamGrid* grid);
CVAPI(void) CvSVMRelease(CvSVM* model);

CVAPI(float) cvSVMPredict(CvSVM* model,  const CvMat* _sample );
CVAPI(const float*) cvSVMGetSupportVector(CvSVM* model, int i);
CVAPI(int) cvSVMGetSupportVectorCount(CvSVM* model);
CVAPI(int) cvSVMGetVarCount(CvSVM* model);

//ANN_MLP
CVAPI(CvANN_MLP*) CvANN_MLPCreate(const CvMat* _layer_sizes,
   int _activ_func= CvANN_MLP::SIGMOID_SYM,
   double _f_param1=0, double _f_param2=0 );
CVAPI(void) CvANN_MLPRelease(CvANN_MLP* model);
CVAPI(int) CvANN_MLPTrain(CvANN_MLP* model, const CvMat* _inputs, const CvMat* _outputs,
   const CvMat* _sample_weights, const CvMat* _sample_idx=0,
   CvANN_MLP_TrainParams _params = CvANN_MLP_TrainParams(),
   int flags=0 );
CVAPI(float) CvANN_MLPPredict(CvANN_MLP* model, const CvMat* _inputs,
   CvMat* _outputs );

//Decision Tree
CVAPI(CvDTreeParams*) CvDTreeParamsCreate();
CVAPI(void) CvDTreeParamsRelease(CvDTreeParams* params);

CVAPI(CvDTree*) CvDTreeCreate();
CVAPI(void) CvDTreeRelease(CvDTree* model);
CVAPI(bool) CvDTreeTrain(CvDTree* model, const CvMat* _train_data, int _tflag,
   const CvMat* _responses, const CvMat* _var_idx=0,
   const CvMat* _sample_idx=0, const CvMat* _var_type=0,
   const CvMat* _missing_mask=0,
   CvDTreeParams params=CvDTreeParams() );
CVAPI(CvDTreeNode*) predict(CvDTree* model, const CvMat* _sample, const CvMat* _missing_data_mask=0,
   bool raw_mode=false );

//Random Tree
CVAPI(CvRTParams*) CvRTParamsCreate();
CVAPI(void) CvRTParamsRelease(CvRTParams* params);

CVAPI(CvRTrees*) CvRTreesCreate();
CVAPI(void) CvRTreesRelease(CvRTrees* model);
CVAPI(bool) CvRTreesTrain( CvRTrees* model, const CvMat* _train_data, int _tflag,
   const CvMat* _responses, const CvMat* _var_idx=0,
   const CvMat* _sample_idx=0, const CvMat* _var_type=0,
   const CvMat* _missing_mask=0,
   CvRTParams params=CvRTParams() );
CVAPI(float) CvRTreesPredict(CvRTrees* model, const CvMat* sample, const CvMat* missing = 0 );

//CvBoost
CVAPI(CvBoostParams*) CvBoostParamsCreate();
CVAPI(void) CvBoostParamsRelease(CvBoostParams* params);

CVAPI(CvBoost*) CvBoostCreate();
CVAPI(void) CvBoostRelease(CvBoost* model);
CVAPI(bool) CvBoostTrain(CvBoost* model, const CvMat* _train_data, int _tflag,
   const CvMat* _responses, const CvMat* _var_idx=0,
   const CvMat* _sample_idx=0, const CvMat* _var_type=0,
   const CvMat* _missing_mask=0,
   CvBoostParams params=CvBoostParams(),
   bool update=false );
CVAPI(float) CvBoostPredict(CvBoost* model, const CvMat* _sample, const CvMat* _missing=0,
   CvMat* weak_responses=0, CvSlice slice=CV_WHOLE_SEQ,
   bool raw_mode=false );

#endif
