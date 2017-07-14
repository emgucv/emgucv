//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "dnn_c.h"

/*
void cveDnnInitModule()
{
   cv::dnn::initModule();
}
*/
cv::dnn::Importer* cveDnnCreateCaffeImporter(cv::String* prototxt, cv::String* caffeModel)
{
   cv::Ptr<cv::dnn::Importer> ptr = cv::dnn::createCaffeImporter(*prototxt, *caffeModel);
   ptr.addref();
   return ptr.get();
}
cv::dnn::Importer* cveDnnCreateTensorflowImporter(cv::String* model)
{
	cv::Ptr<cv::dnn::Importer> ptr = cv::dnn::createTensorflowImporter(*model);
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

void cveDnnNetSetInput(cv::dnn::Net* net, cv::Mat* blob, cv::String* name)
{
	net->setInput(*blob, name ? *name : "");
}
/*
void cveDnnNetSetBlob(cv::dnn::Net* net, cv::String* outputName, cv::dnn::Blob* blob)
{
   net->setBlob(*outputName, *blob);
}
cv::dnn::Blob* cveDnnNetGetBlob(cv::dnn::Net* net, cv::String* outputName)
{
   cv::dnn::Blob* blob = new cv::dnn::Blob();
   *blob = net->getBlob(*outputName);
   return blob;
}*/
void cveDnnNetForward(cv::dnn::Net* net, cv::String* outputName, cv::Mat* output)
{
   cv::Mat m = net->forward(*outputName);
   cv::swap(m, *output);
}
void cveDnnNetRelease(cv::dnn::Net** net)
{
   delete *net;
   *net = 0;
}

/*
cv::dnn::Blob* cveDnnBlobCreate()
{
	return new cv::dnn::Blob();
}
cv::dnn::Blob* cveDnnBlobCreateFromInputArray(cv::_InputArray* image)
{
   return new cv::dnn::Blob(*image);
}
void cveDnnBlobBatchFromImages(cv::dnn::Blob* blob, cv::_InputArray* image, int dstCn)
{
	blob->batchFromImages(*image, dstCn);
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

int cveDnnBlobDims(cv::dnn::Blob* blob)
{
   return blob->dims();
}
int cveDnnBlobChannels(cv::dnn::Blob* blob)
{
   return blob->channels();
}
int cveDnnBlobCols(cv::dnn::Blob* blob)
{
   return blob->cols();
}
int cveDnnBlobNum(cv::dnn::Blob* blob)
{
   return blob->num();
}
int cveDnnBlobRows(cv::dnn::Blob* blob)
{
   return blob->rows();
}
int cveDnnBlobType(cv::dnn::Blob* blob)
{
	return blob->type();
}
int cveDnnBlobElemSize(cv::dnn::Blob* blob)
{
	return blob->elemSize();
}
uchar * cveDnnBlobGetPtr(cv::dnn::Blob* blob, int n, int cn, int row, int col)
{
	return blob->ptr(n, cn, row, col);
}*/

void cveDnnBlobFromImage(
	cv::Mat* image,
	double scalefactor,
	CvSize* size,
	CvScalar* mean,
	bool swapRB,
	cv::Mat* blob)
{
	cv::Mat b = cv::dnn::blobFromImage(*image, scalefactor, *size, *mean, swapRB);
	cv::swap(*blob, b);
}

void cveDnnBlobFromImages(
	std::vector<cv::Mat>* images,
	double scalefactor,
	CvSize* size,
	CvScalar* mean,
	bool swapRB,
	cv::Mat* blob)
{
	cv::Mat b = cv::dnn::blobFromImages(*images, scalefactor, *size, *mean, swapRB);
	cv::swap(*blob, b);
}