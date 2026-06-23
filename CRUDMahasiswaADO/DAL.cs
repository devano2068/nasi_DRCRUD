using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUDMahasiswaADO
{
    public class DAL
    {
        static string connectionString = "Data Source=LAPTOP-BUHABIQL;Initial Catalog=DBAkademikADO;Integrated Security=True;";

        SqlConnection conn = new SqlConnection(connectionString);

        SqlDataAdapter da;
        DataTable dtMahasiswa;
        DataTable dtProdi;

        public string GetConnectionString()
        {
            return connectionString;
        }

        public int CountMhs()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd = new SqlCommand("sp_CountMahasiswa", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter outputParam = new SqlParameter("@pCount", SqlDbType.Int);
            outputParam.Direction = ParameterDirection.Output;

            cmd.Parameters.Add(outputParam);

            cmd.ExecuteNonQuery();

            return Convert.ToInt32(outputParam.Value);
        }

        public DataTable GetMhs()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd = new SqlCommand("sp_GetMahasiswa", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            da = new SqlDataAdapter(cmd);

            dtMahasiswa = new DataTable();
            da.Fill(dtMahasiswa);

            return dtMahasiswa;
        }

        public void InsertMhs(string nim, string nama, string alamat, string jeniskelamin, DateTime tanggalLahir, string kodeProdi, byte[] foto)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();

            SqlTransaction trans = conn.BeginTransaction();
            try
            {
                // Pastikan nama SP sesuai dengan yang ada di SQL Server
                SqlCommand command = new SqlCommand("sp_InsertMahasiswa", conn);
                command.Transaction = trans; // Jangan lupa hubungkan ke transaksi
                command.CommandType = CommandType.StoredProcedure;

                // Gunakan nama parameter yang SAMA PERSIS dengan SP Anda
                command.Parameters.AddWithValue("@NIM", nim);
                command.Parameters.AddWithValue("@Nama", nama);
                command.Parameters.AddWithValue("@Alamat", alamat);
                command.Parameters.AddWithValue("@JenisKelamin", jeniskelamin);
                command.Parameters.AddWithValue("@TanggalLahir", tanggalLahir);
                command.Parameters.AddWithValue("@KodeProdi", kodeProdi); // Pastikan nama ini sesuai di SP
                                                                          // Ganti baris parameter foto lama Anda dengan ini:
                if (foto != null && foto.Length > 0)
                {
                    command.Parameters.Add("@Foto", SqlDbType.VarBinary).Value = foto;
                }
                else
                {
                    command.Parameters.Add("@Foto", SqlDbType.VarBinary).Value = DBNull.Value;
                }

                command.ExecuteNonQuery();
                trans.Commit(); // Data akan tersimpan permanen setelah ini
            }
            catch (Exception ex)
            {
                trans.Rollback();
                // Penting: Tampilkan error agar Anda tahu jika ada masalah
                MessageBox.Show("Gagal Insert ke Database: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public void UpdateMhs(string nim, string nama, string alamat, string jeniskelamin, DateTime tanggalLahir, string kodeProdi, byte[] foto)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            // TES: Munculkan isi NIM yang dikirim untuk memastikan datanya tidak kosong/salah
            //System.Windows.Forms.MessageBox.Show("NIM yang mau di-update: '" + nim + "' (" + nim.Length + " karakter)");

            SqlCommand command = new SqlCommand("sp_UpdateMahasiswa", conn);

            // Gunakan .Trim() untuk membuang spasi kosong di ujung teks
            command.Parameters.AddWithValue("@pNIM", nim.Trim());
            command.Parameters.AddWithValue("@pNama", nama);
            command.Parameters.AddWithValue("@pAlamat", alamat);
            command.Parameters.AddWithValue("@pJenisKelamin", jeniskelamin);
            command.Parameters.AddWithValue("@pTanggalLahir", tanggalLahir);
            command.Parameters.AddWithValue("@pKodeProdi", kodeProdi);
            command.Parameters.AddWithValue("@pFoto", (object)foto ?? DBNull.Value);

            command.CommandType = CommandType.StoredProcedure;

            command.ExecuteNonQuery();
        }

        public void DeleteMhs(string nim)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SqlCommand cmd = new SqlCommand("sp_DeleteMahasiswa", conn);
            cmd.Parameters.AddWithValue("@NIM", nim.Trim());
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.ExecuteNonQuery();
        }

        public void resetData()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            string deleteQuery = "DELETE FROM mahasiswa;";
            SqlCommand cmdDelete = new SqlCommand(deleteQuery, conn);
            cmdDelete.ExecuteNonQuery();

            string insertQuery = @"
                INSERT INTO mahasiswa
                SELECT * FROM mahasiswa_backup;";
            SqlCommand cmdInsert = new SqlCommand(insertQuery, conn);
            cmdInsert.ExecuteNonQuery();
        }

        public void testInject(string nim)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            string query = "Update mahasiswa set nama = 'HACKED' where NIM = " + nim;
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
        }

        public DataTable GetMhsByNIM(string nim)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd = new SqlCommand("sp_GetMahasiswaByNIM", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("pNIM", nim);
            da = new SqlDataAdapter(cmd);

            dtMahasiswa = new DataTable();
            da.Fill(dtMahasiswa);

            return dtMahasiswa;
        }

        public void InsertLog(string message)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd = new SqlCommand("sp_LogMessage", conn);

            cmd.Parameters.AddWithValue("psn", message);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
        }

        // --- BERIKUT ADALAH METHOD GRAFIK DASHBOARD YANG SUDAH DISESUAIKAN ---

        public DataTable getAllDataChart()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SqlCommand cmd = new SqlCommand("sp_DashBoard", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            da = new SqlDataAdapter(cmd);
            dtProdi = new DataTable();
            da.Fill(dtProdi);
            return dtProdi;
        }

        public DataTable getDataChartByTahun(string tahun)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();

            // Pastikan query ini menghasilkan NamaProdi dan JumlahMahasiswa
            string query = @"
        SELECT 
            p.NamaProdi, 
            COUNT(m.NIM) AS JumlahMahasiswa 
        FROM Mahasiswa m 
        JOIN ProgramStudi p ON m.KodeProdi = p.KodeProdi 
        WHERE YEAR(m.TanggalLahir) = @tahun 
        GROUP BY p.NamaProdi";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@tahun", tahun);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dtRekap = new DataTable();
            da.Fill(dtRekap);
            return dtRekap;
        }
        // --- METHOD REKAP BARU YANG SUDAH DISESUAIKAN DENGAN SP_REPORT ANDA ---
        public DataTable getDataRekap(string prodi, DateTime tglMasuk)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            // Memanggil Stored Procedure yang sama seperti di FORM_REKAP Anda
            SqlCommand cmd = new SqlCommand("sp_Report", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            // Parameter 'prodi' dan 'tglMasuk' diambil dari parameter di atas (huruf kecil semua)
            cmd.Parameters.Add("@inProdi", SqlDbType.VarChar, 50).Value = prodi;
            cmd.Parameters.Add("@inTglMsuk", SqlDbType.VarChar, 4).Value = tglMasuk.Year.ToString();

            da = new SqlDataAdapter(cmd);
            DataTable dtRekap = new DataTable();
            da.Fill(dtRekap);

            return dtRekap;
        }
    }
}