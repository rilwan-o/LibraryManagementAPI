using LibraryManagement.Domain.Interfaces.BookManager;
using LibraryManagement.Domain.Models;
using LibraryManagement.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace LibraryManagement.Test
{
    public class BookManagementServiceTest
    {       
        [Fact]
        public void AddAuthor_AddNewAuthor_ReturnsTrue()
        {

            IBookManagementService bookManagementService = new BookManagementService();
            var initialCount = bookManagementService.GetAuthors().Count;
            var firstName = "TesterFirstname1";
            var lastName = "TesterLastName2";
            var actual = bookManagementService.AddAuthor(firstName, lastName);
            var finalCount = bookManagementService.GetAuthors().Count;
            Assert.Equal(initialCount + 1, finalCount);
        }

        [Fact]
        public void AddAuthor_AddExistingAuthor_ThrowsException()
        {

            IBookManagementService bookManagementService = new BookManagementService();
            var initialCount = bookManagementService.GetAuthors().Count;
            var firstName = "John";
            var lastName = "Doe";
            Assert.Throws<Exception>(() => bookManagementService.AddAuthor(firstName, lastName));
            
        }
      
        [Fact]
        public void AddPublisher_AddNewPublisher_ReturnsTrue()
        {
            IBookManagementService bookManagementService = new BookManagementService();
            var initialCount = bookManagementService.GetPublishers().Count;
            var name = "TesterFirstname1";
            var actual = bookManagementService.AddPublisher(name);
            var finalCount = bookManagementService.GetPublishers().Count;
            Assert.Equal(initialCount + 1, finalCount);
        }


        [Fact]
        public void AddPublisher_AddExistingPublisher_ThrowsException()
        {

            IBookManagementService bookManagementService = new BookManagementService();
            var initialCount = bookManagementService.GetPublishers().Count;
            var name = "Wiley";
            Assert.Throws<Exception>(() => bookManagementService.AddPublisher(name));

        }

       

    }
}
