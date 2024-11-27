﻿using System;
using System.Threading.Tasks;
using MySeries.Application.Contracts.Notificaciones;
using MySeries.Domain.Notificaciones;
using MySeries.Notificaciones;

namespace MySeries.Application.Notificaciones
{
    public class NotificadorEmail : INotificator
    {
        public bool PuedeEnviar(TipoNotificacion tipo)
        {
            return tipo == TipoNotificacion.Email;
        }

        public async Task EnvioNotificacionesAsync(NotificacionDto notificacionDTO)
        {
            //pasar de dto a entidad
            var notificacion = new Notificacion
            {
                UsuarioId = notificacionDTO.UsuarioId,
                Titulo = notificacionDTO.Titulo,
                Msj = notificacionDTO.Msj,
                Leida = false,
                Tipo = notificacionDTO.Tipo,
            };

            await Task.Delay(1000);
            Console.WriteLine($"Enviar Mail a {notificacion.UsuarioId}: {notificacion.Titulo} - {notificacion.Msj}");
        }
    }
}