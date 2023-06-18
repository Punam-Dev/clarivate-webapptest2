using System.Threading.Tasks;

namespace PhoneBookTestApp
{
    public interface IPhoneBook
    {
        Task<Person> FindPerson(string firstName, string lastName);
        Task AddPerson(Person person);
    }
}