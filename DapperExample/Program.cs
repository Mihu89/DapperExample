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
            await UsingCRUD();
            Console.Read();
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

             
        }
    }
}
