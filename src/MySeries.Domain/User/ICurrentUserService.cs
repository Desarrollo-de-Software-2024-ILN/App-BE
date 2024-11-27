using System;

namespace MySeries.User
{
    public interface ICurrentUserService
    {
        Guid? GetCurrentUserId();
    }
}
