//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
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
    }
}
