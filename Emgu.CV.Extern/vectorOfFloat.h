#ifndef VECTOR_OF_FLOAT_H
#define VECTOR_OF_FLOAT_H

#include <cxcore.h>

class vectorOfFloat
{ 
public:
   std::vector<float> data;
   vectorOfFloat();
   vectorOfFloat(int size);
};

#endif