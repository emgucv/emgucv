//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Tao.Platform.Windows;
using OsgViewer;
using OsgGA;

namespace Simlpe3DReconstruction
{
   public partial class OsgControl : SimpleOpenGlControl, IDisposable
   {
      private GraphicsWindow _gw;
      private Viewer _viewer;

      public OsgControl()
         : base()
      {
         if (!DesignMode)
         {
            _viewer = new Viewer();
            _gw = _viewer.setUpViewerAsEmbeddedInWindow(0, 0, Width, Height);
            _viewer.setCameraManipulator(new TrackballManipulator());
            _viewer.realize();
            InitializeContexts();
         }
      }

      /// <summary>
      /// Get the Osg Viewer use by this control. Do not dispose this object
      /// </summary>
      public Viewer Viewer
      {
         get { return _viewer; }
      }

      protected override void OnMouseMove(MouseEventArgs e)
      {
         if (!DesignMode)
         {
            _gw.getEventQueue().mouseMotion(e.X, e.Y);
         }
         base.OnMouseMove(e);
      }


      protected override void OnKeyPress(KeyPressEventArgs e)
      {
         if (!DesignMode)
         {
            _gw.getEventQueue().keyPress((int)e.KeyChar);
         }
         base.OnKeyPress(e);
      }

      protected override void OnMouseDown(MouseEventArgs e)
      {
         _gw.getEventQueue().mouseButtonPress(e.X, e.Y, ConvertMouse(e.Button));
         base.OnMouseDown(e);
      }

      protected override void OnMouseUp(MouseEventArgs e)
      {
         _gw.getEventQueue().mouseButtonRelease(e.X, e.Y, ConvertMouse(e.Button));
         base.OnMouseUp(e);
      }

      protected override void OnResize(EventArgs e)
      {
         if (!DesignMode && _gw != null)
         {
            _gw.getEventQueue().windowResize(0, 0, ClientSize.Width,
               ClientSize.Height);
            _gw.resized(0, 0, Width, Height);
         }
         base.OnResize(e);
      }

      private static uint ConvertMouse(MouseButtons button)
      {
         switch (button)
         {
            case MouseButtons.Left: return 1; 
            case MouseButtons.Middle: return 2;
            case MouseButtons.Right: return 3; 
         }
         return 0;
      }

      #region IDisposable Members
      /// <summary>
      /// Release all unmanaged resources
      /// </summary>
      public new void Dispose()
      {
         _viewer.Dispose();
         _gw.Dispose();
         DestroyContexts();
         base.Dispose();
      }
      #endregion
   }
}

