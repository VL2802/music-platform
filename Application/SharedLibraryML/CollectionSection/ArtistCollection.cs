using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SharedLibraryML.MediaSection;


namespace SharedLibraryML.CollectionSection
{
    public class ArtistCollection : Collection
    {
        private readonly IArtistCollectionHelper _helper;

        public ArtistCollection(string title, string image, IArtistCollectionHelper helper) : base(title, image)
        {
            _helper = helper ?? throw new ArgumentNullException("requires a valid InterfaceCreator.");
            _type = CollectionType.ARTISTCOLLECTION;
           
        }

        public ArtistCollection(int id, string title, string image, IArtistCollectionHelper helper) : base(id, title, image)
        {
            _helper = helper ?? throw new ArgumentNullException("requires a valid InterfaceCreator.");
            _type= CollectionType.ARTISTCOLLECTION;

        }




        public override void AddMediaToCollection(Media media)
        {
            _helper.CheckMediaType(media);

            if (!_helper.CheckMediaListQuantity(this))
            {
                if (!this._title.EndsWith(" Part 1", StringComparison.OrdinalIgnoreCase))
                {
                    this._title += " Part 1";
                }

                throw new ArgumentException(
                    "This collection has exceeded the allowed media limit. " +
                    "The title was updated with 'Part 1'. Please create a new collection."
                );
            }

            base.AddMediaToCollection(media);
        }

        public override CollectionType GetCollectionType()
        {
            return _type;
        }

    }


}

