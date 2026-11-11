using Microsoft.Data.SqlClient;
using ML_DAL;
using ML_DAL.MediaDAL;
using SharedLibraryML.Interfaces;
using SharedLibraryML.MediaSection;
using SharedLibraryML;
using ML_LogicLayer;
using System.Net.Http.Headers;

public class MediaRepository : DatabaseConfiguration, IMediaRepository
{
    public MediaRepository(string connectionString)
       : base(connectionString)
    {
    }


    #region Create
    public void CreateMedia(Media media, int userId)
    {
        try
        {
            string query = @"
                INSERT INTO Media (UserId, Title, FileImage, FileAudio, Published, MediaType)
                OUTPUT INSERTED.Id
                VALUES (@UserId, @Title, @FileImage, @FileAudio, @Published, @MediaType)";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                int mediaId;
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Title", media.GetTitle());
                    cmd.Parameters.AddWithValue("@FileImage", media.GetImage());
                    cmd.Parameters.AddWithValue("@FileAudio", media.GetFilePath());
                    cmd.Parameters.AddWithValue("@Published", media.GetPublished());
                    cmd.Parameters.AddWithValue("@MediaType", (int)media.GetMediaType());

                    mediaId = (int)cmd.ExecuteScalar();
                    media.SetId(mediaId);
                }

                var strategyContext = new MediaStrategyContext(media.GetMediaType());
                var strategy = strategyContext.GetMediaStrategy();
                strategy.Create(media, mediaId, conn);
            }
        }
        catch (SqlException ex)
        {
            throw new DataAccessException("Database operation failed while creating media.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("Error creating media", ex);
        }
       
    }
    #endregion

    #region Read
    public Media? ReadMedia(int mediaId)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string typeQuery = "SELECT MediaType FROM Media WHERE Id = @Id";
                MediaType mediaType;
                using (SqlCommand cmd = new SqlCommand(typeQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", mediaId);
                    var result = cmd.ExecuteScalar();
                    if (result == null) return null;
                    mediaType = (MediaType)Enum.Parse(typeof(MediaType), result.ToString());
                }
                var strategyContext = new MediaStrategyContext(mediaType);
                var strategy = strategyContext.GetMediaStrategy();
                return strategy.Read(mediaId, conn);
            }
        }
        catch (SqlException ex)
        {
            throw new DataAccessException("Database operation failed while reading media.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("Error reading media", ex);
        }
    }

    public List<(Media, int)> ReadAllMedia()
    {
        try
        {
            var result = new List<(Media, int)>();
            string query = "SELECT Id, UserId FROM Media";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int mediaId = reader.GetInt32(0);
                            int userId = reader.GetInt32(1);

                            var media = ReadMedia(mediaId);
                            if (media != null)
                            {
                                result.Add((media, userId));
                            }
                        }
                    }
                }
            }

            return result;
        }
        catch (SqlException ex)
        {
            throw new DataAccessException("Database operation failed while reading all media.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("Error reading all media", ex);
        }
    }

    public List<Media> ReadMediaPerUser(int userId)
    {
        try
        {
            var result = new List<Media>();
            string query = "SELECT Id FROM Media WHERE UserId = @UserId";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        var ids = new List<int>();
                        while (reader.Read())
                        {
                            ids.Add(reader.GetInt32(0));
                        }

                        foreach (var id in ids)
                        {
                            var media = ReadMedia(id);
                            if (media != null)
                            {
                                result.Add(media);
                            }
                        }
                    }
                }
            }

            return result;
        }
        catch (SqlException ex)
        {
            throw new DataAccessException("Database operation failed while reading all media.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("Error reading user media", ex);
        }
    }
    #endregion

    #region Update
    public void UpdateMedia(Media media)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string updateMediaQuery = @"
                    UPDATE Media
                    SET Title = @Title, FileImage = @FileImage, FileAudio = @FileAudio, Published = @Published
                    WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(updateMediaQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", media.GetIdDB());
                    cmd.Parameters.AddWithValue("@Title", media.GetTitle());
                    cmd.Parameters.AddWithValue("@FileImage", media.GetImage());
                    cmd.Parameters.AddWithValue("@FileAudio", media.GetFilePath());
                    cmd.Parameters.AddWithValue("@Published", media.GetPublished());
                    cmd.ExecuteNonQuery();
                }

                var strategyContext = new MediaStrategyContext(media.GetMediaType());
                var strategy = strategyContext.GetMediaStrategy();
                strategy.Update(media, conn);
            }
        }
        catch (SqlException ex)
        {
            throw new DataAccessException("Database operation failed while modifying media.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("Error updating media", ex);
        }
    }
    #endregion

    #region Delete
    public void DeleteMedia(int mediaId)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand("DELETE FROM CollectionMedia WHERE MediaId = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", mediaId);
                    cmd.ExecuteNonQuery();
                }

                string typeQuery = "SELECT MediaType FROM Media WHERE Id = @Id";
                MediaType mediaType;
                using (SqlCommand cmd = new SqlCommand(typeQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", mediaId);
                    var result = cmd.ExecuteScalar();
                    if (result == null) return;
                    mediaType = (MediaType)Enum.Parse(typeof(MediaType), result.ToString());
                }

                var strategyContext = new MediaStrategyContext(mediaType);
                var strategy = strategyContext.GetMediaStrategy();
                strategy.Delete(mediaId, conn);

                using (var cmd = new SqlCommand("DELETE FROM Media WHERE Id = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", mediaId);
                    cmd.ExecuteNonQuery();
                }
               

            }
        }
        catch (SqlException ex)
        {
            throw new DataAccessException("Database operation failed while deleting media.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("Error deleting media", ex);
        }
    }
    #endregion
}