using System;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using Microsoft;


namespace ConsoleApp12
{
    class Program
    {
        static int[,] SubtractMatrix(int[,] matrix_a, int[,] matrix_b, int size)
        {
            int[,] c = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    c[i, j] = matrix_a[i, j] - matrix_b[i, j];
                }
            }
            return c;
        }
        static int[,] AddMatrix(int[,] matrix_a, int[,] matrix_b, int size)
        {
            int[,] c = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    c[i, j] = matrix_a[i, j] + matrix_b[i, j];
                }
            }
            return c;
        }
        static int[,] Strassen_Multiplication(int[,] matrix_a, int[,] matrix_b, int size)
        {
            int[,] c = new int[size, size];
            if (size==2)
            {
                int p = (matrix_a[0, 0]+matrix_a[1, 1])*(matrix_b[0, 0]+matrix_b[1, 1]);
                int q = (matrix_a[1, 0]+matrix_a[1, 1])*matrix_b[0, 0];
                int r = matrix_a[0, 0]*(matrix_b[0, 1]-matrix_b[1, 1]);
                int s = matrix_a[1, 1]*(matrix_b[1, 0]-matrix_b[0, 0]);
                int t = (matrix_a[0, 0]+matrix_a[0, 1])*matrix_b[1, 1];
                int u = (matrix_a[1, 0]-matrix_a[0, 0])*(matrix_b[0, 0]+matrix_b[0, 1]);
                int v = (matrix_a[0, 1]-matrix_a[1, 1])*(matrix_b[1, 0]+matrix_b[1, 1]);
                c[0, 0]=p+s-t+v;
                c[0, 1]=r+t;
                c[1, 0]=q+s;
                c[1, 1]=p+r-q+u;
                return c;
            }
            else
            {
                // recursive step
                int newSize = size / 2;
                int[,] a11 = new int[newSize, newSize];
                int[,] a12 = new int[newSize, newSize];
                int[,] a21 = new int[newSize, newSize];
                int[,] a22 = new int[newSize, newSize];
                int[,] b11 = new int[newSize, newSize];
                int[,] b12 = new int[newSize, newSize];
                int[,] b21 = new int[newSize, newSize];
                int[,] b22 = new int[newSize, newSize];

                // dividing the matrices into sub-matrices:
                for (int i = 0; i < newSize; i++)
                {
                    for (int j = 0; j < newSize; j++)
                    {
                        a11[i, j] = matrix_a[i, j];
                        a12[i, j] = matrix_a[i, j + newSize];
                        a21[i, j] = matrix_a[i + newSize, j];
                        a22[i, j] = matrix_a[i + newSize, j + newSize];
                        b11[i, j] = matrix_b[i, j];
                        b12[i, j] = matrix_b[i, j + newSize];
                        b21[i, j] = matrix_b[i + newSize, j];
                        b22[i, j] = matrix_b[i + newSize, j + newSize];
                    }
                }
                int[,] pp = Strassen_Multiplication(AddMatrix(a11, a22, newSize), AddMatrix(b11, b22, newSize), newSize);
                int[,] qq = Strassen_Multiplication(AddMatrix(a21, a22, newSize), b11, newSize);
                int[,] rr = Strassen_Multiplication(a11, SubtractMatrix(b12, b22, newSize), newSize);
                int[,] ss = Strassen_Multiplication(a22, SubtractMatrix(b21, b11, newSize), newSize);
                int[,] tt = Strassen_Multiplication(AddMatrix(a11, a12, newSize), b22, newSize);
                int[,] uu = Strassen_Multiplication(SubtractMatrix(a21, a11, newSize), AddMatrix(b11, b12, newSize), newSize);
                int[,] vv = Strassen_Multiplication(SubtractMatrix(a12, a22, newSize), AddMatrix(b21, b22, newSize), newSize);
                int[,] c11 = AddMatrix(SubtractMatrix(AddMatrix(pp, ss, newSize), tt, newSize), vv, newSize);
                int[,] c12 = AddMatrix(rr, tt, newSize);
                int[,] c21 = AddMatrix(qq, ss, newSize);
                int[,] c22 = AddMatrix(SubtractMatrix(AddMatrix(pp, rr, newSize), qq, newSize), uu, newSize);
                for (int i = 0; i < newSize; i++)
                {
                    for (int j = 0; j < newSize; j++)
                    {
                        c[i, j] = c11[i, j];
                        c[i, j + newSize] = c12[i, j];
                        c[i + newSize, j] = c21[i, j];
                        c[i + newSize, j + newSize] = c22[i, j];
                    }
                }
                return c;
            }
        }
        static void Main(string[] args)
        {
            int x, y, Min_Pow = 0;
            x=Convert.ToInt32(Console.ReadLine());
            y=Convert.ToInt32(Console.ReadLine());
            for (int i = 2; i<2147483647; i*=2)
            {
                if (Math.Max(x, y)<=i)
                {
                    Min_Pow = i;
                    break;
                }
            }
            int[,] matrix_a = new int[Min_Pow, Min_Pow];
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    matrix_a[i, j] = Convert.ToInt32(Console.ReadLine());
                }
            }
            x=Convert.ToInt32(Console.ReadLine());
            y=Convert.ToInt32(Console.ReadLine());
            for (int i = 2; i<2147483647; i*=2)
            {
                if (Math.Max(x, y)<=i)
                {
                    Min_Pow = i;
                    break;
                }
            }
            int[,] matrix_b = new int[Min_Pow, Min_Pow];
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    matrix_b[i, j] = Convert.ToInt32(Console.ReadLine());
                }
            }
            int[,] outcome = Strassen_Multiplication(matrix_a, matrix_b, Min_Pow);
            for (int i = 0; i<outcome.GetLength(0); i++)
            {
                for (int j = 0; j<outcome.GetLength(1); j++)
                {
                    Console.Write(outcome[i, j]+" ");
                }
                Console.Write("\n");
            }
        }
    }
}
