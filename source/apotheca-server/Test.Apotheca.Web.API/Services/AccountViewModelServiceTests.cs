using Apotheca.BLL.Models;
using Apotheca.BLL.Repositories;
using Apotheca.Web.API;
using Apotheca.Web.API.Services;
using Apotheca.Web.API.ViewModels.Common;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Test.Apotheca.Web.API.Services
{
    [TestFixture]
    public class AccountViewModelServiceTests
    {
        private IAccountViewModelService _accountViewModelService;

        private IStoreRepository _storeRepo;

        [SetUp]
        public void AccountViewModelServiceTests_SetUp()
        {

            _storeRepo = Substitute.For<IStoreRepository>();

            _accountViewModelService = new AccountViewModelService(_storeRepo);
        }

        [Test]
        public void LoadUserWithStores_OnExecute_LoadsUserViewModelWithStores()
        {
            AppMap.Reset();
            AppMap.Configure();

            Store store1 = CreateStore();
            Store store2 = CreateStore();
            User user = new User()
            {
                Email = "test@test.com",
                FirstName = "Bob",
                LastName = "von Schtinkle",
            };
            user.Stores.Add(store1.Id);
            user.Stores.Add(store2.Id);

            _storeRepo.GetByIds(Arg.Any<IEnumerable<Guid>>()).Returns(Task.FromResult<IEnumerable<Store>>(new Store[] { store1, store2 }));

            // execute
            UserViewModel userViewModel = _accountViewModelService.LoadUserWithStores(user);

            // assert
            _storeRepo.Received(1).GetByIds(Arg.Any<IEnumerable<Guid>>());
            Assert.AreEqual(user.Id, userViewModel.Id);
            Assert.AreEqual(user.Email, userViewModel.Email);
            Assert.AreEqual(user.FirstName, userViewModel.FirstName);
            Assert.AreEqual(user.LastName, userViewModel.LastName);
            Assert.AreEqual(user.Stores.Count, userViewModel.Stores.Count);
            Assert.AreEqual(user.Stores[0], userViewModel.Stores[0].Id);
            Assert.AreEqual(user.Stores[1], userViewModel.Stores[1].Id);
        }

        private Store CreateStore()
        {
            Store store = new Store()
            {
                Name = Guid.NewGuid().ToString()
            };
            return store;
        }


    }
}
