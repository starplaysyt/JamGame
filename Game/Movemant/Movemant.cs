using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamGame.Game.Movemant
{
    class GameObject
    {
        public char Symbol { get; set; }

        public GameObject(char symbol)
        {
            Symbol = symbol;
        }
    }

    class Player : GameObject
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Player(int x, int y) : base('@')
        {
            X = x;
            Y = y;
        }

        public List<(int, int)> FindPath(Map map, int targetX, int targetY)
        {
            var queue = new Queue<(int, int)>();
            var visited = new bool[map.Height, map.Width];
            var parent = new Dictionary<(int, int), (int, int)>();

            queue.Enqueue((X, Y));
            visited[Y, X] = true;

            while (queue.Count > 0)
            {
                var (currentX, currentY) = queue.Dequeue();

                if (currentX == targetX && currentY == targetY)
                {
                    var path = new List<(int, int)>();
                    var current = (targetX, targetY);
                    while (!current.Equals((X, Y)))
                    {
                        path.Add(current);
                        current = parent[current];
                    }
                    path.Reverse();
                    return path;
                }

                var directions = new[] { (0, 1), (0, -1), (1, 0), (-1, 0) };
                foreach (var (dx, dy) in directions)
                {
                    int newX = currentX + dx;
                    int newY = currentY + dy;

                    if (map.CanMove(newX, newY) && !visited[newY, newX])
                    {
                        queue.Enqueue((newX, newY));
                        visited[newY, newX] = true;
                        parent[(newX, newY)] = (currentX, currentY);
                    }
                }
            }

            return new List<(int, int)>();
        }

        public void MoveAlongPath(Map map, List<(int, int)> path)
        {
            foreach (var (newX, newY) in path)
            {
                map.MapObjects[Y, X] = new GameObject(' ');

                X = newX;
                Y = newY;

                map.MapObjects[Y, X] = this;

                Console.Clear();
                map.Render();
                Thread.Sleep(300);
            }
        }
    }


    class Map
    {
        public GameObject[,] MapObjects { get; private set; }
        public bool[,] Passability { get; private set; }
        public Player Player { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            MapObjects = new GameObject[height, width];
            Passability = new bool[height, width];
            InitializeMap();
        }
        private void InitializeMap()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (x == 0 || x == Width - 1 || y == 0 || y == Height - 1)
                    {
                        MapObjects[y, x] = new GameObject('1');
                        Passability[y, x] = false;
                    }
                    else
                    {
                        MapObjects[y, x] = new GameObject(' ');
                        Passability[y, x] = true;
                    }
                }
            }

            AddObstacle(3, 3);
            AddObstacle(5, 7);
            AddObstacle(2, 6);
            AddObstacle(7, 4);

            Player = new Player(1, 1);
            MapObjects[Player.Y, Player.X] = Player;
        }

        private void AddObstacle(int x, int y)
        {
            Passability[y, x] = false;
            MapObjects[y, x] = new GameObject('1');
        }

        public bool CanMove(int x, int y)
        {
            return x >= 0 && x < Width &&
                   y >= 0 && y < Height &&
                   Passability[y, x];
        }

        public void Render()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Console.Write(MapObjects[y, x].Symbol);
                }
                Console.WriteLine();
            }
        }
    }
}
