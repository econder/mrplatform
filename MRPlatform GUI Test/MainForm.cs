/*
 * Created by SharpDevelop.
 * User: mrsystems
 * Date: 2/28/2014
 * Time: 7:59 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

using MRPlatform.Data.Sql;
using MRPlatform.AlarmEvent;
using MRPlatform.Message;
using MRPlatform.WMI;


namespace MRPlatform_GUI_Test
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
        private MRDbConnection DbConnection { get; set; }


		public MainForm()
		{
			InitializeComponent();
		}

        void BtnSendClick(object sender, EventArgs e)
        {
            MRDbConnection dbConn = new MRDbConnection("WIN-1I5C3456H92\\SQLEXPRESS", "mrsystems", "mrsystems", "Reggie#123", MRDbConnection.RedundantNode.Master);
            DbConnection = dbConn;

            MRAreaMessage mram = new MRAreaMessage(DbConnection);
            mram.Send("mrsystems", cboRecipient.Text, txtMessage.Text, 2);
        }

		void Button1Click(object sender, EventArgs e)
		{
            MRDbConnection dbConn = new MRDbConnection("WIN-1I5C3456H92\\SQLEXPRESS", "mrsystems", "mrsystems", "Reggie#123", MRDbConnection.RedundantNode.Master);
            DbConnection = dbConn;

            MRAreaMessage mram = new MRAreaMessage(DbConnection);
			int i = mram.UnreadCount("mrsystems", "Influent PS");
		}
		
		void Button2Click(object sender, EventArgs e)
		{
            MRDbConnection dbConn = new MRDbConnection("WIN-1I5C3456H92\\SQLEXPRESS", "mrsystems", "mrsystems", "Reggie#123", MRDbConnection.RedundantNode.Master);
            DbConnection = dbConn;

            MRAreaMessage mram = new MRAreaMessage(DbConnection);
			DataSet ds = mram.GetUnreadMessages("mrsystems", "Influent PS");
			dataGrid1.DataSource = ds;
		}
		
		void Button3Click(object sender, EventArgs e)
		{
            MRDbConnection dbConn = new MRDbConnection("WIN-1I5C3456H92\\SQLEXPRESS", "mrsystems", "mrsystems", "Reggie#123", MRDbConnection.RedundantNode.Master);
            DbConnection = dbConn;

            MRUserMessage mrMsg = new MRUserMessage(DbConnection);
			DataSet ds = mrMsg.GetMessages("Administrator", cboPriority.SelectedIndex, chkRead.Checked);
			dataGrid1.DataSource = ds;
		}
		
		void Button4Click(object sender, EventArgs e)
		{
            MRDbConnection dbConn = new MRDbConnection("WIN-1I5C3456H92\\SQLEXPRESS", "mrsystems", "mrsystems", "Reggie#123", MRDbConnection.RedundantNode.Master);
            DbConnection = dbConn;

            MRUserMessage mrMsg = new MRUserMessage(DbConnection);
			DataSet ds = mrMsg.GetMessages("Administrator", cboPriority.SelectedIndex, chkRead.Checked, chkArchived.Checked);
			dataGrid1.DataSource = ds;
		}



        private void button7_Click(object sender, EventArgs e)
        {
            MRPlatform.WMI.OperatingSystem os = new MRPlatform.WMI.OperatingSystem();
            label4.Text = Convert.ToString(os.FreePhysicalMemory);
            label5.Text = Convert.ToString(os.TotalVisibleMemorySize);

            LogicalDisks lds = new LogicalDisks();

            label6.Text = Convert.ToString(lds.Disk("C").Size);

            foreach(LogicalDisk ld in lds)
            {
                txtMessage.Text += String.Format("{0} --- {1} --- {2}\n", ld.Name, ld.Caption, ld.Description);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MRDbConnection dbConn = new MRDbConnection("ECVM-WW2014", "A2ALMDB", "wwAdmin", "wwAdmin", MRDbConnection.RedundantNode.Master);
            DbConnection = dbConn;

            MRAlarmEventLog mrae = new MRAlarmEventLog(dbConn);
            mrae.GetTopAlarmOccurrences(20, Convert.ToDateTime("1/1/2014"), Convert.ToDateTime("11/30/2016"));

            DbConnection.DbConnection.Close();
            DbConnection.DbConnection.Dispose();
        }
    }
}
