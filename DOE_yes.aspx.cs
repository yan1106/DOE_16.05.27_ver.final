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
using System.Text;
using System.Web.SessionState;
using System.Web.Script.Serialization;
public partial class DOE_yes : System.Web.UI.Page
{
    [System.Web.Services.WebMethod]

    
    protected void Page_Load(object sender, EventArgs e)
    {

        string keyitem = Request.QueryString["keyitem"];
        string stage = Request.QueryString["stage"];
        //string SpeChar = Request.QueryString["SpeChar"];
        string Version_Name = Request.QueryString["File_Name"];

        //Response.Write(Version_Name);
        HttpContext.Current.Session["Version_Name"] = Version_Name;
        rec_categorydata(keyitem, stage);


    }

    [System.Web.Services.WebMethod]
    public static string[] GetCustomer()
    {
        List<string> Customer = new List<string>();

        string strSQL = "select EP_Cate_KP from npi_ep_category where EP_Cate_Stage='PI1' and  EP_Cate_SpeChar='PI Thickness(ASI)' and EP_Cate_Iiitems='Wafer PSV type / Thickness'";


        clsMySQL db = new clsMySQL();
        foreach (DataRow dr in db.QueryDataSet(strSQL).Tables[0].Rows)
        {
            //customers.Add(string.Format("{0},{1}", dr["new_customer"], dr["new_device"]));
            Customer.Add(string.Format("{0}", dr["EP_Cate_KP"]));
        }

        return Customer.ToArray();

    }


    public static string GetCount(string count)
    {
        return count;
    }

    protected void count_categorydata()
    {
        string doe_str = "select count(*) from npi_ep_category where EP_Cate_Stage='PI1' and EP_Cate_Iiitems = 'PI Thickness (um)' and EP_Cate_SpeChar='PI CD' ";
        string count = "";

        MySqlConnection MySqlConn = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString);
        MySqlConn.Open();

        MySqlCommand MySqlCmd = new MySqlCommand(doe_str, MySqlConn);
        MySqlDataReader mydr = MySqlCmd.ExecuteReader();

        while (mydr.Read())
        {
            count = mydr["count(*)"].ToString();
        }

        string strScript = string.Format("<script language='javascript'>createtable(" + count + ");</script>");
        Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", strScript);


    }

    public void rec_categorydata(string keyitem, string stage)
    {
        //string doe_str = "select * from npi_ep_category where EP_Cate_Stage='PI1' and  EP_Cate_SpeChar='PI Thickness(ASI)' and EP_Cate_Iiitems='Wafer PSV type / Thickness' ";
        string doe_str = "select * from npi_ep_category where EP_Cate_Stage=" + stage +  " and EP_Cate_Iiitems=" + keyitem+"";
        string count = "";
        string name = Session["Version_Name"].ToString();
        int num = 0;
        string[] category_kp = new string[50];
        List<string> str_stage = new List<string>();
        List<string> str_kp = new List<string>();
        List<string> str_md = new List<string>();
        List<string> str_cate = new List<string>();
        List<string> str_SpeChar = new List<string>();
        string kp = "";


        MySqlConnection MySqlConn = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString);
        MySqlConn.Open();

        MySqlCommand MySqlCmd = new MySqlCommand(doe_str, MySqlConn);
        MySqlDataReader mydr = MySqlCmd.ExecuteReader();


        while (mydr.Read())
        {
            str_stage.Add("'" + (String)mydr["EP_Cate_Stage"] + "'");
            str_SpeChar.Add("'" + (String)mydr["EP_Cate_SpeChar"] + "'");
            str_md.Add("'" + (String)mydr["EP_Cate_Md"] + "'");
            str_cate.Add("'" + (String)mydr["EP_Cate_Cate"] + "'");
            str_kp.Add("'" + (String)mydr["EP_Cate_KP"] + "'");


            num++;
        }
        //Response.Write(num);
        StringBuilder sb = new StringBuilder(); //using System.Text;
        sb.Append("<script>");
        sb.Append(" var array_stage = new Array; var array_SpeChar = new Array;var array_md = new Array;var array_cate = new Array;var array_kp = new Array; var row_count = 0; var version_name;");



        foreach (string str in str_stage)
        {
            sb.Append("array_stage.push(" + str + ");");
        }
        foreach (string str in str_SpeChar)
        {
            sb.Append("array_SpeChar.push(" + str + ");");
        }
        foreach (string str in str_md)
        {
            sb.Append("array_md.push(" + str + ");");
        }
        foreach (string str in str_cate)
        {
            sb.Append("array_cate.push(" + str + ");");
        } foreach (string str in str_kp)
        {
            sb.Append("array_kp.push(" + str + ");");
        }
        sb.Append("row_count=" + num + ";");
        sb.Append("version_name=" + name + ";");
        //sb.Append("createtable(" + num + ")");
        sb.Append("</script>");

        ClientScript.RegisterStartupScript(this.GetType(), "TestArrayScript", sb.ToString());
        string strScript = string.Format("<script language='javascript'>createtable(" + num + ");</script>");
        Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", strScript);


    }




    protected void Button1_Click(object sender, EventArgs e)
    {
        string gg = "'tt'";
        string strScript = string.Format("<script language='javascript'>createtable(20," + gg + ");</script>");
        Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", strScript);
        //createtable
        //Page.RegisterClientScriptBlock("Pass", "<script>createtable(20" +  ",'" + gg + "');</script>");

    }
    [System.Web.Services.WebMethod]
    public static string test_pass(string str)
    {
         List<string> DOE_list = new List<string>();
        clsMySQL db = new clsMySQL();
        //string File_Name = .Session["Version_Name"].ToString();
        string[] str_list = str.Split('|');
        for(int i=0;i<str_list.Length;i++)
        {
            DOE_list.Add(str_list[i]);
        }


        String insert_DOE = string.Format("insert into npi_eptra_doe" +
                                      "(eptra_filename,doe_stage,doe_SpeChar,doe_md," +
                                    "doe_cate,doe_kp,doe_legs_1,doe_legs_2,doe_wafer," +
                                    "doe_wt_dm,doe_wt_pc,doe_wt_live,doe_attr,doe_note," +
                                    "doe_lot,doe_duedate,doe_result)values" +
                                    "('{0}','{1}','{2}'," +
                                     "'{3}','{4}','{5}','{6}','{7}'," +
                                     "'{8}','{9}','{10}','{11}','{12}'," +
                                     "'{13}','{14}','{15}','{16}')"
                                     , DOE_list[0], DOE_list[1], DOE_list[2], DOE_list[3],
                                     DOE_list[4], DOE_list[5], DOE_list[6], DOE_list[7],
                                     DOE_list[8], DOE_list[9], DOE_list[10], DOE_list[11],
                                     DOE_list[12], DOE_list[13], DOE_list[14], DOE_list[15],"bruno");

        try
        {
            if(db.QueryExecuteNonQuery(insert_DOE))
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }



         
    }


   public void Save_DOE_Data(List<string> DOE_List)
    {

    }

    protected void Button1_Click1(object sender, EventArgs e)
    {
        //Label1.Text = Session["123"].ToString();
        //string temp = Session["123"].ToString();
        //Response.Write(temp);
    }
}