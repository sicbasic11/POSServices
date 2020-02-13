using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSServices.Data;
using POSServices.Models;
using static POSServices.Models.HO_MsgModel;

namespace POSServices.WebAPIBackendController
{
    [Route("api/DiscountSetup")]
    [ApiController]
    public class DiscountSetupController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public DiscountSetupController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> getDiscountSetup()
        {
            try
            {
                var discountSetup = (from ds in _context.DiscountSetup
                                     select new
                                     {
                                         DiscountCode = ds.DiscountCode,
                                         DiscountCategory = ds.DiscountCategory,
                                         DiscountName = ds.DiscountName,
                                         DiscountType = ds.DiscountType,
                                         CustomerGroupId = ds.CustomerGroupId,
                                         StartDate = ds.StartDate,
                                         EndDate = ds.EndDate,
                                         Status = ds.Status,
                                         DiscountCash = ds.DiscountCash,
                                         DiscountPercent = ds.DiscountPercent,
                                         QtyMin = ds.QtyMin,
                                         QtyMax = ds.QtyMax,
                                         AmountMin = ds.AmountMin,
                                         AmountMax = ds.AmountMax,
                                         ApprovedDate = ds.ApprovedDate,
                                         Multi = ds.Multi,
                                         TableCode = ds.TableCode
                                     }).ToList();

                return Json(new[] { discountSetup });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "500",
                    message = ex.ToString()
                });
            }            
        }

        [HttpPost("Add")]
        public async Task<IActionResult> create(discountSetupList discSetupList)
        {
            try
            {
                List<DiscountSetup> list = discSetupList.discounts;

                for (int i = 0; i < list.Count; i++)
                {
                    DiscountSetup discSetup = new DiscountSetup();
                    discSetup.DiscountCode = list[i].DiscountCode;
                    discSetup.DiscountName = list[i].DiscountName;
                    discSetup.DiscountCategory = list[i].DiscountCategory;
                    discSetup.CustomerGroupId = list[i].CustomerGroupId;
                    discSetup.DiscountCash = list[i].DiscountCash;
                    discSetup.DiscountPercent = list[i].DiscountPercent;
                    discSetup.DiscountType = list[i].DiscountType;
                    discSetup.AmountMin = list[i].AmountMin;
                    discSetup.AmountMax = list[i].AmountMax;
                    discSetup.QtyMin = list[i].QtyMin;
                    discSetup.QtyMax = list[i].QtyMax;
                    discSetup.StartDate = list[i].StartDate;
                    discSetup.EndDate = list[i].EndDate;
                    discSetup.TableCode = list[i].TableCode;
                    discSetup.Multi = list[i].Multi;
                    discSetup.ApprovedDate = list[i].ApprovedDate;
                    discSetup.Status = list[i].Status;
                    discSetup.CreatedDate = DateTime.Now;
                    _context.Add(discSetup);
                    bool discExist = false;
                    discExist = _context.DiscountSetup.Any(c => c.DiscountCode == discSetup.DiscountCode);
                    if (discExist == false)
                    {
                        _context.SaveChanges();
                    }
                    else
                    {
                        return StatusCode(404, new
                        {
                            status = "404",
                            create = false,
                            message = "Cannot create a record, discount code already exist."
                        });
                    }
                }

                return StatusCode(200, new
                {
                    status = "200",
                    create = true,
                    message = "Created successfully!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "500",
                    create = false,
                    message = ex.ToString()
                });
            }            
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> update(discountSetupList discSetupList)
        {
            try
            {
                List<DiscountSetup> list = discSetupList.discounts;

                for (int i = 0; i < list.Count; i++)
                {
                    bool discExist = false;
                    discExist = _context.DiscountSetup.Any(c => c.DiscountCode == list[i].DiscountCode);
                    if (discExist == true)
                    {
                        var discSetupObj = _context.DiscountSetup.Where(x => x.DiscountCode == list[i].DiscountCode).First();
                        discSetupObj.DiscountCode = list[i].DiscountCode;
                        discSetupObj.DiscountName = list[i].DiscountName;
                        discSetupObj.DiscountCategory = list[i].DiscountCategory;
                        discSetupObj.CustomerGroupId = list[i].CustomerGroupId;
                        discSetupObj.DiscountCash = list[i].DiscountCash;
                        discSetupObj.DiscountPercent = list[i].DiscountPercent;
                        discSetupObj.DiscountType = list[i].DiscountType;
                        discSetupObj.AmountMin = list[i].AmountMin;
                        discSetupObj.AmountMax = list[i].AmountMax;
                        discSetupObj.QtyMin = list[i].QtyMin;
                        discSetupObj.QtyMax = list[i].QtyMax;
                        discSetupObj.StartDate = list[i].StartDate;
                        discSetupObj.EndDate = list[i].EndDate;
                        discSetupObj.TableCode = list[i].TableCode;
                        discSetupObj.Status = list[i].Status;
                        discSetupObj.Multi = list[i].Multi;
                        discSetupObj.ApprovedDate = list[i].ApprovedDate;
                        discSetupObj.CreatedDate = discSetupObj.CreatedDate;

                        _context.DiscountSetup.Update(discSetupObj);
                        _context.SaveChanges();
                    }
                    else
                    {
                        return StatusCode(404, new
                        {
                            status = "404",
                            update = false,
                            message = "Discount code not found."
                        });
                    }
                }

                return StatusCode(200, new
                {
                    status = "200",
                    update = true,
                    message = "updated successfully!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "500",
                    update = false,
                    message = ex.ToString()
                });
            }            
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> delete(discountSetupList discSetupList)
        {
            try
            {
                List<DiscountSetup> list = discSetupList.discounts;

                for (int i = 0; i < list.Count; i++)
                {
                    var discSetupObj = _context.DiscountSetup.Where(x => x.DiscountCode == list[i].DiscountCode).First();
                    _context.DiscountSetup.Remove(discSetupObj);
                    _context.SaveChanges();
                }

                return StatusCode(200, new
                {
                    status = "200",
                    delete = true,
                    message = "Deleted successfully!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "500",
                    delete = false,
                    message = ex.ToString()
                });
            }            
        }
    }
}