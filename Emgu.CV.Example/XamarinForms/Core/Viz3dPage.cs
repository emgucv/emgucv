//----------------------------------------------------------------------------
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if !(NETFX_CORE || __ANDROID__ || __IOS__ || __MACOS__)
using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;

namespace Emgu.CV.XamarinForms
{
    public class Viz3dPage : ButtonTextImagePage
    {
        public Viz3dPage()
        {
            var button = this.GetButton();
            button.Text = "Show 3D Viz";
            button.Clicked += (sender, args) =>
            {
                using (Viz3d viz = new Viz3d("show_simple_widgets"))
                {
                    viz.SetBackgroundMeshLab();
                    using (WCoordinateSystem coor = new WCoordinateSystem())
                    {
                        viz.ShowWidget("coor", coor);
                        using (WCube cube = new WCube(
                            new MCvPoint3D64f(-.5, -.5, -.5),
                            new MCvPoint3D64f(.5, .5, .5),
                            true,
                            new MCvScalar(255, 255, 255)))
                        {
                            viz.ShowWidget("cube", cube);
                            using (WCube cube0 = new WCube(
                                new MCvPoint3D64f(-1, -1, -1),
                                new MCvPoint3D64f(-.5, -.5, -.5),
                                false,
                                new MCvScalar(123, 45, 200)))
                            {
                                viz.ShowWidget("cub0", cube0);
                                viz.Spin();
                            }
                        }
                    }
                }
            };
        }
    }
}
#endif
