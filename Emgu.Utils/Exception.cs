//#define LINUX
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
#if LINUX
#else
using System.Speech;
using System.Speech.Synthesis;
#endif

namespace Emgu
{
    /// <summary>
    /// The exception enviormental information for Emgu class librarys
    /// </summary>
    [XmlRoot("ExceptionEnviorment")]
    public static class ExceptionEnviorment
    {
        private static ExceptionLevel _currentExceptionLevel = ExceptionLevel.Minor;

        private static ExceptionDictionary _dic = new ExceptionDictionary();
        
        /// <summary>
        /// The current exception level
        /// </summary>
        [XmlAttribute("ExceptionLevel")]
        public static ExceptionLevel ExceptionLevel { get { return _currentExceptionLevel; } set { _currentExceptionLevel = value; } }
        
        public static ExceptionDictionary ExceptionDictionary { get { return _dic; } }
#if LINUX
#else
        public static SpeechSynthesizer Synthesizer = new SpeechSynthesizer(); 
#endif
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

    public class ExceptionDetail
    {
        private int _level;

        public int Level { get { return _level; } }

        public ExceptionDetail(int level)
        {
            _level = level;
        }
    }

    #region Define builtin exceptions
    public enum ExceptionHeader
    {
        UnsupportedFileType,
        UnimplementedFunction,
        MinorException,
        LowException, 
        MediumException,
        HighException,
        CriticalException,
    }

    public class ExceptionDictionary : Dictionary<int, ExceptionDetail>
    {
        public ExceptionDictionary()
            : base()
        {
            Add((int)ExceptionHeader.UnsupportedFileType,
                new ExceptionDetail((int)ExceptionLevel.Medium));

            Add((int)ExceptionHeader.LowException,
                new ExceptionDetail((int)ExceptionLevel.Low));

            Add((int)ExceptionHeader.MinorException,
                new ExceptionDetail((int)ExceptionLevel.Minor));

            Add((int)ExceptionHeader.MediumException,
                new ExceptionDetail((int)ExceptionLevel.Medium));

            Add((int)ExceptionHeader.HighException,
                new ExceptionDetail((int)ExceptionLevel.High));

            Add((int)ExceptionHeader.CriticalException,
                new ExceptionDetail((int)ExceptionLevel.Critical));

            Add((int)ExceptionHeader.UnimplementedFunction,
                new ExceptionDetail((int)ExceptionLevel.Low));
        }
    }
    #endregion

    /// <summary>
    /// The exception class used by Emgu programs
    /// </summary>
    public class Exception : System.Exception
    {
        private ExceptionHeader _exceptionHeader;

        /// <summary>
        /// Create an exception with the specific header and message
        /// </summary>
        /// <param name="hdr"></param>
        /// <param name="message"></param>
        public Exception(ExceptionHeader hdr, string message)
            : base(message)
        {
            _exceptionHeader = hdr;
        }

        public ExceptionDetail ExceptionBody { get { return ExceptionEnviorment.ExceptionDictionary[ (int)_exceptionHeader]; } }

        public ExceptionHeader ExceptionHeader { get {return _exceptionHeader;}}

        /// <summary>
        /// Check if the severity of the current exception is greater or equal to the serverity of the Exception Enviorment
        /// </summary>
        /// <returns>True if the serverity is greater or equal to the one defined in the Exception Enviorment</returns>
        public bool isSevere()
        {
            return ((int)ExceptionBody.Level >= (int) ExceptionEnviorment.ExceptionLevel);
        }

        /// <summary>
        /// Alert regardness of the severity of the exception
        /// </summary>
        /// <param name="syn">If true, the operation is synchronous, otherwise, asynchronous</param>
        public void Alert(bool syn)
        {
#if LINUX
#else
            ExceptionEnviorment.Synthesizer.SpeakAsync(Message);
#endif
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
