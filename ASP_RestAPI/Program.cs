using System.Net.Mime;
using System.Text.Json;
using ASP_RestAPI.Repositories;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IItemsRepository, MsSqlDbRepository>();
builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});

builder.Services.AddHealthChecks()
.AddSqlServer(
        System.Configuration.ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString,
        name: "MsSql",
        timeout: TimeSpan.FromSeconds(3),
        tags: new[] { "ready" }
);

builder.Services.AddEndpointsApiExplorer();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
    {
        Predicate = (check) => check.Tags.Contains("ready"),
        ResponseWriter = async (context, report) =>
        {
            var result = JsonSerializer.Serialize(new
            {
                status = report.Status.ToString(),
                cheks = report.Entries.Select(entry => new
                {
                    name = entry.Key,
                    status = entry.Value.Status.ToString(),
                    exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
                    duration = entry.Value.Duration.ToString()
                })
            });
            context.Response.ContentType = MediaTypeNames.Application.Json;
            await context.Response.WriteAsync(result);
        }
    });
    endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
    {
        Predicate = (_) => false
    });
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

















// using System.Data.SqlClient;
// using System.Data;
// using ASP_RestAPI.Entities;

// string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;









// MsSqlDbRepository msSqlDbRepository = new();

// System.Console.WriteLine();
// Item item = msSqlDbRepository.GetItem(Guid.Parse("4c828d63-3f62-4340-b51a-e587213043a0")) with{
//     Name = "Arbalet",
//     Price = 500
// };
// msSqlDbRepository.UpdateItem(item);


















// connected mode
// if (false)
// {

//     SqlConnection connection = new();
//     SqlDataReader reader = null;
//     SqlCommand command = new();



//     Item item1 = new()
//     {
//         Id = Guid.NewGuid(),
//         Name = "Sword",
//         Price = 100,
//         CreatedDate = DateTimeOffset.Now
//     };

//     try
//     {
//         connection.ConnectionString = connectionString;
//         connection.Open();

//         two select commands
//         string selectCommandStr = @"Select *
//                                 From Items";
//         insert command
//         DateTimeOffset date = DateTimeOffset.Parse(item.CreatedDate.ToString());
//         string insertCommandStr = @$"Insert into Items Values('{item.Id}','{item.Name}',{item.Price},'{item.CreatedDate}')";


//         command.Connection = connection;
//         command.CommandText = insertCommandStr;

//         initialization of SqlDataReader obj
//         reader = await command.ExecuteReaderAsync();
//         await command.ExecuteNonQueryAsync();
//         command.CommandText = selectCommandStr;
//         reader = await command.ExecuteReaderAsync();

//         do
//         {
//             int line = 0;
//             one column parameters then second column and so on
//             while (reader.Read())
//             {
//                 if (line == 0)
//                 {
//                     for (int i = 0; i < reader.FieldCount; i++)
//                     {
//                         System.Console.Write(reader.GetName(i) + " ");
//                     }
//                     System.Console.WriteLine();
//                 }

//                 for (int i = 0; i < reader.FieldCount; i++)
//                 {
//                     System.Console.Write(reader[i] + " ");
//                 }
//                 System.Console.WriteLine();

//                 line++;
//             }
//             System.Console.WriteLine();
//             System.Console.WriteLine();

//             looking for the next command
//         } while (reader.NextResult());

//     }
//     finally
//     {
//         if (connection != null)
//             connection.Close();
//         if (reader != null) 
//             reader.Close();
//     }
// }
// Disconnected mode
// if (false)
// {
//     string sql = "SELECT * FROM Authors";

//     using (SqlConnection connection = new(connectionString))
//     {
//         SqlDataAdapter dataAdapter = new(sql, connection);
//         DataSet dataSet = new();

//         dataAdapter.Fill(dataSet);

//         foreach (DataTable dataTable in dataSet.Tables)
//         {
//             foreach (DataColumn column in dataTable.Columns)
//                 System.Console.Write($"{column.ColumnName}\t");
//             System.Console.WriteLine();

//             foreach (DataRow row in dataTable.Rows)
//             {
//                 var cells = row.ItemArray;

//                 foreach (var cell in cells)
//                     System.Console.Write($"{cell}\t");
//                 System.Console.WriteLine();
//             }

//         }
//     }
// }
