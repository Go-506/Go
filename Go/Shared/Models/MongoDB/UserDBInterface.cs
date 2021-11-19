using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Go.Shared.Models.MongoDB
{// This is just a collection of useful safe access methods that I found helpful
 // You don't have to use them, you can directly use the client.
    public class UserDBInterface
    {
        public static bool InsertUser(IUser user)
        {
            IMongoCollection<IUser> coll = Globals.USERS;
            IUser alreadyThere = coll.Find(x => x.name.Equals(user.name)).FirstOrDefault();
            // Username already exists, p much
            if (alreadyThere != null)
            {
                return false;
            }
            coll.InsertOne(user);
            return true;
        }

        public static IUser GetUser(string name)
        {
            IMongoCollection<IUser> coll = Globals.USERS;
            return coll.Find(x => x.name.Equals(name)).FirstOrDefault();
        }
        public static IUser GetUserFromConnString(string name)
        {
            IMongoCollection<IUser> coll = Globals.USERS;
            return coll.Find(x => ((Player)x).connection_string.Equals(name)).FirstOrDefault();
        }

        public static bool UpdateUser(IUser user)
        {
            Globals.USERS.DeleteOne(Builders<IUser>.Filter.Eq("name", user.name));
            InsertUser(user);
            return true;
        }
        public static bool setConnectionString(Player user, String connection_string)
        {
            user.setConnectionString(connection_string);
            UpdateUser(user);
            return true;
        }
    }
}

