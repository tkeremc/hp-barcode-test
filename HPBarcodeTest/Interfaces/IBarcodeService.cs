using HPBarcodeTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace HPBarcodeTest.Interfaces;

public interface IBarcodeService
{
    
    Task<string> GetByBarcode(string barcode);
    Task<BarcodeModel> Create([FromBody] BarcodeModel barcode);
    Task<BarcodeModel> Update([FromBody] BarcodeModel barcode);
    Task<BarcodeModel> Delete(string barcode);
    Task<BarcodeModel> Get(string barcode);
    Task<List<BarcodeModel>> GetAll();
}