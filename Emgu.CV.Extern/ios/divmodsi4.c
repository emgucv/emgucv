//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "opencv2/core/core_c.h"

CVAPI(int) __divmodsi4(int a, int b, int* rem);
CVAPI(unsigned long) __udivmodsi4(unsigned long num, unsigned long den, int modwanted);
/* Returns: a / b, *rem = a % b  */

int __divmodsi4(int a, int b, int* rem)
{
  int d = a / b;
  *rem = a - (d * b); 
  return d; 
}

unsigned long __udivmodsi4(unsigned long num, unsigned long den, int modwanted)
{
  unsigned long bit = 1;
  unsigned long res = 0;

  while (den < num && bit && !(den & (1L<<31)))
    {
      den <<=1;
      bit <<=1;
    }
  while (bit)
    {
      if (num >= den)
	{
	  num -= den;
	  res |= bit;
	}
      bit >>=1;
      den >>=1;
    }
  if (modwanted) return num;
  return res;
}

