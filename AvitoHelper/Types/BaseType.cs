using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AvitoHelper.Types
{
    public abstract class BaseType
    {
        [Key]
        public int Id { get; set; }
    }
}
