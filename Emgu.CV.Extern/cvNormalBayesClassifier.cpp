#include "cvextern.h"

CvNormalBayesClassifier* CvNormalBayesClassifierDefaultCreate()
{
   return new CvNormalBayesClassifier();
}

CvNormalBayesClassifier* CvNormalBayesClassifierCreate( const CvMat* _train_data, const CvMat* _responses, const CvMat* _var_idx, const CvMat* _sample_idx )
{
   return new CvNormalBayesClassifier(_train_data, _responses, _var_idx, _sample_idx);
}

void CvNormalBayesClassifierRelease(CvNormalBayesClassifier* classifier)
{
   classifier->~CvNormalBayesClassifier();
}

bool CvNormalBayesClassifierTrain(CvNormalBayesClassifier* classifier, const CvMat* _train_data, const CvMat* _responses,
        const CvMat* _var_idx , const CvMat* _sample_idx, bool update )
{
   return classifier->train(_train_data, _responses, _var_idx, _sample_idx, update);
}

float CvNormalBayesClassifierPredict(CvNormalBayesClassifier* classifier, const CvMat* _samples, CvMat* results )
{
   return classifier->predict(_samples, results);
}