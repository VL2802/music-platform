using ML_LogicLayer.Services;
using ML_Tests.MockRepository;
using SharedLibraryML.CollectionSection;
using SharedLibraryML.MediaSection;
using SharedLibraryML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibraryML.Interfaces;

namespace ML_Tests
{
    [TestClass]
    public class CollectionServiceTests
    {
        private CollectionService _collectionService;
        private MockCollectionRepository _mockRepo;

        public CollectionServiceTests()
        {
            _mockRepo = new MockCollectionRepository();
            _collectionService = new CollectionService(_mockRepo);
        }

        [TestMethod]
        public void CreateCollection_ValidAlbum_StoresInRepository_Success()
        {
            // Arrange
            var interfaceCreator = new InterfaceCreator(ArtistCollectionType.ALBUM);
            var helper = interfaceCreator.GetArtistCollectionHelper();
            var artistCollection = new ArtistCollection("New Album", "", helper);
            artistCollection.AddMediaToCollection(new Song("Song 1", "img", "audio", DateTime.Now, MainMusicGenre.ROCK, "Sub", "lyrics"));

            // Act
            _collectionService.CreateCollection(artistCollection, interfaceCreator, userId: 1);

            // Assert
            var allCollections = _mockRepo.ReadAllCollections();
            var savedCollection = allCollections.Last();

            Assert.AreEqual("New Album", savedCollection.Item1.GetTitle());
            Assert.AreEqual(ArtistCollectionType.ALBUM, savedCollection.Item2.GetArtistCollectionType());
        }

        [TestMethod]
        public void GetMedias_ThrowsIfListIsEmpty_Fail()
        {//Arrange
            var emptyCollection = new ArtistCollection("Empty", "", new HelperForAlbum());
            //Act
            var ex = Assert.ThrowsException<ArgumentException>(() =>
                _collectionService.CreateCollection(emptyCollection, null, 0));
            //Assert
            Assert.AreEqual("The collection should have at least one media", ex.Message);
        }


     




        [TestMethod]
        public void UpdateCollection_ShouldCorrectlyModifyAndSaveChanges()
        {
            // Arrange 


            var initialCollection = _mockRepo.ReadCollection(1).Value;

            var updatedCollection = new ArtistCollection(
                id: 1,
                title: "Updated Title",
                image: "new_image.jpg",
                helper: new HelperForAudiobook());

            updatedCollection.AddMediaToCollection(new Chapter("Chapter 1", "img", "audio", DateTime.Now, BookGenre.HORROR));



            // Act
            _collectionService.UpdateCollection(updatedCollection, new InterfaceCreator(ArtistCollectionType.AUDIOBOOK));

            // Assert
            var result = _mockRepo.ReadCollection(1);

            Assert.AreEqual("Updated Title", result.Value.Item1.GetTitle());
            Assert.AreEqual("new_image.jpg", result.Value.Item1.GetImage());
            Assert.AreEqual(1, result.Value.Item1.GetIdDB());

            Assert.AreEqual(ArtistCollectionType.AUDIOBOOK, result.Value.Item2.GetArtistCollectionType());

            var media = result.Value.Item1.GetMedias();
            Assert.AreEqual(1, media.Count);
            Assert.AreEqual("Chapter 1", media[0].GetTitle());
        }

          
        [TestMethod]
        public void ReadAllCollections_ShouldExcludePrivatePlaylists()
        {
            // Arrange
            _mockRepo.CreateCollection(
                new ArtistCollection("Public Collection", "", new HelperForAudiobook()),
                new InterfaceCreator(ArtistCollectionType.AUDIOBOOK), 1);

            _mockRepo.CreateCollection(
                new Playlist("Public Playlist", "","", false),
                null, 1);

            _mockRepo.CreateCollection(
                new Playlist("Private Playlist", "","", true), // should be excluded
                null, 1);

            _mockRepo.CreateCollection(
                new ArtistCollection("Another Public Collection", "", new HelperForAudiobook()),
                new InterfaceCreator(ArtistCollectionType.AUDIOBOOK), 1);

            // Act
            var result = _collectionService.ReadAllCollections();

            // Assert
            Assert.AreEqual(4, result.Count); 

            var privatePlaylistExists = result.Any(x => x.Item1.GetIdDB() == 3 && x.Item1 is Playlist p && p.IsPrivate());
            Assert.IsFalse(privatePlaylistExists);

            Assert.IsTrue(result.Any(x => x.Item1.GetIdDB() == 1));
            Assert.IsTrue(result.Any(x => x.Item1.GetIdDB() == 2));
            Assert.IsTrue(result.Any(x => x.Item1.GetIdDB() == 4));
        }

        [TestMethod]
        public void ReadAllCollections_ShouldReturnAll_WhenNoPrivatePlaylistsExist()
        {
            // Arrange
            _mockRepo.CreateCollection(
                new ArtistCollection("Public Collection", "", new HelperForAudiobook()),
                new InterfaceCreator(ArtistCollectionType.AUDIOBOOK), 1);

            _mockRepo.CreateCollection(
                new Playlist("Public Playlist", "","", false),
                null, 1);

            _mockRepo.CreateCollection(
                new ArtistCollection("Another Public Collection", "", new HelperForAudiobook()),
                new InterfaceCreator(ArtistCollectionType.AUDIOBOOK), 1);

            // Act
            var result = _collectionService.ReadAllCollections();

            // Assert
            Assert.AreEqual(4, result.Count);
        }

        [TestMethod]
        public void ReadAllCollections_ShouldReturnEmpty_WhenAllPlaylistsArePrivate()
        {
            // Arrange
            _mockRepo.CreateCollection(
                new Playlist("Private Playlist 1", "", "", true),
                null, 1);

            _mockRepo.CreateCollection(
                new Playlist("Private Playlist 2", "", "", true),
                null, 1);

            // Act
            var result = _collectionService.ReadAllCollections();

            // Assert
            Assert.AreEqual(1, result.Count);
        }

       
    }
} 


