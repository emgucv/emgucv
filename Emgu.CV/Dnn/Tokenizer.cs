//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Dnn
{
    /// <summary>
    /// High-level tokenizer wrapper for DNN usage (cv::dnn::Tokenizer). Provides
    /// a simple API to encode and decode tokens for LLMs.
    /// </summary>
    public partial class Tokenizer : UnmanagedObject
    {
        private Tokenizer(IntPtr ptr)
        {
            _ptr = ptr;
        }

        /// <summary>
        /// Load a tokenizer from a model directory.
        /// </summary>
        /// <param name="modelConfig">
        /// Path to the model's "config.json" file. The json file may contain a
        /// "method" field ("BPE" by default, "Gemma" or "SentencePiece"); for the
        /// BPE method a "model_type" field selects the model family ("gpt2",
        /// "gpt4", "qwen2" or "qwen2.5"). A "tokenizer.json" produced by the
        /// corresponding model family must reside in the same directory.
        /// </param>
        /// <returns>A Tokenizer ready for use.</returns>
        public static Tokenizer Load(String modelConfig)
        {
            using (CvString csModelConfig = new CvString(modelConfig))
                return new Tokenizer(DnnInvoke.cveDnnTokenizerLoad(csModelConfig));
        }

        /// <summary>
        /// Encode UTF-8 text to token ids.
        /// </summary>
        /// <param name="text">UTF-8 input string.</param>
        /// <returns>The token ids.</returns>
        public int[] Encode(String text)
        {
            using (CvString csText = new CvString(text))
            using (VectorOfInt viIds = new VectorOfInt())
            {
                DnnInvoke.cveDnnTokenizerEncode(_ptr, csText, viIds);
                return viIds.ToArray();
            }
        }

        /// <summary>
        /// Decode token ids back to UTF-8 text.
        /// </summary>
        /// <param name="tokens">The token ids.</param>
        /// <returns>The decoded UTF-8 string.</returns>
        public String Decode(int[] tokens)
        {
            using (VectorOfInt viTokens = new VectorOfInt(tokens))
            using (CvString csText = new CvString())
            {
                DnnInvoke.cveDnnTokenizerDecode(_ptr, viTokens, csText);
                return csText.ToString();
            }
        }

        /// <summary>
        /// Release the memory associated with this tokenizer.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                DnnInvoke.cveDnnTokenizerRelease(ref _ptr);
            }
        }
    }

    public static partial class DnnInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDnnTokenizerLoad(IntPtr modelConfig);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnTokenizerRelease(ref IntPtr tokenizer);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnTokenizerEncode(IntPtr tokenizer, IntPtr text, IntPtr ids);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnTokenizerDecode(IntPtr tokenizer, IntPtr tokens, IntPtr text);
    }
}
