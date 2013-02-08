namespace Emgu.CV.UI
{
    partial class OperationsView
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
           this.topPanel = new System.Windows.Forms.Panel();
           this.bottomPanel = new System.Windows.Forms.Panel();
           this.dataGridView1 = new System.Windows.Forms.DataGridView();
           ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
           this.SuspendLayout();
           // 
           // topPanel
           // 
           this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
           this.topPanel.Location = new System.Drawing.Point(0, 0);
           this.topPanel.Name = "topPanel";
           this.topPanel.Size = new System.Drawing.Size(209, 41);
           this.topPanel.TabIndex = 1;
           // 
           // bottomPanel
           // 
           this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
           this.bottomPanel.Location = new System.Drawing.Point(0, 203);
           this.bottomPanel.Name = "bottomPanel";
           this.bottomPanel.Size = new System.Drawing.Size(209, 26);
           this.bottomPanel.TabIndex = 2;
           // 
           // dataGridView1
           // 
           this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
           this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
           this.dataGridView1.Location = new System.Drawing.Point(0, 41);
           this.dataGridView1.Name = "dataGridView1";
           this.dataGridView1.Size = new System.Drawing.Size(209, 162);
           this.dataGridView1.TabIndex = 3;
           // 
           // OperationsView
           // 
           this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
           this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
           this.Controls.Add(this.dataGridView1);
           this.Controls.Add(this.bottomPanel);
           this.Controls.Add(this.topPanel);
           this.Name = "OperationsView";
           this.Size = new System.Drawing.Size(209, 229);
           ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
           this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel topPanel;
       private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}
