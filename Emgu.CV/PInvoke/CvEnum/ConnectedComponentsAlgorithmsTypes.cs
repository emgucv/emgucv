//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Connected Components Algorithms Types
    /// </summary>
    public enum ConnectedComponentsAlgorithmsTypes
    {
        /// <summary>
        /// BBDT algorithm for 8-way connectivity, SAUF algorithm for 4-way connectivity. The parallel implementation is available for both BBDT and SAUF.
        /// </summary>
        Default = -1,
        /// <summary>
        /// SAUF algorithm for 8-way connectivity, SAUF algorithm for 4-way connectivity. The parallel implementation is available for SAUF.
        /// </summary>
        Wu = 0,
        /// <summary>
        /// BBDT algorithm for 8-way connectivity, SAUF algorithm for 4-way connectivity. The parallel implementation is available for both BBDT and SAUF.
        /// </summary>
        Grana = 1,
        /// <summary>
        /// Spaghetti algorithm for 8-way connectivity, SAUF algorithm for 4-way connectivity.
        /// </summary>
        Bolelli = 2,
        /// <summary>
        /// Same as WU. It is preferable to use the flag with the name of the algorithm (SAUF) rather than the one with the name of the first author (WU).
        /// </summary>
        Sauf = 3,
        /// <summary>
        /// Same as GRANA. It is preferable to use the flag with the name of the algorithm (BBDT) rather than the one with the name of the first author (GRANA).
        /// </summary>
        Bbdt = 4,
        /// <summary>
        /// Same as BOLELLI. It is preferable to use the flag with the name of the algorithm (SPAGHETTI) rather than the one with the name of the first author (BOLELLI).
        /// </summary>
        Spaghetti = 5,  
    }

}
