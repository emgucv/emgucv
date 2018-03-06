//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "highgui_c_extra.h"

void cveImshow(cv::String* winname, cv::_InputArray* mat)
{
  cv::imshow(*winname, *mat);
}
void cveNamedWindow(cv::String* winname, int flags)
{
  cv::namedWindow(*winname, flags);
}
void cveDestroyWindow(cv::String* winname)
{
  cv::destroyWindow(*winname);
}
void cveDestroyAllWindows()
{
  cv::destroyAllWindows();
}
int cveWaitKey(int delay)
{
  return cv::waitKey(delay);
}