using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apotheca.BLL.Models
{
    public class StoreSubscriber
    {
        public StoreSubscriber()
        {

        }

        public StoreSubscriber(Guid userId, StoreRole role)
        {
            this.UserId = userId;
            this.Role = role;
        }

        public Guid UserId { get; set; }

        public StoreRole Role { get; set; }
    }
}
