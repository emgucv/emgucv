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

int CvEMGetNclusters(CvEM* model)
{
   return model->get_nclusters();
}

const CvMat* CvEMGetMeans(CvEM* model)
{
   return model->get_means();
}

const CvMat** CvEMGetCovs(CvEM* model)
{
   return model->get_covs();
}

const CvMat* CvEMGetWeights(CvEM* model)
{
   return model->get_weights();
}

const CvMat* CvEMGetProbs(CvEM* model)
{
   return model->get_probs();
}
