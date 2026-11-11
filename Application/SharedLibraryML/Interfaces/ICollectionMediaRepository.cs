using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraryML.Interfaces
{
    public interface ICollectionMediaRepository
    {
        void AddMediaToCollection(int collectionId, int mediaId);
        void RemoveAllMediaFromCollection(int collectionId);
        List<int> GetMediaIdsForCollection(int collectionId);
        List<int> GetCollectionIdsByMediaId(int mediaId);
    }
}
