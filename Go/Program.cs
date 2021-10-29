using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Go.Shared.Models;
using MongoDB.Driver;

namespace Go
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

    public class Globals
    {
        // Access these globals from anywhere via Globals.<varname>, for example Globals.MONGO_URL
        public const string MONGO_URL = "mongodb://127.0.0.1:27017";
        public static MongoClient MONGO_CLIENT = new MongoClient(MONGO_URL);
        public static IMongoDatabase DB = MONGO_CLIENT.GetDatabase("main");

        // These are the primary interfaces we will use for each collection.
        // 'users' collection is a collection of IUser objs, defined in Shared.DBModels.User.cs.
        public static IMongoCollection<IUser> USERS = DB.GetCollection<IUser>("users");

        public static IUser CURR_USER { get; set; } = new Guest("guest");

    }
}
