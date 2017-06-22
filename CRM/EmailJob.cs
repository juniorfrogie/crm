using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using Quartz;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;

namespace CRM
{
    public class EmailJob : IJob
    {
        private string constr;
        private SqlConnection con;
        private DataSet ds = new DataSet();
        private SqlCommand cmd;

        public void Execute(IJobExecutionContext context)
        {
            List<File_Model> exp_contract = getExpireData();
            for (int i = 0; i < exp_contract.Count; i++)
            {
                sendMail(exp_contract[i]);
            }
        }

        private void connection()
        {
            constr = ConfigurationManager.ConnectionStrings["con"].ToString();
            con = new SqlConnection(constr);
            con.Open();
        }

        private bool IsValidEmailId(string InputEmail)
        {

            //Regex To validate Email Address

            Regex regex = new Regex(@"\w+([-+.']\w+)*@cambodiapostbank\.com$");
            Match match = regex.Match(InputEmail);
            
            if (match.Success)
                return true; 
            else
                return false;

        }

        private List<File_Model> getExpireData()
        {
            DateTime currentDate = DateTime.Today;
            connection();
            List<File_Model> list = new List<File_Model>();
            cmd = new SqlCommand("spGetAllFiles", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Contract_No", "");
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                if ((Convert.ToDateTime(sdr["Expire_Date"]) - DateTime.Today).TotalDays < Convert.ToInt16(sdr["Remind_On"]) && (Convert.ToBoolean(sdr["isSent"]) == false) && Convert.ToChar(sdr["Contract_Type"]) == '0')
                {

                    File_Model file = new File_Model();
                    file.contract_Id = Convert.ToInt16(sdr["Contract_Id"]);
                    file.contract_No = Convert.ToString(sdr["Contract_No"]);
                    file.contract_Date = Convert.ToDateTime(sdr["Contract_Date"]);
                    file.objective = Convert.ToString(sdr["Objective"]);
                    file.effective_Date = Convert.ToDateTime(sdr["Effective_Date"]);
                    file.expire_Date = Convert.ToDateTime(sdr["Expire_Date"]);
                    file.remindOn = Convert.ToInt16(sdr["Remind_On"]);
                    file.recipient = Convert.ToString(sdr["Recipient"]);
                    file.responder = Convert.ToString(sdr["Responder"]);
                    file.file_Name = Convert.ToString(sdr["File_Name"]);
                    file.file_Data = (byte[])sdr["File_Data"];
                    list.Add(file);
                }
            }
            sdr.Close();
            con.Close();
            return list;

        }

        private void sendMail(File_Model file)
        {
            ServicePointManager.ServerCertificateValidationCallback =
            delegate(object s, X509Certificate certificate,
                     X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };

            try
            {
                SmtpClient client = new SmtpClient();
                client.Port = 25;
                client.Host = "webmail.cambodiapostbank.com";
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(prepareMail(file));
                updateIsSent(file.contract_Id);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);   //Should print stacktrace + details of inner exception
                
                if (ex.InnerException != null)
                {
                    Console.WriteLine("InnerException is: {0}", ex.InnerException);
                }
            }

        }


        private MailMessage prepareMail(File_Model file)
        {
            MailMessage mailMessage = new MailMessage();

            foreach (var address in file.responder.Split(new[] { ";",","," " }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (IsValidEmailId(address))
                {
                    mailMessage.To.Add(address);
                }
            }
            foreach (var address in file.recipient.Split(new[] { ";",","," " }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (IsValidEmailId(address))
                {
                    mailMessage.CC.Add(address);
                }
            }

            mailMessage.From = new MailAddress("tivea.touch@cambodiapostbank.com");
            mailMessage.Subject = "Contract Renewal Announcement";
            mailMessage.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
            //string htmlBody = "Dear HIT, <br/><br/>Please kindly authorize";
            string htmlBody = "Dear Sir or Madam,<br/><br/>Please be informed that this contract or agreement is going to be expired on " + file.expire_Date.ToShortDateString() + ". Please renew the contract or agreement before the deadline. <br/><br/>Regards, <br/> Legal & Compliance Department";
            mailMessage.Attachments.Add(new Attachment(new MemoryStream(file.file_Data, false), file.file_Name, "application/pdf"));
            System.Net.Mail.AlternateView plainView = System.Net.Mail.AlternateView.CreateAlternateViewFromString
            (System.Text.RegularExpressions.Regex.Replace(htmlBody, @"<(.|\n)*?>", string.Empty), null, "text/plain");
            System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(htmlBody, null, "text/html");
            mailMessage.AlternateViews.Add(plainView);
            mailMessage.AlternateViews.Add(htmlView);
            mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            return mailMessage;
        }

        private void updateIsSent(int Contract_Id)
        {
            try
            {
                connection();
                cmd = new SqlCommand("spUpdateIsSent", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@Contract_Id", SqlDbType.Int).Value = Contract_Id;
                cmd.Parameters.Add("@isSent", SqlDbType.Bit).Value = true;
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
            }
        }
        
    }
}