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
    public class Libro
    {
        private static IMongoCollection<Libro> _coleccion;
        public static IMongoCollection<Libro> Coleccion
        {
            get
            {
                if (_coleccion == null)
                {
                    _coleccion = Cliente.db.GetCollection<Libro>("libros");
                }

                return _coleccion;
            }
        }

        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("isbn")]
        public string ISBN { get; set; }
        [BsonElement("titulo")]
        public string Titulo { get; set; }
        [BsonElement("editorial")]
        public ObjectId Editorial { get; set; }
        [BsonElement("autores")]
        public List<ObjectId> Autores { get; set; }

        public Editorial ObjetoEditorial
        {
            get => Modelos.Editorial.Coleccion.FindAsync(arg => arg.Id == Editorial).Result.FirstOrDefault();
        }

        public string AutoresConcat
        {
            get
            {
                var autores = ListaAutores;
                var ret = "";
                for (int i = 0; i < autores.Count; i++)
                {
                    ret += autores[i].Nombre;
                    if (i < Autores.Count - 1)
                    {
                        ret += ", ";
                    }
                }

                return ret;
            }
        }

        public List<Autor> ListaAutores
        {
            get => Modelos.Autor.Coleccion.Find(arg => arg.Libros.Contains(Id)).ToList();
        }

        public void AgregarAutor(Autor autor)
        {
            if (Autores == null) { Autores = new List<ObjectId>(); }
            Autores.Add(autor.Id);
        }

        public override string ToString()
        {
            return Titulo + ": " + AutoresConcat;
        }
    }
}
