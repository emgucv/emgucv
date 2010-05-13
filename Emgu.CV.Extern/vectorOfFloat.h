#pragma once
#ifndef VECTOR_OF_FLOAT_H
#define VECTOR_OF_FLOAT_H

#include <core_c.h>
#include <vector>

class vectorOfFloat
{ 
public:
   std::vector<float> data;
   vectorOfFloat();
   vectorOfFloat(int size);
};

#endif