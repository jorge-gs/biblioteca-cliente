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
    public class Seccion
    {
        private static IMongoCollection<Seccion> _coleccion;
        public static IMongoCollection<Seccion> Coleccion
        {
            get
            {
                if (_coleccion == null)
                {
                    _coleccion = Cliente.db.GetCollection<Seccion>("secciones");
                }

                return _coleccion;
            }
        }

        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("clase")]
        public ObjectId Clase { get; set; }
        [BsonElement("periodo")]
        public ObjectId Periodo { get; set; }
        [BsonElement("catedratico")]
        public ObjectId Catedratico { get; set; }
        [BsonElement("letra")]
        public string Letra { get; set; }
        [BsonElement("horario")]
        public List<Horario> Horario { get; set; }

        public Clase ObjetoClase
        {
            get
            {
                return Modelos.Clase.Coleccion.FindAsync(arg => arg.Id == Clase).Result.FirstOrDefault();
            }
        }

        public Persona ObjetoCatedratico
        {
            get => Modelos.Persona.Coleccion.Find(arg => arg.Id == Catedratico).FirstOrDefault();
        }

        public Periodo ObjetoPeriodo
        {
            get => Modelos.Periodo.Coleccion.Find(arg => arg.Id == Periodo).FirstOrDefault();
        }

        public void AgregarHorario(Horario horario)
        {
            if (Horario == null) { Horario = new List<Modelos.Horario>(); }
            Horario.Add(horario);
        }

        public override string ToString()
        {
            return Letra;
        }
    }

    public class Horario
    {
        [BsonElement("dia")]
        public Dia Dia { get; set; }
        [BsonElement("inicio")]
        public DateTime Inicio { get; set; }
        [BsonElement("duracion")]
        public DateTime Duracion { get; set; }
    }

    public enum Dia 
    {
        Domingo, Lunes, Martes, Miercoles, Jueves, Viernes, Sabado
    }
}
