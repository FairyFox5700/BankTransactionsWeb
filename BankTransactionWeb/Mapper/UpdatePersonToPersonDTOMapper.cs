using BankTransaction.Configuration;
using BankTransaction.Models.DTOModels;
using BankTransaction.Web.ViewModel;

namespace BankTransaction.Web.Mapper
{
    public class UpdatePersonToPersonDTOMapper:IMapper<UpdatePersonViewModel, PersonDTO>
    {
        private UpdatePersonToPersonDTOMapper() { }
        public static readonly UpdatePersonToPersonDTOMapper Instance = new UpdatePersonToPersonDTOMapper();

        public PersonDTO Map(UpdatePersonViewModel source)
        {
            return new PersonDTO()
            {
                Name = source.Name,
                Surname = source.Surname,
                LastName = source.LastName,
                DataOfBirth = source.DataOfBirth,
                UserName = source.UserName,
                PhoneNumber = source.PhoneNumber,
                ApplicationUserFkId = source.ApplicationUserFkId,
                Id = source.Id
            };
        }

        public UpdatePersonViewModel MapBack(PersonDTO destination)
        {
            return new UpdatePersonViewModel()
            {
                Id = destination.Id,
                UserName = destination.UserName,
                Name = destination.Name,
                Surname = destination.Surname,
                LastName = destination.LastName,
                DataOfBirth = destination.DataOfBirth,
                PhoneNumber = destination.PhoneNumber,
                ApplicationUserFkId = destination.ApplicationUserFkId
            };
        }
    }
}
