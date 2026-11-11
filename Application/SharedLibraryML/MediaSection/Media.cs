using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraryML.MediaSection
{
    public abstract class Media
    {
        private int _id;
        private string _title;
        private string? _image;
        private string _filePath;
        private DateTime _published;
        protected MediaType _type;


        protected Media(string title, string image, string filePath, DateTime published)
        {
            _id = 0;
            _title = title ?? throw new ArgumentNullException("The title cannot be null");
            _image = image;
            _filePath = filePath ?? throw new ArgumentNullException("The file cannot be null");
            _published = published;

        }

        protected Media(int id, string title, string image, string filePath, DateTime published)
        {
            _id = id;
            _title = title ?? throw new ArgumentNullException("The title cannot be null");
            _image = image;
            _filePath = filePath ?? throw new ArgumentNullException("The file cannot be null");
            _published = published;

        }
        public int GetIdDB()
        { return _id; }
        public string GetIdUI()
        {
            return $"MEDIA_{_id}";
        }
        public void SetId(int id)
        {
            if (_id != 0)
                throw new InvalidOperationException("ID is already set.");
            _id = id;
        }

        public string GetTitle()
        { return _title; }
        public string? GetImage() 
        { return _image;}
        public string GetFilePath() 
        { return _filePath;}
        public DateTime GetPublished()
        { return _published;}

        public abstract string GetGenre();
        public abstract MediaType GetMediaType();

        public void SetAudioFile(string audioFilePath)
        {
            this._filePath = audioFilePath;
        }

        public void SetImage(string imageFilePath)
        {
            this._image = imageFilePath;
        }


    }
}
