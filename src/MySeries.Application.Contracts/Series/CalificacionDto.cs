using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace MySeries.Series
{
    public class CalificacionDto
    {
        [Range(1,10,ErrorMessage = "Puntuacion en 1 y 10")]
        public int calificacion {  get; set; }
        public string? comentario { get; set; }
        public DateTime FechaCalificacion { get; set; }

        //Foreing Key
        public int SerieId { get; set; }
        public Guid UsuarioId { get; set; }


    }
}
