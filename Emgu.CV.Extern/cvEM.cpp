#include "cvextern.h"

CvEM* CvEMDefaultCreate()
{
   return new CvEM();
}

void CvEMRelease(CvEM*  model)
{
   model->~CvEM();
}

bool CvEMTrain(CvEM* model, const CvMat* samples, const CvMat* sample_idx,
                        CvEMParams params, CvMat* labels )
{
   return model->train(samples, sample_idx, params, labels);
}

float CvEMPredict(CvEM* model, const CvMat* sample, CvMat* probs )
{
   return model->predict(sample, probs);
}