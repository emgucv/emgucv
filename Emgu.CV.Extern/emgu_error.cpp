//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "emgu_error.h"

cv::ErrorCallback emguErrorCallback = 0;
void* emguErrorCallbackUserData = 0;

void emguSetErrorCallback(cv::ErrorCallback callback, void* userdata)
{
	emguErrorCallback = callback;
	emguErrorCallbackUserData = userdata;
}
