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
			mram.Send("mrsystems", "WIN-1I5C3456H92", cboRecipient.Text, txtMessage.Text, cboPriority.SelectedIndex);
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			MRAreaMessage mram = new MRAreaMessage(DbConnection);
			int i = mram.UnreadCount("mrsystems", "Influent PS");
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			MRAreaMessage mram = new MRAreaMessage(DbConnection);
			DataSet ds = mram.GetUnreadMessages("mrsystems", "Influent PS");
			dataGrid1.DataSource = ds;
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			MRMessage mrMsg = new MRMessage(DbConnection);
			DataSet ds = mrMsg.GetMessages("Administrator", cboPriority.SelectedIndex, chkRead.Checked);
			dataGrid1.DataSource = ds;
		}
		
		void Button4Click(object sender, EventArgs e)
		{
			MRMessage mrMsg = new MRMessage(DbConnection);
			DataSet ds = mrMsg.GetMessages("Administrator", cboPriority.SelectedIndex, chkRead.Checked, chkArchived.Checked);
			dataGrid1.DataSource = ds;
		}
	}
}
