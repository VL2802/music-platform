using ML_WebApp.MDA;
using ML_WebApp.Services;
using SharedLibraryML.MediaSection;
using SharedLibraryML;

namespace ML_WebApp.Mappers.MediaM
{
    public class MediaMapper
    {
        private readonly IFile _fileService;

        public MediaMapper(IFile fileService)
        {
            _fileService = fileService;
        }

        public MDAMedia MapMediaToMDAMedia(Media media, IMediaMapperStrategy strategy)
        {

            return strategy.MapToMDA(media, _fileService);
        }

        public Media MapMDAMediaToMedia(MDAMedia mda, IMediaMapperStrategy strategy)
        {
            return strategy.MapToMedia(mda, _fileService);

        }
    }
}