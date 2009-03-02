namespace Emgu.CV.UI
{
    /// <summary>
    /// The control to display image properties
    /// </summary>
    partial class ImageProperty
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
           this.widthTextbox = new System.Windows.Forms.TextBox();
           this.heightTextBox = new System.Windows.Forms.TextBox();
           this.label2 = new System.Windows.Forms.Label();
           this.label3 = new System.Windows.Forms.Label();
           this.mousePositionTextbox = new System.Windows.Forms.TextBox();
           this.label4 = new System.Windows.Forms.Label();
           this.colorIntensityTextbox = new System.Windows.Forms.TextBox();
           this.label5 = new System.Windows.Forms.Label();
           this.typeOfColorTexbox = new System.Windows.Forms.TextBox();
           this.label6 = new System.Windows.Forms.Label();
           this.typeOfDepthTextBox = new System.Windows.Forms.TextBox();
           this.label7 = new System.Windows.Forms.Label();
           this.label8 = new System.Windows.Forms.Label();
           this.fpsTextBox = new System.Windows.Forms.TextBox();
           this.tabControl1 = new System.Windows.Forms.TabControl();
           this.tabPage1 = new System.Windows.Forms.TabPage();
           this.cSharpOperationView = new Emgu.CV.UI.OperationsView();
           this.tabPage2 = new System.Windows.Forms.TabPage();
           this.cPlusPlusOperationView = new Emgu.CV.UI.OperationsView();
           this.clearOperationsBtn = new System.Windows.Forms.Button();
           this.popOperationButton = new System.Windows.Forms.Button();
           this.showHistogramButton = new System.Windows.Forms.Button();
           this.tabControl1.SuspendLayout();
           this.tabPage1.SuspendLayout();
           this.tabPage2.SuspendLayout();
           this.SuspendLayout();
           // 
           // label1
           // 
           this.label1.AutoSize = true;
           this.label1.Location = new System.Drawing.Point(3, 7);
           this.label1.Name = "label1";
           this.label1.Size = new System.Drawing.Size(30, 13);
           this.label1.TabIndex = 0;
           this.label1.Text = "Size:";
           // 
           // widthTextbox
           // 
           this.widthTextbox.Location = new System.Drawing.Point(40, 3);
           this.widthTextbox.Name = "widthTextbox";
           this.widthTextbox.ReadOnly = true;
           this.widthTextbox.Size = new System.Drawing.Size(38, 20);
           this.widthTextbox.TabIndex = 1;
           // 
           // heightTextBox
           // 
           this.heightTextBox.Location = new System.Drawing.Point(104, 4);
           this.heightTextBox.Name = "heightTextBox";
           this.heightTextBox.ReadOnly = true;
           this.heightTextBox.Size = new System.Drawing.Size(38, 20);
           this.heightTextBox.TabIndex = 2;
           // 
           // label2
           // 
           this.label2.AutoSize = true;
           this.label2.Location = new System.Drawing.Point(84, 6);
           this.label2.Name = "label2";
           this.label2.Size = new System.Drawing.Size(14, 13);
           this.label2.TabIndex = 3;
           this.label2.Text = "X";
           // 
           // label3
           // 
           this.label3.AutoSize = true;
           this.label3.Location = new System.Drawing.Point(3, 45);
           this.label3.Name = "label3";
           this.label3.Size = new System.Drawing.Size(82, 13);
           this.label3.TabIndex = 4;
           this.label3.Text = "Mouse Position:";
           // 
           // mousePositionTextbox
           // 
           this.mousePositionTextbox.Location = new System.Drawing.Point(91, 42);
           this.mousePositionTextbox.Name = "mousePositionTextbox";
           this.mousePositionTextbox.ReadOnly = true;
           this.mousePositionTextbox.Size = new System.Drawing.Size(82, 20);
           this.mousePositionTextbox.TabIndex = 5;
           // 
           // label4
           // 
           this.label4.AutoSize = true;
           this.label4.Location = new System.Drawing.Point(36, 69);
           this.label4.Name = "label4";
           this.label4.Size = new System.Drawing.Size(49, 13);
           this.label4.TabIndex = 6;
           this.label4.Text = "Intensity:";
           // 
           // colorIntensityTextbox
           // 
           this.colorIntensityTextbox.Location = new System.Drawing.Point(91, 66);
           this.colorIntensityTextbox.Name = "colorIntensityTextbox";
           this.colorIntensityTextbox.ReadOnly = true;
           this.colorIntensityTextbox.Size = new System.Drawing.Size(82, 20);
           this.colorIntensityTextbox.TabIndex = 7;
           // 
           // label5
           // 
           this.label5.AutoSize = true;
           this.label5.Location = new System.Drawing.Point(164, 7);
           this.label5.Name = "label5";
           this.label5.Size = new System.Drawing.Size(34, 13);
           this.label5.TabIndex = 8;
           this.label5.Text = "Color:";
           // 
           // typeOfColorTexbox
           // 
           this.typeOfColorTexbox.Location = new System.Drawing.Point(204, 4);
           this.typeOfColorTexbox.Name = "typeOfColorTexbox";
           this.typeOfColorTexbox.ReadOnly = true;
           this.typeOfColorTexbox.Size = new System.Drawing.Size(38, 20);
           this.typeOfColorTexbox.TabIndex = 9;
           // 
           // label6
           // 
           this.label6.AutoSize = true;
           this.label6.Location = new System.Drawing.Point(249, 7);
           this.label6.Name = "label6";
           this.label6.Size = new System.Drawing.Size(39, 13);
           this.label6.TabIndex = 10;
           this.label6.Text = "Depth:";
           // 
           // typeOfDepthTextBox
           // 
           this.typeOfDepthTextBox.Location = new System.Drawing.Point(294, 4);
           this.typeOfDepthTextBox.Name = "typeOfDepthTextBox";
           this.typeOfDepthTextBox.ReadOnly = true;
           this.typeOfDepthTextBox.Size = new System.Drawing.Size(40, 20);
           this.typeOfDepthTextBox.TabIndex = 11;
           // 
           // label7
           // 
           this.label7.AutoSize = true;
           this.label7.Location = new System.Drawing.Point(5, 123);
           this.label7.Name = "label7";
           this.label7.Size = new System.Drawing.Size(61, 13);
           this.label7.TabIndex = 12;
           this.label7.Text = "Operations:";
           // 
           // label8
           // 
           this.label8.AutoSize = true;
           this.label8.Location = new System.Drawing.Point(213, 45);
           this.label8.Name = "label8";
           this.label8.Size = new System.Drawing.Size(30, 13);
           this.label8.TabIndex = 14;
           this.label8.Text = "FPS:";
           // 
           // fpsTextBox
           // 
           this.fpsTextBox.Location = new System.Drawing.Point(249, 42);
           this.fpsTextBox.Name = "fpsTextBox";
           this.fpsTextBox.ReadOnly = true;
           this.fpsTextBox.Size = new System.Drawing.Size(82, 20);
           this.fpsTextBox.TabIndex = 15;
           // 
           // tabControl1
           // 
           this.tabControl1.Controls.Add(this.tabPage1);
           this.tabControl1.Controls.Add(this.tabPage2);
           this.tabControl1.Location = new System.Drawing.Point(8, 149);
           this.tabControl1.Name = "tabControl1";
           this.tabControl1.SelectedIndex = 0;
           this.tabControl1.Size = new System.Drawing.Size(327, 242);
           this.tabControl1.TabIndex = 16;
           // 
           // tabPage1
           // 
           this.tabPage1.Controls.Add(this.cSharpOperationView);
           this.tabPage1.Location = new System.Drawing.Point(4, 22);
           this.tabPage1.Name = "tabPage1";
           this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
           this.tabPage1.Size = new System.Drawing.Size(319, 216);
           this.tabPage1.TabIndex = 0;
           this.tabPage1.Text = "C#";
           this.tabPage1.UseVisualStyleBackColor = true;
           // 
           // cSharpOperationView
           // 
           this.cSharpOperationView.Dock = System.Windows.Forms.DockStyle.Fill;
           this.cSharpOperationView.Location = new System.Drawing.Point(3, 3);
           this.cSharpOperationView.Name = "cSharpOperationView";
           this.cSharpOperationView.Size = new System.Drawing.Size(313, 210);
           this.cSharpOperationView.TabIndex = 0;
           // 
           // tabPage2
           // 
           this.tabPage2.Controls.Add(this.cPlusPlusOperationView);
           this.tabPage2.Location = new System.Drawing.Point(4, 22);
           this.tabPage2.Name = "tabPage2";
           this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
           this.tabPage2.Size = new System.Drawing.Size(319, 216);
           this.tabPage2.TabIndex = 1;
           this.tabPage2.Text = "C++";
           this.tabPage2.UseVisualStyleBackColor = true;
           // 
           // cPlusPlusOperationView
           // 
           this.cPlusPlusOperationView.Dock = System.Windows.Forms.DockStyle.Fill;
           this.cPlusPlusOperationView.Location = new System.Drawing.Point(3, 3);
           this.cPlusPlusOperationView.Name = "cPlusPlusOperationView";
           this.cPlusPlusOperationView.Size = new System.Drawing.Size(313, 210);
           this.cPlusPlusOperationView.TabIndex = 0;
           // 
           // clearOperationsBtn
           // 
           this.clearOperationsBtn.Location = new System.Drawing.Point(249, 118);
           this.clearOperationsBtn.Name = "clearOperationsBtn";
           this.clearOperationsBtn.Size = new System.Drawing.Size(75, 23);
           this.clearOperationsBtn.TabIndex = 17;
           this.clearOperationsBtn.Text = "Remove All";
           this.clearOperationsBtn.UseVisualStyleBackColor = true;
           this.clearOperationsBtn.Click += new System.EventHandler(this.clearOperationBtn_Click);
           // 
           // popOperationButton
           // 
           this.popOperationButton.Location = new System.Drawing.Point(124, 118);
           this.popOperationButton.Name = "popOperationButton";
           this.popOperationButton.Size = new System.Drawing.Size(100, 23);
           this.popOperationButton.TabIndex = 18;
           this.popOperationButton.Text = "Pop Operation";
           this.popOperationButton.UseVisualStyleBackColor = true;
           this.popOperationButton.Click += new System.EventHandler(this.popOperationButton_Click);
           // 
           // showHistogramButton
           // 
           this.showHistogramButton.Location = new System.Drawing.Point(8, 92);
           this.showHistogramButton.Name = "showHistogramButton";
           this.showHistogramButton.Size = new System.Drawing.Size(149, 23);
           this.showHistogramButton.TabIndex = 19;
           this.showHistogramButton.Text = "Color Histogram";
           this.showHistogramButton.UseVisualStyleBackColor = true;
           this.showHistogramButton.Click += new System.EventHandler(this.showHistogramButton_Click);
           // 
           // ImageProperty
           // 
           this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
           this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
           this.Controls.Add(this.showHistogramButton);
           this.Controls.Add(this.popOperationButton);
           this.Controls.Add(this.clearOperationsBtn);
           this.Controls.Add(this.tabControl1);
           this.Controls.Add(this.fpsTextBox);
           this.Controls.Add(this.label8);
           this.Controls.Add(this.label7);
           this.Controls.Add(this.typeOfDepthTextBox);
           this.Controls.Add(this.label6);
           this.Controls.Add(this.typeOfColorTexbox);
           this.Controls.Add(this.label5);
           this.Controls.Add(this.colorIntensityTextbox);
           this.Controls.Add(this.label4);
           this.Controls.Add(this.mousePositionTextbox);
           this.Controls.Add(this.label3);
           this.Controls.Add(this.label2);
           this.Controls.Add(this.heightTextBox);
           this.Controls.Add(this.widthTextbox);
           this.Controls.Add(this.label1);
           this.Name = "ImageProperty";
           this.Size = new System.Drawing.Size(349, 394);
           this.tabControl1.ResumeLayout(false);
           this.tabPage1.ResumeLayout(false);
           this.tabPage2.ResumeLayout(false);
           this.ResumeLayout(false);
           this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox widthTextbox;
        private System.Windows.Forms.TextBox heightTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox mousePositionTextbox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox colorIntensityTextbox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox typeOfColorTexbox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox typeOfDepthTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox fpsTextBox;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private OperationsView cSharpOperationView;
        private OperationsView cPlusPlusOperationView;
        private System.Windows.Forms.Button clearOperationsBtn;
        private System.Windows.Forms.Button popOperationButton;
        private System.Windows.Forms.Button showHistogramButton;
    }
}
