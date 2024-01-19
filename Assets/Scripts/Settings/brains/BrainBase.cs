using System;
using System.IO;

namespace Settings.brains
{
    public class BrainBase
    {
        public double[,] matrix;

        public BrainBase()
        {
            matrix = new double[30, 30];
            for (int i = 0; i < 31; i++)
            {
                for (int j = 0; j < 31; j++)
                {
                    if (double.IsNaN(matrix[i, j]))
                    {
                        matrix[i, j] = 0.0;
                    }
                }
            }
        }
        
        public double GetValue(int row, int column)
        {
            if (row >= 0 && row < 31 && column >= 0 && column < 31)
            {
                var value = matrix[row, column];
                if (double.IsNaN(value))
                {
                    throw new InvalidDataException("Jakims cudem trafiles na NANa i to jest koniec swiata teraz");
                }
                return value;
            }
            else
            {
                throw new IndexOutOfRangeException("Indeksy są poza zakresem tablicy");
            }
        }
    }
}