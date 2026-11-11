using ML_WebApp.MDA;
using ML_WebApp.Services;
using SharedLibraryML.MediaSection;
using SharedLibraryML;

namespace ML_WebApp.Mappers.MediaM
{
    public class SongMapStrategy : IMediaMapperStrategy
    {
        public MDAMedia MapToMDA(Media media, IFile fileService)
        {
            var song = (Song)media;
            return new MDAMedia
            {
                MediaId = media.GetIdDB(),
                MediaTitle = media.GetTitle(),
                FileImage = fileService.LoadFileAsFormFile(media.GetImage()),
                FileAudio = fileService.LoadFileAsFormFile(media.GetFilePath()),
                Published = media.GetPublished(),
                MusicGenre = Enum.TryParse<MainMusicGenre>(media.GetGenre(), true, out var genre)
                    ? genre : MainMusicGenre.NONE,
                SubGenre = song.GetSubGenre(),
                Lyrics = song.GetLyrics(),
                MediaType = song.GetMediaType()
                //MediaType = MyMediaType.SONG
            };
        }

        public Media MapToMedia(MDAMedia mda, IFile fileService)
        {

            string imagePath = fileService.SaveImageFile(mda.FileImage);
            string audioPath = fileService.SaveAudioFile(mda.FileAudio);

            return new Song(
                mda.MediaId,
                mda.MediaTitle,
                imagePath,
                audioPath,
                mda.Published,
                mda.MusicGenre,
                mda.SubGenre ?? "",
                mda.Lyrics ?? ""
            );
        }

    }
}
