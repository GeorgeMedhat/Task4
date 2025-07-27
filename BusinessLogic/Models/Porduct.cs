using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    //[Table("Products",Schema ="MasterSchema")]
    public class Product
    {
        //[Key]
        public int id { get; set; }

        //[Required]
        //[MaxLength(50)]
        public string title { get; set; }

        //[MaxLength(250)]
        //[AllowNull]
        public string  description { get; set; }

        //[Required]
        //[MaxLength(50)]
        public string  author { get; set; }

        //[Required]
        //[Range(1,1000)]
        public double price { get; set; }

        public bool markedAsDeleted { get; set; }

        public int categoryId { get; set; }

        public Category category;
    }
}
