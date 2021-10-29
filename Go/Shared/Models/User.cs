using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Microsoft.AspNetCore.Identity;

namespace Go.Shared.Models
{
    // Interface defining the basic attributes of every user
    // name should be readonly (no set method) because it's the primary key
    // type should be readonly in general because it's tied to obj type.
    // There are issues with deserializing interfaces, so this is an abstract class.
    [BsonKnownTypes(typeof(Guest), typeof(Player), typeof(Admin))]
    public abstract class IUser
    {
        [BsonId]
        public string name { get; protected set; }
        public string email { get; protected set; }
        public string type { get; protected set; }
        public string password { get; protected set; }
    }


    public class Guest : IUser
    {

        public Guest(string name)
        {
            this.name = name;
            this.type = "guest";
        }

    }

    public class Player : IUser
    {
        public Player(string name, string email, string password)
        {
            this.name = name;
            this.password = password;
            this.email = email;
            this.type = "player";
        }
        public void hashPassword()
        {
            this.password = BCrypt.Net.BCrypt.HashPassword(password);
        }
    }

    public class Admin : IUser
    {
        public Admin(string name, string password)
        {
            this.name = name;
            this.password = password;
            this.type = "admin";
        }
    }
}

