using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Lab5.Models
{
    public enum Question
    {
        Earth, Computer
    }
    public class AnswerImage
    {
        public int AnswerImageId
        {
            get;
            set;
        }

        [Required]
        [StringLength(50)]
        [DisplayName("File Name")]
        public string FileName
        {
            get;
            set;
        }

        [Required]
        [Url]
        [DisplayName("URL")]
        public string Url
        {
            get;
            set;
        }

        [Required]
        [Display(Name ="question")]
        public Question question { get; set; }
    }
}
