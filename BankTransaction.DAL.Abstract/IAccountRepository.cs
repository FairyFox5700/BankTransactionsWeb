﻿using BankTransaction.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransaction.DAL.Abstract
{
    public interface IAccountRepository : IRepository<Account>
    {
    }
}