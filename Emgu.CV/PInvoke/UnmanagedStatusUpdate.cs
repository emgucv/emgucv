using System;
using System.Collections.Generic;
using System.Text;
using Emgu.Util;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
   public static class UnmanagedStatusUpdate
   {
      public static event UnmanagedStatusUpdateHandler Updated;

      public delegate void UnmanagedStatusUpdateHandler(object sender, EventArgs<String, int> e);

      static UnmanagedStatusUpdate()
      {
         redirectUnmanagedStatusUpdate(_customeHandler);
      }

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private static extern void redirectUnmanagedStatusUpdate(
         MessageCallback statusReport_handler);

      private delegate void MessageCallback(String message, int type);

      private static readonly MessageCallback _customeHandler = ( MessageCallback ) MessageHandler;

      private static void MessageHandler(String message, int type)
      {
         if (Updated != null)
         {
            Updated(null, new EventArgs<string, int>(message, type));
         }
      }
   }
}
