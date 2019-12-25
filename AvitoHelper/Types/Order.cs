using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AvitoHelper.Types
{
    public class Order
    {
        [Key]
        public int id { get; set; }
        public bool IgnoreFirstIteration { get; set; } = true;
        public string Link { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Product> products { get; set; }
        public virtual ICollection<Word> ignoreWords { get; set; }
        public virtual ICollection<Word> NeedWords { get; set; }
    }
    public class Word
    {
        [Key]
        public int id { get; set; }
        public string Text { get; set; }
    }
}
