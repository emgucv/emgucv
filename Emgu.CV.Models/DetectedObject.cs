//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Emgu.CV.Models
{
    public class DetectedObject
    {
        public Rectangle Region;
        public double Confident;
        public String Label;
        public int ClassId;
    }
}
