#include "unmanagedStatusUpdate.h"

static UnmanagedStatusUpdateCallback unmanagedStatusUpdateCallback = 0;

void redirectUnmanagedStatusUpdate( UnmanagedStatusUpdateCallback statusUpdate_handler )
{
   unmanagedStatusUpdateCallback = statusUpdate_handler;
}

void unmanagedUpdateStatus(char* message, int updateType)
{
   if (unmanagedStatusUpdateCallback)
   {
      unmanagedStatusUpdateCallback(message, updateType);
   }
}

void unmanagedUpdateStatusRequestTestMessage()
{
   unmanagedUpdateStatus("Test", 1);
}
