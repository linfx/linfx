﻿using LinFx.Security.Claims;
using System.Linq;
using System.Threading.Tasks;

namespace LinFx.Extensions.Authorization.Permissions
{
    /// <summary>
    /// 角色授权提供者
    /// </summary>
    public class RolePermissionValueProvider : PermissionValueProvider
    {
        public const string ProviderName = "Role";

        public override string Name => ProviderName;

        public RolePermissionValueProvider(IPermissionStore permissionStore)
            : base(permissionStore) { }

        public override async Task<PermissionValueProviderGrantInfo> CheckAsync(PermissionValueCheckContext context)
        {
            var roles = context.Principal?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray();
            if (roles == null || !roles.Any())
                return PermissionValueProviderGrantInfo.NonGranted;

            foreach (var role in roles)
            {
                if (await PermissionStore.IsGrantedAsync(context.Permission.Name, Name, role))
                    return new PermissionValueProviderGrantInfo(true, role);
            }

            return PermissionValueProviderGrantInfo.NonGranted;
        }
    }
}
