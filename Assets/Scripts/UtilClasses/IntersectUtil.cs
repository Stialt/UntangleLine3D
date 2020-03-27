using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace UntangleLines
{
    class IntersectUtil
    {

        public static Boolean onSegment(myPoint p, myPoint q, myPoint r)
        {
            if (q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) &&
                q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y))
                return true;

            return false;
        }

        public static int orientation(myPoint p, myPoint q, myPoint r)
        {
            // See https://www.geeksforgeeks.org/orientation-3-ordered-points/ 
            // for details of below formula. 
            float val = (q.Y - p.Y) * (r.X - q.X) -
                    (q.X - p.X) * (r.Y - q.Y);

            if (Math.Abs(val - 0) < 0.0001) return 0; // colinear 

            return (val > 0) ? 1 : 2; // clock or counterclock wise 
        }

        public static Boolean doIntersect(myPoint p1, myPoint q1, myPoint p2, myPoint q2)
        {
            // Find the four orientations needed for general and 
            // special cases 
            int o1 = orientation(p1, q1, p2);
            int o2 = orientation(p1, q1, q2);
            int o3 = orientation(p2, q2, p1);
            int o4 = orientation(p2, q2, q1);

            // General case 
            if (o1 != o2 && o3 != o4)
                return true;

            // Special Cases 
            // p1, q1 and p2 are colinear and p2 lies on segment p1q1 
            if (o1 == 0 && onSegment(p1, p2, q1)) return true;

            // p1, q1 and q2 are colinear and q2 lies on segment p1q1 
            if (o2 == 0 && onSegment(p1, q2, q1)) return true;

            // p2, q2 and p1 are colinear and p1 lies on segment p2q2 
            if (o3 == 0 && onSegment(p2, p1, q2)) return true;

            // p2, q2 and q1 are colinear and q1 lies on segment p2q2 
            if (o4 == 0 && onSegment(p2, q1, q2)) return true;

            return false; // Doesn't fall in any of the above cases 
        }

        public static bool globalIntersect(int[,] Edges, myPoint[] points, int N)
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (Edges[i, j] == 1)
                    {
                        for (int k = 0; k < N; k++)
                        {
                            for (int l = 0; l < N; l++)
                            {
                                //If not same edge
                                if (k == i && l == j || k == j && l == i) continue;

                                //If share point
                                if (i == k || i == l || j == k || j == l) continue;

                                //If edge exists
                                if (Edges[k, l] == 1)
                                {
                                    //Check intersection
                                    if (doIntersect(points[i], points[j], points[k], points[l]))
                                    {
                                        //Console.WriteLine("Intersection between: [" + i + "," + j + "] and [" + k + "," + l + "]");
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        public static int[,] getIntersectMap(int[,] Edges, myPoint[] points, int N)
        {
            int[,] Map = new int[N, N];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (Edges[i, j] == 1)
                    {
                        for (int k = 0; k < N; k++)
                        {
                            for (int l = 0; l < N; l++)
                            {
                                //If not same edge
                                if (k == i && l == j || k == j && l == i) continue;

                                //If share point
                                if (i == k || i == l || j == k || j == l) continue;

                                //If edge exists
                                if (Edges[k, l] == 1)
                                {
                                    //Check intersection
                                    if (doIntersect(points[i], points[j], points[k], points[l]))
                                    {
                                        Map[i, j] = Map[k, l] = Map[j, i] = Map[l, k] = 1;
                                        
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return Map;
        }

        public static int distSqr(Point x, Point y)
        {
            return (int)(Math.Pow(x.X - y.X, 2) + Math.Pow(x.Y - y.Y, 2));
        }
    }
}
