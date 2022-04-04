using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace GameProject3
{
    public class SquareMaze : Maze
    {
        private int[] TileMap;

        public int Height { get; }
        public int Width { get; }

        public int ExitX { get; }
        public int ExitY { get; }
        public CardinalDirection ExitDirection { get; }

        public int StartX { get; }
        public int StartY { get; }
        public CardinalDirection StartDirection { get; }

        public SquareMaze(string mazeFile, ContentManager contentManager)
        {
            string data = File.ReadAllText(Path.Join(contentManager.RootDirectory, mazeFile));
            string[] lines = data.Split('\n');

            string[] dimensions = lines[0].Split(',');
            Width = int.Parse(dimensions[0]);
            Height = int.Parse(dimensions[1]);

            string[] startInfo = lines[1].Split(',');
            StartX = int.Parse(startInfo[0]);
            StartY = int.Parse(startInfo[1]);
            StartDirection = (CardinalDirection)int.Parse(startInfo[2]);

            string[] exitInfo = lines[2].Split(',');
            ExitX = int.Parse(exitInfo[0]);
            ExitY = int.Parse(exitInfo[1]);
            ExitDirection = (CardinalDirection)int.Parse(exitInfo[2]);

            TileMap = new int[Width * Height];
            string[] mazeRow;
            for (int i = 0; i < Height; i++)
            {
                mazeRow = lines[i+3].Split(',');
                for (int j = 0; j < Width; j++)
                {
                    TileMap[i * Width + j] = int.Parse(mazeRow[j]);
                }
            }
        }

        public bool HasWall(int X, int Y, CardinalDirection cardinal)
        {
            int buddyX = X;
            int buddyY = Y;
            if (X < 0 || Y < 0 || X >= Width || Y >= Height) return false;
            switch (cardinal)
            {
                case CardinalDirection.North:
                    buddyY--;
                    if (buddyY < 0 || buddyY >= Height) return false;
                    break;
                case CardinalDirection.West:
                    buddyX--;
                    if (buddyX < 0 || buddyX >= Width) return false;
                    break;
                case CardinalDirection.South:
                    buddyY++;
                    if (buddyY < 0 || buddyY >= Height) return false;
                    break;
                case CardinalDirection.East:
                    buddyX++;
                    if (buddyX < 0 || buddyX >= Width) return false;
                    break;
            }

            bool meIsEmpty = TileMap[Y * Width + X] == 0;
            bool buddyIsEmpty = TileMap[buddyY * Width + buddyX] == 0;

            return meIsEmpty ^ buddyIsEmpty;
        }
    }
}
