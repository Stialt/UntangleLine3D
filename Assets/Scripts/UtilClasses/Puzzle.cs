using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace UntangleLines
{
    class Puzzle
    {
        //-------------------------------PUZZLE GENERATION------------------------------

        public bool isDebug = false;
        public bool isShowInit = false;
        public int N = 5;

        public myPoint[] points;
        public int[,] Edges;
        public myPoint[] randomPoints;
        public int[,] EdgesIntersectionMap;

        public int midX = 250, midY = 350;
        public int puzzleSize = 400;

        //Global Random Generator
        Random random = new Random();

        public Puzzle()
        {
            points = new myPoint[N];
            Edges = new int[N, N];
            randomPoints = new myPoint[N];
            EdgesIntersectionMap = new int[N, N];
        }

        public int[,] getIntersectMap()
        {
            EdgesIntersectionMap = IntersectUtil.getIntersectMap(Edges, randomPoints, N);
            return EdgesIntersectionMap;
        }

        public void createPuzzle(int N)
        {
            this.N = N;
            points = initPoints(puzzleSize, N, new Point(midX, midY));

            if (isDebug)
            {
                Console.WriteLine("Points are: ");
                for (int i = 0; i < N; i++)
                {
                    Console.WriteLine(points[i].ID + ": " + points[i].X + ", " + points[i].Y);
                }
            }

            Edges = randomEdges(N, points);

            randomPoints = randomizePoints(points);

            while (!(IntersectUtil.globalIntersect(Edges, randomPoints, N)))
                randomPoints = randomizePoints(points);
        }

        public void createPuzzle(int N, int x, int y, int size)
        {
            midX = x; midY = y; puzzleSize = size;
            createPuzzle(N);
        }

        //Swap Points
        public myPoint[] randomizePoints(myPoint[] points)
        {
            myPoint[] randomPoints = new myPoint[N];
            for (int i = 0; i < N; i++)
            {
                randomPoints[i] = new myPoint(points[i]);
            }

            List<int> inds = new List<int>();
            for (int i = 0; i < N; i++)
                inds.Add(i);

            List<int> randInds = new List<int>();
            while (inds.Count > 0)
            {
                int ind = random.Next(inds.Count);
                randInds.Add(inds[ind]);
                inds.RemoveAt(ind);
            }

            if (isDebug)
            {
                Console.WriteLine("Points random indeces:");
                for (int i = 0; i < randInds.Count; i++)
                {
                    Console.Write(" " + randInds[i]);
                }
                Console.WriteLine();
            }

            for (int i = 0; i < N; i++)
            {
                //randomPoints[i] = points[randInds[i]];
                int shift = 500;
                if (!isShowInit)
                    shift = 0;
                randomPoints[i].X = points[randInds[i]].X + shift;
                randomPoints[i].Y = points[randInds[i]].Y;
            }

            return randomPoints;
        }

        public int[,] randomEdges(int N, myPoint[] points)
        {

            //Console.WriteLine("Random Edges initialization");

            int[,] Edges = new int[N, N];
            List<int> edges = new List<int>();
            for (int i = 0; i < N * N; i++)
                edges.Add(i);

            List<int> randOrder = new List<int>();

            if (isDebug)
                Console.WriteLine("Edges count is " + edges.Count);

            for (int i = 0; i < N * N; i++)
            {
                //1. Take one random index
                int ind = random.Next(edges.Count);

                if (isDebug)
                {
                    Console.Write(" " + edges[ind]);
                }

                //2. Add to randOrder
                randOrder.Add(edges[ind]);

                //3. Remove from edges
                edges.RemoveAt(ind);

            }

            if (isDebug)
                Console.WriteLine("\nEdges count is " + edges.Count);

            for (int i = 0; i < N; i++)
            {
                //Edges[i, (i + 1) % N] = Edges[(i + 1) % N, i] = 1;
            }

            if (isDebug)
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        Console.Write(" " + Edges[i, j]);
                    }
                    Console.WriteLine();
                }

            //For each edge in randOrder:
            for (int i = 0; i < randOrder.Count; i++)
            {
                //1. Convert to edge indices in Matrix
                int x = randOrder[i] / N;
                int y = randOrder[i] % N;

                if (x == y) continue;

                if (isDebug)
                {
                    //Console.WriteLine("Edge #" + randOrder[i] + ": (i,j)=(" + x + "," + y + ")");
                }


                //if (Edges[x, y] == 1)
                //    continue;

                //2. Check if will create intersection
                Edges[x, y] = Edges[y, x] = 1;

                //3. If yes: skip
                if (IntersectUtil.globalIntersect(Edges, points, N))
                {
                    Edges[x, y] = Edges[y, x] = 0;
                    //continue;

                    //if (isDebug) Console.WriteLine("BAD Creates Intersection\n");
                }
                else
                {
                    //if (isDebug) Console.WriteLine("GOOD No Intersection\n");
                }


                //4. If no: randomize between 0 and 1
                /*else
                {
                    Edges[x, y] = random.Next(2);
                }*/

            }

            if (isDebug)
            {
                Console.WriteLine("\n\nEdges are: ");
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        Console.Write(" " + Edges[i, j]);
                    }
                    Console.WriteLine();
                }
            }

            return Edges;
        }

        //Initialize points
        private myPoint[] initPoints(int size, int N, Point center)
        {
            myPoint[] points = new myPoint[N];
            points[0] = new myPoint(center.X, center.Y - size / 2, 0);

            for (int i = 0; i < N - 1; i++)
            {
                points[i + 1] = rotate_point(center.X, center.Y, 2 * Math.PI / N, points[i]);
                points[i + 1].ID = i + 1;
            }

            return points;
        }

        //Rotate Point aroudn center
        myPoint rotate_point(int cx, int cy, double angle, myPoint point)
        {
            double s = Math.Sin(angle);
            double c = Math.Cos(angle);


            myPoint p = new myPoint(point);

            // translate point back to origin:
            p.X -= cx;
            p.Y -= cy;

            // rotate point
            int xnew = (int)Math.Round(p.X * c - p.Y * s);
            int ynew = (int)Math.Round(p.X * s + p.Y * c);

            // translate point back:
            p.X = xnew + cx;
            p.Y = ynew + cy;
            return p;
        }



    }
}
