using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSServices.Data;
using POSServices.Models;

namespace POSServices.WebAPIBackendController
{
    [Route("api/DiscountSetupLine")]
    [ApiController]
    public class DiscountSetupLineController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public DiscountSetupLineController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> getDiscountSetupLine()
        {
            try
            {
                var discountSetupLine = (from dsl in _context.DiscountSetupLines
                                         select new
                                         {
                                             DiscountCode = dsl.DiscountCode,
                                             GroupCode = dsl.GroupCode,
                                             Code = dsl.Code,
                                             DiscountPercent = dsl.DiscountPrecentage,
                                             DiscountCash = dsl.DiscountCash,
                                             QtyMin = dsl.QtyMin,
                                             QtyMax = dsl.QtyMax,
                                             AmountMin = dsl.AmountMin,
                                             AmountMax = dsl.AmountMax,
                                             ArticleIdDisc = dsl.ArticleIdDiscount,
                                             Multi = dsl.Multi
                                         }).ToList();

                return Json(new[] { discountSetupLine });
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

        [HttpPost("Create")]
        public async Task<IActionResult> create(discountSetupLineList discSetupLnList)
        {
            try
            {
                List<DiscountSetupLines> list = discSetupLnList.discountLines;

                for (int i = 0; i < list.Count; i++)
                {
                    var discSetup = _context.DiscountSetup.Where(x => x.DiscountCode == list[i].DiscountCode).First();

                    DiscountSetupLines discSetupLines = new DiscountSetupLines();
                    discSetupLines.DiscountCode = list[i].DiscountCode;
                    discSetupLines.GroupCode = list[i].GroupCode;
                    discSetupLines.Code = list[i].Code;
                    discSetupLines.DiscountPrecentage = list[i].DiscountPrecentage;
                    discSetupLines.DiscountCash = list[i].DiscountCash;
                    discSetupLines.QtyMin = list[i].QtyMin;
                    discSetupLines.QtyMax = list[i].QtyMax;
                    discSetupLines.AmountMin = list[i].AmountMin;
                    discSetupLines.AmountMax = list[i].AmountMax;
                    discSetupLines.ArticleIdDiscount = list[i].ArticleIdDiscount;
                    discSetupLines.Multi = list[i].Multi;
                    discSetupLines.DiscountSetupId = discSetup.Id;
                    discSetupLines.CreatedDate = DateTime.Now;
                    _context.Add(discSetupLines);
                    _context.SaveChanges();
                }

                return StatusCode(200, new
                {
                    status = "200",
                    create = true,
                    message = "created successfully!"
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

        [HttpPost("Update")]
        public async Task<IActionResult> update(discountSetupLineList discSetupLnList)
        {
            try
            {
                List<DiscountSetupLines> list = discSetupLnList.discountLines;

                for (int i = 0; i < list.Count; i++)
                {
                    bool discExist = false;
                    discExist = _context.DiscountSetupLines.Any(c => c.DiscountCode == list[i].DiscountCode && c.Code == list[i].Code);
                    if (discExist == true)
                    {
                        var discSetupLnObj = _context.DiscountSetupLines.Where(x => x.DiscountCode == list[i].DiscountCode && x.Code == list[i].Code).First();
                        discSetupLnObj.DiscountCode = list[i].DiscountCode;
                        discSetupLnObj.GroupCode = list[i].GroupCode;
                        discSetupLnObj.Code = list[i].Code;
                        discSetupLnObj.DiscountCash = list[i].DiscountCash;
                        discSetupLnObj.DiscountPrecentage = list[i].DiscountPrecentage;                        
                        discSetupLnObj.AmountMin = list[i].AmountMin;
                        discSetupLnObj.AmountMax = list[i].AmountMax;
                        discSetupLnObj.QtyMin = list[i].QtyMin;
                        discSetupLnObj.QtyMax = list[i].QtyMax;
                        discSetupLnObj.ArticleIdDiscount = list[i].ArticleIdDiscount;
                        discSetupLnObj.Multi = list[i].Multi;                        
                        discSetupLnObj.CreatedDate = discSetupLnObj.CreatedDate;

                        _context.DiscountSetupLines.Update(discSetupLnObj);
                        _context.SaveChanges();
                    }
                    else
                    {
                        return StatusCode(404, new
                        {
                            status = "404",
                            update = false,
                            message = "Record not found."
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
        public async Task<IActionResult> delete(discountSetupLineList discSetupLnList)
        {
            try
            {
                List<DiscountSetupLines> list = discSetupLnList.discountLines;

                for (int i = 0; i < list.Count; i++)
                {
                    var discSetupLnObj = _context.DiscountSetupLines.Where(x => x.DiscountCode == list[i].DiscountCode && x.Code == list[i].Code).First();
                    _context.DiscountSetupLines.Remove(discSetupLnObj);
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