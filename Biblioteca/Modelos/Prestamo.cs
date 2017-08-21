using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Biblioteca.Modelos
{
    public class Prestamo
    {
        private static IMongoCollection<Prestamo> _coleccion;
        public static IMongoCollection<Prestamo> Coleccion
        {
            get
            {
                if (_coleccion == null)
                {
                    _coleccion = Cliente.db.GetCollection<Prestamo>("prestamos");
                }

                return _coleccion;
            }
        }

        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("persona")]
        public ObjectId Persona { get; set; }
        [BsonElement("prestable")]
		public ObjectId Prestable { get; set; }
		[BsonElement("fecha")]
		public DateTime Fecha { get; set; }
		[BsonElement("estado")]
		public Estado Estado { get; set; }
		[BsonElement("fechaDevolucion")]
		public DateTime FechaDevolucion { get; set; }
		[BsonElement("estadoDevolucion")]
		public Estado EstadoDevolucion { get; set; }
        [BsonElement("seccion"), BsonIgnoreIfNull]
        public ObjectId Seccion { get; set; }

        public Persona ObjetoPersona
        {
            get => Modelos.Persona.Coleccion.FindAsync(arg => arg.Id == Persona).Result.FirstOrDefault();
        }

        public Prestable ObjetoPrestable
        {
            get => Modelos.Prestable.Collection.FindAsync(arg => arg.Id == Prestable).Result.FirstOrDefault();
        }
        
        public Seccion ObjetoSeccion
        {
            get => Modelos.Seccion.Coleccion.Find(arg => arg.Id == Seccion).FirstOrDefault();
        }

        public DateTime FechaDia
        {
            get => Fecha.Date;
        }

        public DateTime? ObjetoFechaDevolucion
        {
            get
            {
                if (FechaDevolucion < Fecha) { return null; }
                return FechaDevolucion;
            }
        }

        public Estado? ObjetoEstadoDevolucion
        {
            get
            {
                if(FechaDevolucion < Fecha) { return null; }
                return EstadoDevolucion;
            }
        }
    }
}
