using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace Emgu.CV.UI
{
   /// <summary>
   /// A picture box with pan and zoom functionality
   /// </summary>
   public class PanAndZoomPictureBox : PictureBox
   {
      private bool _panableAndZoomable;
      /// <summary>
      /// The zoom scale of the image to be displayed
      /// </summary>
      private double _zoomScale;

      private Point _mouseDownPosition;
      private MouseButtons _mouseDownButton;
      private Point _bufferPoint;
      private HScrollBar horizontalScrollBar;
      private VScrollBar verticalScrollBar;
      private InterpolationMode _interpolationMode = InterpolationMode.NearestNeighbor;
      private static readonly Cursor _defaultCursor = Cursors.Cross;

      /// <summary>
      /// The available zoom levels for the displayed image 
      /// </summary>
      public static double[] ZoomLevels = new double[] { 0.125, 0.25, 0.5, 1.0, 2.0, 4.0, 8.0 };

      /// <summary>
      /// Create a picture box with pan and zoom functionality
      /// </summary>
      public PanAndZoomPictureBox()
      {
         InitializeComponent();
         _zoomScale = 1.0;

         SetScrollBarVisibilityAndMaxMin();
         //Enable double buffering
         ResizeRedraw = false;
         DoubleBuffered = true;
         //this.BorderStyle = BorderStyle.Fixed3D;
         PanableAndZoomable = true;
      }

      /// <summary>
      /// Get or Set the property to enable(disable) Pan and Zoom
      /// </summary>
      protected bool PanableAndZoomable
      {
         get
         {
            return _panableAndZoomable;
         }
         set
         {
            if (_panableAndZoomable != value)
            {
               _panableAndZoomable = value;
               if (_panableAndZoomable)
               {
                  MouseEnter += OnMouseEnter;
                  MouseWheel += OnMouseWheel;
                  MouseMove += OnMouseMove;
                  Resize += OnResize;
                  MouseDown += OnMouseDown;
                  MouseUp += OnMouseUp;
               }
               else
               {
                  MouseEnter -= OnMouseEnter;
                  MouseWheel -= OnMouseWheel;
                  MouseMove -= OnMouseMove;
                  Resize -= OnResize;
                  MouseDown -= OnMouseDown;
                  MouseUp -= OnMouseUp;
               }
            }
         }
      }

      private void OnMouseDown(object sender, MouseEventArgs e)
      {
         _mouseDownPosition = e.Location;
         _mouseDownButton = e.Button;

         _bufferPoint = Point.Empty;
         if (e.Button == MouseButtons.Middle)
            Cursor = Cursors.Hand;
      }

      private void OnMouseUp(object sender, MouseEventArgs e)
      {
         if (e.Button == MouseButtons.Left && _mouseDownButton == MouseButtons.Left)
         {
            ReverseRectangle();
            Size viewSize = GetViewSize();
            Rectangle imageRegion = new Rectangle(Point.Empty, viewSize);
            if (!imageRegion.Contains(_mouseDownPosition))
               return;

            Rectangle selectedRectangle = GetSelectedRectangle(e.X, e.Y);

            if ((selectedRectangle.Width / _zoomScale) > 2 && (selectedRectangle.Height / _zoomScale) > 2)
            {
               int h = Math.Min(horizontalScrollBar.Maximum, horizontalScrollBar.Value + (int)(selectedRectangle.Location.X / _zoomScale));
               int v = Math.Min(verticalScrollBar.Maximum, verticalScrollBar.Value + (int)(selectedRectangle.Location.Y / _zoomScale));

               _zoomScale = _zoomScale * viewSize.Width / selectedRectangle.Width;

               SetScrollBarVisibilityAndMaxMin();

               horizontalScrollBar.Value = h;
               verticalScrollBar.Value = v;

               Invalidate();
            }
         }
         Cursor = _defaultCursor;
         _mouseDownButton = MouseButtons.None;
      }

      private void OnMouseEnter(object sender, EventArgs e)
      {  //set this as the active control 
         Form f = TopLevelControl as Form;
         if (f != null) f.ActiveControl = this;
      }

      private void OnResize(object sender, EventArgs e)
      {
         Size viewSize = GetViewSize();
         if (base.Image != null && viewSize.Width > 0 && viewSize.Height > 0)
         {
            SetScrollBarVisibilityAndMaxMin();
            Invalidate();
         }
      }

      private void OnMouseWheel(object sender, MouseEventArgs e)
      {  //handle the mouse whell scroll (for zooming)
         double scale = 1.0;
         if (e.Delta > 0)
         {
            scale = 2.0;
         }
         else if (e.Delta < 0)
         {
            scale = 0.5;
         }
         else
            return;

         SetZoomScale(ZoomScale * scale, e.Location);
      }

      /// <summary>
      /// The event to fire when the zoom scale is changed
      /// </summary>
      public event EventHandler OnZoomScaleChange;

      #region Handling ScrollBars

      private void OnScroll(object sender, ScrollEventArgs e)
      {
         Invalidate();
      }

      /// <summary>
      /// Get or Set the interpolation mode for zooming operation
      /// </summary>
      [Bindable(false)]
      [Category("Design")]
      [DefaultValue(InterpolationMode.NearestNeighbor)]
      public InterpolationMode InterpolationMode
      {
         get
         {
            return _interpolationMode;
         }
         set
         {
            _interpolationMode = value;
         }
      }


      /// <summary>
      /// Paint the image
      /// </summary>
      /// <param name="pe">The paint event</param>
      protected override void OnPaint(PaintEventArgs pe)
      {
         if (Image != null          //image is set
            &&          //either pan or zoom
            (_zoomScale != 1.0f || 
            (horizontalScrollBar.Visible && horizontalScrollBar.Value != 0) || 
            (verticalScrollBar.Visible && verticalScrollBar.Value != 0)))
         {
            using (Matrix transform = pe.Graphics.Transform)
            {
               transform.Scale((float)_zoomScale, (float)_zoomScale, MatrixOrder.Append);
               transform.Translate(
                  horizontalScrollBar.Visible ? -horizontalScrollBar.Value : 0,
                  verticalScrollBar.Visible ? -verticalScrollBar.Value : 0);
               pe.Graphics.Transform = transform;
            }

            if (pe.Graphics.InterpolationMode != _interpolationMode)
               pe.Graphics.InterpolationMode = _interpolationMode;
         }

         base.OnPaint(pe);
      }

      private void SetScrollBarVisibilityAndMaxMin()
      {
         #region determine if the scroll bar should be visible or not
         horizontalScrollBar.Visible = false;
         verticalScrollBar.Visible = false;

         if (Image == null) return;

         // If the image is wider than the PictureBox, show the HScrollBar.
         horizontalScrollBar.Visible =
            (int)(Image.Size.Width * _zoomScale) > ClientSize.Width;

         // If the image is taller than the PictureBox, show the VScrollBar.
         verticalScrollBar.Visible =
            (int)(Image.Size.Height * _zoomScale) > ClientSize.Height;
         
         #endregion

         // Set the Maximum, LargeChange and SmallChange properties.
         if (horizontalScrollBar.Visible)
         {  // If the offset does not make the Maximum less than zero, set its value.            
            horizontalScrollBar.Maximum =
               Image.Size.Width -
               (int)(Math.Max(0, ClientSize.Width - (verticalScrollBar.Visible ? verticalScrollBar.Width : 0)) / _zoomScale);
         }
         else
         {
            horizontalScrollBar.Maximum = 0;
         }
         horizontalScrollBar.LargeChange = (int)Math.Max(horizontalScrollBar.Maximum / 10, 1);
         horizontalScrollBar.SmallChange = (int)Math.Max(horizontalScrollBar.Maximum / 20, 1);

         if (verticalScrollBar.Visible)
         {  // If the offset does not make the Maximum less than zero, set its value.            
            verticalScrollBar.Maximum =
               Image.Size.Height -
               (int)(Math.Max(0, ClientSize.Height - (horizontalScrollBar.Visible ? horizontalScrollBar.Height : 0)) / _zoomScale);
         }
         else
         {
            verticalScrollBar.Maximum = 0;
         }
         verticalScrollBar.LargeChange = (int)Math.Max(verticalScrollBar.Maximum / 10, 1);
         verticalScrollBar.SmallChange = (int)Math.Max(verticalScrollBar.Maximum / 20, 1);
      }
      #endregion

      /// <summary>
      /// Get the horizontal scroll bar from this control
      /// </summary>
      [Browsable(false)]
      public HScrollBar HorizontalScrollBar
      {
         get
         {
            return horizontalScrollBar;
         }
      }

      /// <summary>
      /// Get the vertical scroll bar of this control
      /// </summary>
      [Browsable(false)]
      public VScrollBar VerticalScrollBar
      {
         get
         {
            return verticalScrollBar;
         }
      }

      /// <summary>
      /// Used for tracking the mouse position on the image
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void OnMouseMove(object sender, MouseEventArgs e)
      {
         if (_mouseDownButton == MouseButtons.Middle && (horizontalScrollBar.Visible || verticalScrollBar.Visible))
         {
            int horizontalShift = (int)((e.X - _mouseDownPosition.X) / _zoomScale);
            int verticalShift = (int)((e.Y - _mouseDownPosition.Y) / _zoomScale);

            if (horizontalShift == 0 && verticalShift == 0) return;

            //if (horizontalScrollBar.Visible)
            horizontalScrollBar.Value =
                  Math.Max(Math.Min(horizontalScrollBar.Value - horizontalShift, horizontalScrollBar.Maximum), horizontalScrollBar.Minimum);
            //if (verticalScrollBar.Visible)
            verticalScrollBar.Value =
                  Math.Max(Math.Min(verticalScrollBar.Value - verticalShift, verticalScrollBar.Maximum), verticalScrollBar.Minimum);

            if (horizontalShift != 0) _mouseDownPosition.X = e.Location.X;
            if (verticalShift != 0) _mouseDownPosition.Y = e.Location.Y;

            Invalidate();
         }
         else if (_mouseDownButton == MouseButtons.Left)
         {
            //reverse the previous highlighted rectangle, if there is any
            ReverseRectangle();

            Rectangle rect = GetSelectedRectangle(e.X, e.Y);
            rect.Location = PointToScreen(rect.Location);
            ControlPaint.DrawReversibleFrame(
               rect,
               Color.White,
               FrameStyle.Dashed);
            _bufferPoint = e.Location;
         }
      }

      private void ReverseRectangle()
      {
         if (!_bufferPoint.IsEmpty)
         {
            Rectangle rect = GetSelectedRectangle(_bufferPoint.X, _bufferPoint.Y);
            rect.Location = PointToScreen(rect.Location);
            ControlPaint.DrawReversibleFrame(
               rect,
               Color.White,
               FrameStyle.Dashed);
            _bufferPoint = Point.Empty;
         }
      }

      private Size GetViewSize()
      {
         return new Size(
            ClientSize.Width - (verticalScrollBar.Visible ? verticalScrollBar.Width : 0) ,
            ClientSize.Height - (horizontalScrollBar.Visible ? horizontalScrollBar.Height : 0));
      }

      private Rectangle GetSelectedRectangle(int x, int y)
      {
         int top = Math.Min(y, _mouseDownPosition.Y);
         int bottom = Math.Max(y, _mouseDownPosition.Y);
         int left = Math.Min(x, _mouseDownPosition.X);
         int right = Math.Max(x, _mouseDownPosition.X);
         Size viewSize = GetViewSize();

         Rectangle rect = new Rectangle(left, top, right - left, bottom - top);
         rect.Intersect(new Rectangle(Point.Empty, viewSize));

         if ((double)rect.Width / rect.Height > (double)viewSize.Width / viewSize.Height)
         {
            rect.Width = (int)((double)viewSize.Width / viewSize.Height * rect.Height);
         }
         else if ((double)rect.Width / rect.Height < (double)viewSize.Width / viewSize.Height)
         {
            rect.Height = (int)((double)rect.Width / viewSize.Width * viewSize.Height);
         }

         if (rect.Y != _mouseDownPosition.Y)
         {
            rect.Y =  _mouseDownPosition.Y - rect.Height;
         }

         if (rect.X != _mouseDownPosition.X)
         {
            rect.X = _mouseDownPosition.X - rect.Width;
         }

         return rect;
      }

      /// <summary>
      /// Get or Set the zoom scale
      /// </summary>
      [Browsable(false)]
      public double ZoomScale
      {
         get
         {
            return _zoomScale;
         }
      }

      /// <summary>
      /// Set the new zoom scale for the displayed image
      /// </summary>
      /// <param name="zoomScale">The new zoom scale</param>
      /// <param name="fixPoint">The point to be fixed, in display coordinate</param>
      public void SetZoomScale(double zoomScale, Point fixPoint)
      {
         if (
            Image != null &&
            _zoomScale != zoomScale //the scale has been changed
            && //and, the scale is not too small
            !(zoomScale < _zoomScale &&
               (Image.Size.Width * zoomScale < (2.0 + verticalScrollBar.Width)
               || Image.Size.Height * zoomScale < (2.0 + horizontalScrollBar.Height)))
            && //and, the scale is not too big
            !(zoomScale > _zoomScale &&
               (GetViewSize().Width < zoomScale * 2
               || GetViewSize().Height < zoomScale * 2)))
         {
            //constrain the coordinate to be within valide range
            fixPoint.X = Math.Min(fixPoint.X, (int)(Image.Size.Width * _zoomScale));
            fixPoint.Y = Math.Min(fixPoint.Y, (int)(Image.Size.Height * _zoomScale));

            int shiftX = (int)(fixPoint.X * (zoomScale - _zoomScale) / zoomScale / _zoomScale);
            int shiftY = (int)(fixPoint.Y * (zoomScale - _zoomScale) / zoomScale / _zoomScale);

            _zoomScale = zoomScale;

            int h = (int)(horizontalScrollBar.Value + shiftX);
            int v = (int)(verticalScrollBar.Value + shiftY);
            SetScrollBarVisibilityAndMaxMin();
            horizontalScrollBar.Value = Math.Min(Math.Max(horizontalScrollBar.Minimum, h), horizontalScrollBar.Maximum); ;
            verticalScrollBar.Value = Math.Min(Math.Max(verticalScrollBar.Minimum, v), verticalScrollBar.Maximum);

            Invalidate();

            if (OnZoomScaleChange != null)
               OnZoomScaleChange(this, new EventArgs());

         }
      }

      private void InitializeComponent()
      {
         this.horizontalScrollBar = new System.Windows.Forms.HScrollBar();
         this.verticalScrollBar = new System.Windows.Forms.VScrollBar();
         ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
         this.SuspendLayout();
         // 
         // horizontalScrollBar
         // 
         this.horizontalScrollBar.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.horizontalScrollBar.Location = new System.Drawing.Point(0, 0);
         this.horizontalScrollBar.Name = "horizontalScrollBar";
         this.horizontalScrollBar.Size = new System.Drawing.Size(80, 17);
         this.horizontalScrollBar.TabIndex = 2;
         this.horizontalScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.OnScroll);
         this.Controls.Add(horizontalScrollBar);
         // 
         // verticalScrollBar
         // 
         this.verticalScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
         this.verticalScrollBar.Location = new System.Drawing.Point(0, 0);
         this.verticalScrollBar.Name = "verticalScrollBar";
         this.verticalScrollBar.Size = new System.Drawing.Size(17, 80);
         this.verticalScrollBar.TabIndex = 1;
         this.verticalScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.OnScroll);
         this.Controls.Add(verticalScrollBar);
         // 
         // PanAndZoomPictureBox
         // 

         ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
         this.ResumeLayout(false);

      }
   }
}
