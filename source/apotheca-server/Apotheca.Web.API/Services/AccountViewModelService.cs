using Apotheca.BLL.Models;
using Apotheca.BLL.Repositories;
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
        Task<UserViewModel> LoadUserWithStores(User user);
    }

    public class AccountViewModelService : IAccountViewModelService
    {
        private readonly IStoreRepository _storeRepo;

        public AccountViewModelService(IStoreRepository storeRepo)
        {
            this._storeRepo = storeRepo;
        }

        public async Task<UserViewModel> LoadUserWithStores(User user)
        {
            // send back the user account
            UserViewModel userViewModel = Mapper.Map<User, UserViewModel>(user);

            // load the user stores
            IEnumerable<Store> stores = await _storeRepo.GetByIds(user.Stores);
            IEnumerable<StoreViewModel> storeViewModels = Mapper.Map<IEnumerable<Store>, IEnumerable<StoreViewModel>>(stores);
            userViewModel.Stores.AddRange(storeViewModels);
            return userViewModel;
        }
    }
}
