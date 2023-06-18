using Newtonsoft.Json;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace PhoneBookTestApp
{
    public class PhoneBook : IPhoneBook
    {
        private readonly Repository<Person> _repository;
        public PhoneBook()
        {
            _repository = new Repository<Person>();
        }

        public async Task AddPerson(Person person)
        {
            SQLiteParameter[] parameters = new SQLiteParameter[]
            {
                new SQLiteParameter("@name", person.name),
                new SQLiteParameter("@phoneNumber", person.phoneNumber),
                new SQLiteParameter("@address", person.address),
            };
            string commandText = "INSERT INTO PHONEBOOK (NAME, PHONENUMBER, ADDRESS) VALUES(@name, @phoneNumber, @address)";

            await _repository.ExecuteAsync(commandText, CommandType.Text, parameters);
        }

        public async Task<Person> FindPerson(string firstName, string lastName)
        {
            SQLiteParameter[] parameters = new SQLiteParameter[]
            {
                new SQLiteParameter("@name", $"%{firstName} {lastName}%"),
            };
            string commandText = "SELECT * FROM PHONEBOOK WHERE NAME LIKE @name LIMIT 1";

            return await _repository.GetRecordAsync(commandText, CommandType.Text, parameters);
        }

        public override string ToString()
        {
            string commandText = "SELECT * FROM PHONEBOOK";

            var persons = _repository.GetRecordsAsync(commandText, CommandType.Text, null).Result;

            return JsonConvert.SerializeObject(persons);
        }
    }
}