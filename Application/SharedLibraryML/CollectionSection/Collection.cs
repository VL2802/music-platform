using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SharedLibraryML.MediaSection;

namespace SharedLibraryML.CollectionSection
{
    public abstract class Collection
    {
        private int _id;
        protected string _title;
        private string _image;
        protected List<Media> _mediaList;
        protected CollectionType _type;

        protected Collection(string title, string image) 
        {
            _id = 0;
            _title = title ?? throw new ArgumentNullException("Enter the title");
            _image = image ?? "";
            _mediaList = new List<Media>();
            
        }

        protected Collection(int id, string title, string image)
        {
            _id = id;
            _title = title ?? throw new ArgumentNullException("Enter the title");
            _image = image ?? "";
            _mediaList = new List<Media>();
            
            
        }

        public void SetId(int id)
        {
            if (_id != 0)
                throw new InvalidOperationException("ID is already set.");
            _id = id;
        }
        public int GetIdDB()
        { return _id; } 
        public string GetIdUI()
        { return $"COLLECTION_{_id}"; }

        public string GetTitle()
        { return _title; }

       
        public int GetQuantity() 
        { return _mediaList.Count; }
        public string? GetImage()
        { return _image;}
        public virtual List<Media> GetMedias()
        {
           
            return _mediaList;
        }
        public DateTime GetFirstPublished()
        {
            if (_mediaList == null || _mediaList.Count == 0)
            {
                throw new InvalidOperationException("The media list is empty");
            }

            DateTime _published = DateTime.MaxValue;

            foreach (Media media in _mediaList)
            {
                DateTime mediaPublished = media.GetPublished();
                if (mediaPublished < _published)
                {
                    _published = mediaPublished;
                }
            }

            return _published;
        }


        public virtual void AddMediaToCollection(Media media)
        {
            if (media == null)
            {
                throw new ArgumentNullException(nameof(media));
            }


            if (_mediaList.Contains(media))
            {
                throw new ArgumentException($"This collection already contains the {media.GetTitle()}.");
            }

           

            _mediaList.Add(media);

        }
        public List<string> GetGenres()
        {
            List<string> genreList = new List<string>();
            if (_mediaList == null || _mediaList.Count == 0)
            {
                throw new InvalidOperationException("The media list is empty");
            }
            foreach (Media media in _mediaList)
            {
                string genre = media.GetGenre();
                genreList.Add(genre);

            }
            genreList.Distinct().ToList();
            return genreList;
        }

        public abstract CollectionType GetCollectionType();

        public void SetImage(string imageFilePath)
        {
            this._image = imageFilePath;
        }
    }
}
