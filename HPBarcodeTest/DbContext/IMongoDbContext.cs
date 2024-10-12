using MongoDB.Driver;

namespace HPBarcodeTest.DbContext;

public interface IMongoDbContext
{
    IMongoCollection<TEntity> GetCollection<TEntity>(string name);
    Task<int> SaveChanges();
}