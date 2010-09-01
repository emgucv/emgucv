#pragma once
#ifndef VECTOR_OF_FLOAT_H
#define VECTOR_OF_FLOAT_H

#include "opencv2/core/core_c.h"
#include <vector>

class VectorOfFloat
{ 
public:
   std::vector<float> data;
   VectorOfFloat() : data() {};
   VectorOfFloat(int size) : data(size) {};
};

#endif