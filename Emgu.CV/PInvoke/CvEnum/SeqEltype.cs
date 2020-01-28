//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Sequence element type
    /// </summary>
    public enum SeqEltype
    {
        ///<summary>
        ///  (x,y) 
        ///</summary>
        Point = (((int)DepthType.Cv32S) + (((2) - 1) << 3)),
        ///<summary>  
        ///freeman code: 0..7 
        ///</summary>
        Code = DepthType.Cv8U + 0 << 3,
        ///<summary>  
        ///unspecified type of sequence elements 
        ///</summary>
        Generic = 0,
        ///<summary>  
        ///=6 
        ///</summary>
        Ptr = 7,
        ///<summary>  
        ///pointer to element of other sequence 
        ///</summary>
        Ppoint = 7,
        ///<summary>  
        ///index of element of some other sequence 
        ///</summary>
        Index = DepthType.Cv32S,
        ///<summary>  
        ///next_o, next_d, vtx_o, vtx_d 
        ///</summary>
        GraphEdge = 0,
        ///<summary>  
        ///first_edge, (x,y) 
        ///</summary>
        GraphVertex = 0,
        ///<summary>  
        ///vertex of the binary tree   
        ///</summary>
        TrainAtr = 0,
        ///<summary>  
        ///connected component  
        ///</summary>
        ConnectedComp = 0,
        ///<summary>  
        ///(x,y,z)  
        ///</summary>
        Point3D = DepthType.Cv32F + 2 << 3

    }
}
