using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSServices.Data;
using POSServices.PosMsgModels;

namespace POSServices.WebAPIBackendController
{
    [Route("api/JobMsg")]
    [ApiController]
    public class JobMsgController : Controller
    {
        private readonly HO_MsgContext _context;

        public JobMsgController(HO_MsgContext context)
        {
            _context = context;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> create(JobList jobList)
        {
            try
            {
                List<Job> list = jobList.Jobs;

                for (int i = 0; i < list.Count; i++)
                {                    
                    Job job = new Job();
                    job.Description = list[i].Description;
                    job.StoreCode = list[i].StoreCode;
                    job.Synchdate = DateTime.Now;
                    job.LastSynch = DateTime.Now;
                    job.TableName = list[i].TableName;                     
                    job.Synctype = list[i].Synctype;
                    _context.Add(job);
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
    }
}