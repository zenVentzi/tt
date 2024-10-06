using System.Collections.Generic;
using TT.Lib.Entities;

namespace TT.Lib.DTOs
{
    public class BrandDTO
    {
        public string Name { get; set; }
        public IEnumerable<ProductDTO> products { get; set; }
    }
}
