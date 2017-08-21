using System;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Biblioteca
{
    public class Cliente
    {
        private static MongoClient cliente;
        public static IMongoDatabase db;

        static Cliente() {
            Cliente.cliente = new MongoClient();
            Cliente.db = cliente.GetDatabase("biblioteca");
        }
    }
}
