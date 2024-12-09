public class HillClimbing
{
    public static List<double> data;

    public HillClimbing(List<double> d)
    {
        data = new List<double>(d);
    }

    public void runHC(BinPacking binPackingProblem, int iterations)
    {
        double[,] result = new double[iterations, 3]; // Add an extra column for the new fitness
        List<List<int>> solutions = new List<List<int>>();

        BinPacking currentSolution = binPackingProblem;
        BinPacking newSolution = new BinPacking(data, binPackingProblem.binCapacity);

        // Copy the current solution to the new solution
        newSolution.copySolution(currentSolution.solution);

        // Start searching for better solutions
        for (int i = 0; i < iterations; i++)
        {
            result[i, 0] = i; // Iteration number
            result[i, 1] = currentSolution.fitness; // Current fitness

            // Make a small change to the new solution
            newSolution = smallChange(newSolution);

            result[i, 2] = newSolution.fitness; // New (suggested) fitness

            Console.WriteLine("\n");
            Console.WriteLine($"Iteration {i + 1}");
            Console.WriteLine($"Current Solution Fitness: {currentSolution.fitness}");
            currentSolution.printSolution();
            Console.WriteLine($"\nNew Solution Fitness: {newSolution.fitness}");
            newSolution.printSolution();

            // Greedy Acceptance: Only accept the new solution if its fitness is lower
            if (newSolution.fitness < currentSolution.fitness)
            {
                currentSolution.copySolution(newSolution.solution);
                Console.WriteLine("New solution accepted.");
            }
            else
            {
                Console.WriteLine("New solution rejected.");
                // Restore the new solution to the current solution for consistency
                newSolution.copySolution(currentSolution.solution);
            }

            solutions.Add(new List<int>(currentSolution.solution)); // Save the current solution
        }

        Console.WriteLine($"Final Solution Fitness: {currentSolution.fitness}");
        ReadWriteFile.writeResults(result, "C:\\Users\\izunn\\source\\repos\\Bin Packing-ADS\\result.csv"); // Write results with current and new fitness
        ReadWriteFile.writeSolutions(solutions, "C:\\Users\\izunn\\source\\repos\\Bin Packing-ADS\\solutions.csv");
    }


    public BinPacking smallChange(BinPacking solution)
    {
        Random random = new Random();

        // Step 1: Select 3 distinct weights randomly
        HashSet<int> selectedIndices = new HashSet<int>();
        while (selectedIndices.Count < 4)
        {
            selectedIndices.Add(random.Next(solution.solution.Count));
        }

        foreach (int weightIndex in selectedIndices)
        {
            int currentBin = solution.solution[weightIndex];
            double itemWeight = HillClimbing.data[weightIndex];
            bool moved = false;

            // Step 2: Try to find a new bin that can accommodate the weight without overflow
            for (int i = 0; i < solution.binWeights.Count; i++)
            {
                if (i != currentBin && solution.binWeights[i] + itemWeight <= solution.binCapacity)
                {
                    // Move the weight to the new bin
                    solution.binWeights[currentBin] -= itemWeight;
                    solution.binWeights[i] += itemWeight;
                    solution.solution[weightIndex] = i;
                    moved = true;
                    break;
                }
            }

            // Step 3: If no suitable bin is found, create a new bin
            if (!moved)
            {
                solution.binWeights.Add(itemWeight); // Create a new bin
                solution.solution[weightIndex] = solution.binWeights.Count - 1;
                solution.binWeights[currentBin] -= itemWeight; // Remove the weight from the current bin
            }
        }

        // Step 4: Recalculate fitness
        solution.calculateFitness();

        return solution;
    }


}
