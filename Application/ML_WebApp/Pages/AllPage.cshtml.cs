using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ML_LogicLayer;
using ML_LogicLayer.Services;
using SharedLibraryML;
using SharedLibraryML.CollectionSection;
using SharedLibraryML.MediaSection;
using System.Collections.Generic;
using System.Linq;

namespace ML_WebApp.Pages
{
    [Authorize(Policy = "ListenerOnly")]
    public class AllPageModel : PageModel
    {
        private readonly MediaService _mediaService;
        private readonly CollectionService _collectionService;

        public List<object> AllItems { get; set; } = new List<object>();
        public List<string> AvailableGenres { get; set; } = new List<string>();
        public List<Media> SelectedAudioItems { get; set; }
        public object CurrentSelectedItem { get; set; }

        [BindProperty]
        public List<(Collection, InterfaceCreator)> AllCollections { get; set; } = new List<(Collection, InterfaceCreator)>();

        [BindProperty]
        public List<Media> AllMedia { get; set; } = new List<Media>();

        [BindProperty]
        public List<string> SelectedGenres { get; set; } = new List<string>();

        [BindProperty]
        public bool ShowSongs { get; set; } = true;

        [BindProperty]
        public bool ShowAlbums { get; set; } = true;

        [BindProperty]
        public bool ShowAudiobooks { get; set; } = true;

        [BindProperty]
        public bool ShowPlaylists { get; set; } = true;

        [ViewData]
        public string? ActionLogger { get; set; }

        public AllPageModel(MediaService mediaService, CollectionService collectionService)
        {

            _mediaService = mediaService;
            _collectionService = collectionService;
        }

        public void OnGet()
        {
            try
            {
                LoadData();
                InitializeGenres();
                CombineItems();
            }
            catch (Exception ex)
            {
                ActionLogger = ExceptionHandler.Handle(ex);
            }
        }

        public IActionResult OnPostApplyFilter()
        {
            LoadData();
            InitializeGenres();
            CombineItems();
            ApplyFilters();
            return Page();
        }

        public IActionResult OnPostPlayMedia(int id)
        {
            LoadData();
            InitializeGenres();
            CombineItems();

            var media = AllMedia.FirstOrDefault(m => m.GetIdDB() == id);
            if (media != null)
            {
                CurrentSelectedItem = media;
                SelectedAudioItems = new List<Media> { media };
            }

            return Page();
        }

        public IActionResult OnPostPlayCollection(int id)
        {
            LoadData();
            InitializeGenres();
            CombineItems();

            var collection = AllCollections.FirstOrDefault(c => c.Item1.GetIdDB() == id);
            if (collection.Item1 != null)
            {
                CurrentSelectedItem = collection.Item1;
                SelectedAudioItems = new List<Media>(collection.Item1.GetMedias());
            }

            return Page();
        }

        private void LoadData()
        {
            AllCollections = _collectionService.ReadAllCollections();
            AllMedia = _mediaService.ReadAllMedia().Select(m => m.Item1).ToList();
        }

        private void CombineItems()
        {
            AllItems = new List<object>();
            AllItems.AddRange(AllMedia);
            AllItems.AddRange(AllCollections.Select(c => c.Item1));
        }

        private void ApplyFilters()
        {
            var filteredItems = new List<object>();

            if (ShowSongs)
            {
                filteredItems.AddRange(AllMedia.Where(m => m is Song));
            }

            if (ShowAlbums)
            {
                filteredItems.AddRange(AllCollections
                    .Where(c => c.Item1 is ArtistCollection)
                    .Select(c => c.Item1));
            }

            if (ShowAudiobooks)
            {
                filteredItems.AddRange(AllMedia.Where(m => m is Chapter));
            }

            if (ShowPlaylists)
            {
                filteredItems.AddRange(AllCollections
                    .Where(c => c.Item1 is Playlist)
                    .Select(c => c.Item1));
            }

            if (SelectedGenres != null && SelectedGenres.Any())
            {
                filteredItems = filteredItems.Where(item =>
                {
                    if (item is Media media)
                    {
                        return SelectedGenres.Contains(media.GetGenre());
                    }
                    else if (item is Collection collection)
                    {
                        var collectionGenres = collection.GetGenres();
                        return collectionGenres.Any(g => SelectedGenres.Contains(g.Trim()));
                    }
                    return false;
                }).ToList();
            }

            AllItems = filteredItems;
        }

        private void InitializeGenres()
        {
            AvailableGenres =
            [
                .. Enum.GetNames(typeof(MainMusicGenre)),
                .. Enum.GetNames(typeof(BookGenre)),
            ];
        }
    }
}