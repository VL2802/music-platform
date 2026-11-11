using SharedLibraryML.CollectionSection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_LogicLayer.Validators
{
    public interface IMediaDeletionValidator
    {
        bool CanDeleteMedia(int mediaId, out List<(int Id, string Title)> collectionsOnlyWithThisMedia);
    }
}
