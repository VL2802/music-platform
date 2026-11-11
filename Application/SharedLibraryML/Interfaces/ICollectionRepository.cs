using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibraryML.CollectionSection;
namespace SharedLibraryML.Interfaces
{
    public interface ICollectionRepository
    {
        void CreateCollection(Collection collection, InterfaceCreator creator, int userId);
        (Collection, InterfaceCreator)? ReadCollection(int collectionId);
        void UpdateCollection(Collection collection, InterfaceCreator creator);
        void DeleteCollection(int collectionId);
        List<(Collection, InterfaceCreator)> ReadAllCollections();
        List<(Collection, InterfaceCreator)> ReadCollectionsForUser(int userId);
    }
}
