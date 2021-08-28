namespace TrialProgram
{
    partial class CodeForm
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
            this.TextCode = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TextUri = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // TextCode
            // 
            this.TextCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextCode.Location = new System.Drawing.Point(12, 67);
            this.TextCode.Name = "TextCode";
            this.TextCode.ReadOnly = true;
            this.TextCode.Size = new System.Drawing.Size(868, 656);
            this.TextCode.TabIndex = 0;
            this.TextCode.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Uri:";
            // 
            // TextUri
            // 
            this.TextUri.AutoSize = true;
            this.TextUri.Location = new System.Drawing.Point(45, 20);
            this.TextUri.Name = "TextUri";
            this.TextUri.Size = new System.Drawing.Size(0, 17);
            this.TextUri.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Текст страницы";
            // 
            // CodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 735);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TextUri);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TextCode);
            this.Name = "CodeForm";
            this.Text = "Ссылка в коде";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox TextCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label TextUri;
        private System.Windows.Forms.Label label2;
    }
}