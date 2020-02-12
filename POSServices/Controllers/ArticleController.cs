using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSServices.Models;
using POSServices.WebAPIModel;

namespace POSServices.Controllers
{
    [Route("api/Article")]
    [ApiController]
    public class ArticleController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public ArticleController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }


        [HttpGet]
        public List<Article> getArticle(String customerCode)
        {
            List<Article> allArticle = new List<Article>();
            try
            {

                Customer customer = _context.Customer.Where(c => c.CustId == customerCode).First();
                Store store = _context.Store.Where(c => c.Id == customer.StoreId).First();
                List<Article> listArtice = new List<Article>();
                var priceMaster = _context.PriceList;
                var itemMaster = _context.Item;
                var inventoryMaster = _context.InventoryLines.Where(c => c.WarehouseId == store.Code);
                var itemPrice =
                 from price in priceMaster
                 join item in itemMaster on price.ItemId equals item.ItemIdAlias
                 join inventoryLines in inventoryMaster on item.Id equals inventoryLines.ItemId
                 select new Article
                 {
                     id = item.Id,
                     price = price.SalesPrice,
                     gender = item.Gender,
                     articleId = item.ItemId,
                     articleIdAlias = item.ItemIdAlias,
                     articleName = item.Name,
                     brand = item.Brand,
                     color = item.Color,
                     department = item.Department,
                     departmentType = item.DepartmentType,
                     size = item.Size,
                     itemGroup = item.ItemGroup,
                     itemGroupDesc = item.ItemGroupDesc,
                     unit = "PCS",
                     isService = item.IsServiceItem.HasValue ? item.IsServiceItem.Value : false

                 };
                allArticle = itemPrice.ToList();

                //  List<InventoryTransaction> deliveryOrder = _context.InventoryTransaction.Where(c => c.WarehouseDestination == store.Code && c.TransactionTypeId == Config.RetailEnum.doTransaction && c.StatusId == Config.RetailEnum.doStatusPending).ToList();
                List<InventoryTransaction> deliveryOrder = _context.InventoryTransaction.Where(c => c.WarehouseDestination == store.Code && c.TransactionTypeId == Config.RetailEnum.doTransaction).ToList();
                if (deliveryOrder.Count > 0)
                {
                    for (int i = 0; i < deliveryOrder.Count; i++)
                    {
                        InventoryTransaction inventoryTransaction = deliveryOrder[i];
                        List<InventoryTransactionLines> list = _context.InventoryTransactionLines.Where(c => c.InventoryTransactionId == inventoryTransaction.Id).ToList();
                        for (int j = 0; j < list.Count; j++)
                        {
                            InventoryTransactionLines inventoryTransactionLines = list[j];
                            try
                            {

                                //check if exist in current item;

                                bool isItemExistInCurrent = allArticle.Any(c => c.articleIdAlias == list[j].ArticleId); //sebelum ny c.artileid //ganti itemprice.any jadi allitem.any
                                if (!isItemExistInCurrent)
                                {

                                    Item item = itemMaster.Where(c => c.ItemIdAlias == inventoryTransactionLines.ArticleId).First();// sbelum ny c.ItemId
                                    Article article = new Article();

                                    article.id = item.Id;
                                    if (priceMaster.Any(c => c.ItemId == item.ItemIdAlias))
                                    {
                                        article.price = priceMaster.Where(c => c.ItemId == item.ItemIdAlias).First().SalesPrice;
                                    }
                                    else
                                    {
                                        article.price = 0;
                                    }
                                    article.articleId = item.ItemId;
                                    article.articleIdAlias = item.ItemIdAlias;
                                    article.articleName = item.Name;
                                    article.brand = item.Brand;
                                    article.color = item.Color;
                                    article.gender = item.Gender;
                                    article.department = item.Department;
                                    article.itemGroup = item.ItemGroup;
                                    article.itemGroupDesc = item.ItemGroupDesc;
                                    article.departmentType = item.DepartmentType;
                                    article.size = item.Size;
                                    article.unit = "PCS";
                                    allArticle.Add(article);


                                }
                            }
                            catch { }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return allArticle;
        }
    }
}