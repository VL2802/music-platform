using Microsoft.Data.SqlClient;
using SharedLibraryML.MediaSection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_DAL.MediaDAL
{
    public interface IMediaStrategy
    {
        void Create(Media media, int mediaId, SqlConnection conn);
        Media? Read(int mediaId, SqlConnection conn);
        void Update(Media media, SqlConnection conn);
        void Delete(int mediaId, SqlConnection conn);
    }
}
