using System;
using System.Collections.Generic;
using System.Text;

namespace Apotheca.BLL.Models
{
    public class Store
    {

        public Store()
        {
            this.Id = Guid.NewGuid();
            this.Created = DateTime.UtcNow;
            this.Subscribers = new List<StoreSubscriber>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; }

        public List<StoreSubscriber> Subscribers { get; set; }
    }
}
