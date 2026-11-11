using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraryML.CollectionSection
{
    public class Playlist : Collection
    {
        private string? _description;
        private bool _privacy;

        public Playlist(string title, string image, string description, bool privacy) : base(title, image)
        {
            _description = description;
            _privacy = privacy;
            _type = CollectionType.PLAYLIST;
        }

        public Playlist(int id, string title, string image, string description, bool privacy) : base(id, title, image)
        {
            _description = description;
            _privacy = privacy;
            _type= CollectionType.PLAYLIST;
        }

        public string GetDescription()
        { 
            return _description; }

        public bool IsPrivate() 
        { return _privacy; }

        public override CollectionType GetCollectionType()
        {
            return _type;
        }
    }
}
