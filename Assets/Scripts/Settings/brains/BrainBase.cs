using System;
using System.Globalization;
using System.IO;
using System.Linq;
using CoinPackage.Debugging;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex32;

namespace Settings.brains
{
    public static class BrainBase {
        public static Matrix<double> PredatorMatrix;
        public static Matrix<double> PreyMatrix;

        static BrainBase()
        {
            CreatePreyMatrix();
            CreatePredatorMatrix();
        }

        static void CreatePreyMatrix()
        {
            var matrix = new double[,]
            {
                { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 4f, 0f, 0f, 0.1f, 0f, -1f, 1f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f },
                { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, -4f, 0f, 0f, 0f, 0f, 0.5f, -0.5f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f },
                { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f },
                { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f },
                { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.5f, 0f, -0.1f, 0.1f, 0.5f, -0.5f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f },
                { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, -0.4f, 0.2f, -0.2f, -0.7f, 0.7f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f },
                { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.5f, -0.1f, 0.1f, 0.5f, -0.5f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f },
                { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, -0.4f, 0.2f, -0.2f, -0.5f, 0.5f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f },
                { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.4f, 4f, -1.5f, 0f, 0f, -2.2f, 2.2f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f },
                { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, -1f, 1.5f, 0.2f, -0.2f, 1.5f, -1.5f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f },
                { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, -0.2f, 0f, -0.3f, 0.3f, 1.1f, -1.1f, 0f, 0f, 0f, 0f, 0f, 0f, 2.6f, 0f },
                { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.2f, 0f, 0.8, -1f, -1.1f, 1.1f, 0f, 0f, 0f, 0f, 0f, 0f, -4f, 0f },
                { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, -0.4f, 0.4f, 0.5f, -0.5f, 0f, 0f, 0f, -4.8f, 0f, 0f, 0f, 1.9f },
                { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.5f, 0.3f, -0.3f, -0.8f, 0.8f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, -4f },
                { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f },
                { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.5f, 0f, 0f, -0.4f, 0f, 0f, 0f, 3.5f, 0f, -0.8f, -1f, 0.3f, -1f, -1f, -1f },
                { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.3f, 0f, 0f, 0f, 0f, 0f, -0.8f, 0f, 2.1f, -0.7f, 0.7f, -0.5f, 4f, -1.8f },
                { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.2f, 0f, 0f, 0f, 0f, -0.2f, 0f, 0f, 1.5f, 0.5f, -0.3f, -0.4f, 3f },
                { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.1f, 0f, 0f, 0f, -0.1f, 0f, 0.5f, 0.3f, 1.5f, -0.2f, -0.3f, -0.2f },
                { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.1f, 0f, 0f, 0f, 0f, -0.5f, -0.3f, -1.2f, 0.2f, 0.3f, 0.2f },
                { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, -0.1f, 0f, -0.8f, -0.2f, -2f, 1.5f, 0.8f, 0.7f },
                { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.4f, 0f, 1f, 0.2f, 2f, -1.2f, -0.7f, -0.7f },
                { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f },
                { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f },
                { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f },
                { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f },
                { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f },
                { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.2f, 0f, 0f },
                { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f },
                { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }
            };

            PreyMatrix = Matrix<double>.Build.Dense(30, 30, matrix.Cast<double>().ToArray());
        }

        static void CreatePredatorMatrix() {
            var matrix = new double[,] {
                {
                    0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f
                }, {
                    0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f
                }, {
                    0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.7f, 0f, 0f, 0f, -0.1f, 0f, 0.5f, -0.5f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f
                }, {
                    0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, -0.5f, 0f, 0.7f, 0.1f, 0.4f, -0.4f, -0.5f, 0.5f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f
                }, {
                    0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, -2.6f, 0f, 0.7f, 0f, -0.1f, 0.1f, 0.5f, -0.5f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f
                }, {
                    0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.8f, 0f, -0.2f, 0.1f, 0.2f, -0.2f, -0.6f, 0.6f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f
                }, {
                    0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.7f, 0f, 0f, 0.4f, -0.4f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f
                }, {
                    0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, -0.5f, 0.3f, -0.3f, -4f, 0.4f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f
                }, {
                    0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 3.5f, 0f, 6.0f, -1.2f, 0f, 0.2f, -1.5f, 1.5f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f
                }, {
                    0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, -2f, 0f, -0.3f, 1.4f, 0.3f, -0.3f, 1f, -1f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f
                }, {
                    0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, -2.5f, 0f, 0.3f, -0.2f, -0.3f, 0.3f, 1f, -1f, 0f, 0f, 0f, 0f, 0f, 0f, 10f, 0f
                }, {
                    0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 1.7f, 0f, 0f, 0.2f, 0.7f, -1f, -1f, 1f, 0f, 0f, 0f, 0f, 0f, 0f, -5f, 0f
                }, {
                    0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, -0.3f, 0f, 0f, 0f, -0.4f, 0.4f, 0.8f, -0.8f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 2f
                }, {
                    0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.3f, 0f, 0f, 0.5f, 0.3f, -0.3f, -8f, 0.8f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, -5f
                }, {
                    0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.2f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 1.5f, -0.5f, -0.4f, 0.3f, -0.4f, 0f, -0.4f
                }, {
                    0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f
                }, {
                    0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.3f, 0f, -0.2f, 0f, 0f, 0f, 0f, 1.5f, 3.1f, -1.2f, 0.1f, -0.4f, 3.5f, -0.8f
                }, {
                    0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.2f, 0f, 0f, 0f, 0f, 0f, -0.8f, -0.8f, 4.5f, 0.3f, -0.5f, -0.6f, 3f
                }, {
                    0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.1f, 0f, 0f, 0f, 0f, 0.3f, 0.6f, 0.3f, 1.3f, -0.4f, -0.3f, -0.2f
                }, {
                    0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.1f, 0f, 0f, 0f, -0.3f, -0.3f, -0.3f, -1.5f, 0.4f, 0.3f, 0.2f
                }, {
                    0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, -0.8f, -0.8f, -0.2f, -1.8f, 1f, 0.8f, 0.8f
                }, {
                    0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 1f, 0.8f, 0.2f, 2f, -1f, -0.6f, -0.8f
                }, {
                    0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f
                }, {
                    0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f
                }, {
                    0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f
                }, {
                    0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f
                }, {
                    0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f
                }, {
                    0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.2f, 0f, 0f
                }, {
                    0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f
                }, {
                    0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f
                }
            };
            // var matrix = new double[31, 31];
            // for (var i = 0; i < 31; i++)
            // {
            //     for (var j = 0; j < 31; j++)
            //     {
            //         if (double.IsNaN(matrix[i, j]))
            //         {
            //             matrix[i, j] = 0.0;
            //         }
            //     }
            // }
            // matrix[2, 14] = 0.7;
            // matrix[3, 14] = -0.5; matrix[3, 16] = 0.7; matrix[3, 17] = 0.1; matrix[3, 18] = 0.4; matrix[3, 19] = -0.4; matrix[3, 20] = -0.5; matrix[3, 21] = 0.5;
            // matrix[4, 14] = -0.5; matrix[4, 16] = 0.7; matrix[4, 18] = -0.1; matrix[4, 19] = 0.1; matrix[4, 20] = 0.5; matrix[4, 21] = -0.5;
            // matrix[5, 14] = 0.8; matrix[5, 16] = -0.2; matrix[5, 17] = 0.1; matrix[5, 18] = 0.2; matrix[5, 19] = -0.2; matrix[5, 20] = -0.6; matrix[5, 21] = 0.6;
            // matrix[6, 17] = 0.7; matrix[6, 20] = 0.4; matrix[6, 21] = -0.4;
            // matrix[7, 17] = -0.5; matrix[7, 18] = 0.3; matrix[7, 19] = -0.3; matrix[7, 20] = -4.0; matrix[7, 21] = 0.4;
            // matrix[8, 14] = 3.5; matrix[8, 16] = 5.0; matrix[8, 17] = -1.2; matrix[8, 19] = 0.2; matrix[8, 20] = -1.5; matrix[8, 21] = 1.5;
            // matrix[9, 14] = -2.0; matrix[9, 15] = -3.0; matrix[9, 17] = 1.4; matrix[9, 18] = 0.3; matrix[9, 19] = -0.3; matrix[9, 20] = 1.0; matrix[9, 21] = -1.0;
            // matrix[10, 14] = -1.5; matrix[10, 16] = 0.3; matrix[10, 17] = -0.2; matrix[10, 18] = -0.3; matrix[10, 19] = 0.3; matrix[10, 20] = 1.0; matrix[10, 21] = -1.0; matrix[10, 27] = 4.0;
            // matrix[11, 14] = 1.7; matrix[11, 16] = 0.0; matrix[11, 17] = 0.2; matrix[11, 18] = 1.0; matrix[11, 19] = -1.0; matrix[11, 20] = -1.0; matrix[11, 21] = 1.0; matrix[11, 27] = -5.0;
            // matrix[12, 17] = -0.3; matrix[12, 18] = 0.4; matrix[12, 20] = 0.8; matrix[12, 21] = -8.0; matrix[12, 28] = 2.0;
            // matrix[13, 14] = 0.3; matrix[13, 17] = 0.5; matrix[13, 18] = 0.3; matrix[13, 19] = -3.0; matrix[13, 20] = -8.0; matrix[13, 21] = 0.8; matrix[13, 28] = -5.0;
            // matrix[14, 14] = 0.2; matrix[14, 22] = 1.5; matrix[14, 23] = -0.2; matrix[14, 24] = -4.0; matrix[14, 25] = 0.3; matrix[14, 26] = -0.4; matrix[14, 28] = -0.4;
            // matrix[16, 16] = 0.3; matrix[16, 22] = 1.5; matrix[16, 23] = 2.5; matrix[16, 24] = -1.2; matrix[16, 25] = 0.3; matrix[16, 26] = -0.4; matrix[16, 27] = 3.5; matrix[16, 28] = -0.8;
            // matrix[17, 17] = 0.2; matrix[17, 23] = -0.8; matrix[17, 24] = -8.0; matrix[17, 25] = 1.5; matrix[17, 26] = 0.3; matrix[17, 27] = -0.5; matrix[17, 28] = -0.6; matrix[17, 29] = 3.0;
            // matrix[18, 18] = 0.1; matrix[18, 23] = 0.3; matrix[18, 24] = 0.3; matrix[18, 25] = 0.3; matrix[18, 26] = 1.5; matrix[18, 27] = -0.4; matrix[18, 28] = -0.3; matrix[18, 29] = -0.2;
            // matrix[19, 19] = 0.1; matrix[19, 24] = -0.3; matrix[19, 25] = -0.3; matrix[19, 26] = -0.3; matrix[19, 27] = -1.5; matrix[19, 28] = 0.4; matrix[19, 29] = 0.3; matrix[19, 30] = 0.2;
            // matrix[20, 23] = -0.8; matrix[20, 24] = -0.8; matrix[20, 25] = -0.2; matrix[20, 26] = -1.8; matrix[20, 27] = 1.0; matrix[20, 28] = 0.8; matrix[20, 29] = 0.8;
            // matrix[21, 23] = 1.0; matrix[21, 24] = 0.8; matrix[21, 25] = 0.2; matrix[21, 26] = 2.0; matrix[21, 27] = -1.0; matrix[21, 28] = -0.6; matrix[21, 29] = -8.0;
            
            PredatorMatrix = Matrix<double>.Build.Dense(30, 30, matrix.Cast<double>().ToArray());
        }
    }
}