using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.Drawing.Text;

namespace CRUDMahasiswaADO
{
    public partial class FormMahasiswa : Form
    {
        private readonly string connectionString =
            "Data Source=LAPTOP-BUHABIQL;Initial Catalog=DBAkademikADO;Integrated Security=True";
        DAL dbLogic = new DAL();

        private void SimpanLog(string pesan)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO LogError VALUES(GETDATE(), @pesan)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@pesan", pesan);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private BindingSource bindingSource = new BindingSource();
        private DataTable dtMahasiswa = new DataTable();

        public FormMahasiswa()
        {
            InitializeComponent();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    MessageBox.Show("Koneksi berhasil");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Koneksi gagal: " + ex.Message);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }


        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (txtNIM.Text == "")
            {
                MessageBox.Show("NIM harus diisi");
                txtNIM.Focus();
                return;
            }

            try
            {
                // PERBAIKAN: Menggunakan pengambilan 'as byte[]' yang lebih aman dari null
                byte[] foto = fotoMhs.Tag as byte[] ?? null;

                dbLogic.InsertMhs(
                    txtNIM.Text,
                    txtNama.Text,
                    txtAlamat.Text,
                    cmbJK.Text,
                    dtpTanggalLahir.Value,
                    txtKodeProdi.Text,
                    foto
                );

                MessageBox.Show("Data berhasil ditambahkan");
                ClearForm();
                LoadData();
            }
            catch (Exception ex)
            {
                // PERBAIKAN: Disamakan menggunakan SimpanLog lokal agar tidak crash
                SimpanLog("Insert Gagal: " + ex.Message);
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // VALIDASI INPUT (Bagian dari Poin 13)
            if (string.IsNullOrEmpty(txtNIM.Text))
            {
                MessageBox.Show("Pilih data mahasiswa terlebih dahulu dari tabel!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(txtNama.Text))
            {
                MessageBox.Show("Nama mahasiswa tidak boleh kosong!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNama.Focus();
                return;
            }

            try
            {
                byte[] foto = fotoMhs.Tag as byte[] ?? new byte[0];

                dbLogic.UpdateMhs(
                    txtNIM.Text,
                    txtNama.Text,
                    txtAlamat.Text,
                    cmbJK.Text,
                    dtpTanggalLahir.Value.Date,
                    txtKodeProdi.Text,
                    foto
                );

                MessageBox.Show("Data berhasil diupdate");
                ClearForm();
                LoadData();
            }
            catch (Exception ex)
            {
                // Ganti pemanggilan dbLogic.InsertLog dengan SimpanLog lokal yang ada di Form Anda
                // agar tidak crash mencari sp_LogMessage ke DAL
                SimpanLog("Update Gagal: " + ex.Message);

                MessageBox.Show("Penyebab Update Gagal Sebenarnya: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNIM.Text))
            {
                MessageBox.Show("Pilih data mahasiswa yang ingin dihapus dari tabel!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Beri konfirmasi sebelum menghapus demi keamanan data
            if (MessageBox.Show("Apakah Anda yakin ingin menghapus mahasiswa dengan NIM " + txtNIM.Text + "?", "Konfirmasi", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                try
                {
                    dbLogic.DeleteMhs(txtNIM.Text);

                    MessageBox.Show("Data berhasil dihapus");
                    ClearForm();
                    LoadData();
                }
                catch (Exception ex)
                {
                    // Menggunakan fungsi SimpanLog lokal yang mengarah ke tabel LogError Anda
                    SimpanLog("Delete Gagal: " + ex.Message);
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ClearForm()
        {
            txtNIM.DataBindings.Clear();
            txtNama.DataBindings.Clear();
            cmbJK.DataBindings.Clear();
            dtpTanggalLahir.DataBindings.Clear();
            txtAlamat.DataBindings.Clear();
            txtKodeProdi.DataBindings.Clear();

            txtNIM.Clear();
            txtNama.Clear();
            cmbJK.SelectedIndex = -1;
            txtAlamat.Clear();
            txtKodeProdi.Clear();
            dtpTanggalLahir.Value = DateTime.Now;
            txtNIM.Focus();
        }

        private void FormMahasiswa_Load(object sender, EventArgs e)
        {
            // ComboBox JK manual
            cmbJK.DataSource = new string[] { "L", "P" };

            // Setting Grid
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // BindingNavigator
            bindingNavigator1.BindingSource = bindingSource;

            LoadData();
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                txtNIM.Text = row.Cells[0].Value.ToString().Trim();
                txtNama.Text = row.Cells[1].Value.ToString().Trim();
                cmbJK.Text = row.Cells[2].Value.ToString().Trim();

                if (row.Cells[3].Value != DBNull.Value)
                    dtpTanggalLahir.Value = Convert.ToDateTime(row.Cells[3].Value);

                txtAlamat.Text = row.Cells[4].Value.ToString().Trim();

                // JIKA SEBELUMNYA INDEX 5 MUNCUL SYSTEM.BYTE (FOTO),
                // MAKA KODE PRODI PASTI ADA DI INDEX 6 ATAU SEBALIKNYA.
                // Coba ganti ke angka 6 untuk menggeser pencarian ke kolom Kode Prodi yang benar:
                txtKodeProdi.Text = row.Cells[6].Value.ToString().Trim();
            }
        }
        private void LoadData()
        {
            txtNIM.DataBindings.Clear();
            txtNama.DataBindings.Clear();
            cmbJK.DataBindings.Clear();
            dtpTanggalLahir.DataBindings.Clear();
            txtAlamat.DataBindings.Clear();
            txtKodeProdi.DataBindings.Clear();

            bindingSource.DataSource = null;
            bindingSource.Clear();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetMahasiswa", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dtMahasiswa = new DataTable();
                        da.Fill(dtMahasiswa);
                        bindingSource.DataSource = dtMahasiswa;
                        dataGridView1.DataSource = bindingSource;
                        // TIDAK memanggil BindControls() sama sekali
                    }
                }
                HitungTotal();
            }
        }
        private void BindControls()
        {

        }
        private void btnResetData_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
            IF OBJECT_ID('dbo.Mahasiswa_Backup') IS NOT NULL
            BEGIN
                DELETE FROM dbo.Mahasiswa;
                INSERT INTO dbo.Mahasiswa
                SELECT * FROM dbo.Mahasiswa_Backup;
            END";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Data berhasil direset");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Reset gagal: " + ex.Message);
            }
        }

        private void FormMahasiswa_Load_1(object sender, EventArgs e)
        {

        }
        private void btnTestInjection_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    string query = "UPDATE Mahasiswa SET Nama=@Nama WHERE NIM=@NIM";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nama", txtNama.Text);
                        cmd.Parameters.AddWithValue("@NIM", txtNIM.Text);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Update berhasil");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void HitungTotal()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_CountMahasiswa", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlParameter outputParam = new SqlParameter("@Total", SqlDbType.Int);
                        outputParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(outputParam);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        lblTotal.Text = "Total Mahasiswa: " + outputParam.Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menghitung total: " + ex.Message);
            }
        }

        private void lblTotal_Click(object sender, EventArgs e)
        {

        }

        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FORM_REKAP frmDashboard = new FORM_REKAP();
            frmDashboard.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormDashboard frmGrafik = new FormDashboard();
            frmGrafik.Show();
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                // Membatasi format file hanya untuk gambar
                ofd.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.png; *.bmp)|*.jpg; *.jpeg; *.gif; *.png; *.bmp";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // 1. Tampilkan gambar ke PictureBox
                    fotoMhs.Image = new Bitmap(ofd.FileName);
                    fotoMhs.SizeMode = PictureBoxSizeMode.StretchImage; // Biar gambar pas dengan ukuran box

                    // 2. Konversi gambar menjadi byte[] untuk disimpan ke database
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                    {
                        fotoMhs.Image.Save(ms, fotoMhs.Image.RawFormat);
                        byte[] fotoBytes = ms.ToArray();

                        // Simpan byte[] ke properti Tag PictureBox agar bisa dipanggil oleh btnInsert/btnUpdate
                        fotoMhs.Tag = fotoBytes;
                    }
                }
            }
        }

        private void btnCariNIM_Click(object sender, EventArgs e)
        {
            try
            {
                // Cek apakah data di bindingSource masih kosong atau belum di-Load
                if (bindingSource.DataSource == null || ((DataTable)bindingSource.DataSource).Rows.Count == 0)
                {
                    // Jika kosong, otomatis panggil LoadData() terlebih dahulu agar data muncul
                    LoadData();
                }

                // Ambil NIM dari textbox utama Anda
                string keywordNIM = txtNIM.Text.Trim().Replace("'", "''");

                if (string.IsNullOrEmpty(keywordNIM))
                {
                    bindingSource.Filter = "";
                }
                else
                {
                    // Melakukan penyaringan data secara instan
                    bindingSource.Filter = string.Format("NIM = '{0}'", keywordNIM);
                }

                HitungTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal melakukan pencarian NIM: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel Files (*.xlsx;*.xls)|*.xlsx;*.xls";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook workbook = null;
                Microsoft.Office.Interop.Excel.Worksheet worksheet = null;
                Microsoft.Office.Interop.Excel.Range range = null;

                try
                {
                    workbook = excelApp.Workbooks.Open(ofd.FileName);
                    worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                    range = worksheet.UsedRange;

                    DataTable dtExcel = new DataTable();

                    // 1. Buat Header Kolom di Tabel Layar
                    for (int col = 1; col <= range.Columns.Count; col++)
                    {
                        string columnName = (range.Cells[1, col] as Microsoft.Office.Interop.Excel.Range).Value2?.ToString();
                        dtExcel.Columns.Add(columnName ?? $"Kolom_{col}");
                    }

                    // 2. Baca isi data Excel ke dalam DataTable internal
                    for (int row = 2; row <= range.Rows.Count; row++)
                    {
                        DataRow dr = dtExcel.NewRow();
                        for (int col = 1; col <= range.Columns.Count; col++)
                        {
                            dr[col - 1] = (range.Cells[row, col] as Microsoft.Office.Interop.Excel.Range).Value2?.ToString();
                        }
                        dtExcel.Rows.Add(dr);
                    }

                    // 3. Tampilkan ke DataGridView di layar Anda
                    dataGridView1.DataSource = dtExcel;

                    // =========================================================================
                    // PROSES BARU: Looping untuk menyimpan data Excel ke SQL Server Database
                    // =========================================================================
                    int jumlahSukses = 0;
                    int jumlahGagal = 0;

                    foreach (DataRow row in dtExcel.Rows)
                    {
                        try
                        {
                            // Mengambil data berdasarkan urutan kolom di file Excel Anda
                            string nim = row[0].ToString().Trim();         // Kolom 1: NIM
                            string nama = row[1].ToString();               // Kolom 2: Nama
                            string alamat = row[2].ToString();             // Kolom 3: Alamat
                            string jk = row[3].ToString();                 // Kolom 4: Jenis Kelamin (L/P)

                            // Konversi text tanggal lahir dari Excel ke tipe DateTime C#
                            DateTime tglLahir;
                            if (!DateTime.TryParse(row[4].ToString(), out tglLahir))
                            {
                                tglLahir = new DateTime(2000, 1, 1); // Default jika format tanggal di excel aneh
                            }

                            string kodeProdi = row[5].ToString().Trim();   // Kolom 6: Kode Prodi
                            byte[] foto = null;                            // Foto di-null kan dulu karena excel tidak menyimpan gambar blob

                            // Panggil fungsi Insert asli dari file DAL.cs Anda
                            dbLogic.InsertMhs(nim, nama, alamat, jk, tglLahir, kodeProdi, foto);
                            jumlahSukses++;

                            MessageBox.Show($"Import Selesai!\n\nBerhasil disimpan: {jumlahSukses} Mahasiswa.\nGagal/Skip: {jumlahGagal} data (Mungkin NIM sudah terdaftar).",
                            "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // PERBAIKAN: Panggil fungsi ini agar DataGridView & Label Total langsung ter-refresh otomatis
                            LoadData();
                        }
                        catch (Exception)
                        {
                            // Jika ada 1 baris yang error (misal NIM sudah ada di DB), skip dan lanjut ke baris berikutnya
                            jumlahGagal++;
                            continue;
                        }
                    }

                    MessageBox.Show($"Import Selesai!\n\nBerhasil disimpan: {jumlahSukses} Mahasiswa.\nGagal/Skip: {jumlahGagal} data (Mungkin NIM sudah terdaftar).",
                                    "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal melakukan import resmi: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (workbook != null) workbook.Close(false);
                    excelApp.Quit();

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(range);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                }
            }
        }

        private void txtKodeProdi_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnImportToDB_Click(object sender, EventArgs e)
        {
            int jumlahSukses = 0;
            int jumlahGagal = 0;
            string nim = ""; // Deklarasi di luar agar bisa diakses di catch

            // Looping data mahasiswa dari baris pertama sampai akhir di DataGridView
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                // Mencegah error jika ada baris kosong di akhir tabel DataGridView
                if (dataGridView1.Rows[i].Cells[0].Value == null || string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[0].Value.ToString()))
                {
                    continue;
                }

                try
                {
                    // Ambil data sesuai urutan kolom dari DataGridView
                    nim = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    string nama = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    string alamat = dataGridView1.Rows[i].Cells[2].Value.ToString();
                    string jk = dataGridView1.Rows[i].Cells[3].Value.ToString();

                    // Konversi format tanggal sesuai tipe DateTime C#
                    DateTime tglLahir = Convert.ToDateTime(dataGridView1.Rows[i].Cells[4].Value);
                    string kodeProdi = dataGridView1.Rows[i].Cells[5].Value.ToString();

                    // Variabel foto dikosongkan karena data excel tidak memiliki foto
                    byte[] fotokosong = null;

                    // Panggil method InsertMhs dengan 7 parameter
                    dbLogic.InsertMhs(nim, nama, alamat, jk, tglLahir, kodeProdi, fotokosong);
                    jumlahSukses++;
                }
                catch (Exception ex)
                {
                    jumlahGagal++;
                    // Log error ke output debug agar tidak menghentikan proses looping
                    System.Diagnostics.Debug.WriteLine("Gagal import NIM " + nim + ": " + ex.Message);
                }
            }

            // Menampilkan hasil akhir proses import
            MessageBox.Show($"Import Selesai!\n\nBerhasil disimpan: {jumlahSukses} Mahasiswa.\nGagal/Skip: {jumlahGagal} data (Mungkin NIM sudah terdaftar).",
                            "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Memperbarui isi DataGridView utama agar data barunya langsung muncul
            LoadData();
        }

        private void fotoMhs_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}


