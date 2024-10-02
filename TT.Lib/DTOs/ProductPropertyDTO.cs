namespace TT.Lib.DTOs
{
    public class ProductPropertyDTO
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public ProductPropertyDTO[] Children { get; set; }
    }
}
