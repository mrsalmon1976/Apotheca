using Apotheca.BLL.Models;
using Apotheca.BLL.Repositories;
using Apotheca.BLL.Services;
using Apotheca.Web.API.ViewModels.Common;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apotheca.Web.API.Services
{

    public interface IAccountViewModelService
    {
        UserViewModel LoadUserWithStores(User user);
    }

    public class AccountViewModelService : IAccountViewModelService
    {
        private readonly IStoreRepository _storeRepo;

        public AccountViewModelService(IStoreRepository storeRepo)
        {
            this._storeRepo = storeRepo;
        }

        public UserViewModel LoadUserWithStores(User user)
        {
            // send back the user account
            UserViewModel userViewModel = Mapper.Map<User, UserViewModel>(user);

            // load the user stores
            IEnumerable<Store> stores = Task.Run<IEnumerable<Store>>(() => _storeRepo.GetByIds(user.Stores)).Result;
            IEnumerable<StoreViewModel> storeViewModels = Mapper.Map<IEnumerable<Store>, IEnumerable<StoreViewModel>>(stores);
            userViewModel.Stores.AddRange(storeViewModels);
            return userViewModel;
        }
    }
}
