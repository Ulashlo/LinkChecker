namespace TrialProgram
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
            this.SitesListGridView = new System.Windows.Forms.DataGridView();
            this.recursionLevel = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.findButton = new System.Windows.Forms.Button();
            this.textUrl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.SitesListGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.recursionLevel)).BeginInit();
            this.SuspendLayout();
            // 
            // SitesListGridView
            // 
            this.SitesListGridView.AllowUserToAddRows = false;
            this.SitesListGridView.AllowUserToDeleteRows = false;
            this.SitesListGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SitesListGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SitesListGridView.Location = new System.Drawing.Point(12, 68);
            this.SitesListGridView.Name = "SitesListGridView";
            this.SitesListGridView.ReadOnly = true;
            this.SitesListGridView.RowTemplate.Height = 24;
            this.SitesListGridView.Size = new System.Drawing.Size(858, 261);
            this.SitesListGridView.TabIndex = 0;
            this.SitesListGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.SitesListGridView_CellMouseDoubleClick);
            // 
            // recursionLevel
            // 
            this.recursionLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.recursionLevel.Location = new System.Drawing.Point(671, 13);
            this.recursionLevel.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.recursionLevel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.recursionLevel.Name = "recursionLevel";
            this.recursionLevel.Size = new System.Drawing.Size(57, 22);
            this.recursionLevel.TabIndex = 14;
            this.recursionLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.recursionLevel.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(512, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(153, 17);
            this.label4.TabIndex = 13;
            this.label4.Text = "Уровень вложенности";
            // 
            // findButton
            // 
            this.findButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.findButton.Location = new System.Drawing.Point(746, 12);
            this.findButton.Name = "findButton";
            this.findButton.Size = new System.Drawing.Size(125, 23);
            this.findButton.TabIndex = 12;
            this.findButton.Text = "Добавить";
            this.findButton.UseVisualStyleBackColor = true;
            this.findButton.Click += new System.EventHandler(this.findButton_Click);
            // 
            // textUrl
            // 
            this.textUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textUrl.Location = new System.Drawing.Point(54, 12);
            this.textUrl.Name = "textUrl";
            this.textUrl.Size = new System.Drawing.Size(452, 22);
            this.textUrl.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 17);
            this.label1.TabIndex = 10;
            this.label1.Text = "URL:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 17);
            this.label2.TabIndex = 15;
            this.label2.Text = "Список сайтов:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 341);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.recursionLevel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.findButton);
            this.Controls.Add(this.textUrl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SitesListGridView);
            this.MinimumSize = new System.Drawing.Size(600, 300);
            this.Name = "MainForm";
            this.Text = "Сыщик ссылок";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SitesListGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.recursionLevel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView SitesListGridView;
        private System.Windows.Forms.NumericUpDown recursionLevel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button findButton;
        private System.Windows.Forms.TextBox textUrl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}