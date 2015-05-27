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
            this.btn_DataSample = new System.Windows.Forms.Button();
            this.btn_testcmd = new System.Windows.Forms.Button();
            this.btn_RemoteCmd = new System.Windows.Forms.Button();
            this.btn_TestSample = new System.Windows.Forms.Button();
            this.btn_tasks = new System.Windows.Forms.Button();
            this.btn_Connect = new System.Windows.Forms.Button();
            this.btn_EnableD0 = new System.Windows.Forms.Button();
            this.btn_DisableD0 = new System.Windows.Forms.Button();
            this.btn_StatusD0 = new System.Windows.Forms.Button();
            this.btn_TestAzure = new System.Windows.Forms.Button();
            this.btn_pushbullet = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.data_main)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.data_Stats)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 69);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 48);
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
            this.textBox2.Location = new System.Drawing.Point(1305, 56);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(49, 22);
            this.textBox2.TabIndex = 3;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(119, 539);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(819, 95);
            this.textBox3.TabIndex = 4;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 123);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(101, 46);
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
            this.data_main.Size = new System.Drawing.Size(1157, 381);
            this.data_main.TabIndex = 6;
            // 
            // btn_logs
            // 
            this.btn_logs.Location = new System.Drawing.Point(12, 175);
            this.btn_logs.Name = "btn_logs";
            this.btn_logs.Size = new System.Drawing.Size(101, 46);
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
            this.btn_device.Size = new System.Drawing.Size(101, 46);
            this.btn_device.TabIndex = 9;
            this.btn_device.Text = "Device";
            this.btn_device.UseVisualStyleBackColor = true;
            this.btn_device.Click += new System.EventHandler(this.btn_device_Click);
            // 
            // btn_AllPackets
            // 
            this.btn_AllPackets.Location = new System.Drawing.Point(12, 279);
            this.btn_AllPackets.Name = "btn_AllPackets";
            this.btn_AllPackets.Size = new System.Drawing.Size(101, 46);
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
            // btn_DataSample
            // 
            this.btn_DataSample.Location = new System.Drawing.Point(12, 331);
            this.btn_DataSample.Name = "btn_DataSample";
            this.btn_DataSample.Size = new System.Drawing.Size(101, 46);
            this.btn_DataSample.TabIndex = 12;
            this.btn_DataSample.Text = "DataSample";
            this.btn_DataSample.UseVisualStyleBackColor = true;
            this.btn_DataSample.Click += new System.EventHandler(this.btn_DataSample_Click);
            // 
            // btn_testcmd
            // 
            this.btn_testcmd.Location = new System.Drawing.Point(815, 12);
            this.btn_testcmd.Name = "btn_testcmd";
            this.btn_testcmd.Size = new System.Drawing.Size(145, 48);
            this.btn_testcmd.TabIndex = 13;
            this.btn_testcmd.Text = "TestRemoteCmd";
            this.btn_testcmd.UseVisualStyleBackColor = true;
            this.btn_testcmd.Click += new System.EventHandler(this.btn_testcmd_Click);
            // 
            // btn_RemoteCmd
            // 
            this.btn_RemoteCmd.Location = new System.Drawing.Point(12, 383);
            this.btn_RemoteCmd.Name = "btn_RemoteCmd";
            this.btn_RemoteCmd.Size = new System.Drawing.Size(101, 46);
            this.btn_RemoteCmd.TabIndex = 14;
            this.btn_RemoteCmd.Text = "RemoteCmd";
            this.btn_RemoteCmd.UseVisualStyleBackColor = true;
            this.btn_RemoteCmd.Click += new System.EventHandler(this.btn_RemoteCmd_Click);
            // 
            // btn_TestSample
            // 
            this.btn_TestSample.Location = new System.Drawing.Point(975, 13);
            this.btn_TestSample.Name = "btn_TestSample";
            this.btn_TestSample.Size = new System.Drawing.Size(145, 48);
            this.btn_TestSample.TabIndex = 15;
            this.btn_TestSample.Text = "TestSample";
            this.btn_TestSample.UseVisualStyleBackColor = true;
            this.btn_TestSample.Click += new System.EventHandler(this.btn_TestSample_Click);
            // 
            // btn_tasks
            // 
            this.btn_tasks.Location = new System.Drawing.Point(12, 435);
            this.btn_tasks.Name = "btn_tasks";
            this.btn_tasks.Size = new System.Drawing.Size(101, 46);
            this.btn_tasks.TabIndex = 16;
            this.btn_tasks.Text = "Tasks";
            this.btn_tasks.UseVisualStyleBackColor = true;
            this.btn_tasks.Click += new System.EventHandler(this.btn_tasks_Click);
            // 
            // btn_Connect
            // 
            this.btn_Connect.Location = new System.Drawing.Point(12, 487);
            this.btn_Connect.Name = "btn_Connect";
            this.btn_Connect.Size = new System.Drawing.Size(101, 46);
            this.btn_Connect.TabIndex = 17;
            this.btn_Connect.Text = "Connect";
            this.btn_Connect.UseVisualStyleBackColor = true;
            this.btn_Connect.Click += new System.EventHandler(this.btn_Connect_Click);
            // 
            // btn_EnableD0
            // 
            this.btn_EnableD0.Location = new System.Drawing.Point(143, 466);
            this.btn_EnableD0.Name = "btn_EnableD0";
            this.btn_EnableD0.Size = new System.Drawing.Size(101, 46);
            this.btn_EnableD0.TabIndex = 18;
            this.btn_EnableD0.Text = "EnableD0";
            this.btn_EnableD0.UseVisualStyleBackColor = true;
            this.btn_EnableD0.Click += new System.EventHandler(this.btn_EnableD0_Click);
            // 
            // btn_DisableD0
            // 
            this.btn_DisableD0.Location = new System.Drawing.Point(250, 466);
            this.btn_DisableD0.Name = "btn_DisableD0";
            this.btn_DisableD0.Size = new System.Drawing.Size(101, 46);
            this.btn_DisableD0.TabIndex = 19;
            this.btn_DisableD0.Text = "DisableD0";
            this.btn_DisableD0.UseVisualStyleBackColor = true;
            this.btn_DisableD0.Click += new System.EventHandler(this.btn_DisableD0_Click);
            // 
            // btn_StatusD0
            // 
            this.btn_StatusD0.Location = new System.Drawing.Point(357, 466);
            this.btn_StatusD0.Name = "btn_StatusD0";
            this.btn_StatusD0.Size = new System.Drawing.Size(101, 46);
            this.btn_StatusD0.TabIndex = 20;
            this.btn_StatusD0.Text = "StatusD0";
            this.btn_StatusD0.UseVisualStyleBackColor = true;
            this.btn_StatusD0.Click += new System.EventHandler(this.btn_StatusD0_Click);
            // 
            // btn_TestAzure
            // 
            this.btn_TestAzure.Location = new System.Drawing.Point(1126, 10);
            this.btn_TestAzure.Name = "btn_TestAzure";
            this.btn_TestAzure.Size = new System.Drawing.Size(145, 48);
            this.btn_TestAzure.TabIndex = 21;
            this.btn_TestAzure.Text = "TestAzure";
            this.btn_TestAzure.UseVisualStyleBackColor = true;
            this.btn_TestAzure.Click += new System.EventHandler(this.btn_TestAzure_Click);
            // 
            // btn_pushbullet
            // 
            this.btn_pushbullet.Location = new System.Drawing.Point(504, 464);
            this.btn_pushbullet.Name = "btn_pushbullet";
            this.btn_pushbullet.Size = new System.Drawing.Size(145, 48);
            this.btn_pushbullet.TabIndex = 22;
            this.btn_pushbullet.Text = "pushbullet";
            this.btn_pushbullet.UseVisualStyleBackColor = true;
            this.btn_pushbullet.Click += new System.EventHandler(this.btn_pushbullet_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1366, 745);
            this.Controls.Add(this.btn_pushbullet);
            this.Controls.Add(this.btn_TestAzure);
            this.Controls.Add(this.btn_StatusD0);
            this.Controls.Add(this.btn_DisableD0);
            this.Controls.Add(this.btn_EnableD0);
            this.Controls.Add(this.btn_Connect);
            this.Controls.Add(this.btn_tasks);
            this.Controls.Add(this.btn_TestSample);
            this.Controls.Add(this.btn_RemoteCmd);
            this.Controls.Add(this.btn_testcmd);
            this.Controls.Add(this.btn_DataSample);
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
        private System.Windows.Forms.Button btn_DataSample;
        private System.Windows.Forms.Button btn_testcmd;
        private System.Windows.Forms.Button btn_RemoteCmd;
        private System.Windows.Forms.Button btn_TestSample;
        private System.Windows.Forms.Button btn_tasks;
        private System.Windows.Forms.Button btn_Connect;
        private System.Windows.Forms.Button btn_EnableD0;
        private System.Windows.Forms.Button btn_DisableD0;
        private System.Windows.Forms.Button btn_StatusD0;
        private System.Windows.Forms.Button btn_TestAzure;
        private System.Windows.Forms.Button btn_pushbullet;
    }
}

