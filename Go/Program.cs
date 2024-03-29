using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Go.Shared.Models;
using Go.Shared.Models.MongoDB;
using MongoDB.Driver;
using Blazored.LocalStorage;

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

    public static class Globals
    {
        // Access these globals from anywhere via Globals.<varname>, for example Globals.MONGO_URL
        public const string MONGO_URL = "mongodb://127.0.0.1:27017";
        public static MongoClient MONGO_CLIENT = new MongoClient(MONGO_URL);
        public static IMongoDatabase DB = MONGO_CLIENT.GetDatabase("main");

        // These are the primary interfaces we will use for each collection.
        // 'users' collection is a collection of IUser objs, defined in Shared.DBModels.User.cs.
        public static IMongoCollection<IUser> USERS = GetCollection<IUser>(Globals.DB, "users");
        public static IMongoCollection<Lesson> LESSONS = GetCollection<Lesson>(Globals.DB, "lessons");
        public static IMongoCollection<Game> GAMES = GetCollection<Game>(Globals.DB, "games");

        public static async Task<IUser> GetUser(ILocalStorageService localStorage)
        {
            string connection_string = await localStorage.GetItemAsync<string>("connection_string");
            return UserDBInterface.GetUserFromConnString(connection_string);
        }

        public static async Task SetUser(ILocalStorageService localStorage, IUser user)
        {
            if (user is null)
            {
                await LogoutUser(localStorage);
            }
            else
            {
                UserDBInterface.setConnectionString(((Player)user), GenerateConnectionString(16));
                await localStorage.SetItemAsync<string>("connection_string", user.connection_string);
            }
        }

        public static async Task LogoutUser(ILocalStorageService localStorage)
        {
            await localStorage.RemoveItemAsync("connection_string");
        }

        // More testable way to set the globals.
        private static IMongoCollection<T> GetCollection<T> (IMongoDatabase db, string t)
        {
            return db.GetCollection<T>(t);
        }
        private static string GenerateConnectionString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string connection_string = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            if (UserDBInterface.GetUserFromConnString(connection_string) != null)
                connection_string = GenerateConnectionString(length);
            return connection_string;
        }
    }
}
