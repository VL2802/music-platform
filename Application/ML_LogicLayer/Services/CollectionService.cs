using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibraryML.MediaSection;
using SharedLibraryML.CollectionSection;
using SharedLibraryML.Interfaces;
using SharedLibraryML;
using ML_LogicLayer.Validators;
namespace ML_LogicLayer.Services
{
    public class CollectionService
    {
        private readonly ICollectionRepository _collectionRepository;


        public CollectionService(ICollectionRepository collectionRepository)
        {
            _collectionRepository = collectionRepository;
        }

        public void CreateCollection(Collection collection, InterfaceCreator? creator, int userId)
        {
            CollectionValidator.Validate(collection);

            _collectionRepository.CreateCollection(collection, creator, userId);
        }
        public (Collection, InterfaceCreator)? ReadCollection(int collectionId)
        {
            return _collectionRepository.ReadCollection(collectionId);
        }
        public void UpdateCollection(Collection collection, InterfaceCreator? creator)
        {

           CollectionValidator.Validate(collection);
            var existingCollectionTuple = _collectionRepository.ReadCollection(collection.GetIdDB());


            var (existingCollection, _) = existingCollectionTuple.Value;

            if (string.IsNullOrEmpty(collection.GetImage()))
            {
                collection.SetImage(existingCollection.GetImage());
            }

            _collectionRepository.UpdateCollection(collection, creator);
        }
        public void DeleteCollection(int collectionId)
        {
            _collectionRepository.DeleteCollection(collectionId);
        }
        public List<(Collection, InterfaceCreator)> ReadAllCollections()
        {
            var allCollections = _collectionRepository.ReadAllCollections();

            var filteredCollections = allCollections
                .Where(c =>
                    !(c.Item1 is Playlist playlist && playlist.IsPrivate()))
                .ToList();

            return filteredCollections;
        }
        public List<(Collection, InterfaceCreator)> ReadCollectionsForUser(int userId)
        {
            return _collectionRepository.ReadCollectionsForUser(userId);
        }
    }
}
