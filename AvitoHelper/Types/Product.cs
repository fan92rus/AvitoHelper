using System.ComponentModel.DataAnnotations;

namespace AvitoHelper.Types
{
    public class Product : BaseType
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Price { get; set; }
        public string Link { get; set; }
    }
}
