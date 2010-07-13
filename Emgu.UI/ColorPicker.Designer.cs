namespace Emgu.UI
{
   partial class ColorPicker
   {
      /// <summary> 
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary> 
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.label1 = new System.Windows.Forms.Label();
         this.colorPickerButton1 = new Emgu.UI.ColorPickerButton();
         this.SuspendLayout();
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(3, 5);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(66, 13);
         this.label1.TabIndex = 1;
         this.label1.Text = "Pick a color:";
         // 
         // colorPickerButton1
         // 
         this.colorPickerButton1.Location = new System.Drawing.Point(79, 0);
         this.colorPickerButton1.Name = "colorPickerButton1";
         this.colorPickerButton1.Size = new System.Drawing.Size(28, 23);
         this.colorPickerButton1.TabIndex = 0;
         this.colorPickerButton1.UseVisualStyleBackColor = true;
         // 
         // ColorPicker
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.label1);
         this.Controls.Add(this.colorPickerButton1);
         this.Name = "ColorPicker";
         this.Size = new System.Drawing.Size(110, 25);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private ColorPickerButton colorPickerButton1;
      private System.Windows.Forms.Label label1;
   }
}
