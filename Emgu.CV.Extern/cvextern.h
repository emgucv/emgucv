#include "ml.h"

#define EMGU_CV_EXTERN_API __declspec(dllexport)

#if defined(__cplusplus)
extern "C" 
{
#endif
//StatModel
extern EMGU_CV_EXTERN_API void StatModelSave(CvStatModel* model, const char* filename, const char* name=0 );
extern EMGU_CV_EXTERN_API void StatModelLoad(CvStatModel* model, const char* filename, const char* name=0 );
extern EMGU_CV_EXTERN_API void StatModelClear(CvStatModel* model);

//CvNormalBayesClassifier
extern EMGU_CV_EXTERN_API CvNormalBayesClassifier* CvNormalBayesClassifierDefaultCreate();
extern EMGU_CV_EXTERN_API CvNormalBayesClassifier* CvNormalBayesClassifierCreate( const CvMat* _train_data, const CvMat* _responses, const CvMat* _var_idx=0, const CvMat* _sample_idx=0 );
extern EMGU_CV_EXTERN_API void CvNormalBayesClassifierRelease(CvNormalBayesClassifier* classifier);
extern EMGU_CV_EXTERN_API bool CvNormalBayesClassifierTrain(CvNormalBayesClassifier* classifier, const CvMat* _train_data, const CvMat* _responses,
        const CvMat* _var_idx = 0, const CvMat* _sample_idx=0, bool update=false );
extern EMGU_CV_EXTERN_API float CvNormalBayesClassifierPredict(CvNormalBayesClassifier* classifier, const CvMat* _samples, CvMat* results=0 );

//KNearest
extern EMGU_CV_EXTERN_API CvKNearest* CvKNearestDefaultCreate();
extern EMGU_CV_EXTERN_API bool CvKNearestTrain(CvKNearest* classifier, const CvMat* _train_data, const CvMat* _responses,
                        const CvMat* _sample_idx=0, bool is_regression=false,
                        int _max_k=32, bool _update_base=false );
extern EMGU_CV_EXTERN_API CvKNearest* CvKNearestCreate(const CvMat* _train_data, const CvMat* _responses,
                const CvMat* _sample_idx=0, bool _is_regression=false, int max_k=32 );
extern EMGU_CV_EXTERN_API void CvKNearestRelease(CvKNearest* classifier);
extern EMGU_CV_EXTERN_API float CvKNearestFindNearest(CvKNearest* classifier, const CvMat* _samples, int k, CvMat* results=0,
        const float** neighbors=0, CvMat* neighbor_responses=0, CvMat* dist=0 );

//EM
extern EMGU_CV_EXTERN_API CvEM* CvEMDefaultCreate();
extern EMGU_CV_EXTERN_API void CvEMRelease(CvEM* model);
extern EMGU_CV_EXTERN_API bool CvEMTrain(CvEM* model, const CvMat* samples, const CvMat* sample_idx=0,
                        CvEMParams params=CvEMParams(), CvMat* labels=0 );
extern EMGU_CV_EXTERN_API float CvEMPredict(CvEM* model, const CvMat* sample, CvMat* probs );
extern EMGU_CV_EXTERN_API int CvEMGetNclusters(CvEM* model);
extern EMGU_CV_EXTERN_API const CvMat* CvEMGetMeans(CvEM* model);
extern EMGU_CV_EXTERN_API const CvMat** CvEMGetCovs(CvEM* model);
extern EMGU_CV_EXTERN_API const CvMat* CvEMGetWeights(CvEM* model);
extern EMGU_CV_EXTERN_API const CvMat* CvEMGetProbs(CvEM* model);

//SVM
extern EMGU_CV_EXTERN_API CvSVM* CvSVMDefaultCreate();
extern EMGU_CV_EXTERN_API bool CvSVMTrain(CvSVM* model, const CvMat* _train_data, const CvMat* _responses,
                   const CvMat* _var_idx=0, const CvMat* _sample_idx=0,
                   CvSVMParams _params=CvSVMParams() );
extern EMGU_CV_EXTERN_API bool CvSVMTrainAuto(CvSVM* model, const CvMat* _train_data, const CvMat* _responses,
        const CvMat* _var_idx, const CvMat* _sample_idx, CvSVMParams _params,
        int k_fold = 10,
        CvParamGrid C_grid      = CvSVM::get_default_grid(CvSVM::C),
        CvParamGrid gamma_grid  = CvSVM::get_default_grid(CvSVM::GAMMA),
        CvParamGrid p_grid      = CvSVM::get_default_grid(CvSVM::P),
        CvParamGrid nu_grid     = CvSVM::get_default_grid(CvSVM::NU),
        CvParamGrid coef_grid   = CvSVM::get_default_grid(CvSVM::COEF),
        CvParamGrid degree_grid = CvSVM::get_default_grid(CvSVM::DEGREE) );
extern EMGU_CV_EXTERN_API void CvSVMGetDefaultGrid(int gridType, CvParamGrid* grid);
extern EMGU_CV_EXTERN_API void CvSVMRelease(CvSVM* model);

//ANN_MLP
extern EMGU_CV_EXTERN_API CvANN_MLP* CvANN_MLPCreate(const CvMat* _layer_sizes,
               int _activ_func= CvANN_MLP::SIGMOID_SYM,
               double _f_param1=0, double _f_param2=0 );
extern EMGU_CV_EXTERN_API void CvANN_MLPRelease(CvANN_MLP* model);
extern EMGU_CV_EXTERN_API int CvANN_MLPTrain(CvANN_MLP* model, const CvMat* _inputs, const CvMat* _outputs,
                       const CvMat* _sample_weights, const CvMat* _sample_idx=0,
                       CvANN_MLP_TrainParams _params = CvANN_MLP_TrainParams(),
                       int flags=0 );
extern EMGU_CV_EXTERN_API float CvANN_MLPPredict(CvANN_MLP* model, const CvMat* _inputs,
                           CvMat* _outputs );

//Decision Tree
extern EMGU_CV_EXTERN_API CvDTreeParams* CvDTreeParamsCreate();
extern EMGU_CV_EXTERN_API void CvDTreeParamsRelease(CvDTreeParams* params);

extern EMGU_CV_EXTERN_API CvDTree* CvDTreeCreate();
extern EMGU_CV_EXTERN_API void CvDTreeRelease(CvDTree* model);
extern EMGU_CV_EXTERN_API bool CvDTreeTrain(CvDTree* model, const CvMat* _train_data, int _tflag,
                     const CvMat* _responses, const CvMat* _var_idx=0,
                     const CvMat* _sample_idx=0, const CvMat* _var_type=0,
                     const CvMat* _missing_mask=0,
                     CvDTreeParams params=CvDTreeParams() );
extern EMGU_CV_EXTERN_API CvDTreeNode* predict(CvDTree* model, const CvMat* _sample, const CvMat* _missing_data_mask=0,
                                  bool raw_mode=false );

//Random Tree
extern EMGU_CV_EXTERN_API CvRTParams* CvRTParamsCreate();
extern EMGU_CV_EXTERN_API void CvRTParamsRelease(CvRTParams* params);

extern EMGU_CV_EXTERN_API CvRTrees* CvRTreesCreate();
extern EMGU_CV_EXTERN_API void CvRTreesRelease(CvRTrees* model);
extern EMGU_CV_EXTERN_API bool CvRTreesTrain( CvRTrees* model, const CvMat* _train_data, int _tflag,
                        const CvMat* _responses, const CvMat* _var_idx=0,
                        const CvMat* _sample_idx=0, const CvMat* _var_type=0,
                        const CvMat* _missing_mask=0,
                        CvRTParams params=CvRTParams() );
extern EMGU_CV_EXTERN_API float CvRTreesPredict(CvRTrees* model, const CvMat* sample, const CvMat* missing = 0 );

//CvBoost
extern EMGU_CV_EXTERN_API CvBoost* CvBoostCreate();
extern EMGU_CV_EXTERN_API void CvBoostRelease(CvBoost* model);
#if defined(__cplusplus)
}
#endif
