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

namespace CRM
{
    public partial class addfiles : System.Web.UI.Page
    {
        private string constr, query;
        private SqlConnection con;
        private SqlDataAdapter adt;
        private DataSet ds = new DataSet();
        private SqlCommand cmd;
        private static String file_name;
        private static Byte[] file_data;
        private int Contract_Id;

        protected void Page_Load(object sender, EventArgs e)
        {

            getDataFromDB("");
            if (!IsPostBack)
            {
                bindDataToDDLBranch(0);
            }
            if (this.Page.PreviousPage != null)
            {
                Contract_Id = int.Parse(Request.QueryString["Contract_Id"]);
                getDataToEdit(Contract_Id.ToString());
            }
        }



        protected void btnAdd_Click(object sender, EventArgs e)
        {

            if (btnAdd.Text == "Add")
            {
                if (!RequiredFileUpload.IsValid)
                {
                    RequiredFileUpload.ErrorMessage = "Please select file!";
                }
                else
                {
                    addToDB();
                }
            }
            else
            {
                updateToDB();
                RequiredFileUpload.ErrorMessage = "";
            }






        }
        

        private void getDataToEdit(String contract_id)
        {
            
            for (int loopcounter = 0; loopcounter < ds.Tables[0].Rows.Count; loopcounter++)
            {
                DataRow row = ds.Tables[0].Rows[loopcounter];
                if (Convert.ToString(ds.Tables[0].Rows[loopcounter]["Contract_Id"]) == contract_id)
                {
                    tb_contract_id.Text = Convert.ToString(row["Contract_Id"]);
                    tb_contract_no.Text = Convert.ToString(row["Contract_No"]);
                    tb_contract_date.Text = (Convert.ToDateTime(row["Contract_Date"]).Date).ToShortDateString();
                    tb_objective.Text = Convert.ToString(row["Objective"]);
                    tb_effective.Text = (Convert.ToDateTime(row["Effective_Date"]).Date).ToShortDateString();
                    tb_expire.Text = (Convert.ToDateTime(row["Expire_Date"]).Date).ToShortDateString();
                    tb_remindon.Text = Convert.ToString(row["Remind_On"]);
                    tb_recipient.Text = Convert.ToString(row["Recipient"]);
                    tb_responder.Text = Convert.ToString(row["Responder"]);
                    file_name = Convert.ToString(row["File_Name"]);
                    file_data = (byte[])row["File_Data"];
                    ddlBranch.SelectedValue = Convert.ToString(ds.Tables[0].Rows[loopcounter]["Branch_Id"]);
                    ddlType.SelectedValue = Convert.ToString(row["Contract_Type"]);
                    

                    btnAdd.Text = "Update";
                }
            }
        }
        
        protected void btnClear_Click(object sender, EventArgs e)
        {
            if (btnAdd.Text == "Update")
            {
                btnAdd.Text = "Add";
            }
            clearControl();
        }

        //========================================================================================
        private void clearControl()
        {
            tb_contract_no.Text = "";
            tb_contract_date.Text = "";
            tb_objective.Text = "";
            tb_effective.Text = "";
            tb_expire.Text = "";
            tb_remindon.Text = "";
            tb_recipient.Text = "";
            tb_responder.Text = "";
            file_name = "";
            file_data = new byte[0];
            ddlBranch.SelectedValue = "1";
            ddlType.SelectedValue = "0";

        }

        private DateTime getDateOnly(String parse_date)
        {
            DateTime contract_date = DateTime.ParseExact(parse_date, "dd/MM/yyyy", null);
            return contract_date;
        }
        private void addToDB()
        {
            string filePath = tb_file.PostedFile.FileName;
            string filename1 = Path.GetFileName(filePath);
            string ext = Path.GetExtension(filename1);
            string type = String.Empty;

            try
            {
                switch (ext)
                {
                    case ".pdf":
                        type = "application/pdf";
                        break;
                }

                if (type != String.Empty)
                {
                    connection();
                    Stream fs = tb_file.PostedFile.InputStream;
                    BinaryReader br = new BinaryReader(fs);
                    Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    //query = "insert into tb_contracts(Contract_No,Contract_Date,Objective,Effective_Date,Expire_Date,Remind_On,File_Name,File_Data,Recipient,Responder)" + " values ( @Contract_No, @Contract_Date, @Objective, @Effective_Date, @Expire_Date, @Remind_On, @File_Name, @File_Data,  @Recipient, @Responder)";

                    cmd = new SqlCommand("spAddContract", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("Contract_No", SqlDbType.NVarChar).Value = tb_contract_no.Text;
                    cmd.Parameters.Add("Contract_Date", SqlDbType.Date).Value = getDateOnly(tb_contract_date.Text);
                    cmd.Parameters.Add("Objective", SqlDbType.NVarChar).Value = tb_objective.Text;
                    cmd.Parameters.Add("Effective_Date", SqlDbType.Date).Value = getDateOnly(tb_effective.Text);
                    cmd.Parameters.Add("Expire_Date", SqlDbType.Date).Value = getDateOnly(tb_expire.Text);
                    cmd.Parameters.Add("Remind_On", SqlDbType.Int).Value = tb_remindon.Text;
                    cmd.Parameters.Add("File_Name", SqlDbType.NVarChar).Value = filename1;
                    cmd.Parameters.Add("File_Data", SqlDbType.Binary).Value = bytes;
                    cmd.Parameters.Add("Recipient", SqlDbType.NVarChar).Value = tb_recipient.Text;
                    cmd.Parameters.Add("Responder", SqlDbType.NVarChar).Value = tb_responder.Text;
                    cmd.Parameters.Add("Branch_Id", SqlDbType.Int).Value = ddlBranch.SelectedValue;
                    cmd.Parameters.Add("Contract_Type", SqlDbType.Int).Value = ddlType.SelectedValue;

                    cmd.ExecuteNonQuery();
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Add Success!');", true);
                    
                    clearControl();

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Error!');", true);

            }

        }
        private void updateToDB()
        {
            connection();
            cmd = new SqlCommand("spUpdateContractAllField", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            if (tb_file.HasFile)
            {
                string filePath = tb_file.PostedFile.FileName;
                string filename1 = Path.GetFileName(filePath);
                string ext = Path.GetExtension(filename1);
                string type = String.Empty;

                Stream fs = tb_file.PostedFile.InputStream;
                BinaryReader br = new BinaryReader(fs);
                Byte[] bytes = br.ReadBytes((Int32)fs.Length);

                cmd.Parameters.Add("@File_Name", SqlDbType.NVarChar).Value = filename1;
                cmd.Parameters.Add("@File_Data", SqlDbType.Binary).Value = bytes;
            }
            else
            {
                cmd.Parameters.Add("@File_Name", SqlDbType.NVarChar).Value = file_name;
                cmd.Parameters.Add("@File_Data", SqlDbType.Binary).Value = file_data;
            }

            cmd.Parameters.Add("@Contract_Id", SqlDbType.Int).Value = tb_contract_id.Text;
            cmd.Parameters.Add("@Contract_No", SqlDbType.NVarChar).Value = tb_contract_no.Text;
            cmd.Parameters.Add("@Contract_Date", SqlDbType.Date).Value = getDateOnly(tb_contract_date.Text);
            cmd.Parameters.Add("@Objective", SqlDbType.NVarChar).Value = tb_objective.Text;
            cmd.Parameters.Add("@Effective_Date", SqlDbType.Date).Value = getDateOnly(tb_effective.Text);
            cmd.Parameters.Add("@Expire_Date", SqlDbType.Date).Value = getDateOnly(tb_expire.Text);
            cmd.Parameters.Add("@Remind_On", SqlDbType.Int).Value = tb_remindon.Text;
            cmd.Parameters.Add("@Recipient", SqlDbType.NVarChar).Value = tb_recipient.Text;
            cmd.Parameters.Add("@Responder", SqlDbType.NVarChar).Value = tb_responder.Text;
            cmd.Parameters.Add("@isSent", SqlDbType.Bit).Value = false;
            cmd.Parameters.Add("@Branch_Id", SqlDbType.Int).Value = ddlBranch.SelectedValue;
            cmd.Parameters.Add("@Contract_Type", SqlDbType.Int).Value = ddlType.SelectedValue;

            try
            {

                cmd.ExecuteNonQuery();
                con.Close();
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Update Successfully');", true);
                btnAdd.Text = "Add";
                
                clearControl();


            }
            catch (Exception ex)
            {
                con.Close();
            }
        }
        private void connection()
        {
            constr = ConfigurationManager.ConnectionStrings["con"].ToString();
            con = new SqlConnection(constr);
            con.Open();
        }


        private void getDataFromDB(String search)
        {
            connection();
            cmd = new SqlCommand("spGetAllFiles", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Contract_No", search);
            adt = new SqlDataAdapter(cmd);
            ds = new DataSet();
            adt.Fill(ds);
            con.Close();
        }

        protected void btnGotoList_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/contract_list.aspx");
        }

        private void bindDataToDDLBranch(int selectedValue){
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

            }
            catch (Exception ex)
            {
                con.Close();
            }
        }
    }
}