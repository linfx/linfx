﻿using LinFx.Extensions.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LinFx.Extensions.Account
{
    /// <summary>
    /// token 服务
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// create an access_token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string> CreateAccessTokenAsync(User user);

        /// <summary>
        /// create refresh_token
        /// </summary>
        /// <returns></returns>
        string CreateRefreshToken();

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

        bool ValidateToken(string identityId, string token);

        void RemoveUserToken(long userId);
    }
}
