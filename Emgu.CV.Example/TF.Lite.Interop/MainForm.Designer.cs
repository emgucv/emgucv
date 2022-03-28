
namespace CVInterop.Lite.Net
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.fileNameTextBox = new System.Windows.Forms.TextBox();
            this.openFileButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cameraButton = new System.Windows.Forms.Button();
            this.messageLabel = new System.Windows.Forms.Label();
            this.imageBox = new Emgu.CV.UI.ImageBox();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(193, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Recognized Image File:";
            // 
            // fileNameTextBox
            // 
            this.fileNameTextBox.Location = new System.Drawing.Point(228, 13);
            this.fileNameTextBox.Name = "fileNameTextBox";
            this.fileNameTextBox.Size = new System.Drawing.Size(718, 31);
            this.fileNameTextBox.TabIndex = 1;
            // 
            // openFileButton
            // 
            this.openFileButton.Location = new System.Drawing.Point(973, 13);
            this.openFileButton.Name = "openFileButton";
            this.openFileButton.Size = new System.Drawing.Size(81, 34);
            this.openFileButton.TabIndex = 2;
            this.openFileButton.Text = "...";
            this.openFileButton.UseVisualStyleBackColor = true;
            this.openFileButton.Click += new System.EventHandler(this.openFileButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(212, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "Recognize camera image:";
            // 
            // cameraButton
            // 
            this.cameraButton.Location = new System.Drawing.Point(243, 74);
            this.cameraButton.Name = "cameraButton";
            this.cameraButton.Size = new System.Drawing.Size(230, 34);
            this.cameraButton.TabIndex = 4;
            this.cameraButton.Text = "Start Camera";
            this.cameraButton.UseVisualStyleBackColor = true;
            this.cameraButton.Click += new System.EventHandler(this.cameraButton_Click);
            // 
            // messageLabel
            // 
            this.messageLabel.AutoSize = true;
            this.messageLabel.Location = new System.Drawing.Point(13, 136);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(123, 25);
            this.messageLabel.TabIndex = 5;
            this.messageLabel.Text = "messageLabel";
            // 
            // imageBox
            // 
            this.imageBox.Location = new System.Drawing.Point(13, 178);
            this.imageBox.Name = "imageBox";
            this.imageBox.Size = new System.Drawing.Size(1084, 703);
            this.imageBox.TabIndex = 2;
            this.imageBox.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1109, 893);
            this.Controls.Add(this.imageBox);
            this.Controls.Add(this.messageLabel);
            this.Controls.Add(this.cameraButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.openFileButton);
            this.Controls.Add(this.fileNameTextBox);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "Emgu CV Interop";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox fileNameTextBox;
        private System.Windows.Forms.Button openFileButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button cameraButton;
        private System.Windows.Forms.Label messageLabel;
        private Emgu.CV.UI.ImageBox imageBox;
    }
}

