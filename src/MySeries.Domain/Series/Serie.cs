﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace MySeries.Series
{
    public class Serie : AggregateRoot<int>, IMustHaveCreator<Guid>
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Descripcion { get; set; }

        public string IdSerie { get; set; }

        public Guid Creator { get; set; }
        public Guid CreatorId { get; set; }

        public ICollection<Calification> Califications { get; set; }
        public Serie()
        {
            Califications = new List<Calification>();
        }
    }
}
