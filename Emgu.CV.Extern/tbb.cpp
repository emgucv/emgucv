//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "opencv2/core/core_c.h"

#if HAVE_TBB
#include "tbb/task_scheduler_init.h"
#endif

CVAPI(void*) tbbTaskSchedulerInit()
{
#if HAVE_TBB
  return new tbb::task_scheduler_init();
#else
  return 0;
#endif
}

CVAPI(void) tbbTaskSchedulerRelease(void** scheduler)
{
#if HAVE_TBB
   tbb::task_scheduler_init* ptr = (tbb::task_scheduler_init*) *scheduler;
   if (*scheduler) delete ptr;
#endif
}