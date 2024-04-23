using StackExchange.Redis;

namespace HistoryService.Service
{
    public class DbService
    {
        private readonly ConnectionMultiplexer redis;

        private readonly IDatabase db;

        public DbService()
        {
            redis = ConnectionMultiplexer.Connect("redis:6379");
            db = redis.GetDatabase();
        }
        public List<string> GetAll(string name)
        {
            var hashEntries = db.HashGetAll(name);
            if (hashEntries.Length == 0) return null;
            List<string> History = new List<string>();

            foreach (var item in hashEntries) 
            {
                History.Add(item.Value.ToString());
            }
            return History;
        }
        public void SaveData(string name , string str) { 
            var hashEntries = db.HashGetAll(name);
            if (hashEntries.Length == 20)
            {
                for (int i = 0; i < hashEntries.Length - 1; i++)
                {
                    string thisfield = hashEntries[i].Value.ToString();
                    db.HashSet(name, i+1, thisfield);
                }
                db.HashSet(name, 0, str);
            }
            else {
                db.HashSet(name,hashEntries.Length,str);
            }
        }


    }
}
