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

void CvKNearestRelease(CvKNearest* classifier)
{
   classifier->~CvKNearest();
}

float CvKNearestFindNearest(CvKNearest* classifier, const CvMat* _samples, int k, CvMat* results,
        const float** neighbors, CvMat* neighbor_responses, CvMat* dist )
{
   return classifier->find_nearest(_samples, k, results, neighbors, neighbor_responses, dist);
}