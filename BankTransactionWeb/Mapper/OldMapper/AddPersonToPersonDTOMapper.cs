using BankTransaction.Configuration;
using BankTransaction.Models.DTOModels;
using BankTransaction.Web.ViewModel;

namespace BankTransaction.Web.Mapper.OldMapper
{
    public class AddPersonToPersonDTOMapper:IMapper< AddPersonViewModel, PersonDTO>
    {
        private AddPersonToPersonDTOMapper() { }
        public static readonly AddPersonToPersonDTOMapper Instance = new AddPersonToPersonDTOMapper();


        public PersonDTO Map(AddPersonViewModel source)
        {
            return new PersonDTO()
            {
                Name = source.Name,
                Surname = source.Surname,
                LastName = source.LastName,
                DataOfBirth = source.DataOfBirth
            };
        }

        public AddPersonViewModel MapBack(PersonDTO destination)
        {
            return new AddPersonViewModel()
            {
                Name = destination.Name,
                Surname = destination.Surname,
                LastName = destination.LastName,
                DataOfBirth = destination.DataOfBirth
            };
        }
    }
}
