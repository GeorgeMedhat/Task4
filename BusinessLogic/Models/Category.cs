using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    //[Table("Categories",Schema ="MasterSchema")]
    public class Category
    {
        //[Key]
        public int id { get; set; }
        //[Required]
        //[MaxLength(50)]
        public string catName { get; set; }
        //[Required]
        public int catOrder { get; set; }
        //[NotMapped]
        public DateTime createdDate { get; set; }
        //[Column("isDeleted")]
        public bool markedAsDeleted { get; set; }

    }
}
