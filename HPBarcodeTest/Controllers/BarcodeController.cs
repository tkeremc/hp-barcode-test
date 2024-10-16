using HPBarcodeTest.Interfaces;
using HPBarcodeTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace HPBarcodeTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BarcodeController : ControllerBase
    {
        private readonly IBarcodeService _barcodeService;

        public BarcodeController(IBarcodeService barcodeService)
        {
            _barcodeService = barcodeService;
        }
        
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllBarcodes()
        {
            var barcodes = await _barcodeService.GetAll();
            return Ok(barcodes);
        }

        [HttpGet("gethpid")]
        public async Task<IActionResult> GetByBarcode(string barcode)
        {
            var model = await _barcodeService.GetByBarcode(barcode);
            return Ok(model);
        }
        
        
        [HttpGet("get")]
        public async Task<IActionResult> GetBarcode(string id)
        {
            var barcode = await _barcodeService.Get(id);
            if (barcode == null)
            {
                return NotFound();
            }
            return Ok(barcode);
        }
        
        [HttpPost("create")]
        public async Task<IActionResult> CreateBarcode([FromBody] BarcodeModel barcodeModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdBarcode = await _barcodeService.Create(barcodeModel);
            return CreatedAtAction(nameof(GetBarcode), new { id = createdBarcode.QrId }, createdBarcode);
        }
        
        [HttpPut("update")]
        public async Task<IActionResult> UpdateBarcode([FromBody] BarcodeModel barcodeModel)
        {
            var updatedBarcode = await _barcodeService.Update(barcodeModel);

            if (updatedBarcode == null)
            {
                return NotFound();
            }

            return Ok(updatedBarcode);
        }
        
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteBarcode(string id)
        {
            var deletedBarcode = await _barcodeService.Delete(id);
            if (deletedBarcode == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return StatusCode(StatusCodes.Status200OK, deletedBarcode);
        }
        
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("API is working");
        }

    }
}
