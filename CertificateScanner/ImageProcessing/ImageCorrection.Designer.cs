namespace CertificateScanner.ImageProcessing
{
    partial class ImageCorrection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageCorrection));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxAfter = new System.Windows.Forms.GroupBox();
            this.pictureBoxAfter = new System.Windows.Forms.PictureBox();
            this.groupBoxBefore = new System.Windows.Forms.GroupBox();
            this.pictureBoxBefore = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.histogram = new CertificateScanner.ImageComputation.HistogramaDesenat();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBoxAfter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAfter)).BeginInit();
            this.groupBoxBefore.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBefore)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupBoxAfter, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBoxBefore, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(987, 449);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBoxAfter
            // 
            this.groupBoxAfter.Controls.Add(this.pictureBoxAfter);
            this.groupBoxAfter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxAfter.Location = new System.Drawing.Point(496, 3);
            this.groupBoxAfter.Name = "groupBoxAfter";
            this.groupBoxAfter.Size = new System.Drawing.Size(488, 243);
            this.groupBoxAfter.TabIndex = 1;
            this.groupBoxAfter.TabStop = false;
            this.groupBoxAfter.Text = "После";
            // 
            // pictureBoxAfter
            // 
            this.pictureBoxAfter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxAfter.Location = new System.Drawing.Point(3, 16);
            this.pictureBoxAfter.Name = "pictureBoxAfter";
            this.pictureBoxAfter.Size = new System.Drawing.Size(482, 224);
            this.pictureBoxAfter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxAfter.TabIndex = 1;
            this.pictureBoxAfter.TabStop = false;
            // 
            // groupBoxBefore
            // 
            this.groupBoxBefore.Controls.Add(this.pictureBoxBefore);
            this.groupBoxBefore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxBefore.Location = new System.Drawing.Point(3, 3);
            this.groupBoxBefore.Name = "groupBoxBefore";
            this.groupBoxBefore.Size = new System.Drawing.Size(487, 243);
            this.groupBoxBefore.TabIndex = 0;
            this.groupBoxBefore.TabStop = false;
            this.groupBoxBefore.Text = "До";
            // 
            // pictureBoxBefore
            // 
            this.pictureBoxBefore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxBefore.Location = new System.Drawing.Point(3, 16);
            this.pictureBoxBefore.Name = "pictureBoxBefore";
            this.pictureBoxBefore.Size = new System.Drawing.Size(481, 224);
            this.pictureBoxBefore.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxBefore.TabIndex = 0;
            this.pictureBoxBefore.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.trackBar1);
            this.groupBox1.Controls.Add(this.histogram);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 252);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(487, 194);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Рівні зображення";
            // 
            // histogram
            // 
            this.histogram.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.histogram.DisplayColor = System.Drawing.Color.Black;
            this.histogram.Dock = System.Windows.Forms.DockStyle.Top;
            this.histogram.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.histogram.Location = new System.Drawing.Point(3, 16);
            this.histogram.Name = "histogram";
            this.histogram.Offset = 20;
            this.histogram.Size = new System.Drawing.Size(481, 133);
            this.histogram.TabIndex = 0;
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(-3, 149);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(104, 45);
            this.trackBar1.TabIndex = 1;
            // 
            // ImageCorrection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(987, 449);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ImageCorrection";
            this.Text = "ImageCorrection";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBoxAfter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAfter)).EndInit();
            this.groupBoxBefore.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBefore)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBoxAfter;
        private System.Windows.Forms.PictureBox pictureBoxAfter;
        private System.Windows.Forms.GroupBox groupBoxBefore;
        private System.Windows.Forms.PictureBox pictureBoxBefore;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TrackBar trackBar1;
        private ImageComputation.HistogramaDesenat histogram;
    }
}