namespace Base64S
{
    partial class Base64S
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.but_S_to_64 = new System.Windows.Forms.Button();
            this.but_64_to_S = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.but_Close = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.but_S_to_64);
            this.groupBox1.Controls.Add(this.but_64_to_S);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(472, 144);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "變身 Base64";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(16, 104);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(248, 22);
            this.textBox3.TabIndex = 4;
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // but_S_to_64
            // 
            this.but_S_to_64.Location = new System.Drawing.Point(272, 24);
            this.but_S_to_64.Name = "but_S_to_64";
            this.but_S_to_64.Size = new System.Drawing.Size(184, 56);
            this.but_S_to_64.TabIndex = 0;
            this.but_S_to_64.Text = "Convert String to Base64";
            this.but_S_to_64.Click += new System.EventHandler(this.but_S_to_64_Click);
            // 
            // but_64_to_S
            // 
            this.but_64_to_S.Location = new System.Drawing.Point(272, 104);
            this.but_64_to_S.Name = "but_64_to_S";
            this.but_64_to_S.Size = new System.Drawing.Size(184, 24);
            this.but_64_to_S.TabIndex = 5;
            this.but_64_to_S.Text = "Convert Base64 string to String";
            this.but_64_to_S.Click += new System.EventHandler(this.but_64_to_S_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(16, 56);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(248, 22);
            this.textBox2.TabIndex = 2;
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(16, 24);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(248, 22);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "1qaz2wsx";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // but_Close
            // 
            this.but_Close.Location = new System.Drawing.Point(490, 20);
            this.but_Close.Name = "but_Close";
            this.but_Close.Size = new System.Drawing.Size(41, 136);
            this.but_Close.TabIndex = 9;
            this.but_Close.Text = "Bye";
            this.but_Close.Click += new System.EventHandler(this.but_Close_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(284, 162);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(184, 24);
            this.button1.TabIndex = 10;
            this.button1.Text = "測換行";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 170);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 12);
            this.label1.TabIndex = 11;
            // 
            // Base64S
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 243);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.but_Close);
            this.Controls.Add(this.groupBox1);
            this.Name = "Base64S";
            this.Text = "Base64S";
            this.Load += new System.EventHandler(this.Base64S_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button but_S_to_64;
        private System.Windows.Forms.Button but_64_to_S;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button but_Close;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
    }
}

