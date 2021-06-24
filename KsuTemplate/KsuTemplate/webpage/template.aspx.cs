﻿using Aspose.Words;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using Aspose.Words.MailMerging;
using KsuTemplate.App_Code;
using System.Data.SqlClient;

namespace KsuTemplate.webpage
{
    public partial class template : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["Username"] != null)
            {
                if (!Authenticate())
                {
                    btnDoc.Visible = false;
                    btnPdf.Visible = false;
                    lblData.Text = "Please fill your profile information first!! ";
                }
                else
                {
                    btnDoc.Visible=true;
                    btnPdf.Visible = true;
                    
                }

                
            }
        }

        protected Document mailMergeEffect()
        {
            String dataDir = "~\\template\\";
            String fileName = "effective.docx";
            // Open an existing document.
            Document doc = new Document(Server.MapPath(dataDir + fileName));

            // Trim trailing and leading whitespaces mail merge values
            doc.MailMerge.TrimWhitespaces = false;

            // Fill the fields in the document with template data.
            String[] effectTemplate = new string[] { "Intern", "Id", "Mobile", "Telephone", "Email",
                                 "IT", "CS", "SWE", "IS",
                                 "NS", "DM", "WM", "CyberS", "DS", "NIOT",
                                 "Institution", "Address", "Department", "TrainingSupervisor",
                                 "Position", "InstitutionTelephone", "InstitutionMobile",
                                 "InstitutionEmail", "StartingDate"};

            object[] effectData = effectNoticeData();

            doc.MailMerge.Execute(effectTemplate, effectData);

            return doc;

        }

        protected object[] effectNoticeData()
        {


            //intern info
            CRUD myCrud = new CRUD();
            string mySql = @"SELECT internId, intern, majorId
                             , trackId, id, internMobile, telephone
                            , internEmail, startDate, endDate, userName
                             FROM intern
                             WHERE userName = @userName";

            Dictionary<String, object> myPara = new Dictionary<String, object>();
            myPara.Add("@userName", Session["Username"]);
            SqlDataReader dr = myCrud.getDrPassSql(mySql, myPara);
            String intern="";
            String id = "";
            String internMobile = "";
            String telephone = "";
            String internEmail = "";
            int majorId = -1;
            int trackId = -1;
            String endDate = "";
            String startDate = "";

            //major
            String IT = "False";
            String CS= "False";
            String SWE = "False";
            String IS = "False";

            //Track
            String NS = "False";
            String DM = "False";
            String WM = "False";
            String CyberS = "False";
            String DS = "False";
            String NIOT = "False";

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    intern = dr["intern"].ToString();
                    id = dr["id"].ToString();
                    internMobile = dr["internMobile"].ToString();
                    telephone = dr["telephone"].ToString();
                    internEmail = dr["internEmail"].ToString();
                    majorId = int.Parse(dr["majorId"].ToString());

                    if (majorId == 1)
                    {
                        IT= "True";
                        trackId = int.Parse(dr["trackId"].ToString());
                        switch (trackId)
                        {
                            case 4:
                                NS = "True";
                                break;

                            case 5:
                                DM = "True";
                                break;

                            case 6:
                                WM = "True";
                                break;

                            case 7:
                                CyberS = "True";
                                break;
                            case 8:
                                DS = "True";
                                break;

                            case 9:
                                NIOT = "True";
                                break;
                        }
                    }else
                    {
                        switch (majorId)
                        {
                            case 2:
                                CS = "True";
                                break;

                            case 3:
                                SWE = "True";
                                break;

                            case 4:
                                IS = "True";
                                break;
                        }
                    }

                    endDate = Convert.ToDateTime(dr["endDate"]).ToString("MM-dd-yyyy");
                    startDate = Convert.ToDateTime(dr["startDate"]).ToString("MM-dd-yyyy");
                    break;
                }
            }


            //institution info
            CRUD instituationCrud = new CRUD();
            string instituationSql = @"SELECT institution,address,departmentId,supervisorName,
                                        supervisorPosition,supervisorTelephone 
                                        ,supervisorEmail,supervisorMobile
                                          FROM institution
                                          WHERE institutionId=@institutionId";

            Dictionary<String, object> instituationPara = new Dictionary<String, object>();
            instituationPara.Add("@institutionId", 4);
            SqlDataReader instituationDr = instituationCrud.getDrPassSql(instituationSql, instituationPara);
            String institution = "";
            String address = "";
            int departmentId = 1;
            String supervisorName = "";
            String supervisorPosition = "";
            String supervisorTelephone = "";
            String supervisorEmail = "";
            String supervisorMobile = "";

            if (instituationDr.HasRows)
            {
                while (instituationDr.Read())
                {
                    institution = instituationDr["institution"].ToString();
                    address = instituationDr["address"].ToString();
                    departmentId = int.Parse(instituationDr["departmentId"].ToString());
                    supervisorName = instituationDr["supervisorName"].ToString();
                    supervisorPosition = instituationDr["supervisorPosition"].ToString();
                    supervisorTelephone = instituationDr["supervisorTelephone"].ToString();
                    supervisorEmail = instituationDr["supervisorEmail"].ToString();
                    supervisorMobile = instituationDr["supervisorMobile"].ToString();
                    break;
                }
            }

            //Department info
            //CRUD departmentCrud = new CRUD();
            //string departmentSql = @"SELECT department
            //                          FROM department
            //                          WHERE departmentId=@departmentId";

            //Dictionary<String, object> departmentPara = new Dictionary<String, object>();
            //departmentPara.Add("@departmentId", 1);
            //SqlDataReader departmentDr = departmentCrud.getDrPassSql(departmentSql, departmentPara);
            //String department = departmentDr["department"].ToString();

            CRUD departmentCrud = new CRUD();
            string departmentSql = @"SELECT department
                                      FROM department
                                      WHERE departmentId=@departmentId";

            Dictionary<String, object> departmentPara = new Dictionary<String, object>();
            departmentPara.Add("@departmentId", 1);
            SqlDataReader departmentDr = departmentCrud.getDrPassSql(departmentSql, departmentPara);
            String department = "";

            if (departmentDr.HasRows)
            {
                while (departmentDr.Read())
                {
                    department = departmentDr["department"].ToString();
                    break;
                }
            }
            //String department = "department";

            object[] effectData = new object[]{
                intern, id, internMobile, telephone, internEmail,
                              IT, CS, SWE, IS,
                               NS, DM, WM, CyberS, DS, NIOT,
                               institution,address, department, supervisorName,
                                 supervisorPosition, supervisorTelephone, supervisorMobile,
                                 supervisorEmail, startDate };
            return effectData;
        }

        protected void saveAsDoc(Document doc, String fileName)
        {
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            // Send the document in Word format to the client browser with an option to save to disk or open inside the current browser.
            doc.Save(desktop + fileName);

        }

        protected void saveAsPdf(Document doc, String fileName)
        {
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            doc.Save(desktop+ fileName, SaveFormat.Pdf);
        }

        protected bool Authenticate()
        {
            string mySql = @"SELECT *
                              FROM intern
                              WHERE userName = @user";
            CRUD myCrud = new CRUD();
            Dictionary<String, object> myPara = new Dictionary<String, object>();
            myPara.Add("@user", Session["Username"]);
            bool userFound = myCrud.authenticateUser(mySql, myPara); // pass the sql and the dic para
            return userFound;
        }

        protected void btnDoc_Click(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("\\Account\\Login.aspx");
            }
            Document doc = mailMergeEffect();
            saveAsDoc(doc, "\\effectivejsdkh.docx");
        }

        protected void btnPdf_Click(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("\\Account\\Login.aspx");
            }
            Document doc = mailMergeEffect();
            saveAsDoc(doc, "\\effective.Pdf");

        }
    }
}