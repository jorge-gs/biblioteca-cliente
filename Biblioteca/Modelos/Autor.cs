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
    public class Autor
    {
        private static IMongoCollection<Autor> _coleccion;
        public static IMongoCollection<Autor> Coleccion
        {
            get
            {
                if (_coleccion == null)
                {
                    _coleccion = Cliente.db.GetCollection<Autor>("autores");
                }

                return _coleccion;
            }
		}

		[BsonId]
		public ObjectId Id { get; set; }
		[BsonElement("nombre")]
		public string Nombre { get; set; }
		[BsonElement("libros")]
        [BsonIgnoreIfNull]
        public List<ObjectId> Libros { get; set; }

        public void AgregarLibro(Libro libro)
        {
            if (Libros == null) { Libros = new List<ObjectId>(); }
            Libros.Add(libro.Id);
        }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
