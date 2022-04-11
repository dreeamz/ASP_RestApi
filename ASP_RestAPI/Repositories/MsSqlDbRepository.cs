using System.Data;
using System.Data.SqlClient;
using ASP_RestAPI.Entities;

namespace ASP_RestAPI.Repositories
{
    public class MsSqlDbRepository : IItemsRepository
    {


        private readonly string connectionString =
        System.Configuration.ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

        private SqlConnection connection = new();
        private SqlDataReader reader = null;
        private SqlCommand command = new();
        public MsSqlDbRepository()
        {
            connection.ConnectionString = connectionString;
        }
        public IEnumerable<Item> GetItems()
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();

            command.Connection = connection;
            command.CommandText = "SELECT * FROM ITEMS";
            reader = command.ExecuteReader();

            //Item Properties
            string ItemName = " ";
            Guid ItemGuid = new();
            decimal ItemPrice = 0;
            DateTimeOffset ItemCreatedDate = new();

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    switch (i)
                    {
                        case 0:
                            ItemGuid = Guid.Parse($"{reader[i]}");
                            break;
                        case 1:
                            ItemName = (string)reader[i];
                            break;
                        case 2:
                            ItemPrice = (decimal)reader[i];
                            break;
                        case 3:
                            ItemCreatedDate = DateTimeOffset.Parse($"{reader[i]}");
                            break;
                    }
                }

                Item item = new Item()
                {
                    Name = ItemName,
                    Id = ItemGuid,
                    Price = ItemPrice,
                    CreatedDate = ItemCreatedDate
                };



                yield return item;
            };
            connection.Close();
        }



        public async Task CreateItemAsync(Item item)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            command.Connection = connection;
            command.CommandText = @$"Insert into Items Values('{item.Id}','{item.Name}',{item.Price},'{item.CreatedDate}')";
            await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();
        }

        public async Task DeleteItemAsync(Guid id)
        {

            if (connection.State != ConnectionState.Open)
                connection.Open();
            command.Connection = connection;

            command.CommandText = @$"DELETE FROM Items WHERE ID ='{id}'";
            await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();
        }

        public async Task<Item> GetItemAsync(Guid id)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            command.Connection = connection;

            command.CommandText = @$"Select * FROM Items WHERE ID ='{id}'";
            reader = await command.ExecuteReaderAsync();

            if (reader.Read())
            {
                Item item = new()
                {
                    Id = Guid.Parse(reader[0].ToString()),
                    Name = (string)reader[1],
                    Price = (decimal)reader[2],
                    CreatedDate = (DateTimeOffset)reader[3]
                };
                await connection.CloseAsync();
                return item;
            }
            await connection.CloseAsync();
            return null;
        }

        public async Task UpdateItemAsync(Item item)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            command.Connection = connection;

            command.CommandText = @$"UPDATE Items SET Name = '{item.Name}', Price = {item.Price} WHERE ID = '{item.Id}'";
            await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();
        }

    }
}