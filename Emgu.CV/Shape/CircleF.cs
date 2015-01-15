//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Xml.Serialization;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   ///<summary> A circle </summary>
   [StructLayout(LayoutKind.Sequential)]  
   public struct CircleF : IEquatable<CircleF>
   {
      private PointF _center;

      // The radius of the circle
      private float _radius;

      ///<summary> Create a circle with the specific center and radius </summary>
      ///<param name="center"> The center of this circle </param>
      ///<param name="radius"> The radius of this circle </param>
      public CircleF(PointF center, float radius)
      {
         _center = center;
         _radius = radius;
      }

      ///<summary> Get or Set the center of the circle </summary>
      public PointF Center
      {
         get { return _center; }
         set { _center = value; }
      }

      ///<summary> The radius of the circle </summary>
      [XmlAttribute("Radius")]
      public float Radius { get { return _radius; } set { _radius = value; } }

      ///<summary> The area of the circle </summary>
      public double Area
      {
         get
         {
            return _radius * _radius * Math.PI;
         }
      }

      /// <summary>
      /// Compare this circle with <paramref name="circle2"/>
      /// </summary>
      /// <param name="circle2">The other box to be compared</param>
      /// <returns>true if the two boxes equals</returns>
      public bool Equals(CircleF circle2)
      {
         return Center.Equals(circle2.Center) && Radius == circle2.Radius;
      }
   }
}
