#include "cvextern.h"

void StatModelSave(CvStatModel* model, const char* filename, const char* name)
{
   model->save(filename, name);
}

void StatModelLoad(CvStatModel* model, const char* filename, const char* name)
{
   model->load(filename, name);
}

void StatModelClear(CvStatModel* model)
{
   model->clear();
}
