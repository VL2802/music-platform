using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibraryML.MediaSection;
namespace SharedLibraryML.CollectionSection
{
    public class HelperForAudiobook : IArtistCollectionHelper
    {
        public HelperForAudiobook() { }

        public bool CheckMediaType(Media media)
        {
            if (media is not Chapter)
            {
                throw new ArgumentException("This item is not Chapter");
            }
            return true;
        }



        public bool CheckMediaListQuantity(Collection collection)
        {
            return collection.GetMedias().Count <= 100;
        }


    }
}
