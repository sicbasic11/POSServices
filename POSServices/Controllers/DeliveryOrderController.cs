using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using POSServices.Config;
using POSServices.Models;
using POSServices.WebAPIModel;

namespace POSServices.Controllers
{
    [Route("api/DeliveryOrder")]
    [ApiController]
    public class DeliveryOrderController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public DeliveryOrderController(DB_BIENSI_POSContext context)
        {
            _context = context;

        }
        //LIST
        private int indexttrans(int id, List<InventoryTransaction> indexlist)
        {
            int value = 0;
            for (int i = 0; i < indexlist.Count; i++)
            {
                if (indexlist[i].Id == id)
                {
                    value = i;
                    break;
                }
            }
            return value;
        }

        private int indexttransline(int id, List<InventoryTransactionLines> indexlist)
        {
            int value = 0;
            for (int i = 0; i < indexlist.Count; i++)
            {
                if (indexlist[i].Id == id)
                {
                    value = i;
                    break;
                }
            }
            return value;
        }

        private int indextitems(string id, List<Item> indexlist)
        {
            int value = 0;
            for (int i = 0; i < indexlist.Count; i++)
            {
                if (indexlist[i].ItemIdAlias == id)
                {
                    value = i;
                    break;
                }
            }
            return value;
        }

        //
        // POST: api/PostTransaction
        [HttpGet]
        public async Task<List<DeliveryOrder>> getDO(String StoreCode)
        {            
            List<DeliveryOrder> deliveryOrders = new List<DeliveryOrder>();

            List<InventoryTransaction> dos = _context.InventoryTransaction.Where(c => c.WarehouseDestination == StoreCode
            && c.TransactionTypeId == RetailEnum.doTransaction && c.ToBeConfirmed == null && c.StatusId == 0).OrderByDescending(c => c.Id).ToList();

            List<InventoryTransactionLines> listtransline = _context.InventoryTransactionLines.Where(c => dos.Any(d => d.Id == c.InventoryTransactionId)).ToList();
            List<Item> listitem = _context.Item.Where(c => listtransline.Any(d => d.ArticleId == c.ItemIdAlias)).ToList();
            for (int i = 0; i < dos.Count; i++)
            {
                // tambah ini
                InventoryTransaction inventory = dos[i];
                //Select For Header
                DeliveryOrder doObj = new DeliveryOrder
                {
                    date = DateTime.Now.ToString("yyyy-MM-dd"),
                    deliveryDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    id = inventory.Id,
                    deliveryOrderId = inventory.TransactionId,
                    SjFIsik = inventory.DeliveryOrderNumber,
                    deliveryTime = DateTime.Now.ToString("yyyy-MM-dd"),
                    time = DateTime.Now.ToString("hh:mm:dd"),
                    status = inventory.StatusId,
                    statusName = inventory.Status,
                    totalQty = inventory.TotalQty,
                    warehouseFrom = inventory.WarehouseOriginal,
                    warehouseTo = inventory.WarehouseDestination,
                    //add for pbmBpk
                    isPbmPbk = (inventory.InforBypass.HasValue) ? inventory.InforBypass.Value : false

                };

                List<DeliveryOrderLine> deliveryOrderLines = new List<DeliveryOrderLine>();
                List<InventoryTransactionLines> dosLines = listtransline.Where(c => c.InventoryTransactionId == inventory.Id).ToList();
                var doLines = from invenlines in dosLines
                              join items in listitem
                              on invenlines.ArticleId equals items.ItemIdAlias
                              select new DeliveryOrderLine
                              {
                                  id = invenlines.Id,
                                  article = new Article
                                  {
                                      articleId = items.ItemId,
                                      articleIdAlias = items.ItemIdAlias,
                                      articleName = invenlines.ArticleName
                                  },
                                  articleIdFk = items.Id,
                                  deliveryOrderIdFk = inventory.Id,
                                  deliveryOrderId = inventory.TransactionId,
                                  qtyDeliver = invenlines.Qty,
                                  qtyReceive = invenlines.RecieveQty,
                                  amount = invenlines.ValueSalesPrice,
                                  packingNumber = invenlines.PackingNumber
                              };
                deliveryOrderLines = doLines.ToList();
                doObj.totalAmount = deliveryOrderLines.Sum(d => d.amount);
                doObj.deliveryOrderLines = deliveryOrderLines;
                doObj.totalQty = inventory.TotalQty;
                deliveryOrders.Add(doObj);

            }
            return deliveryOrders;
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] DeliveryOrder transactionApi)
        {
            //add for log
            try
            {
                LogRecord log = new LogRecord();
                log.TimeStamp = DateTime.Now;

                log.Tag = "Pre Confirm DO";

                log.Message = JsonConvert.SerializeObject(transactionApi);
                log.TransactionId = transactionApi.deliveryOrderId;
                _context.LogRecord.Add(log);
                //   _context.SaveChanges();
            }
            catch
            {

            }
            //end log

            APIResponse response = new APIResponse();
            try
            {

                bool statusConfirmed = _context.InventoryTransaction.Any(c => c.TransactionId == transactionApi.deliveryOrderId
                && c.Status == "Confirmed");
                if (statusConfirmed)
                {
                    response.code = "0";
                    response.message = "DO has been confirmed";
                    return Ok(response);
                }

                bool exsist = await _context.InventoryTransactionLines.AnyAsync(x => x.Urridn == transactionApi.deliveryOrderId);
                if (!exsist)
                {

                    Models.InventoryTransaction transaction = await _context.InventoryTransaction.Where(c => c.TransactionId == transactionApi.deliveryOrderId).FirstAsync();
                    transaction.Status = "Confirmed";
                    transaction.StatusId = RetailEnum.doStatusConfirmed;
                    transaction.Id = transaction.Id;
                    transaction.SyncDate = DateTime.Now;   
                    transaction.EmployeeCode = transactionApi.employeeId;
                    transaction.EmployeeName = transactionApi.employeeName;
                    
                    try
                    {                        
                        transaction.RequestDeliveryDate = DateTime.ParseExact(transactionApi.deliveryTime, "yyyy-MM-dd", CultureInfo.InvariantCulture); // MPOS
                    }
                    catch
                    {                        
                        //POS                        
                        transaction.RequestDeliveryDate = DateTime.Now;
                    }
                    //log record

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    _context.InventoryTransaction.Update(transaction);
                    _context.SaveChanges();


                    List<Article> listar = new List<Article>();
                    //save Lines 
                    for (int i = 0; i < transactionApi.deliveryOrderLines.Count; i++)
                    {

                        var artikelaidifk = _context.Item.Where(x => x.Id == transactionApi.deliveryOrderLines[i].articleIdFk).First().ItemIdAlias;
                        var itemanme = _context.Item.Where(x => x.Id == transactionApi.deliveryOrderLines[i].articleIdFk).First().Name;
                        //add  and remarkby frank 
                        // 7 april 2019
                        // untuk case yang double lines tidak ada packing number, referencenya beredsarkan id uniq di DO lins

                        /*Models.InventoryTransactionLines transactionLines = await _context.InventoryTransactionLines.Where(c => c.InventoryTransactionId == transaction.Id &&
                        c.ArticleId == artikelaidifk
                        && c.PackingNumber == transactionApi.deliveryOrderLines[i].packingNumber).FirstAsync();
                        */
                        Models.InventoryTransactionLines transactionLines = _context.InventoryTransactionLines.Find(transactionApi.deliveryOrderLines[i].id);
                        //end of add end remark by frank

                        transactionLines.ArticleName = itemanme;
                        //reamakr by frank 1 oktobr
                        // coa request
                        //   bool check = listar.Any(c => c.id == transactionApi.deliveryOrderLines[i].articleIdFk && c.articleIdAlias == transactionApi.deliveryOrderLines[i].packingNumber);
                        //   if (check)
                        //   {
                        //       transactionLines.RecieveQty = 0;
                        //   }
                        //   else
                        //  {
                        transactionLines.RecieveQty = transactionApi.deliveryOrderLines[i].qtyReceive;
                        //  }
                        //end of remark 1 oktoer


                        Article dupli = new Article
                        {
                            id = transactionApi.deliveryOrderLines[i].articleIdFk,
                            articleIdAlias = transactionApi.deliveryOrderLines[i].packingNumber
                        };
                        listar.Add(dupli);

                        _context.InventoryTransactionLines.Update(transactionLines);
                        _context.SaveChanges();
                    }

                    response.code = "1";
                    response.message = "Sucess Add Data";
                    DateTime maret20 = DateTime.ParseExact("2019-02-28", "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    if (transaction.TransactionDate >= maret20)
                    {
                        if (transaction.InforBypass == true)
                        {


                        }
                        else
                        {
                            //   WebAPIInforController.InforAPIController inforAPIController = new WebAPIInforController.InforAPIController(_context);
                            //   inforAPIController.RecieveRequestOrder(transactionApi, transaction.Id).Wait();
                        }

                        if (transaction.StatusId == RetailEnum.doStatusConfirmed)
                        {
                            this.insertAndCalculateDO(transactionApi);
                        }
                    }

                }


            }
            catch (Exception ex)
            {
                response.code = "0";
                response.message = ex.ToString();
            }

            return Ok(response);            
        }

        private void insertAndCalculateDO(DeliveryOrder transactionApi)
        {
            for (int i = 0; i < transactionApi.deliveryOrderLines.Count; i++)
            {
                try
                {
                    InventoryLines inventoryLines = new InventoryLines();
                    bool exist = _context.InventoryLines.Any(c => c.WarehouseId == transactionApi.warehouseTo && c.ItemId == transactionApi.deliveryOrderLines[i].articleIdFk);
                    if (exist)
                    {
                        inventoryLines = _context.InventoryLines.Where(c => c.WarehouseId == transactionApi.warehouseTo && c.ItemId == transactionApi.deliveryOrderLines[i].articleIdFk).First();
                    }
                    else
                    {
                        inventoryLines = this.createdInventory(transactionApi, i);
                    }

                    //Remark sementara
                    //InventoryTransactionLines transaction = new InventoryTransactionLines();
                    //transaction.TransactionTypeId = RetailEnum.DeliveryOrder;
                    //transaction.TransactionTypeName = "DeliveryOrder";
                    //transaction.TransRefId = transactionApi.deliveryOrderId;
                    //transaction.Qty = transactionApi.deliveryOrderLines[i].qtyReceive;
                    //transaction.TransactionLinesId = inventoryLines.Id;
                    //transaction.TransactionDate = DateTime.Now;//DateTime.ParseExact(transactionApi.date, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                    //_context.Add(transaction);
                    //_context.SaveChanges();

                    ////update qty
                    //inventoryLines.Qty = _context.InventoryTransactionLines.
                    //                     Where(c => c.InventoryTransactionId == inventoryLines.Id)
                    //                    .Select(c => c.Qty)
                    //                    .DefaultIfEmpty()
                    //                    .Sum();
                    //_context.InventoryLines.Update(inventoryLines);
                    //_context.SaveChanges();
                }
                catch (Exception ex)
                {

                }
            }
        }

        private InventoryLines createdInventory(DeliveryOrder transactionApi, int i)
        {
            InventoryLines inventory = new InventoryLines();
            inventory.WarehouseId = transactionApi.warehouseTo;
            inventory.Qty = 0;
            inventory.ItemId = transactionApi.deliveryOrderLines[i].articleIdFk;
            _context.Add(inventory);
            _context.SaveChanges();
            return inventory;
        }

        [HttpPost]
        public IActionResult Reject([FromBody] DeliveryOrder transactionApi, string status)
        {
            return Ok();
        }
    }
}