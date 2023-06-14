using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplexMethod;

namespace testSimplex
{
    class Program
    {
        static void Main(string[] args)
        {
            Fraction[,] table = { {new Fraction(12), new Fraction(3), new Fraction(2)},
                                {new Fraction(8), new Fraction(1),  new Fraction(2)},
                                {new Fraction(16),  new Fraction(5),  new Fraction(0)},
                                {new Fraction(3),  new Fraction(0),  new Fraction(1)},
                                { new Fraction(0), new Fraction(-3), new Fraction(-4)} };

            Fraction[] result = new Fraction[2];
            Fraction[,] table_result;
            Simplex S = new Simplex(table);
            table_result = S.Calculate(result);
            Fraction fraction = new Fraction(1, 2);
            Fraction fraction1 = new Fraction(1, 4);
            Fraction fraction2 = fraction / fraction1;
            Console.WriteLine("Решенная симплекс-таблица:");
            for (int i = 0; i < table_result.GetLength(0); i++)
            {
                for (int j = 0; j < table_result.GetLength(1); j++)
                    Console.Write(table_result[i, j].ToString() + " ");
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine("Решение:");
            Console.WriteLine("X[1] = " + result[0].ToString());
            Console.WriteLine("X[2] = " + result[1].ToString());
            Console.ReadLine();
        }
    }
}