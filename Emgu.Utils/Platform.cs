using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu
{
    /// <summary>
    /// Provide information for the platform which is using 
    /// </summary>
    public static class Platform
    {
        /// <summary>
        /// The operating system that is using
        /// </summary>
        public static OS OperationSystem
        {
            get
            {
                int p = (int)Environment.OSVersion.Platform;
                if ((p == 4) || (p == 128))
                {
                    return OS.Linux;
                }
                else
                {
                    return OS.Windows;
                } 
            }
        }
    }

    /// <summary>
    /// Type of operating system
    /// </summary>
    public enum OS
    {
        /// <summary>
        /// Windows
        /// </summary>
        Windows, 
        /// <summary>
        /// Linux
        /// </summary>
        Linux
    }
}
