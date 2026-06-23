namespace CRUDMahasiswaADO
{
    partial class FormDashboard
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpTanggalMasuk = new System.Windows.Forms.DateTimePicker();
            this.btnLoadChart = new System.Windows.Forms.Button();
            this.btnResetChart = new System.Windows.Forms.Button();
            this.cmbTipe = new System.Windows.Forms.ComboBox();
            this.chartProdi = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btnDataMahasiswa = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chartProdi)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(224, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(325, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "REKAP DATA MAHASISWA";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(67, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Tahun Masuk :";
            // 
            // dtpTanggalMasuk
            // 
            this.dtpTanggalMasuk.Location = new System.Drawing.Point(166, 82);
            this.dtpTanggalMasuk.Name = "dtpTanggalMasuk";
            this.dtpTanggalMasuk.Size = new System.Drawing.Size(200, 22);
            this.dtpTanggalMasuk.TabIndex = 2;
            // 
            // btnLoadChart
            // 
            this.btnLoadChart.BackColor = System.Drawing.Color.Green;
            this.btnLoadChart.Location = new System.Drawing.Point(386, 76);
            this.btnLoadChart.Name = "btnLoadChart";
            this.btnLoadChart.Size = new System.Drawing.Size(80, 33);
            this.btnLoadChart.TabIndex = 3;
            this.btnLoadChart.Text = "Load";
            this.btnLoadChart.UseVisualStyleBackColor = false;
            this.btnLoadChart.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnResetChart
            // 
            this.btnResetChart.BackColor = System.Drawing.Color.Red;
            this.btnResetChart.Location = new System.Drawing.Point(481, 76);
            this.btnResetChart.Name = "btnResetChart";
            this.btnResetChart.Size = new System.Drawing.Size(84, 33);
            this.btnResetChart.TabIndex = 4;
            this.btnResetChart.Text = "Reset";
            this.btnResetChart.UseVisualStyleBackColor = false;
            this.btnResetChart.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // cmbTipe
            // 
            this.cmbTipe.FormattingEnabled = true;
            this.cmbTipe.Location = new System.Drawing.Point(667, 77);
            this.cmbTipe.Name = "cmbTipe";
            this.cmbTipe.Size = new System.Drawing.Size(121, 24);
            this.cmbTipe.TabIndex = 5;
            this.cmbTipe.Click += new System.EventHandler(this.cmbTipe_SelectedValueChanged);
            // 
            // chartProdi
            // 
            chartArea4.Name = "ChartArea1";
            this.chartProdi.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.chartProdi.Legends.Add(legend4);
            this.chartProdi.Location = new System.Drawing.Point(13, 110);
            this.chartProdi.Name = "chartProdi";
            series4.ChartArea = "ChartArea1";
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            this.chartProdi.Series.Add(series4);
            this.chartProdi.Size = new System.Drawing.Size(775, 291);
            this.chartProdi.TabIndex = 6;
            this.chartProdi.Text = "chart1";
            this.chartProdi.Click += new System.EventHandler(this.chart1_Click);
            // 
            // btnDataMahasiswa
            // 
            this.btnDataMahasiswa.Location = new System.Drawing.Point(644, 407);
            this.btnDataMahasiswa.Name = "btnDataMahasiswa";
            this.btnDataMahasiswa.Size = new System.Drawing.Size(144, 31);
            this.btnDataMahasiswa.TabIndex = 7;
            this.btnDataMahasiswa.Text = "Data Mahasiswa";
            this.btnDataMahasiswa.UseVisualStyleBackColor = true;
            this.btnDataMahasiswa.Click += new System.EventHandler(this.btnDataMahasiswa_Click);
            // 
            // FormDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnDataMahasiswa);
            this.Controls.Add(this.chartProdi);
            this.Controls.Add(this.cmbTipe);
            this.Controls.Add(this.btnResetChart);
            this.Controls.Add(this.btnLoadChart);
            this.Controls.Add(this.dtpTanggalMasuk);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FormDashboard";
            this.Text = "FormDashboard";
            this.Load += new System.EventHandler(this.FormDashboard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartProdi)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpTanggalMasuk;
        private System.Windows.Forms.Button btnLoadChart;
        private System.Windows.Forms.Button btnResetChart;
        private System.Windows.Forms.ComboBox cmbTipe;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartProdi;
        private System.Windows.Forms.Button btnDataMahasiswa;
    }
}