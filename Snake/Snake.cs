using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    internal class Snake
    {
        static void Main()
        {

            double frames = 0.2;
            double delay = 1000 / frames;
            Console.WriteLine("frame delay is " + delay);

            long lastFrame = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            int fieldSize = 5;
            int startLength = 2;

            while (true)
            {

                string frame;

                // add edges
                for (int row = 0; row < fieldSize + 2; row++)
                {
                    for (int col = 0; col < fieldSize + 2; col++)
                    {
                        int state = isEdge(row, fieldSize) ? 1 : 0 + isEdge(col, fieldSize) ? 1 : 0;
                        if (isEdge(row, fieldSize) && isEdge(col, fieldSize))
                        {
                            frame += "+";
                            continue;
                        }
                        if (isEdge(row, fieldSize))
                        {
                            frame += "-";
                            continue;
                        }
                        if (isEdge(col, fieldSize))
                        {
                            frame += "|";
                            continue;
                        }
                    }
                }


                int timeSinceLastFrame = (int) (DateTimeOffset.Now.ToUnixTimeMilliseconds() - lastFrame);
                Console.WriteLine(timeSinceLastFrame);
                if (timeSinceLastFrame < delay) {
                    Task.Delay((int)(delay - timeSinceLastFrame));
                }

                // draw the frame
                Console.WriteLine("frame");

                lastFrame = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            }

        }

        private static bool isEdge(int coord, int fieldSize)
        {
            return coord == 0 || coord == fieldSize + 1;
        }

    }

    internal class Game
    {
        private int[][] field;
        private Vector2 head;
        private Vector2 direction;

        public Game(int size)
        {
            field = new int[size][];
        }

    }

    internal class QueueMap<T>
    {

        private Queue<T> queue;
        private int deleteAt;

        public QueueMap(int size)
        {
            this.deleteAt = size;
        }

        public void add(T value) 
        {
            if (map.Count() == deleteAt)
            {
                queue.Dequeue()
            }
        }
    }

    internal class SnakeTile
    {

        private Vector2 position;
        private Vector2 direction;

        public SnakeTile(Vector2 position, Vector2 direction)
        {
            this.position = position;
            this.direction = direction;
        }

        public Vector2 GetPosition()
        {
            return this.position;
        }
        public Vector2 GetDirection()
        {
            return this.direction;
        }
    }

}
