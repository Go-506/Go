﻿using System;
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
        public string name { get; protected set; }
        public List<LessonPage> lessonPages { get; protected set; }

        public Lesson(string name)
        {
            this.name = name;
        }
    }
}
