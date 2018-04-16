namespace INF1009
{
    partial class Form1
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
            this.buttonStart = new System.Windows.Forms.Button();
            this.rtbS_lec = new System.Windows.Forms.RichTextBox();
            this.label_s_lec = new System.Windows.Forms.Label();
            this.rtbS_ecr = new System.Windows.Forms.RichTextBox();
            this.rtbL_lec = new System.Windows.Forms.RichTextBox();
            this.rtbL_ecr = new System.Windows.Forms.RichTextBox();
            this.label_s_ecr = new System.Windows.Forms.Label();
            this.label_l_lec = new System.Windows.Forms.Label();
            this.label_l_ecr = new System.Windows.Forms.Label();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonQuit = new System.Windows.Forms.Button();
            this.buttonGenerate = new System.Windows.Forms.Button();
            this.richTextBoxGen = new System.Windows.Forms.RichTextBox();
            this.buttonSend2File = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStart.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.buttonStart.Location = new System.Drawing.Point(703, 362);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(158, 28);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "Start/Resume";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // rtbS_lec
            // 
            this.rtbS_lec.Location = new System.Drawing.Point(16, 27);
            this.rtbS_lec.Name = "rtbS_lec";
            this.rtbS_lec.Size = new System.Drawing.Size(330, 221);
            this.rtbS_lec.TabIndex = 1;
            this.rtbS_lec.Text = "";
            // 
            // label_s_lec
            // 
            this.label_s_lec.AutoSize = true;
            this.label_s_lec.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_s_lec.Location = new System.Drawing.Point(13, 11);
            this.label_s_lec.Name = "label_s_lec";
            this.label_s_lec.Size = new System.Drawing.Size(36, 14);
            this.label_s_lec.TabIndex = 2;
            this.label_s_lec.Text = "S_lec";
            // 
            // rtbS_ecr
            // 
            this.rtbS_ecr.Location = new System.Drawing.Point(359, 27);
            this.rtbS_ecr.Name = "rtbS_ecr";
            this.rtbS_ecr.Size = new System.Drawing.Size(329, 221);
            this.rtbS_ecr.TabIndex = 3;
            this.rtbS_ecr.Text = "";
            // 
            // rtbL_lec
            // 
            this.rtbL_lec.Location = new System.Drawing.Point(16, 267);
            this.rtbL_lec.Name = "rtbL_lec";
            this.rtbL_lec.Size = new System.Drawing.Size(330, 214);
            this.rtbL_lec.TabIndex = 4;
            this.rtbL_lec.Text = "";
            // 
            // rtbL_ecr
            // 
            this.rtbL_ecr.Location = new System.Drawing.Point(359, 267);
            this.rtbL_ecr.Name = "rtbL_ecr";
            this.rtbL_ecr.Size = new System.Drawing.Size(329, 214);
            this.rtbL_ecr.TabIndex = 5;
            this.rtbL_ecr.Text = "";
            // 
            // label_s_ecr
            // 
            this.label_s_ecr.AutoSize = true;
            this.label_s_ecr.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_s_ecr.Location = new System.Drawing.Point(356, 11);
            this.label_s_ecr.Name = "label_s_ecr";
            this.label_s_ecr.Size = new System.Drawing.Size(38, 14);
            this.label_s_ecr.TabIndex = 6;
            this.label_s_ecr.Text = "S_ecr";
            // 
            // label_l_lec
            // 
            this.label_l_lec.AutoSize = true;
            this.label_l_lec.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_l_lec.Location = new System.Drawing.Point(13, 251);
            this.label_l_lec.Name = "label_l_lec";
            this.label_l_lec.Size = new System.Drawing.Size(36, 14);
            this.label_l_lec.TabIndex = 7;
            this.label_l_lec.Text = "L_lec";
            // 
            // label_l_ecr
            // 
            this.label_l_ecr.AutoSize = true;
            this.label_l_ecr.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_l_ecr.Location = new System.Drawing.Point(356, 251);
            this.label_l_ecr.Name = "label_l_ecr";
            this.label_l_ecr.Size = new System.Drawing.Size(38, 14);
            this.label_l_ecr.TabIndex = 8;
            this.label_l_ecr.Text = "L_ecr";
            // 
            // buttonReset
            // 
            this.buttonReset.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonReset.Location = new System.Drawing.Point(703, 405);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(158, 28);
            this.buttonReset.TabIndex = 9;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // buttonQuit
            // 
            this.buttonQuit.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonQuit.Location = new System.Drawing.Point(703, 453);
            this.buttonQuit.Name = "buttonQuit";
            this.buttonQuit.Size = new System.Drawing.Size(158, 28);
            this.buttonQuit.TabIndex = 10;
            this.buttonQuit.Text = "Quit";
            this.buttonQuit.UseVisualStyleBackColor = true;
            this.buttonQuit.Click += new System.EventHandler(this.buttonQuit_Click);
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonGenerate.Location = new System.Drawing.Point(703, 132);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(158, 23);
            this.buttonGenerate.TabIndex = 11;
            this.buttonGenerate.Text = "Generate";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
            // 
            // richTextBoxGen
            // 
            this.richTextBoxGen.Location = new System.Drawing.Point(703, 27);
            this.richTextBoxGen.Name = "richTextBoxGen";
            this.richTextBoxGen.Size = new System.Drawing.Size(158, 85);
            this.richTextBoxGen.TabIndex = 12;
            this.richTextBoxGen.Text = "";
            // 
            // buttonSend2File
            // 
            this.buttonSend2File.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSend2File.Location = new System.Drawing.Point(703, 171);
            this.buttonSend2File.Name = "buttonSend2File";
            this.buttonSend2File.Size = new System.Drawing.Size(158, 23);
            this.buttonSend2File.TabIndex = 13;
            this.buttonSend2File.Text = "Send to file";
            this.buttonSend2File.UseVisualStyleBackColor = true;
            this.buttonSend2File.Click += new System.EventHandler(this.buttonSend2File_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 491);
            this.Controls.Add(this.buttonSend2File);
            this.Controls.Add(this.richTextBoxGen);
            this.Controls.Add(this.buttonGenerate);
            this.Controls.Add(this.buttonQuit);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.label_l_ecr);
            this.Controls.Add(this.label_l_lec);
            this.Controls.Add(this.label_s_ecr);
            this.Controls.Add(this.rtbL_ecr);
            this.Controls.Add(this.rtbL_lec);
            this.Controls.Add(this.rtbS_ecr);
            this.Controls.Add(this.label_s_lec);
            this.Controls.Add(this.rtbS_lec);
            this.Controls.Add(this.buttonStart);
            this.Name = "Form1";
            this.Text = "INF1009";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.RichTextBox rtbS_lec;
        private System.Windows.Forms.Label label_s_lec;
        private System.Windows.Forms.RichTextBox rtbS_ecr;
        private System.Windows.Forms.RichTextBox rtbL_lec;
        private System.Windows.Forms.RichTextBox rtbL_ecr;
        private System.Windows.Forms.Label label_s_ecr;
        private System.Windows.Forms.Label label_l_lec;
        private System.Windows.Forms.Label label_l_ecr;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonQuit;
        private System.Windows.Forms.Button buttonGenerate;
        private System.Windows.Forms.RichTextBox richTextBoxGen;
        private System.Windows.Forms.Button buttonSend2File;
    }
}

