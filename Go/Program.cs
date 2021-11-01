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
        public static IMongoCollection<IUser> USERS = DB.GetCollection<IUser>("users");
        public static IMongoCollection<Lesson> LESSONS = DB.GetCollection<Lesson>("lessons");
        public static IMongoCollection<Game> GAMES = DB.GetCollection<Game>("games");

        public static IUser CURR_USER { get; set; } = new Guest("guest");

        public static async Task<IUser> GetUser(ILocalStorageService localStorage)
        {
            string username = await localStorage.GetItemAsync<string>("user");
            return UserDBInterface.GetUser(username);
        }

        public static async Task SetUser(ILocalStorageService localStorage, IUser user)
        {
            await localStorage.SetItemAsync<string>("user", user.name);
        }

        public static async Task LogoutUser(ILocalStorageService localStorage)
        {
            await localStorage.RemoveItemAsync("user");
        }

    }
}
