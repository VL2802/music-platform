using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ML_LogicLayer.Security;
using ML_LogicLayer.Validators;
using SharedLibraryML.Interfaces;
using SharedLibraryML.MediaSection;

    namespace ML_LogicLayer.Services
    {
        public class MediaService
        {
            private readonly IMediaRepository _mediaRepository;
            private readonly IMediaDeletionValidator _mediaDeletionValidator;
            private readonly ICurrentUserContext _currentUserContext;
            private readonly IAuthorizationService _authorizationService;
        public MediaService(ICurrentUserContext currentUserContext, IAuthorizationService authorizationService,
IMediaRepository mediaRepository, IMediaDeletionValidator mediaDeletionValidator)
            {
                _mediaRepository = mediaRepository;
            _mediaDeletionValidator = mediaDeletionValidator;
            _currentUserContext = currentUserContext;

        }

        public void CreateMedia(Media media, int userId)
            {
            _authorizationService.EnsureAuthenticated();
            _authorizationService.EnsureArtist();
            _authorizationService.EnsureActiveAccount();
            _mediaRepository.CreateMedia(media, userId);
            }
            public Media? ReadMedia(int mediaId)
            {
                return _mediaRepository.ReadMedia(mediaId);
            }
            public void UpdateMedia(Media media)
            {
            var existingMedia = _mediaRepository.ReadMedia(media.GetIdDB());
            

            if (string.IsNullOrEmpty(media.GetFilePath()))
            {
                media.SetAudioFile(existingMedia.GetFilePath());
            }
           

            if (string.IsNullOrEmpty(media.GetImage()))
            {
                media.SetImage(existingMedia.GetImage());
            }
           
            _mediaRepository.UpdateMedia(media);
            }
            public void DeleteMedia(int mediaId)
            {
              _authorizationService.EnsureAuthenticated();

            if (!_mediaDeletionValidator.CanDeleteMedia(mediaId, out var collectionsOnlyWithThisMedia))
                {
                 var collectionNames = string.Join(", ", collectionsOnlyWithThisMedia.Select(x => x.Title));
                   throw new InvalidOperationException(
                    $"Cannot delete media. It's the last item in these collections: {collectionNames}. Delete these collections first.");
            }

          


            _mediaRepository.DeleteMedia(mediaId);
            }
            public List<(Media, int)> ReadAllMedia()
            {
                return _mediaRepository.ReadAllMedia();
            }
            public List<Media> ReadMediaPerUser()
            {
                _authorizationService.EnsureAuthenticated();

                int currentUserId = _currentUserContext.UserId;
                return _mediaRepository.ReadMediaPerUser(currentUserId);
             }

    }

   
}

