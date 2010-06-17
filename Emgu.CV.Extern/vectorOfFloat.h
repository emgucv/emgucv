#pragma once
#ifndef VECTOR_OF_FLOAT_H
#define VECTOR_OF_FLOAT_H

#include "opencv2/core/core_c.h"
#include <vector>

class vectorOfFloat
{ 
public:
   std::vector<float> data;
   vectorOfFloat() : data() {};
   vectorOfFloat(int size) : data(size) {};
};

#endif