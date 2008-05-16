using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Emgu
{
    /// <summary>
    /// The exception enviormental information for Emgu class librarys
    /// </summary>
    [XmlRoot("ExceptionEnviorment")]
    public static class ExceptionEnviorment
    {
        private static ExceptionLevel _currentExceptionLevel = ExceptionLevel.Minor;
        
        /// <summary>
        /// The current exception level
        /// </summary>
        [XmlAttribute("ExceptionLevel")]
        public static ExceptionLevel ExceptionLevel { get { return _currentExceptionLevel; } set { _currentExceptionLevel = value; } }
        
        }

    /// <summary>
    /// The Level of exception
    /// </summary>
    public enum ExceptionLevel
    {
        /// <summary>
        /// Indicate the Exception level is Minor
        /// </summary>
        Minor = 1,
        /// <summary>
        /// Indicate the Exception level is Low
        /// </summary>
        Low = 2,
        /// <summary>
        /// Indicate the Exception level is Medium
        /// </summary>
        Medium = 3,
        /// <summary>
        /// Indicate the Exception level is High
        /// </summary>
        High = 4,
        /// <summary>
        /// Indicate the Exception level is Critical
        /// </summary>
        Critical = 5,
    }

    /// <summary>
    /// The exception class used by Emgu programs
    /// </summary>
    public class PrioritizedException : Exception
    {
        private ExceptionLevel _exceptionHeader;

        /// <summary>
        /// Create an exception with the specific header and message
        /// </summary>
        /// <param name="hdr"></param>
        /// <param name="message"></param>
        public PrioritizedException(ExceptionLevel hdr, string message)
            : base(message)
        {
            _exceptionHeader = hdr;
        }

        /// <summary>
        /// The level of Exception
        /// </summary>
        public ExceptionLevel ExceptionLevel { get { return _exceptionHeader; } }

        /// <summary>
        /// Check if the severity of the current exception is greater or equal to the serverity of the Exception Enviorment
        /// </summary>
        /// <returns>True if the serverity is greater or equal to the one defined in the Exception Enviorment</returns>
        public bool isSevere()
        {
            return ((int)ExceptionLevel >= (int)ExceptionEnviorment.ExceptionLevel);
        }

        /// <summary>
        /// Alert regardness of the severity of the exception
        /// </summary>
        /// <param name="syn">If true, the operation is synchronous, otherwise, asynchronous</param>
        public void Alert(bool syn)
        {
            if (syn)
                MessageBox.Show(Message);
            else
            {
                System.Threading.Thread t = new System.Threading.Thread(delegate() { MessageBox.Show(Message); });
                t.Start();
            }
        }

        /// <summary>
        /// Alert if the exception is severe
        /// </summary>
        /// <param name="syn">If true, the operation is synchronous, otherwise, asynchronous</param>
        public void AlertIfServere(bool syn)
        {
            if (isSevere())
                Alert(syn);
        }

    }
}
