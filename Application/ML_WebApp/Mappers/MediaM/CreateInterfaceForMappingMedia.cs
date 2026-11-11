using SharedLibraryML;
namespace ML_WebApp.Mappers.MediaM
{
    public class CreateInterfaceForMappingMedia
    {
        private MediaType _type;
        private IMediaMapperStrategy _strategy;
        public CreateInterfaceForMappingMedia(MediaType type)
        {
            _type = type;

            if (_type is MediaType.SONG)
            { _strategy = new SongMapStrategy(); }
            if (_type is MediaType.CHAPTER)
            { _strategy = new ChapterMapStrategy(); }

        }

        public IMediaMapperStrategy GetStrategy()
        {
            return _strategy;
        }

        public MediaType GetCollectionType()
        {
            return _type;
        }
    }
}
