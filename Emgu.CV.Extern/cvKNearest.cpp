#include "cvextern.h"

CvKNearest* CvKNearestDefaultCreate()
{
   return new CvKNearest();
}

CvKNearest* CvKNearestCreate(const CvMat* _train_data, const CvMat* _responses,
                const CvMat* _sample_idx, bool _is_regression, int max_k )
{
   return new CvKNearest(_train_data, _responses, _sample_idx, _is_regression, max_k);
}

bool CvKNearestTrain(CvKNearest* model, const CvMat* _train_data, const CvMat* _responses,
                        const CvMat* _sample_idx, bool is_regression,
                        int _max_k, bool _update_base )
{
   return model->train(_train_data, _responses, _sample_idx, is_regression, _max_k, _update_base);
}

void CvKNearestRelease(CvKNearest* classifier)
{
   classifier->~CvKNearest();
}

float CvKNearestFindNearest(CvKNearest* classifier, const CvMat* _samples, int k, CvMat* results,
        const float** neighbors, CvMat* neighbor_responses, CvMat* dist )
{
   return classifier->find_nearest(_samples, k, results, neighbors, neighbor_responses, dist);
}