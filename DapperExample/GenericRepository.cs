using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DapperExample
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected string _tableName;

        private SqlConnection SqlConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString);
        }

        private IDbConnection CreateConnection()
        {
            var connection = SqlConnection();
            connection.Open();
            return connection;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using (var connection = CreateConnection())
            {
                return await connection.QueryAsync<T>($"SELECT * FROM {_tableName}");
            }
        }

        public async Task DeleteRowAsync(int id)
        {
            using (var connection = CreateConnection())
            {
                await connection.QueryAsync<T>($"DELETE FROM {_tableName} WHERE Id=@Id", new { Id = id });
            }
        }

        public async Task<T> GetAsync(int id)
        {
            using (var connection = CreateConnection())
            {
                var result= await connection.QuerySingleOrDefaultAsync<T>($"SELECT * {_tableName} WHERE Id = @Id", new { Id = id });
                if (result == null)
                {
                    throw new KeyNotFoundException($"{_tableName} with id[{id}] could not be found.");
                }
                return result; 
            }
        }

        public async Task UpdateAsync(T t)
        {
            var updatedQuery = GenerateUpdateQuery();

            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(updatedQuery, t);
            }
        }

        private string GenerateUpdateQuery()
        {
            var updatedQuery = new StringBuilder($"UPDATE {_tableName} SET ");

            var properties = GenerateListOfProperties(GetProperties);

            properties.ForEach(property =>
            {
                if (!property.Equals("Id"))
                {
                    updatedQuery.Append($"{property}=@{property},");
                }
            });
            updatedQuery.Remove(updatedQuery.Length - 1, 1);

            updatedQuery.Append(" WHERE Id=@Id");
            return updatedQuery.ToString();
        }

        private List<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
        {
            return (from prop in listOfProperties
                    let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    where attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore"
                    select prop.Name).ToList();
        }

        public async Task InsertAsync(T t)
        {
            using (var connection = CreateConnection()) { }
        }

        public async Task<int> SaveRangeAsync(IEnumerable<T> list)
        {
            using (var connection = CreateConnection())
            {
                var counter = 0;
                //var query = 
                return 1;
            }
        }

        private IEnumerable<PropertyInfo> GetProperties => typeof(T).GetProperties();

    }
}
