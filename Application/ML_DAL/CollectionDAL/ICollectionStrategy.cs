using Microsoft.Data.SqlClient;
using SharedLibraryML.CollectionSection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_DAL.CollectionDAL
{
    public interface ICollectionStrategy
    {
        void Create(Collection collection, int collectionId, SqlConnection conn);
        (Collection, InterfaceCreator)? Read(int collectionId, SqlConnection conn);
        void Update(Collection collection, SqlConnection conn);
        void Delete(int collectionId, SqlConnection conn);
    }
}
