//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "dnn_c.h"

cv::dnn::Importer* cveDnnCreateCaffeImporter(cv::String* prototxt, cv::String* caffeModel)
{
   cv::Ptr<cv::dnn::Importer> ptr = cv::dnn::createCaffeImporter(*prototxt, *caffeModel);
   ptr.addref();
   return ptr.get();
}
void cveDnnImporterRelease(cv::dnn::Importer** importer)
{
   delete *importer;
   *importer = 0;
}
void cveDnnImporterPopulateNet(cv::dnn::Importer* importer, cv::dnn::Net* net)
{
   importer->populateNet(*net);
}

cv::dnn::Net* cveDnnNetCreate()
{
   return new cv::dnn::Net();
}
void cveDnnNetSetBlob(cv::dnn::Net* net, cv::String* outputName, cv::dnn::Blob* blob)
{
   net->setBlob(*outputName, *blob);
}
cv::dnn::Blob* cveDnnNetGetBlob(cv::dnn::Net* net, cv::String* outputName)
{
   cv::dnn::Blob* blob = new cv::dnn::Blob();
   *blob = net->getBlob(*outputName);
   return blob;
}
void cveDnnNetForward(cv::dnn::Net* net)
{
   net->forward();
}
void cveDnnNetRelease(cv::dnn::Net** net)
{
   delete *net;
   *net = 0;
}


cv::dnn::Blob* cveDnnBlobCreateFromInputArray(cv::_InputArray* image, int dstCn)
{
   return new cv::dnn::Blob(*image, dstCn);
}
void cveDnnBlobMatRef(cv::dnn::Blob* blob, cv::Mat* outMat)
{
   cv::Mat m = blob->matRef();
   cv::swap(m, *outMat);
}
void cveDnnBlobRelease(cv::dnn::Blob** blob)
{
   delete *blob;
   *blob = 0;
}