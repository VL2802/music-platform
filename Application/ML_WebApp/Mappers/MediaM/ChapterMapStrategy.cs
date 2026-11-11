using ML_WebApp.MDA;
using ML_WebApp.Services;
using SharedLibraryML.MediaSection;
using SharedLibraryML;

namespace ML_WebApp.Mappers.MediaM
{
    public class ChapterMapStrategy : IMediaMapperStrategy
    {
        public MDAMedia MapToMDA(Media media, IFile fileService)
        {
            var chapter = (Chapter)media;
            return new MDAMedia
            {
                MediaId = chapter.GetIdDB(),
                MediaTitle = chapter.GetTitle(),
                FileImage = fileService.LoadFileAsFormFile(chapter.GetImage()),
                FileAudio = fileService.LoadFileAsFormFile(chapter.GetFilePath()),
                Published = chapter.GetPublished(),
                BookGenre = Enum.TryParse<BookGenre>(chapter.GetGenre(), true, out var genre) ? genre : BookGenre.NONE,
                MediaType = chapter.GetMediaType()
                // MediaType = MyMediaType.CHAPTER
            };
        }

        public Media MapToMedia(MDAMedia mda, IFile fileService)
        {

            string imagePath = fileService.SaveImageFile(mda.FileImage);
            string audioPath = fileService.SaveAudioFile(mda.FileAudio);

            return new Chapter(
                mda.MediaId,
                mda.MediaTitle,
                imagePath,
                audioPath,
                mda.Published,
                mda.BookGenre
            );
        }
    }
}
