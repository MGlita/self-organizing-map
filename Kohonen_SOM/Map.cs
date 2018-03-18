using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kohonen_SOM
{
      class Map
    {
        int iteration = 1;
        int currVector;
        double BMU;
        public Node[,] Nodes;
        Node Winner;
        Random rnd;

        public Map(int length, int dimensions)
        {
            rnd = new Random(0);
            Initialise(length, dimensions);
        }

        public void Train(int length, int epochs, double[][] vector)
        {
                ChooseVector();
                FindBMU(vector);
                UpdatingWeights(length, epochs, Winner, vector);
                System.Threading.Thread.Sleep(1);
        }

        void Initialise(int length, int dimensions)
        {
            Nodes = new Node[length, length];
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    Nodes[i, j] = new Node(i, j, dimensions);
                }
            }
        }

        void FindBMU(double[][] vec)
        {
            BMU = double.MaxValue;
            for (int i = 0; i < Nodes.GetLength(1); i++)
            {
                for (int j = 0; j < Nodes.GetLength(1); j++)
                {
                    if (BMU > Nodes[i, j].GetDistance(vec[currVector]))
                    {
                        BMU = Nodes[i, j].GetDistance(vec[currVector]);
                        Winner = Nodes[i, j];
                    }

                }
            }
        }

        void UpdatingWeights(int length, int epochs, Node win, double[][] vec)
        {
            //Calc local neighbourhood
            double mapRadius = length / 2;
            double timeConstant = epochs / Math.Log(mapRadius);
            double neighbourhoodRadius = mapRadius * Math.Exp(-iteration / timeConstant);
            double learningRate = 0.1 * Math.Exp((double)-(iteration) / epochs);

            for (int i = 0; i < Nodes.GetLength(1); i++)
            {
                for (int j = 0; j < Nodes.GetLength(1); j++)
                {
                    double distToNode = Math.Sqrt(Math.Pow(win.x - Nodes[i, j].x, 2) + Math.Pow(win.y - Nodes[i, j].y, 2));

                    if (distToNode <= neighbourhoodRadius * neighbourhoodRadius)
                    {
                        double influence = Math.Exp(-(distToNode) / 2 * (neighbourhoodRadius * neighbourhoodRadius));
                        //Update weight
                        for (int k = 0; k < Nodes[i, j].weights.Length; k++)
                        {
                            Nodes[i, j].weights[k] = Nodes[i, j].weights[k] + influence * learningRate * (vec[currVector][k] - Nodes[i, j].weights[k]);
                        }
                    }
                }
            }

            iteration++;
        }

        void ChooseVector()
        {
            currVector = rnd.Next(0, 6);
        }
    }
}
