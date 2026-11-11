using Microsoft.Data.SqlClient;
using SharedLibraryML.MediaSection;
using SharedLibraryML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_DAL.MediaDAL
{
    public class SongStrategy : IMediaStrategy
    {
        public void Create(Media media, int mediaId, SqlConnection conn)
        {
            var song = (Song)media;
            string query = @"
            INSERT INTO Songs (Id, MusicGenre, SubGenre, Lyrics)
            VALUES (@Id, @MusicGenre, @SubGenre, @Lyrics)";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Id", mediaId);
                cmd.Parameters.AddWithValue("@MusicGenre", song.GetGenre().ToString());
                cmd.Parameters.AddWithValue("@SubGenre", song.GetSubGenre());
                cmd.Parameters.AddWithValue("@Lyrics", song.GetLyrics());
                cmd.ExecuteNonQuery();
            }
        }

        public Media? Read(int mediaId, SqlConnection conn)
        {
            string query = @"
            SELECT m.Id, m.UserId, m.Title, m.FileImage, m.FileAudio, m.Published,
                   s.MusicGenre, s.SubGenre, s.Lyrics
            FROM Media m
            INNER JOIN Songs s ON m.Id = s.Id
            WHERE m.Id = @Id";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Id", mediaId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int userId = reader.GetInt32(1);
                        string title = reader.GetString(2);
                        string image = reader.GetString(3);
                        string fileAudio = reader.GetString(4);
                        DateTime published = reader.GetDateTime(5);
                        var genre = Enum.Parse<MainMusicGenre>(reader.GetString(6));
                        var subGenre = reader.GetString(7);
                        string lyrics = reader.GetString(8);

                        return new Song(mediaId, title, image, fileAudio, published, genre, subGenre, lyrics);
                    }
                }
            }
            return null;
        }

        public void Update(Media media, SqlConnection conn)
        {
            var song = (Song)media;
            string query = @"
            UPDATE Songs
            SET MusicGenre = @MusicGenre, SubGenre = @SubGenre, Lyrics = @Lyrics
            WHERE Id = @Id";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Id", song.GetIdDB());
                cmd.Parameters.AddWithValue("@MusicGenre", song.GetGenre().ToString());
                cmd.Parameters.AddWithValue("@SubGenre", song.GetSubGenre());
                cmd.Parameters.AddWithValue("@Lyrics", song.GetLyrics());
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int mediaId, SqlConnection conn)
        {
            using (var cmd = new SqlCommand("DELETE FROM Songs WHERE Id = @Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", mediaId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
