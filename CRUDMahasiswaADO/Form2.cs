using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUDMahasiswaADO
{
    public partial class Form2 : Form
    {
        // 1. Deklarasi object DAL dan Report HARUS di sini (di luar fungsi)
        DAL dbLogic = new DAL();
        ListMahasiswa listMahasiswa = new ListMahasiswa();

        // 2. Deklarasi variabel penampung parameter lemparan
        private string prodi;
        private DateTime tglmasuk;

        // 3. CONSTRUCTOR BARU: Menerima parameter Prodi dan TglMasuk
        public Form2(string Prodi, DateTime TglMasuk)
        {
            InitializeComponent();

            // Masukkan parameter ke variabel lokal
            prodi = Prodi;
            tglmasuk = TglMasuk;

            try
            {
                // Ambil data menggunakan method getDataRekap dari class DAL
                DataTable dtMahasiswa = dbLogic.getDataRekap(prodi, tglmasuk);

                // --- MEMPERTAHANKAN KODINGAN LOGIN DATABASE ANDA YANG AMAN ---
                CrystalDecisions.Shared.ConnectionInfo crConnectionInfo = new CrystalDecisions.Shared.ConnectionInfo();
                crConnectionInfo.ServerName = "LAPTOP-BUHABIQL";
                crConnectionInfo.DatabaseName = "DBAkademikADO";
                crConnectionInfo.IntegratedSecurity = true;

                CrystalDecisions.Shared.TableLogOnInfo crTableLogoninfo = new CrystalDecisions.Shared.TableLogOnInfo();
                foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in listMahasiswa.Database.Tables)
                {
                    crTableLogoninfo = crTable.LogOnInfo;
                    crTableLogoninfo.ConnectionInfo = crConnectionInfo;
                    crTable.ApplyLogOnInfo(crTableLogoninfo);
                }
                // -------------------------------------------------------------

                // Masukkan dtMahasiswa sebagai sumber data report
                listMahasiswa.SetDataSource(dtMahasiswa);

                // --- MEMPERTAHANKAN TRIK SAKTI TOTAL MHS ANDA ---
                int totalMhs = dtMahasiswa.Rows.Count;
                listMahasiswa.DataDefinition.FormulaFields["Total mhs"].Text = "\"" + totalMhs.ToString() + "\"";
                // ------------------------------------------------

                // Tampilkan ke viewer komponen
                crystalReportViewer1.ReportSource = listMahasiswa;
                crystalReportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal load data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // Dibiarkan kosong sesuai instruksi modul
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {
            // Dibiarkan kosong
        }
    }
}