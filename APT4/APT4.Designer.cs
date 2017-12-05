namespace APT4
{
    partial class APT4
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
            this.labMInvoiceNumberS = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labPrint = new System.Windows.Forms.Label();
            this.comPrinter = new System.Windows.Forms.ComboBox();
            this.butCheck = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labMInvoiceNumberS
            // 
            this.labMInvoiceNumberS.AutoSize = true;
            this.labMInvoiceNumberS.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labMInvoiceNumberS.Location = new System.Drawing.Point(1, 57);
            this.labMInvoiceNumberS.Name = "labMInvoiceNumberS";
            this.labMInvoiceNumberS.Size = new System.Drawing.Size(146, 16);
            this.labMInvoiceNumberS.TabIndex = 15;
            this.labMInvoiceNumberS.Text = "labMInvoiceNumberS";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(12, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 19);
            this.label1.TabIndex = 14;
            this.label1.Text = "請選擇印表機";
            // 
            // labPrint
            // 
            this.labPrint.AutoSize = true;
            this.labPrint.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labPrint.Location = new System.Drawing.Point(150, 7);
            this.labPrint.Name = "labPrint";
            this.labPrint.Size = new System.Drawing.Size(56, 16);
            this.labPrint.TabIndex = 13;
            this.labPrint.Text = "labPrint";
            // 
            // comPrinter
            // 
            this.comPrinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comPrinter.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.comPrinter.FormattingEnabled = true;
            this.comPrinter.Location = new System.Drawing.Point(4, 27);
            this.comPrinter.Name = "comPrinter";
            this.comPrinter.Size = new System.Drawing.Size(526, 24);
            this.comPrinter.TabIndex = 12;
            // 
            // butCheck
            // 
            this.butCheck.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.butCheck.Location = new System.Drawing.Point(153, 57);
            this.butCheck.Name = "butCheck";
            this.butCheck.Size = new System.Drawing.Size(87, 26);
            this.butCheck.TabIndex = 11;
            this.butCheck.Text = "確定";
            this.butCheck.UseVisualStyleBackColor = true;
            this.butCheck.Click += new System.EventHandler(this.butCheck_Click);
            // 
            // APT4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 90);
            this.Controls.Add(this.labMInvoiceNumberS);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labPrint);
            this.Controls.Add(this.comPrinter);
            this.Controls.Add(this.butCheck);
            this.Name = "APT4";
            this.Text = "APT4請選擇印表機";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labMInvoiceNumberS;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labPrint;
        private System.Windows.Forms.ComboBox comPrinter;
        private System.Windows.Forms.Button butCheck;
    }
}

