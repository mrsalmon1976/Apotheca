using Apotheca.BLL.Commands.Category;
using Apotheca.BLL.Data;
using Apotheca.BLL.Models;
using Apotheca.BLL.Repositories;
using Apotheca.BLL.Validators;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Data;
using Test.Apotheca.BLL.TestHelpers;

namespace Test.Apotheca.BLL.Commands.Category
{
    [TestFixture]
    public class SaveCategoryCommandTest
    {
        private ISaveCategoryCommand _command;

        private ICategoryRepository _categoryRepo;
        private IDbTransaction _dbTransaction;
        private IUnitOfWork _unitOfWork;

        private ICategoryValidator _categoryValidator;

        [SetUp]
        public void CreateDocumentCommandTest_SetUp()
        {
            _categoryRepo = Substitute.For<ICategoryRepository>();
            _dbTransaction = Substitute.For<IDbTransaction>();
            _categoryValidator = Substitute.For<ICategoryValidator>();

            _unitOfWork = Substitute.For<IUnitOfWork>();
            _unitOfWork.CategoryRepo.Returns(_categoryRepo);
            _unitOfWork.CurrentTransaction.Returns(_dbTransaction);

            _command = new SaveCategoryCommand(_unitOfWork, _categoryValidator);
        }

        [Test]
        public void Execute_NoCategory_ThrowsException()
        {
            _command.Category = null;
            Assert.Throws(typeof(NullReferenceException), () => _command.Execute());  
        }

        [Test]
        public void Execute_NoTransaction_ThrowsException()
        {
            IDbTransaction transaction = null;
            _unitOfWork.CurrentTransaction.Returns(transaction);
            _command.Category = TestEntityHelper.CreateCategory();
            Assert.Throws(typeof(InvalidOperationException), () => _command.Execute());
        }


        [Test]
        public void Execute_WithCategory_Validate()
        {
            CategoryEntity cat = TestEntityHelper.CreateCategory();
            _categoryRepo.When(x => x.Create(cat)).Do((c) => { cat.Id = Guid.NewGuid(); });

            _command.Category = cat;
            _command.Execute();

            _categoryValidator.Received(1).Validate(cat);
        }

        [Test]
        public void Execute_WithNewCategory_SetsCreatedOn()
        {
            CategoryEntity cat = TestEntityHelper.CreateCategory();
            cat.CreatedOn = null;
            _categoryRepo.When(x => x.Create(cat)).Do((c) => { cat.Id = Guid.NewGuid(); });

            DateTime dtBefore = DateTime.UtcNow;
            _command.Category = cat;
            _command.Execute();
            DateTime dtAfter = DateTime.UtcNow;

            Assert.IsNotNull(cat.CreatedOn);
            Assert.LessOrEqual(dtBefore, cat.CreatedOn);
            Assert.GreaterOrEqual(dtAfter, cat.CreatedOn);
        }

        [Test]
        public void Execute_NewCategory_CreatesAndReturnsId()
        {
            Guid id = Guid.NewGuid();
            CategoryEntity cat = TestEntityHelper.CreateCategory();
            _categoryRepo.When(x => x.Create(cat)).Do((c) => { cat.Id = id; });

            _command.Category = cat;
            Guid result = _command.Execute();

            _categoryRepo.Received(1).Create(cat);
            Assert.AreEqual(id, result);
        }

        [Test]
        public void Execute_ExistingCategory_Updates()
        {
            CategoryEntity doc = TestEntityHelper.CreateCategoryWithData();

            _command.Category = doc;
            Guid result = _command.Execute();

            _categoryRepo.Received(1).Update(doc);
            Assert.AreEqual(doc.Id, result);
        }


    }
}
