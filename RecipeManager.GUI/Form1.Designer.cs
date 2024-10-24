namespace UI
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.PC = new System.Windows.Forms.Button();
            this.NIF = new System.Windows.Forms.Button();
            this.SLGF = new System.Windows.Forms.Button();
            this.RMF = new System.Windows.Forms.Button();
            this.Username = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelRecipeManagement = new System.Windows.Forms.Panel();
            this.panelShoppingList = new System.Windows.Forms.Panel();
            this.panelNutritionInfo = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(64)))), ((int)(((byte)(153)))));
            this.panel1.Controls.Add(this.PC);
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
            // PC
            // 
            this.PC.Location = new System.Drawing.Point(0, 0);
            this.PC.Name = "PC";
            this.PC.Size = new System.Drawing.Size(75, 23);
            this.PC.TabIndex = 0;
            // 
            // NIF
            // 
            this.NIF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NIF.Font = new System.Drawing.Font("Nirmala UI Semilight", 9.75F);
            this.NIF.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.NIF.Location = new System.Drawing.Point(0, 241);
            this.NIF.Name = "NIF";
            this.NIF.Size = new System.Drawing.Size(186, 42);
            this.NIF.TabIndex = 4;
            this.NIF.Text = "Nutrition Information Form";
            this.NIF.UseVisualStyleBackColor = true;
            this.NIF.Click += new System.EventHandler(this.NIF_Click);
            // 
            // SLGF
            // 
            this.SLGF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SLGF.Font = new System.Drawing.Font("Nirmala UI Semilight", 9.75F);
            this.SLGF.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.SLGF.Location = new System.Drawing.Point(0, 193);
            this.SLGF.Name = "SLGF";
            this.SLGF.Size = new System.Drawing.Size(186, 42);
            this.SLGF.TabIndex = 3;
            this.SLGF.Text = "Shopping List Generator Form";
            this.SLGF.UseVisualStyleBackColor = true;
            this.SLGF.Click += new System.EventHandler(this.SLGF_Click);
            // 
            // RMF
            // 
            this.RMF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RMF.Font = new System.Drawing.Font("Nirmala UI Semilight", 9.75F);
            this.RMF.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.RMF.Location = new System.Drawing.Point(0, 145);
            this.RMF.Name = "RMF";
            this.RMF.Size = new System.Drawing.Size(186, 42);
            this.RMF.TabIndex = 2;
            this.RMF.Text = "Recipe Management Form";
            this.RMF.UseVisualStyleBackColor = false;
            this.RMF.Click += new System.EventHandler(this.RMF_Click);
            // 
            // Username
            // 
            this.Username.Location = new System.Drawing.Point(0, 0);
            this.Username.Name = "Username";
            this.Username.Size = new System.Drawing.Size(100, 23);
            this.Username.TabIndex = 5;
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 100);
            this.panel2.TabIndex = 6;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panelRecipeManagement
            // 
            this.panelRecipeManagement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRecipeManagement.Location = new System.Drawing.Point(0, 0);
            this.panelRecipeManagement.Name = "panelRecipeManagement";
            this.panelRecipeManagement.Size = new System.Drawing.Size(930, 537);
            this.panelRecipeManagement.TabIndex = 1;
            this.panelRecipeManagement.Visible = false;
            // 
            // panelShoppingList
            // 
            this.panelShoppingList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelShoppingList.Location = new System.Drawing.Point(0, 0);
            this.panelShoppingList.Name = "panelShoppingList";
            this.panelShoppingList.Size = new System.Drawing.Size(930, 537);
            this.panelShoppingList.TabIndex = 2;
            this.panelShoppingList.Visible = false;
            // 
            // panelNutritionInfo
            // 
            this.panelNutritionInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelNutritionInfo.Location = new System.Drawing.Point(0, 0);
            this.panelNutritionInfo.Name = "panelNutritionInfo";
            this.panelNutritionInfo.Size = new System.Drawing.Size(930, 537);
            this.panelNutritionInfo.TabIndex = 3;
            this.panelNutritionInfo.Visible = false;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(930, 537);
            this.Controls.Add(this.panelRecipeManagement);
            this.Controls.Add(this.panelShoppingList);
            this.Controls.Add(this.panelNutritionInfo);
            this.Name = "Form1";
            this.panel1.ResumeLayout(false);
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
        private System.Windows.Forms.Button PC;
        private System.Windows.Forms.Panel panelRecipeManagement;
        private System.Windows.Forms.Panel panelShoppingList;
        private System.Windows.Forms.Panel panelNutritionInfo;
    }

