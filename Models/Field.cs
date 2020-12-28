using System;

namespace Hodogram.Models
{
    public class Field
    {
        public Guid Id { get; set; }
        public int X_Cord { get; set; }
        public int Y_Cord { get; set; }
        public Field Up { get; set; }
        public Field Down { get; set; }
        public Field Left { get; set; }
        public Field Right { get; set; }
        public string Status { get; set; }
        /*
         1 - prazno
         2 - polica
         3 - zid          
         */
    }
}