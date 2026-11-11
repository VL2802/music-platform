using ML_LogicLayer.Services;
using ML_WebApp.Services;
using SharedLibraryML.CollectionSection;

namespace ML_WebApp.Mappers.CollectionM
{
    public class PlaylistMappingStrategy : ICollectionMappingStrategy
    {
        public MDACollection MapToMDA(Collection collection, InterfaceCreator creator, IFile fileService)
        {
            var playlist = (Playlist)collection;
            var fileImage = fileService.LoadFileAsFormFile(playlist.GetImage());
            int privacyValue = playlist.IsPrivate() ? 0 : 1;


            return new MDACollection
            {
                CollectionId = playlist.GetIdDB(),
                CollectionTitle = playlist.GetTitle(),
                Description = playlist.GetDescription(),
                Privacy = privacyValue,
                FileImage = fileImage,
                MediaIds = new List<int>()
            };
        }

        public Collection MapToCollection(MDACollection mda, IFile fileService, IArtistCollectionHelper helper)
        {
            string imagePath = fileService.SaveImageFile(mda.FileImage);
            bool isPrivate = mda.Privacy == 0;

            return new Playlist(
                mda.CollectionId,
                mda.CollectionTitle,
                imagePath,
                mda.Description,
                isPrivate);

        }

    }
}
