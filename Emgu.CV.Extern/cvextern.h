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
#if defined(__cplusplus)
}
#endif
