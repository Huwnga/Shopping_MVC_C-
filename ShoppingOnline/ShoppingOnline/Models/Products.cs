using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace ShoppingOnline.Models
{
    public class Products
    {
        [Required]
        public int ProductID { get; set; }
        [Required]
        [StringLength(50)]
        public string ProductName { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public float Price { get; set; }
        public string Details { get; set; }
        [Required]
        public Categories Category { get; set; }

        public static List<Products> AddByDataSet (DataSet ds)
        {
            List<Products> products = new List<Products>();
            DataRowCollection rows = ds.Tables[0].Rows;

            foreach (DataRow row in rows)
            {
                Products p = ConvertDataset.AddByDataRow<Products>(row);

                if (!DBNull.Value.Equals(row["CategoryID"]))
                {
                    p.Category = ConvertDataset.AddByDataRow<Categories>(row);
                }

                products.Add(p);
            }

            return products;
        }
    }
}