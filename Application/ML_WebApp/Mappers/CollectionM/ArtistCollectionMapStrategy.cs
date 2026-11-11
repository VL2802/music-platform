using ML_LogicLayer.Services;
using ML_WebApp.Services;
using SharedLibraryML.CollectionSection;

namespace ML_WebApp.Mappers.CollectionM
{
    public class ArtistCollectionMappingStrategy : ICollectionMappingStrategy
    {
        public MDACollection MapToMDA(Collection collection, InterfaceCreator creator, IFile fileService)
        {
            var artistCollection = (ArtistCollection)collection;
            var fileImage = fileService.LoadFileAsFormFile(artistCollection.GetImage());

            return new MDACollection
            {
                CollectionId = artistCollection.GetIdDB(),
                CollectionTitle = artistCollection.GetTitle(),
                Type = creator.GetArtistCollectionType(),
                FileImage = fileImage,
                MediaIds = new List<int>()
            };
        }

        public Collection MapToCollection(MDACollection mda, IFile fileService, IArtistCollectionHelper helper)
        {
            string imagePath = fileService.SaveImageFile(mda.FileImage);

            return new ArtistCollection(
                mda.CollectionId,
                mda.CollectionTitle,
                imagePath,
                helper
            );


        }
    }
}
