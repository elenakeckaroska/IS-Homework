using EBilets.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EBilets.Domain.DTO
{
    public class BilletsByGenreDto
    {
        public List<Billet> Billets { get; set; }
        public string SelectedGenre { get; set; }
        public List<string> Genres { get; set; }
    }
}
