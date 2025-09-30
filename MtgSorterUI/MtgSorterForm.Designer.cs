namespace MtgSorterUI
{
    partial class MtgSorterForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
         this.richTextBox1 = new System.Windows.Forms.RichTextBox();
         this.archidectButton = new System.Windows.Forms.Button();
         this.listButton = new System.Windows.Forms.Button();
         this.submitButton = new System.Windows.Forms.Button();
         this.clearButton = new System.Windows.Forms.Button();
         this.addButton = new System.Windows.Forms.Button();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.richTextBox2 = new System.Windows.Forms.RichTextBox();
         this.treeView1 = new System.Windows.Forms.TreeView();
         this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
         this.groupBox1.SuspendLayout();
         this.tableLayoutPanel1.SuspendLayout();
         this.SuspendLayout();
         // 
         // richTextBox1
         // 
         this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.richTextBox1.Location = new System.Drawing.Point(203, 3);
         this.richTextBox1.Name = "richTextBox1";
         this.richTextBox1.Size = new System.Drawing.Size(299, 620);
         this.richTextBox1.TabIndex = 0;
         this.richTextBox1.Text = "";
         // 
         // archidectButton
         // 
         this.archidectButton.Enabled = false;
         this.archidectButton.Location = new System.Drawing.Point(113, 63);
         this.archidectButton.Name = "archidectButton";
         this.archidectButton.Size = new System.Drawing.Size(75, 23);
         this.archidectButton.TabIndex = 6;
         this.archidectButton.Text = "archidect";
         this.archidectButton.UseVisualStyleBackColor = true;
         this.archidectButton.Click += new System.EventHandler(this.ConvertButtonCLick);
         // 
         // listButton
         // 
         this.listButton.Enabled = false;
         this.listButton.Location = new System.Drawing.Point(113, 34);
         this.listButton.Name = "listButton";
         this.listButton.Size = new System.Drawing.Size(75, 23);
         this.listButton.TabIndex = 7;
         this.listButton.Text = "Sorted list";
         this.listButton.UseVisualStyleBackColor = true;
         this.listButton.Click += new System.EventHandler(this.SortedListButtonClick);
         // 
         // submitButton
         // 
         this.submitButton.Location = new System.Drawing.Point(6, 34);
         this.submitButton.Name = "submitButton";
         this.submitButton.Size = new System.Drawing.Size(75, 23);
         this.submitButton.TabIndex = 1;
         this.submitButton.Text = "submit";
         this.submitButton.UseVisualStyleBackColor = true;
         this.submitButton.Click += new System.EventHandler(this.SubmitButtonClick);
         // 
         // clearButton
         // 
         this.clearButton.Enabled = false;
         this.clearButton.Location = new System.Drawing.Point(6, 92);
         this.clearButton.Name = "clearButton";
         this.clearButton.Size = new System.Drawing.Size(75, 23);
         this.clearButton.TabIndex = 2;
         this.clearButton.Text = "Clear";
         this.clearButton.UseVisualStyleBackColor = true;
         this.clearButton.Click += new System.EventHandler(this.ClearStoredCards);
         // 
         // addButton
         // 
         this.addButton.Enabled = false;
         this.addButton.Location = new System.Drawing.Point(6, 63);
         this.addButton.Name = "addButton";
         this.addButton.Size = new System.Drawing.Size(75, 23);
         this.addButton.TabIndex = 9;
         this.addButton.Text = "add";
         this.addButton.UseVisualStyleBackColor = true;
         this.addButton.Click += new System.EventHandler(this.AddButtonCLick);
         // 
         // groupBox1
         // 
         this.groupBox1.Controls.Add(this.archidectButton);
         this.groupBox1.Controls.Add(this.submitButton);
         this.groupBox1.Controls.Add(this.listButton);
         this.groupBox1.Controls.Add(this.addButton);
         this.groupBox1.Controls.Add(this.clearButton);
         this.groupBox1.Location = new System.Drawing.Point(3, 3);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(194, 121);
         this.groupBox1.TabIndex = 11;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "groupBox1";
         // 
         // richTextBox2
         // 
         this.richTextBox2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.richTextBox2.Location = new System.Drawing.Point(915, 3);
         this.richTextBox2.Name = "richTextBox2";
         this.richTextBox2.ReadOnly = true;
         this.richTextBox2.Size = new System.Drawing.Size(300, 620);
         this.richTextBox2.TabIndex = 3;
         this.richTextBox2.Text = "";
         // 
         // treeView1
         // 
         this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.treeView1.Location = new System.Drawing.Point(508, 3);
         this.treeView1.Name = "treeView1";
         this.treeView1.Size = new System.Drawing.Size(401, 620);
         this.treeView1.TabIndex = 10;
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 4;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
         this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
         this.tableLayoutPanel1.Controls.Add(this.richTextBox2, 3, 0);
         this.tableLayoutPanel1.Controls.Add(this.treeView1, 2, 0);
         this.tableLayoutPanel1.Controls.Add(this.richTextBox1, 1, 0);
         this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RowCount = 1;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(1218, 626);
         this.tableLayoutPanel1.TabIndex = 13;
         // 
         // Form1
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1218, 626);
         this.Controls.Add(this.tableLayoutPanel1);
         this.Name = "Form1";
         this.Text = "MTG sorter";
         this.groupBox1.ResumeLayout(false);
         this.tableLayoutPanel1.ResumeLayout(false);
         this.ResumeLayout(false);

        }


        #endregion

        private RichTextBox richTextBox1;
        private Button submitButton;
        private Button clearButton;
        private Button archidectButton;
        private Button listButton;
        private Button addButton;
        private GroupBox groupBox1;
        private RichTextBox richTextBox2;
        private TreeView treeView1;
        private TableLayoutPanel tableLayoutPanel1;
    }
}