//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

//#define LINUX
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using Emgu.Util;

#if LINUX
using System.Runtime.InteropServices;
#else
using System.Speech.Synthesis;
#endif

namespace Emgu.RPC.Speech
{
   /// <summary>
   /// An implementation of a singleton speech service
   /// </summary>
   [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
   public class SpeechService : DisposableObject, ISpeechService
   {
#if LINUX
        private const String LIB_FESTIVAL = "libFestival.so.1.96.0";
        /// <summary>
        /// This must be called before any other festival functions may be called. It sets up the synthesizer system.
        /// </summary>
        /// <param name="load_init_files">The first argument if true, causes the system set up files to be loaded (which is normallly what is necessary)</param>
        /// <param name="heapsize">the second argument is the initial size of the Scheme heap, this should normally be 210000 unless you envisage processing very large Lisp structures. </param>
        [DllImport(LIB_FESTIVAL)]
        private extern static void festival_initialize(int load_init_files, int heapsize); 
        
        /// <summary>
        /// Say the contents of the given text
        /// </summary>
        /// <param name="text">The text to speak</param>
        /// <returns>Returns TRUE or FALSE depending on where this was successful</returns>
        [DllImport(LIB_FESTIVAL)]
        private extern static int festival_say_text([MarshalAs(UnmanagedType.LPStr)] String text); 
#else
      private SpeechSynthesizer _synthesizer;
#endif

      public SpeechService()
      {
#if LINUX
            festival_initialize(1, 210000);
#else
         _synthesizer = new SpeechSynthesizer();
#endif
      }

      public void Speak(String sentences)
      {
#if LINUX
            festival_say_text(sentences);
#else
         _synthesizer.SpeakAsync(sentences);
#endif
      }

      protected override void DisposeObject()
      {
#if LINUX
#else
         _synthesizer.Dispose();
#endif
      }
   }
}
