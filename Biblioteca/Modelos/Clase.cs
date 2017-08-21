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
    public class Clase
    {
        private static IMongoCollection<Clase> _coleccion;
        public static IMongoCollection<Clase> Coleccion
        {
            get
            {
                if (_coleccion == null)
                {
                    _coleccion = Cliente.db.GetCollection<Clase>("clases");
                }

                return _coleccion;
            }
        }

        [BsonId]
		public ObjectId Id { get; set; }
		[BsonElement("codigo")]
		public string Codigo { get; set; }
		[BsonElement("nombre")]
		public string Nombre { get; set; }
		[BsonElement("horas")]
        public int Horas { get; set; }
		[BsonElement("uvs")]
        public int UVs { get; set; }
        [BsonElement("secciones"), BsonIgnoreIfNull]
        public List<ObjectId> Secciones { get; set; }

        public void AgregarSeccion(Seccion seccion)
        {
            if (Secciones == null) { Secciones = new List<ObjectId>(); }
            Secciones.Add(seccion.Id);
        }

        public override string ToString()
        {
            return Codigo + " " + Nombre;
        }
    }
}
