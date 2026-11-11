using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibraryML.MediaSection;
namespace SharedLibraryML.CollectionSection
{
    public class HelperForAlbum : IArtistCollectionHelper
    {
        public HelperForAlbum() { }

        public bool CheckMediaType(Media media)
        {
           if (media is not Song)
            {
                throw new ArgumentException("This item is not song");
            }
            return true;
        }

        public bool CheckMediaListQuantity(Collection collection)
        {
            return collection.GetMedias().Count <= 20;
        }
    }
}
