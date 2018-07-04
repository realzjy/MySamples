namespace RabbitMQ服務器加客戶端測試例子
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
            this.bSend = new System.Windows.Forms.Button();
            this.bReceive = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.bOpenQ = new System.Windows.Forms.Button();
            this.bCloseQ = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bSend
            // 
            this.bSend.Location = new System.Drawing.Point(158, 38);
            this.bSend.Name = "bSend";
            this.bSend.Size = new System.Drawing.Size(96, 23);
            this.bSend.TabIndex = 0;
            this.bSend.Text = "發送一個數據";
            this.bSend.UseVisualStyleBackColor = true;
            this.bSend.Click += new System.EventHandler(this.bSend_Click);
            // 
            // bReceive
            // 
            this.bReceive.Location = new System.Drawing.Point(158, 84);
            this.bReceive.Name = "bReceive";
            this.bReceive.Size = new System.Drawing.Size(96, 23);
            this.bReceive.TabIndex = 1;
            this.bReceive.Text = "獲取數據";
            this.bReceive.UseVisualStyleBackColor = true;
            this.bReceive.Click += new System.EventHandler(this.bReceive_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(60, 152);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(457, 151);
            this.textBox1.TabIndex = 2;
            // 
            // bOpenQ
            // 
            this.bOpenQ.Location = new System.Drawing.Point(66, 38);
            this.bOpenQ.Name = "bOpenQ";
            this.bOpenQ.Size = new System.Drawing.Size(75, 23);
            this.bOpenQ.TabIndex = 3;
            this.bOpenQ.Text = "打開通道";
            this.bOpenQ.UseVisualStyleBackColor = true;
            this.bOpenQ.Click += new System.EventHandler(this.bOpenQ_Click);
            // 
            // bCloseQ
            // 
            this.bCloseQ.Location = new System.Drawing.Point(66, 84);
            this.bCloseQ.Name = "bCloseQ";
            this.bCloseQ.Size = new System.Drawing.Size(75, 23);
            this.bCloseQ.TabIndex = 3;
            this.bCloseQ.Text = "關閉通道";
            this.bCloseQ.UseVisualStyleBackColor = true;
            this.bCloseQ.Click += new System.EventHandler(this.bCloseQ_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 355);
            this.Controls.Add(this.bCloseQ);
            this.Controls.Add(this.bOpenQ);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.bReceive);
            this.Controls.Add(this.bSend);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bSend;
        private System.Windows.Forms.Button bReceive;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button bOpenQ;
        private System.Windows.Forms.Button bCloseQ;
    }
}

