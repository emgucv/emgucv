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
        /// Spaghetti algorithm for 8-way connectivity, Spaghetti4C algorithm for 4-way connectivity.
        /// </summary>
        Default = -1,
        /// <summary>
        /// SAUF algorithm for 8-way connectivity, SAUF algorithm for 4-way connectivity. The parallel implementation described is available for SAUF.
        /// </summary>
        Wu = 0,
        /// <summary>
        /// BBDT algorithm for 8-way connectivity, SAUF algorithm for 4-way connectivity. The parallel implementation is available for both BBDT and SAUF.
        /// </summary>
        Grana = 1,
        /// <summary>
        /// Spaghetti algorithm for 8-way connectivity, Spaghetti4C algorithm for 4-way connectivity. The parallel implementation described is available for both Spaghetti and Spaghetti4C.
        /// </summary>
        Bolelli = 2,
        /// <summary>
        /// Same as WU. It is preferable to use the flag with the name of the algorithm (SAUF) rather than the one with the name of the first author (Wu).
        /// </summary>
        Sauf = 3,
        /// <summary>
        /// Same as Grana. It is preferable to use the flag with the name of the algorithm (BBDT) rather than the one with the name of the first author (Grana).
        /// </summary>
        Bbdt = 4,
        /// <summary>
        /// Same as Bolelli. It is preferable to use the flag with the name of the algorithm (Spaghetti) rather than the one with the name of the first author (Bolelli).
        /// </summary>
        Spaghetti = 5,  
    }

}
