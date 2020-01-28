//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// The kind of sequence available
    /// </summary>
    public enum SeqKind
    {
        /// <summary>
        /// Generic (unspecified) kind of sequence 
        /// </summary>
        Generic = (0 << SeqConst.EltypeBits),
        /// <summary>
        /// Dense sequence subtypes 
        /// </summary>
        Curve = (1 << SeqConst.EltypeBits),
        /// <summary>
        /// Dense sequence subtypes 
        /// </summary>
        BinTree = (2 << SeqConst.EltypeBits),
        /// <summary>
        /// Sparse sequence (or set) subtypes 
        /// </summary>
        Graph = (1 << SeqConst.EltypeBits),
        /// <summary>
        /// Sparse sequence (or set) subtypes 
        /// </summary>
        Subdiv2D = (2 << SeqConst.EltypeBits)
    }
}
