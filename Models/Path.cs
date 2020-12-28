using System.Collections.Generic;

namespace Hodogram.Models
{
    public class Path
    {
        public Field field { get; set; }
        public int cost { get; set; }
        private bool stuck { get; set; }
    }
}