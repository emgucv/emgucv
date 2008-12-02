#include "cvextern.h"

CvANN_MLP* CvANN_MLPCreate(const CvMat* _layer_sizes,
               int _activ_func,
               double _f_param1, double _f_param2 )
{
   return new CvANN_MLP(_layer_sizes, _activ_func, _f_param1, _f_param2);
}

void CvANN_MLPRelease(CvANN_MLP* model)
{
   model->~CvANN_MLP();
}

int CvANN_MLPTrain(CvANN_MLP* model, const CvMat* _inputs, const CvMat* _outputs,
                       const CvMat* _sample_weights, const CvMat* _sample_idx,
                       CvANN_MLP_TrainParams _params,
                       int flags)
{
   return model->train(_inputs, _outputs, _sample_weights, _sample_idx, _params, flags);
}

float CvANN_MLPPredict(CvANN_MLP* model, const CvMat* _inputs,
                           CvMat* _outputs )
{
   return model->predict(_inputs, _outputs);
}