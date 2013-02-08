namespace Emgu.UI
{
    /// <summary>
    /// A Combo Box Selector 
    /// </summary>
    partial class ComboBoxSelector
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
           this.panel1 = new System.Windows.Forms.Panel();
           this.SelectedIndexLabel = new System.Windows.Forms.Label();
           this.ItemSizeLabel = new System.Windows.Forms.Label();
           this.label1 = new System.Windows.Forms.Label();
           this.panel2 = new System.Windows.Forms.Panel();
           this.comboBox1 = new System.Windows.Forms.ComboBox();
           this.NextButton = new System.Windows.Forms.Button();
           this.PreviousButton = new System.Windows.Forms.Button();
           this.panel1.SuspendLayout();
           this.panel2.SuspendLayout();
           this.SuspendLayout();
           // 
           // panel1
           // 
           this.panel1.Controls.Add(this.SelectedIndexLabel);
           this.panel1.Controls.Add(this.ItemSizeLabel);
           this.panel1.Controls.Add(this.label1);
           this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
           this.panel1.Location = new System.Drawing.Point(268, 0);
           this.panel1.Name = "panel1";
           this.panel1.Size = new System.Drawing.Size(56, 22);
           this.panel1.TabIndex = 4;
           // 
           // SelectedIndexLabel
           // 
           this.SelectedIndexLabel.AutoSize = true;
           this.SelectedIndexLabel.Location = new System.Drawing.Point(3, 5);
           this.SelectedIndexLabel.Name = "SelectedIndexLabel";
           this.SelectedIndexLabel.Size = new System.Drawing.Size(13, 13);
           this.SelectedIndexLabel.TabIndex = 2;
           this.SelectedIndexLabel.Text = "0";
           // 
           // ItemSizeLabel
           // 
           this.ItemSizeLabel.AutoSize = true;
           this.ItemSizeLabel.Location = new System.Drawing.Point(35, 5);
           this.ItemSizeLabel.Name = "ItemSizeLabel";
           this.ItemSizeLabel.Size = new System.Drawing.Size(13, 13);
           this.ItemSizeLabel.TabIndex = 1;
           this.ItemSizeLabel.Text = "0";
           // 
           // label1
           // 
           this.label1.AutoSize = true;
           this.label1.Location = new System.Drawing.Point(18, 5);
           this.label1.Name = "label1";
           this.label1.Size = new System.Drawing.Size(16, 13);
           this.label1.TabIndex = 0;
           this.label1.Text = "of";
           // 
           // panel2
           // 
           this.panel2.Controls.Add(this.comboBox1);
           this.panel2.Controls.Add(this.NextButton);
           this.panel2.Controls.Add(this.PreviousButton);
           this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
           this.panel2.Location = new System.Drawing.Point(0, 0);
           this.panel2.Name = "panel2";
           this.panel2.Size = new System.Drawing.Size(268, 22);
           this.panel2.TabIndex = 6;
           // 
           // comboBox1
           // 
           this.comboBox1.Dock = System.Windows.Forms.DockStyle.Fill;
           this.comboBox1.FormattingEnabled = true;
           this.comboBox1.Location = new System.Drawing.Point(19, 0);
           this.comboBox1.Name = "comboBox1";
           this.comboBox1.Size = new System.Drawing.Size(232, 21);
           this.comboBox1.TabIndex = 7;
           // 
           // NextButton
           // 
           this.NextButton.Dock = System.Windows.Forms.DockStyle.Right;
           this.NextButton.Enabled = false;
           this.NextButton.Location = new System.Drawing.Point(251, 0);
           this.NextButton.Name = "NextButton";
           this.NextButton.Size = new System.Drawing.Size(17, 22);
           this.NextButton.TabIndex = 6;
           this.NextButton.Text = ">";
           this.NextButton.UseVisualStyleBackColor = true;
           this.NextButton.Click += new System.EventHandler(this.NextButton_Click_1);
           // 
           // PreviousButton
           // 
           this.PreviousButton.Dock = System.Windows.Forms.DockStyle.Left;
           this.PreviousButton.Enabled = false;
           this.PreviousButton.Location = new System.Drawing.Point(0, 0);
           this.PreviousButton.Name = "PreviousButton";
           this.PreviousButton.Size = new System.Drawing.Size(19, 22);
           this.PreviousButton.TabIndex = 1;
           this.PreviousButton.Text = "<";
           this.PreviousButton.UseVisualStyleBackColor = true;
           this.PreviousButton.Click += new System.EventHandler(this.PreviousButton_Click_1);
           // 
           // ComboBoxSelector
           // 
           this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
           this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
           this.Controls.Add(this.panel2);
           this.Controls.Add(this.panel1);
           this.Name = "ComboBoxSelector";
           this.Size = new System.Drawing.Size(324, 22);
           this.panel1.ResumeLayout(false);
           this.panel1.PerformLayout();
           this.panel2.ResumeLayout(false);
           this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button PreviousButton;
        private System.Windows.Forms.Label ItemSizeLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button NextButton;
        private System.Windows.Forms.Label SelectedIndexLabel;

    }
}
