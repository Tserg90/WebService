using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebService.Model
{
    [Table("FileTable")]
    public class FileInDb
    {
        [Key]
        [Column("fileName")]
        public string fileName { get; set; }
        public string fileMime { get; set; }
        public byte[] fileContent { get; set; }
    }
}
