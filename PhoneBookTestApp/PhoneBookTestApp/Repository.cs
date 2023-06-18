using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookTestApp
{
    public class Repository<T> where T : new()
    {
        public async Task<int> ExecuteAsync(string cmdText, CommandType cmdType, SQLiteParameter[] parameters)
        {
            using (var dbConnection = DatabaseUtil.GetConnection())
            {
                var command = new SQLiteCommand(cmdText, dbConnection)
                {
                    CommandType = cmdType
                };

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                //dbConnection.Open();
                return await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<IEnumerable<T>> GetRecordsAsync(string cmdText, CommandType cmdType, SQLiteParameter[] parameters)
        {
            List<T> lstItems = new List<T>();

            using (var dbConnection = DatabaseUtil.GetConnection())
            {
                var command = new SQLiteCommand(cmdText, dbConnection)
                {
                    CommandType = cmdType
                };

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                using (var rdr = await command.ExecuteReaderAsync())
                {
                    while (await rdr.ReadAsync())
                    {
                        lstItems.Add(Map(rdr));
                    }
                }
            }

            return lstItems;
        }

        public async Task<T> GetRecordAsync(string cmdText, CommandType cmdType, SQLiteParameter[] parameters)
        {
            using (var dbConnection = DatabaseUtil.GetConnection())
            {
                var command = new SQLiteCommand(cmdText, dbConnection)
                {
                    CommandType = cmdType
                };

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                using (var rdr = await command.ExecuteReaderAsync())
                {
                    while (await rdr.ReadAsync())
                    {
                        return Map(rdr);
                    }
                }
            }
            return default;
        }

        private T Map(IDataRecord record)
        {
            var objT = Activator.CreateInstance<T>();
            foreach (var property in typeof(T).GetProperties())
            {
                if (record.HasColumn(property.Name) && !record.IsDBNull(record.GetOrdinal(property.Name)))
                {
                    if (property.PropertyType == typeof(bool) && record[property.Name].GetType() == typeof(int))
                    {
                        property.SetValue(objT, Convert.ToBoolean(record[property.Name]));
                    }
                    else
                    {
                        property.SetValue(objT, record[property.Name]);
                    }
                }
            }
            return objT;
        }
    }
}
