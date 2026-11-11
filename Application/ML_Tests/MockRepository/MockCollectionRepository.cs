using System;
using System.Collections.Generic;
using System.Linq;
using SharedLibraryML.CollectionSection;
using SharedLibraryML;
using SharedLibraryML.Interfaces;
using ML_LogicLayer;

namespace ML_Tests.MockRepository
{
    public class MockCollectionRepository : ICollectionRepository
    {
        private List<(Collection, InterfaceCreator)> _collections = new List<(Collection, InterfaceCreator)>
        {
            (new ArtistCollection(1,"Testik", "", new HelperForAudiobook()),
             new InterfaceCreator(ArtistCollectionType.AUDIOBOOK))
        };

        private int _nextId = 1;
        private int userId = 1;

        public void CreateCollection(Collection collection, InterfaceCreator creator, int userId)
        {
            collection.SetId(_nextId++);
            _collections.Add((collection, creator));
        }

        public (Collection, InterfaceCreator)? ReadCollection(int collectionId)
        {
            var result = _collections.FirstOrDefault(c => c.Item1.GetIdDB() == collectionId);

            return (result.Item1, result.Item2);
        }

        public void UpdateCollection(Collection collection, InterfaceCreator creator)
        {
            var index = _collections.FindIndex(c => c.Item1.GetIdDB() == collection.GetIdDB());
            if (index != -1)
            {
                //var userId = _collections[index].Item3; 
                _collections[index] = (collection, creator);
            }

           
        }

        public void DeleteCollection(int collectionId)
        {
            _collections.RemoveAll(c => c.Item1.GetIdDB() == collectionId);
        }

        public List<(Collection, InterfaceCreator)> ReadAllCollections()
        {
            return _collections;
        }

        public List<(Collection, InterfaceCreator)> ReadCollectionsForUser(int userId)
        {
           throw new NotImplementedException();
        }
    }

}
