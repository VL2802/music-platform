using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedLibraryML.CollectionSection;
using SharedLibraryML.MediaSection;
using SharedLibraryML;
using ML_LogicLayer.Validators;
using ML_LogicLayer.Services;
using ML_Tests.MockRepository;

namespace ML_Tests
{
    [TestClass]
    public class CollectionTests
    {

        public CollectionTests()
        {
            
        }

       

        [TestMethod]
        public void AddWrongMediaTypeToAlbum_ThrowsException_Fail()
        {//Arrange
            var album = new ArtistCollection("Album", "", new HelperForAlbum());
        //Act
            var ex = Assert.ThrowsException<ArgumentException>(() =>
                album.AddMediaToCollection(new Chapter("Wrong Type", "image", "audio", DateTime.Now, BookGenre.HORROR)));
        //Assert
            Assert.AreEqual("This item is not song", ex.Message);
        }

        [TestMethod]
        public void AddWrongMediaTypeToAudiobook_ThrowsException_Fail()
        {//Arrange
            var audiobook = new ArtistCollection("Audiobook", "", new HelperForAudiobook());
         //Act
            var ex = Assert.ThrowsException<ArgumentException>(() =>
                audiobook.AddMediaToCollection(new Song("My Song", "image", "audio", DateTime.Now, MainMusicGenre.BLUES, "Pop", "lyrics")));
        //Assert
            Assert.AreEqual("This item is not Chapter", ex.Message);
        }

       

        [TestMethod]
        public void GetGenres_ThrowsIfListIsEmpty_Fail()
        {//Arrange
            var emptyCollection = new ArtistCollection("Empty", "", new HelperForAlbum());
            //Act
            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                emptyCollection.GetGenres());
            //Assert
            Assert.AreEqual("The media list is empty", ex.Message);
        }

        [TestMethod]
        public void GetFirstPublished_ThrowsIfEmpty_Fail()
        {//Arrange
            var emptyCollection = new ArtistCollection("Empty", "", new HelperForAlbum());
            //Act
            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                emptyCollection.GetFirstPublished());
            //Assert
            Assert.AreEqual("The media list is empty", ex.Message);
        }

        [TestMethod]
        public void AddDuplicateMedia_ThrowsException_Fail()
        {//Arrange
            var song = new Song("My Song","image", "audio", DateTime.Now, MainMusicGenre.BLUES, "Pop", "lyrics");
            var collection = new ArtistCollection("Duplicate Test", "", new HelperForAlbum());
            collection.AddMediaToCollection(song);
            //Act
            var ex = Assert.ThrowsException<ArgumentException>(() =>
                collection.AddMediaToCollection(song));
            //Assert
            StringAssert.Contains(ex.Message, "already contains");
        }

        [TestMethod]
        public void SetId_Twice_ThrowsException_Fail()
        {//Arrange
            var collection = new ArtistCollection("ID Test", "", new HelperForAlbum());
            collection.SetId(1);
            //Act
            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                collection.SetId(2));
            //Assert
            Assert.AreEqual("ID is already set.", ex.Message);
        }

        [TestMethod]
        public void AddMedia_ThrowsException_WhenMoreThan20SongsInAlbum_Fail()
        {
            // Arrange
            var album = new ArtistCollection("Big Album", "", new HelperForAlbum());

            // Act
            for (int i = 0; i < 21; i++)
            {
                album.AddMediaToCollection(new Song("Song", "image", "audio", DateTime.Now, MainMusicGenre.BLUES, "Pop", ""));
            }

            // Assert
            var ex = Assert.ThrowsException<ArgumentException>(() =>
                album.AddMediaToCollection(new Song("Extra Song", "image", "audio", DateTime.Now, MainMusicGenre.BLUES, "Pop", ""))
            );

            Assert.IsTrue(ex.Message.Contains("exceeded the allowed media limit."));
            Assert.IsTrue(album.GetTitle().Contains("Part 1"));
        }

        [TestMethod]
        public void AddMedia_DoesNotThrow_When20SongsInAlbum_Success()
        {
            // Arrange
            var album = new ArtistCollection("Good Album", "", new HelperForAlbum());

            // Act & Assert
            for (int i = 0; i < 20; i++)
            {
                album.AddMediaToCollection(new Song("Song", "image", "audio", DateTime.Now, MainMusicGenre.BLUES, "Pop", ""));
            }

            Assert.AreEqual(20, album.GetMedias().Count);
            Assert.IsFalse(album.GetTitle().Contains("Part 1")); // Should not have Part 1 if not exceeded
        }

        [TestMethod]
        public void AddMedia_ThrowsException_WhenMoreThan100ChaptersInAudiobook_Fail()
        {
            // Arrange
            var audiobook = new ArtistCollection("Epic Audiobook", "", new HelperForAudiobook());

            for (int i = 0; i < 101; i++)
            {
                audiobook.AddMediaToCollection(new Chapter("Chapter", "image", "audio", DateTime.Now, BookGenre.HORROR));
            }

            // Act & Assert
            var ex = Assert.ThrowsException<ArgumentException>(() =>
                audiobook.AddMediaToCollection(new Chapter("Extra Chapter", "image", "audio", DateTime.Now, BookGenre.HORROR))
            );

            Assert.IsTrue(ex.Message.Contains("exceeded the allowed media limit"));
            Assert.IsTrue(audiobook.GetTitle().Contains("Part 1"));
        }

        [TestMethod]
        public void AddMedia_DoesNotThrow_When100ChaptersInAudiobook_Success()
        {
            // Arrange
            var audiobook = new ArtistCollection("Normal Audiobook", "", new HelperForAudiobook());

            // Act
            for (int i = 0; i < 100; i++)
            {
                audiobook.AddMediaToCollection(new Chapter("Chapter", "image", "audio", DateTime.Now, BookGenre.HORROR));
            }

            // Assert
            Assert.AreEqual(100, audiobook.GetMedias().Count);
            Assert.IsFalse(audiobook.GetTitle().Contains("Part 1")); 
        }




        [TestMethod]
        public void ArtistCollection_Constructor_NullInterfaceCreator_ThrowsException_Fail()
        {
            // Arrange
            string title = "Invalid Collection";
            string description = "";
            HelperForAlbum helper = null; 

            // Act & Assert
            var ex = Assert.ThrowsException<ArgumentNullException>(() =>
                new ArtistCollection(title, description, helper) 
            );

            // Assert
            Assert.IsTrue(ex.Message.Contains("requires a valid InterfaceCreator."));
        }

       

    }
}
