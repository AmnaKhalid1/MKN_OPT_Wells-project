using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ExcelDemo.Models
{
    public class OptimizationData
    {

        [Display(Name = "ID")]
        [Required(ErrorMessage = "Required Field")]
        public int Id { get; set; }


        [Display(Name = "Date")]
        [Required(ErrorMessage = "Required Field")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime RecordDate { get; set; }


        [Display(Name = "Optimization Types")]
        [Required(ErrorMessage = "Required Field")]
        public OPTIMIZATIONTYPE OPTIMIZATIONTYPES { get; set; }

        [NotMapped]
        public IFormFile FormFile { get; set; }

    }
}
