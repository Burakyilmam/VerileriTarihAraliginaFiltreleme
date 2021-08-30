using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
    public partial class Form1 : Form
    {
        string constring = ("Data source =  192.168.7.19,1433;initial catalog = CPM; integrated security = True; User ID=sa;Password=sql;Trusted_Connection=False;");
        String firma = ConfigurationManager.AppSettings["FİRMA"];
        String donem = ConfigurationManager.AppSettings["DÖNEM"];
        SqlConnection cn = null;
        public Form1()
        {
            InitializeComponent();   
        }
        
       
        private void Form1_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Today;
            dateTimePicker2.Value = DateTime.Today;
            constring = ConfigurationManager.AppSettings["connectionstrings"];
            firma = ConfigurationManager.AppSettings["FİRMA"];
            donem = ConfigurationManager.AppSettings["DÖNEM"];
            cn = new SqlConnection(constring);
        }
   
        private void listBtn_Click(object sender, EventArgs e)
        {
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnFiltre_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adp = new SqlDataAdapter("SELECT BAS.BELGENO,BAS.TARIH,FT.FIRMAADI,ST.STOKKODU AS \"REFERANS NUMARASI\",FT.ISTIHBARAT MAIL,ST.MALINCINSI,KARSISTOKKODU AS \"Zn+Ni Min-Max\",KARSIBARKOD AS \"KTL Min-Max\",BARKOD AS \"Zn Min-Max\",FDT.PIRIM AS \"%Ni\",MAILRESPONSE FROM " + firma + donem + "TBLSATIRSHAREKET FDT,"+firma+ "TBLCARI FT,"+firma + "TBLSTOKLAR ST," + firma + donem + "TBLSATIRSBASLIK BAS WHERE FDT.FIRMANO=FT.IND and FDT.STOKNO =ST.IND and FDT.EVRAKNO=BAS.IND and (ISNULL(FDT.PIRIM,0)!=0 OR (ISNULL(BARKOD,'')!='') OR ISNULL(KARSISTOKKODU,'')!='' OR ISNULL(KARSIBARKOD,'')!='') AND MAILSTATUS=0 AND MAILRESPONSE IS NULL and BAS.TARIH BETWEEN @tarih1 and @tarih2 ORDER BY FDT.FIRMANO ", cn);
            adp.SelectCommand.Parameters.AddWithValue("@tarih1", dateTimePicker1.Value);
            adp.SelectCommand.Parameters.AddWithValue("@tarih2", dateTimePicker2.Value);
            cn.Open();
            adp.Fill(dt);
            dataGridView1.DataSource = dt;
            cn.Close();       
        }

        private void updateBtn_Click_1(object sender, EventArgs e)
        {
            cn.Open();
            SqlCommand cmd = new SqlCommand("Update " + firma + donem + "TBLSATIRSHAREKET SET MAILSTATUS=NULL where MAILSTATUS=0", cn);
            cmd.ExecuteNonQuery();
            MessageBox.Show("MAILSTATUS DEĞERİ 0 OLAN DEĞERLER NULL OLARAK GÜNCELLENDİ.");
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            cn.Close();
        }
    }
}
