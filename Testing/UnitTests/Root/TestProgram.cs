using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Driver;
using Go.Shared.Models;

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
            Assert.IsType<string>(Go.Globals.MONGO_URL);
        }

        [Fact]
        public void MongoUrlIsValid()
        {
            Assert.True(Uri.IsWellFormedUriString(Go.Globals.MONGO_URL, UriKind.Absolute));
        }

        [Fact]
        public void MongoClientInitialized()
        {
            Assert.IsType<MongoClient>(Go.Globals.MONGO_CLIENT);
        }

        [Fact]
        public void MongoDBInitialized()
        {
            Assert.True(Go.Globals.DB is IMongoDatabase);
        }

        [Fact]
        public void UserInterfaceInitialized()
        {
            Assert.True(Go.Globals.USERS is IMongoCollection<IUser>);
        }

        [Fact]
        public void LessonInterfaceInitialized()
        {
            Assert.True(Go.Globals.LESSONS is IMongoCollection<Lesson>);
        }

        [Fact]
        public void GameInterfaceInitialized()
        {
            Assert.True(Go.Globals.GAMES is IMongoCollection<Game>);
        }


    }
}
