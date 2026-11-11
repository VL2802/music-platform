using SharedLibraryML.Interfaces;
using SharedLibraryML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibraryML.CollectionSection;

namespace ML_DAL.CollectionDAL
{
    public class CollectionStrategyContext
    {
        private readonly CollectionType _type;
        private ICollectionStrategy _strategy;

        public CollectionStrategyContext(CollectionType type)
        {
            _type = type;

            if (_type is CollectionType.ARTISTCOLLECTION)
            { _strategy = new ArtistCollectionStrategy(); }
            if (_type is CollectionType.PLAYLIST)
            { _strategy = new PlaylistStrategy(); }

        }

        public ICollectionStrategy GetCollectionStrategy()
        {
            return _strategy;
        }

        public CollectionType GetCollectionType()
        {
            return _type;
        }
    }
}
