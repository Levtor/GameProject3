using System;
using System.Collections.Generic;
using System.Text;

namespace GameProject3
{
    public interface IMaze
    {
        public int Height { get; }
        public int Width { get; }

        public int ExitX { get; }
        public int ExitY { get; }

        public int StartX { get; }
        public int StartY { get; }
        public CardinalDirection StartDirection { get; }

        public CardinalDirection ExitDirection { get; }

        public bool HasWall(int X, int Y, CardinalDirection cardinal);
    }
}
