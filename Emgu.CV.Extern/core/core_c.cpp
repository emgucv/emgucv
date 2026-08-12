//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "core_c_extra.h"

bool cveSetBreakOnError(bool flag)
{
	// NOTE: intentionally not wrapped. cv::setBreakOnError is a trivial bool swap that
	// cannot throw.
	return cv::setBreakOnError(flag);
}

cv::ErrorCallback cveRedirectError(cv::ErrorCallback errorHandler, void* userdata, void** prevUserdata)
{
	// NOTE: intentionally not wrapped. This function is part of the error-detection
	// bootstrap machinery itself (it registers the callback that CvInvoke.CheckError()
	// relies on) -- see the analogous NOTE on cveStringCreate() below for why mixing
	// exception-checking into this specific plumbing is unsafe.
	// Also keep our own copy of the registered callback, so the
	// CVAPI_CATCH_CV_ERRORS macros (emgu_error.h) can invoke it directly for
	// exceptions that don't go through cv::error() (which invokes it
	// automatically, but only for cv::Exception).
	emguSetErrorCallback(errorHandler, userdata);
	return cv::redirectError(errorHandler, userdata, prevUserdata);
}

cv::String* cveTempfile(cv::String* suffix)
{
	try
	{
		cv::String tempFile;
		if (!suffix)
			tempFile = cv::tempfile();
		else
			tempFile = cv::tempfile(suffix->c_str());
		return new cv::String(tempFile);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

int cveSetLogLevel(int logLevel)
{
	try
	{
		return cv::utils::logging::setLogLevel( static_cast<cv::utils::logging::LogLevel>(logLevel));
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
int cveGetLogLevel()
{
	try
	{
		return cv::utils::logging::getLogLevel();
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

int cveGetThreadNum()
{
	try
	{
		return cv::getThreadNum();
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveSetNumThreads(int nthreads)
{
	try
	{
		cv::setNumThreads(nthreads);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
int cveGetNumThreads()
{
	try
	{
		return cv::getNumThreads();
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
int cveGetNumberOfCPUs()
{
	try
	{
		return cv::getNumberOfCPUs();
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

bool cveSetParallelForBackend(cv::String* backendName, bool propagateNumThreads)
{
	try
	{
		return cv::parallel::setParallelForBackend(*backendName, propagateNumThreads);
	}
	CVAPI_CATCH_CV_ERRORS(false)
}
void cveGetParallelBackends(std::vector< cv::String >* backendNames)
{
	try
	{
		backendNames->clear();
	#ifdef HAVE_TBB
		backendNames->push_back("TBB");
	#endif
	#ifdef HAVE_OPENMP
		backendNames->push_back("OPENMP");
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::String* cveStringCreate()
{
	// NOTE: intentionally not wrapped in CVAPI_CATCH_CV_ERRORS. CvString (the C# wrapper
	// around cv::String) is used internally by CvInvoke.CheckError()'s own error-marshaling
	// path (see CoreInvoke.cs), so its C# call sites must never call CheckError() themselves
	// (doing so would recurse infinitely when an error is actually pending). Wrapping the
	// native side without a consuming CheckError() would instead leave a stale pending-error
	// flag that could incorrectly surface at a later, unrelated CheckError() call.
	return new cv::String();
}
cv::String* cveStringCreateFromStr(const char* c, int size)
{
	// NOTE: intentionally not wrapped, see cveStringCreate() above.
	return new cv::String(c, size);
}
void cveStringGetCStr(cv::String* string, const char** c, int* size)
{
	// NOTE: intentionally not wrapped, see cveStringCreate() above.
	*c = string->c_str();
	*size = static_cast<int>(string->size());
}
int cveStringGetLength(cv::String* string)
{
	// NOTE: intentionally not wrapped, see cveStringCreate() above.
	return static_cast<int>(string->size());
}
void cveStringRelease(cv::String** string)
{
	delete *string;
	*string = 0;
}

cv::_InputArray* cveInputArrayFromDouble(double* scalar)
{
	// NOTE: intentionally not wrapped -- cv::_InputArray's constructors are trivial,
	// non-throwing pointer/value wraps (see the unwrapped GpuMat sibling below), and this is
	// one of the hottest call paths in the entire library (every IInputArray conversion goes
	// through here).
	return new cv::_InputArray(*scalar);
}

cv::_InputArray* cveInputArrayFromScalar(cv::Scalar* scalar)
{
	// NOTE: intentionally not wrapped, see cveInputArrayFromDouble() above.
	return new cv::_InputArray(*scalar);
}
cv::_InputArray* cveInputArrayFromMat(cv::Mat* mat)
{
	// NOTE: intentionally not wrapped, see cveInputArrayFromDouble() above.
	return new cv::_InputArray(*mat);
}

cv::_InputArray* cveInputArrayFromGpuMat(cv::cuda::GpuMat* mat)
{
	return new cv::_InputArray(*mat);
}

cv::_InputArray* cveInputArrayFromUMat(cv::UMat* mat)
{
	// NOTE: intentionally not wrapped, see cveInputArrayFromDouble() above.
	return new cv::_InputArray(*mat);
}

int cveInputArrayGetDims(cv::_InputArray* ia, int i)
{
	try
	{
		return ia->dims(i);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveInputArrayGetSize(cv::_InputArray* ia, cv::Size* size, int idx)
{
	try
	{
		cv::Size s = ia->size(idx);
		size->width = s.width;
		size->height = s.height;
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
int cveInputArrayGetDepth(cv::_InputArray* ia, int idx)
{
	try
	{
		return ia->depth(idx);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
int cveInputArrayGetChannels(cv::_InputArray* ia, int idx)
{
	try
	{
		return ia->channels(idx);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
bool cveInputArrayIsEmpty(cv::_InputArray* ia)
{
	try
	{
		return ia->empty();
	}
	CVAPI_CATCH_CV_ERRORS(false)
}
void cveInputArrayRelease(cv::_InputArray** arr)
{
	delete *arr;
	*arr = 0;
}

void cveInputArrayGetMat(cv::_InputArray* ia, int idx, cv::Mat* mat)
{
	try
	{
		cv::Mat m = ia->getMat(idx);
		cv::swap(m, *mat);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveInputArrayGetUMat(cv::_InputArray* ia, int idx, cv::UMat* umat)
{
	try
	{
		cv::UMat m = ia->getUMat(idx);
		cv::swap(m, *umat);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveInputArrayGetGpuMat(cv::_InputArray* ia, cv::cuda::GpuMat* gpuMat)
{
	cv::cuda::GpuMat m = ia->getGpuMat();
	cv::swap(m, *gpuMat);
}

void cveInputArrayCopyTo(cv::_InputArray* ia, cv::_OutputArray* arr, cv::_InputArray* mask)
{
	try
	{
		if (mask)
			ia->copyTo(*arr, *mask);
		else
			ia->copyTo(*arr);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}


cv::_OutputArray* cveOutputArrayFromMat(cv::Mat* mat)
{
	// NOTE: intentionally not wrapped, see cveInputArrayFromDouble() above.
	return new cv::_OutputArray(*mat);
}

cv::_OutputArray* cveOutputArrayFromGpuMat(cv::cuda::GpuMat* mat)
{
	return new cv::_OutputArray(*mat);
}

cv::_OutputArray* cveOutputArrayFromUMat(cv::UMat* mat)
{
	// NOTE: intentionally not wrapped, see cveInputArrayFromDouble() above.
	return new cv::_OutputArray(*mat);
}

void cveOutputArrayRelease(cv::_OutputArray** arr)
{
	delete *arr;
	*arr = 0;
}

cv::_InputOutputArray* cveInputOutputArrayFromMat(cv::Mat* mat)
{
	// NOTE: intentionally not wrapped, see cveInputArrayFromDouble() above.
	return new cv::_InputOutputArray(*mat);
}
cv::_InputOutputArray* cveInputOutputArrayFromUMat(cv::UMat* mat)
{
	// NOTE: intentionally not wrapped, see cveInputArrayFromDouble() above.
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

cv::Scalar* cveScalarCreate(cv::Scalar* scalar)
{
	// NOTE: intentionally not wrapped, see cveInputArrayFromDouble() above -- a trivial,
	// non-throwing copy construction.
	return new cv::Scalar(scalar->val[0], scalar->val[1], scalar->val[2], scalar->val[3]);
}
void cveScalarRelease(cv::Scalar** scalar)
{
	delete *scalar;
	*scalar = 0;
}

void cveMinMaxIdx(cv::_InputArray* src, double* minVal, double* maxVal, int* minIdx, int* maxIdx, cv::_InputArray* mask)
{
	try
	{
		cv::minMaxIdx(*src, minVal, maxVal, minIdx, maxIdx, mask ? *mask : static_cast<cv::InputArray>(cv::noArray()));
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveMinMaxLoc(cv::_InputArray* src, double* minVal, double* maxVal, cv::Point* minLoc, cv::Point* maxLoc, cv::_InputArray* mask)
{
	try
	{
		cv::Point minPt;
		cv::Point maxPt;
		cv::minMaxLoc(*src, minVal, maxVal, &minPt, &maxPt, mask ? *mask : static_cast<cv::InputArray>(cv::noArray()));
		minLoc->x = minPt.x; minLoc->y = minPt.y;
		maxLoc->x = maxPt.x; maxLoc->y = maxPt.y;
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveReduceArgMin(cv::_InputArray* src, cv::_OutputArray* dst, int axis, bool lastIndex)
{
	try
	{
		cv::reduceArgMin(*src, *dst, axis, lastIndex);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveReduceArgMax(cv::_InputArray* src, cv::_OutputArray* dst, int axis, bool lastIndex)
{
	try
	{
		cv::reduceArgMax(*src, *dst, axis, lastIndex);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveBitwiseAnd(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask)
{
	try
	{
		cv::bitwise_and(*src1, *src2, *dst, mask ? *mask : static_cast<cv::InputArray>(cv::noArray()));
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveBitwiseNot(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* mask)
{
	try
	{
		cv::bitwise_not(*src, *dst, mask ? *mask : static_cast<cv::InputArray>(cv::noArray()));
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveBitwiseOr(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask)
{
	try
	{
		cv::bitwise_or(*src1, *src2, *dst, mask ? *mask : static_cast<cv::InputArray>(cv::noArray()));
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveBitwiseXor(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask)
{
	try
	{
		cv::bitwise_xor(*src1, *src2, *dst, mask ? *mask : static_cast<cv::InputArray>(cv::noArray()));
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveAdd(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask, int dtype)
{
	try
	{
		cv::add(*src1, *src2, *dst, mask ? *mask : static_cast<cv::InputArray>(cv::noArray()), dtype);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveSubtract(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask, int dtype)
{
	try
	{
		cv::subtract(*src1, *src2, *dst, mask ? *mask : static_cast<cv::InputArray>(cv::noArray()), dtype);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveDivide(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, double scale, int dtype)
{
	try
	{
		cv::divide(*src1, *src2, *dst, scale, dtype);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveMultiply(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, double scale, int dtype)
{
	try
	{
		cv::multiply(*src1, *src2, *dst, scale, dtype);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
bool cveHasNonZero(cv::_InputArray* src)
{
	try
	{
		return cv::hasNonZero(*src);
	}
	CVAPI_CATCH_CV_ERRORS(false)
}
int cveCountNonZero(cv::_InputArray* src)
{
	try
	{
		return cv::countNonZero(*src);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveFindNonZero(cv::_InputArray* src, cv::_OutputArray* idx)
{
	try
	{
		cv::findNonZero(*src, *idx);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveMin(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst)
{
	try
	{
		cv::min(*src1, *src2, *dst);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveMax(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst)
{
	try
	{
		cv::max(*src1, *src2, *dst);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveAbsDiff(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst)
{
	try
	{
		cv::absdiff(*src1, *src2, *dst);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveInRange(cv::_InputArray* src1, cv::_InputArray* lowerb, cv::_InputArray* upperb, cv::_OutputArray* dst)
{
	try
	{
		cv::inRange(*src1, *lowerb, *upperb, *dst);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveSqrt(cv::_InputArray* src, cv::_OutputArray* dst)
{
	try
	{
		cv::sqrt(*src, *dst);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveCompare(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, int compop)
{
	try
	{
		cv::compare(*src1, *src2, *dst, compop);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveFlip(cv::_InputArray* src, cv::_OutputArray* dst, int flipCode)
{
	try
	{
		cv::flip(*src, *dst, flipCode);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveFlipND(cv::_InputArray* src, cv::_OutputArray* dst, int axis)
{
	try
	{
		cv::flipND(*src, *dst, axis);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveBroadcast(cv::_InputArray* src, cv::_InputArray* shape, cv::_OutputArray* dst)
{
	try
	{
		cv::broadcast(*src, *shape, *dst);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveRotate(cv::_InputArray* src, cv::_OutputArray* dst, int rotateCode)
{
	try
	{
		cv::rotate(*src, *dst, rotateCode);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveTranspose(cv::_InputArray* src, cv::_OutputArray* dst)
{
	try
	{
		cv::transpose(*src, *dst);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveLUT(cv::_InputArray* src, cv::_InputArray* lut, cv::_OutputArray* dst)
{
	try
	{
		cv::LUT(*src, *lut, *dst);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveSum(cv::_InputArray* src, cv::Scalar* result)
{
	try
	{
		cv::Scalar sum = cv::sum(*src);
		memcpy(&result->val[0], &sum.val[0], sizeof(double) * 4);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveMean(cv::_InputArray* src, cv::_InputArray* mask, cv::Scalar* result)
{
	try
	{
		cv::Scalar mean = cv::mean(*src, mask ? *mask : static_cast<cv::InputArray>(cv::noArray()));
		memcpy(&result->val[0], &mean.val[0], sizeof(double) * 4);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveMeanStdDev(cv::_InputArray* src, cv::_OutputArray* mean, cv::_OutputArray* stddev, cv::_InputArray* mask)
{
	try
	{
		cv::meanStdDev(*src, *mean, *stddev, mask ? *mask : static_cast<cv::InputArray>(cv::noArray()));
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveTrace(cv::_InputArray* mtx, cv::Scalar* result)
{
	try
	{
		cv::Scalar trace = cv::trace(*mtx);
		memcpy(&result->val[0], &trace.val[0], sizeof(double) * 4);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
double cveDeterminant(cv::_InputArray* mtx)
{
	try
	{
		return cv::determinant(*mtx);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
double cveNorm(cv::_InputArray* src1, cv::_InputArray* src2, int normType, cv::_InputArray* mask)
{
	try
	{
		if (src2)
		{
			return cv::norm(*src1, *src2, normType, mask ? *mask : static_cast<cv::InputArray>(cv::noArray()));
		}
		else
		{
			return cv::norm(*src1, normType, mask ? *mask : static_cast<cv::InputArray>(cv::noArray()));
		}
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
bool cveCheckRange(cv::_InputArray* arr, bool quiet, cv::Point* index, double minVal, double maxVal)
{
	try
	{
		cv::Point p;
		bool result = cv::checkRange(*arr, quiet, &p, minVal, maxVal);
		index->x = p.x;
		index->y = p.y;
		return result;
	}
	CVAPI_CATCH_CV_ERRORS(false)
}
void cvePatchNaNs(cv::_InputOutputArray* a, double val)
{
	try
	{
		cv::patchNaNs(*a, val);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveGemm(cv::_InputArray* src1, cv::_InputArray* src2, double alpha, cv::_InputArray* src3, double beta, cv::_OutputArray* dst, int flags)
{
	try
	{
		cv::gemm(*src1, *src2, alpha, src3 ? *src3 : static_cast<cv::InputArray>(cv::noArray()), beta, *dst, flags);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveScaleAdd(cv::_InputArray* src1, double alpha, cv::_InputArray* src2, cv::_OutputArray* dst)
{
	try
	{
		cv::scaleAdd(*src1, alpha, *src2, *dst);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveAddWeighted(cv::_InputArray* src1, double alpha, cv::_InputArray* src2, double beta, double gamma, cv::_OutputArray* dst, int dtype)
{
	try
	{
		cv::addWeighted(*src1, alpha, *src2, beta, gamma, *dst, dtype);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveConvertScaleAbs(cv::_InputArray* src, cv::_OutputArray* dst, double alpha, double beta)
{
	try
	{
		cv::convertScaleAbs(*src, *dst, alpha, beta);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveReduce(cv::_InputArray* src, cv::_OutputArray* dst, int dim, int rtype, int dtype)
{
	try
	{
		cv::reduce(*src, *dst, dim, rtype, dtype);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveRandShuffle(cv::_InputOutputArray* dst, double iterFactor, uint64 rng)
{
	try
	{
		if (rng == 0)
		{
			cv::randShuffle(*dst, iterFactor);
		}
		else
		{
			cv::RNG r(rng);
			cv::randShuffle(*dst, iterFactor, &r);
		}
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cvePow(cv::_InputArray* src, double power, cv::_OutputArray* dst)
{
	try
	{
		cv::pow(*src, power, *dst);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveExp(cv::_InputArray* src, cv::_OutputArray* dst)
{
	try
	{
		cv::exp(*src, *dst);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveLog(cv::_InputArray* src, cv::_OutputArray* dst)
{
	try
	{
		cv::log(*src, *dst);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveCartToPolar(cv::_InputArray* x, cv::_InputArray* y, cv::_OutputArray* magnitude, cv::_OutputArray* angle, bool angleInDegrees)
{
	try
	{
		cv::cartToPolar(*x, *y, *magnitude, angle ? *angle : static_cast<cv::OutputArray>(cv::noArray()), angleInDegrees);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cvePolarToCart(cv::_InputArray* magnitude, cv::_InputArray* angle, cv::_OutputArray* x, cv::_OutputArray* y, bool angleInDegrees)
{
	try
	{
		cv::polarToCart(*magnitude, *angle, *x, *y, angleInDegrees);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveSetIdentity(cv::_InputOutputArray* mtx, cv::Scalar* scalar)
{
	try
	{
		cv::setIdentity(*mtx, *scalar);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
int cveSolveCubic(cv::_InputArray* coeffs, cv::_OutputArray* roots)
{
	try
	{
		return cv::solveCubic(*coeffs, *roots);
	}
	CVAPI_CATCH_CV_ERRORS(-1)
}
double cveSolvePoly(cv::_InputArray* coeffs, cv::_OutputArray* roots, int maxIters)
{
	try
	{
		return cv::solvePoly(*coeffs, *roots, maxIters);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
bool cveSolve(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, int flags)
{
	try
	{
		return cv::solve(*src1, *src2, *dst, flags);
	}
	CVAPI_CATCH_CV_ERRORS(false)
}
void cveSort(cv::_InputArray* src, cv::_OutputArray* dst, int flags)
{
	try
	{
		cv::sort(*src, *dst, flags);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveSortIdx(cv::_InputArray* src, cv::_OutputArray* dst, int flags)
{
	try
	{
		cv::sortIdx(*src, *dst, flags);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
double cveInvert(cv::_InputArray* src, cv::_OutputArray* dst, int flags)
{
	try
	{
		return cv::invert(*src, *dst, flags);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveDft(cv::_InputArray* src, cv::_OutputArray* dst, int flags, int nonzeroRows)
{
	try
	{
		cv::dft(*src, *dst, flags, nonzeroRows);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveDct(cv::_InputArray* src, cv::_OutputArray* dst, int flags)
{
	try
	{
		cv::dct(*src, *dst, flags);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveMulSpectrums(cv::_InputArray *a, cv::_InputArray* b, cv::_OutputArray* c, int flags, bool conjB)
{
	try
	{
		cv::mulSpectrums(*a, *b, *c, flags, conjB);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

int cveGetOptimalDFTSize(int vecsize)
{
	try
	{
		return cv::getOptimalDFTSize(vecsize);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveTransform(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* m)
{
	try
	{
		cv::transform(*src, *dst, *m);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

double cveMahalanobis(cv::_InputArray* v1, cv::_InputArray* v2, cv::_InputArray* icovar)
{
	try
	{
		return cv::Mahalanobis(*v1, *v2, *icovar);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveCalcCovarMatrix(cv::_InputArray* samples, cv::_OutputArray* covar, cv::_InputOutputArray* mean, int flags, int ctype)
{
	try
	{
		cv::calcCovarMatrix(*samples, *covar, *mean, flags, ctype);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveNormalize(cv::_InputArray* src, cv::_InputOutputArray* dst, double alpha, double beta, int normType, int dType, cv::_InputArray* mask)
{
	try
	{
		cv::normalize(*src, *dst, alpha, beta, normType, dType, mask ? *mask : static_cast<cv::InputArray>(cv::noArray()));
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cvePerspectiveTransform(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* m)
{
	try
	{
		cv::perspectiveTransform(*src, *dst, *m);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveMulTransposed(cv::_InputArray* src, cv::_OutputArray* dst, bool aTa, cv::_InputArray* delta, double scale, int dtype)
{
	try
	{
		cv::mulTransposed(*src, *dst, aTa, delta ? *delta : static_cast<cv::InputArray>(cv::noArray()), scale, dtype);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveSplit(cv::_InputArray* src, cv::_OutputArray* mv)
{
	try
	{
		cv::split(*src, *mv);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveMerge(cv::_InputArray* mv, cv::_OutputArray* dst)
{
	try
	{
		cv::merge(*mv, *dst);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveMixChannels(cv::_InputArray* src, cv::_InputOutputArray* dst, const int* fromTo, int npairs)
{
	try
	{
		cv::mixChannels(*src, *dst, fromTo, npairs);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveExtractChannel(cv::_InputArray* src, cv::_OutputArray* dst, int coi)
{
	try
	{
		cv::extractChannel(*src, *dst, coi);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveInsertChannel(cv::_InputArray* src, cv::_InputOutputArray* dst, int coi)
{
	try
	{
		cv::insertChannel(*src, *dst, coi);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}


double cveKmeans(cv::_InputArray* data, int k, cv::_InputOutputArray* bestLabels, cv::TermCriteria* criteria, int attempts, int flags, cv::_OutputArray* centers)
{
	try
	{
		return cv::kmeans(*data, k, *bestLabels, *criteria, attempts, flags, centers ? *centers : static_cast<cv::OutputArray>(cv::noArray()));
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveHConcat(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst)
{
	try
	{
		cv::hconcat(*src1, *src2, *dst);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveVConcat(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst)
{
	try
	{
		cv::vconcat(*src1, *src2, *dst);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveHConcat2(cv::_InputArray* src, cv::_OutputArray* dst)
{
	try
	{
		cv::hconcat(*src, *dst);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveVConcat2(cv::_InputArray* src, cv::_OutputArray* dst)
{
	try
	{
		cv::vconcat(*src, *dst);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}


double cvePSNR(cv::_InputArray* src1, cv::_InputArray* src2)
{
	try
	{
		return cv::PSNR(*src1, *src2);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

bool cveEigen(cv::_InputArray* src, cv::_OutputArray* eigenValues, cv::_OutputArray* eigenVectors)
{
	try
	{
		return cv::eigen(*src, *eigenValues, eigenVectors ? *eigenVectors : static_cast<cv::OutputArray>(cv::noArray()));
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

//Algorithm
void cveAlgorithmRead(cv::Algorithm* algorithm, cv::FileNode* node)
{
	try
	{
		algorithm->read(*node);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveAlgorithmWrite(cv::Algorithm* algorithm, cv::FileStorage* storage)
{
	try
	{
		algorithm->write(*storage);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveAlgorithmWrite2(cv::Algorithm* algorithm, cv::FileStorage* storage, cv::String* name)
{
	try
	{
		//cv::Ptr<cv::FileStorage> storagePtr(storage, [](cv::FileStorage*) {});
		algorithm->write(*storage, *name);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveAlgorithmSave(cv::Algorithm* algorithm, cv::String* filename)
{
	try
	{
		algorithm->save(*filename);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveAlgorithmClear(cv::Algorithm* algorithm)
{
	try
	{
		algorithm->clear();
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

bool cveAlgorithmEmpty(cv::Algorithm* algorithm)
{
	try
	{
		return algorithm->empty();
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

void cveAlgorithmGetDefaultName(cv::Algorithm* algorithm, cv::String* defaultName)
{
	try
	{
		cv::String name = algorithm->getDefaultName();
		*defaultName = name;
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

bool cveClipLine(cv::Rect* rect, cv::Point* pt1, cv::Point* pt2)
{
	try
	{
		cv::Point p1 = *pt1, p2 = *pt2;
		bool r = cv::clipLine(*rect, p1, p2);
		pt1->x = p1.x; pt1->y = p1.y;
		pt2->x = p2.x; pt2->y = p2.y;
		return r;
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

void cveRandn(cv::_InputOutputArray* dst, cv::_InputArray* mean, cv::_InputArray* stddev)
{
	try
	{
		cv::randn(*dst, *mean, *stddev);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveRandu(cv::_InputOutputArray* dst, cv::_InputArray* low, cv::_InputArray* high)
{
	try
	{
		cv::randu(*dst, *low, *high);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

//File Storage
cv::FileStorage* cveFileStorageCreate(cv::String* source, int flags, cv::String* encoding)
{
	try
	{
		return new cv::FileStorage(*source, flags, *encoding);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
bool cveFileStorageIsOpened(cv::FileStorage* storage)
{
	try
	{
		return storage->isOpened();
	}
	CVAPI_CATCH_CV_ERRORS(false)
}
void cveFileStorageReleaseAndGetString(cv::FileStorage* storage, cv::String* result)
{
	try
	{
		*result = storage->releaseAndGetString();
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveFileStorageRelease(cv::FileStorage** storage)
{
	delete *storage;
	*storage = 0;
}
void cveFileStorageWriteMat(cv::FileStorage* fs, cv::String* name, cv::Mat* value)
{
	try
	{
		cv::write(*fs, *name, *value);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveFileStorageWriteInt(cv::FileStorage* fs, cv::String* name, int value)
{
	try
	{
		cv::write(*fs, *name, value);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveFileStorageWriteInt64(cv::FileStorage* fs, cv::String* name, int64_t value)
{
	try
	{
		cv::write(*fs, *name, value);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveFileStorageWriteFloat(cv::FileStorage* fs, cv::String* name, float value)
{
	try
	{
		cv::write(*fs, *name, value);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveFileStorageWriteDouble(cv::FileStorage* fs, cv::String* name, double value)
{
	try
	{
		cv::write(*fs, *name, value);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveFileStorageWriteString(cv::FileStorage* fs, cv::String* name, cv::String* value)
{
	try
	{
		cv::write(*fs, *name, *value);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveFileStorageInsertString(cv::FileStorage* fs, cv::String* value)
{
	try
	{
		(*fs) << *value;
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::FileNode* cveFileStorageRoot(cv::FileStorage* fs, int streamIdx)
{
	try
	{
		cv::FileNode* n = new cv::FileNode();
		cv::FileNode root = fs->root(streamIdx);
		*n = root;
		return n;
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::FileNode* cveFileStorageGetFirstTopLevelNode(cv::FileStorage* fs)
{
	try
	{
		cv::FileNode* n = new cv::FileNode();
		cv::FileNode root = fs->getFirstTopLevelNode();
		*n = root;
		return n;
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::FileNode* cveFileStorageGetNode(cv::FileStorage* fs, cv::String* nodeName)
{
	try
	{
		cv::FileNode* n = new cv::FileNode();
		cv::FileNode root = (*fs)[*nodeName];
		*n = root;
		return n;
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

//File Node
void cveFileNodeReadMat(cv::FileNode* node, cv::Mat* mat, cv::Mat* defaultMat)
{
	try
	{
		cv::read(*node, *mat, *defaultMat);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
int cveFileNodeGetType(cv::FileNode* node)
{
	try
	{
		return node->type();
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveFileNodeGetName(cv::FileNode* node, cv::String* name)
{
	try
	{
		cv::String n = node->name();
		*name = n;
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveFileNodeGetKeys(cv::FileNode* node, std::vector< cv::String >* keys)
{
	std::vector< cv::String > kv = node->keys();
	keys->clear();
	for (std::vector< cv::String >::iterator it = kv.begin(); it != kv.end(); ++it)
		keys->push_back(*it);
}
void cveFileNodeReadString(cv::FileNode* node, cv::String* str, cv::String* defaultStr)
{
	try
	{
		cv::read(*node, *str, *defaultStr);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
int cveFileNodeReadInt(cv::FileNode* node, int defaultInt)
{
	try
	{
		int result = 0;
		cv::read(*node, result, defaultInt);
		return result;
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
int64_t cveFileNodeReadInt64(cv::FileNode* node, int64_t defaultInt)
{
	try
	{
		int64_t result = 0;
		cv::read(*node, result, defaultInt);
		return result;
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
double cveFileNodeReadDouble(cv::FileNode* node, double defaultDouble)
{
	try
	{
		double result = 0;
		cv::read(*node, result, defaultDouble);
		return result;
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
float cveFileNodeReadFloat(cv::FileNode* node, float defaultFloat)
{
	try
	{
		float result = 0;
		cv::read(*node, result, defaultFloat);
		return result;
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveFileNodeRelease(cv::FileNode** node)
{
	delete *node;
	*node = 0;
}

cv::FileNodeIterator* cveFileNodeIteratorCreate()
{
	try
	{
		return new cv::FileNodeIterator();
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::FileNodeIterator* cveFileNodeIteratorCreateFromNode(cv::FileNode* node, bool seekEnd)
{
	try
	{
		return new cv::FileNodeIterator(*node, seekEnd);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
bool cveFileNodeIteratorEqualTo(cv::FileNodeIterator* iterator, cv::FileNodeIterator* otherIterator)
{
	try
	{
		return iterator->equalTo(*otherIterator);
	}
	CVAPI_CATCH_CV_ERRORS(false)
}
void cveFileNodeIteratorNext(cv::FileNodeIterator* iterator)
{
	try
	{
		++(*iterator);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
cv::FileNode* cveFileNodeIteratorGetFileNode(cv::FileNodeIterator* iterator)
{
	try
	{
		cv::FileNode* node = new cv::FileNode();
		*node = *(*iterator);
		return node;
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveFileNodeIteratorRelease(cv::FileNodeIterator** iterator)
{
	delete* iterator;
	*iterator = 0;
}

bool cveUseOptimized()
{
	try
	{
		return cv::useOptimized();
	}
	CVAPI_CATCH_CV_ERRORS(false)
}
void cveSetUseOptimized(bool onoff)
{
	try
	{
		cv::setUseOptimized(onoff);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveGetBuildInformation(cv::String* buildInformation)
{
	try
	{
		*buildInformation = cv::getBuildInformation();
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveSVDecomp(cv::_InputArray* src, cv::_OutputArray* w, cv::_OutputArray* u, cv::_OutputArray* vt, int flags)
{
	try
	{
		cv::SVDecomp(*src, *w, *u, *vt, flags);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveSVBackSubst(cv::_InputArray* w, cv::_InputArray* u, cv::_InputArray* vt, cv::_InputArray* rhs, cv::_OutputArray* dst)
{
	try
	{
		cv::SVBackSubst(*w, *u, *vt, *rhs, *dst);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cvePCACompute1(cv::_InputArray* data, cv::_InputOutputArray* mean, cv::_OutputArray* eigenvectors, int maxComponents)
{
	try
	{
		cv::PCACompute(*data, *mean, *eigenvectors, maxComponents);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cvePCACompute2(cv::_InputArray* data, cv::_InputOutputArray* mean, cv::_OutputArray* eigenvectors, double retainedVariance)
{
	try
	{
		cv::PCACompute(*data, *mean, *eigenvectors, retainedVariance);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cvePCAProject(cv::_InputArray* data, cv::_InputArray* mean, cv::_InputArray* eigenvectors, cv::_OutputArray* result)
{
	try
	{
		cv::PCAProject(*data, *mean, *eigenvectors, *result);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cvePCABackProject(cv::_InputArray* data, cv::_InputArray* mean, cv::_InputArray* eigenvectors, cv::_OutputArray* result)
{
	try
	{
		cv::PCABackProject(*data, *mean, *eigenvectors, *result);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveGetRangeAll(cv::Range* range)
{
	try
	{
		*range = cv::Range::all();
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::Affine3d* cveAffine3dCreate()
{
	try
	{
		return new cv::Affine3d();
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::Affine3d* cveAffine3dGetIdentity()
{
	try
	{
		cv::Affine3d* result = new cv::Affine3d();
		*result = cv::Affine3d::Identity();
		return result;
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::Affine3d* cveAffine3dRotate(cv::Affine3d* affine, double r0, double r1, double r2)
{
	try
	{
		cv::Affine3d::Vec3 r(r0, r1, r2);
		cv::Affine3d rotated = affine->rotate(r);
		cv::Affine3d* result = new cv::Affine3d();
		*result = rotated;
		return result;
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::Affine3d* cveAffine3dTranslate(cv::Affine3d* affine, double t0, double t1, double t2)
{
	try
	{
		cv::Affine3d::Vec3 t(t0, t1, t2);
		cv::Affine3d rotated = affine->translate(t);
		cv::Affine3d* result = new cv::Affine3d();
		*result = rotated;
		return result;
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveAffine3dGetValues(cv::Affine3d* affine, double* values)
{
	try
	{
		memcpy(values, affine->matrix.val, 16 * sizeof(double));
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveAffine3dRelease(cv::Affine3d** affine)
{
	delete* affine;
	*affine = 0;
}

cv::RNG* cveRngCreate()
{
	try
	{
		return new cv::RNG();
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::RNG* cveRngCreateWithSeed(uint64 state)
{
	try
	{
		return new cv::RNG(state);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveRngFill(cv::RNG* rng, cv::_InputOutputArray* mat, int distType, cv::_InputArray* a, cv::_InputArray* b, bool saturateRange)
{
	try
	{
		rng->fill(*mat, distType, *a, *b, saturateRange);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
double cveRngGaussian(cv::RNG* rng, double sigma)
{
	try
	{
		return rng->gaussian(sigma);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
unsigned cveRngNext(cv::RNG* rng)
{
	try
	{
		return rng->next();
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
int cveRngUniformInt(cv::RNG* rng, int a, int b)
{
	try
	{
		return rng->uniform(a, b);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
float cveRngUniformFloat(cv::RNG* rng, float a, float b)
{
	try
	{
		return rng->uniform(a, b);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
double cveRngUniformDouble(cv::RNG* rng, double a, double b)
{
	try
	{
		return rng->uniform(a, b);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveRngRelease(cv::RNG** rng)
{
	delete *rng;
	*rng = 0;
}

cv::Moments* cveMomentsCreate()
{
	try
	{
		return new cv::Moments();
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveMomentsRelease(cv::Moments** moments)
{
	delete *moments;
	*moments = 0;
}

void cveGetConfigDict(std::vector<cv::String>* key, std::vector<double>* value)
{
	try
	{
		key->clear();
		value->clear();

		key->push_back("HAVE_OPENCV_GAPI");
	#ifdef HAVE_OPENCV_GAPI
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_ALPHAMAT");
	#ifdef HAVE_OPENCV_ALPHAMAT
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_ARUCO");
	#ifdef HAVE_OPENCV_ARUCO
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_BGSEGM");
	#ifdef HAVE_OPENCV_BGSEGM
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_BIOINSPIRED");
	#ifdef HAVE_OPENCV_BIOINSPIRED
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_CALIB");
	#ifdef HAVE_OPENCV_CALIB
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_3D");
	#ifdef HAVE_OPENCV_3D
		value->push_back(1);
	#else
		value->push_back(0);
	#endif


		key->push_back("HAVE_OPENCV_CORE");
	#ifdef HAVE_OPENCV_CORE
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_DNN");
	#ifdef HAVE_OPENCV_DNN
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_DNN_OBJDETECT");
	#ifdef HAVE_OPENCV_DNN_OBJDETECT
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_DNN_SUPERRES");
	#ifdef HAVE_OPENCV_DNN_SUPERRES
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_DPM");
	#ifdef HAVE_OPENCV_DPM
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_FACE");
	#ifdef HAVE_OPENCV_FACE
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_FEATURES");
	#ifdef HAVE_OPENCV_FEATURES
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_FLANN");
	#ifdef HAVE_OPENCV_FLANN
		value->push_back(1);
	#else
		value->push_back(0);
	#endif
	
		key->push_back("HAVE_OPENCV_FREETYPE");
	#ifdef HAVE_OPENCV_FREETYPE
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_FUZZY");
	#ifdef HAVE_OPENCV_FUZZY
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_HDF");
	#ifdef HAVE_OPENCV_HDF
		value->push_back(1);
	#else
		value->push_back(0);
	#endif
	
		key->push_back("HAVE_OPENCV_HFS");
	#ifdef HAVE_OPENCV_HFS
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_HIGHGUI");
	#ifdef HAVE_OPENCV_HIGHGUI
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_IMG_HASH");
	#ifdef HAVE_OPENCV_IMG_HASH
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_IMGCODECS");
	#ifdef HAVE_OPENCV_IMGCODECS
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_IMGPROC");
	#ifdef HAVE_OPENCV_IMGPROC
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_INTENSITY_TRANSFORM");
	#ifdef HAVE_OPENCV_INTENSITY_TRANSFORM
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_LINE_DESCRIPTOR");
	#ifdef HAVE_OPENCV_LINE_DESCRIPTOR
		value->push_back(1);
	#else
		value->push_back(0);
	#endif
	
		key->push_back("HAVE_OPENCV_MCC");
	#ifdef HAVE_OPENCV_MCC
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_ML");
	#ifdef HAVE_OPENCV_ML
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_OBJDETECT");
	#ifdef HAVE_OPENCV_OBJDETECT
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_OPTFLOW");
	#ifdef HAVE_OPENCV_OPTFLOW
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_PHASE_UNWRAPPING");
	#ifdef HAVE_OPENCV_PHASE_UNWRAPPING
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_PHOTO");
	#ifdef HAVE_OPENCV_PHOTO
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_PLOT");
	#ifdef HAVE_OPENCV_PLOT
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_QUALITY");
	#ifdef HAVE_OPENCV_QUALITY
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_RAPID");
	#ifdef HAVE_OPENCV_RAPID
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_SALIENCY");
	#ifdef HAVE_OPENCV_SALIENCY
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_SHAPE");
	#ifdef HAVE_OPENCV_SHAPE
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_STEREO");
	#ifdef HAVE_OPENCV_STEREO
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_XSTEREO");
	#ifdef HAVE_OPENCV_XSTEREO
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_STITCHING");
	#ifdef HAVE_OPENCV_STITCHING
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_SUPERRES");
	#ifdef HAVE_OPENCV_SUPERRES
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_SURFACE_MATCHING");
	#ifdef HAVE_OPENCV_SURFACE_MATCHING
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_TEXT");
	#ifdef HAVE_OPENCV_TEXT
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_TRACKING");
	#ifdef HAVE_OPENCV_TRACKING
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_VIDEO");
	#ifdef HAVE_OPENCV_VIDEO
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_VIDEOIO");
	#ifdef HAVE_OPENCV_VIDEOIO
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_VIDEOSTAB");
	#ifdef HAVE_OPENCV_VIDEOSTAB
		value->push_back(1);
	#else
		value->push_back(0);
	#endif
	
		key->push_back("HAVE_OPENCV_VIZ");
	#ifdef HAVE_OPENCV_VIZ
		value->push_back(1);
	#else
		value->push_back(0);
	#endif
	
		key->push_back("HAVE_OPENCV_XFEATURES2D");
	#ifdef HAVE_OPENCV_XFEATURES2D
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_XIMGPROC");
	#ifdef HAVE_OPENCV_XIMGPROC
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_XOBJDETECT");
	#ifdef HAVE_OPENCV_XOBJDETECT
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_XPHOTO");
	#ifdef HAVE_OPENCV_XPHOTO
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_OPENCV_WECHAT_QRCODE");
	#ifdef HAVE_OPENCV_WECHAT_QRCODE
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		/*
		key->push_back("HAVE_OPENCV_BARCODE");
	#ifdef HAVE_OPENCV_BARCODE
		value->push_back(1);
	#else
		value->push_back(0);
	#endif
	*/

		key->push_back("HAVE_OPENCV_RGBD");
	#ifdef HAVE_OPENCV_RGBD
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_EMGUCV_TESSERACT");
	#ifdef HAVE_EMGUCV_TESSERACT
		value->push_back(1);
	#else
		value->push_back(0);
	#endif
	
		key->push_back("HAVE_DEPTHAI");
	#ifdef HAVE_DEPTHAI
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_DIRECTX");
	#ifdef HAVE_DIRECTX
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

		key->push_back("HAVE_ONNXRUNTIME");
	#ifdef HAVE_ONNXRUNTIME
		value->push_back(1);
	#else
		value->push_back(0);
	#endif

	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

#if defined(CV_ICC) && defined(_M_IX86)
//Fix for intel compiler: Intel compiler has not implemented __iso_volatile_load64 for x86 architecture.
__int64 __iso_volatile_load64(const volatile __int64* _mem)
{
	return *_mem;
}
#endif