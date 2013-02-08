namespace Emgu.CV.UI
{
    partial class ParameterInputDialog
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
           System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParameterInputDialog));
           this.buttonsPanel = new System.Windows.Forms.Panel();
           this.cancelButton = new System.Windows.Forms.Button();
           this.okButton = new System.Windows.Forms.Button();
           this.parameterInputPanel = new System.Windows.Forms.Panel();
           this.buttonsPanel.SuspendLayout();
           this.SuspendLayout();
           // 
           // buttonsPanel
           // 
           this.buttonsPanel.AccessibleDescription = null;
           this.buttonsPanel.AccessibleName = null;
           resources.ApplyResources(this.buttonsPanel, "buttonsPanel");
           this.buttonsPanel.BackgroundImage = null;
           this.buttonsPanel.Controls.Add(this.cancelButton);
           this.buttonsPanel.Controls.Add(this.okButton);
           this.buttonsPanel.Font = null;
           this.buttonsPanel.Name = "buttonsPanel";
           // 
           // cancelButton
           // 
           this.cancelButton.AccessibleDescription = null;
           this.cancelButton.AccessibleName = null;
           resources.ApplyResources(this.cancelButton, "cancelButton");
           this.cancelButton.BackgroundImage = null;
           this.cancelButton.Font = null;
           this.cancelButton.Name = "cancelButton";
           this.cancelButton.UseVisualStyleBackColor = true;
           this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
           // 
           // okButton
           // 
           this.okButton.AccessibleDescription = null;
           this.okButton.AccessibleName = null;
           resources.ApplyResources(this.okButton, "okButton");
           this.okButton.BackgroundImage = null;
           this.okButton.Font = null;
           this.okButton.Name = "okButton";
           this.okButton.UseVisualStyleBackColor = true;
           this.okButton.Click += new System.EventHandler(this.okButton_Click);
           // 
           // parameterInputPanel
           // 
           this.parameterInputPanel.AccessibleDescription = null;
           this.parameterInputPanel.AccessibleName = null;
           resources.ApplyResources(this.parameterInputPanel, "parameterInputPanel");
           this.parameterInputPanel.BackgroundImage = null;
           this.parameterInputPanel.Font = null;
           this.parameterInputPanel.Name = "parameterInputPanel";
           // 
           // ParameterInputDialog
           // 
           this.AccessibleDescription = null;
           this.AccessibleName = null;
           resources.ApplyResources(this, "$this");
           this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
           this.BackgroundImage = null;
           this.Controls.Add(this.parameterInputPanel);
           this.Controls.Add(this.buttonsPanel);
           this.Font = null;
           this.Icon = null;
           this.Name = "ParameterInputDialog";
           this.buttonsPanel.ResumeLayout(false);
           this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel buttonsPanel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Panel parameterInputPanel;
    }
}
