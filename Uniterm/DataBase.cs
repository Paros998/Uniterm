using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
//using System.Data.SqlServerCe;


namespace Uniterm
{
    class DataBase
    {
        #region Variables

        //public static string connectionString = "Data Source=DRACO\\SQLEXPRESS;Initial Catalog=uniterm;Integrated security=true";
        //public static string connectionString = "Data Source=MSSQL-SERWER\\MSSQL;Initial Catalog=s73484;Password=s73484; User Id=s73484";
        //public static string connectionString = "Data Source=ovsyak\\SQLEXPRESS;Initial Catalog=slowik;Password=ovsyak; User Id=ovsyak";
        //public static string connectionString = "Data Source=VOLODYMYR-ПК\\Microsoft SQL Server Compact 4.0 (.NET Framework);Initial Catalog=uniterms;Password=ovsyak; User Id=ovsyak";
        //public static string connectionString = "Data Source=VOLODYMYR-ПК\\D:SLOWIK;Initial Catalog=uniterms";
        //public static string connectionString = @"Data Source=KZI-VOSTRO2\\SQL Server Compact;Initial Catalog=C:\Users\Volodymir Ovsyak\Desktop\MASI_2013\MyDatabase#1.sdf;Password=ovsyak";
        //public static string connectionString = @"Data Source=127.0.0.1:1520\\SQL Server Compact;C:\Users\Volodymir Ovsyak\Desktop\MASI_2013\MyDatabase#1.sdf;Password=ovsyak";
        //public static string connectionString = "Data Source=C:\\Users\\Volodymir Ovsyak\\Desktop\\MASI_2013\\MyDatabase#1.sdf;";
        //public static string connectionString = "Data Source=C:\\Users\\Volodymir Ovsyak\\Documents\\MyDatabase#3.sdf;";
        //public static string connectionString = @"Data Source=C:\SLOWIK\Uniterm\Uniterm\_MyDatabase_1DataSet.xsd;";
        //public static string connectionString = @"Data Source=Data Source=KZI-VOSTRO2\SQLEXPRESS;Initial Catalog=uniterm;Integrated Security=True";
        //public static String connectionString = @"Data Source=KZI-VOSTRO2\VOSTROSQLSERWER;Initial Catalog=uniterms;Integrated Security=True";
        //public static String connectionString = @"Data Source=KZI-VOSTRO2\VOSTROSQLSERWER;Initial Catalog=MOJA;Integrated Security=True";
        //public static String connectionString = @"Data Source=KZI-VOSTRO2\VOSTROSQLSERWER;Initial Catalog=STUDENT;Integrated Security=True";
        //public static String connectionString = @"Data Source=KZI-VOSTRO2\VOSTROSQLSERWER;Initial Catalog=STUDENTKA;Integrated Security=True";
        //public static string connectionString = @"Data Source=C:\SLOWIK\VOSTROSQLSERWER;Initial Catalog=MASI.sdf;Password=ovsyak;User Id=ovsyak";
        //public static string connectionString = @"Data Source=C:\SLOWIK\VOSTROSQLSERWER;Initial Catalog=ARTUR.dbo;Integrated Security=True";
        //public static String connectionString = @"Data Source=KZI-VOSTRO2\VOSTROSQLSERWER;Initial Catalog=ARTUR;Integrated Security=True";
        //Data Source=KZI-VOSTRO2\VOSTROSQLSERWER;Initial Catalog=ARTUR;Integrated Security=True;Pooling=False
        //public static String connectionString = @"Data Source=KZI-VOSTRO2\VOSTROSQLSERWER;Initial Catalog=PIT;Integrated Security=True";

        //public static String connectionString = @"Data Source=KZI-VOSTRO2\VOSTROSQLSERWER;Initial Catalog=BAZ;Integrated Security=True";

      //  public static String connectionString = @"Data Source=KZI-VOSTRO2\VOSTROSQLSERWER;Initial Catalog=DH;Integrated Security=True";

        private SqlConnection conString;

        #endregion

        #region Builders and Finalizers

        /*public DataBase()
        {
            this.conString = new SqlConnection(DataBase.connectionString);
        }*/

       /* public DataBase(string conStr)
        {
            this.conString = new SqlConnection(conStr);
        }*/

        #endregion

        #region Properties

        public SqlConnection ConnectionString
        {
            get
            {
                return this.conString;
            }
            set
            {
                this.conString = value;
            }
        }

        #endregion

        #region Methods

       /* public void Connect()
        {
            try
            {
                this.conString.Open();
            }
            catch (InvalidCastException ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }*/

        public void Disconnect()
        {
            try
            {
                this.conString.Close();
            }
            catch
            {

            }
        }

        public SqlDataAdapter CreateAdapter(string query)
        {
            return new SqlDataAdapter(query, this.ConnectionString);
        }

        /*public DataTable CreateDataTable(string query)
        {
            DataTable tab = new DataTable();

           // this.Connect();

            if (this.ConnectionString.State == ConnectionState.Open)
            {
                SqlDataAdapter ad = CreateAdapter(query);

                ad.Fill(tab);
            }
            else
            {
                throw new Exception("Nie można połączyć się z bazą daych");
            }

            this.Disconnect();

            return tab;
        }*/

        public DataRow CreateDataRow(string query)
        {
            DataTable tab = new DataTable();

            DataRow row;

           // this.Connect();

            if (this.ConnectionString.State == ConnectionState.Open)
            {

                SqlDataAdapter ad = CreateAdapter(query);

                ad.Fill(tab);
                try
                {
                    row = tab.Rows[0];
                }
                catch
                {
                    row = null;
                }
            }
            else
            {
                throw new Exception("Nie można połączyć się z bazą daych");
            }

            this.Disconnect();

            return row;
        }

        public void RunQuery(string query)
        {
           // this.Connect();

            if (this.ConnectionString.State == ConnectionState.Open)
            {
                SqlCommand cmd = new SqlCommand(query, this.ConnectionString);

                cmd.ExecuteNonQuery();
            }
            else
            {
                throw new Exception("Nie można połączyć się z bazą daych");
            }

            this.Disconnect();
        }
        #endregion
    }
}
