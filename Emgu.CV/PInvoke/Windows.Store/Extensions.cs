using System;
using System.Text;

namespace Emgu.CV
{
   public delegate TOutput Converter<TInput, TOutput>(TInput input);

   public static class Extensions
   {
      public static TOutput[] ConvertAll<TInput, TOutput>(TInput[] array, Converter<TInput, TOutput> converter)
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