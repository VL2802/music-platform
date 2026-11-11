using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ML_LogicLayer;
using SharedLibraryML.CollectionSection;
using SharedLibraryML.MediaSection;
using SharedLibraryML;
using ML_LogicLayer.Services;
using ML_WebApp.MDA;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text.RegularExpressions;
using System.Text;
using Microsoft.Data.SqlClient;
using ML_WebApp.Services;
using Microsoft.AspNetCore.Http;
using ML_WebApp.Mappers.CollectionM;
using ML_WebApp.Mappers.MediaM;

namespace ML_WebApp.Pages
{
    [Authorize(Policy = "ArtistOnly")]
    public class ArtistPageModel : PageModel
    {
        private readonly MediaService _mediaService;
        private readonly CollectionService _collectionService;
        private readonly UserContextService _userContextService;
        private readonly MediaMapper _mediaMapper;
        private readonly CollectionMapper _collectionMapper;

        public ArtistPageModel(MediaService mediaService, CollectionService collectionService, MediaMapper mediaMapper, CollectionMapper collectionMapper, UserService userService)
        {
            _mediaService = mediaService;
            _collectionService = collectionService;

            _userContextService = new UserContextService(userService);
            _mediaMapper = mediaMapper;
            _collectionMapper = collectionMapper;
        }
        [BindProperty]
        public MDAMedia EditableMedia { get; set; }

       
        [BindProperty]


        public List<Media> ArtistMedia { get; set; } = new List<Media>();
        [BindProperty]

        public MDACollection EditableCollection { get; set; }
        [BindProperty]

        public List<(Collection, InterfaceCreator)> ArtistCollections { get; set; } = new List<(Collection, InterfaceCreator)>();

        [ViewData]
        public string? ActionLogger { get; set; }


        public bool ShowMediaModal { get; set; } = false;
        public bool ShowCollectionModal { get; set; } = false;



        public void OnGet()
        {
            try
            {
                int userId = _userContextService.GetCurrentUserId(User);

                UpdateMainPage(userId);
            }
            catch (Exception ex)
            {
                ActionLogger = ExceptionHandler.Handle(ex);
            }

        }
        #region Media
        public IActionResult OnPostSaveMedia()
        {
            int userId = _userContextService.GetCurrentUserId(User);

            try
            {
                ModelState.Clear();

                if (!TryValidateModel(EditableMedia))
                {
                    ShowMediaModal = true;
                    return Page();
                }

                var createInterfaceForMapping = new CreateInterfaceForMappingMedia(EditableMedia.MediaType);
                var strategy = createInterfaceForMapping.GetStrategy();
                Media media = _mediaMapper.MapMDAMediaToMedia(EditableMedia, strategy);

                if (EditableMedia.MediaId == 0)
                {
                    _mediaService.CreateMedia(media, userId);
                }
                else
                {
                    _mediaService.UpdateMedia(media);
                }

                ShowMediaModal = false;

                UpdateMainPage(userId);
                ReNull();
                ActionLogger = "Media successfully added to the system";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ActionLogger = ExceptionHandler.Handle(ex);
                ShowMediaModal = true;
                UpdateMainPage(userId);
                return Page();
            }

        }



        [BindProperty]
        public string DeleteItemId { get; set; }

        public IActionResult OnPostDelete()
        {
            int userId = _userContextService.GetCurrentUserId(User);

            try
            {
                if (DeleteItemId.StartsWith("COLLECTION_"))
                {
                    var idString = DeleteItemId.Replace("COLLECTION_", "");
                    if (int.TryParse(idString, out int collectionId))
                    {
                        _collectionService.DeleteCollection(collectionId);
                    }

                }
                else if (DeleteItemId.StartsWith("MEDIA_"))
                {
                    var idString = DeleteItemId.Replace("MEDIA_", "");
                    if (int.TryParse(idString, out int mediaId))
                    {
                        _mediaService.DeleteMedia(mediaId);
                    }

                }
                ActionLogger = "This Item is removed from the system";

                UpdateMainPage(userId);

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ActionLogger = ExceptionHandler.Handle(ex);
                UpdateMainPage(userId);
                return Page();
            }
        }

        public IActionResult OnPostEditMedia(int id)
        {
            int userId = _userContextService.GetCurrentUserId(User);

            try
            {
                var selectedMedia = _mediaService.ReadMedia(id);
                if (selectedMedia == null)
                {
                    ShowMediaModal = false;
                    return Page();
                }

                var createInterface = new CreateInterfaceForMappingMedia(selectedMedia.GetMediaType());

                var strategy = createInterface.GetStrategy();
                EditableMedia = _mediaMapper.MapMediaToMDAMedia(selectedMedia, strategy);

                ShowMediaModal = true;

                UpdateMainPage(userId);
                return Page();
            }
            catch (Exception ex)
            {
                ActionLogger = ExceptionHandler.Handle(ex);
                UpdateMainPage(userId);
                return Page();
            }
        }

        #endregion

        #region Collection
        public IActionResult OnPostEditCollection(int id)
        {
            int userId = _userContextService.GetCurrentUserId(User);

            try
            {
                var selectedCollection = _collectionService.ReadCollection(id);
                if (selectedCollection == null)
                {
                    ActionLogger = "Collection not found.";/// !!!!
                    ShowCollectionModal = false;
                    UpdateMainPage(userId);
                    return Page();
                }
                var (collection, creator) = selectedCollection.Value;

                var createInterface = new CreateInterfaceForMapping(CollectionType.ARTISTCOLLECTION);
                var strategy = createInterface.GetStrategy();
                EditableCollection = _collectionMapper.MapCollectionToMDACollection(collection, creator, strategy);
                ShowCollectionModal = true;
                UpdateMainPage(userId);
                return Page();
            }
            catch (Exception ex)
            {
                ActionLogger = ExceptionHandler.Handle(ex);
                UpdateMainPage(userId);
                return Page();
            }
        }



        public IActionResult OnPostSaveCollection()
        {
            int userId = _userContextService.GetCurrentUserId(User);

            try
            {
                ModelState.Clear();
                if (!TryValidateModel(EditableCollection, nameof(EditableCollection)))
                {
                    ShowCollectionModal = true;
                    UpdateMainPage(userId);
                    return Page();
                }

                var InterfaceCreator = new InterfaceCreator(EditableCollection.Type);
                var helper = InterfaceCreator.GetArtistCollectionHelper();
                var createInterfaceForMapping = new CreateInterfaceForMapping(CollectionType.ARTISTCOLLECTION);
                var strategy = createInterfaceForMapping.GetStrategy();

                Collection thisCollection = _collectionMapper.MapMDACollectionToCollection(EditableCollection, helper, strategy);

                if (EditableCollection.CollectionId == 0)
                {
                    _collectionService.CreateCollection(thisCollection, InterfaceCreator, userId);
                }
                else
                {
                    _collectionService.UpdateCollection(thisCollection, InterfaceCreator);
                }
                ShowCollectionModal = false;
                ActionLogger = "This Collection is successfully added to the system";
                ReNull();
                UpdateMainPage(userId);
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ActionLogger = ExceptionHandler.Handle(ex);
                ShowCollectionModal = true;
                UpdateMainPage(userId);
                return Page();
            }
        }



        private void UpdateMainPage(int userId)
        {
            ArtistMedia = _mediaService.ReadMediaPerUser();
            ArtistCollections = _collectionService.ReadCollectionsForUser(userId);


        }

        private void ReNull()
        {
            EditableMedia = new MDAMedia { MediaId = 0 };
            EditableCollection = new MDACollection { CollectionId = 0 };

        }

        public IActionResult OnPostResetForm()
        {
            ReNull();
            return RedirectToPage();
        }

        #endregion




    }

}
