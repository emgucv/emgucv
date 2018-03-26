//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if NETFX_CORE || NETSTANDARD1_4
namespace System
{
    /// <summary>
    /// A converter
    /// </summary>
    /// <typeparam name="TInput">The input object type</typeparam>
    /// <typeparam name="TOutput">The output object type</typeparam>
    /// <param name="input">The input object</param>
    /// <returns>The converted object</returns>
    public delegate TOutput Converter<TInput, TOutput>(TInput input);
}

namespace Emgu.CV
{
    /// <summary>
    /// This class provide additional extension methods
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Convert an array of elements
        /// </summary>
        /// <typeparam name="TInput">The input array type</typeparam>
        /// <typeparam name="TOutput">The output array type</typeparam>
        /// <param name="array">The array to be converted</param>
        /// <param name="converter">The converter</param>
        /// <returns>The converted array</returns>
        public static TOutput[] ConvertAll<TInput, TOutput>(TInput[] array, System.Converter<TInput, TOutput> converter)
        {
            TOutput[] result = new TOutput[array.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = converter(array[i]);
            }
            return result;
        }
    }
}
#endif