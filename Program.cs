using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Bin Packing Problem - Hill Climbing Algorithm");

        // Read the weight dataset
        string datasetPath = "C:\\Users\\izunn\\source\\repos\\Bin Packing-ADS\\weightDataset.csv";
        List<double> weights = ReadWriteFile.readData(datasetPath);

        // Define the bin capacity
        double binCapacity = 130.0; // Set bin capacity as required

        // Create the BinPacking problem instance
        BinPacking binPackingProblem = new BinPacking(weights, binCapacity);

        // Create and run the HillClimbing algorithm
        HillClimbing hillClimbing = new HillClimbing(weights);
        int iterations = 15; // Set the number of iterations
        hillClimbing.runHC(binPackingProblem, iterations);

        Console.WriteLine("Algorithm completed. Results are saved in result.csv and solutions.csv.");
    }
}

