using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using ML_LogicLayer;
using System.ComponentModel;
using SharedLibraryML;
using System.ComponentModel.DataAnnotations;

namespace ML_WebApp.MDA
{
    public class MDAMedia
    {
        public int MediaId { get; set; }
        [Required]
        public string MediaTitle { get; set; }

        
        [Display(Name = "Image Picture")]
        public IFormFile? FileImage { get; set; }

        public IFormFile? FileAudio { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Published { get; set; }

        public SharedLibraryML.MediaType MediaType { get; set; }
        public MainMusicGenre MusicGenre { get; set; }

        public string? SubGenre { get; set; }

        public string? Lyrics { get; set; }



        public BookGenre BookGenre { get; set; }

    }
   

}
