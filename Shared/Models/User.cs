using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Go.Shared.Models
{
    // Interface defining the basic attributes of every user
    // name should be readonly (no set method) because it's the primary key
    // type should be readonly in general because it's tied to obj type.
    // There are issues with deserializing interfaces, so this is an abstract class.
    [BsonKnownTypes(typeof(Applicant), typeof(Owner), typeof(Tenant))]
    public abstract class IUser
    {
        [BsonId]
        public string name { get; protected set; }
        public string type { get; protected set; }
        public string password { get; protected set; }
    }

    // Applicant obj with basic implementation. Will need some kind of Application
    // obj, but that's not my (alex)'s job.
    public class Applicant : IUser
    {
        public Applicant(string name, string password)
        {
            this.name = name;
            this.password = password;
            this.type = "applicant";
        }

    }

    // Owner obj, implements abstract class IUser
    public class Owner : IUser
    {
        public Owner(string name, string password)
        {
            this.name = name;
            this.password = password;
            this.type = "owner";
        }
    }

    // Tenant obj, implements abstract class IUser
    public class Tenant : IUser
    {
        public Tenant(string name, string password)
        {
            this.name = name;
            this.password = password;
            this.type = "tenant";
        }
    }
}

