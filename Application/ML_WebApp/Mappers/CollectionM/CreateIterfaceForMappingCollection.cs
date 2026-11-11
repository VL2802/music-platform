using SharedLibraryML.CollectionSection;
using SharedLibraryML;

namespace ML_WebApp.Mappers.CollectionM
{
    public class CreateInterfaceForMapping
    {
        private CollectionType _type;
        private ICollectionMappingStrategy _strategy;
        public CreateInterfaceForMapping(CollectionType type)
        {
            _type = type;

            if (_type is CollectionType.ARTISTCOLLECTION)
            { _strategy = new ArtistCollectionMappingStrategy(); }
            if (_type is CollectionType.PLAYLIST)
            { _strategy = new PlaylistMappingStrategy(); }

        }

        public ICollectionMappingStrategy GetStrategy()
        {
            return _strategy;
        }

        public CollectionType GetCollectionType()
        {
            return _type;
        }
    }
}
