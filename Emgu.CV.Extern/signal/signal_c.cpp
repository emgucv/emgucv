//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "signal_c.h"

void cveResampleSignal(cv::_InputArray* inputSignal, cv::_OutputArray* outSignal, int inFreq, int outFreq)
{
#ifdef HAVE_OPENCV_SIGNAL
	cv::signal::resampleSignal(*inputSignal, *outSignal, inFreq, outFreq);
#else
	throw_no_signal();
#endif
}
