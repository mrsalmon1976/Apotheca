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

        public static CategoryEntity CreateCategory(Guid? id = null, string name = null, string description = null, DateTime? createdOn = null)
        {
            CategoryEntity category = new CategoryEntity();
            category.Id = (id ?? Guid.Empty);
            category.Name = name;
            category.Description = description;
            category.CreatedOn = createdOn;
            return category;
        }

        public static CategoryEntity CreateCategoryWithData()
        {
            CategoryEntity category = new CategoryEntity();
            category.Id = Guid.NewGuid();
            category.Name = Guid.NewGuid().ToString();
            category.Description = Guid.NewGuid().ToString();
            category.CreatedOn = DateTime.UtcNow;
            return category;
        }

        public static DocumentEntity CreateDocument(Guid? id = null, int versionNo = 1, string name = null, string extension = null, string description = null, byte[] fileContents = null, DateTime? createdOn = null, Guid? createdByUserId = null)
        {
            DocumentEntity document = new DocumentEntity();
            document.Id = (id ?? Guid.Empty);
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
            document.MimeType = "text/plain";
            return document;
        }

        public static DocumentCategoryAsscEntity CreateDocumentCategoryAssc(int id = 0, Guid? documentId = null, int? documentVersionNo = null, Guid? categoryId = null)
        {
            DocumentCategoryAsscEntity docCatAssc = new DocumentCategoryAsscEntity();
            docCatAssc.Id = id;
            docCatAssc.DocumentId = (documentId ?? Guid.NewGuid());
            docCatAssc.DocumentVersionNo = (documentVersionNo ?? 1);
            docCatAssc.CategoryId = (categoryId ?? Guid.NewGuid());
            return docCatAssc;
        }

        public static UserEntity CreateUser(Guid? id = null, string email = null, string firstName = null, string surname = null, string password = null, string role = null, DateTime? createdOn = null, string apiKey = null)
        {
            UserEntity user = new UserEntity();
            user.Id = (id ?? Guid.Empty);
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
            user.Salt = Guid.NewGuid().ToString();
            user.Password = "password1";
            user.Role = Roles.Moderator;
            user.CreatedOn = DateTime.UtcNow;
            return user;
        }

    }
}
