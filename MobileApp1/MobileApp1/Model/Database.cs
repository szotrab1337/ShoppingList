using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp1.Model
{
    public class Database
    {
        readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Item>().Wait();
        }

        public Task<List<Item>> GetItemsAsync()
        {
            return _database.Table<Item>().ToListAsync();
        }
        public Task<Item> GetItemAsync(Item item)
        {
            return _database.Table<Item>().Where(x => x.ID_Item == item.ID_Item).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemsAsync(Item item)
        {
            return _database.InsertAsync(item);
        }

        public Task<int> UpdateItemAsync(Item item)
        {
            return _database.UpdateAsync(item);
        }
        public Task<int> InsertItemAsync(Item item)
        {
            return _database.InsertOrReplaceAsync(item);
        }
        public Task<int> DeleteItemAsync(Item item)
        {
            return _database.DeleteAsync(item);
        }
    }
}
