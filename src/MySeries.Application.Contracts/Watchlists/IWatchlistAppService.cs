﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace MySeries.Watchlists
{
    public interface IWatchlistAppService : IApplicationService
    {
        Task AgregarSerieAsync(string title);
    }
}
