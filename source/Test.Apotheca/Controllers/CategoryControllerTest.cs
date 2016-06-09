using Apotheca.BLL.Commands.Category;
using Apotheca.BLL.Data;
using Apotheca.BLL.Exceptions;
using Apotheca.BLL.Models;
using Apotheca.Controllers;
using Apotheca.ViewModels;
using Apotheca.Web.Results;
using NSubstitute;
using NUnit.Framework;
using System;
using Test.Apotheca.BLL.TestHelpers;

namespace Test.Apotheca.Controllers
{
    [TestFixture]
    public class CategoryControllerTest
    {
        private ICategoryController _categoryController;
        private IUnitOfWork _unitOfWork;
        private ISaveCategoryCommand _saveCategoryCommand;

        [SetUp]
        public void CategoryControllerTest_SetUp()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _saveCategoryCommand = Substitute.For<ISaveCategoryCommand>();
            _categoryController = new CategoryController(_unitOfWork, _saveCategoryCommand);

        }

        [Test]
        public void HandleCategoryPost_ValidationErrorOccurs_ReturnsFalse()
        {
            // setup
            ValidationException exception = new ValidationException(new string[] { "error1", "error2" });

            CategoryEntity category = TestEntityHelper.CreateCategoryWithData();
            _unitOfWork.When(x => x.BeginTransaction()).Do((ci) => { throw exception; });

            // execute
            JsonResult jsonResult = _categoryController.HandleCategoryPost(category) as JsonResult;

            // assert
            Assert.IsNotNull(jsonResult);
            BasicResult result = jsonResult.Model as BasicResult;
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(2, result.Messages.Length);
            Assert.AreEqual("error1", result.Messages[0]);
            Assert.AreEqual("error2", result.Messages[1]);
        }

        [Test]
        public void HandleCategoryPost_NonValidationErrorOccurs_ReturnsFalse()
        {
            // setup
            const string error = "Test";
            CategoryEntity category = TestEntityHelper.CreateCategoryWithData();
            _unitOfWork.When(x => x.BeginTransaction()).Do((ci) => { throw new Exception(error); });

            // execute
            JsonResult jsonResult = _categoryController.HandleCategoryPost(category) as JsonResult;

            // assert
            Assert.IsNotNull(jsonResult);
            BasicResult result = jsonResult.Model as BasicResult;
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(1, result.Messages.Length);
            Assert.AreEqual(error, result.Messages[0]);
        }

        [Test]
        public void HandleCategoryPost_SuccessfulSave_ReturnsTrue()
        {
            // setup
            CategoryEntity category = TestEntityHelper.CreateCategoryWithData();

            // execute
            JsonResult jsonResult = _categoryController.HandleCategoryPost(category) as JsonResult;

            // assert
            Assert.IsNotNull(jsonResult);
            BasicResult result = jsonResult.Model as BasicResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);

            _unitOfWork.Received(1).BeginTransaction();
            _saveCategoryCommand.Received(1).Category = category;
            _saveCategoryCommand.Received(1).Execute();
            _unitOfWork.Received(1).Commit();
        }


    }
}
