using Apotheca.BLL.Caching;
using Apotheca.BLL.Repositories;
using Apotheca.Security;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Apotheca.Caching
{
    /// <summary>
    /// Overrides the default UserMapper for caching.
    /// </summary>
    public class CacheUserMapper : UserMapper
    {
        private ICacheProvider _cacheProvider;

        public CacheUserMapper(ICacheProvider cacheProvider, IUserRepository userRepo) : base(userRepo)
        {
            _cacheProvider = cacheProvider;
        }

        public override IUserIdentity GetUserFromIdentifier(Guid identifier, Nancy.NancyContext context)
        {
            var cacheKey = string.Format("User_{0}", identifier);
            return _cacheProvider.GetCacheable<IUserIdentity>(cacheKey, () => base.GetUserFromIdentifier(identifier, context), null, TimeSpan.FromHours(1));
        }
    }
}

