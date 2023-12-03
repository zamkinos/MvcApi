using MvcApi.Nesneler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MvcApi.Islemler
{
    public class VeritabaniIslemleri : IDisposable
    {
        public SqlCommand SqlCommand { get; set; }
        public SqlConnection Baglanti { get; set; }

        public VeritabaniIslemleri()
        {
            this.Baglanti = new SqlConnection(SrvSabitler.VERITABANI_BAGLANTI_CUMLESI);
            baglantiKontrol();
        }

        public DataTable ReturnDataTable(string spIsim, List<SqlParameter> spParametreleri)
        {
            SqlDataAdapter _sqlDataAdapter;
            try
            {
                baglantiKontrol(); // Aciksa zaten tekrar acmiyor...
                this.SqlCommand = new SqlCommand()
                {
                    Connection = this.Baglanti,
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 120,
                    CommandText = spIsim
                };
                foreach (SqlParameter param in spParametreleri)
                {
                    this.SqlCommand.Parameters.Add(param);
                }
                _sqlDataAdapter = new SqlDataAdapter(this.SqlCommand);
                DataTable dt = new DataTable();
                _sqlDataAdapter.Fill(dt);
                return dt;
            }
            finally
            {
                KapatBaglanti(this.SqlCommand);
            }
        }

        public DataTable ReturnDataTable(string strSql)
        {
            SqlDataAdapter _sqlDataAdapter;
            try
            {
                baglantiKontrol(); // Aciksa zaten tekrar acmiyor...
                this.SqlCommand = new SqlCommand()
                {
                    Connection = this.Baglanti,
                    CommandType = CommandType.Text,
                    CommandTimeout = 120,
                    CommandText = strSql
                };
                _sqlDataAdapter = new SqlDataAdapter(this.SqlCommand);
                DataTable dt = new DataTable();
                _sqlDataAdapter.Fill(dt);
                return dt;
            }
            finally
            {
                KapatBaglanti(this.SqlCommand);
            }
        }

        public int ExecuteInsertUpdateDelete(string strSql)
        {
            int etkilenenKayitSayisi = 0;
            using (SqlConnection con = new SqlConnection(veritabaniBaglantiCumlesi))
            {
                using (SqlCommand cmd = new SqlCommand(strSql, con) { CommandTimeout = 40 }) // 40 saniye icinde guncellemesi lazim. Yoksa patlasin.
                {
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    etkilenenKayitSayisi = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return etkilenenKayitSayisi;
        }

        public int ExecuteInsertUpdateDelete(string spIsim, List<SqlParameter> spParametreleri)
        {
            int etkilenenKayitSayisi = 0;
            using (SqlConnection con = new SqlConnection(veritabaniBaglantiCumlesi))
            {
                using (SqlCommand cmd = new SqlCommand(spIsim, con) { CommandTimeout = 40 }) // 40 saniye icinde guncellemesi lazim. Yoksa patlasin.
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (SqlParameter param in spParametreleri)
                    {
                        cmd.Parameters.Add(param);
                    }
                    con.Open();
                    etkilenenKayitSayisi = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return etkilenenKayitSayisi;
        }

        private bool disposed = false; // to detect redundant calls

        private void baglantiKontrol()
        {
            if (this.Baglanti.State == ConnectionState.Closed || this.Baglanti.State == ConnectionState.Broken)
            {
                this.Baglanti.Open();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (this.Baglanti != null)
                    {
                        this.Baglanti.Dispose();
                    }
                    if (this.SqlCommand != null)
                    {
                        this.SqlCommand.Dispose();
                    }
                }
                disposed = true;
            }
        }

        public SqlConnection KapatBaglanti(SqlCommand cmd)
        {
            if (this.Baglanti.State == ConnectionState.Open)
            {
                this.Baglanti.Close();
            }
            if (cmd != null)
            {
                cmd.Dispose();
            }
            return this.Baglanti;
        }
    }

}