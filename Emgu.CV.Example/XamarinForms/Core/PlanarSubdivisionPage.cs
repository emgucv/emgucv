//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using PlanarSubdivisionExample;

namespace Emgu.CV.XamarinForms
{
    public class PlanarSubdivisionPage : ButtonTextImagePage
    {
       public PlanarSubdivisionPage()
       {
          var button = this.GetButton();
          button.Text = "Calculate Planar Subdivision";
          button.Clicked += (sender, args) =>
          {
             int maxValue = 600, pointCount = 30;

             SetImage(DrawSubdivision.Draw(maxValue, pointCount));
          };
       }
    }
}
