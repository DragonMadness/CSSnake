﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    internal class Program
    {

        public static Vector2[] directions = new Vector2[4] { new Vector2(0, -1), new Vector2(0, 1), new Vector2(-1, 0), new Vector2(1, 0) };
        public static bool isRunning = true;
        public static ConsoleKey lastPressedKey = ConsoleKey.Delete;
        public static Thread keyListener;

        static void Main()
        {

            double frames = 1;
            double delay = 1000 / frames;
            Console.WriteLine("frame delay is " + delay);

            int fieldSize = 15;
            int startLength = 3;

            Game game = new Game(fieldSize, startLength);
            long lastFrame = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            bool isInitial = true;

            while (isRunning)
            {

                if (isInitial)
                {
                    Console.WriteLine("-------------------------------------Console snake-------------------------------------");
                    Console.WriteLine("===================================by DragonMadness_===================================");
                    Console.WriteLine("-----------------------------------PRESS ANY  BUTTON-----------------------------------");
                    Console.ReadKey();

                    StartKeyListener();
                    isInitial = false;
                }

                Console.WriteLine(lastPressedKey.ToString());
                Vector2 direction = GetDirection(lastPressedKey);
                if (!game.TryMove(direction))
                {
                    isRunning = false;
                    keyListener.Join();
                    Console.WriteLine("You lost! Wanna try again? (y/n)");
                    char response = Console.ReadKey().KeyChar;
                    if (response == 'y') 
                    {
                        isRunning = true;
                        continue;
                    }
                    return;
                }

                string frame = game.Draw();

                int timeSinceLastFrame = (int) (DateTimeOffset.Now.ToUnixTimeMilliseconds() - lastFrame);
                if (timeSinceLastFrame < delay) {
                    Thread.Sleep((int)(delay - timeSinceLastFrame));
                }

                // draw the frame
                Console.WriteLine(frame);

                lastFrame = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            }

        }

        private static void StartKeyListener()
        {
            keyListener = new Thread(() =>
            {
                while (isRunning)
                {
                    lastPressedKey = Console.ReadKey(true).Key;
                }
            }
            );
            keyListener.Start();
        }

        public static Vector2 GetDirection(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow: return new Vector2(-1, 0);
                case ConsoleKey.DownArrow: return new Vector2(1, 0);
                case ConsoleKey.RightArrow: return new Vector2(0, 1);
                case ConsoleKey.LeftArrow: return new Vector2(0, -1);
                default: return new Vector2(0, 0);
            }
        }

    }

    internal class Game
    {
        private readonly int[][] field;
        private readonly Snake snake;

        public Game(int size, int length)
        {
            this.field = new int[size][];
            for (int row = 0; row < size; row++)
            {
                this.field[row] = new int[size];
                for (int col = 0; col < size; col++)
                {
                    this.field[row][col] = 0;
                }
            }


            this.snake = new Snake(length, new Vector2((int) Math.Floor(size / 2D), (int) Math.Floor(size / 2D)));
        }

        public Snake GetSnake()
        {
            return this.snake;
        }

        public int GetFieldSize()
        {
            return field.Length;
        }

        public string Draw()
        {
            string frame = "";

            int fieldSize = field.Length;
            for (int row = 0; row < fieldSize + 2; row++)
            {
                for (int col = 0; col < fieldSize + 2; col++)
                {
                    // Edges
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

                    // Field
                    frame += this.field[row - 1][col - 1] == 0 ? " " : "■";
                }
                frame += "\n";
            }

            return frame;
        }

        public bool TryMove(Vector2 overrideDirection)
        {
            SnakeTile head = this.snake.GetHead();
            Vector2 originalDirection = head.GetDirection();
            if ((overrideDirection.X != 0 || overrideDirection.Y != 0) && 
                field[(int) (head.GetPosition().X + overrideDirection.X)][(int) (head.GetPosition().Y + overrideDirection.Y)] == 0) { head.SetDirection(overrideDirection); }
            SnakeTile next = head.GetNext();

            if (!IsInside(next.GetPosition(), GetFieldSize())) return false;

            this.field[(int)next.GetPosition().X][(int)next.GetPosition().Y] = 1;
            SnakeTile tail = this.snake.Add(next);
            if (tail != null)
            {
                this.field[(int)tail.GetPosition().X][(int)tail.GetPosition().Y] = 0;
            }
            return true;
        }

        private static bool isEdge(int coord, int fieldSize)
        {
            return coord == 0 || coord == fieldSize + 1;
        }

        private static bool IsInside(Vector2 pos, int fieldSize)
        {
            Console.WriteLine(); // debug upper bound
            return pos.X > 0 && pos.X < fieldSize && pos.Y > 0 && pos.Y < fieldSize;
        }
    }


    internal class Snake
    {

        private readonly Queue<SnakeTile> snakeTiles;
        private int length;

        public Snake(int length, Vector2 center)
        {
            this.snakeTiles = new Queue<SnakeTile>();
            this.snakeTiles.Enqueue(new SnakeTile(center, Program.directions[Random.Shared.Next(4)]));
            this.length = length;
        }

        public SnakeTile GetHead()
        {
            return snakeTiles.Last();
        }

        public int SetLength(int newLength)
        {
            return this.length = newLength;
        }

        public SnakeTile Add(SnakeTile value) 
        {
            snakeTiles.Enqueue(value);
            if (snakeTiles.Count() == length + 1)
            {
                return snakeTiles.Dequeue();
            }
            return null;
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

        public void SetDirection(Vector2 direction)
        {
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

        public SnakeTile GetNext()
        {
            return new SnakeTile(this.position + this.direction, this.direction);
        }
    }

}