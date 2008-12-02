#include "cvextern.h"

CvSVM* CvSVMDefaultCreate()
{
   return new CvSVM();
}

void CvSVMRelease(CvSVM* model)
{
   model->~CvSVM();
}

bool CvSVMTrain(CvSVM* model, const CvMat* _train_data, const CvMat* _responses,
                   const CvMat* _var_idx, const CvMat* _sample_idx,
                   CvSVMParams _params)
{
   return model->train(_train_data, _responses, _var_idx, _sample_idx, _params);
}