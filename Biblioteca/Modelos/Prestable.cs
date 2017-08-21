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
    public class Prestable
    {
        private static IMongoCollection<Prestable> _coleccion;
        public static IMongoCollection<Prestable> Collection
        {
            get
            {
                if (_coleccion == null)
                {
                    _coleccion = Cliente.db.GetCollection<Prestable>("prestables");
                }

                return _coleccion;
            }
        }

        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("codigo")]
        public string Codigo { get; set; }
        [BsonElement("tipo")]
        public Tipo Tipo { get; set; }
        [BsonElement("estado")]
        public Estado Estado { get; set; }
        [BsonElement("descripcion")]
        [BsonIgnoreIfNull]
        public string Descripcion { get; set; }
        [BsonElement("copia")]
        [BsonIgnoreIfNull]
        public Copia Copia { get; set; }

        public override string ToString()
        {
            var libro = (Copia == null) ? "" : " " + Copia.ObjetoLibro.Titulo + " " + Copia.ObjetoLibro.AutoresConcat;
            return Tipo.ToString() + " " + Codigo + " " + libro;
        }
    }

    public class Copia
    {
        [BsonElement("libro")]
        public ObjectId Libro { get; set; }
        [BsonElement("numero")]
        public int Numero { get; set; }

        public Libro ObjetoLibro
        {
            get => Modelos.Libro.Coleccion.FindAsync(arg => arg.Id == Libro).Result.FirstOrDefault();
        }
    }

    public enum Tipo
    {
        Aula, Cubiculo, Libro, Otro
    }

    public enum Estado
    {
        Nuevo, Bueno, Aceptable, Deteriorado, Retirado
    }
}
