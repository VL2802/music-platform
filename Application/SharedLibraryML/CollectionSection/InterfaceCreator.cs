using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
namespace SharedLibraryML.CollectionSection
{
    public class InterfaceCreator
    {
        private ArtistCollectionType _type;
        private IArtistCollectionHelper? _helper;
        public InterfaceCreator(ArtistCollectionType type) 
        {
            _type = type;
            
            if (_type is ArtistCollectionType.ALBUM)
            { _helper = new HelperForAlbum(); }
            if (_type is ArtistCollectionType.AUDIOBOOK)
            { _helper = new HelperForAudiobook(); }
            
        }

       public IArtistCollectionHelper? GetArtistCollectionHelper() 
        {
            return _helper;
        }

        public ArtistCollectionType GetArtistCollectionType()
        {
            return _type;
        }
      
    }
}
