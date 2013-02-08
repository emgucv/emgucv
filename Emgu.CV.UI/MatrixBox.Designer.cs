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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MatrixBox));
         this.panel1 = new System.Windows.Forms.Panel();
         this.channelSelectComboBox = new System.Windows.Forms.ComboBox();
         this.channelLabel = new System.Windows.Forms.Label();
         this.errorMsg = new System.Windows.Forms.Label();
         this.dataGridView1 = new System.Windows.Forms.DataGridView();
         this.panel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
         this.SuspendLayout();
         // 
         // panel1
         // 
         this.panel1.AccessibleDescription = null;
         this.panel1.AccessibleName = null;
         resources.ApplyResources(this.panel1, "panel1");
         this.panel1.BackgroundImage = null;
         this.panel1.Controls.Add(this.channelSelectComboBox);
         this.panel1.Controls.Add(this.channelLabel);
         this.panel1.Controls.Add(this.errorMsg);
         this.panel1.Font = null;
         this.panel1.Name = "panel1";
         // 
         // channelSelectComboBox
         // 
         this.channelSelectComboBox.AccessibleDescription = null;
         this.channelSelectComboBox.AccessibleName = null;
         resources.ApplyResources(this.channelSelectComboBox, "channelSelectComboBox");
         this.channelSelectComboBox.BackgroundImage = null;
         this.channelSelectComboBox.Font = null;
         this.channelSelectComboBox.FormattingEnabled = true;
         this.channelSelectComboBox.Name = "channelSelectComboBox";
         // 
         // channelLabel
         // 
         this.channelLabel.AccessibleDescription = null;
         this.channelLabel.AccessibleName = null;
         resources.ApplyResources(this.channelLabel, "channelLabel");
         this.channelLabel.Font = null;
         this.channelLabel.Name = "channelLabel";
         // 
         // errorMsg
         // 
         this.errorMsg.AccessibleDescription = null;
         this.errorMsg.AccessibleName = null;
         resources.ApplyResources(this.errorMsg, "errorMsg");
         this.errorMsg.Font = null;
         this.errorMsg.Name = "errorMsg";
         // 
         // dataGridView1
         // 
         this.dataGridView1.AccessibleDescription = null;
         this.dataGridView1.AccessibleName = null;
         this.dataGridView1.AllowUserToAddRows = false;
         this.dataGridView1.AllowUserToDeleteRows = false;
         resources.ApplyResources(this.dataGridView1, "dataGridView1");
         this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
         this.dataGridView1.BackgroundImage = null;
         this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         this.dataGridView1.Font = null;
         this.dataGridView1.Name = "dataGridView1";
         // 
         // MatrixBox
         // 
         this.AccessibleDescription = null;
         this.AccessibleName = null;
         resources.ApplyResources(this, "$this");
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackgroundImage = null;
         this.Controls.Add(this.dataGridView1);
         this.Controls.Add(this.panel1);
         this.Font = null;
         this.Name = "MatrixBox";
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
