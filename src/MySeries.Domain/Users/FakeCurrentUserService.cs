using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySeries.Users
{
    public class FakeCurrentUserService : ICurrentUserService
    {
        public Guid? GetCurrentUserID()
        {
            return Guid.NewGuid();
        }
    }
}
