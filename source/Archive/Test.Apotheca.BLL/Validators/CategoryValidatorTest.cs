using Apotheca.BLL.Validators;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using Apotheca.BLL.Models;
using Test.Apotheca.BLL.TestHelpers;
using Apotheca.BLL.Exceptions;
using Apotheca.BLL.Repositories;
using Apotheca.BLL.Data;

namespace Test.Apotheca.BLL.Validators
{
    [TestFixture]
    public class CategoryValidatorTest
    {
        private ICategoryValidator _categoryValidator;
        private IUnitOfWork _unitOfWork;
        private ICategoryRepository _categoryRepo;
 
        [SetUp]
        public void CategoryValidatorTest_SetUp()
        {
            _categoryRepo = Substitute.For<ICategoryRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _unitOfWork.CategoryRepo.Returns(_categoryRepo);

            _categoryValidator = new CategoryValidator(_unitOfWork);
        }

        [Test]
        public void Validate_CategoryNull_ThrowsArgumentNullException()
        {
            Assert.Throws(typeof(ArgumentNullException), () => _categoryValidator.Validate(null));
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("      ")]
        public void Validate_InvalidName_FailsValidation(string name)
        {
            CategoryEntity cat = TestEntityHelper.CreateCategoryWithData();
            cat.Name = name;
            AssertValidationFailed(cat, "Name");
        }

        [Test]
        public void Validate_IsValidObject_PassesValidation()
        {
            CategoryEntity cat = TestEntityHelper.CreateCategoryWithData();

            CategoryEntity dbCat = null;
            _categoryRepo.GetByNameOrDefault(cat.Name).Returns(dbCat);
            _categoryValidator.Validate(cat);
        }

        [Test]
        public void Validate_CategoryWithSameNameExists_FailsValidation()
        {
            CategoryEntity cat = TestEntityHelper.CreateCategoryWithData();
            CategoryEntity dbCat = TestEntityHelper.CreateCategoryWithData(); ;
            dbCat.Name = cat.Name;

            _categoryRepo.GetByNameOrDefault(cat.Name).Returns(dbCat);

            try
            {
                _categoryValidator.Validate(cat);
                Assert.Fail("Validation did not fail as expected");
            }
            catch (ValidationException ex)
            {
                Assert.AreEqual("Category already exists", ex.Errors.Single());
            }
        }

        [Test]
        public void Validate_CategoryAlreadyExistsAndValidates_PassesValidation()
        {
            CategoryEntity cat = TestEntityHelper.CreateCategoryWithData();
            CategoryEntity dbCat = TestEntityHelper.CreateCategoryWithData(); ;
            dbCat.Id = cat.Id;

            _categoryRepo.GetByNameOrDefault(cat.Name).Returns(dbCat);

            _categoryValidator.Validate(cat);
        }

        /// <summary>
        /// Helper method to check validation failed and the errors collections contains an expected message.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="validationMsg"></param>
        private void AssertValidationFailed(CategoryEntity category, string validationMsg)
        {
            try
            {
                _categoryValidator.Validate(category);
                Assert.Fail("Validation did not fail as expected");
            }
            catch (ValidationException ex)
            {
                string error = ex.Errors.FirstOrDefault(x => x.Contains(validationMsg));
                Assert.IsNotNull(error, "Unable to find expected validation error in Errors collection");
            }
        }


    }
}
