using Domain.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Identity
{
    public class User : IdentityUser<string>, IEntityBase<string>, ICreatedByEntity, IModifiedByEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTimeOffset CreatedOn { get; set; }
        public string? CreatedByUserId { get; set; }

        public DateTimeOffset? ModifiedOn { get; set; }
        public string? ModifiedByUserId { get; set; }
    }
}
