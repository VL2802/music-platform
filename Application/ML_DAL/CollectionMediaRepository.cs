using Microsoft.Data.SqlClient;
using SharedLibraryML.MediaSection;
using System;
using System.Collections.Generic;
using SharedLibraryML.Interfaces;
using ML_LogicLayer;
namespace ML_DAL
{
    public class CollectionMediaRepository : DatabaseConfiguration, ICollectionMediaRepository
    {
      

        public void AddMediaToCollection(int collectionId, int mediaId)
        {
            try
            {
                string query = @"
                    INSERT INTO CollectionMedia(CollectionId, MediaId)
                    VALUES (@CollectionId, @MediaId)";

                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@CollectionId", collectionId);
                    cmd.Parameters.AddWithValue("@MediaId", mediaId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException("Database operation failed while adding media to collection.", ex);
            }
        }

        public void RemoveAllMediaFromCollection(int collectionId)
        {
            try
            {
                string query = @"DELETE FROM CollectionMedia WHERE CollectionId = @CollectionId";

                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@CollectionId", collectionId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException("Database operation failed while removing media from collection.", ex);
            }
        }

        public List<int> GetMediaIdsForCollection(int collectionId)
        {
            var mediaIds = new List<int>();

            try
            {
                string query = @"SELECT MediaId FROM CollectionMedia WHERE CollectionId = @CollectionId";

                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@CollectionId", collectionId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            mediaIds.Add(reader.GetInt32(0));
                        }
                    }
                }
                return mediaIds;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException("Database operation failed while getting media for collection.", ex);
            }
        }

        public List<int> GetCollectionIdsByMediaId(int mediaId)
        {
            var collectionIds = new List<int>();
            string query = "SELECT CollectionId FROM CollectionMedia WHERE MediaId = @MediaId";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MediaId", mediaId);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        collectionIds.Add(reader.GetInt32(0));
                    }
                }
            }
            return collectionIds;
        }
    }
}