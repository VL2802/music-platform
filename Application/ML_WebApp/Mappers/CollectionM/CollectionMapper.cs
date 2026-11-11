using Microsoft.AspNetCore.Http.HttpResults;
using ML_LogicLayer.Services;
using ML_WebApp.MDA;
using ML_WebApp.Services;
using SharedLibraryML;
using SharedLibraryML.CollectionSection;
using SharedLibraryML.MediaSection;

namespace ML_WebApp.Mappers.CollectionM
{
    public class CollectionMapper
    {
        private readonly IFile _fileService;
        private readonly MediaService _mediaService;
        public CollectionMapper(IFile fileService, MediaService mediaService)
        {
            _fileService = fileService;
            _mediaService = mediaService;
        }

        public MDACollection MapCollectionToMDACollection(Collection collection, InterfaceCreator creator, ICollectionMappingStrategy mappingStrategy)
        {


            MDACollection MDAcollection = mappingStrategy.MapToMDA(collection, creator, _fileService);
            var mediaList = collection.GetMedias();
            foreach (var media in mediaList)
            {
                MDAcollection.MediaIds.Add(media.GetIdDB());
            }

            return MDAcollection;
        }


        public Collection MapMDACollectionToCollection(MDACollection mda, IArtistCollectionHelper helper, ICollectionMappingStrategy mappingStrategy)
        {

            Collection collection = mappingStrategy.MapToCollection(mda, _fileService, helper);


            var listOfMediaIds = mda.MediaIds;
            foreach (var mediaId in listOfMediaIds)
            {
                var actualmedia = _mediaService.ReadMedia(mediaId);

                collection.AddMediaToCollection(actualmedia);


            }
            return collection;
        }


    }
}
