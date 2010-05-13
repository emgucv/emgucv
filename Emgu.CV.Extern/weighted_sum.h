#pragma once

#include "core_c.h"
#include "sse.h"

CVAPI(void) weightedSum(double* d1, double* d2, int elementCount, double w1, double w2, double* r);