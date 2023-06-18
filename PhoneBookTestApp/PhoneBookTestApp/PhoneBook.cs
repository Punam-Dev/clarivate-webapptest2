using Newtonsoft.Json;
using System;
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
            if (person is null)
            {
                throw new ArgumentNullException(nameof(person));
            }

            if (string.IsNullOrEmpty(person.name))
            {
                throw new ArgumentNullException(nameof(person.name));
            }

            SQLiteParameter[] parameters = new SQLiteParameter[]
            {
                new SQLiteParameter("@name", $"%{person.name}%"),
            };
            string commandText = "SELECT * FROM PHONEBOOK WHERE NAME LIKE @name LIMIT 1";

            var existingPerson = await _repository.GetRecordAsync(commandText, CommandType.Text, parameters);

            if(existingPerson != null)
            {
                throw new ArgumentException($"Person with name {person.name} already exists in db");
            }

            parameters = new SQLiteParameter[]
            {
                new SQLiteParameter("@name", person.name),
                new SQLiteParameter("@phoneNumber", person.phoneNumber),
                new SQLiteParameter("@address", person.address),
            };
            commandText = "INSERT INTO PHONEBOOK (NAME, PHONENUMBER, ADDRESS) VALUES(@name, @phoneNumber, @address)";

            await _repository.ExecuteAsync(commandText, CommandType.Text, parameters);
        }

        public async Task<Person> FindPerson(string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentNullException(nameof(firstName));
            }

            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentNullException(nameof(lastName));
            }

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