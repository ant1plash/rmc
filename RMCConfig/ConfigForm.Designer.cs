namespace ConfigTool
{
    partial class ConfigForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigForm));
            this.saveButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Settings = new System.Windows.Forms.GroupBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.infoLabel = new System.Windows.Forms.Label();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.adaptersBox = new System.Windows.Forms.ComboBox();
            this.startButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.passBox = new System.Windows.Forms.TextBox();
            this.portNum = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.copylabel = new System.Windows.Forms.Label();
            this.adapterTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.Settings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.portNum)).BeginInit();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.saveButton.Location = new System.Drawing.Point(169, 149);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(66, 27);
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(7, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "IP Address ";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Location = new System.Drawing.Point(257, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(278, 278);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // Settings
            // 
            this.Settings.Controls.Add(this.pictureBox2);
            this.Settings.Controls.Add(this.infoLabel);
            this.Settings.Controls.Add(this.linkLabel3);
            this.Settings.Controls.Add(this.adaptersBox);
            this.Settings.Controls.Add(this.startButton);
            this.Settings.Controls.Add(this.label3);
            this.Settings.Controls.Add(this.saveButton);
            this.Settings.Controls.Add(this.passBox);
            this.Settings.Controls.Add(this.label1);
            this.Settings.Controls.Add(this.portNum);
            this.Settings.Controls.Add(this.label2);
            this.Settings.Location = new System.Drawing.Point(12, 12);
            this.Settings.Name = "Settings";
            this.Settings.Size = new System.Drawing.Size(239, 251);
            this.Settings.TabIndex = 10;
            this.Settings.TabStop = false;
            this.Settings.Text = "Settings";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::RMCConfig.Properties.Resources.network;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Location = new System.Drawing.Point(9, 89);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(23, 26);
            this.pictureBox2.TabIndex = 16;
            this.pictureBox2.TabStop = false;
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.ForeColor = System.Drawing.Color.Red;
            this.infoLabel.Location = new System.Drawing.Point(9, 227);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(91, 13);
            this.infoLabel.TabIndex = 11;
            this.infoLabel.Text = "Server not started";
            // 
            // linkLabel3
            // 
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.linkLabel3.Location = new System.Drawing.Point(116, 201);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(117, 16);
            this.linkLabel3.TabIndex = 15;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "Help and F.A.Q.";
            this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_LinkClicked);
            // 
            // adaptersBox
            // 
            this.adaptersBox.BackColor = System.Drawing.SystemColors.Window;
            this.adaptersBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.adaptersBox.DropDownWidth = 200;
            this.adaptersBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.adaptersBox.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.adaptersBox.FormattingEnabled = true;
            this.adaptersBox.ItemHeight = 18;
            this.adaptersBox.Location = new System.Drawing.Point(38, 89);
            this.adaptersBox.Name = "adaptersBox";
            this.adaptersBox.Size = new System.Drawing.Size(195, 26);
            this.adaptersBox.TabIndex = 13;
            this.adaptersBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.adaptersBox_DrawItem);
            this.adaptersBox.DropDown += new System.EventHandler(this.adaptersBox_DropDown);
            this.adaptersBox.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.adaptersBox_MeasureItem);
            this.adaptersBox.SelectedIndexChanged += new System.EventHandler(this.adaptersBox_SelectedIndexChanged);
            this.adaptersBox.DropDownClosed += new System.EventHandler(this.adaptersBox_DropDownClosed);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(9, 195);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(89, 29);
            this.startButton.TabIndex = 12;
            this.startButton.Text = "Start Server";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 16);
            this.label3.TabIndex = 9;
            this.label3.Text = "Password";
            // 
            // passBox
            // 
            this.passBox.Location = new System.Drawing.Point(9, 35);
            this.passBox.Name = "passBox";
            this.passBox.Size = new System.Drawing.Size(224, 20);
            this.passBox.TabIndex = 8;
            this.passBox.TextChanged += new System.EventHandler(this.passBox_TextChanged);
            // 
            // portNum
            // 
            this.portNum.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.portNum.Location = new System.Drawing.Point(9, 149);
            this.portNum.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.portNum.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.portNum.Name = "portNum";
            this.portNum.Size = new System.Drawing.Size(154, 27);
            this.portNum.TabIndex = 4;
            this.portNum.Value = new decimal(new int[] {
            9050,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(9, 130);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Server Port";
            // 
            // copylabel
            // 
            this.copylabel.AutoSize = true;
            this.copylabel.Location = new System.Drawing.Point(12, 269);
            this.copylabel.Name = "copylabel";
            this.copylabel.Size = new System.Drawing.Size(104, 13);
            this.copylabel.TabIndex = 16;
            this.copylabel.Text = "© ant1plash.tk 2016";
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 287);
            this.Controls.Add(this.copylabel);
            this.Controls.Add(this.Settings);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ConfigForm";
            this.Text = "Remote Media Control Configuration";
            this.Load += new System.EventHandler(this.ConfigForm_Load);
            this.Shown += new System.EventHandler(this.ConfigForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.Settings.ResumeLayout(false);
            this.Settings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.portNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox Settings;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox passBox;
        private System.Windows.Forms.NumericUpDown portNum;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.ComboBox adaptersBox;
        private System.Windows.Forms.LinkLabel linkLabel3;
        private System.Windows.Forms.ToolTip adapterTip;
        private System.Windows.Forms.Label copylabel;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}

