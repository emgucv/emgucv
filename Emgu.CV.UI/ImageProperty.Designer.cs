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

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageProperty));
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
         this.tabPage2 = new System.Windows.Forms.TabPage();
         this.removeAllFilterButton = new System.Windows.Forms.Button();
         this.removeFilterButton = new System.Windows.Forms.Button();
         this.showHistogramButton = new System.Windows.Forms.Button();
         this.zoomLevelComboBox = new System.Windows.Forms.ComboBox();
         this.label9 = new System.Windows.Forms.Label();
         this.splitter1 = new System.Windows.Forms.Splitter();
         this.cSharpOperationView = new Emgu.CV.UI.OperationsView();
         this.cPlusPlusOperationView = new Emgu.CV.UI.OperationsView();
         this.tabControl1.SuspendLayout();
         this.tabPage1.SuspendLayout();
         this.tabPage2.SuspendLayout();
         this.SuspendLayout();
         // 
         // label1
         // 
         resources.ApplyResources(this.label1, "label1");
         this.label1.Name = "label1";
         // 
         // widthTextbox
         // 
         resources.ApplyResources(this.widthTextbox, "widthTextbox");
         this.widthTextbox.Name = "widthTextbox";
         this.widthTextbox.ReadOnly = true;
         // 
         // heightTextBox
         // 
         resources.ApplyResources(this.heightTextBox, "heightTextBox");
         this.heightTextBox.Name = "heightTextBox";
         this.heightTextBox.ReadOnly = true;
         // 
         // label2
         // 
         resources.ApplyResources(this.label2, "label2");
         this.label2.Name = "label2";
         // 
         // label3
         // 
         resources.ApplyResources(this.label3, "label3");
         this.label3.Name = "label3";
         // 
         // mousePositionTextbox
         // 
         resources.ApplyResources(this.mousePositionTextbox, "mousePositionTextbox");
         this.mousePositionTextbox.Name = "mousePositionTextbox";
         this.mousePositionTextbox.ReadOnly = true;
         // 
         // label4
         // 
         resources.ApplyResources(this.label4, "label4");
         this.label4.Name = "label4";
         // 
         // colorIntensityTextbox
         // 
         resources.ApplyResources(this.colorIntensityTextbox, "colorIntensityTextbox");
         this.colorIntensityTextbox.Name = "colorIntensityTextbox";
         this.colorIntensityTextbox.ReadOnly = true;
         // 
         // label5
         // 
         resources.ApplyResources(this.label5, "label5");
         this.label5.Name = "label5";
         // 
         // typeOfColorTexbox
         // 
         resources.ApplyResources(this.typeOfColorTexbox, "typeOfColorTexbox");
         this.typeOfColorTexbox.Name = "typeOfColorTexbox";
         this.typeOfColorTexbox.ReadOnly = true;
         // 
         // label6
         // 
         resources.ApplyResources(this.label6, "label6");
         this.label6.Name = "label6";
         // 
         // typeOfDepthTextBox
         // 
         resources.ApplyResources(this.typeOfDepthTextBox, "typeOfDepthTextBox");
         this.typeOfDepthTextBox.Name = "typeOfDepthTextBox";
         this.typeOfDepthTextBox.ReadOnly = true;
         // 
         // label7
         // 
         resources.ApplyResources(this.label7, "label7");
         this.label7.Name = "label7";
         // 
         // label8
         // 
         resources.ApplyResources(this.label8, "label8");
         this.label8.Name = "label8";
         // 
         // fpsTextBox
         // 
         resources.ApplyResources(this.fpsTextBox, "fpsTextBox");
         this.fpsTextBox.Name = "fpsTextBox";
         this.fpsTextBox.ReadOnly = true;
         // 
         // tabControl1
         // 
         this.tabControl1.Controls.Add(this.tabPage1);
         this.tabControl1.Controls.Add(this.tabPage2);
         resources.ApplyResources(this.tabControl1, "tabControl1");
         this.tabControl1.Name = "tabControl1";
         this.tabControl1.SelectedIndex = 0;
         // 
         // tabPage1
         // 
         this.tabPage1.Controls.Add(this.cSharpOperationView);
         resources.ApplyResources(this.tabPage1, "tabPage1");
         this.tabPage1.Name = "tabPage1";
         this.tabPage1.UseVisualStyleBackColor = true;
         // 
         // tabPage2
         // 
         this.tabPage2.Controls.Add(this.splitter1);
         this.tabPage2.Controls.Add(this.cPlusPlusOperationView);
         resources.ApplyResources(this.tabPage2, "tabPage2");
         this.tabPage2.Name = "tabPage2";
         this.tabPage2.UseVisualStyleBackColor = true;
         // 
         // removeAllFilterButton
         // 
         resources.ApplyResources(this.removeAllFilterButton, "removeAllFilterButton");
         this.removeAllFilterButton.Name = "removeAllFilterButton";
         this.removeAllFilterButton.UseVisualStyleBackColor = true;
         this.removeAllFilterButton.Click += new System.EventHandler(this.clearOperationBtn_Click);
         // 
         // removeFilterButton
         // 
         resources.ApplyResources(this.removeFilterButton, "removeFilterButton");
         this.removeFilterButton.Name = "removeFilterButton";
         this.removeFilterButton.UseVisualStyleBackColor = true;
         this.removeFilterButton.Click += new System.EventHandler(this.popOperationButton_Click);
         // 
         // showHistogramButton
         // 
         resources.ApplyResources(this.showHistogramButton, "showHistogramButton");
         this.showHistogramButton.Name = "showHistogramButton";
         this.showHistogramButton.UseVisualStyleBackColor = true;
         this.showHistogramButton.Click += new System.EventHandler(this.showHistogramButton_Click);
         // 
         // zoomLevelComboBox
         // 
         this.zoomLevelComboBox.FormattingEnabled = true;
         resources.ApplyResources(this.zoomLevelComboBox, "zoomLevelComboBox");
         this.zoomLevelComboBox.Name = "zoomLevelComboBox";
         this.zoomLevelComboBox.SelectedIndexChanged += new System.EventHandler(this.zoomLevelComboBox_SelectedIndexChanged);
         // 
         // label9
         // 
         resources.ApplyResources(this.label9, "label9");
         this.label9.Name = "label9";
         // 
         // splitter1
         // 
         resources.ApplyResources(this.splitter1, "splitter1");
         this.splitter1.Name = "splitter1";
         this.splitter1.TabStop = false;
         // 
         // cSharpOperationView
         // 
         resources.ApplyResources(this.cSharpOperationView, "cSharpOperationView");
         this.cSharpOperationView.Name = "cSharpOperationView";
         // 
         // cPlusPlusOperationView
         // 
         resources.ApplyResources(this.cPlusPlusOperationView, "cPlusPlusOperationView");
         this.cPlusPlusOperationView.Name = "cPlusPlusOperationView";
         // 
         // ImageProperty
         // 
         resources.ApplyResources(this, "$this");
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.label9);
         this.Controls.Add(this.zoomLevelComboBox);
         this.Controls.Add(this.showHistogramButton);
         this.Controls.Add(this.removeFilterButton);
         this.Controls.Add(this.removeAllFilterButton);
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
      private System.Windows.Forms.Button removeAllFilterButton;
      private System.Windows.Forms.Button removeFilterButton;
      private System.Windows.Forms.Button showHistogramButton;
      private System.Windows.Forms.ComboBox zoomLevelComboBox;
      private System.Windows.Forms.Label label9;
      private System.Windows.Forms.Splitter splitter1;
   }
}
