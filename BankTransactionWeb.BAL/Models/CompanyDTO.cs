using System;
using System.Collections.Generic;

namespace BankTransaction.Models.DTOModels
{
    public class CompanyDTO:BaseModel
    {
        public string Name { get; set; }
        public DateTime DateOfCreation { get; set; }
        private List<ShareholderDTO> shareholders;
        public List<ShareholderDTO> Shareholders
        {
            get
            {
                return shareholders ?? (shareholders = new List<ShareholderDTO>());
            }
            set
            {
                shareholders = value;
            }
        }
    }
}
