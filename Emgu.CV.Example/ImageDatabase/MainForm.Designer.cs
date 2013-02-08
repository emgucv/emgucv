namespace ImageDatabase
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
         this.panel1 = new System.Windows.Forms.Panel();
         this.button4 = new System.Windows.Forms.Button();
         this.button3 = new System.Windows.Forms.Button();
         this.button2 = new System.Windows.Forms.Button();
         this.button1 = new System.Windows.Forms.Button();
         this.panel2 = new System.Windows.Forms.Panel();
         this.dataGridView1 = new System.Windows.Forms.DataGridView();
         this.idColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.timeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.viewColumn = new System.Windows.Forms.DataGridViewButtonColumn();
         this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
         this.panel1.SuspendLayout();
         this.panel2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
         this.SuspendLayout();
         // 
         // panel1
         // 
         this.panel1.Controls.Add(this.button4);
         this.panel1.Controls.Add(this.button3);
         this.panel1.Controls.Add(this.button2);
         this.panel1.Controls.Add(this.button1);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panel1.Location = new System.Drawing.Point(0, 0);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(732, 72);
         this.panel1.TabIndex = 0;
         // 
         // button4
         // 
         this.button4.Location = new System.Drawing.Point(534, 11);
         this.button4.Name = "button4";
         this.button4.Size = new System.Drawing.Size(128, 23);
         this.button4.TabIndex = 3;
         this.button4.Text = "Clear Database";
         this.button4.UseVisualStyleBackColor = true;
         this.button4.Click += new System.EventHandler(this.clearDatabase_Click);
         // 
         // button3
         // 
         this.button3.Location = new System.Drawing.Point(347, 12);
         this.button3.Name = "button3";
         this.button3.Size = new System.Drawing.Size(159, 23);
         this.button3.TabIndex = 2;
         this.button3.Text = "Add 100 random images";
         this.button3.UseVisualStyleBackColor = true;
         this.button3.Click += new System.EventHandler(this.addHundredImages_Click);
         // 
         // button2
         // 
         this.button2.Location = new System.Drawing.Point(196, 12);
         this.button2.Name = "button2";
         this.button2.Size = new System.Drawing.Size(126, 23);
         this.button2.TabIndex = 1;
         this.button2.Text = "Add 10 random images";
         this.button2.UseVisualStyleBackColor = true;
         this.button2.Click += new System.EventHandler(this.addTenImages_Click);
         // 
         // button1
         // 
         this.button1.Location = new System.Drawing.Point(12, 12);
         this.button1.Name = "button1";
         this.button1.Size = new System.Drawing.Size(144, 23);
         this.button1.TabIndex = 0;
         this.button1.Text = "Add Images From File ...";
         this.button1.UseVisualStyleBackColor = true;
         this.button1.Click += new System.EventHandler(this.addImagesFromFiles_Click);
         // 
         // panel2
         // 
         this.panel2.Controls.Add(this.dataGridView1);
         this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panel2.Location = new System.Drawing.Point(0, 72);
         this.panel2.Name = "panel2";
         this.panel2.Size = new System.Drawing.Size(732, 375);
         this.panel2.TabIndex = 1;
         // 
         // dataGridView1
         // 
         this.dataGridView1.AllowUserToAddRows = false;
         this.dataGridView1.AllowUserToOrderColumns = true;
         this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idColumn,
            this.timeColumn,
            this.viewColumn});
         this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.dataGridView1.Location = new System.Drawing.Point(0, 0);
         this.dataGridView1.Name = "dataGridView1";
         this.dataGridView1.Size = new System.Drawing.Size(732, 375);
         this.dataGridView1.TabIndex = 0;
         this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
         // 
         // idColumn
         // 
         this.idColumn.HeaderText = "ID";
         this.idColumn.Name = "idColumn";
         this.idColumn.ReadOnly = true;
         // 
         // timeColumn
         // 
         this.timeColumn.HeaderText = "Time Stamp";
         this.timeColumn.Name = "timeColumn";
         this.timeColumn.ReadOnly = true;
         // 
         // viewColumn
         // 
         this.viewColumn.HeaderText = "View";
         this.viewColumn.Name = "viewColumn";
         this.viewColumn.Text = "";
         // 
         // openFileDialog1
         // 
         this.openFileDialog1.FileName = "openFileDialog1";
         this.openFileDialog1.Multiselect = true;
         // 
         // Form1
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(732, 447);
         this.Controls.Add(this.panel2);
         this.Controls.Add(this.panel1);
         this.Name = "Form1";
         this.Text = "Image Database";
         this.panel1.ResumeLayout(false);
         this.panel2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Panel panel1;
      private System.Windows.Forms.Button button3;
      private System.Windows.Forms.Button button2;
      private System.Windows.Forms.Button button1;
      private System.Windows.Forms.Panel panel2;
      private System.Windows.Forms.DataGridView dataGridView1;
      private System.Windows.Forms.DataGridViewTextBoxColumn idColumn;
      private System.Windows.Forms.DataGridViewTextBoxColumn timeColumn;
      private System.Windows.Forms.DataGridViewButtonColumn viewColumn;
      private System.Windows.Forms.Button button4;
      private System.Windows.Forms.OpenFileDialog openFileDialog1;
   }
}

