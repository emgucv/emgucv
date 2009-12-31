#include "ml.h"

//StatModel
CVAPI(void) StatModelSave(CvStatModel* model, char* filename, char* name=0 ) { model->save(filename, name); }
CVAPI(void) StatModelLoad(CvStatModel* model, char* filename, char* name=0 ) { model->load(filename, name); }
CVAPI(void) StatModelClear(CvStatModel* model) { model->clear(); }

//CvNormalBayesClassifier
CVAPI(CvNormalBayesClassifier*) CvNormalBayesClassifierDefaultCreate() { return new CvNormalBayesClassifier; }
CVAPI(CvNormalBayesClassifier*) CvNormalBayesClassifierCreate( CvMat* _train_data, CvMat* _responses, CvMat* _var_idx, CvMat* _sample_idx )
{ return new CvNormalBayesClassifier(_train_data, _responses, _var_idx, _sample_idx); }
CVAPI(void) CvNormalBayesClassifierRelease(CvNormalBayesClassifier* classifier) { delete classifier; }
CVAPI(bool) CvNormalBayesClassifierTrain(CvNormalBayesClassifier* classifier, CvMat* _train_data, CvMat* _responses,
                                         CvMat* _var_idx, CvMat* _sample_idx, bool update )
{ return classifier->train(_train_data, _responses, _var_idx, _sample_idx, update); }
CVAPI(float) CvNormalBayesClassifierPredict(CvNormalBayesClassifier* classifier, CvMat* _samples, CvMat* results )
{ return classifier->predict(_samples, results); }

//KNearest
CVAPI(CvKNearest*) CvKNearestDefaultCreate() { return new CvKNearest; }
CVAPI(void) CvKNearestRelease(CvKNearest* classifier) { delete classifier; }
CVAPI(bool) CvKNearestTrain(CvKNearest* classifier, CvMat* _train_data, CvMat* _responses,
                            CvMat* _sample_idx, bool is_regression,
                            int _max_k, bool _update_base)
{ return classifier->train(_train_data, _responses, _sample_idx, is_regression, _max_k, _update_base); }
CVAPI(CvKNearest*) CvKNearestCreate(CvMat* _train_data, CvMat* _responses,
                                    CvMat* _sample_idx, bool _is_regression, int max_k )
{ return new CvKNearest(_train_data, _responses, _sample_idx, _is_regression, max_k); }
CVAPI(float) CvKNearestFindNearest(CvKNearest* classifier, CvMat* _samples, int k, CvMat* results,
                                   float** neighbors, CvMat* neighbor_responses, CvMat* dist )
{ return classifier->find_nearest(_samples, k, results, (const float**) neighbors, neighbor_responses, dist); }

//EM
CVAPI(CvEM*) CvEMDefaultCreate() { return new CvEM; }
CVAPI(void) CvEMRelease(CvEM* model) { delete model; }
CVAPI(bool) CvEMTrain(CvEM* model, CvMat* samples, CvMat* sample_idx,
                      CvEMParams params, CvMat* labels )
{ return model->train(samples, sample_idx, params, labels); }
CVAPI(float) CvEMPredict(CvEM* model, CvMat* sample, CvMat* probs )
{ return model->predict(sample, probs); }
CVAPI(int) CvEMGetNclusters(CvEM* model) { return model->get_nclusters(); }
CVAPI(CvMat*) CvEMGetMeans(CvEM* model) { return (CvMat*) model->get_means(); }
CVAPI(CvMat**) CvEMGetCovs(CvEM* model) { return (CvMat**) model->get_covs(); }
CVAPI(CvMat*) CvEMGetWeights(CvEM* model) { return (CvMat*) model->get_weights(); }
CVAPI(CvMat*) CvEMGetProbs(CvEM* model) { return (CvMat*) model->get_probs(); }

//SVM
CVAPI(CvSVM*) CvSVMDefaultCreate() { return new CvSVM; }
CVAPI(bool) CvSVMTrain(CvSVM* model, CvMat* _train_data, CvMat* _responses,
                       CvMat* _var_idx, CvMat* _sample_idx,
                       CvSVMParams _params)
{ return model->train(_train_data, _responses, _var_idx, _sample_idx, _params); }
CVAPI(bool) CvSVMTrainAuto(CvSVM* model, CvMat* _train_data, CvMat* _responses,
                           CvMat* _var_idx, CvMat* _sample_idx, CvSVMParams _params,
                           int k_fold,
                           CvParamGrid C_grid,
                           CvParamGrid gamma_grid,
                           CvParamGrid p_grid,
                           CvParamGrid nu_grid,
                           CvParamGrid coef_grid,
                           CvParamGrid degree_grid)
{ return model->train_auto(_train_data, _responses, _var_idx, _sample_idx, _params, k_fold,
                           C_grid, gamma_grid, p_grid, nu_grid, coef_grid, degree_grid); }
CVAPI(void) CvSVMGetDefaultGrid(int gridType, CvParamGrid* grid)
{  CvParamGrid defaultGrid = CvSVM::get_default_grid(gridType);
grid->max_val = defaultGrid.max_val;
grid->min_val = defaultGrid.min_val;
grid->step = defaultGrid.step;
}
CVAPI(void) CvSVMRelease(CvSVM* model) { delete model; }
CVAPI(float) CvSVMPredict(CvSVM* model,  CvMat* _sample )
{ return model->predict(_sample); }
CVAPI(float*) CvSVMGetSupportVector(CvSVM* model, int i)
{ return (float*) model->get_support_vector(i); }
CVAPI(int) CvSVMGetSupportVectorCount(CvSVM* model)
{ return model->get_support_vector_count(); }
CVAPI(int) CvSVMGetVarCount(CvSVM* model)
{ return model->get_var_count(); }
CVAPI(void) CvSVMGetParameters(CvSVM* model, CvSVMParams* param) { CvSVMParams p = model->get_params(); memcpy(param, &p, sizeof(CvSVMParams)); }

//ANN_MLP
CVAPI(CvANN_MLP*) CvANN_MLPCreate(CvMat* _layer_sizes,
                                  int _activ_func,
                                  double _f_param1, double _f_param2 )
{ return new CvANN_MLP(_layer_sizes, _activ_func, _f_param1, _f_param2); }
CVAPI(void) CvANN_MLPRelease(CvANN_MLP* model) { delete model; }
CVAPI(int) CvANN_MLPTrain(CvANN_MLP* model, CvMat* _inputs, CvMat* _outputs,
                          CvMat* _sample_weights, CvMat* _sample_idx,
                          CvANN_MLP_TrainParams* _params,
                          int flags)
{ return model->train(_inputs, _outputs, _sample_weights, _sample_idx, *_params, flags); }
CVAPI(float) CvANN_MLPPredict(CvANN_MLP* model, CvMat* _inputs,
                              CvMat* _outputs )
{ return model->predict(_inputs, _outputs); }
CVAPI(int) CvANN_MLPGetLayerCount(CvANN_MLP* model) { return model->get_layer_count(); }

//Decision Tree
CVAPI(CvDTreeParams*) CvDTreeParamsCreate() { return new CvDTreeParams; }
CVAPI(void) CvDTreeParamsRelease(CvDTreeParams* params) { delete params; }

CVAPI(CvDTree*) CvDTreeCreate() { return new CvDTree; }
CVAPI(void) CvDTreeRelease(CvDTree* model) { delete model; }
CVAPI(bool) CvDTreeTrain(CvDTree* model, CvMat* _train_data, int _tflag,
                         CvMat* _responses, CvMat* _var_idx,
                         CvMat* _sample_idx, CvMat* _var_type,
                         CvMat* _missing_mask,
                         CvDTreeParams params )
{ return model->train(_train_data, _tflag, _responses, _var_idx, _sample_idx, _var_type, _missing_mask, params); }
CVAPI(CvDTreeNode*) CvDTreePredict(CvDTree* model, CvMat* _sample, CvMat* _missing_data_mask, bool raw_mode )
{ return model->predict(_sample, _missing_data_mask, raw_mode); }

//Random Tree
CVAPI(CvRTParams*) CvRTParamsCreate() { return new CvRTParams(); }
CVAPI(void) CvRTParamsRelease(CvRTParams* params) { delete params; }

CVAPI(CvRTrees*) CvRTreesCreate() { return new CvRTrees(); }
CVAPI(void) CvRTreesRelease(CvRTrees* model) { delete model; }
CVAPI(bool) CvRTreesTrain( CvRTrees* model, CvMat* _train_data, int _tflag,
                          CvMat* _responses, CvMat* _var_idx,
                          CvMat* _sample_idx, CvMat* _var_type,
                          CvMat* _missing_mask,
                          CvRTParams params )
{ return model->train(_train_data, _tflag, _responses, _var_idx, _sample_idx, _var_type, _missing_mask, params); }
/*
CVAPI(bool) CvRTreesTrain( CvRTrees* model, CvMat* _train_data, int _tflag,
                          CvMat* _responses, CvMat* _var_idx=0,
                          CvMat* _sample_idx=0, CvMat* _var_type=0,
                          CvMat* _missing_mask=0,
                          CvRTParams params=CvRTParams() )
{ return model->train(_train_data, _tflag, _responses, _var_idx, _sample_idx, _var_type, _missing_mask, params); }
*/
CVAPI(float) CvRTreesPredict(CvRTrees* model, CvMat* sample, CvMat* missing ) 
{ return model->predict(sample, missing); }
CVAPI(int) CvRTreesGetTreeCount(CvRTrees* model) { return model->get_tree_count(); }
CVAPI(CvMat*) CvRTreesGetVarImportance(CvRTrees* model) { return (CvMat*) model->get_var_importance(); }

//Extreme Random Tree
CVAPI(CvERTrees*) CvERTreesCreate() { return new CvERTrees(); }
CVAPI(void) CvERTreesRelease(CvERTrees* model) { delete model; }

//CvBoost
CVAPI(CvBoostParams*) CvBoostParamsCreate() { return new CvBoostParams(); }
CVAPI(void) CvBoostParamsRelease(CvBoostParams* params) { delete params; }

CVAPI(CvBoost*) CvBoostCreate() { return new CvBoost(); }
CVAPI(void) CvBoostRelease(CvBoost* model) { delete model; }
CVAPI(bool) CvBoostTrain(CvBoost* model, CvMat* _train_data, int _tflag,
                         CvMat* _responses, CvMat* _var_idx,
                         CvMat* _sample_idx, CvMat* _var_type,
                         CvMat* _missing_mask,
                         CvBoostParams params,
                         bool update )
{ return model->train(_train_data, _tflag, _responses, _var_idx,
                      _sample_idx, _var_type, _missing_mask, params, update); }

CVAPI(float) CvBoostPredict(CvBoost* model, CvMat* _sample, CvMat* _missing=0,
                            CvMat* weak_responses=0, CvSlice slice=CV_WHOLE_SEQ,
                            bool raw_mode=false )
{ return model->predict(_sample, _missing, weak_responses, slice, raw_mode); }

