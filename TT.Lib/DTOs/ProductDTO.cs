using System.Collections.Generic;

namespace TT.Lib.DTOs
{
    public class ProductDTO
    {
        public string Code { get; set; }
        public Dictionary<string, object> Properties { get; set; }
    }
}
