namespace UI
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Username = new System.Windows.Forms.Label();
            this.RMF = new System.Windows.Forms.Button();
            this.SLGF = new System.Windows.Forms.Button();
            this.NIF = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(64)))), ((int)(((byte)(153)))));
            this.panel1.Controls.Add(this.NIF);
            this.panel1.Controls.Add(this.SLGF);
            this.panel1.Controls.Add(this.RMF);
            this.panel1.Controls.Add(this.Username);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(186, 577);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Location = new System.Drawing.Point(0, 39);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(201, 100);
            this.panel2.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::UI.Properties.Resources.mmexport1709635195078;
            this.pictureBox1.Location = new System.Drawing.Point(12, -11);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(148, 111);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // Username
            // 
            this.Username.AutoSize = true;
            this.Username.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Username.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Username.Location = new System.Drawing.Point(43, 18);
            this.Username.Name = "Username";
            this.Username.Size = new System.Drawing.Size(90, 18);
            this.Username.TabIndex = 1;
            this.Username.Text = "User name";
            // 
            // RMF
            // 
            this.RMF.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(64)))), ((int)(((byte)(153)))));
            this.RMF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RMF.Font = new System.Drawing.Font("Nirmala UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RMF.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.RMF.Location = new System.Drawing.Point(0, 145);
            this.RMF.Name = "RMF";
            this.RMF.Size = new System.Drawing.Size(186, 42);
            this.RMF.TabIndex = 2;
            this.RMF.Text = "Recipe Management Form";
            this.RMF.UseVisualStyleBackColor = false;
            this.RMF.Click += new System.EventHandler(this.RMF_Click);
            // 
            // SLGF
            // 
            this.SLGF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SLGF.Font = new System.Drawing.Font("Nirmala UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SLGF.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.SLGF.Location = new System.Drawing.Point(0, 193);
            this.SLGF.Name = "SLGF";
            this.SLGF.Size = new System.Drawing.Size(186, 42);
            this.SLGF.TabIndex = 3;
            this.SLGF.Text = "Shopping List Generator Form";
            this.SLGF.UseVisualStyleBackColor = true;
            // 
            // NIF
            // 
            this.NIF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NIF.Font = new System.Drawing.Font("Nirmala UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NIF.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.NIF.Location = new System.Drawing.Point(0, 241);
            this.NIF.Name = "NIF";
            this.NIF.Size = new System.Drawing.Size(186, 42);
            this.NIF.TabIndex = 4;
            this.NIF.Text = "Nutrition Information Form";
            this.NIF.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(56)))), ((int)(((byte)(102)))));
            this.ClientSize = new System.Drawing.Size(951, 577);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label Username;
        private System.Windows.Forms.Button RMF;
        private System.Windows.Forms.Button NIF;
        private System.Windows.Forms.Button SLGF;
    }
}

