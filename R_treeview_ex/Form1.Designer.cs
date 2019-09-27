namespace R_treeview_ex
{
    partial class R_Treeview
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
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.lblDataPathR = new System.Windows.Forms.Label();
            this.btnChooseFileR = new System.Windows.Forms.Button();
            this.lblStatTree = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabCode = new System.Windows.Forms.TabPage();
            this.tbCode = new System.Windows.Forms.TextBox();
            this.tabLog = new System.Windows.Forms.TabPage();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.tabData = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lblDataPathNet = new System.Windows.Forms.Label();
            this.btnChooseFileNet = new System.Windows.Forms.Button();
            this.btnCloseTab = new System.Windows.Forms.Button();
            this.btnCreateR = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabCode.SuspendLayout();
            this.tabLog.SuspendLayout();
            this.tabData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.treeView1.Location = new System.Drawing.Point(9, 96);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(144, 444);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // lblDataPathR
            // 
            this.lblDataPathR.AutoSize = true;
            this.lblDataPathR.Location = new System.Drawing.Point(6, 48);
            this.lblDataPathR.Name = "lblDataPathR";
            this.lblDataPathR.Size = new System.Drawing.Size(126, 13);
            this.lblDataPathR.TabIndex = 2;
            this.lblDataPathR.Text = "Choose data set using R:";
            // 
            // btnChooseFileR
            // 
            this.btnChooseFileR.Location = new System.Drawing.Point(177, 43);
            this.btnChooseFileR.Name = "btnChooseFileR";
            this.btnChooseFileR.Size = new System.Drawing.Size(75, 23);
            this.btnChooseFileR.TabIndex = 3;
            this.btnChooseFileR.Text = "Using R";
            this.btnChooseFileR.UseVisualStyleBackColor = true;
            this.btnChooseFileR.Click += new System.EventHandler(this.btnChooseFileR_Click);
            // 
            // lblStatTree
            // 
            this.lblStatTree.AutoSize = true;
            this.lblStatTree.Location = new System.Drawing.Point(6, 80);
            this.lblStatTree.Name = "lblStatTree";
            this.lblStatTree.Size = new System.Drawing.Size(49, 13);
            this.lblStatTree.TabIndex = 5;
            this.lblStatTree.Text = "Statistics";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(618, 515);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(96, 25);
            this.btnClear.TabIndex = 6;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(516, 515);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(96, 25);
            this.btnSubmit.TabIndex = 7;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabCode);
            this.tabControl1.Controls.Add(this.tabLog);
            this.tabControl1.Controls.Add(this.tabData);
            this.tabControl1.Location = new System.Drawing.Point(177, 96);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(537, 413);
            this.tabControl1.TabIndex = 8;
            // 
            // tabCode
            // 
            this.tabCode.Controls.Add(this.tbCode);
            this.tabCode.Location = new System.Drawing.Point(4, 22);
            this.tabCode.Name = "tabCode";
            this.tabCode.Padding = new System.Windows.Forms.Padding(3);
            this.tabCode.Size = new System.Drawing.Size(529, 387);
            this.tabCode.TabIndex = 0;
            this.tabCode.Text = "Code";
            this.tabCode.UseVisualStyleBackColor = true;
            // 
            // tbCode
            // 
            this.tbCode.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbCode.Location = new System.Drawing.Point(0, 0);
            this.tbCode.Multiline = true;
            this.tbCode.Name = "tbCode";
            this.tbCode.Size = new System.Drawing.Size(529, 387);
            this.tbCode.TabIndex = 0;
            // 
            // tabLog
            // 
            this.tabLog.Controls.Add(this.tbLog);
            this.tabLog.Location = new System.Drawing.Point(4, 22);
            this.tabLog.Name = "tabLog";
            this.tabLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabLog.Size = new System.Drawing.Size(529, 387);
            this.tabLog.TabIndex = 1;
            this.tabLog.Text = "Log";
            this.tabLog.UseVisualStyleBackColor = true;
            // 
            // tbLog
            // 
            this.tbLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbLog.Location = new System.Drawing.Point(0, 0);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.Size = new System.Drawing.Size(529, 387);
            this.tbLog.TabIndex = 0;
            // 
            // tabData
            // 
            this.tabData.Controls.Add(this.dataGridView1);
            this.tabData.Location = new System.Drawing.Point(4, 22);
            this.tabData.Name = "tabData";
            this.tabData.Size = new System.Drawing.Size(529, 387);
            this.tabData.TabIndex = 2;
            this.tabData.Text = "Data";
            this.tabData.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(4, 4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(520, 378);
            this.dataGridView1.TabIndex = 2;
            // 
            // lblDataPathNet
            // 
            this.lblDataPathNet.AutoSize = true;
            this.lblDataPathNet.Location = new System.Drawing.Point(6, 19);
            this.lblDataPathNet.Name = "lblDataPathNet";
            this.lblDataPathNet.Size = new System.Drawing.Size(138, 13);
            this.lblDataPathNet.TabIndex = 9;
            this.lblDataPathNet.Text = "Choose data set using .Net:";
            // 
            // btnChooseFileNet
            // 
            this.btnChooseFileNet.Location = new System.Drawing.Point(177, 9);
            this.btnChooseFileNet.Name = "btnChooseFileNet";
            this.btnChooseFileNet.Size = new System.Drawing.Size(75, 23);
            this.btnChooseFileNet.TabIndex = 0;
            this.btnChooseFileNet.Text = "Using .Net";
            this.btnChooseFileNet.Click += new System.EventHandler(this.btnChooseFileNet_Click);
            // 
            // btnCloseTab
            // 
            this.btnCloseTab.Location = new System.Drawing.Point(414, 515);
            this.btnCloseTab.Name = "btnCloseTab";
            this.btnCloseTab.Size = new System.Drawing.Size(96, 25);
            this.btnCloseTab.TabIndex = 10;
            this.btnCloseTab.Text = "Close Tab";
            this.btnCloseTab.UseVisualStyleBackColor = true;
            this.btnCloseTab.Click += new System.EventHandler(this.btnCloseTab_Click);
            // 
            // btnCreateR
            // 
            this.btnCreateR.Location = new System.Drawing.Point(177, 515);
            this.btnCreateR.Name = "btnCreateR";
            this.btnCreateR.Size = new System.Drawing.Size(96, 25);
            this.btnCreateR.TabIndex = 11;
            this.btnCreateR.Text = "Create R";
            this.btnCreateR.UseVisualStyleBackColor = true;
            this.btnCreateR.Click += new System.EventHandler(this.btnCreateR_Click);
            // 
            // R_Treeview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 546);
            this.Controls.Add(this.btnCreateR);
            this.Controls.Add(this.btnCloseTab);
            this.Controls.Add(this.btnChooseFileNet);
            this.Controls.Add(this.lblDataPathNet);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.lblStatTree);
            this.Controls.Add(this.btnChooseFileR);
            this.Controls.Add(this.lblDataPathR);
            this.Controls.Add(this.treeView1);
            this.Name = "R_Treeview";
            this.Text = "R_Treeview";
            this.Load += new System.EventHandler(this.R_Treeview_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabCode.ResumeLayout(false);
            this.tabCode.PerformLayout();
            this.tabLog.ResumeLayout(false);
            this.tabLog.PerformLayout();
            this.tabData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void TreeView1_NodeMouseClick(object sender, System.Windows.Forms.TreeNodeMouseClickEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void TreeView1_SelectItemChanged(object sender, System.Windows.Forms.TreeNodeMouseClickEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        #endregion
        private System.Windows.Forms.Label lblDataPathR;
        private System.Windows.Forms.Button btnChooseFileR;
        private System.Windows.Forms.Label lblStatTree;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabCode;
        private System.Windows.Forms.TabPage tabLog;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.TextBox tbCode;
        private System.Windows.Forms.TabPage tabData;
        private System.Windows.Forms.Label lblDataPathNet;
        private System.Windows.Forms.Button btnChooseFileNet;
        private System.Windows.Forms.DataGridView dataGridView1;
        public System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button btnCloseTab;
        private System.Windows.Forms.Button btnCreateR;
    }
}

