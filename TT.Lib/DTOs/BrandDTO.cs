using TT.Lib.Entities;

namespace TT.Lib.DTOs
{
    public class BrandDTO
    {
        public string Name { get; set; }
        public ProductDTO[] products { get; set; }
    }
}
