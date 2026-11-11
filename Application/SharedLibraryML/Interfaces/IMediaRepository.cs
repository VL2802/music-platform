using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibraryML.MediaSection;
namespace SharedLibraryML.Interfaces
{
    public interface IMediaRepository
    {
        void CreateMedia(Media media, int id);
        Media? ReadMedia(int mediaId);
        void UpdateMedia(Media media);
        void DeleteMedia(int mediaId);
        List<(Media, int)> ReadAllMedia();
        List<Media> ReadMediaPerUser(int userId);
    }
}
