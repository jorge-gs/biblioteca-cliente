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
    public class Carrera
    {
        private static IMongoCollection<Carrera> _coleccion;
        public static IMongoCollection<Carrera> Coleccion
        {
            get
            {
                if (_coleccion == null) 
                {
                    _coleccion = Cliente.db.GetCollection<Carrera>("carreras");
                }
                return _coleccion;
            }
        }

        //Get Set
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("codigo")]
        public string Codigo { get; set; }
        [BsonElement("nombre")]
        public string Nombre { get; set; }
    }
}
