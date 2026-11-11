using Microsoft.Data.SqlClient;
using SharedLibraryML.CollectionSection;
using SharedLibraryML.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_DAL.CollectionDAL
{
    public class PlaylistStrategy : ICollectionStrategy
    {

        public PlaylistStrategy()
        {
        }

        public void Create(Collection collection, int collectionId, SqlConnection conn)
        {
            var playlist = (Playlist)collection;
            string query = @"
            INSERT INTO Playlists (Id, Privacy, Description)
            VALUES (@Id, @Privacy, @Description)";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Id", collectionId);
                cmd.Parameters.AddWithValue("@Privacy", playlist.IsPrivate() ? 1 : 0);
                cmd.Parameters.AddWithValue("@Description", playlist.GetDescription());
                cmd.ExecuteNonQuery();
            }
        }

        public (Collection, InterfaceCreator)?Read(int collectionId, SqlConnection conn)
        {
            string query = @"
            SELECT c.Id, c.Title, c.FileImage, p.Privacy, p.Description, c.UserId
            FROM Collections c
            INNER JOIN Playlists p ON c.Id = p.Id
            WHERE c.Id = @Id";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Id", collectionId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string title = reader.GetString(1);
                        string image = reader.GetString(2);
                        bool privacy = reader.GetBoolean(3);
                        var description = reader.GetString(4);

                        var playlist = new Playlist(collectionId, title, image, description, privacy);
                        return (playlist, null);
                    }
                }
            }
            return null;
        }

        public void Update(Collection collection, SqlConnection conn)
        {
            var playlist = (Playlist)collection;
            string query = @"
            UPDATE Playlists
            SET Privacy = @Privacy, Description = @Description
            WHERE Id = @Id";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Id", playlist.GetIdDB());
                cmd.Parameters.AddWithValue("@Privacy", playlist.IsPrivate() ? 1 : 0);
                cmd.Parameters.AddWithValue("@Description", playlist.GetDescription());
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int collectionId, SqlConnection conn)
        {
            using (var cmd = new SqlCommand("DELETE FROM Playlists WHERE Id = @Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", collectionId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
