#include "cvextern.h"

CvRTParams* CvRTParamsCreate()
{
   return new CvRTParams();
}

void CvRTParamsRelease(CvRTParams* params)
{
   delete params;
}

CvRTrees* CvRTreesCreate()
{
   return new CvRTrees();
}

void CvRTreesRelease(CvRTrees* model)
{
   model->~CvRTrees();
}

bool CvRTreesTrain( CvRTrees* model, const CvMat* _train_data, int _tflag,
                        const CvMat* _responses, const CvMat* _var_idx,
                        const CvMat* _sample_idx, const CvMat* _var_type,
                        const CvMat* _missing_mask,
                        CvRTParams params )
{
   return model->train(_train_data, _tflag, _responses, _var_idx, _sample_idx, _var_type, _missing_mask, params);
}

float CvRTreesPredict(CvRTrees* model, const CvMat* sample, const CvMat* missing )
{
   return model->predict(sample, missing);
}