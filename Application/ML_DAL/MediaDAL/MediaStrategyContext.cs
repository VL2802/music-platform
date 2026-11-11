using ML_DAL.CollectionDAL;
using SharedLibraryML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_DAL.MediaDAL
{
    public class MediaStrategyContext
    {
        private readonly MediaType _type;
        private IMediaStrategy _strategy;

        public MediaStrategyContext(MediaType type)
        {
            _type = type;

            if (_type is MediaType.CHAPTER)
            { _strategy = new ChapterStrategy(); }
            if (_type is MediaType.SONG)
            { _strategy = new SongStrategy(); }

        }

        public IMediaStrategy GetMediaStrategy()
        {
            return _strategy;
        }

        public MediaType GetMediaType()
        {
            return _type;
        }
    }
}
