//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "opencv2/core/core.hpp"

CVAPI(int) __divmodsi4(int a, int b, int* rem);

/* Returns: a / b, *rem = a % b  */

int __divmodsi4(int a, int b, int* rem)
{
  *rem = a%b;
  return a/b; 
}


