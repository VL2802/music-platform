using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ML_LogicLayer;
using SharedLibraryML.CollectionSection;
using SharedLibraryML.MediaSection;
using ML_LogicLayer.Services;
using ML_WebApp.MDA;
using Microsoft.AspNetCore.Authorization;
using ML_WebApp.Mappers.CollectionM;
using ML_WebApp.Services;
using Microsoft.AspNetCore.Http;
using ML_WebApp.Mappers;
using SharedLibraryML;

namespace ML_WebApp.Pages
{
    [Authorize(Policy = "ListenerOnly")]

    public class ListenerPageModel : PageModel
    {
        private readonly MediaService _mediaService;
        private readonly UserContextService _userContextService;
        private readonly CollectionService _collectionService;
        private readonly CollectionMapper _collectionMapper;

        public ListenerPageModel(CollectionMapper collectionMapper,  MediaService mediaService, CollectionService collectionService, UserService userService)
        {
            _mediaService = mediaService;
            _collectionService = collectionService;

            _userContextService = new UserContextService(userService);
            _collectionMapper = collectionMapper;
        }
        public bool ShowPlaylistModal { get; set; } = false;
        [BindProperty]
        public MDACollection EditablePlaylist { get; set; }
        [BindProperty]
        public List<(Collection, InterfaceCreator)> ListenerPlaylists { get; set; } = new List<(Collection, InterfaceCreator)>();
        [BindProperty]
        public List<(Media, int)> AllMedia { get; set; } = new List<(Media, int)>();
        [TempData]
        public string? ActionLogger { get; set; }
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

       

        private void UpdateMainPage(int userId)
        {
            ListenerPlaylists = _collectionService.ReadCollectionsForUser(userId);
            AllMedia = _mediaService.ReadAllMedia();
            EditablePlaylist = new MDACollection { CollectionId = 0 };
        }

        public IActionResult OnPostSavePlaylist()
        {
           int userId =  _userContextService.GetCurrentUserId(User);

            try
            {
                ModelState.Clear();
                if (!TryValidateModel(EditablePlaylist, nameof(EditablePlaylist)))
                {
                    ShowPlaylistModal = true;
                    return Page();
                }
                var createInterface = new CreateInterfaceForMapping(CollectionType.PLAYLIST);
                var strategy = createInterface.GetStrategy();
                
                Collection thisPlaylist = _collectionMapper.MapMDACollectionToCollection(EditablePlaylist, null, strategy);
                if (EditablePlaylist.CollectionId == 0)
                {

                    _collectionService.CreateCollection(thisPlaylist, null, userId);

                }
                else
                {
                    _collectionService.UpdateCollection(thisPlaylist, null);
                }

                UpdateMainPage(userId);
                return Page();
            }
            catch (Exception ex)
            {
                ActionLogger = ExceptionHandler.Handle(ex);
                ShowPlaylistModal = true;
                UpdateMainPage(userId);
                return Page();
            }

            
        }


        public IActionResult OnPostEditCollection(int id)
        {
            int userId = _userContextService.GetCurrentUserId(User);

            try
            {
                var selectedCollection = _collectionService.ReadCollection(id);

               
               var (collection, creator) = selectedCollection.Value;

                var createInterface = new CreateInterfaceForMapping(CollectionType.PLAYLIST);
                var strategy = createInterface.GetStrategy();
                EditablePlaylist = _collectionMapper.MapCollectionToMDACollection(collection, creator, strategy);
                ShowPlaylistModal = true;
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

       
        [BindProperty]
        private int DeleteItemID {  get; set; }
        public IActionResult OnPostDelete()
        {
            int userId = _userContextService.GetCurrentUserId(User);

            try
            {
                _collectionService.DeleteCollection(DeleteItemID);

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

       
        public Playlist? SelectedPlaylist { get; set; }
        public List<Media>? SelectedMediaItems { get; set; }

        public IActionResult OnPostViewPlaylist(int id)
        {
            int userId = _userContextService.GetCurrentUserId(User);

            try
            {
                ListenerPlaylists = _collectionService.ReadCollectionsForUser(userId);

                var playlistTuple = ListenerPlaylists.FirstOrDefault(p => p.Item1.GetIdDB() == id);
                if (playlistTuple.Item1 != null)
                {
                    SelectedPlaylist = (Playlist)playlistTuple.Item1;
                    SelectedMediaItems = SelectedPlaylist.GetMedias();
                }

                return Page();
            }
            catch (Exception ex)
            {
                ActionLogger = ExceptionHandler.Handle(ex);
                UpdateMainPage(userId);
                return Page();
            }
        }

    }
}
