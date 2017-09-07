namespace Aruco
{
   partial class MainForm
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

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
            this.components = new System.ComponentModel.Container();
            this.printArucoBoardButton = new System.Windows.Forms.Button();
            this.cameraButton = new System.Windows.Forms.Button();
            this.useThisFrameButton = new System.Windows.Forms.Button();
            this.cameraImageBox = new Emgu.CV.UI.ImageBox();
            this.messageLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.cameraImageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // printArucoBoardButton
            // 
            this.printArucoBoardButton.Location = new System.Drawing.Point(13, 13);
            this.printArucoBoardButton.Name = "printArucoBoardButton";
            this.printArucoBoardButton.Size = new System.Drawing.Size(165, 42);
            this.printArucoBoardButton.TabIndex = 0;
            this.printArucoBoardButton.Text = "Print Aruco Board";
            this.printArucoBoardButton.UseVisualStyleBackColor = true;
            this.printArucoBoardButton.Click += new System.EventHandler(this.printArucoBoardButton_Click);
            // 
            // cameraButton
            // 
            this.cameraButton.Location = new System.Drawing.Point(232, 13);
            this.cameraButton.Name = "cameraButton";
            this.cameraButton.Size = new System.Drawing.Size(306, 42);
            this.cameraButton.TabIndex = 1;
            this.cameraButton.Text = "Open Camera";
            this.cameraButton.UseVisualStyleBackColor = true;
            this.cameraButton.Click += new System.EventHandler(this.cameraButton_Click);
            // 
            // useThisFrameButton
            // 
            this.useThisFrameButton.Enabled = false;
            this.useThisFrameButton.Location = new System.Drawing.Point(571, 13);
            this.useThisFrameButton.Name = "useThisFrameButton";
            this.useThisFrameButton.Size = new System.Drawing.Size(264, 42);
            this.useThisFrameButton.TabIndex = 3;
            this.useThisFrameButton.Text = "Use this frame";
            this.useThisFrameButton.UseVisualStyleBackColor = true;
            this.useThisFrameButton.Click += new System.EventHandler(this.useThisFrameButton_Click);
            // 
            // cameraImageBox
            // 
            this.cameraImageBox.Location = new System.Drawing.Point(13, 113);
            this.cameraImageBox.Name = "cameraImageBox";
            this.cameraImageBox.Size = new System.Drawing.Size(1302, 770);
            this.cameraImageBox.TabIndex = 2;
            this.cameraImageBox.TabStop = false;
            // 
            // messageLabel
            // 
            this.messageLabel.AutoSize = true;
            this.messageLabel.Location = new System.Drawing.Point(13, 71);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(51, 20);
            this.messageLabel.TabIndex = 4;
            this.messageLabel.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1327, 895);
            this.Controls.Add(this.messageLabel);
            this.Controls.Add(this.useThisFrameButton);
            this.Controls.Add(this.cameraImageBox);
            this.Controls.Add(this.cameraButton);
            this.Controls.Add(this.printArucoBoardButton);
            this.Name = "Form1";
            this.Text = "Aruco demo";
            ((System.ComponentModel.ISupportInitialize)(this.cameraImageBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Button printArucoBoardButton;
      private System.Windows.Forms.Button cameraButton;
      private Emgu.CV.UI.ImageBox cameraImageBox;
      private System.Windows.Forms.Button useThisFrameButton;
      private System.Windows.Forms.Label messageLabel;
   }
}

