﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySeries.Series
{
    public interface ISeriesApiService
    {
        Task<SerieDto[]> GetSeriesAsync(string title, string gender);
    }
}