using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraryML.MediaSection
{
    public class Song : Media
    {
        private string? _subGenre;
        private MainMusicGenre _musicGenre;
        private string? _lyrics;

        public Song(string title, string image, string filePath, DateTime published , MainMusicGenre mainGenre, string subGenre, string lyrics) : base(title, image, filePath, published)
        {
            _subGenre = subGenre ?? "";
            if (mainGenre == MainMusicGenre.NONE)
            {
                throw new ArgumentException("The Genre must be selected");
            }
            _musicGenre = mainGenre;
            _lyrics = lyrics ?? "";
            _type = MediaType.SONG;
        }

        public Song(int id, string title, string image, string filePath, DateTime published, MainMusicGenre mainGenre,  string subGenre, string lyrics) : base(id, title, image, filePath, published)
        {
            _subGenre = subGenre ?? "";
            _musicGenre = mainGenre;
            _lyrics = lyrics ?? "";
            _type = MediaType.SONG;
        }


       

        public string GetSubGenre()
        { if (_subGenre == null )
            {
                throw new ArgumentNullException("This song doesnt have any sub genres.");
            }
            return _subGenre; }
        public override string GetGenre()
        {
           
            return _musicGenre.ToString();
        }
        public string GetLyrics()
        { if ( _lyrics == null )
            {
                throw new ArgumentNullException("This Song has no lyrics");
            }
            return _lyrics; }

       public override MediaType GetMediaType() { return _type; }
    }
}
