using AutoMapper;
using BankTransaction.Web.Areas.Admin.Models.ViewModels;
using BankTransaction.Web.Areas.Identity.Models.ViewModels;
using BankTransaction.Web.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace BankTransaction.Web.Configuration
{
    public static class AutoMapperConfiguration
    {
        public static MapperConfiguration ConfigureAutoMapper()
        {

            MapperConfiguration configuration = new MapperConfiguration(confg =>
            {
            });
            return configuration;
        }
    }
}
