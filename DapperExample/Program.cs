using Dapper;
using DapperExample.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperExample
{
    class Program
    {
        private static string _connectionString = @"Server=93.115.141.254,10433;Database=MAGAZIN;User Id=itstep;Password=itstep++;";
        static async Task Main(string[] args)
        {
            //await UsingCRUD();
            await UsingRepository();
            Console.Read();
        }

        private static async Task UsingRepository()
        {
            var userRepository = new UserRepository("Users");
            var users = await userRepository.GetAllAsync();
            foreach (var user in users)
            {
                Console.WriteLine($"{user.Id} {user.Name} {user.Age}");
            }
        }

        private static async Task UsingCRUD()
        {
            IDbConnection db = new SqlConnection(_connectionString);

            // select all firms
            var firms = await db.QueryAsync<Firma>(@"SELECT TOP (1000) [FIRMA_ID]
                                                      ,[NUME]
                                                      ,[COD_FISCAL]
                                                      ,[ADRESA]
                                                  FROM[MAGAZIN].[dbo].[FIRMA]");

            foreach (var firma in firms)
            {
                Console.WriteLine($" {firma.FIRMA_ID} {firma.NUME} {firma.COD_FISCAL} {firma.ADRESA}");
            }

            // Insert example
            // read from console 
            string nume = "SA Bucurel-2";
            string codFiscal = "1009600029041";
            string adresa = "Chisinau str.Uzinelor 21";
            var affectedRows = await db.ExecuteAsync(@"insert FIRMA(NUME,COD_FISCAL,ADRESA) values (@nume, @codFiscal, @adresa)",
                                                new {nume, codFiscal, adresa });
            Console.WriteLine("Insert was done {0}", affectedRows);

            // Delete example
            Console.WriteLine("Write id to delete");
            int id = int.Parse(Console.ReadLine());
            var removed = await db.ExecuteAsync(
                "delete from FIRMA where FIRMA_ID = @id", new { id });
            Console.WriteLine($"Was removed {removed} rows");

            // update example

            //Get id after insert

            using (IDbConnection _db = new SqlConnection(_connectionString))
            {
                var firmaToInsert = new Firma
                {
                    NUME = "SIS",
                    COD_FISCAL = "1009650029038",
                    ADRESA = "str. Stefan cel Mare 100"
                };
                var sqlQuery = "INSERT INTO FIRMA (NUME,COD_FISCAL,ADRESA) VALUES(@NUME, @COD_FISCAL,@ADRESA); SELECT CAST(SCOPE_IDENTITY() as int)";
                int? firmaID = db.Query<int>(sqlQuery, firmaToInsert).FirstOrDefault();
                firmaToInsert.FIRMA_ID = firmaID ?? 0;
                Console.WriteLine("Last id is {0}", firmaToInsert.FIRMA_ID);
            }
        }
    }
}
