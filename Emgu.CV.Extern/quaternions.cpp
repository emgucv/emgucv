//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "quaternions.h"

void eulerToQuaternions(double x, double y, double z, Quaternions* quaternions)
{
	quaternions->setEuler(x, y, z);
}

void quaternionsToEuler(const Quaternions* quaternions, double* x, double* y, double* z)
{
	quaternions->getEuler(x, y, z);
}

void quaternionsRotatePoint(const Quaternions* quaternions, const cv::Point3d* point, cv::Point3d* pointDst)
{
	quaternions->rotatePoint(point, pointDst);
}

void quaternionsRotatePoints(const Quaternions* quaternions, const cv::Mat* p, cv::Mat* pDst)
{
	CV_Assert((p->rows == 3 && p->cols == 1) || p->cols == 3);
	if (pDst->empty())
	{
		pDst->create(p->size(), p->type());
	}
	CV_Assert(pDst->rows == p->rows && pDst->cols == p->cols);

	cv::MatConstIterator_<double> pIter = p->begin<double>();
	cv::MatIterator_<double> pDstIter = pDst->begin<double>();

	if ((p->rows == 3 && p->cols == 1))
	{
		quaternionsRotatePoint(quaternions, (cv::Point3d*)pIter.ptr, (cv::Point3d*)pDstIter.ptr);
	}
	else
	{
		for (int i = 0; i < p->rows; i++, pIter += 3, pDstIter += 3)
		{
			quaternionsRotatePoint(quaternions, (cv::Point3d*)pIter.ptr, (cv::Point3d*)pDstIter.ptr);
		}
	}
}

void quaternionsToRotationMatrix(const Quaternions* quaternions, cv::Mat* r)
{
	double w = quaternions->w;
	double x = quaternions->x;
	double y = quaternions->y;
	double z = quaternions->z;

	if (r->empty())
	{
		r->create(cv::Size(3, 3), CV_MAKETYPE(CV_64F, 1));
	}
	CV_Assert(r->rows == 3 && r->cols == 3);
	cv::MatIterator_<double> rIter = r->begin<double>();
	*rIter++ = w * w + x * x - y * y - z * z; *rIter++ = 2.0 * (x * y - w * z); *rIter++ = 2.0 * (x * z + w * y);
	*rIter++ = 2.0 * (x * y + w * z); *rIter++ = w * w - x * x + y * y - z * z; *rIter++ = 2.0 * (y * z - w * x);
	*rIter++ = 2.0 * (x * z - w * y); *rIter++ = 2.0 * (y * z + w * x); *rIter++ = w * w - x * x - y * y + z * z;
}

void quaternionsMultiply(const Quaternions* quaternions1, const Quaternions* quaternions2, Quaternions* quaternionsDst)
{
	quaternions1->multiply(quaternions2, quaternionsDst);
}

void axisAngleToQuaternions(const cv::Point3d* axisAngle, Quaternions* quaternions)
{
	quaternions->setAxisAngle(axisAngle);
}

void quaternionsToAxisAngle(const Quaternions* quaternions, cv::Point3d* axisAngle)
{
	quaternions->getAxisAngle(axisAngle);
}

void quaternionsRenorm(Quaternions* quaternions) { quaternions->renorm(); }

void quaternionsSlerp(const Quaternions* qa, const Quaternions* qb, double t, Quaternions* qm)
{
	qa->slerp(qb, t, qm);
}