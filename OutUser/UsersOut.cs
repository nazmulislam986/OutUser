using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

using System.Windows.Forms;

namespace OutUser
{
    public partial class UsersOut : Form
    {
        #region Comments
        private OdbcConnection conn = new OdbcConnection("Dsn=RETAILMasterSHOPS;uid=sa;Pwd=Ajwahana$@$;");
        private string pcid;
        #endregion
        public UsersOut()
        {
            InitializeComponent();
            this.pcid = UsersOut.GetMACAddress();
            this.panel1.Visible = false;
            this.panel2.Visible = false;
            this.label2.Text = "";
            this.label4.Text = "";
            this.panel1.Visible = false;
            this.label1.Text = "";
            this.slabel8.Visible = false;
            DataTable dataTable1 = new DataTable();
            OdbcDataAdapter odbcDataAdapter1 = new OdbcDataAdapter(string.Concat("Select Serial from MCounter where [PCname]='", this.pcid, "'"), this.conn);
            odbcDataAdapter1.Fill(dataTable1);
            this.label6.Text = dataTable1.Rows[0][0].ToString();
        }
        public static string GetMACAddress()
        {
            NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            string empty = string.Empty;
            NetworkInterface[] networkInterfaceArray = allNetworkInterfaces;
            for (int i = 0; i < (int)networkInterfaceArray.Length; i++)
            {
                NetworkInterface networkInterface = networkInterfaceArray[i];
                if (empty == string.Empty)
                {
                    empty = networkInterface.GetPhysicalAddress().ToString();
                }
            }
            return empty;
        }
        private void GetOnlineUsers()
        {
            DataTable dataTable = new DataTable();
            OdbcDataAdapter odbcDataAdapter = new OdbcDataAdapter(string.Concat("Select PCName,UserName,Designation from OnlineUser"), this.conn);
            odbcDataAdapter.Fill(dataTable);
            this.cmbcomboBox1.DataSource = dataTable.DefaultView;
            this.cmbcomboBox1.DisplayMember = "UserName";
            this.cmbcomboBox1.ValueMember = "Designation";
        }
        private void insCopyBar()
        {
            OdbcCommand odbcCommand1;
            string[] strArrays;
            object[] objArray;
            do
            {
                DataTable dataTable = new DataTable();
                OdbcDataAdapter odbcDataAdapter1 = new OdbcDataAdapter(string.Concat("Select * from CounterXXXX where Barcode='", this.textBox5.Text.Trim(), "'"), this.conn);
                odbcDataAdapter1.Fill(dataTable);
                if (dataTable.Rows.Count != 0)
                {
                    DataTable dataTable5 = new DataTable();
                    strArrays = new string[] { "select Barcode,CPU,RPU,PrdName,BalQty,DateTime,sPC from CounterXXXX where Barcode='", this.textBox5.Text.Trim(), "' " };
                    OdbcDataAdapter odbcDataAdapter6 = new OdbcDataAdapter(string.Concat(strArrays), this.conn);
                    odbcDataAdapter6.Fill(dataTable5);
                    string num3  = dataTable5.Rows[0][0].ToString();
                    string num4  = dataTable5.Rows[0][1].ToString();
                    string num5  = dataTable5.Rows[0][2].ToString();
                    string num6  = dataTable5.Rows[0][3].ToString();
                    string str12 = dataTable5.Rows[0][4].ToString();
                    string str13 = dataTable5.Rows[0][6].ToString();
                    string num7  = dataTable5.Rows[0][5].ToString();
                    DataTable dataTable6 = new DataTable();
                    OdbcDataAdapter odbcDataAdapter7 = new OdbcDataAdapter(string.Concat("Select Barcode from CounterXXXX where Barcode='", this.textBox5.Text.Trim(), "'"), this.conn);
                    odbcDataAdapter7.Fill(dataTable6);
                    if (!(this.textBox5.Text.Trim() == dataTable6.Rows[0][0].ToString()))
                    {
                        objArray = new object[] { " Update CounterXXXX set DateTime='", num7, "' " };
                        OdbcCommand odbcCommand6 = new OdbcCommand(string.Concat(objArray), this.conn);
                        odbcCommand6.ExecuteNonQuery();
                    }
                    else
                    {
                        this.conn.Open();
                        objArray = new object[] { "Update CounterXXXX set DateTime='", num7, "' where Barcode='", this.textBox5.Text.Trim(), "'" };
                        OdbcCommand odbcCommand = new OdbcCommand(string.Concat(objArray), this.conn);
                        odbcCommand.ExecuteNonQuery();
                        this.conn.Close();
                    }
                }
                else
                {
                    this.conn.Open();
                    strArrays = new string[] { "Insert into CounterXXXX(Barcode,CPU,RPU,PrdName,BalQty,DateTime,sPC) " +
                                               "Values('",Convert.ToString(dataGridView1.Rows[0].Cells[0].Value), "','",Convert.ToString(dataGridView1.Rows[0].Cells[1].Value),"','",Convert.ToString(dataGridView1.Rows[0].Cells[2].Value),"','",Convert.ToString(dataGridView1.Rows[0].Cells[3].Value), "','",Convert.ToString(dataGridView1.Rows[0].Cells[4].Value),"', '",DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss tt"),"','",this.label6.Text.Trim(),"')" };
                    odbcCommand1 = new OdbcCommand(string.Concat(strArrays), this.conn);
                    odbcCommand1.ExecuteNonQuery();
                    this.conn.Close();
                    this.label1.Text = "-";
                }
            }
            while (this.dataGridView1.Rows.Count == 0);
            this.conn.Close();
        }
        private void txttextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                if (this.txttextBox1.Text.Trim() == "sbazarbd")
                {
                    this.panel2.Visible = true;
                    GetOnlineUsers();
                    this.panel3.Visible = false;
                }
                else
                {
                    this.lbllabel2.Text = "worng pass / empty";
                    this.txttextBox1.Text = "";
                }
            }
        }
        private void btnbutton1_Click(object sender, EventArgs e)
        {
            this.conn.Open();
            string[] strArrays = new string[] { "Delete from OnlineUser where UserName='", this.cmbcomboBox1.Text.Trim(), "'" };
            OdbcCommand odbcCommand = new OdbcCommand(string.Concat(strArrays), this.conn);
            odbcCommand.ExecuteNonQuery();
            this.conn.Close();
            this.lbllabel4.Visible = true;
            this.lbllabel4.Text = "Sucessfully Logout [ " + this.cmbcomboBox1.Text.Trim() + " ]";
            this.GetOnlineUsers();
        }
        private void lbllabel5_Click(object sender, EventArgs e)
        {
            this.panel3.Visible = false;
            this.panel2.Visible = false;
            this.panel1.Visible = true;
            this.txttextBox1.Text = "";
            this.textBox5.Select();
        }
        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                if (!(this.textBox5.Text.Trim() != ""))
                {
                    this.dataGridView1.DataSource = null;
                    this.textBox5.Focus();
                }
                else
                {
                    DataTable dataTable = new DataTable();
                    string[] strArrays = new string[] { "Select Barcode,CPU as [Cost/Unit],RPU as [Sale/Unit],PrdName as Product,BalQty as Balance from SaleSearch where Barcode like '%", this.textBox5.Text.Trim(), "%' order by PrdName Asc" };
                    OdbcDataAdapter odbcDataAdapter = new OdbcDataAdapter(string.Concat(strArrays), this.conn);
                    odbcDataAdapter.Fill(dataTable);
                    if (dataTable.Rows.Count <= 0)
                    {
                        this.dataGridView2.Visible = false;
                        this.dataGridView1.DataSource = null;
                        this.label2.Text = "No Data Found & Copyed. [ " + this.textBox5.Text.Trim() + " ]";
                        this.textBox5.Focus();
                        this.textBox5.SelectAll();
                        this.textBox5.Copy();
                    }
                    else
                    {
                        this.dataGridView2.Visible = false;
                        this.dataGridView1.DataSource = dataTable;
                        this.label2.Text = "";
                        this.textBox5.SelectAll();
                        this.textBox5.Copy();
                        this.label4.Text = "Copyed [ " + this.textBox5.Text.Trim() + " ]";
                        this.insCopyBar();
                    }
                }
            }
        }
        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (this.dataGridView1.RowCount > 0)
                {
                    this.dataGridView1.Focus();
                }
            }
        }
        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                if (this.textBox6.Text == "")
                {
                    DataTable dataTable = new DataTable();
                    (new OdbcDataAdapter("Select Sbarcode,Barcode,SSName as [Style Size],BTName as [Brand Name],PrdName as [Product Name],GroupName,RPU as [Sale Price],Balqty as [Balance Quantity],SupID as [Supplier ID],SupName as [Supplier Name] from buy where balqty!=0 order by Balqty", this.conn)).Fill(dataTable);
                    this.dataGridView1.DataSource = dataTable;
                }
            }
        }
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            this.dataGridView2.Visible = false;
            DataTable dataTable;
            OdbcDataAdapter odbcDataAdapter;
            string[] strArrays;
            if (!(this.textBox6.Text.Trim() != ""))
            {
                this.dataGridView1.DataSource = null;
            }
            else
            {
                dataTable = new DataTable();
                strArrays = new string[] { "Select TOP(500) Barcode,CPU as [Cost/Unit],RPU as [Sale/Unit],PrdName as Product,BalQty as Balance from SaleSearch where PrdName like '%", this.textBox6.Text.Trim(), "%' order by PrdName Asc" };
                odbcDataAdapter = new OdbcDataAdapter(string.Concat(strArrays), this.conn);
                odbcDataAdapter.Fill(dataTable);
                if (dataTable.Rows.Count <= 0)
                {
                    this.dataGridView1.DataSource = null;
                    this.label2.Text = "No Search Product. [ " + this.textBox6.Text.Trim() + " ]";
                    this.textBox5.Text = "";
                    this.textBox5.Text = "";
                }
                else
                {
                    this.dataGridView1.DataSource = dataTable;
                    this.label2.Text = "";
                    this.textBox5.Text = "";
                }
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridView1.RowCount > 0)
            {
                DataTable dataTable = new DataTable();
                OdbcDataAdapter odbcDataAdapter = new OdbcDataAdapter(string.Concat("Select Barcode from buy where Barcode='", this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), "'"), this.conn);
                odbcDataAdapter.Fill(dataTable);
                this.textBox5.Text = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                this.textBox5.Focus();
                this.textBox5.Copy();
                this.label4.Text = "Copyed [ " + this.textBox5.Text.Trim() + " ]";
            }
        }
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (this.dataGridView1.RowCount > 0)
                {
                    DataTable dataTable = new DataTable();
                    OdbcDataAdapter odbcDataAdapter = new OdbcDataAdapter(string.Concat("Select Barcode from buy where Barcode='", this.dataGridView1.SelectedRows[0].Cells[3].Value.ToString(), "'"), this.conn);
                    odbcDataAdapter.Fill(dataTable);
                    this.textBox5.Text = this.dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                }
            }
        }
        private void Serchlabel1Cross_Click(object sender, EventArgs e)
        {
            this.panel1.Visible = false;
            this.panel3.Visible = true;
            this.txttextBox1.Text = "";
        }
        private void ActiveUserlabel6Cross_Click(object sender, EventArgs e)
        {
            this.panel2.Visible = false;
            this.panel3.Visible = true;
            this.txttextBox1.Text = "";
        }
        private void label3_DoubleClick(object sender, EventArgs e)
        {
            this.dataGridView2.Visible = true;
            this.conn.Open();
            DataTable dataTable = new DataTable();
            OdbcDataAdapter odbcDataAdapter = new OdbcDataAdapter(string.Concat("Select sPC as PC,DateTime as [Date Time],Barcode as Barcode,CPU,RPU,PrdName as Name,BalQty from CounterXXXX"), this.conn);
            odbcDataAdapter.Fill(dataTable);
            dataGridView2.DataSource = dataTable;
            this.conn.Close();
        }
        private void label3_Click(object sender, EventArgs e)
        {
            this.dataGridView2.Visible = false;
        }
        private void label1_DoubleClick(object sender, EventArgs e)
        {
            this.conn.Open();
            string[] strArrays = new string[] { "Delete from CounterXXXX " };
            OdbcCommand odbcCommand = new OdbcCommand(string.Concat(strArrays), this.conn);
            odbcCommand.ExecuteNonQuery();
            this.conn.Close();
            this.label1.Text = "";
        }
        private void label5_Click(object sender, EventArgs e)
        {
            this.textBox6.Text = "";
            this.textBox5.Text = "";
            this.textBox5.Focus();
        }
        private void label7_DoubleClick(object sender, EventArgs e)
        {
            this.slabel8.Visible = true;
            string str = "";
            string[] strArrays = new string[] { "SELECT Sum(netAmt) AS max FROM Ssummary where SaleDT>='", this.dateTimePicker1.Text.Trim(), "' and SaleDT<='", this.dateTimePicker2.Text.Trim(), "' and SaleType='Direct'" };
            OdbcCommand odbcCommand = new OdbcCommand(string.Concat(strArrays), this.conn);
            this.conn.Open();
            OdbcDataReader odbcDataReader = odbcCommand.ExecuteReader();
            if (odbcDataReader.Read())
            {
                str = odbcDataReader[0].ToString();
            }
            odbcDataReader.Close();
            this.conn.Close();
            if (!(str == ""))
            {
                double num = Convert.ToDouble(str);
                this.slabel8.Text = (num).ToString();
            }
            else
            {
                this.slabel8.Text = "0";
            }
        }
        private void label7_Click(object sender, EventArgs e)
        {
            this.slabel8.Visible = false;
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
