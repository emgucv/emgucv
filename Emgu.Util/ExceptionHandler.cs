using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.Util
{
    /// <summary>
    /// The exception handler
    /// </summary>
    public abstract class ExceptionHandler
    {
        /// <summary>
        /// Handle exception
        /// </summary>
        /// <param name="ex">The exception to be handled</param>
        /// <returns>True if the exception has been handled, or false if the exception should be rethrown and the application terminated.</returns>
        public abstract bool HandleException(Exception ex);
    }
}
