//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "legacy_c.h"

//EMLegacy
CvEM* CvEMLegacyDefaultCreate() 
{ 
   return new CvEM; 
}
void CvEMLegacyRelease(CvEM** model) 
{ 
   delete *model; 
   *model = 0;
}

bool CvEMLegacyTrain(CvEM* model, CvMat* samples, CvMat* sample_idx,
   CvEMParams* params, CvMat* labels )
{ 
   return model->train(samples, sample_idx, *params, labels); 
}
float CvEMLegacyPredict(CvEM* model, CvMat* sample, CvMat* probs )
{ 
   return model->predict(sample, probs); 
}
int CvEMLegacyGetNclusters(CvEM* model) 
{ 
   return model->get_nclusters(); 
}
CvMat* CvEMLegacyGetMeans(CvEM* model) 
{ 
   return (CvMat*) model->get_means(); 
}
CvMat** CvEMLegacyGetCovs(CvEM* model) 
{ 
   return (CvMat**) model->get_covs(); 
}
CvMat* CvEMLegacyGetWeights(CvEM* model) 
{ 
   return (CvMat*) model->get_weights(); 
}
CvMat* CvEMLegacyGetProbs(CvEM* model) 
{ 
   return (CvMat*) model->get_probs(); 
}
