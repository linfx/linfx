﻿using LinFx.Application.Models;

namespace LinFx.Extensions.TenantManagement
{
    public class TenantInput : PagedAndSortedResultRequest
    {
        /// <summary>
        /// 过滤条件
        /// </summary>
        public string Filter { get; set; }
    }
}