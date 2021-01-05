﻿using DataManager.Library.Internal.DataAccess;
using DataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Library.DataAccess
{
    public class SaleData
    {
        public void SaveSale(SaleModel saleInfo, string cashierId)
        {
            List<SaleLineDBModel> details = new List<SaleLineDBModel>();
            ProductData productData = new ProductData();
            var taxRate = ConfigHelper.GetTaxRate() / 100;

            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleLineDBModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                var productInfo = productData.GetProductById(detail.ProductId);

                if (productInfo == null)
                {
                    throw new Exception($"The product Id of { detail.ProductId } could not be found in the database.");
                }

                detail.PurchasePrice = productInfo.RetailPrice * detail.Quantity;

                if (productInfo.IsTaxable)
                {
                    detail.Tax = (detail.PurchasePrice * taxRate);
                }

                details.Add(detail);
            }

            SaleDBModel sale = new SaleDBModel
            {
                SubTotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax),
                CashierId = cashierId
            };

            sale.Total = sale.SubTotal + sale.Tax;

            SqlDataAccess sql = new SqlDataAccess();
            sql.SaveData("dbo.spSale_Insert", sale, "RetailManagerDB");

            sale.Id = sql.LoadData<int, dynamic>("spSale_Lookup", new { sale.CashierId, sale.SaleDate }, "RetailManagerDB").FirstOrDefault();

            foreach (var item in details)
            {
                item.SaleId = sale.Id;
                sql.SaveData("dbo.spSaleLine_Insert", item, "RetailManagerDB");
            }
        }
    }
}
