using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public abstract class EntityBase<TKey> : IEntityBase<TKey>,ICreatedByEntity
    {
        public TKey Id { get; set; }

        public DateTimeOffset CreatedOn { get; set; }
        public string? CreatedByUserId { get; set; }

    }
}
