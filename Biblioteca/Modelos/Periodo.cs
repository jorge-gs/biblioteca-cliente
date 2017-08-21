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
    public class Periodo
    {
        private static IMongoCollection<Periodo> _coleccion;
        public static IMongoCollection<Periodo> Coleccion
        {
            get
            {
                if (_coleccion == null)
                {
                    _coleccion = Cliente.db.GetCollection<Periodo>("periodos");
                }

                return _coleccion;
            }
        }

        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("fechaInicio")]
        public DateTime FechaInicio { get; set; }
        [BsonElement("fechaFin")]
        public DateTime FechaFin { get; set; }
        [BsonElement("numero")]
        public int Numero { get; set; }

        public override string ToString()
        {
            return Numero + " " + FechaInicio.Year;
        }
    }
}
