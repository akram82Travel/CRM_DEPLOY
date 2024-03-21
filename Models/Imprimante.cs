using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CRMSTUBSOFT.Models
{
    public class Imprimante
    {
        private static Imprimante instance = new Imprimante();

        #region *****attributs ******
        protected string _NumeroImprimante;
        protected string _Libelle;
        protected string _Configuration;
        #endregion

        #region *****get /set ******
        public virtual string NumeroImprimante
        {
            get
            {
                return _NumeroImprimante;
            }
            set
            {
                _NumeroImprimante = value;
            }
        }

        public virtual string Libelle
        {
            get
            {
                return _Libelle;
            }
            set
            {
                _Libelle = value;
            }
        }
        public virtual string Configuration
        {
            get
            {
                return _Configuration;
            }
            set
            {
                _Configuration = value;
            }
        }

        #endregion

        #region ***** CRUD *****
        public static Imprimante getInstance()
        {
            return instance;
        }
        #endregion

        #region *****GetNomImprimante******
        public String GetNomImprimante(String P_Numero)
        {
            string reqSQL = "Select Configuration From Imprimante where NumeroImprimante = '" + P_Numero + "' ";

            try
            {
                String Config = "";
                DataTable myResult = GetDataTable(reqSQL);
                if (myResult != null)
                {
                    Config = (String)myResult.Rows[0][0];
                }


                return Config;

            }
            catch (Exception myErr)
            {
                throw (new Exception(myErr.ToString() + reqSQL));
            }
        }
        public static DataTable GetDataTable(string strSQL)
        {
            Boolean succeeded = false;
            DataTable DTT = null;
            //int code = 0;
            int Retry = 0;
            while (!succeeded && Retry < 3)
            {
                Retry++;
                try
                {
                    string Serveur = ConfigurationManager.AppSettings["Serveur"];
                    string Db = ConfigurationManager.AppSettings["Db"];
                    string Usr = ConfigurationManager.AppSettings["Usr"];
                    string Pwd = ConfigurationManager.AppSettings["Pwd"];

                    SqlConnection myConn;
                    string cnxString = "user id=" + Usr + ";Password =" + Pwd + ";data source=" + Serveur + ";initial catalog=" + Db;
                    myConn = new SqlConnection(cnxString);
                    myConn.Open();
                    using (SqlDataAdapter myDataAdapter = new SqlDataAdapter(strSQL, myConn))
                    {
                        DataSet myDataSet = new DataSet();
                        myDataAdapter.Fill(myDataSet, "MySrcTable");
                        succeeded = true;
                        //return myDataSet.Tables["MySrcTable"];
                        DTT = myDataSet.Tables["MySrcTable"];
                        return DTT;

                    }
                    //myConn.Close();
                }
                catch (Exception myException)
                {
                    if (!succeeded && Retry >= 3)
                    {
                        throw (new Exception(myException.Message));

                    }


                }
            }

            return DTT;
        }

        #endregion
    }
}
