using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BankTransactionWeb.Areas.Admin.Çontrollers
{
    public class AdminController : Controller
    {
        private readonly Logger<AdminController> logger;

        public AdminController(Logger<AdminController> logger)
        {
            this.logger = logger;
        }
    }
}