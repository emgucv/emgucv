//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.Util
{
   /// <summary>
   /// The default exception to be thrown when error encounter in Open CV 
   /// </summary>
#if !NETFX_CORE
   [Serializable]
#endif
   public class CvException : Exception
   {
      private int _status;
      private string _functionName;
      private String _errMsg;
      private String _fileName;
      private int _line;

      /// <summary>
      /// The numeric code for error status
      /// </summary>
      public int Status
      {
         get { return _status; }
         set { _status = value; }
      }

      /// <summary>
      /// The corresponding error string for the Status code
      /// </summary>
      public String ErrorStr
      {
         get { return CvInvoke.ErrorStr(Status); }
      }

      /// <summary>
      /// The name of the function the error is encountered
      /// </summary>
      public string FunctionName
      {
         get { return _functionName; }
         set { _functionName = value; }
      }

      /// <summary>
      /// A description of the error
      /// </summary>
      public String ErrorMessage
      {
         get { return _errMsg; }
         set { _errMsg = value; }
      }

      /// <summary>
      /// The source file name where error is encountered
      /// </summary>
      public String FileName
      {
         get { return _fileName; }
         set { _fileName = value; }
      }

      /// <summary>
      /// The line number in the souce where error is encountered
      /// </summary>
      public int Line
      {
         get { return _line; }
         set { _line = value; }
      }

      private CvException()
      {
      }

      /// <summary>
      /// The default exception to be thrown when error is encountered in Open CV 
      /// </summary>
      /// <param name="status">The numeric code for error status</param>
      /// <param name="funcName">The source file name where error is encountered</param>
      /// <param name="errMsg">A description of the error</param>
      /// <param name="fileName">The source file name where error is encountered</param>
      /// <param name="line">The line number in the souce where error is encountered</param>
      public CvException(int status, String funcName, String errMsg, String fileName, int line)
         : base("OpenCV: " + errMsg)
      {
         _status = status;
         _functionName = funcName;
         _errMsg = errMsg;
         _fileName = fileName;
         _line = line;
      }
   }
}
