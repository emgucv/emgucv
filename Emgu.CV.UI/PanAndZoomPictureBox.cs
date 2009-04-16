using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace Emgu.CV.UI
{
   /// <summary>
   /// A picture box with pan and zoom functionality
   /// </summary>
   public partial class PanAndZoomPictureBox : PictureBox
   {
      /// <summary>
      /// Create a picture box with pan and zoom functionality
      /// </summary>
      public PanAndZoomPictureBox()
         : base()
      {
         InitializeComponent();
         SetScrollBarValues();
         BorderStyle = BorderStyle.Fixed3D;
         SetStyle(
            ControlStyles.OptimizedDoubleBuffer,
            true);
         this.MouseWheel += OnMouseWheel;
      }

      /// <summary>
      /// The zoom scale of the image to be displayed
      /// </summary>
      private double _zoomScale = 1.0;

      /// <summary>
      /// The available zoom levels for the displayed image 
      /// </summary>
      public static double[] ZoomLevels = new double[] { 0.125, 0.25, 0.5, 1.0, 2.0, 4.0, 8.0 };

      private Point _mouseDownPosition;
      private MouseButtons _mouseDownButton;
      private Point _bufferPoint;
      private HScrollBar horizontalScrollBar;
      private VScrollBar verticalScrollBar;

      private static readonly Cursor _defaultCursor = Cursors.Cross;

      private void OnMouseDown(object sender, MouseEventArgs e)
      {
         _mouseDownPosition = e.Location;
         _mouseDownButton = e.Button;

         _bufferPoint = Point.Empty;
         if (e.Button == MouseButtons.Middle)
            this.Cursor = Cursors.Hand;
      }

      private void OnMouseUp(object sender, MouseEventArgs e)
      {
         _mouseDownButton = MouseButtons.None;
         this.Cursor = _defaultCursor;

         if (e.Button == MouseButtons.Left)
         {
            ReverseRectangle();

            Rectangle imageRegion = new Rectangle(Point.Empty, ClientSize);
            if (imageRegion.Contains(_mouseDownPosition))
            {
               Rectangle selectedRectangle = GetSelectedRectangle(e.X, e.Y);

               if ( (selectedRectangle.Width / _zoomScale) > 2 && (selectedRectangle.Height / _zoomScale) > 2)
               {
                  horizontalScrollBar.Value = Math.Min(horizontalScrollBar.Maximum, horizontalScrollBar.Value +(int)(selectedRectangle.Location.X / _zoomScale));
                  verticalScrollBar.Value = Math.Min(verticalScrollBar.Maximum, verticalScrollBar.Value +(int)(selectedRectangle.Location.Y / _zoomScale));

                  _zoomScale = _zoomScale * ClientSize.Width / selectedRectangle.Width;

                  Invalidate();
               }
            }
         }
      }

      private void OnMouseEnter(object sender, EventArgs e)
      {  //set this as the active control 
         Control parent = this.Parent;
         while (!(parent is Form)) parent = parent.Parent;
         (parent as Form).ActiveControl = this;
      }

      private void OnResize(object sender, EventArgs e)
      {
         if (base.Image != null && ClientSize.Width > 0 && ClientSize.Height > 0)
         {
            RenderImage();
            SetScrollBarValues();
         }
      }

      private void OnMouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
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
      public event EventHandler ZoomScaleChange;

      #region Handling ScrollBars

      private void OnScroll(object sender, ScrollEventArgs e)
      {
         Invalidate();
      }

      /// <summary>
      /// Paint the image
      /// </summary>
      /// <param name="pe">The paint event</param>
      protected override void OnPaint(PaintEventArgs pe)
      {
         if (Image != null          //image is set
            &&          //either pan or zoom
            (_zoomScale != 1.0f || horizontalScrollBar.Visible || verticalScrollBar.Visible))
         {
            Matrix matrix = new Matrix((float)_zoomScale, 0, 0, (float)_zoomScale, 0, 0);
            matrix.Translate(
               horizontalScrollBar.Visible ? -horizontalScrollBar.Value : 0,
               verticalScrollBar.Visible ? -verticalScrollBar.Value : 0);
            pe.Graphics.Transform = matrix;
            pe.Graphics.InterpolationMode = InterpolationMode.High;
         }

         base.OnPaint(pe);
      }

      private void SetScrollBarValues()
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
               (int)(Math.Max(0, Size.Width - (verticalScrollBar.Visible ? verticalScrollBar.Width : 0)) / _zoomScale);
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
               (int)(Math.Max(0, Size.Height - (horizontalScrollBar.Visible ? horizontalScrollBar.Height : 0)) / _zoomScale);
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
         int offsetX = (int)(e.Location.X / _zoomScale);
         int offsetY = (int)(e.Location.Y / _zoomScale);

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

      private Rectangle GetSelectedRectangle(int x, int y)
      {
         int top = Math.Min(y, _mouseDownPosition.Y);
         int bottom = Math.Max(y, _mouseDownPosition.Y);
         int left = Math.Min(x, _mouseDownPosition.X);
         int right = Math.Max(x, _mouseDownPosition.X);

         Rectangle rect = new Rectangle(left, top, right - left, bottom - top);
         rect.Intersect(new Rectangle(Point.Empty, ClientSize));

         if ((double)rect.Width / rect.Height > (double)ClientSize.Width / ClientSize.Height)
         {
            rect.Width = (int)((double)ClientSize.Width / ClientSize.Height * rect.Height);
         }
         else if ((double)rect.Width / rect.Height < (double)ClientSize.Width / ClientSize.Height)
         {
            rect.Height = (int)((double)rect.Width / ClientSize.Width * ClientSize.Height);
         }
         return rect;
      }

      protected void RenderImage()
      {
         int width = (int)(Image.Size.Width * _zoomScale);
         int height = (int)(Image.Size.Height * _zoomScale);

         if (width <= Width && height <= Height)
         {  //no ROI is required           
            verticalScrollBar.Visible = false;
            horizontalScrollBar.Visible = false;
         }
         else
         {
            SetScrollBarValues();
         }
         Invalidate();
      }

      /// <summary>
      /// Get or Set the zoom scale
      /// </summary>
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
      /// <param name="fixPoint">The point to be fixed</param>
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
               (ClientSize.Width < zoomScale * 2
               || ClientSize.Height < zoomScale * 2)))
         {
            fixPoint.X = Math.Min(fixPoint.X, (int)(Image.Size.Width * _zoomScale));
            fixPoint.Y = Math.Min(fixPoint.Y, (int)(Image.Size.Height * _zoomScale));

            int shiftX = (int)(fixPoint.X * (zoomScale - _zoomScale) / zoomScale / _zoomScale);
            int shiftY = (int)(fixPoint.Y * (zoomScale - _zoomScale) / zoomScale / _zoomScale);

            _zoomScale = zoomScale;

            horizontalScrollBar.Maximum = Int32.MaxValue;
            verticalScrollBar.Maximum = Int32.MaxValue;
            horizontalScrollBar.Value = Math.Min(Math.Max(horizontalScrollBar.Minimum, (int)(horizontalScrollBar.Value + shiftX)), horizontalScrollBar.Maximum);
            verticalScrollBar.Value = Math.Min(Math.Max(verticalScrollBar.Minimum, (int)(verticalScrollBar.Value + shiftY)), verticalScrollBar.Maximum);

            RenderImage();

            if (ZoomScaleChange != null)
               ZoomScaleChange(this, new EventArgs());

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
         this.Dock = System.Windows.Forms.DockStyle.Right;
         this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseMove);
         this.Resize += new System.EventHandler(this.OnResize);
         this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
         this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
         this.MouseEnter += new System.EventHandler(this.OnMouseEnter);
         ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
         this.ResumeLayout(false);

      }
   }
}
