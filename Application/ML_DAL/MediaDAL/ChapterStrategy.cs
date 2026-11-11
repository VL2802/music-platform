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
    public class ChapterStrategy : IMediaStrategy
    {
        public void Create(Media media, int mediaId, SqlConnection conn)
        {
            var chapter = (Chapter)media;
            string query = @"
            INSERT INTO Chapters (Id, BookGenre)
            VALUES (@Id, @BookGenre)";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Id", mediaId);
                cmd.Parameters.AddWithValue("@BookGenre", chapter.GetGenre().ToString());
                cmd.ExecuteNonQuery();
            }
        }

        public Media? Read(int mediaId, SqlConnection conn)
        {
            string query = @"
            SELECT m.Id, m.UserId, m.Title, m.FileImage, m.FileAudio, m.Published,
                   c.BookGenre
            FROM Media m
            INNER JOIN Chapters c ON m.Id = c.Id
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
                        var genre = Enum.Parse<BookGenre>(reader.GetString(6));

                        return new Chapter(mediaId, title, image, fileAudio, published, genre);
                    }
                }
            }
            return null;
        }

        public void Update(Media media, SqlConnection conn)
        {
            var chapter = (Chapter)media;
            string query = @"
            UPDATE Chapters
            SET BookGenre = @BookGenre
            WHERE Id = @Id";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Id", chapter.GetIdDB());
                cmd.Parameters.AddWithValue("@BookGenre", chapter.GetGenre().ToString());
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int mediaId, SqlConnection conn)
        {
            using (var cmd = new SqlCommand("DELETE FROM Chapters WHERE Id = @Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", mediaId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
