#include "ml.h"

//StatModel
CVAPI(void) StatModelSave(CvStatModel* model, const char* filename, const char* name=0 ) { model->save(filename, name); }
CVAPI(void) StatModelLoad(CvStatModel* model, const char* filename, const char* name=0 ) { model->load(filename, name); }
CVAPI(void) StatModelClear(CvStatModel* model) { model->clear(); }

//CvNormalBayesClassifier
CVAPI(CvNormalBayesClassifier*) CvNormalBayesClassifierDefaultCreate() { return new CvNormalBayesClassifier(); }
CVAPI(CvNormalBayesClassifier*) CvNormalBayesClassifierCreate( const CvMat* _train_data, const CvMat* _responses, const CvMat* _var_idx=0, const CvMat* _sample_idx=0 )
{ return new CvNormalBayesClassifier(_train_data, _responses, _var_idx, _sample_idx); }
CVAPI(void) CvNormalBayesClassifierRelease(CvNormalBayesClassifier* classifier) { classifier->~CvNormalBayesClassifier(); }
CVAPI(bool) CvNormalBayesClassifierTrain(CvNormalBayesClassifier* classifier, const CvMat* _train_data, const CvMat* _responses,
                                         const CvMat* _var_idx = 0, const CvMat* _sample_idx=0, bool update=false )
{ return classifier->train(_train_data, _responses, _var_idx, _sample_idx, update); }
CVAPI(float) CvNormalBayesClassifierPredict(CvNormalBayesClassifier* classifier, const CvMat* _samples, CvMat* results=0 )
{ return classifier->predict(_samples, results); }

//KNearest
CVAPI(CvKNearest*) CvKNearestDefaultCreate()
{ return new CvKNearest(); }
CVAPI(bool) CvKNearestTrain(CvKNearest* classifier, const CvMat* _train_data, const CvMat* _responses,
                            const CvMat* _sample_idx=0, bool is_regression=false,
                            int _max_k=32, bool _update_base=false )
{ return classifier->train(_train_data, _responses, _sample_idx, is_regression, _max_k, _update_base); }
CVAPI(CvKNearest*) CvKNearestCreate(const CvMat* _train_data, const CvMat* _responses,
                                    const CvMat* _sample_idx=0, bool _is_regression=false, int max_k=32 )
{ return new CvKNearest(_train_data, _responses, _sample_idx, _is_regression, max_k); }
CVAPI(void) CvKNearestRelease(CvKNearest* classifier) { classifier->~CvKNearest(); }
CVAPI(float) CvKNearestFindNearest(CvKNearest* classifier, const CvMat* _samples, int k, CvMat* results=0,
                                   const float** neighbors=0, CvMat* neighbor_responses=0, CvMat* dist=0 )
{ return classifier->find_nearest(_samples, k, results, neighbors, neighbor_responses, dist); }

//EM
CVAPI(CvEM*) CvEMDefaultCreate() { return new CvEM(); }
CVAPI(void) CvEMRelease(CvEM* model) { model->~CvEM(); }
CVAPI(bool) CvEMTrain(CvEM* model, const CvMat* samples, const CvMat* sample_idx=0,
                      CvEMParams params=CvEMParams(), CvMat* labels=0 )
{ return model->train(samples, sample_idx, params, labels); }
CVAPI(float) CvEMPredict(CvEM* model, const CvMat* sample, CvMat* probs )
{ return model->predict(sample, probs); }
CVAPI(int) CvEMGetNclusters(CvEM* model) { return model->get_nclusters(); }
CVAPI(const CvMat*) CvEMGetMeans(CvEM* model) { return model->get_means(); }
CVAPI(const CvMat**) CvEMGetCovs(CvEM* model) { return model->get_covs(); }
CVAPI(const CvMat*) CvEMGetWeights(CvEM* model) { return model->get_weights(); }
CVAPI(const CvMat*) CvEMGetProbs(CvEM* model) { return model->get_probs(); }

//SVM
CVAPI(CvSVM*) CvSVMDefaultCreate() { return new CvSVM(); }
CVAPI(bool) CvSVMTrain(CvSVM* model, const CvMat* _train_data, const CvMat* _responses,
                       const CvMat* _var_idx=0, const CvMat* _sample_idx=0,
                       CvSVMParams _params=CvSVMParams() )
{ return model->train(_train_data, _responses, _var_idx, _sample_idx, _params); }
CVAPI(bool) CvSVMTrainAuto(CvSVM* model, const CvMat* _train_data, const CvMat* _responses,
                           const CvMat* _var_idx, const CvMat* _sample_idx, CvSVMParams _params,
                           int k_fold = 10,
                           CvParamGrid C_grid      = CvSVM::get_default_grid(CvSVM::C),
                           CvParamGrid gamma_grid  = CvSVM::get_default_grid(CvSVM::GAMMA),
                           CvParamGrid p_grid      = CvSVM::get_default_grid(CvSVM::P),
                           CvParamGrid nu_grid     = CvSVM::get_default_grid(CvSVM::NU),
                           CvParamGrid coef_grid   = CvSVM::get_default_grid(CvSVM::COEF),
                           CvParamGrid degree_grid = CvSVM::get_default_grid(CvSVM::DEGREE) )
{ return model->train_auto(_train_data, _responses, _var_idx, _sample_idx, _params, k_fold,
                           C_grid, gamma_grid, p_grid, nu_grid, coef_grid, degree_grid); }
CVAPI(void) CvSVMGetDefaultGrid(int gridType, CvParamGrid* grid)
{  CvParamGrid defaultGrid = CvSVM::get_default_grid(gridType);
grid->max_val = defaultGrid.max_val;
grid->min_val = defaultGrid.min_val;
grid->step = defaultGrid.step;
}
CVAPI(void) CvSVMRelease(CvSVM* model) { model->~CvSVM(); }
CVAPI(float) CvSVMPredict(CvSVM* model,  const CvMat* _sample )
{ return model->predict(_sample); }
CVAPI(const float*) CvSVMGetSupportVector(CvSVM* model, int i)
{ return  model->get_support_vector(i); }
CVAPI(int) CvSVMGetSupportVectorCount(CvSVM* model)
{ return model->get_support_vector_count(); }
CVAPI(int) CvSVMGetVarCount(CvSVM* model)
{ return model->get_var_count(); }

//ANN_MLP
CVAPI(CvANN_MLP*) CvANN_MLPCreate(const CvMat* _layer_sizes,
                                  int _activ_func= CvANN_MLP::SIGMOID_SYM,
                                  double _f_param1=0, double _f_param2=0 )
{ return new CvANN_MLP(_layer_sizes, _activ_func, _f_param1, _f_param2); }
CVAPI(void) CvANN_MLPRelease(CvANN_MLP* model) { model->~CvANN_MLP(); }
CVAPI(int) CvANN_MLPTrain(CvANN_MLP* model, const CvMat* _inputs, const CvMat* _outputs,
                          const CvMat* _sample_weights, const CvMat* _sample_idx=0,
                          CvANN_MLP_TrainParams _params = CvANN_MLP_TrainParams(),
                          int flags=0 )
{ return model->train(_inputs, _outputs, _sample_weights, _sample_idx, _params, flags); }
CVAPI(float) CvANN_MLPPredict(CvANN_MLP* model, const CvMat* _inputs,
                              CvMat* _outputs )
{ return model->predict(_inputs, _outputs); }
CVAPI(int) CvANN_MLPGetLayerCount(CvANN_MLP* model) { return model->get_layer_count(); }

//Decision Tree
CVAPI(CvDTreeParams*) CvDTreeParamsCreate() { return new CvDTreeParams(); }
CVAPI(void) CvDTreeParamsRelease(CvDTreeParams* params) { delete params; }

CVAPI(CvDTree*) CvDTreeCreate() { return new CvDTree(); }
CVAPI(void) CvDTreeRelease(CvDTree* model) { model->~CvDTree(); }
CVAPI(bool) CvDTreeTrain(CvDTree* model, const CvMat* _train_data, int _tflag,
                         const CvMat* _responses, const CvMat* _var_idx=0,
                         const CvMat* _sample_idx=0, const CvMat* _var_type=0,
                         const CvMat* _missing_mask=0,
                         CvDTreeParams params=CvDTreeParams() )
{ return model->train(_train_data, _tflag, _responses, _var_idx, _sample_idx, _var_type, _missing_mask, params); }
CVAPI(CvDTreeNode*) CvDTreePredict(CvDTree* model, const CvMat* _sample, const CvMat* _missing_data_mask=0, bool raw_mode=false )
{ return model->predict(_sample, _missing_data_mask, raw_mode); }

//Random Tree
CVAPI(CvRTParams*) CvRTParamsCreate() { return new CvRTParams(); }
CVAPI(void) CvRTParamsRelease(CvRTParams* params) { delete params; }

CVAPI(CvRTrees*) CvRTreesCreate() { return new CvRTrees(); }
CVAPI(void) CvRTreesRelease(CvRTrees* model) { model->~CvRTrees(); }
CVAPI(bool) CvRTreesTrain( CvRTrees* model, const CvMat* _train_data, int _tflag,
                          const CvMat* _responses, const CvMat* _var_idx,
                          const CvMat* _sample_idx, const CvMat* _var_type,
                          const CvMat* _missing_mask,
                          CvRTParams params )
{ return model->train(_train_data, _tflag, _responses, _var_idx, _sample_idx, _var_type, _missing_mask, params); }
/*
CVAPI(bool) CvRTreesTrain( CvRTrees* model, const CvMat* _train_data, int _tflag,
                          const CvMat* _responses, const CvMat* _var_idx=0,
                          const CvMat* _sample_idx=0, const CvMat* _var_type=0,
                          const CvMat* _missing_mask=0,
                          CvRTParams params=CvRTParams() )
{ return model->train(_train_data, _tflag, _responses, _var_idx, _sample_idx, _var_type, _missing_mask, params); }
*/
CVAPI(float) CvRTreesPredict(CvRTrees* model, const CvMat* sample, const CvMat* missing ) 
{ return model->predict(sample, missing); }
CVAPI(int) CvRTreesGetTreeCount(CvRTrees* model) { return model->get_tree_count(); }
CVAPI(const CvMat*) CvRTreesGetVarImportance(CvRTrees* model) { return model->get_var_importance(); }

//Extreme Random Tree
CVAPI(CvERTrees*) CvERTreesCreate() { return new CvERTrees(); }
CVAPI(void) CvERTreesRelease(CvERTrees* model) { model->~CvERTrees(); }

//CvBoost
CVAPI(CvBoostParams*) CvBoostParamsCreate() { return new CvBoostParams(); }
CVAPI(void) CvBoostParamsRelease(CvBoostParams* params) { delete params; }

CVAPI(CvBoost*) CvBoostCreate() { return new CvBoost(); }
CVAPI(void) CvBoostRelease(CvBoost* model) { model->~CvBoost(); }
CVAPI(bool) CvBoostTrain(CvBoost* model, const CvMat* _train_data, int _tflag,
                         const CvMat* _responses, const CvMat* _var_idx=0,
                         const CvMat* _sample_idx=0, const CvMat* _var_type=0,
                         const CvMat* _missing_mask=0,
                         CvBoostParams params=CvBoostParams(),
                         bool update=false )
{ return model->train(_train_data, _tflag, _responses, _var_idx,
                      _sample_idx, _var_type, _missing_mask, params, update); }

CVAPI(float) CvBoostPredict(CvBoost* model, const CvMat* _sample, const CvMat* _missing=0,
                            CvMat* weak_responses=0, CvSlice slice=CV_WHOLE_SEQ,
                            bool raw_mode=false )
{ return model->predict(_sample, _missing, weak_responses, slice, raw_mode); }

