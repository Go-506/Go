using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.Shared.Models
{
    public class LessonPage
    {
        public string text { get; set; }
        public string img { get; set; }
        public Shared.Models.Puzzle puzzle { get; set; }
        public LessonPage()
        {

        }
    }
}