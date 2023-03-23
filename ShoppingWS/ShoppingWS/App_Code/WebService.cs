using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService
{
    private string strconn = ConfigurationManager.ConnectionStrings["ShoppingWS"].ConnectionString;
    public WebService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public DataSet GetAllCategories()
    {
        string Sql = "SELECT * FROM Categories";
        DataSet ds = QuerySelectDataBase(Sql, "Categories");

        return ds;
    }

    [WebMethod]
    public DataSet GetProducts(int? CategoryID)
    {
        string Sql = "SELECT * FROM Products ";

        if (CategoryID != null)
        {
            Sql += "WHERE CategoryID=" + CategoryID;
        }

        DataSet ds = QuerySelectDataBase(Sql, "Products");

        return ds;
    }

    [WebMethod]
    public DataSet GetAllColOfDetailInProducts ()
    {
        string Sql = "SELECT detail.value('local-name(.)','nvarchar(max)') AS detail_col "
                   + "FROM Products AS p "
                   + "CROSS APPLY p.Details.nodes('/*') AS XMLData(detail) ";

        DataSet ds = QuerySelectDataBase(Sql, "Products");

        return ds;
    }

    [WebMethod]
    public DataSet Search(int CategoryID, string Property, string Value)
    {
        string Sql = "SELECT * FROM Products WHERE 1=1";

        if (CategoryID != null)
        {
            Sql += " AND CategoryID=" + CategoryID;
        }

        Sql += " AND Details.value('(/"+ Property + ")[1]','nvarchar(50)') LIKE N'%"+ Value + "%'";

        DataSet ds = QuerySelectDataBase(Sql, "Products");

        return ds;
    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }

    private DataSet QuerySelectDataBase(string Sql, string table)
    {
        SqlConnection conn = new SqlConnection(strconn);

        SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
        DataSet ds = new DataSet();
        da.Fill(ds, table);

        return ds;
    }

    private SqlCommand QueryChangeDataBase(string Sql)
    {
        SqlConnection conn = new SqlConnection(strconn);

        if (conn.State == ConnectionState.Closed)
        {
            conn.Open();
        }

        SqlCommand cmd = new SqlCommand(Sql, conn);

        return cmd;
    }
}
