using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductManagementSystemDashboard.Models;

namespace ProductManagementSystemDashboard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DAL _dal;
        private readonly string _documentPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProductionDocuments");
        public ProductsController(DAL dal)
        {
            _dal = dal;    
        }

        //Add a new product
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] Products product, IFormFile productionDocument)
        {
            try
            {
                if (product.ProductionDocument != null && productionDocument.Length > 0)
                {
                    string fileName = $"{Guid.NewGuid().ToString()}_{Path.GetFileName(productionDocument.FileName)}";
                    string filePath = Path.Combine(_documentPath, fileName);

                    if (!Directory.Exists(_documentPath))
                    {
                        Directory.CreateDirectory(_documentPath);
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await productionDocument.CopyToAsync(fileStream);
                    }
                    product.ProductionDocument = $"/ProductionDocuments/{fileName}";
                }
                else
                {
                    return BadRequest("Production document is required");
                }
                int newRecord = _dal.InsertProduct(product);
                if (newRecord > 0) {
                    return Ok(new { Message = "product created" , ProductId = newRecord });
                }
                else
                {
                    return BadRequest("Failed to create product");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //Get a product by id
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            try
            {
                List<Products> products = _dal.GetProducts();
                if (products.Count > 0)
                {
                    return Ok(products);
                }
                else
                {
                    return NotFound("No products found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetAllProducts(int id)
        {
            try
            {
                Products product = _dal.GetProductById(id);
                if (product != null)
                {
                    return Ok(product);
                }
                else
                {
                    return NotFound("Product not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //Update a product
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] Products product, IFormFile productionDocument)
        {
            product.Id = id;
            try
            {
                if (product.ProductionDocument != null && productionDocument.Length > 0)
                {
                    string fileName = $"{Guid.NewGuid().ToString()}_{Path.GetFileName(productionDocument.FileName)}";
                    string filePath = Path.Combine(_documentPath, fileName);
                    if (!Directory.Exists(_documentPath))
                    {
                        Directory.CreateDirectory(_documentPath);
                    }
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await productionDocument.CopyToAsync(fileStream);
                    }
                    product.ProductionDocument = $"/ProductionDocuments/{fileName}";
                }
                else
                {
                    return BadRequest("Production document is required");
                }

                bool isUpdated = _dal.UpdateProduct(product);
                if (isUpdated)
                {
                    return Ok("Product updated");
                }
                else
                {
                    return BadRequest("Failed to update product");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //Delete a product
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                bool isDeleted = _dal.DeleteProduct(id);
                if (isDeleted)
                {
                    return Ok("Product deleted");
                }
                else
                {
                    return BadRequest("Failed to delete product");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
