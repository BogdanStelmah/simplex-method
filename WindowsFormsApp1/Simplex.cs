using System;
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    internal class Simplex
    {
        Fraction[,] simplexTable;
        int m, n;
        List<Fraction[,]> tables = new List<Fraction[,]>();
        List<int> basis;

        public Simplex(Fraction[,] source)
        {
            m = source.GetLength(0);
            n = source.GetLength(1);
            simplexTable = new Fraction[m, n + m - 1];
            basis = new List<int>();

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < simplexTable.GetLength(1); j++)
                {
                    if (j < n)
                        simplexTable[i, j] = source[i, j];
                    else
                        simplexTable[i, j] = new Fraction(0);
                }
                // Exhibit a ratio of 1 to a basic variable in row
                if ((n + i) < simplexTable.GetLength(1))
                {
                    simplexTable[i, n + i] = new Fraction(1);
                    basis.Add(n + i);
                }
            }

            n = simplexTable.GetLength(1);
        }

        public List<Fraction[,]> Calculate(Fraction[] result)
        {
            int mainCol, mainRow;
            while (!IsEnd())
            {
                mainCol = FindMainColumn();
                mainRow = FindMainRow(mainCol);
                basis[mainRow] = mainCol;

                var newTable = new Fraction[m, n];

                for (int j = 0; j < n; j++)
                    newTable[mainRow, j] = (simplexTable[mainRow, j] / simplexTable[mainRow, mainCol]).Abbreviation();

                for (int i = 0; i < m; i++)
                {
                    if (i == mainRow)
                        continue;

                    for (int j = 0; j < n; j++)
                        newTable[i, j] = (simplexTable[i, j] - simplexTable[i, mainCol] * newTable[mainRow, j]).Abbreviation();
                }
                tables.Add(newTable);
                simplexTable = newTable;
            }

            for (int i = 0; i < result.Length; i++)
            {
                int k = basis.IndexOf(i + 1);
                if (k != -1)
                    result[i] = simplexTable[k, 0].Abbreviation();
                else
                    result[i] = new Fraction(0);
            }
            return tables;
        }

        public Fraction GetFraction()
        {
            var fraction = simplexTable[simplexTable.GetLength(0) - 1, 0].Abbreviation();
            if ((double)fraction != 0)
                return fraction;

            throw new Exception("Сталася помилка під час розрахунку. Перевірте, будь ласка, введені дані");
        }

        private bool IsEnd()
        {
            for (int j = 1; j < n; j++)
                if (simplexTable[m - 1, j] < new Fraction(0))
                    return false;

            return true;
        }

        private int FindMainColumn()
        {
            int mainColumn = 1;

            for (int j = 2; j < n; j++)
                if (simplexTable[m - 1, j] < simplexTable[m - 1, mainColumn])
                    mainColumn = j;

            return mainColumn;
        }

        private int FindMainRow(int mainColumn)
        {
            int mainRow = 0;

            for (int i = 0; i < m - 1; i++)
                if (simplexTable[i, mainColumn] > new Fraction(0))
                {
                    mainRow = i;
                    break;
                }

            for (int i = mainRow + 1; i < m - 1; i++)
                if (simplexTable[i, mainColumn] > new Fraction(0) && (simplexTable[i, 0] / simplexTable[i, mainColumn] < simplexTable[mainRow, 0] / simplexTable[mainRow, mainColumn]))
                    mainRow = i;

            return mainRow;
        }
    }
}
