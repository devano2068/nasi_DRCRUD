using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUDMahasiswaADO
{
    public partial class FORM_REKAP : Form
    {
        // PERBAIKAN: Menggunakan Windows Authentication murni agar tidak error login 'sa' lagi
        static string connectionString = "Data Source=LAPTOP-BUHABIQL;Initial Catalog=DBAkademikADO;Integrated Security=True";
        SqlConnection conn = new SqlConnection(connectionString);
        SqlDataAdapter da;
        DataTable dtMahasiswa;
        DataTable dtProdi;
        DAL dbLogic = new DAL();

        public FORM_REKAP()
        {
            InitializeComponent();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
        }

        private void FORM_REKAP_Load(object sender, EventArgs e)
        {
            dtpTanggalMasuk.Format = DateTimePickerFormat.Custom;
            dtpTanggalMasuk.CustomFormat = "yyyy";
            dtpTanggalMasuk.ShowUpDown = true;
            dtpTanggalMasuk.MinDate = new DateTime(2000, 1, 1);
            dtpTanggalMasuk.MaxDate = DateTime.Now;

            cmbProdi.DropDownStyle = ComboBoxStyle.DropDownList;
            btnCetak.Enabled = false;

            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                SqlCommand cmd = new SqlCommand("select namaprodi from programstudi", conn);
                cmd.CommandType = CommandType.Text;
                dtProdi = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dtProdi);
                cmbProdi.DataSource = dtProdi;
                cmbProdi.DisplayMember = "namaprodi";
                cmbProdi.ValueMember = "namaprodi";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal Load data prodi: " + ex.Message);
            }
        }

        // Ini Tombol LOAD DATA
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                SqlCommand cmd = new SqlCommand("sp_Report", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@inProdi", SqlDbType.VarChar, 50).Value = cmbProdi.SelectedValue;
                cmd.Parameters.Add("@inTglMsuk", SqlDbType.VarChar, 4).Value = dtpTanggalMasuk.Value.Year.ToString();

                da = new SqlDataAdapter(cmd);
                dtMahasiswa = new DataTable();
                da.Fill(dtMahasiswa);
                dataGridView1.DataSource = dtMahasiswa;

                if (dtMahasiswa.Rows.Count > 0)
                    btnCetak.Enabled = true; // Menyalakan tombol cetak jika data ada
                else
                {
                    btnCetak.Enabled = false;
                    MessageBox.Show("Data tidak ditemukan");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal Load data mahasiswa: " + ex.Message);
            }
        }

        // Ini Tombol CETAK (Membuka Form2)
        private void button2_Click(object sender, EventArgs e)
        {
            if (dtMahasiswa != null && dtMahasiswa.Rows.Count > 0)
            {
                // 1. Ambil nilai Prodi dan Tanggal dari komponen di layar
                string prodiTerpilih = cmbProdi.SelectedValue.ToString();
                DateTime tanggalTerpilih = dtpTanggalMasuk.Value;

                // 2. Lempar nilai tersebut ke Form2 (Sesuai Poin 16)
                Form2 formCetak = new Form2(prodiTerpilih, tanggalTerpilih);
                formCetak.ShowDialog();
            }
            else
            {
                MessageBox.Show("Silakan klik Load data terlebih dahulu!");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void cmbProdi_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}