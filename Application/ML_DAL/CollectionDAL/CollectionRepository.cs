using Microsoft.Data.SqlClient;
using ML_DAL.MediaDAL;
using SharedLibraryML;
using SharedLibraryML.CollectionSection;
using SharedLibraryML.Interfaces;
using SharedLibraryML.MediaSection;
using System;
using ML_LogicLayer;
using System.Collections.Generic;
using System.Data;

namespace ML_DAL.CollectionDAL
{
    public class CollectionRepository : DatabaseConfiguration, ICollectionRepository
    {
        private readonly IMediaRepository _mediaRepository;
        private readonly ICollectionMediaRepository _collectionMediaRepository;

        public CollectionRepository(IMediaRepository mediaRepository, ICollectionMediaRepository collectionMediaRepository, string connString): base(connString)
        {
            _mediaRepository = mediaRepository;
            _collectionMediaRepository = collectionMediaRepository;
        }

        public CollectionRepository(string connectionString)
      : base(connectionString)
        {
        }


        #region Create
        public void CreateCollection(Collection collection, InterfaceCreator creator, int userId)
        {
            try
            {
                string query = @"
                    INSERT INTO Collections (Title, FileImage, CollectionType, UserId)
                    OUTPUT INSERTED.Id
                    VALUES (@Title, @FileImage, @CollectionType, @UserId)";

                int collectionId;

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Title", collection.GetTitle());
                        cmd.Parameters.AddWithValue("@FileImage", collection.GetImage());
                        cmd.Parameters.AddWithValue("@CollectionType", (int)collection.GetCollectionType());
                        cmd.Parameters.AddWithValue("@UserId", userId);

                        collectionId = (int)cmd.ExecuteScalar();
                        collection.SetId(collectionId);
                    }

                    var strategyContext = new CollectionStrategyContext(collection.GetCollectionType());
                    var strategy = strategyContext.GetCollectionStrategy();
                    strategy.Create(collection, collectionId, conn);
                }

                foreach (Media media in collection.GetMedias())
                {
                    _collectionMediaRepository.AddMediaToCollection(collectionId, media.GetIdDB());
                }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException("Database operation failed while creating collection.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occurred while creating collection.", ex);
            }
        }
        #endregion

        #region Read
        public (Collection, InterfaceCreator)? ReadCollection(int collectionId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    string typeQuery = "SELECT CollectionType FROM Collections WHERE Id = @Id";
                    CollectionType collectionType;
                    using (SqlCommand cmd = new SqlCommand(typeQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", collectionId);
                        var result = cmd.ExecuteScalar();
                        if (result == null) return null;
                        collectionType = (CollectionType)Enum.Parse(typeof(CollectionType), result.ToString());
                    }

                    var strategyContext = new CollectionStrategyContext(collectionType);
                    var strategy = strategyContext.GetCollectionStrategy();
                    var finalResult = strategy.Read(collectionId, conn);

                    if (finalResult != null)
                    {
                        var mediaIds = _collectionMediaRepository.GetMediaIdsForCollection(collectionId);
                        foreach (int mediaId in mediaIds)
                        {
                            var media = _mediaRepository.ReadMedia(mediaId);
                            if (media != null)
                            {
                                finalResult.Value.Item1.AddMediaToCollection(media);
                            }
                        }
                    }

                    return finalResult;
                }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException("Database operation failed while reading collection.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occurred while reading collection.", ex);
            }
        }

        public List<(Collection, InterfaceCreator)> ReadAllCollections()
        {
            try
            {
                var result = new List<(Collection, InterfaceCreator)>();
                string query = "SELECT Id FROM Collections";

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int collectionId = reader.GetInt32(0);
                                var collection = ReadCollection(collectionId);
                                if (collection != null)
                                {
                                    result.Add(collection.Value);
                                }
                            }
                        }
                    }
                }

                return result;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException("Database operation failed while reading all collections.", ex);
            }
        }

        public List<(Collection, InterfaceCreator)> ReadCollectionsForUser(int userId)
        {
            try
            {
                var result = new List<(Collection, InterfaceCreator)>();

                string query = "SELECT Id FROM Collections WHERE UserId = @UserId";

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int collectionId = reader.GetInt32(0);
                                var collection = ReadCollection(collectionId);
                                if (collection != null)
                                {
                                    result.Add(collection.Value);
                                }
                            }
                        }
                    }
                }

                return result;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException("Database operation failed while reading user collections.", ex);
            }
        }
        #endregion

        #region Update
        public void UpdateCollection(Collection collection, InterfaceCreator creator)
        {
            try
            {
                string updateQuery = @"
                    UPDATE Collections
                    SET Title = @Title, FileImage = @FileImage
                    WHERE Id = @Id";

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", collection.GetIdDB());
                        cmd.Parameters.AddWithValue("@Title", collection.GetTitle());
                        cmd.Parameters.AddWithValue("@FileImage", collection.GetImage());
                        cmd.ExecuteNonQuery();
                    }

                    var strategyContext = new CollectionStrategyContext(collection.GetCollectionType());
                    var strategy = strategyContext.GetCollectionStrategy();
                    strategy.Update(collection, conn);
                }

                _collectionMediaRepository.RemoveAllMediaFromCollection(collection.GetIdDB());
                foreach (Media media in collection.GetMedias())
                {
                    _collectionMediaRepository.AddMediaToCollection(collection.GetIdDB(), media.GetIdDB());
                }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException("Database operation failed while updating collection.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occurred while updating collection.", ex);
            }
        }
        #endregion

        #region Delete
        public void DeleteCollection(int collectionId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    CollectionType type;
                    using (SqlCommand typeCmd = new SqlCommand(
                        "SELECT CollectionType FROM Collections WHERE Id = @Id", conn))
                    {
                        typeCmd.Parameters.AddWithValue("@Id", collectionId);
                        var result = typeCmd.ExecuteScalar();
                        if (result == null) return;
                        type = (CollectionType)Enum.Parse(typeof(CollectionType), result.ToString());
                    }

                    _collectionMediaRepository.RemoveAllMediaFromCollection(collectionId);

                    var strategyContext = new CollectionStrategyContext(type);
                    var strategy = strategyContext.GetCollectionStrategy();
                    strategy.Delete(collectionId, conn);

                    using (var cmd = new SqlCommand("DELETE FROM Collections WHERE Id = @Id", conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", collectionId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException("Database operation failed while deleting collection.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occurred while deleting collection.", ex);
            }
        }
        #endregion
    }
}