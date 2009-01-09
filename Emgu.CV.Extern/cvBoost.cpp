#include "cvextern.h"

CvBoostParams* CvBoostParamsCreate()
{
   return new CvBoostParams();
}

void CvBoostParamsRelease(CvBoostParams* params)
{
   delete params;
}

CvBoost* CvBoostCreate()
{
   return new CvBoost();
}

void CvBoostRelease(CvBoost* model)
{
   model->~CvBoost();
}

bool CvBoostTrain(CvBoost* model, const CvMat* _train_data, int _tflag,
             const CvMat* _responses, const CvMat* _var_idx,
             const CvMat* _sample_idx, const CvMat* _var_type,
             const CvMat* _missing_mask,
             CvBoostParams params,
             bool update)
{
   return model->train(_train_data, _tflag, _responses, _var_idx,
             _sample_idx, _var_type, _missing_mask, params, update);
}

float CvBoostPredict(CvBoost* model, const CvMat* _sample, const CvMat* _missing,
                           CvMat* weak_responses, CvSlice slice,
                           bool raw_mode)
{
   return model->predict(_sample, _missing, weak_responses, slice, raw_mode);
}