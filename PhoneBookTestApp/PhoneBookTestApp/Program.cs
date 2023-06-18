using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookTestApp
{
    class Program
    {
        private static readonly PhoneBook phonebook = new PhoneBook();
        static async Task Main(string[] args)
        {
            try
            {
                DatabaseUtil.InitializeDatabase();
                /* TODO: create person objects and put them in the PhoneBook and database
                * John Smith, (248) 123-4567, 1234 Sand Hill Dr, Royal Oak, MI
                * Cynthia Smith, (824) 128-8758, 875 Main St, Ann Arbor, MI
                */

                await phonebook.AddPerson(new Person
                {
                    name = "John Smith",
                    phoneNumber = "(248) 123-4567",
                    address = "1234 Sand Hill Dr, Royal Oak, MI"
                });

                await phonebook.AddPerson(new Person
                {
                    name = "Cynthia Smith",
                    phoneNumber = "(824) 128-8758",
                    address = "875 Main St, Ann Arbor, MI"
                });

                // TODO: print the phone book out to System.out

                Console.WriteLine(phonebook);

                // TODO: find Cynthia Smith and print out just her entry
                Console.WriteLine("");

                var person = await phonebook.FindPerson("Cynthia", "Smith");
                if (person is null)
                {
                    Console.WriteLine("Record not found");
                }
                else
                {
                    Console.WriteLine(JsonConvert.SerializeObject(person));
                }

                // TODO: insert the new person objects into the database
                await phonebook.AddPerson(new Person
                {
                    name = "Punam Pal",
                    phoneNumber = "9090909090",
                    address = "Arambagh"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                DatabaseUtil.CleanUp();
                Console.ReadLine();
            }
        }
    }
}
