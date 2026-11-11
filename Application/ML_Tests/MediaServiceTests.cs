using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ML_LogicLayer.Services;
using SharedLibraryML.Interfaces;
using SharedLibraryML.MediaSection;
using System;
using System.Collections.Generic;
using System.Linq;
using ML_Tests.MockRepository;
using ML_LogicLayer.Validators;

namespace ML_Tests
{
    [TestClass]
    public class MediaServiceTests
    {
        private MockMediaRepository _mockMediaRepo;
        private Mock<IMediaDeletionValidator> _mockValidator;
        private MediaService _mediaService;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockMediaRepo = new MockMediaRepository();
            _mockValidator = new Mock<IMediaDeletionValidator>();
            _mediaService = new MediaService(_mockMediaRepo, _mockValidator.Object);
        }

        [TestMethod]
        public void DeleteMedia_WhenValidationPasses_ShouldCallRepositoryDelete()
        {
            // Arrange
            int mediaId = 5;
            var emptyCollections = new List<(int Id, string Title)>();

            _mockValidator.Setup(v => v.CanDeleteMedia(mediaId, out emptyCollections))
                         .Returns(true);

            // Act
            _mediaService.DeleteMedia(mediaId);

            // Assert
            var mediaExists = _mockMediaRepo.ReadMedia(mediaId);
            Assert.IsNull(mediaExists); 
        }

        [TestMethod]
        public void DeleteMedia_WhenValidationFails_ShouldThrowWithCollectionNames()
        {
            // Arrange
            int mediaId = 5;
            var blockingCollections = new List<(int Id, string Title)>
            {
                (1, "Album 1"),
                (2, "Playlist 1")
            };

            _mockValidator.Setup(v => v.CanDeleteMedia(mediaId, out blockingCollections))
                         .Returns(false);

            // Act & Assert
            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                _mediaService.DeleteMedia(mediaId));

            Assert.IsTrue(ex.Message.Contains("Album 1"));
            Assert.IsTrue(ex.Message.Contains("Playlist 1"));

        
            var mediaExists = _mockMediaRepo.ReadMedia(mediaId);
            Assert.IsNotNull(mediaExists);
        }

        [TestMethod]
        public void DeleteMedia_WhenNoBlockingCollections_ShouldSucceed()
        {
            // Arrange
            int mediaId = 5;
            var emptyCollections = new List<(int Id, string Title)>();

            _mockValidator.Setup(v => v.CanDeleteMedia(mediaId, out emptyCollections))
                         .Returns(true);

            // Act
            _mediaService.DeleteMedia(mediaId);

            // Assert
            var mediaExists = _mockMediaRepo.ReadMedia(mediaId);
            Assert.IsNull(mediaExists);
        }

        [TestMethod]
        public void DeleteMedia_WhenSingleBlockingCollection_ShouldThrowWithCorrectMessage()
        {
            // Arrange
            int mediaId = 5;
            var singleCollection = new List<(int Id, string Title)>
            {
                (1, "My Playlist")
            };

            _mockValidator.Setup(v => v.CanDeleteMedia(mediaId, out singleCollection))
                         .Returns(false);

            // Act & Assert
            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                _mediaService.DeleteMedia(mediaId));

            Assert.AreEqual(
                "Cannot delete media. It's the last item in these collections: My Playlist. Delete these collections first.",
                ex.Message);
        }

        [TestMethod]
        public void DeleteMedia_WhenMultipleBlockingCollections_ShouldIncludeAllInMessage()
        {
            // Arrange
            int mediaId = 5;
            var collections = new List<(int Id, string Title)>
            {
                (1, "Album 1"),
                (2, "Favorites"),
                (3, "Best Of")
            };

            _mockValidator.Setup(v => v.CanDeleteMedia(mediaId, out collections))
                         .Returns(false);

            // Act & Assert
            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                _mediaService.DeleteMedia(mediaId));

            Assert.IsTrue(ex.Message.Contains("Album 1"));
            Assert.IsTrue(ex.Message.Contains("Favorites"));
            Assert.IsTrue(ex.Message.Contains("Best Of"));
        }

        [TestMethod]
        public void DeleteMedia_WhenMediaDoesNotExist_ShouldStillCallValidator()
        {
            // Arrange
            int nonExistentMediaId = 404;
            var emptyCollections = new List<(int Id, string Title)>();

            _mockValidator.Setup(v => v.CanDeleteMedia(nonExistentMediaId, out emptyCollections))
                         .Returns(true);

            // Act
            _mediaService.DeleteMedia(nonExistentMediaId);

            // Assert
            _mockValidator.Verify(v => v.CanDeleteMedia(nonExistentMediaId, out emptyCollections), Times.Once);
        }
    }
}