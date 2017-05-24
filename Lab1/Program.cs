using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApplication
{
    internal class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool H { get; set; }
        public bool V { get; set; }
    }

    enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("small-test.out.txt"))
                using (StreamReader sr = new StreamReader("small-test.in.txt"))
                {
                    int n = int.Parse(sr.ReadLine());

                    for (int i = 0; i < n; i++)
                    {
                        sw.WriteLine("Case #{0}:", i + 1);

                        string[] input = sr.ReadLine().Split(' ');

                        int gX = 0;
                        int gY = -1;
                        var points = new List<Point>();
                        var direct = Direction.Down;
                        Point last = null;
                        Direction lastDirect = 0;

                        for (int k = 0; k <= 1; k++)
                        {
                            var buf = input[k];

                            if (k == 1)
                            {
                                buf = "RR" + buf;
                            }

                            for (int j = 0; j < buf.Length; j++)
                            {
                                if (buf[j] == 'W')
                                {
                                    if (direct == Direction.Left)
                                    {
                                        var now = points.FirstOrDefault(q => q.X == gX && q.Y == gY);
                                        if (now != null)
                                        {
                                            now.V = true;
                                        }
                                        gX--;

                                        var checkPoint = points.FirstOrDefault(q => q.X == gX && q.Y == gY);
                                        if (checkPoint == null)
                                        {
                                            var newP = new Point();
                                            newP.X = gX;
                                            newP.Y = gY;
                                            points.Add(newP);
                                        }
                                    }
                                    if (direct == Direction.Right)
                                    {
                                        gX++;

                                        var checkPoint = points.FirstOrDefault(q => q.X == gX && q.Y == gY);
                                        if (checkPoint == null)
                                        {
                                            var newP = new Point();
                                            newP.X = gX;
                                            newP.Y = gY;
                                            newP.V = true;
                                            points.Add(newP);
                                        }
                                        else
                                        {
                                            checkPoint.V = true;
                                        }
                                    }
                                    if (direct == Direction.Up)
                                    {
                                        var now = points.FirstOrDefault(q => q.X == gX && q.Y == gY);
                                        if (now != null)
                                        {
                                            now.H = true;
                                        }
                                        gY--;

                                        var checkPoint = points.FirstOrDefault(q => q.X == gX && q.Y == gY);
                                        if (checkPoint == null)
                                        {
                                            var newP = new Point();
                                            newP.X = gX;
                                            newP.Y = gY;
                                            points.Add(newP);
                                        }

                                    }
                                    if (direct == Direction.Down)
                                    {
                                        gY++;

                                        var checkPoint = points.FirstOrDefault(q => q.X == gX && q.Y == gY);
                                        if (checkPoint == null)
                                        {
                                            var newP = new Point();
                                            newP.X = gX;
                                            newP.Y = gY;
                                            newP.H = true;
                                            points.Add(newP);
                                        }
                                        else
                                        {
                                            checkPoint.H = true;
                                        }
                                    }

                                }
                                else if (buf[j] == 'L')
                                {
                                    if (direct == Direction.Up)
                                        direct = Direction.Left;
                                    else
                                        direct--;
                                }
                                else
                                {
                                    if (direct == Direction.Left)
                                        direct = Direction.Up;
                                    else
                                        direct++;
                                }

                                if (j == buf.Length - 1)
                                {
                                    points.RemoveAt(points.Count - 1);

                                    if (k == 0)
                                    {
                                        last = points.Last();
                                        lastDirect = direct;
                                    }
                                }
                            }
                        }

                        var min = points.FirstOrDefault(x => x.X == points.Min(y => y.X) && x.Y == points.Min(y => y.Y));
                        var nowPoint = min;

                        while (true)
                        {
                            bool north = false, south = false, west = false, east = false;
                            if (nowPoint.V == true)
                            {
                                west = true;
                            }
                            if (nowPoint.H == true)
                            {
                                north = true;
                            }
                            var eastPoint = points.FirstOrDefault(x => x.X == nowPoint.X + 1 && x.Y == nowPoint.Y);
                            if (eastPoint != null && eastPoint.V == true)
                            {
                                east = true;
                            }
                            if (eastPoint == null && nowPoint == last && lastDirect == Direction.Right)
                            {
                                east = true;
                            }
                            var southPoint = points.FirstOrDefault(x => x.X == nowPoint.X && x.Y == nowPoint.Y + 1);
                            if (southPoint != null && southPoint.H == true)
                            {
                                south = true;
                            }
                            if (southPoint == null && nowPoint == last && lastDirect == Direction.Down)
                            {
                                south = true;
                            }

                            sw.Write(getSymbol(north, south, west, east).ToString());

                            var next = points.FirstOrDefault(x => x.X == nowPoint.X + 1 && x.Y == nowPoint.Y);
                            if (next == null)
                            {
                                sw.WriteLine();
                                next = points.FirstOrDefault(x => x.X == min.X && x.Y == nowPoint.Y + 1);
                                if (next == null)
                                {
                                    break;
                                }
                            }

                            nowPoint = next;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка:");
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Выходной файл был успешно создан.");
            Console.Read();
        }

        public static char getSymbol(bool north, bool south, bool west, bool east)
        {
            if (east == true)
            {
                if (west == true)
                {
                    if (south == true)
                    {
                        if (north == true)
                        {
                            return 'f';
                        }
                        else
                        {
                            return 'e';
                        }
                    }
                    else
                    {
                        if (north == true)
                        {
                            return 'd';
                        }
                        else
                        {
                            return 'c';
                        }
                    }
                }
                else
                {
                    if (south == true)
                    {
                        if (north == true)
                        {
                            return 'b';
                        }
                        else
                        {
                            return 'a';
                        }
                    }
                    else
                    {
                        if (north == true)
                        {
                            return '9';
                        }
                        else
                        {
                            return '8';
                        }
                    }
                }
            }
            else
            {
                if (west == true)
                {
                    if (south == true)
                    {
                        if (north == true)
                        {
                            return '7';
                        }
                        else
                        {
                            return '6';
                        }
                    }
                    else
                    {
                        if (north == true)
                        {
                            return '5';
                        }
                        else
                        {
                            return '4';
                        }
                    }
                }
                else
                {
                    if (south == true)
                    {
                        if (north == true)
                        {
                            return '3';
                        }
                        else
                        {
                            return '2';
                        }
                    }
                    else
                    {
                        if (north == true)
                        {
                            return '1';
                        }
                        else
                        {
                            return '0';
                        }
                    }
                }
            }
        }
    }
}
