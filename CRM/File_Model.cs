using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM
{
    public class File_Model
    {
        private int Contract_Id;

        public int contract_Id
        {
            get { return Contract_Id; }
            set { Contract_Id = value; }
        }

        private String Contract_No;

        public String contract_No
        {
            get { return Contract_No; }
            set { Contract_No = value; }
        }
        private DateTime Contract_Date;

        public DateTime contract_Date
        {
            get { return Contract_Date; }
            set { Contract_Date = value; }
        }
        private String Objective;

        public String objective
        {
            get { return Objective; }
            set { Objective = value; }
        }
        private DateTime Effective_Date;

        public DateTime effective_Date
        {
            get { return Effective_Date; }
            set { Effective_Date = value; }
        }
        private DateTime Expire_Date;

        public DateTime expire_Date
        {
            get { return Expire_Date; }
            set { Expire_Date = value; }
        }
        private String File_Name;

        public String file_Name
        {
            get { return File_Name; }
            set { File_Name = value; }
        }
        private Byte[] File_Data;

        public Byte[] file_Data
        {
            get { return File_Data; }
            set { File_Data = value; }
        }
        private String Recipient;

        public String recipient
        {
            get { return Recipient; }
            set { Recipient = value; }
        }
        private String Responder;

        public String responder
        {
            get { return Responder; }
            set { Responder = value; }
        }

        private int RemindOn;

        public int remindOn
        {
            get { return RemindOn; }
            set { RemindOn = value; }
        }
        private bool IsSent;

        public bool issSent
        {
            get { return IsSent; }
            set { IsSent = value; }
        }

        private int Branch_Id;

        public int branch_Id
        {
            get { return Branch_Id; }
            set { Branch_Id = value; }
        }

        private int Contract_Type;

        public int contract_Type
        {
            get { return Contract_Type; }
            set { Contract_Type = value; }
        }
        
    }
}