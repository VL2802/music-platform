using Microsoft.Data.SqlClient;
using SharedLibraryML.CollectionSection;
using SharedLibraryML.Interfaces;
using SharedLibraryML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_DAL.CollectionDAL
{
    public class ArtistCollectionStrategy : ICollectionStrategy
    {

        public ArtistCollectionStrategy()
        {
        }

        public void Create(Collection collection, int collectionId, SqlConnection conn)
        {
            var artistCollection = (ArtistCollection)collection;
            string query = @"
            INSERT INTO ArtistCollections (Id, ArtistCollectionType)
            VALUES (@Id, @ArtistCollectionType)";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Id", collectionId);
                cmd.Parameters.AddWithValue("@ArtistCollectionType", (int)artistCollection.GetCollectionType());
                cmd.ExecuteNonQuery();
            }
        }

        public (Collection, InterfaceCreator)? Read(int collectionId, SqlConnection conn)
        {
            string query = @"
            SELECT c.Id, c.Title, c.FileImage,
                   a.ArtistCollectionType, c.UserId
            FROM Collections c
            INNER JOIN ArtistCollections a ON c.Id = a.Id
            WHERE c.Id = @Id";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Id", collectionId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("Id"));
                        string title = reader.GetString(reader.GetOrdinal("Title"));
                        string image = reader.GetString(reader.GetOrdinal("FileImage"));
                        ArtistCollectionType artistCollectionType = (ArtistCollectionType)reader.GetInt32(reader.GetOrdinal("ArtistCollectionType"));

                        var creator = new InterfaceCreator(artistCollectionType);
                        var helper = creator.GetArtistCollectionHelper();
                        var artistCollection = new ArtistCollection(id, title, image, helper);

                        return (artistCollection, creator);
                    }
                }
            }
            return null;
        }

        public void Update(Collection collection, SqlConnection conn)
        {
            var artistCollection = (ArtistCollection)collection;
            string query = @"
            UPDATE ArtistCollections
            SET ArtistCollectionType = @ArtistCollectionType
            WHERE Id = @Id";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Id", artistCollection.GetIdDB());
                cmd.Parameters.AddWithValue("@ArtistCollectionType", (int)artistCollection.GetCollectionType());
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int collectionId, SqlConnection conn)
        {
            using (var cmd = new SqlCommand("DELETE FROM ArtistCollections WHERE Id = @Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", collectionId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
