using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TT.Lib.DataAccess;
using TT.Lib.Entities;
using TT.Lib.Mvc;

namespace TT.Api.Controllers
{
    [Route("[controller]")]
    public class BrandController : ReadWriteController<Brand, int>
    {
        private readonly IBrandService mainService;
        private readonly IProductService productService;

        public BrandController(IBrandService mainService, IProductService productService) : base((IReadWriteService<Brand>)mainService)
        {
            this.mainService = mainService;
            this.productService = productService;
        }

        [HttpGet("export/{productKey?}")]
        public async Task<IActionResult> Export(string? productKey)
        {
            try
            {
                if (productKey == null)
                {
                    return Ok(await mainService.Export());
                }

                return Ok(await this.productService.ExportProduct(productKey));
            }
            catch (System.Exception e)
            {
                // log exception
                return StatusCode(500, "Something went wrong. Please check admin or server logs for more info.");
            }
        }

        [HttpDelete]
        public new async Task<IActionResult> Delete(Brand brand) => Ok(await base.Delete(brand));
    }
}
