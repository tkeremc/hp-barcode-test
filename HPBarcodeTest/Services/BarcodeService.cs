using HPBarcodeTest.DbContext;
using HPBarcodeTest.Helpers;
using HPBarcodeTest.Interfaces;
using HPBarcodeTest.Models;
using HPBarcodeTest.Utils;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace HPBarcodeTest.Services;

public class BarcodeService : IBarcodeService
{
    private readonly IMongoCollection<BarcodeModel> _barcodeCollection;
    
    public BarcodeService(IMongoDbContext mongoDbContext)
    {
        _barcodeCollection = mongoDbContext.GetCollection<BarcodeModel>(AppSettingConfig.Configuration["MongoDBSettings:UserCollection"]!);
    }

    public async Task<BarcodeModel> Create([FromBody] BarcodeModel barcode)
    {
        await _barcodeCollection.InsertOneAsync(barcode);
        return barcode;
    }

    public async Task<BarcodeModel> Update([FromBody] BarcodeModel barcode)
    {

        var defaultModel = new BarcodeModel
        {
            HpId = "zmhp-0000",
            QrId = "123456789",
            IsDeleted = true
        };
        
        var previousModel = await _barcodeCollection.Find(x => x.QrId == barcode.QrId).FirstOrDefaultAsync();
        if (previousModel != null) return defaultModel;
        if (previousModel.QrId != barcode.QrId) return defaultModel;
        
        UpdateCheckHelper.Checker(previousModel, barcode);

        var updateDefinition = Builders<BarcodeModel>.Update
            .Set(x => x.QrId, barcode.QrId)
            .Set(x => x.HpId, barcode.HpId);
        
        var result = await _barcodeCollection.UpdateOneAsync(x => x.QrId == barcode.QrId, updateDefinition);
        if (result.MatchedCount == 0) return defaultModel;
        
        return barcode;
    }

    public async Task<BarcodeModel> Delete(string barcode)
    {
        var oldBarcode = await _barcodeCollection.Find(x => x.QrId == barcode).FirstOrDefaultAsync();
        var updateDefinition = Builders<BarcodeModel>.Update
            .Set(x => x.IsDeleted, true);
        var result = await _barcodeCollection.UpdateOneAsync(x => x.QrId == barcode, updateDefinition);
        
        var deletedBarcode = await _barcodeCollection.Find(x => x.QrId == barcode).FirstOrDefaultAsync();
        
        if (result.MatchedCount == 0) return oldBarcode;
        return deletedBarcode;
    }

    public async Task<BarcodeModel> Get(string barcode)
    {
        return await _barcodeCollection.Find(x => x.QrId == barcode).FirstOrDefaultAsync();
    }

    public async Task<List<BarcodeModel>> GetAll()
    {
        return await _barcodeCollection.Find(_ => true).ToListAsync();
    }
}