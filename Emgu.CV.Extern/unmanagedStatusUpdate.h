#include "opencv2/core/core.hpp"

typedef void (_stdcall *UnmanagedStatusUpdateCallback)( const char* message, int updateType);

/* Assigns a new status update function */
CVAPI(void) redirectUnmanagedStatusUpdate( UnmanagedStatusUpdateCallback statusUpdate_handler );

CVAPI(void) unmanagedUpdateStatus(char* message, int updateType);

CVAPI(void) unmanagedUpdateStatusRequestTestMessage();
