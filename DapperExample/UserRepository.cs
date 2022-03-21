using DapperExample.Model;

namespace DapperExample
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(string name)
        {
            this._tableName = name;  
        }
    }
}