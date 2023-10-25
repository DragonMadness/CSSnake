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

            while (true)
            {

                string frame;



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

    internal class QueueMap<T, K>
    {

        private Queue<> map;
        private int deleteAt;

        public QueueMap(int size)
        {
            this.deleteAt = size;
        }

        public void add(T key, K value) 
        {
            if (map.Count() == deleteAt)
            {

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
