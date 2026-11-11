using ML_WebApp.Services;
using SharedLibraryML.CollectionSection;
using SharedLibraryML.MediaSection;
using ML_WebApp.MDA;

namespace ML_WebApp.Mappers.MediaM
{
    public interface IMediaMapperStrategy
    {
        MDAMedia MapToMDA(Media media, IFile fileService);
        Media MapToMedia(MDAMedia mda, IFile fileService);
    }
}
