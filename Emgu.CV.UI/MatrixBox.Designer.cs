namespace Emgu.CV.UI
{
   partial class MatrixBox
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
         this.dataGridView1 = new System.Windows.Forms.DataGridView();
         this.errorMsg = new System.Windows.Forms.Label();
         this.channelLabel = new System.Windows.Forms.Label();
         this.channelSelectComboBox = new System.Windows.Forms.ComboBox();
         this.panel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
         this.SuspendLayout();
         // 
         // panel1
         // 
         this.panel1.Controls.Add(this.channelSelectComboBox);
         this.panel1.Controls.Add(this.channelLabel);
         this.panel1.Controls.Add(this.errorMsg);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panel1.Location = new System.Drawing.Point(0, 0);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(383, 56);
         this.panel1.TabIndex = 0;
         // 
         // dataGridView1
         // 
         this.dataGridView1.AllowUserToAddRows = false;
         this.dataGridView1.AllowUserToDeleteRows = false;
         this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
         this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.dataGridView1.Location = new System.Drawing.Point(0, 56);
         this.dataGridView1.Name = "dataGridView1";
         this.dataGridView1.RowHeadersWidth = 80;
         this.dataGridView1.Size = new System.Drawing.Size(383, 190);
         this.dataGridView1.TabIndex = 1;
         // 
         // errorMsg
         // 
         this.errorMsg.AutoSize = true;
         this.errorMsg.Location = new System.Drawing.Point(13, 37);
         this.errorMsg.Name = "errorMsg";
         this.errorMsg.Size = new System.Drawing.Size(75, 13);
         this.errorMsg.TabIndex = 0;
         this.errorMsg.Text = "Error Message";
         // 
         // channelLabel
         // 
         this.channelLabel.AutoSize = true;
         this.channelLabel.Location = new System.Drawing.Point(13, 14);
         this.channelLabel.Name = "channelLabel";
         this.channelLabel.Size = new System.Drawing.Size(46, 13);
         this.channelLabel.TabIndex = 1;
         this.channelLabel.Text = "Channel";
         // 
         // channelSelectComboBox
         // 
         this.channelSelectComboBox.FormattingEnabled = true;
         this.channelSelectComboBox.Location = new System.Drawing.Point(68, 11);
         this.channelSelectComboBox.Name = "channelSelectComboBox";
         this.channelSelectComboBox.Size = new System.Drawing.Size(121, 21);
         this.channelSelectComboBox.TabIndex = 2;
         // 
         // MatrixBox
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.dataGridView1);
         this.Controls.Add(this.panel1);
         this.Name = "MatrixBox";
         this.Size = new System.Drawing.Size(383, 246);
         this.panel1.ResumeLayout(false);
         this.panel1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Panel panel1;
      private System.Windows.Forms.DataGridView dataGridView1;
      private System.Windows.Forms.Label errorMsg;
      private System.Windows.Forms.Label channelLabel;
      private System.Windows.Forms.ComboBox channelSelectComboBox;
   }
}
