using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossplatformLab3
{
    internal class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Cell Previous { get; set; }
        public bool IsBlocked { get; set; }

        public Cell(int x,int y,Cell previous,bool isBlocked)
        {
            X = x;
            Y = y;
            Previous = previous;
            IsBlocked = isBlocked;
        }
    }
}
