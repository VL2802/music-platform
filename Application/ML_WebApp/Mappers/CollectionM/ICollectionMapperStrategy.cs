using ML_LogicLayer.Services;
using ML_WebApp.Services;
using SharedLibraryML.CollectionSection;

namespace ML_WebApp.Mappers.CollectionM
{
    public interface ICollectionMappingStrategy
    {
        MDACollection MapToMDA(Collection collection, InterfaceCreator creator, IFile fileService);
        Collection MapToCollection(MDACollection mda, IFile fileService, IArtistCollectionHelper helper);
    }
}
