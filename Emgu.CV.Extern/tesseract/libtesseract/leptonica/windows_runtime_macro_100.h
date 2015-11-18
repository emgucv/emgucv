//NEED to have separate macro file for leptonica and tessearact. 
//Some macro definition, such as CopyFile will conflict with tesseract.
#define getpid() GetCurrentProcessId()
#define getcwd(A,B) NULL
#define GetProcessTimes(A,B,C,D,E) ((void)0)
#define FindFirstFileA(A,B) INVALID_HANDLE_VALUE
#define getenv(A) NULL
#define CopyFile(A,B,C) 0
#define GetTempPath(A,B) B[0]=0
#define GetFileAttributes(A) INVALID_FILE_ATTRIBUTES

#include <process.h>
//#define system(A) -1