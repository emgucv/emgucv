#include "blob.h"

CvBlobSeq* CvBlobSeqCreate(int BlobSize)
{
   return new CvBlobSeq(BlobSize);
}

void CvBlobSeqRelease(CvBlobSeq* blobSeq)
{
   blobSeq->~CvBlobSeq();
}

CvBlob* CvBlobSeqGetBlob(CvBlobSeq* blobSeq, int blobIndex)
{
   return blobSeq->GetBlobByID(blobIndex);
}

int CvBlobSeqGetBlobNum(CvBlobSeq* blobSeq)
{
   return blobSeq->GetBlobNum();
}

void CvBlobDetectorRelease(CvBlobDetector* detector)
{
   detector->~CvBlobDetector();
}

int CvBlobDetectorDetectNewBlob(CvBlobDetector* detector, IplImage* pImg, IplImage* pImgFG, CvBlobSeq* pNewBlobList, CvBlobSeq* pOldBlobList)
{
   return detector->DetectNewBlob(pImg, pImgFG, pNewBlobList, pOldBlobList);
}

CvBlobDetector* CvCreateBlobDetectorSimple()
{
   return cvCreateBlobDetectorSimple();
}

CvBlobDetector* CvCreateBlobDetectorCC()
{
   return cvCreateBlobDetectorCC();
}

