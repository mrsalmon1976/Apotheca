using Apotheca.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Apotheca.BLL.TestHelpers
{
    /// <summary>
    /// Contains static methods to create test entities.
    /// </summary>
    public class TestEntityHelper
    {
        public static DocumentEntity CreateDocument(Guid? id = null, int versionNo = 1, string name = null, string extension = null, string description = null, byte[] fileContents = null, DateTime? createdOn = null, Guid? createdByUserId = null)
        {
            DocumentEntity document = new DocumentEntity();
            document.Id = id;
            document.VersionNo = versionNo;
            document.FileName = name;
            document.Extension = extension;
            document.FileContents = fileContents;
            document.CreatedOn = createdOn;
            document.CreatedByUserId = createdByUserId ?? Guid.Empty;
            return document;
        }

        public static DocumentEntity CreateDocumentWithData()
        {
            byte[] fileContents = new byte[100];
            new Random().NextBytes(fileContents);

            DocumentEntity document = new DocumentEntity();
            document.Id = Guid.NewGuid();
            document.VersionNo = 1;
            document.FileName = "Test.txt";
            document.Extension = ".txt";
            document.Description = "This is a test document";
            document.FileContents = fileContents;
            document.CreatedOn = DateTime.UtcNow;
            document.CreatedByUserId = Guid.NewGuid();
            return document;
        }

        public static UserEntity CreateUser(Guid? id = null, string email = null, string firstName = null, string surname = null, string password = null, string role = null, DateTime? createdOn = null, string apiKey = null)
        {
            UserEntity user = new UserEntity();
            user.Id = id;
            user.Email = email;
            user.FirstName = firstName;
            user.Surname = surname;
            user.ApiKey = apiKey;
            user.Password = password;
            user.Role = role;
            user.CreatedOn = createdOn;
            return user;
        }

        public static UserEntity CreateUserWithData()
        {
            UserEntity user = new UserEntity();
            user.Id = Guid.NewGuid();
            user.Email = "test@test.com";
            user.FirstName = "Joe";
            user.Surname = "Soap";
            user.ApiKey = Guid.NewGuid().ToString();
            user.Password = "password1";
            user.Role = Roles.Moderator;
            user.CreatedOn = DateTime.UtcNow;
            return user;
        }

    }
}
