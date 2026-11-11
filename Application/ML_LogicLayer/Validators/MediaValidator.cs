using SharedLibraryML.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ML_LogicLayer.Validators;
using SharedLibraryML.CollectionSection;

namespace ML_LogicLayer.Validators
{
   
    
    public class MediaDeletionValidator : IMediaDeletionValidator
    {
        private readonly ICollectionMediaRepository _collectionMediaRepo;
        private readonly ICollectionRepository _collectionRepo;

        public MediaDeletionValidator(ICollectionMediaRepository collectionMediaRepo, ICollectionRepository collectionRepo)
        {
            _collectionMediaRepo = collectionMediaRepo;
            _collectionRepo = collectionRepo;
        }

        public bool CanDeleteMedia(int mediaId, out List<(int Id, string Title)> collectionsOnlyWithThisMedia)
        {
            collectionsOnlyWithThisMedia = new List<(int, string)>();
            var collectionIds = _collectionMediaRepo.GetCollectionIdsByMediaId(mediaId);

            foreach (var collectionId in collectionIds)
            {
                if (_collectionMediaRepo.GetMediaIdsForCollection(collectionId).Count <= 1)
                {
                    var collection = _collectionRepo.ReadCollection(collectionId);
                    if (collection.HasValue)
                    {
                        collectionsOnlyWithThisMedia.Add((collectionId, collection.Value.Item1.GetTitle()));
                    }
                }
            }

            return !collectionsOnlyWithThisMedia.Any();
        }

      
    }
    
}
