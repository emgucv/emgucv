#include "cvextern.h"

CvDTreeParams* CvDTreeParamsCreate()
{
   return new CvDTreeParams();
}

void CvDTreeParamsRelease(CvDTreeParams* params)
{
   delete params;
}

CvDTree* CvDTreeCreate()
{
   return new CvDTree();
}

void CvDTreeRelease(CvDTree* model)
{
   model->~CvDTree();
}

bool CvDTreeTrain(CvDTree* model, const CvMat* _train_data, int _tflag,
                     const CvMat* _responses, const CvMat* _var_idx,
                     const CvMat* _sample_idx, const CvMat* _var_type,
                     const CvMat* _missing_mask,
                     CvDTreeParams params)
{
   return model->train(_train_data, _tflag, _responses, _var_idx, _sample_idx, _var_type, _missing_mask, params);
}

CvDTreeNode* predict(CvDTree* model, const CvMat* _sample, const CvMat* _missing_data_mask,
                                  bool raw_mode )
{
   return model->predict(_sample, _missing_data_mask, raw_mode);
}