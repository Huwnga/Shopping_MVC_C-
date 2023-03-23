using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using ShoppingOnline.localhost;
using ShoppingOnline.Models;

namespace ShoppingOnline.Controllers
{
    public class ProductsController : Controller
    {
        private WebService ws = new WebService();
        // GET: Products
        public ActionResult Index(int? CategoryID)
        {
            DataSet ds = new DataSet();

            List<Products> products = new List<Products>();

            ds = ws.GetProducts(CategoryID);
            products = Products.AddByDataSet(ds);

            ViewBag.Categories = GetAllCategory();
            ViewBag.DetailCols = GetAllColOfDetailInProducts();

            Session["search_CategoryID"] = null;
            Session["search_property"] = null;
            Session["search_value"] = null;

            return View(products);
        }

        [HttpPost]
        public ActionResult Index(FormCollection fr)
        {
            string value = fr["Value"];
            string categoryID = fr["Category.CategoryID"];
            string property = fr["Property"];

            Session["search_CategoryID"] = categoryID;
            Session["search_property"] = property;
            Session["search_value"] = value;

            DataSet ds = ws.Search(Convert.ToInt32(categoryID), property, value);
            List<Products> products = new List<Products>();
            products = Products.AddByDataSet(ds);
            
            ViewBag.Categories = GetAllCategory();
            ViewBag.DetailCols = GetAllColOfDetailInProducts();

            return View(products);
        }

        private List<Categories> GetAllCategory ()
        {
            DataSet ds = ws.GetAllCategories();
            List<Categories> categories = ConvertDataset.ToList<Categories>(ds);

            return categories;
        }

        private List<string> GetAllColOfDetailInProducts ()
        {
            DataSet ds = ws.GetAllColOfDetailInProducts();
            List<string> detailCols = new List<string>();
            DataRowCollection rows = ds.Tables[0].Rows;

            foreach (DataRow row in rows)
            {
                string s = row["detail_col"].ToString();

                if (!detailCols.Contains(s))
                {
                    detailCols.Add(s);
                }
            }

            return detailCols;
        }
    }
}
