namespace _5Client
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.다시시작ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.복기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.끝내기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.보기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.그리기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.이미지ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.txtHistory = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTTS = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dToolStripMenuItem,
            this.보기ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1004, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // dToolStripMenuItem
            // 
            this.dToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.다시시작ToolStripMenuItem,
            this.복기ToolStripMenuItem,
            this.끝내기ToolStripMenuItem});
            this.dToolStripMenuItem.Name = "dToolStripMenuItem";
            this.dToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.dToolStripMenuItem.Text = "파일";
            // 
            // 다시시작ToolStripMenuItem
            // 
            this.다시시작ToolStripMenuItem.Name = "다시시작ToolStripMenuItem";
            this.다시시작ToolStripMenuItem.Size = new System.Drawing.Size(149, 26);
            this.다시시작ToolStripMenuItem.Text = "다시 시작";
            // 
            // 복기ToolStripMenuItem
            // 
            this.복기ToolStripMenuItem.Name = "복기ToolStripMenuItem";
            this.복기ToolStripMenuItem.Size = new System.Drawing.Size(149, 26);
            this.복기ToolStripMenuItem.Text = "복기";
            this.복기ToolStripMenuItem.Click += new System.EventHandler(this.복기ToolStripMenuItem_Click);
            // 
            // 끝내기ToolStripMenuItem
            // 
            this.끝내기ToolStripMenuItem.Name = "끝내기ToolStripMenuItem";
            this.끝내기ToolStripMenuItem.Size = new System.Drawing.Size(149, 26);
            this.끝내기ToolStripMenuItem.Text = "끝내기";
            this.끝내기ToolStripMenuItem.Click += new System.EventHandler(this.끝내기ToolStripMenuItem_Click);
            // 
            // 보기ToolStripMenuItem
            // 
            this.보기ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.그리기ToolStripMenuItem,
            this.이미지ToolStripMenuItem});
            this.보기ToolStripMenuItem.Name = "보기ToolStripMenuItem";
            this.보기ToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.보기ToolStripMenuItem.Text = "보기";
            // 
            // 그리기ToolStripMenuItem
            // 
            this.그리기ToolStripMenuItem.Name = "그리기ToolStripMenuItem";
            this.그리기ToolStripMenuItem.Size = new System.Drawing.Size(129, 26);
            this.그리기ToolStripMenuItem.Text = "그리기";
            this.그리기ToolStripMenuItem.Click += new System.EventHandler(this.그리기ToolStripMenuItem_Click);
            // 
            // 이미지ToolStripMenuItem
            // 
            this.이미지ToolStripMenuItem.Name = "이미지ToolStripMenuItem";
            this.이미지ToolStripMenuItem.Size = new System.Drawing.Size(129, 26);
            this.이미지ToolStripMenuItem.Text = "이미지";
            this.이미지ToolStripMenuItem.Click += new System.EventHandler(this.이미지ToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DarkOrange;
            this.panel1.Location = new System.Drawing.Point(12, 45);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(589, 581);
            this.panel1.TabIndex = 1;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(607, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "상대방";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(665, 56);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(170, 25);
            this.txtAddress.TabIndex = 3;
            this.txtAddress.Text = "127.0.0.1";
            // 
            // txtHistory
            // 
            this.txtHistory.BackColor = System.Drawing.SystemColors.HighlightText;
            this.txtHistory.Location = new System.Drawing.Point(619, 128);
            this.txtHistory.Multiline = true;
            this.txtHistory.Name = "txtHistory";
            this.txtHistory.ReadOnly = true;
            this.txtHistory.Size = new System.Drawing.Size(385, 461);
            this.txtHistory.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(619, 598);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "메세지";
            // 
            // txtTTS
            // 
            this.txtTTS.Location = new System.Drawing.Point(685, 595);
            this.txtTTS.Name = "txtTTS";
            this.txtTTS.Size = new System.Drawing.Size(239, 25);
            this.txtTTS.TabIndex = 6;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(930, 597);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(62, 23);
            this.btnSend.TabIndex = 7;
            this.btnSend.Text = "전송";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.OnSendData);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(945, 56);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(47, 25);
            this.btnStart.TabIndex = 8;
            this.btnStart.Text = "시작";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.BeginStartServer);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(841, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 15);
            this.label3.TabIndex = 9;
            this.label3.Text = "ID";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(867, 56);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(72, 25);
            this.txtPort.TabIndex = 10;
            this.txtPort.Text = "15000";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1004, 626);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtTTS);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtHistory);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 다시시작ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 끝내기ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 보기ToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem 그리기ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 이미지ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 복기ToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.TextBox txtHistory;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTTS;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPort;
    }
}

