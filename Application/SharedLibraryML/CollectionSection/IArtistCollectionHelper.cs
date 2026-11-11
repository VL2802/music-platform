using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibraryML.MediaSection;

namespace SharedLibraryML.CollectionSection
{
    public interface IArtistCollectionHelper
    {
        bool CheckMediaType(Media media);
        bool CheckMediaListQuantity(Collection collection);
    }
}
