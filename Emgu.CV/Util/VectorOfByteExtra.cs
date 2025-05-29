//----------------------------------------------------------------------------
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using System.IO;

namespace Emgu.CV.Util
{
   public partial class VectorOfByte : Emgu.CV.Util.UnmanagedVector, ISerializable , IInputOutputArray

   {
        /// <summary>
        /// Initializes a new instance of the <see cref="VectorOfByte"/> class using the data from the specified stream.
        /// </summary>
        /// <param name="stream">The stream containing the data to initialize the vector. The length of the stream determines the size of the vector.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="stream"/> is <c>null</c>.</exception>
        /// <exception cref="IOException">Thrown when an I/O error occurs during the operation.</exception>
        public VectorOfByte(Stream stream)
            : this((int)stream.Length)
        {
            stream.CopyTo(this);
        }
   }
}
