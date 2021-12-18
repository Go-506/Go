using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Go.Shared.Models
{
    public class Puzzle
    {
        public Game game { get; set; }
        public Queue<int[]> solution { get; set; }
        public Queue<string> hints { get; set; }
        public Queue<string> correct { get; set; }
        public Puzzle(Game game)
        {
            this.game = game;
        }

        [JsonConstructorAttribute]
        public Puzzle()
        {

        }
    }
}
