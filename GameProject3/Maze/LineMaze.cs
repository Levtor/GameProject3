using System;
using System.Collections.Generic;
using System.Text;

namespace GameProject3
{
    public class LineMaze : Maze
    {
        private bool[] HorizontalWallOrNot;
        private bool[] VerticalWallOrNot;

        public int Height { get; }
        public int Width { get; }

        public int ExitX { get; }
        public int ExitY { get; }
        public CardinalDirection ExitDirection { get; }

        public int StartX { get; }
        public int StartY { get; }
        public CardinalDirection StartDirection { get; }

        public LineMaze(int width, int height, bool[] horizontalDefinition, bool[] verticalDefinition,
            int exitX, int exitY, CardinalDirection exitDirection, int startX, int startY, CardinalDirection startDirection)
        {
            Width = width;
            Height = height;

            HorizontalWallOrNot = horizontalDefinition;
            VerticalWallOrNot = verticalDefinition;

            ExitX = exitX;
            ExitY = exitY;
            ExitDirection = exitDirection;

            StartX = startX;
            StartY = startY;
            StartDirection = startDirection;
        }

        public bool HasWall(int X, int Y, CardinalDirection cardinal)
        {
            bool horizontal = false;
            switch (cardinal)
            {
                case CardinalDirection.North:
                    horizontal = true;
                    break;
                case CardinalDirection.West:
                    horizontal = false;
                    break;
                case CardinalDirection.South:
                    horizontal = true;
                    Y += 1;
                    break;
                case CardinalDirection.East:
                    horizontal = false;
                    X += 1;
                    break;
            }
            if (horizontal)
            {
                int index = Y * Width + X;
                if (index < 0 || index >= HorizontalWallOrNot.Length) return false;
                else return HorizontalWallOrNot[index];
            }
            else
            {
                int index = Y * (Width + 1) + X;
                if (index < 0 || index >= VerticalWallOrNot.Length) return false;
                else return VerticalWallOrNot[index];
            }
        }
    }
}
