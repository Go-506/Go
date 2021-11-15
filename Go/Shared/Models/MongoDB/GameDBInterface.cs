using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Go.Shared.Models.MongoDB
{
    public class GameDBInterface
    {
        public static bool InsertGame(Game game)
        {
            IMongoCollection<Game> coll = Globals.GAMES;
            coll.InsertOne(game);
            return true;
        }

        public static Game GetGame(string id)
        {
            IMongoCollection<Game> coll = Globals.GAMES;
            return coll.Find(x => x.Id.Equals(new ObjectId(id))).FirstOrDefault();
        }

        public static bool UpdateGame(Game game)
        {
            
            Globals.GAMES.DeleteOne(Builders<Game>.Filter.Eq("Id", game.Id));
            InsertGame(game);
            return true;
        }

        public static List<Game> GetGameList(IUser user)
        {
            IMongoCollection<Game> coll = Globals.GAMES;
            List<Game> gameList = new List<Game>();
            foreach(ObjectId id in user.gameHistory)
                gameList.Add(coll.Find(x => x.Id.Equals(id)).FirstOrDefault());
            return gameList;
        }
    }
}
