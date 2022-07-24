using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TT.Lib.DataAccess;
using TT.Lib.Entities;
using TT.Lib.Mvc;

namespace TT.Api.Controllers
{
    [Route("[controller]")]
    public class ProductController :  ReadWriteController<Product, int>
    {
        private readonly IProductService mainService;

        public ProductController(IProductService mainService) : base((IReadWriteService<Product>)mainService)
        {
            this.mainService = mainService;
        }

        [HttpDelete]
        public new async Task<IActionResult> Delete(Product product) => Ok(await base.Delete(product));
    }
}
