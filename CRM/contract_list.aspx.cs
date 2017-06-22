using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Drawing;
using Excel = Microsoft.Office.Interop.Excel;
using System.Xml;
using System.Xml.Xsl;

namespace CRM
{
    public partial class contract_list : System.Web.UI.Page
    {
        private string constr, query;
        private SqlConnection con;
        private SqlDataAdapter adt;
        private DataSet ds = new DataSet();
        private SqlCommand cmd;
        private static String file_name;
        private static Byte[] file_data;


        protected void Page_Load(object sender, EventArgs e)
        {
            //getDataFromDB("");
            if (!IsPostBack)
            {
                bindDataToDDLBranch(0);
            }
            getDataFromDB(tb_contract_no.Text, ddlBranch.SelectedValue, ddlType.SelectedValue, tb_Objective.Text);
            
        }

        private void connection()
        {
            constr = ConfigurationManager.ConnectionStrings["con"].ToString();
            con = new SqlConnection(constr);
            con.Open();
        }
        private void bindDataToDDLBranch(int selectedValue)
        {
            try
            {
                connection();
                cmd = new SqlCommand("spGetAllBranch", con);
                adt = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adt.Fill(dt);
                ddlBranch.DataSource = dt;
                ddlBranch.DataTextField = "Branch_Name";
                ddlBranch.DataValueField = "Branch_Id";
                ddlBranch.DataBind();
                con.Close();
                ddlBranch.Items.Insert(0, new ListItem("-- All --", "-1"));
                ddlBranch.SelectedValue = "-1";

            }
            catch (Exception ex)
            {
                con.Close();
            }
        }
        //private void bindFiletoGrid(String search)
        //{
        //    string type = ddlType.SelectedValue;
        //    if (type == "-1")
        //        type = "";
        //    getDataFromDB(tb_contract_no.Text, Convert.ToInt16(ddlBranch.SelectedValue), type, tb_Objective.Text);            
        //    gvFile.DataSource = ds;
        //    gvFile.DataBind();
            
        //}

        private void hideColumn(bool isShow)
        {
            if (gvFile.Columns.Count > 0)
                gvFile.Columns[4].Visible = isShow;
            else
            {
                gvFile.HeaderRow.Cells[4].Visible = isShow;
                foreach (GridViewRow gvr in gvFile.Rows)
                {
                    gvr.Cells[4].Visible = isShow;
                }
            }
        }

        //private void getDataFromDB(String search)
        //{
        //    connection();
        //    cmd = new SqlCommand("spGetAllFiles", con);
        //    cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@Contract_No", search);
        //    adt = new SqlDataAdapter(cmd);
        //    ds = new DataSet();
        //    adt.Fill(ds);
        //    con.Close();
        //    gvFile.DataSource = ds;
        //    gvFile.DataBind();
        //}
        private void getDataFromDB(String contract_id, String branch, String type, String objective)
        {
            if (type == "-1")
                type = "";

            connection();
            cmd = new SqlCommand("spFindContract", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Contract_No", contract_id);
            cmd.Parameters.AddWithValue("@Branch_Id", branch);
            cmd.Parameters.AddWithValue("@Contract_Type", type);
            cmd.Parameters.AddWithValue("@Objective", objective);
            adt = new SqlDataAdapter(cmd);
            ds = new DataSet();
            adt.Fill(ds);
            con.Close();
            gvFile.DataSource = ds;
            gvFile.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //bindFiletoGrid(tb_contract_no.Text);
            getDataFromDB(tb_contract_no.Text, ddlBranch.SelectedValue, ddlType.SelectedValue, tb_Objective.Text);       
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/addfiles.aspx");
        }

        protected void gvFile_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvFile.PageIndex = e.NewPageIndex;
            //bindFiletoGrid(tb_contract_no.Text);
            getDataFromDB(tb_contract_no.Text, ddlBranch.SelectedValue, ddlType.SelectedValue, tb_Objective.Text); }

        protected void gvFile_Delete(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                LinkButton lnkRemove = (LinkButton)sender;
                cmd = new SqlCommand("spDeleteContract", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Contract_Id", lnkRemove.CommandArgument);
                cmd.ExecuteNonQuery();
                con.Close();
                getDataFromDB(tb_contract_no.Text, ddlBranch.SelectedValue, ddlType.SelectedValue, tb_Objective.Text);
            }
            catch (Exception ex)
            {
                con.Close();
            }
        }

        protected void gvFile_Download(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkDownload = (LinkButton)sender;
                for (int loopcounter = 0; loopcounter < ds.Tables[0].Rows.Count; loopcounter++)
                {
                    String f_name = Server.UrlEncode(Convert.ToString(ds.Tables[0].Rows[loopcounter]["File_Name"]));
                    Byte[] f_data = (byte[])ds.Tables[0].Rows[loopcounter]["File_Data"];

                    if (Convert.ToString(ds.Tables[0].Rows[loopcounter]["Contract_Id"]) == lnkDownload.CommandArgument)
                    {
                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = "PDF";
                        Response.AddHeader("content-disposition", "attachment;filename=" + f_name);     // to open file prompt Box open or Save file         
                        Response.Charset = "";
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.BinaryWrite(f_data);
                        Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
            }

        }

        //protected void btnExport_Click(object sender, EventArgs e)
        //{

        //    Response.Clear();

        //    Response.Buffer = true;

        //    Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xls");

        //    Response.Charset = "";

        //    Response.ContentType = "application/vnd.ms-excel";

        //    using (StringWriter sw = new StringWriter())
        //    {

        //        HtmlTextWriter hw = new HtmlTextWriter(sw);



        //        //To Export all pages

        //        gvFile.AllowPaging = false;

        //        //this.BindGrid();




        //        gvFile.HeaderRow.BackColor = Color.White;

        //        foreach (TableCell cell in gvFile.HeaderRow.Cells)
        //        {

        //            cell.BackColor = gvFile.HeaderStyle.BackColor;

        //        }

        //        foreach (GridViewRow row in gvFile.Rows)
        //        {

        //            row.BackColor = Color.White;

        //            foreach (TableCell cell in row.Cells)
        //            {

        //                if (row.RowIndex % 2 == 0)
        //                {

        //                    cell.BackColor = gvFile.AlternatingRowStyle.BackColor;

        //                }

        //                else
        //                {

        //                    cell.BackColor = gvFile.RowStyle.BackColor;

        //                }

        //                cell.CssClass = "textmode";

        //            }

        //        }



        //        gvFile.RenderControl(hw);



        //        //style to format numbers to string

        //        string style = @"<style> .textmode { } </style>";

        //        Response.Write(style);

        //        Response.Output.Write(sw.ToString());

        //        Response.Flush();

        //        Response.End();



        //    }
        //}
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
        //private void ExportGridToExcel()   
        //{   
        //    Response.Clear();   
        //    Response.AddHeader("content-disposition", "attachment;filename=ExportGridData.xls");
        //    Response.ContentType = "application/ms-excel";
        //    gvFile.AllowPaging = false;
        //    this.bindFiletoGrid("");
        //    //hideColumn(true);

        //    StringWriter StringWriter = new System.IO.StringWriter();   
        //    HtmlTextWriter HtmlTextWriter = new HtmlTextWriter(StringWriter);

        //    gvFile.HeaderRow.Cells[6].Visible = false;
        //    gvFile.HeaderRow.Cells[7].Visible = false;
        //    for (int i = 0; i < gvFile.Rows.Count; i++)
        //    {
        //        GridViewRow row = gvFile.Rows[i];
        //        row.Cells[6].Visible = false;
        //        row.Cells[7].Visible = false;

        //        row.BackColor = System.Drawing.Color.White;
        //        row.Attributes.Add("class", "textmode");
        //    }

        //    gvFile.RenderControl(HtmlTextWriter);
        //    string style = @"<style> .textmode { mso-number-format:\@; } </style>";
        //    Response.Write(style);
        //    Response.Output.Write(StringWriter.ToString());   
        //    Response.End();   
        //}

        protected void btnExport_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }
        protected void ExportToExcel()
        {
            if (ds.Tables[0].Rows.Count != 0)
            {
                //Create a dummy GridView
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = ds;
                GridView1.DataBind();


                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=Report.xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";

                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                GridView1.HeaderRow.Cells[0].Text = "No";
                GridView1.HeaderRow.Cells[6].Visible = false;
                GridView1.HeaderRow.Cells[7].Visible = false;
                GridView1.HeaderRow.Cells[8].Visible = false;
                GridView1.HeaderRow.Cells[9].Visible = false;
                GridView1.HeaderRow.Cells[10].Visible = false;
                GridView1.HeaderRow.Cells[11].Visible = false;
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    GridViewRow row = GridView1.Rows[i];
                    row.Cells[0].Text = Convert.ToString(i + 1);
                    row.Cells[2].Text = (Convert.ToDateTime(row.Cells[2].Text).Date).ToShortDateString();
                    row.Cells[4].Text = (Convert.ToDateTime(row.Cells[4].Text).Date).ToShortDateString();
                    row.Cells[5].Text = (Convert.ToDateTime(row.Cells[5].Text).Date).ToShortDateString();
                    row.Cells[6].Visible = false;
                    row.Cells[7].Visible = false;
                    row.Cells[8].Visible = false;
                    row.Cells[9].Visible = false;
                    row.Cells[10].Visible = false;
                    row.Cells[11].Visible = false;
                    if (row.Cells[13].Text == "0")
                    {
                        row.Cells[13].Text = "Renewable";
                    }
                    else {
                        row.Cells[13].Text = "Non Renewable";
                    }
                    row.Attributes.Add("class", "textmode");
                }

                GridView1.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }

        }
    }
    
}