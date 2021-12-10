using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Go.Shared.Models.MongoDB
{
    public class LessonDBInterface
    {
        public static bool InsertLesson(Lesson lesson)
        {
            IMongoCollection<Lesson> coll = Globals.LESSONS;
            coll.InsertOne(lesson);
            return true;
        }

        public static Lesson GetLesson(string name)
        {
            IMongoCollection<Lesson> coll = Globals.LESSONS;
            return coll.Find(x => x.name.Equals(name)).FirstOrDefault();
        }

        public static List<Lesson> GetLessonList()
        {
            IMongoCollection<Lesson> coll = Globals.LESSONS;
            List<Lesson> lessonList = coll.Find(p => true).ToListAsync().Result;
            return lessonList;
        }
    }
}
