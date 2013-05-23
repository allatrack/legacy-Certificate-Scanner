namespace CertificateScanner
{
    partial class Crop
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Crop));
            this.SrcPicBox = new System.Windows.Forms.PictureBox();
            this.BtnCrop = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButtonMainRect = new System.Windows.Forms.RadioButton();
            this.radioButtonBar = new System.Windows.Forms.RadioButton();
            this.radioButtonPhoto = new System.Windows.Forms.RadioButton();
            this.radioButtonSignature = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.SrcPicBox)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // SrcPicBox
            // 
            this.SrcPicBox.Cursor = System.Windows.Forms.Cursors.Cross;
            this.SrcPicBox.Location = new System.Drawing.Point(3, 0);
            this.SrcPicBox.Name = "SrcPicBox";
            this.SrcPicBox.Size = new System.Drawing.Size(400, 400);
            this.SrcPicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.SrcPicBox.TabIndex = 0;
            this.SrcPicBox.TabStop = false;
            this.SrcPicBox.Paint += new System.Windows.Forms.PaintEventHandler(this.SrcPicBox_Paint);
            this.SrcPicBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SrcPicBox_MouseDown);
            this.SrcPicBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SrcPicBox_MouseMove);
            this.SrcPicBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SrcPicBox_MouseUp);
            // 
            // BtnCrop
            // 
            this.BtnCrop.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnCrop.Location = new System.Drawing.Point(488, 0);
            this.BtnCrop.Name = "BtnCrop";
            this.BtnCrop.Size = new System.Drawing.Size(90, 24);
            this.BtnCrop.TabIndex = 2;
            this.BtnCrop.Text = "Застосувати";
            this.BtnCrop.UseVisualStyleBackColor = true;
            this.BtnCrop.Click += new System.EventHandler(this.BtnCrop_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(584, 562);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioButtonMainRect);
            this.panel1.Controls.Add(this.radioButtonBar);
            this.panel1.Controls.Add(this.radioButtonPhoto);
            this.panel1.Controls.Add(this.radioButtonSignature);
            this.panel1.Controls.Add(this.BtnCrop);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 535);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(578, 24);
            this.panel1.TabIndex = 3;
            // 
            // radioButtonMainRect
            // 
            this.radioButtonMainRect.AutoSize = true;
            this.radioButtonMainRect.Location = new System.Drawing.Point(323, 4);
            this.radioButtonMainRect.Name = "radioButtonMainRect";
            this.radioButtonMainRect.Size = new System.Drawing.Size(159, 17);
            this.radioButtonMainRect.TabIndex = 6;
            this.radioButtonMainRect.TabStop = true;
            this.radioButtonMainRect.Text = "Змінити осоновну область";
            this.radioButtonMainRect.UseVisualStyleBackColor = true;
            this.radioButtonMainRect.Click += new System.EventHandler(this.radioButtonClick);
            // 
            // radioButtonBar
            // 
            this.radioButtonBar.AutoSize = true;
            this.radioButtonBar.Location = new System.Drawing.Point(123, 4);
            this.radioButtonBar.Name = "radioButtonBar";
            this.radioButtonBar.Size = new System.Drawing.Size(62, 17);
            this.radioButtonBar.TabIndex = 5;
            this.radioButtonBar.TabStop = true;
            this.radioButtonBar.Text = "QR код";
            this.radioButtonBar.UseVisualStyleBackColor = true;
            this.radioButtonBar.Visible = false;
            this.radioButtonBar.Click += new System.EventHandler(this.radioButtonClick);
            // 
            // radioButtonPhoto
            // 
            this.radioButtonPhoto.AutoSize = true;
            this.radioButtonPhoto.Location = new System.Drawing.Point(67, 4);
            this.radioButtonPhoto.Name = "radioButtonPhoto";
            this.radioButtonPhoto.Size = new System.Drawing.Size(53, 17);
            this.radioButtonPhoto.TabIndex = 4;
            this.radioButtonPhoto.TabStop = true;
            this.radioButtonPhoto.Text = "Фото";
            this.radioButtonPhoto.UseVisualStyleBackColor = true;
            this.radioButtonPhoto.Visible = false;
            this.radioButtonPhoto.Click += new System.EventHandler(this.radioButtonClick);
            // 
            // radioButtonSignature
            // 
            this.radioButtonSignature.AutoSize = true;
            this.radioButtonSignature.Location = new System.Drawing.Point(9, 4);
            this.radioButtonSignature.Name = "radioButtonSignature";
            this.radioButtonSignature.Size = new System.Drawing.Size(59, 17);
            this.radioButtonSignature.TabIndex = 3;
            this.radioButtonSignature.TabStop = true;
            this.radioButtonSignature.Text = "Підпис";
            this.radioButtonSignature.UseVisualStyleBackColor = true;
            this.radioButtonSignature.Visible = false;
            this.radioButtonSignature.Click += new System.EventHandler(this.radioButtonClick);
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.SrcPicBox);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(578, 526);
            this.panel2.TabIndex = 4;
            // 
            // Crop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 562);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 600);
            this.Name = "Crop";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Оберіть область";
            this.Load += new System.EventHandler(this.Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SrcPicBox)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox SrcPicBox;
        private System.Windows.Forms.Button BtnCrop;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton radioButtonPhoto;
        private System.Windows.Forms.RadioButton radioButtonSignature;
        private System.Windows.Forms.RadioButton radioButtonBar;
        private System.Windows.Forms.RadioButton radioButtonMainRect;
    }
}

