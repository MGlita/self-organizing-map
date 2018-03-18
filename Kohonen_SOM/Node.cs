using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kohonen_SOM
{
    class Node
    {
        public int x, y;
        public double[] weights;
        Random rnd = new Random();

        public Node(int x, int y, int numWeights)
        {
            this.x = x;
            this.y = y;
            weights = new double[numWeights];

            //Weights init
            for (int i = 0; i < numWeights; i++)
            {
                System.Threading.Thread.Sleep(10);
                weights[i] = rnd.NextDouble();
            }
        }

        //The Euclidean distance
        public double GetDistance(double[] inputValues)
        {
            double distance = 0;
            for (int i = 0; i < weights.Length; i++)
            {
                distance += (inputValues[i] - weights[i]) * (inputValues[i] - weights[i]);
            }
            return Math.Sqrt(distance);
        }
    }
}
