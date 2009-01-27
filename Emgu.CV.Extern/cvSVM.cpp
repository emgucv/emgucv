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

void CvSVMGetDefaultGrid(int gridType, CvParamGrid* grid)
{
   CvParamGrid defaultGrid = CvSVM::get_default_grid(gridType);
   grid->max_val = defaultGrid.max_val;
   grid->min_val = defaultGrid.min_val;
   grid->step = defaultGrid.step;
}

bool CvSVMTrainAuto(CvSVM* model, const CvMat* _train_data, const CvMat* _responses,
        const CvMat* _var_idx, const CvMat* _sample_idx, CvSVMParams _params,
        int k_fold,
        CvParamGrid C_grid,
        CvParamGrid gamma_grid,
        CvParamGrid p_grid,
        CvParamGrid nu_grid,
        CvParamGrid coef_grid,
        CvParamGrid degree_grid)
{
   return model->train_auto(_train_data, _responses, _var_idx, _sample_idx, _params, k_fold,
      C_grid, gamma_grid, p_grid, nu_grid, coef_grid, degree_grid);
}

float cvSVMPredict(CvSVM* model, const CvMat* _sample )
{
    return model->predict(_sample);
}
const float* cvSVMGetSupportVector(CvSVM* model, int i)
{
    return  model->get_support_vector(i);
}
int cvSVMGetSupportVectorCount(CvSVM* model)
{
    return model->get_support_vector_count();
}

int cvSVMGetVarCount(CvSVM* model)
{
    return model->get_var_count();
}
