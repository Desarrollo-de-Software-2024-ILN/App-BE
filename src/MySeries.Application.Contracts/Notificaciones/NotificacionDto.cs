﻿using MySeries.Notificaciones;
using System;

namespace MySeries.Application.Contracts.Notificaciones
{
    public class NotificacionDto
    {
        public int UsuarioId { get; set; }
        public string Titulo { get; set; }
        public string Msj { get; set; }
        public bool Leida { get; set; }
        public TipoNotificacion Tipo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
