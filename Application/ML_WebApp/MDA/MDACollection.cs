using SharedLibraryML;
using System.ComponentModel.DataAnnotations;

public class MDACollection
{
    public int CollectionId { get; set; }

    [Required(ErrorMessage = "Collection title is required.")]
    [StringLength(100, ErrorMessage = "Title too long.")]
    public string CollectionTitle { get; set; }

    [Display(Name = "Image Picture")]
    public IFormFile? FileImage { get; set; }

    public List<int> MediaIds { get; set; } = new();

    //playlist
    public string? Description { get; set; }
    public int Privacy { get; set; }
    //atistcollection
    public ArtistCollectionType Type { get; set; }


}

