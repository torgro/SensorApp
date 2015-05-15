namespace FormsAsyncTest
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
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.data_main = new System.Windows.Forms.DataGridView();
            this.btn_logs = new System.Windows.Forms.Button();
            this.data_Stats = new System.Windows.Forms.DataGridView();
            this.btn_device = new System.Windows.Forms.Button();
            this.btn_AllPackets = new System.Windows.Forms.Button();
            this.btn_ManPacket = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.data_main)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.data_Stats)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 69);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 48);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(119, 10);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(530, 22);
            this.textBox1.TabIndex = 2;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(1043, 10);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(203, 22);
            this.textBox2.TabIndex = 3;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(119, 483);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(819, 95);
            this.textBox3.TabIndex = 4;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 123);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(86, 46);
            this.button2.TabIndex = 5;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // data_main
            // 
            this.data_main.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.data_main.Location = new System.Drawing.Point(119, 67);
            this.data_main.Name = "data_main";
            this.data_main.RowTemplate.Height = 24;
            this.data_main.Size = new System.Drawing.Size(1157, 223);
            this.data_main.TabIndex = 6;
            // 
            // btn_logs
            // 
            this.btn_logs.Location = new System.Drawing.Point(12, 175);
            this.btn_logs.Name = "btn_logs";
            this.btn_logs.Size = new System.Drawing.Size(86, 46);
            this.btn_logs.TabIndex = 7;
            this.btn_logs.Text = "Logs";
            this.btn_logs.UseVisualStyleBackColor = true;
            this.btn_logs.Click += new System.EventHandler(this.btn_logs_Click);
            // 
            // data_Stats
            // 
            this.data_Stats.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.data_Stats.Location = new System.Drawing.Point(963, 483);
            this.data_Stats.Name = "data_Stats";
            this.data_Stats.RowTemplate.Height = 24;
            this.data_Stats.Size = new System.Drawing.Size(313, 233);
            this.data_Stats.TabIndex = 8;
            // 
            // btn_device
            // 
            this.btn_device.Location = new System.Drawing.Point(12, 227);
            this.btn_device.Name = "btn_device";
            this.btn_device.Size = new System.Drawing.Size(86, 46);
            this.btn_device.TabIndex = 9;
            this.btn_device.Text = "Device";
            this.btn_device.UseVisualStyleBackColor = true;
            this.btn_device.Click += new System.EventHandler(this.btn_device_Click);
            // 
            // btn_AllPackets
            // 
            this.btn_AllPackets.Location = new System.Drawing.Point(12, 279);
            this.btn_AllPackets.Name = "btn_AllPackets";
            this.btn_AllPackets.Size = new System.Drawing.Size(86, 46);
            this.btn_AllPackets.TabIndex = 10;
            this.btn_AllPackets.Text = "AllPackets";
            this.btn_AllPackets.UseVisualStyleBackColor = true;
            this.btn_AllPackets.Click += new System.EventHandler(this.btn_AllPackets_Click);
            // 
            // btn_ManPacket
            // 
            this.btn_ManPacket.Location = new System.Drawing.Point(655, 10);
            this.btn_ManPacket.Name = "btn_ManPacket";
            this.btn_ManPacket.Size = new System.Drawing.Size(145, 48);
            this.btn_ManPacket.TabIndex = 11;
            this.btn_ManPacket.Text = "ManualPacket";
            this.btn_ManPacket.UseVisualStyleBackColor = true;
            this.btn_ManPacket.Click += new System.EventHandler(this.btn_ManPacket_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1366, 745);
            this.Controls.Add(this.btn_ManPacket);
            this.Controls.Add(this.btn_AllPackets);
            this.Controls.Add(this.btn_device);
            this.Controls.Add(this.data_Stats);
            this.Controls.Add(this.btn_logs);
            this.Controls.Add(this.data_main);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.data_main)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.data_Stats)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView data_main;
        private System.Windows.Forms.Button btn_logs;
        private System.Windows.Forms.DataGridView data_Stats;
        private System.Windows.Forms.Button btn_device;
        private System.Windows.Forms.Button btn_AllPackets;
        private System.Windows.Forms.Button btn_ManPacket;
    }
}

