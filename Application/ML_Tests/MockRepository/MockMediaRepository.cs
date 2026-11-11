using System;
using System.Collections.Generic;
using System.Linq;
using SharedLibraryML.Interfaces;
using SharedLibraryML.MediaSection;
using SharedLibraryML;

namespace ML_Tests.MockRepository
{
    public class MockMediaRepository : IMediaRepository
    {
        private List<(Media, int)> _medias = new List<(Media, int)>
        {
            (new Song(5, "test", "filepath", "filepath", new DateTime(2002, 12, 11), MainMusicGenre.ROCK, "subpop", "blabla"), 1)
        };

        private int _nextId = 6; 

        public void CreateMedia(Media media, int userId)
        {
            media.SetId(_nextId++);
            _medias.Add((media, userId));
        }

        public Media? ReadMedia(int mediaId)
        {
            return _medias.FirstOrDefault(u => u.Item1.GetIdDB() == mediaId).Item1;
        }

        public void UpdateMedia(Media media)
        {
            var index = _medias.FindIndex(u => u.Item1.GetIdDB() == media.GetIdDB());
            if (index != -1)
            {
                _medias[index] = (media, _medias[index].Item2);
            }
        }

        public void DeleteMedia(int mediaId)
        {
            _medias.RemoveAll(m => m.Item1.GetIdDB() == mediaId);
        }

        public List<(Media, int)> ReadAllMedia()
        {
            return _medias;
        }

        public List<Media> ReadMediaPerUser(int userId)
        {
            return _medias
                .Where(m => m.Item2 == userId)
                .Select(m => m.Item1)
                .ToList();
        }
    }
}
