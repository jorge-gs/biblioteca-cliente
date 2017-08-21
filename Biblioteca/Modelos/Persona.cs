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
    public class Persona
    {
        private static IMongoCollection<Persona> _coleccion;
        public static IMongoCollection<Persona> Coleccion
        {
            get
            {
                if (_coleccion == null)
                {
                    _coleccion = Cliente.db.GetCollection<Persona>("personas");
                }

                return _coleccion;
            }
        }

        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("identidad")]
        public string Identidad { get; set; }
        [BsonElement("nombre")]
        public string Nombre { get; set; }
        [BsonElement("apellido")]
        public string Apellido { get; set; }
        [BsonElement("catedratico")]
        public bool Catedratico { get; set; }
        [BsonElement("estudiante")]
        [BsonIgnoreIfNull]
        public Estudiante Estudiante { get; set; }

        public string NombreCompleto
        {
            get => Nombre + " " + Apellido;
        }

        public string CodigoNombre
        {
            get => Identidad + ", " + NombreCompleto;
        }

        public override string ToString()
        {
            return CodigoNombre;
        }
    }

    public class Estudiante
    {
        [BsonElement("noCuenta")]
        public string NoCuenta { get; set; }
        [BsonElement("carreras")]
        public List<ObjectId> Carreras { get; set; }

        public void AgregarCarrera(Carrera carrera)
        {
            if (Carreras == null) { Carreras = new List<ObjectId>(); }
            Carreras.Add(carrera.Id);
        }
    }
}
