#include "opencv2/core/cvdef.h"

#ifdef __cplusplus
#  define CV_EXTERN_C extern "C"
#else
#  define CV_EXTERN_C
#endif

#if defined(_WIN32) || defined(WINCE) || defined(__CYGWIN__)
#  define CV_EXPORTS __declspec(dllexport)
#elif defined(__GNUC__) && __GNUC__ >= 4
#  define CV_EXPORTS __attribute__ ((visibility ("default")))
#else
#  define CV_EXPORTS
#endif

#ifndef CVAPI
#  define CVAPI(rettype) CV_EXTERN_C CV_EXPORTS rettype
#endif

/** @brief Call the error handler.

Currently, the error handler prints the error code and the error message to the standard
error stream `stderr`. In the Debug configuration, it then provokes memory access violation, so that
the execution stack and all the parameters can be analyzed by the debugger. In the Release
configuration, the exception is thrown.

@param code one of Error::Code
@param msg error message
*/
#define CV_Error( code, msg ) cv::error( code, msg, CV_Func, __FILE__, __LINE__ )