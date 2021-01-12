//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Methods for comparing two array
    /// </summary>
    public enum TemplateMatchingType
    {
        /// <summary>
        /// R(x,y)=sumx',y'[T(x',y')-I(x+x',y+y')]2
        /// </summary>
        Sqdiff = 0,
        /// <summary>
        /// R(x,y)=sumx',y'[T(x',y')-I(x+x',y+y')]2/sqrt[sumx',y'T(x',y')2 sumx',y'I(x+x',y+y')2]
        /// </summary>
        SqdiffNormed = 1,
        /// <summary>
        /// R(x,y)=sumx',y'[T(x',y') I(x+x',y+y')]
        /// </summary>
        Ccorr = 2,
        /// <summary>
        /// R(x,y)=sumx',y'[T(x',y') I(x+x',y+y')]/sqrt[sumx',y'T(x',y')2 sumx',y'I(x+x',y+y')2]
        /// </summary>
        CcorrNormed = 3,
        /// <summary>
        /// R(x,y)=sumx',y'[T'(x',y') I'(x+x',y+y')],
        /// where T'(x',y')=T(x',y') - 1/(wxh) sumx",y"T(x",y")
        ///    I'(x+x',y+y')=I(x+x',y+y') - 1/(wxh) sumx",y"I(x+x",y+y")
        /// </summary>
        Ccoeff = 4,
        /// <summary>
        /// R(x,y)=sumx',y'[T'(x',y') I'(x+x',y+y')]/sqrt[sumx',y'T'(x',y')2 sumx',y'I'(x+x',y+y')2]
        /// </summary>
        CcoeffNormed = 5
    }
}
