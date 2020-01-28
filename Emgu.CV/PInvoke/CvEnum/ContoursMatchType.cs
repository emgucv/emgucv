//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Type used by cvMatchShapes
    /// </summary>
    public enum ContoursMatchType
    {
        /// <summary>
        /// I_1(A,B)=sum_{i=1..7} abs(1/m^A_i - 1/m^B_i) where m^A_i=sign(h^A_i) log(h^A_i), m^B_i=sign(h^B_i) log(h^B_i), h^A_i, h^B_i - Hu moments of A and B, respectively
        /// </summary> 
        I1 = 1,
        /// <summary>
        /// I_2(A,B)=sum_{i=1..7} abs(m^A_i - m^B_i) where m^A_i=sign(h^A_i) log(h^A_i), m^B_i=sign(h^B_i) log(h^B_i), h^A_i, h^B_i - Hu moments of A and B, respectively
        /// </summary>
        I2 = 2,
        /// <summary>
        /// I_3(A,B)=sum_{i=1..7} abs(m^A_i - m^B_i)/abs(m^A_i) where m^A_i=sign(h^A_i) log(h^A_i), m^B_i=sign(h^B_i) log(h^B_i), h^A_i, h^B_i - Hu moments of A and B, respectively
        /// </summary>
        I3 = 3
    }
}
