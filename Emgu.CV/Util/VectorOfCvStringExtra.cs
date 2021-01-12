//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;

namespace Emgu.CV.Util
{
    /// <summary>
    /// Wrapped class of the C++ standard vector of CvString.
    /// </summary>
    public partial class VectorOfCvString : Emgu.Util.UnmanagedObject, IInputOutputArray
    {

        /// <summary>
        /// Convert the standard vector to an array of String
        /// </summary>
        /// <returns>An array of String</returns>
        public String[] ToArray()
        {
            String[] names = new String[Size];
            for (int i = 0; i < names.Length; i++)
            {
                names[i] = this[i].ToString();
            }
            return names;
        }

        /// <summary>
        /// Create a VectorOfCvString object from an array of String
        /// </summary>
        /// <param name="strings">The strings to be placed in this VectorOfCvString</param>
        public VectorOfCvString(String[] strings)
            : this()
        {
            foreach (String s in strings)
                using (CvString cs = new CvString(s))
                    Push(cs);
        }
    }
}
