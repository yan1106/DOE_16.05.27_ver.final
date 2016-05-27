using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using MySql.Data.MySqlClient;
using System.ComponentModel;
using System.Drawing;
using System.Web.SessionState;





public partial class EP_TRA_Level : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string keyitem = Request.QueryString["keyitem"];
        string stage = Request.QueryString["stage"];
                           
        string Version_Name = Request.QueryString["filename"];
             
        //Response.Write(keyitem + stage + Version_Name);
        receive_Lv(Version_Name, stage, keyitem);
        HttpContext.Current.Session["Version_Name"] = Version_Name;
        HttpContext.Current.Session["keyitem"] = keyitem;
        HttpContext.Current.Session["stage"] = stage;

    }

    protected Boolean jude_Lv(string filename, string stage, string keyitem,string md,string cate,string kp)
    {
       
        string sign = "1";
        string sql = "select * from npi_eptra_doe_lv where Lv_filename='" + filename +"' and Lv_stage ='" + stage + "' and Lv_Iiitems='" + keyitem  +
               "' and Lv_md='" + md + "' and Lv_cate='" + cate  + "' and Lv_kp='" + kp + "'";

        MySqlConnection MySqlConn = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString);
        MySqlConn.Open();

        MySqlCommand MySqlCmd = new MySqlCommand(sql, MySqlConn);
        MySqlDataReader mydr = MySqlCmd.ExecuteReader();
       


        while (mydr.Read())
        {
            if(mydr["Lv_filename"].ToString()=="")
            {
                sign = "1";
                
            }
            else
            {
                sign = "0";
                break;
            }
        }

        mydr.Close();
        MySqlConn.Close();


        if (sign == "1")
            return true;
        else
            return false;

    }



    protected String select_Lv(string filename, string stage, string keyitem, string md, string cate, string kp)
    {

        string Lv = "";
        string sql = "select Lv_filename from npi_eptra_doe_lv where Lv_filename='" + filename+"'and Lv_stage='" + stage + "' and Lv_Iiitems='" + keyitem + "'" +
               "' and Lv_md='" + md + "'" + "' and Lv_cate='" + cate + "'" + "' and Lv_kp='" + kp + "'";

        MySqlConnection MySqlConn = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString);
        MySqlConn.Open();

        MySqlCommand MySqlCmd = new MySqlCommand(sql, MySqlConn);
        MySqlDataReader mydr = MySqlCmd.ExecuteReader();

       while(mydr.Read())
        {
            Lv = mydr["Lv_filename"].ToString();
        }

        mydr.Close();
        MySqlConn.Close();


        return Lv;

    }




    protected void receive_Lv (string filename,string stage,string keyitem)
    {
        
        string SpeChar="";
        string md = "";
        string cate = "";
        string key = "";
       

    string str_sql = "select * from npi_ep_category where EP_Cate_Stage='" + stage + "' and EP_Cate_Iiitems='" + keyitem + "'";

        clsMySQL db = new clsMySQL(); //Connection MySQL
        clsMySQL.DBReply dr = db.QueryDS(str_sql);
        GridView1.DataSource = dr.dsDataSet.Tables[0].DefaultView;
        GridView1.DataBind();
        db.Close();

        



        for(int i=0;i<GridView1.Rows.Count;i++)
        {
            SpeChar = GridView1.Rows[i].Cells[0].Text;
            md = GridView1.Rows[i].Cells[1].Text;
            cate = GridView1.Rows[i].Cells[2].Text;
            key = GridView1.Rows[i].Cells[3].Text;


            if (jude_Lv(filename,stage,SpeChar,md,cate,key))
            {
                DropDownList  ddl_Lv = (DropDownList)GridView1.Rows[i].Cells[4].FindControl("Doe_Lv");
                ddl_Lv.Items.Add(new ListItem("Lv.3", "Lv.3"));
                ddl_Lv.Items.Add(new ListItem("Lv.4", "Lv.4"));
                ddl_Lv.Items.Add(new ListItem("Lv.5", "Lv.5"));
            }
           else
            {
                DropDownList ddl_Lv = (DropDownList)GridView1.Rows[i].Cells[4].FindControl("Doe_Lv");
                if (select_Lv(filename, stage, SpeChar, md, cate, key) == "Lv.3")
                {
                    ddl_Lv.Items.Add(new ListItem("Lv.3", "Lv.3"));
                    ddl_Lv.Items.Add(new ListItem("Lv.4", "Lv.4"));
                    ddl_Lv.Items.Add(new ListItem("Lv.5", "Lv.5"));
                }
                else if(select_Lv(filename, stage, SpeChar, md, cate, key) == "Lv.4")
                {
                    ddl_Lv.Items.Add(new ListItem("Lv.4", "Lv.4"));
                                    
                    ddl_Lv.Items.Add(new ListItem("Lv.5", "Lv.5"));
                    ddl_Lv.Items.Add(new ListItem("Lv.3", "Lv.3"));
                }
                else if(select_Lv(filename, stage, SpeChar, md, cate, key) == "Lv.5")
                {
                    ddl_Lv.Items.Add(new ListItem("Lv.5", "Lv.5"));
                    ddl_Lv.Items.Add(new ListItem("Lv.4", "Lv.4"));                   
                    ddl_Lv.Items.Add(new ListItem("Lv.3", "Lv.3"));
                }
            }


            


        }


        

        //test.Items.Remove(test.Items.FindByValue("Lv.4"));



    }





    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        string vn =Session["Version_Name"].ToString(); 
        string key = Session["keyitem"].ToString();
        string stage = Session["stage"].ToString();
        GridView1.PageIndex = e.NewPageIndex;
        receive_Lv(vn, stage, key);
    }

    protected void GridView1_PageIndexChanged(object sender, EventArgs e)
    {
        string vn = Session["Version_Name"].ToString();
        string key = Session["keyitem"].ToString();
        string stage = Session["stage"].ToString();
       
        receive_Lv(vn, stage, key);
    }




   




    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {

            case "Save":
                

                break;

            


        }
    }
}