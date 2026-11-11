using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraryML.MediaSection
{
    public class Chapter : Media
    {
        private BookGenre _bookGenre;

        public Chapter(string title, string image, string filePath, DateTime published, BookGenre bookGenre): base(title, image, filePath, published)
        {
            if(bookGenre == BookGenre.NONE)
            {
                throw new ArgumentException("The Genre must be selected");
            }
            _bookGenre = bookGenre;
            _type = MediaType.CHAPTER;
        }

        public Chapter(int id, string title, string image, string filePath, DateTime published , BookGenre bookGenre) : base(id, title, image, filePath, published)
        {
            _bookGenre = bookGenre ;
            _type = MediaType.CHAPTER;
        }

        public override string GetGenre()
        {
           
            return _bookGenre.ToString();
        }

       public override MediaType GetMediaType() { return _type; }

    }
}
