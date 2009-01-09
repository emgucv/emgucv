#ifndef CVEXTERN_H
#define CVEXTERN_H

#include "ml.h"

#define EMGU_CV_EXTERN_API extern "C"

#if defined(__cplusplus)
extern "C" 
{
#endif
   //StatModel
   EMGU_CV_EXTERN_API void StatModelSave(CvStatModel* model, const char* filename, const char* name=0 );
   EMGU_CV_EXTERN_API void StatModelLoad(CvStatModel* model, const char* filename, const char* name=0 );
   EMGU_CV_EXTERN_API void StatModelClear(CvStatModel* model);

   //CvNormalBayesClassifier
   EMGU_CV_EXTERN_API CvNormalBayesClassifier* CvNormalBayesClassifierDefaultCreate();
   EMGU_CV_EXTERN_API CvNormalBayesClassifier* CvNormalBayesClassifierCreate( const CvMat* _train_data, const CvMat* _responses, const CvMat* _var_idx=0, const CvMat* _sample_idx=0 );
   EMGU_CV_EXTERN_API void CvNormalBayesClassifierRelease(CvNormalBayesClassifier* classifier);
   EMGU_CV_EXTERN_API bool CvNormalBayesClassifierTrain(CvNormalBayesClassifier* classifier, const CvMat* _train_data, const CvMat* _responses,
      const CvMat* _var_idx = 0, const CvMat* _sample_idx=0, bool update=false );
   EMGU_CV_EXTERN_API float CvNormalBayesClassifierPredict(CvNormalBayesClassifier* classifier, const CvMat* _samples, CvMat* results=0 );

   //KNearest
   EMGU_CV_EXTERN_API CvKNearest* CvKNearestDefaultCreate();
   EMGU_CV_EXTERN_API bool CvKNearestTrain(CvKNearest* classifier, const CvMat* _train_data, const CvMat* _responses,
      const CvMat* _sample_idx=0, bool is_regression=false,
      int _max_k=32, bool _update_base=false );
   EMGU_CV_EXTERN_API CvKNearest* CvKNearestCreate(const CvMat* _train_data, const CvMat* _responses,
      const CvMat* _sample_idx=0, bool _is_regression=false, int max_k=32 );
   EMGU_CV_EXTERN_API void CvKNearestRelease(CvKNearest* classifier);
   EMGU_CV_EXTERN_API float CvKNearestFindNearest(CvKNearest* classifier, const CvMat* _samples, int k, CvMat* results=0,
      const float** neighbors=0, CvMat* neighbor_responses=0, CvMat* dist=0 );

   //EM
   EMGU_CV_EXTERN_API CvEM* CvEMDefaultCreate();
   EMGU_CV_EXTERN_API void CvEMRelease(CvEM* model);
   EMGU_CV_EXTERN_API bool CvEMTrain(CvEM* model, const CvMat* samples, const CvMat* sample_idx=0,
      CvEMParams params=CvEMParams(), CvMat* labels=0 );
   EMGU_CV_EXTERN_API float CvEMPredict(CvEM* model, const CvMat* sample, CvMat* probs );
   EMGU_CV_EXTERN_API int CvEMGetNclusters(CvEM* model);
   EMGU_CV_EXTERN_API const CvMat* CvEMGetMeans(CvEM* model);
   EMGU_CV_EXTERN_API const CvMat** CvEMGetCovs(CvEM* model);
   EMGU_CV_EXTERN_API const CvMat* CvEMGetWeights(CvEM* model);
   EMGU_CV_EXTERN_API const CvMat* CvEMGetProbs(CvEM* model);

   //SVM
   EMGU_CV_EXTERN_API CvSVM* CvSVMDefaultCreate();
   EMGU_CV_EXTERN_API bool CvSVMTrain(CvSVM* model, const CvMat* _train_data, const CvMat* _responses,
      const CvMat* _var_idx=0, const CvMat* _sample_idx=0,
      CvSVMParams _params=CvSVMParams() );
   EMGU_CV_EXTERN_API bool CvSVMTrainAuto(CvSVM* model, const CvMat* _train_data, const CvMat* _responses,
      const CvMat* _var_idx, const CvMat* _sample_idx, CvSVMParams _params,
      int k_fold = 10,
      CvParamGrid C_grid      = CvSVM::get_default_grid(CvSVM::C),
      CvParamGrid gamma_grid  = CvSVM::get_default_grid(CvSVM::GAMMA),
      CvParamGrid p_grid      = CvSVM::get_default_grid(CvSVM::P),
      CvParamGrid nu_grid     = CvSVM::get_default_grid(CvSVM::NU),
      CvParamGrid coef_grid   = CvSVM::get_default_grid(CvSVM::COEF),
      CvParamGrid degree_grid = CvSVM::get_default_grid(CvSVM::DEGREE) );
   EMGU_CV_EXTERN_API void CvSVMGetDefaultGrid(int gridType, CvParamGrid* grid);
   EMGU_CV_EXTERN_API void CvSVMRelease(CvSVM* model);

   //ANN_MLP
   EMGU_CV_EXTERN_API CvANN_MLP* CvANN_MLPCreate(const CvMat* _layer_sizes,
      int _activ_func= CvANN_MLP::SIGMOID_SYM,
      double _f_param1=0, double _f_param2=0 );
   EMGU_CV_EXTERN_API void CvANN_MLPRelease(CvANN_MLP* model);
   EMGU_CV_EXTERN_API int CvANN_MLPTrain(CvANN_MLP* model, const CvMat* _inputs, const CvMat* _outputs,
      const CvMat* _sample_weights, const CvMat* _sample_idx=0,
      CvANN_MLP_TrainParams _params = CvANN_MLP_TrainParams(),
      int flags=0 );
   EMGU_CV_EXTERN_API float CvANN_MLPPredict(CvANN_MLP* model, const CvMat* _inputs,
      CvMat* _outputs );

   //Decision Tree
   EMGU_CV_EXTERN_API CvDTreeParams* CvDTreeParamsCreate();
   EMGU_CV_EXTERN_API void CvDTreeParamsRelease(CvDTreeParams* params);

   EMGU_CV_EXTERN_API CvDTree* CvDTreeCreate();
   EMGU_CV_EXTERN_API void CvDTreeRelease(CvDTree* model);
   EMGU_CV_EXTERN_API bool CvDTreeTrain(CvDTree* model, const CvMat* _train_data, int _tflag,
      const CvMat* _responses, const CvMat* _var_idx=0,
      const CvMat* _sample_idx=0, const CvMat* _var_type=0,
      const CvMat* _missing_mask=0,
      CvDTreeParams params=CvDTreeParams() );
   EMGU_CV_EXTERN_API CvDTreeNode* predict(CvDTree* model, const CvMat* _sample, const CvMat* _missing_data_mask=0,
      bool raw_mode=false );

   //Random Tree
   EMGU_CV_EXTERN_API CvRTParams* CvRTParamsCreate();
   EMGU_CV_EXTERN_API void CvRTParamsRelease(CvRTParams* params);

   EMGU_CV_EXTERN_API CvRTrees* CvRTreesCreate();
   EMGU_CV_EXTERN_API void CvRTreesRelease(CvRTrees* model);
   EMGU_CV_EXTERN_API bool CvRTreesTrain( CvRTrees* model, const CvMat* _train_data, int _tflag,
      const CvMat* _responses, const CvMat* _var_idx=0,
      const CvMat* _sample_idx=0, const CvMat* _var_type=0,
      const CvMat* _missing_mask=0,
      CvRTParams params=CvRTParams() );
   EMGU_CV_EXTERN_API float CvRTreesPredict(CvRTrees* model, const CvMat* sample, const CvMat* missing = 0 );

   //CvBoost
   EMGU_CV_EXTERN_API CvBoostParams* CvBoostParamsCreate();
   EMGU_CV_EXTERN_API void CvBoostParamsRelease(CvBoostParams* params);

   EMGU_CV_EXTERN_API CvBoost* CvBoostCreate();
   EMGU_CV_EXTERN_API void CvBoostRelease(CvBoost* model);
   EMGU_CV_EXTERN_API bool CvBoostTrain(CvBoost* model, const CvMat* _train_data, int _tflag,
      const CvMat* _responses, const CvMat* _var_idx=0,
      const CvMat* _sample_idx=0, const CvMat* _var_type=0,
      const CvMat* _missing_mask=0,
      CvBoostParams params=CvBoostParams(),
      bool update=false );
   EMGU_CV_EXTERN_API float CvBoostPredict(CvBoost* model, const CvMat* _sample, const CvMat* _missing=0,
      CvMat* weak_responses=0, CvSlice slice=CV_WHOLE_SEQ,
      bool raw_mode=false );
#if defined(__cplusplus)
}
#endif
#endif
