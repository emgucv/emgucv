using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    public struct MCvSubdiv2D
    {     
       int       flags;         /* micsellaneous flags */          
    int       header_size;   /* size of sequence header */      
    IntPtr h_prev; /* previous sequence */        
    IntPtr h_next; /* next sequence */            
    IntPtr v_prev; /* 2nd previous sequence */
    IntPtr v_next;  /* 2nd next sequence */        
    int       total;          /* total number of elements */            
    int       elem_size;      /* size of sequence element in bytes */   
    IntPtr     block_max;      /* maximal bound of the last block */     
    IntPtr     ptr;            /* current write pointer */               
    int       delta_elems;    /* how many elements allocated when the seq grows */  
    IntPtr storage;    /* where the seq is stored */             
    IntPtr free_blocks;  /* free blocks list */                    
    IntPtr first; /* pointer to the first sequence block */
   IntPtr free_elems;   
    int active_count;
    IntPtr edges;
    int  quad_edges;            
    int  is_geometry_valid;     
    IntPtr recent_edge; 
    MCvPoint2D32f  topleft;      
    MCvPoint2D32f  bottomright;
    }
}
