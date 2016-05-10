using Apotheca.BLL.Models;
using Apotheca.ViewModels.Document;
using Apotheca.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Apotheca.TestHelpers
{
    /// <summary>
    /// Contains static methods to create test view models.
    /// </summary>
    public class TestViewModelHelper
    {

        public static DocumentViewModel CreateDocumentViewModelWithData()
        {
            DocumentViewModel model = new DocumentViewModel();
            model.Description = "This is a test document view model.";
            model.FileName = "test.doc";
            model.UploadedFileName = "1000_test.doc";
            return model;
        }

        public static UserViewModel CreateUserViewModel(Guid? id = null, string email = null, string firstName = null, string surname = null, string password = null, string confirmPassword = null, string role = null, DateTime? createdOn = null, string apiKey = null)
        {
            UserViewModel model = new UserViewModel();
            model.Id = id;
            model.Email = email;
            model.FirstName = firstName;
            model.Surname = surname;
            model.ApiKey = apiKey;
            model.Password = password;
            model.ConfirmPassword = confirmPassword; 
            model.Role = role;
            model.CreatedOn = createdOn;
            return model;
        }

        public static UserViewModel CreateUserViewModelWithData()
        {
            UserViewModel model = new UserViewModel();
            model.Id = Guid.NewGuid();
            model.Email = "test@test.com";
            model.FirstName = "Joe";
            model.Surname = "Soap";
            model.ApiKey = Guid.NewGuid().ToString();
            model.Password = "password1";
            model.ConfirmPassword = model.Password;
            model.Role = Roles.Moderator;
            model.CreatedOn = DateTime.UtcNow;
            return model;
        }

    }
}
