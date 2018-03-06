//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "core_c_extra.h"

CvErrorCallback cveRedirectError(CvErrorCallback error_handler, void* userdata, void** prev_userdata)
{
   return cvRedirectError(error_handler, userdata, prev_userdata);
}

int cveGetErrMode()
{
   return cvGetErrMode();
}
int cveSetErrMode(int mode)
{
   return cvSetErrMode(mode);
}
int cveGetErrStatus()
{
   return cvGetErrStatus();
}
void cveSetErrStatus(int status)
{
   cvSetErrStatus(status); 
}
const char* cveErrorStr(int status)
{
   return cvErrorStr(status);
}

int cveGetThreadNum()
{
   return cv::getThreadNum();
}
void cveSetNumThreads(int nthreads)
{
   cv::setNumThreads(nthreads);
}
int cveGetNumThreads()
{
   return cv::getNumThreads();
}

cv::String* cveStringCreate()
{
   return new cv::String();
}
cv::String* cveStringCreateFromStr(const char* c)
{
   return new cv::String(c);
}
void cveStringGetCStr(cv::String* string, const char** c, int* size)
{
   *c = string->c_str();
   *size = static_cast<int>(string->size());
}
int cveStringGetLength(cv::String* string)
{
   return static_cast<int>(string->size());
}
void cveStringRelease(cv::String** string)
{
   delete *string;
   *string = 0;
}

cv::_InputArray* cveInputArrayFromDouble(double* scalar)
{
   return new cv::_InputArray(*scalar);
}

cv::_InputArray* cveInputArrayFromScalar(cv::Scalar* scalar)
{
   return new cv::_InputArray(*scalar);
}
cv::_InputArray* cveInputArrayFromMat(cv::Mat* mat)
{
   return new cv::_InputArray(*mat);
}

cv::_InputArray* cveInputArrayFromGpuMat(cv::cuda::GpuMat* mat)
{
   return new cv::_InputArray(*mat);
}

cv::_InputArray* cveInputArrayFromUMat(cv::UMat* mat)
{
   return new cv::_InputArray(*mat);
}

int cveInputArrayGetDims(cv::_InputArray* ia, int i)
{
   return ia->dims(i);
}

void cveInputArrayGetSize(cv::_InputArray* ia, CvSize* size, int idx)
{
   cv::Size s = ia->size(idx);
   *size = s;
}
int cveInputArrayGetDepth(cv::_InputArray* ia, int idx)
{
   return ia->depth(idx);
}
int cveInputArrayGetChannels(cv::_InputArray* ia, int idx)
{
   return ia->channels(idx);
}
bool cveInputArrayIsEmpty(cv::_InputArray* ia)
{
   return ia->empty();
}
void cveInputArrayRelease(cv::_InputArray** arr)
{
   delete *arr;
   *arr = 0;
}

void cveInputArrayGetMat(cv::_InputArray* ia, int idx, cv::Mat* mat)
{
   cv::Mat m = ia->getMat(idx);
   cv::swap(m, *mat);
}
void cveInputArrayGetUMat(cv::_InputArray* ia, int idx, cv::UMat* umat)
{
   cv::UMat m = ia->getUMat(idx);
   cv::swap(m, *umat);
}
void cveInputArrayGetGpuMat(cv::_InputArray* ia, cv::cuda::GpuMat* gpuMat)
{
   cv::cuda::GpuMat m = ia->getGpuMat();
   cv::swap(m, *gpuMat);
}

void cveInputArrayCopyTo(cv::_InputArray* ia, cv::_OutputArray* arr, cv::_InputArray* mask)
{
   if (mask)
      ia->copyTo(*arr, *mask);
   else
      ia->copyTo(*arr);
}


cv::_OutputArray* cveOutputArrayFromMat(cv::Mat* mat)
{
   return new cv::_OutputArray(*mat);
}

cv::_OutputArray* cveOutputArrayFromGpuMat(cv::cuda::GpuMat* mat)
{
   return new cv::_OutputArray(*mat);
}

cv::_OutputArray* cveOutputArrayFromUMat(cv::UMat* mat)
{
   return new cv::_OutputArray(*mat);
}

void cveOutputArrayRelease(cv::_OutputArray** arr)
{
   delete *arr;
   *arr = 0;
}

cv::_InputOutputArray* cveInputOutputArrayFromMat(cv::Mat* mat)
{
   return new cv::_InputOutputArray(*mat);
}
cv::_InputOutputArray* cveInputOutputArrayFromUMat(cv::UMat* mat)
{
   return new cv::_InputOutputArray(*mat);
}
cv::_InputOutputArray* cveInputOutputArrayFromGpuMat(cv::cuda::GpuMat* mat)
{
   return new cv::_InputOutputArray(*mat);
}
void cveInputOutputArrayRelease(cv::_InputOutputArray** arr)
{
   delete *arr;
   *arr = 0;
}

cv::Scalar* cveScalarCreate(CvScalar* scalar)
{
   return new cv::Scalar(scalar->val[0], scalar->val[1], scalar->val[2], scalar->val[3]);
}
void cveScalarRelease(cv::Scalar** scalar)
{
   delete *scalar;
   *scalar = 0;
}

void cveMinMaxIdx(cv::_InputArray* src, double* minVal, double* maxVal, int* minIdx, int* maxIdx, cv::_InputArray* mask)
{
   cv::minMaxIdx(*src, minVal, maxVal, minIdx, maxIdx, mask ? *mask : (cv::InputArray) cv::noArray());
}

void cveMinMaxLoc(cv::_InputArray* src, double* minVal, double* maxVal, CvPoint* minLoc, CvPoint* maxLoc, cv::_InputArray* mask)
{
   cv::Point minPt;
   cv::Point maxPt;
   cv::minMaxLoc(*src, minVal, maxVal, &minPt, &maxPt, mask ? *mask : (cv::InputArray) cv::noArray());
   minLoc->x = minPt.x; minLoc->y = minPt.y;
   maxLoc->x = maxPt.x; maxLoc->y = maxPt.y;
}

void cveBitwiseAnd(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask)
{
   cv::bitwise_and(*src1, *src2, *dst, mask ? *mask : (cv::InputArray) cv::noArray());
}
void cveBitwiseNot(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* mask)
{
   cv::bitwise_not(*src, *dst, mask ? *mask : (cv::InputArray) cv::noArray());
}
void cveBitwiseOr(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask)
{
   cv::bitwise_or(*src1, *src2, *dst, mask ? *mask : (cv::InputArray) cv::noArray());
}
void cveBitwiseXor(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask)
{
   cv::bitwise_xor(*src1, *src2, *dst, mask ? *mask : (cv::InputArray) cv::noArray());
}

void cveAdd(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask, int dtype)
{
   cv::add(*src1, *src2, *dst, mask ? *mask : (cv::InputArray) cv::noArray(), dtype);
}
void cveSubtract(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask, int dtype)
{
   cv::subtract(*src1, *src2, *dst, mask ? *mask : (cv::InputArray) cv::noArray(), dtype);
}
void cveDivide(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, double scale, int dtype)
{
   cv::divide(*src1, *src2, *dst, scale, dtype);
}
void cveMultiply(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, double scale, int dtype)
{
   cv::multiply(*src1,*src2, *dst, scale, dtype);
}
void cveCountNonZero(cv::_InputArray* src)
{
   cv::countNonZero(*src);
}
void cveFindNonZero(cv::_InputArray* src, cv::_OutputArray* idx )
{
   cv::findNonZero(*src, *idx);
}
void cveMin(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst)
{
   cv::min(*src1, *src2, *dst);
}
void cveMax(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst)
{
   cv::max(*src1, *src2, *dst);
}
void cveAbsDiff(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst)
{
   cv::absdiff(*src1, *src2, *dst);
}
void cveInRange(cv::_InputArray* src1, cv::_InputArray* lowerb, cv::_InputArray* upperb, cv::_OutputArray* dst)
{
   cv::inRange(*src1, *lowerb, *upperb, *dst);
}
void cveSqrt(cv::_InputArray* src, cv::_OutputArray* dst)
{
   cv::sqrt(*src, *dst);
}

void cveCompare(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, int compop)
{
   cv::compare(*src1, *src2, *dst, compop);
}

void cveFlip(cv::_InputArray* src, cv::_OutputArray* dst, int flipCode)
{
   cv::flip(*src, *dst, flipCode);
}

void cveRotate(cv::_InputArray* src, cv::_OutputArray* dst, int rotateCode)
{
	cv::rotate(*src, *dst, rotateCode);
}

void cveTranspose(cv::_InputArray* src, cv::_OutputArray* dst)
{
   cv::transpose(*src, *dst);
}

void cveLUT(cv::_InputArray* src, cv::_InputArray* lut, cv::_OutputArray* dst)
{
   cv::LUT(*src, *lut, *dst);
}

void cveSum(cv::_InputArray* src, CvScalar* result)
{
   cv::Scalar sum = cv::sum(*src);
   memcpy(&result->val[0], &sum.val[0], sizeof(double) * 4);
}
void cveMean(cv::_InputArray* src, cv::_InputArray* mask, CvScalar* result)
{
   cv::Scalar mean = cv::mean(*src, mask ? *mask : (cv::InputArray) cv::noArray());
   memcpy(&result->val[0], &mean.val[0], sizeof(double) * 4);
}
void cveMeanStdDev(cv::_InputArray* src, cv::_OutputArray* mean, cv::_OutputArray* stddev, cv::_InputArray* mask)
{
   cv::meanStdDev(*src, *mean, *stddev, mask ? *mask : (cv::InputArray) cv::noArray());
}
void cveTrace(cv::_InputArray* mtx, CvScalar* result)
{
   cv::Scalar trace = cv::trace(*mtx);
   memcpy(&result->val[0], &trace.val[0], sizeof(double) * 4);
}
double cveDeterminant(cv::_InputArray* mtx)
{
   return cv::determinant(*mtx);
}
double cveNorm(cv::_InputArray* src1, cv::_InputArray* src2, int normType, cv::_InputArray* mask)
{
   if (src2)
   {
      return cv::norm(*src1, *src2, normType, mask ? *mask : (cv::InputArray) cv::noArray());
   } else
   {
      return cv::norm(*src1, normType, mask ? *mask : (cv::InputArray) cv::noArray());
   }
}
bool cveCheckRange(cv::_InputArray* arr, bool quiet, CvPoint* index, double minVal, double maxVal)
{
   cv::Point p;
   bool result = cv::checkRange(*arr, quiet, &p , minVal, maxVal);
   index->x = p.x;
   index->y = p.y;
   return result;
}
void cvePatchNaNs(cv::_InputOutputArray* a, double val)
{
	cv::patchNaNs(*a, val);
}

void cveGemm(cv::_InputArray* src1, cv::_InputArray* src2, double alpha, cv::_InputArray* src3, double beta, cv::_OutputArray* dst, int flags)
{
   cv::gemm(*src1, *src2, alpha, src3 ? *src3: (cv::InputArray) cv::noArray(), beta, *dst, flags);
}

void cveAddWeighted(cv::_InputArray* src1, double alpha, cv::_InputArray* src2, double beta, double gamma, cv::_OutputArray* dst, int dtype)
{
   cv::addWeighted(*src1, alpha, *src2, beta, gamma, *dst, dtype);
}
void cveConvertScaleAbs(cv::_InputArray* src, cv::_OutputArray* dst, double alpha, double beta)
{
   cv::convertScaleAbs(*src, *dst, alpha, beta);
}
void cveReduce(cv::_InputArray* src, cv::_OutputArray* dst, int dim, int rtype, int dtype)
{
   cv::reduce(*src, *dst, dim, rtype, dtype);
}
void cveRandShuffle(cv::_InputOutputArray* dst, double iterFactor, uint64 rng)
{
   if (rng == 0)
   {
      cv::randShuffle(*dst, iterFactor);
   } else
   {
      cv::RNG r(rng);
      cv::randShuffle(*dst, iterFactor, &r);
   }
}
void cvePow(cv::_InputArray* src, double power, cv::_OutputArray* dst)
{
   cv::pow(*src, power, *dst);
}
void cveExp(cv::_InputArray* src, cv::_OutputArray* dst)
{
   cv::exp(*src, *dst);
}
void cveLog(cv::_InputArray* src, cv::_OutputArray* dst)
{
   cv::log(*src, *dst);
}
void cveCartToPolar(cv::_InputArray* x, cv::_InputArray* y, cv::_OutputArray* magnitude, cv::_OutputArray* angle, bool angleInDegrees)
{
   cv::cartToPolar(*x, *y, *magnitude, angle ?  *angle : (cv::OutputArray) cv::noArray(), angleInDegrees);
}
void cvePolarToCart(cv::_InputArray* magnitude, cv::_InputArray* angle, cv::_OutputArray* x, cv::_OutputArray* y, bool angleInDegrees)
{
   cv::polarToCart(*magnitude, *angle, *x, *y, angleInDegrees);
}
void cveSetIdentity(cv::_InputOutputArray* mtx, CvScalar* scalar)
{
   cv::setIdentity(*mtx, *scalar);
}
int cveSolveCubic(cv::_InputArray* coeffs, cv::_OutputArray* roots)
{
   return cv::solveCubic(*coeffs, *roots);
}
double cveSolvePoly(cv::_InputArray* coeffs, cv::_OutputArray* roots, int maxIters)
{
   return cv::solvePoly(*coeffs, *roots, maxIters);
}
void cveSolve(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, int flags)
{
   cv::solve(*src1, *src2, *dst, flags);
}
void cveSort(cv::_InputArray* src, cv::_OutputArray* dst, int flags)
{
	cv::sort(*src, *dst, flags);
}
void cveSortIdx(cv::_InputArray* src, cv::_OutputArray* dst, int flags)
{
	cv::sortIdx(*src, *dst, flags);
}
void cveInvert(cv::_InputArray* src, cv::_OutputArray* dst, int flags)
{
   cv::invert(*src, *dst, flags);
}

void cveDft(cv::_InputArray* src, cv::_OutputArray* dst, int flags, int nonzeroRows)
{
   cv::dft(*src, *dst, flags, nonzeroRows);
}
void cveDct(cv::_InputArray* src, cv::_OutputArray* dst, int flags)
{
   cv::dct(*src, *dst, flags);
}
void cveMulSpectrums(cv::_InputArray *a, cv::_InputArray* b, cv::_OutputArray* c, int flags, bool conjB)
{
   cv::mulSpectrums(*a, *b, *c, flags, conjB);
}

int cveGetOptimalDFTSize(int vecsize)
{
   return cv::getOptimalDFTSize(vecsize);
}

void cveTransform(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* m)
{
   cv::transform(*src, *dst, *m);
}

void cveMahalanobis(cv::_InputArray* v1, cv::_InputArray* v2, cv::_InputArray* icovar)
{
   cv::Mahalanobis(*v1, *v2, *icovar);
}

void cveCalcCovarMatrix(cv::_InputArray* samples, cv::_OutputArray* covar, cv::_InputOutputArray* mean, int flags, int ctype)
{
   cv::calcCovarMatrix(*samples, *covar, *mean, flags, ctype);
}

void cveNormalize(cv::_InputArray* src, cv::_InputOutputArray* dst, double alpha, double beta, int normType, int dType, cv::_InputArray* mask)
{
   cv::normalize(*src, *dst, alpha, beta, normType, dType, mask ? *mask : (cv::InputArray) cv::noArray());
}

void cvePerspectiveTransform(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* m)
{
   cv::perspectiveTransform(*src, *dst, *m);
}

void cveMulTransposed(cv::_InputArray* src, cv::_OutputArray* dst, bool aTa, cv::_InputArray* delta, double scale, int dtype)
{
   cv::mulTransposed(*src, *dst, aTa, delta ? *delta : (cv::InputArray) cv::noArray(), dtype);
}

void cveSplit(cv::_InputArray* src, cv::_OutputArray* mv)
{
   cv::split(*src, *mv);
}
void cveMerge(cv::_InputArray* mv, cv::_OutputArray* dst)
{
   cv::merge(*mv, *dst);
}
void cveMixChannels(cv::_InputArray* src, cv::_InputOutputArray* dst, const int* fromTo, int npairs)
{
   cv::mixChannels(*src, *dst, fromTo, npairs);
}

void cveExtractChannel(cv::_InputArray* src, cv::_OutputArray* dst, int coi)
{
   cv::extractChannel(*src, *dst, coi);
}
void cveInsertChannel(cv::_InputArray* src, cv::_InputOutputArray* dst, int coi)
{
   cv::insertChannel(*src, *dst, coi);
}


double cveKmeans(cv::_InputArray* data, int k, cv::_InputOutputArray* bestLabels, CvTermCriteria* criteria, int attempts, int flags, cv::_OutputArray* centers)
{
   return cv::kmeans(*data, k, *bestLabels, *criteria, attempts, flags, centers ? *centers : (cv::OutputArray) cv::noArray());
}

void cveHConcat(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst)
{
   cv::hconcat(*src1, *src2, *dst);
}
void cveVConcat(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst)
{
   cv::vconcat(*src1, *src2, *dst);
}


double cvePSNR(cv::_InputArray* src1, cv::_InputArray* src2)
{
   return cv::PSNR(*src1, *src2);
}

bool cveEigen(cv::_InputArray* src, cv::_OutputArray* eigenValues, cv::_OutputArray* eigenVectors)
{
   return cv::eigen(*src, *eigenValues, eigenVectors ? *eigenVectors : (cv::OutputArray) cv::noArray());
}

//Algorithm 
void cveAlgorithmRead(cv::Algorithm* algorithm, cv::FileNode* node)
{
   algorithm->read(*node);
}

void cveAlgorithmWrite(cv::Algorithm* algorithm, cv::FileStorage* storage)
{
   algorithm->write(*storage);
}

void cveAlgorithmSave(cv::Algorithm* algorithm, cv::String* filename)
{
	algorithm->save(*filename);
}

void cveAlgorithmClear(cv::Algorithm* algorithm)
{
	algorithm->clear();
}

bool cveAlgorithmEmpty(cv::Algorithm* algorithm)
{
	return algorithm->empty();
}

void cveAlgorithmGetDefaultName(cv::Algorithm* algorithm, cv::String* defaultName)
{
	cv::String name = algorithm->getDefaultName();
	*defaultName = name;
}

bool cveClipLine(CvRect* rect, CvPoint* pt1, CvPoint* pt2)
{
   cv::Point p1 = *pt1, p2 = *pt2;
   bool r = cv::clipLine(*rect, p1, p2);
   pt1->x = p1.x; pt1->y = p1.y;
   pt2->x = p2.x; pt2->y = p2.y;
   return r;
}

void cveRandn(cv::_InputOutputArray* dst, cv::_InputArray* mean, cv::_InputArray* stddev)
{
   cv::randn(*dst, *mean, *stddev);
}

void cveRandu(cv::_InputOutputArray* dst, cv::_InputArray* low, cv::_InputArray* high)
{
   cv::randu(*dst, *low, *high);
}

//File Storage
cv::FileStorage* cveFileStorageCreate(const cv::String* source, int flags, const cv::String* encoding)
{
   return new cv::FileStorage(*source, flags, *encoding);
}
bool cveFileStorageIsOpened(cv::FileStorage* storage)
{
   return storage->isOpened();
}
void cveFileStorageReleaseAndGetString(cv::FileStorage* storage, cv::String* result)
{
   cv::String res = storage->releaseAndGetString();
   res.swap(*result);
}
void cveFileStorageRelease(cv::FileStorage** storage)
{
   delete *storage;
   *storage = 0;
}
void cveFileStorageWriteMat(cv::FileStorage* fs, cv::String* name, cv::Mat* value)
{
   cv::write(*fs, *name, *value);
}
void cveFileStorageWriteInt(cv::FileStorage* fs, cv::String* name, int value)
{
   cv::write(*fs, *name, value);
}
void cveFileStorageWriteFloat(cv::FileStorage* fs, cv::String* name, float value)
{
      cv::write(*fs, *name, value);  
}
void cveFileStorageWriteDouble(cv::FileStorage* fs, cv::String* name, double value)
{
   cv::write(*fs, *name, value);
}
void cveFileStorageWriteString(cv::FileStorage* fs, cv::String* name, cv::String* value)
{
   cv::write(*fs, *name, *value);
}
void cveFileStorageInsertString(cv::FileStorage* fs, cv::String* value)
{
	(*fs) << *value;
}

cv::FileNode* cveFileStorageRoot(cv::FileStorage* fs, int streamIdx)
{
   cv::FileNode* n = new cv::FileNode();
   cv::FileNode root = fs->root(streamIdx);
   *n = root;
   return n;
}
cv::FileNode* cveFileStorageGetFirstTopLevelNode(cv::FileStorage* fs)
{
   cv::FileNode* n = new cv::FileNode();
   cv::FileNode root = fs->getFirstTopLevelNode();
   *n = root;
   return n;
}
cv::FileNode* cveFileStorageGetNode(cv::FileStorage* fs, cv::String* nodeName)
{
   cv::FileNode* n = new cv::FileNode();
   cv::FileNode root = (*fs)[*nodeName];
   *n = root;
   return n;
}

//File Node
void cveFileNodeReadMat(cv::FileNode* node, cv::Mat* mat, cv::Mat* defaultMat)
{
   cv::read(*node, *mat, *defaultMat);
}
int cveFileNodeGetType(cv::FileNode* node)
{
   return node->type();
}
bool cveFileNodeIsEmpty(cv::FileNode* node)
{
   return node->empty();
}
void cveFileNodeReadString(cv::FileNode* node, cv::String* str, cv::String* defaultStr)
{
   cv::read(*node, *str, *defaultStr);
}
int cveFileNodeReadInt(cv::FileNode* node, int defaultInt)
{
   int result = 0;
   cv::read(*node, result, defaultInt);
   return result;
}
double cveFileNodeReadDouble(cv::FileNode* node, double defaultDouble)
{
   double result = 0;
   cv::read(*node, result, defaultDouble);
   return result;
}
float cveFileNodeReadFloat(cv::FileNode* node, float defaultFloat)
{
   float result = 0;
   cv::read(*node, result, defaultFloat);
   return result;
}
void cveFileNodeRelease(cv::FileNode** node)
{
   delete *node;
   *node = 0;
}

IplImage* cveCreateImage(CvSize* size, int depth, int channels)
{
   return cvCreateImage(*size, depth, channels);
}
IplImage* cveCreateImageHeader(CvSize* size, int depth, int channels)
{
   return cvCreateImageHeader(*size, depth, channels);
}
IplImage* cveInitImageHeader(IplImage* image, CvSize* size, int depth, int channels, int origin, int align)
{
   return cvInitImageHeader(image, *size, depth, channels, origin, align);
}
void cveSetData(CvArr* arr, void* data, int step)
{
   cvSetData(arr, data, step);
}
void cveReleaseImageHeader(IplImage** image)
{
   cvReleaseImageHeader(image);
}
void cveSetImageCOI(IplImage* image, int coi)
{
   cvSetImageCOI(image, coi);
}
int cveGetImageCOI(IplImage* image)
{
   return cvGetImageCOI(image);
}
void cveResetImageROI(IplImage* image)
{
   cvResetImageROI(image);
}
void cveSetImageROI(IplImage* image, CvRect* rect)
{
   cvSetImageROI(image, *rect);
}
void cveGetImageROI(IplImage* image, CvRect* rect)
{
   CvRect rect2 = cvGetImageROI(image);
   memcpy(rect, &rect2, sizeof(CvRect));
}

CvMat* cveInitMatHeader(CvMat* mat, int rows, int cols, int type, void* data, int step)
{
   return cvInitMatHeader(mat, rows, cols, type, data, step);
}

CvMat* cveCreateMat(int rows, int cols, int type)
{
   return cvCreateMat(rows, cols, type);
}

CvMatND* cveInitMatNDHeader(CvMatND* mat, int dims, int* sizes, int type, void* data)
{
   return cvInitMatNDHeader(mat, dims, sizes, type, data);
}

void cveReleaseMat(CvMat** mat)
{
   cvReleaseMat(mat);
}

CvSparseMat* cveCreateSparseMat(int dim, int* sizes, int type)
{
   return cvCreateSparseMat(dim, sizes, type);
}
void cveReleaseSparseMat(CvSparseMat** mat)
{
   cvReleaseSparseMat(mat);
}

void cveSet2D(CvArr* arr, int idx0, int idx1, CvScalar* value)
{
   cvSet2D(arr, idx0, idx1, *value);
}

CvMat* cveGetSubRect(CvArr* arr, CvMat* submat, CvRect* rect)
{
   return cvGetSubRect(arr, submat, *rect);
}
CvMat* cveGetRows(CvArr* arr, CvMat* submat, int startRow, int endRow, int deltaRow)
{
   return cvGetRows(arr, submat, startRow, endRow, deltaRow);
}
CvMat* cveGetCols(CvArr* arr, CvMat* submat, int startCol, int endCol)
{
   return cvGetCols(arr, submat, startCol, endCol);
}

void cveGetSize(CvArr* arr, CvSize* size)
{
   *size = cvGetSize(arr);
}

void cveCopy(CvArr* src, CvArr* dst, CvArr* mask)
{
   cvCopy(src, dst, mask);
}
void cveRange(CvArr* mat, double start, double end)
{
   cvRange(mat, start, end);
}

void cveSetReal1D(CvArr* arr, int idx0, double value)
{
   cvSetReal1D(arr, idx0, value);
}
void cveSetReal2D(CvArr* arr, int idx0, int idx1, double value)
{
   cvSetReal2D(arr, idx0, idx1, value);
}
void cveSetReal3D(CvArr* arr, int idx0, int idx1, int idx2, double value)
{
   cvSetReal3D(arr, idx0, idx1, idx2, value);
}
void cveSetRealND(CvArr* arr, int* idx, double value)
{
   cvSetRealND(arr, idx, value);
}
void cveGet1D(CvArr* arr, int idx0, CvScalar* value)
{
   *value = cvGet1D(arr, idx0);
}
void cveGet2D(CvArr* arr, int idx0, int idx1, CvScalar* value)
{
   *value = cvGet2D(arr, idx0, idx1);
}
void cveGet3D(CvArr* arr, int idx0, int idx1, int idx2, CvScalar* value)
{
   *value = cvGet3D(arr, idx0, idx1, idx2);
}
double cveGetReal1D(CvArr* arr, int idx0)
{
   return cvGetReal1D(arr, idx0);
}
double cveGetReal2D(CvArr* arr, int idx0, int idx1)
{
   return cvGetReal2D(arr, idx0, idx1);
}
double cveGetReal3D(CvArr* arr, int idx0, int idx1, int idx2)
{
   return cvGetReal3D(arr, idx0, idx1, idx2);
}
void cveClearND(CvArr* arr, int* idx)
{
   cvClearND(arr, idx);
}

bool cveUseOptimized()
{
   return cv::useOptimized();
}
void cveSetUseOptimized(bool onoff)
{
   cv::setUseOptimized(onoff);
}
void cveGetBuildInformation(cv::String* buildInformation)
{
	cv::String bi = cv::getBuildInformation();
	*buildInformation = bi;
}

void cveGetRawData(CvArr* arr, uchar** data, int* step, CvSize* roiSize)
{
   cvGetRawData(arr, data, step, roiSize);
}
CvMat* cveGetMat(CvArr* arr, CvMat* header, int* coi, int allowNd)
{
   return cvGetMat(arr, header, coi, allowNd);
}
IplImage* cveGetImage(CvArr* arr, IplImage* imageHeader)
{
   return cvGetImage(arr, imageHeader);
}

int cveCheckArr(CvArr* arr, int flags, double minVal, double maxVal)
{
   return cvCheckArr(arr, flags, minVal, maxVal);
}

CvMat* cveReshape(CvArr* arr, CvMat* header, int newCn, int newRows)
{
   return cvReshape(arr, header, newCn, newRows);
}
CvMat* cveGetDiag(CvArr* arr, CvMat* submat, int diag)
{
   return cvGetDiag(arr, submat, diag);
}
void cveConvertScale(CvArr* arr, CvArr* dst, double scale, double shift)
{
   cvConvertScale(arr, dst, scale, shift);
}

void cveReleaseImage(IplImage** image)
{
   cvReleaseImage(image);
}

void cveSVDecomp(cv::_InputArray* src, cv::_OutputArray* w, cv::_OutputArray* u, cv::_OutputArray* vt, int flags)
{
   cv::SVDecomp(*src, *w, *u, *vt, flags);
}
void cveSVBackSubst(cv::_InputArray* w, cv::_InputArray* u, cv::_InputArray* vt, cv::_InputArray* rhs, cv::_OutputArray* dst)
{
   cv::SVBackSubst(*w, *u, *vt, *rhs, *dst);
}

void cvePCACompute1(cv::_InputArray* data, cv::_InputOutputArray* mean, cv::_OutputArray* eigenvectors, int maxComponents)
{
   cv::PCACompute(*data, *mean, *eigenvectors, maxComponents);
}

void cvePCACompute2(cv::_InputArray* data, cv::_InputOutputArray* mean, cv::_OutputArray* eigenvectors, double retainedVariance)
{
   cv::PCACompute(*data, *mean, *eigenvectors, retainedVariance);
}
void cvePCAProject(cv::_InputArray* data, cv::_InputArray* mean, cv::_InputArray* eigenvectors, cv::_OutputArray* result)
{
   cv::PCAProject(*data, *mean, *eigenvectors, *result);
}
void cvePCABackProject(cv::_InputArray* data, cv::_InputArray* mean, cv::_InputArray* eigenvectors, cv::_OutputArray* result)
{
   cv::PCABackProject(*data, *mean, *eigenvectors, *result);
}

void cveGetRangeAll(cv::Range* range)
{
   *range = cv::Range::all();
}

cv::Affine3d* cveAffine3dCreate()
{
   return new cv::Affine3d();
}
cv::Affine3d* cveAffine3dGetIdentity()
{
   cv::Affine3d i = cv::Affine3d::Identity();
   cv::Affine3d* result = new cv::Affine3d();
   *result = i;
   return result;
}
cv::Affine3d* cveAffine3dRotate(cv::Affine3d* affine, double r0, double r1, double r2)
{
   cv::Affine3d::Vec3 r(r0, r1, r2);
   cv::Affine3d rotated = affine->rotate(r);
   cv::Affine3d* result = new cv::Affine3d();
   *result = rotated;
   return result;
}
cv::Affine3d* cveAffine3dTranslate(cv::Affine3d* affine, double t0, double t1, double t2)
{
   cv::Affine3d::Vec3 t(t0, t1, t2);
   cv::Affine3d rotated = affine->translate(t);
   cv::Affine3d* result = new cv::Affine3d();
   *result = rotated;
   return result;
}
void cveAffine3dGetValues(cv::Affine3d* affine, double* values)
{
   memcpy(values, affine->matrix.val, 16 * sizeof(double));
}
void cveAffine3dRelease(cv::Affine3d** affine)
{
   delete* affine;
   *affine = 0;
}