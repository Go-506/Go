using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections;
using Go.Shared.Models.MongoDB;

namespace Go.Shared.Models
{
    public class Lesson
    {
        [BsonId]
        public string name { get; set; }
        public List<LessonPage> lessonPages { get; set; }
        public Lesson(string name, List<LessonPage> lessonPages)
        {
            this.name = name;
            this.lessonPages = lessonPages;
        }
    }
}
