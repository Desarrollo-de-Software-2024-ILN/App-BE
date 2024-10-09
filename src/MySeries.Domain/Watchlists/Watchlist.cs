﻿
using MySeries.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace MySeries.Watchlists
{
    public class Watchlist : AggregateRoot<int>
    {
        public List<Serie> Series { get; set; }

        public Watchlist()
        {
            Series = new List<Serie>();
        }
    }
}
