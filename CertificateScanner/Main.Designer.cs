namespace CertificateScanner
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.pictureBoxPhoto = new System.Windows.Forms.PictureBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.buttonScan = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxNumber = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.buttonEditSignature = new System.Windows.Forms.Button();
            this.checkBoxBarNumber = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonBarRect = new System.Windows.Forms.Button();
            this.checkBoxAuto = new System.Windows.Forms.CheckBox();
            this.buttonClearSkannerUUID = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonPhotoRect = new System.Windows.Forms.Button();
            this.buttonSignatureRect = new System.Windows.Forms.Button();
            this.pictureBoxSignature = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPhoto)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSignature)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBoxPhoto
            // 
            this.pictureBoxPhoto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxPhoto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxPhoto.Location = new System.Drawing.Point(0, 23);
            this.pictureBoxPhoto.Name = "pictureBoxPhoto";
            this.pictureBoxPhoto.Size = new System.Drawing.Size(538, 518);
            this.pictureBoxPhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxPhoto.TabIndex = 1;
            this.pictureBoxPhoto.TabStop = false;
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(215, 348);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(24, 20);
            this.buttonBrowse.TabIndex = 4;
            this.buttonBrowse.Text = "...";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // textBoxPath
            // 
            this.textBoxPath.Location = new System.Drawing.Point(5, 348);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.ReadOnly = true;
            this.textBoxPath.Size = new System.Drawing.Size(204, 20);
            this.textBoxPath.TabIndex = 5;
            this.textBoxPath.Text = "C:\\";
            // 
            // buttonScan
            // 
            this.buttonScan.Location = new System.Drawing.Point(86, 374);
            this.buttonScan.Name = "buttonScan";
            this.buttonScan.Size = new System.Drawing.Size(73, 24);
            this.buttonScan.TabIndex = 10;
            this.buttonScan.Text = "Сканувати";
            this.buttonScan.UseVisualStyleBackColor = true;
            this.buttonScan.Click += new System.EventHandler(this.buttonScan_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxNumber, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(794, 572);
            this.tableLayoutPanel1.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(244, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "№ Заявки:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxNumber
            // 
            this.textBoxNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxNumber.Location = new System.Drawing.Point(253, 3);
            this.textBoxNumber.Name = "textBoxNumber";
            this.textBoxNumber.ReadOnly = true;
            this.textBoxNumber.Size = new System.Drawing.Size(538, 20);
            this.textBoxNumber.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.buttonEditSignature);
            this.panel1.Controls.Add(this.checkBoxBarNumber);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.buttonBarRect);
            this.panel1.Controls.Add(this.checkBoxAuto);
            this.panel1.Controls.Add(this.buttonClearSkannerUUID);
            this.panel1.Controls.Add(this.buttonSave);
            this.panel1.Controls.Add(this.buttonPhotoRect);
            this.panel1.Controls.Add(this.buttonSignatureRect);
            this.panel1.Controls.Add(this.pictureBoxSignature);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.textBoxPath);
            this.panel1.Controls.Add(this.buttonScan);
            this.panel1.Controls.Add(this.buttonBrowse);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(244, 541);
            this.panel1.TabIndex = 4;
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(3, 241);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(238, 23);
            this.button3.TabIndex = 24;
            this.button3.Text = "Редагувати фото";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.buttonEditPhoto_Click);
            // 
            // buttonEditSignature
            // 
            this.buttonEditSignature.Enabled = false;
            this.buttonEditSignature.Location = new System.Drawing.Point(3, 214);
            this.buttonEditSignature.Name = "buttonEditSignature";
            this.buttonEditSignature.Size = new System.Drawing.Size(238, 23);
            this.buttonEditSignature.TabIndex = 23;
            this.buttonEditSignature.Text = "Редагувати підпис";
            this.buttonEditSignature.UseVisualStyleBackColor = true;
            this.buttonEditSignature.Click += new System.EventHandler(this.buttonEditSignature_Click);
            // 
            // checkBoxBarNumber
            // 
            this.checkBoxBarNumber.AutoSize = true;
            this.checkBoxBarNumber.Checked = true;
            this.checkBoxBarNumber.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxBarNumber.Location = new System.Drawing.Point(3, 289);
            this.checkBoxBarNumber.Name = "checkBoxBarNumber";
            this.checkBoxBarNumber.Size = new System.Drawing.Size(207, 17);
            this.checkBoxBarNumber.TabIndex = 22;
            this.checkBoxBarNumber.Text = "Автоматичне визначення № заявки";
            this.checkBoxBarNumber.UseVisualStyleBackColor = true;
            this.checkBoxBarNumber.CheckedChanged += new System.EventHandler(this.checkBoxBarNumber_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(135, 455);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 21;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonBarRect
            // 
            this.buttonBarRect.Enabled = false;
            this.buttonBarRect.Location = new System.Drawing.Point(3, 171);
            this.buttonBarRect.Name = "buttonBarRect";
            this.buttonBarRect.Size = new System.Drawing.Size(238, 23);
            this.buttonBarRect.TabIndex = 20;
            this.buttonBarRect.Text = "Неправильно визначений номер";
            this.buttonBarRect.UseVisualStyleBackColor = true;
            this.buttonBarRect.Click += new System.EventHandler(this.buttonBarRect_Click);
            // 
            // checkBoxAuto
            // 
            this.checkBoxAuto.AutoSize = true;
            this.checkBoxAuto.Checked = true;
            this.checkBoxAuto.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAuto.Location = new System.Drawing.Point(3, 312);
            this.checkBoxAuto.Name = "checkBoxAuto";
            this.checkBoxAuto.Size = new System.Drawing.Size(154, 17);
            this.checkBoxAuto.TabIndex = 19;
            this.checkBoxAuto.Text = "Автоматичне сканування";
            this.checkBoxAuto.UseVisualStyleBackColor = true;
            // 
            // buttonClearSkannerUUID
            // 
            this.buttonClearSkannerUUID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonClearSkannerUUID.Location = new System.Drawing.Point(9, 509);
            this.buttonClearSkannerUUID.Name = "buttonClearSkannerUUID";
            this.buttonClearSkannerUUID.Size = new System.Drawing.Size(230, 23);
            this.buttonClearSkannerUUID.TabIndex = 16;
            this.buttonClearSkannerUUID.Text = "Скинути налаштування сканеру";
            this.buttonClearSkannerUUID.UseVisualStyleBackColor = true;
            this.buttonClearSkannerUUID.Click += new System.EventHandler(this.buttonClearSkannerUUID_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Enabled = false;
            this.buttonSave.Location = new System.Drawing.Point(166, 374);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(73, 24);
            this.buttonSave.TabIndex = 15;
            this.buttonSave.Text = "Зберегти";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonPhotoRect
            // 
            this.buttonPhotoRect.Enabled = false;
            this.buttonPhotoRect.Location = new System.Drawing.Point(3, 143);
            this.buttonPhotoRect.Name = "buttonPhotoRect";
            this.buttonPhotoRect.Size = new System.Drawing.Size(238, 23);
            this.buttonPhotoRect.TabIndex = 14;
            this.buttonPhotoRect.Text = "Неправильно вирізане фото";
            this.buttonPhotoRect.UseVisualStyleBackColor = true;
            this.buttonPhotoRect.Click += new System.EventHandler(this.buttonPhotoRect_Click);
            // 
            // buttonSignatureRect
            // 
            this.buttonSignatureRect.Enabled = false;
            this.buttonSignatureRect.Location = new System.Drawing.Point(3, 114);
            this.buttonSignatureRect.Name = "buttonSignatureRect";
            this.buttonSignatureRect.Size = new System.Drawing.Size(238, 23);
            this.buttonSignatureRect.TabIndex = 13;
            this.buttonSignatureRect.Text = "Неправильно вирізаний підпис";
            this.buttonSignatureRect.UseVisualStyleBackColor = true;
            this.buttonSignatureRect.Click += new System.EventHandler(this.buttonSignatureRect_Click);
            // 
            // pictureBoxSignature
            // 
            this.pictureBoxSignature.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxSignature.Location = new System.Drawing.Point(3, 23);
            this.pictureBoxSignature.Name = "pictureBoxSignature";
            this.pictureBoxSignature.Size = new System.Drawing.Size(241, 85);
            this.pictureBoxSignature.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxSignature.TabIndex = 2;
            this.pictureBoxSignature.TabStop = false;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(244, 23);
            this.label4.TabIndex = 12;
            this.label4.Text = "Підпис";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 332);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(177, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Тека для збереження документів";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pictureBoxPhoto);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(253, 28);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(538, 541);
            this.panel2.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(538, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "Фотографія";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 572);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Сканер анкети";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.fMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPhoto)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSignature)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxPhoto;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.Button buttonScan;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxNumber;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBoxSignature;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonClearSkannerUUID;
        private System.Windows.Forms.CheckBox checkBoxAuto;
        private System.Windows.Forms.Button buttonPhotoRect;
        private System.Windows.Forms.Button buttonSignatureRect;
        private System.Windows.Forms.Button buttonBarRect;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBoxBarNumber;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button buttonEditSignature;
    }
}

