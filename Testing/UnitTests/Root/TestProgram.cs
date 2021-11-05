using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Driver;
using Go.Shared.Models;
using Go.Shared.Models.MongoDB;

using Go;
using Moq;
using Xunit;

namespace Testing.UnitTests.Root
{
    public class TestProgram
    {

        [Fact]
        public void MongoUrlExists()
        {
            Assert.IsType<string>(Globals.MONGO_URL);
        }

        [Fact]
        public void MongoUrlIsValid()
        {
            Assert.True(Uri.IsWellFormedUriString(Globals.MONGO_URL, UriKind.Absolute));
        }

        [Fact]
        public void MongoClientInitialized()
        {
            Assert.IsType<MongoClient>(Globals.MONGO_CLIENT);
        }

        [Fact]
        public void MongoDBInitialized()
        {
            Assert.True(Globals.DB is IMongoDatabase);
        }

        [Fact]
        public void UserInterfaceInitialized()
        {
            Assert.True(Globals.USERS is IMongoCollection<IUser>);
        }

        [Fact]
        public void LessonInterfaceInitialized()
        {
            Assert.True(Globals.LESSONS is IMongoCollection<Lesson>);
        }

        [Fact]
        public void GameInterfaceInitialized()
        {
            Assert.True(Globals.GAMES is IMongoCollection<Game>);
        }


    }
}
