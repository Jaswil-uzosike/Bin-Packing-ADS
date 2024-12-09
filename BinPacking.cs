public class BinPacking : HillClimbing
{
    public List<int> solution = new List<int>();
    public int solSize = 0;
    public double fitness = 0.00;
    public List<double> binWeights = new List<double>();
    public double binCapacity;
    public double weightBinCount = 5.0; // Weighting for number of bins
    public double weightExtraSpace = 1.0; // Weighting for extra space
    public double weightOverflow = 10.0; // Heavy penalty for overflow

    public BinPacking(List<double> data, double binCapacity) : base(data)
    {
        solSize = data.Count;
        this.binCapacity = binCapacity;
        solution = generateInitialSolution();
        calculateFitness();
    }

    public void copySolution(List<int> sol)
    {
        solution = new List<int>(sol);
        calculateFitness();
    }

    public void calculateFitness()
    {
        binWeights = new List<double>();
        int maxBin = solution.Max() + 1;
        for (int i = 0; i < maxBin; i++) binWeights.Add(0);

        // Calculate bin weights
        for (int i = 0; i < solSize; i++)
        {
            binWeights[solution[i]] += data[i];
        }

        // Initialize fitness components
        double binCountPenalty = weightBinCount * maxBin;
        double extraSpacePenalty = 0.0;
        double overflowPenalty = 0.0;

        // Calculate penalties
        foreach (var binWeight in binWeights)
        {
            if (binWeight > binCapacity)
            {
                overflowPenalty += weightOverflow * (binWeight - binCapacity);
            }
            else
            {
                extraSpacePenalty += weightExtraSpace * (binCapacity - binWeight);
            }
        }

        // Combine penalties into fitness
        fitness = binCountPenalty + extraSpacePenalty + overflowPenalty;
    }

    public List<int> generateInitialSolution()
    {
        List<int> res = new List<int>();
        Random random = new Random();

        // Step 1: Create a randomized list of indices to shuffle the weights
        List<int> indices = Enumerable.Range(0, data.Count).ToList();
        indices = indices.OrderBy(x => random.Next()).ToList(); // Shuffle indices

        // Step 2: Initialize bins and their loads
        double totalWeight = data.Sum();
        int estimatedBins = (int)Math.Ceiling(totalWeight / binCapacity) + 3;

        List<double> binLoad = new List<double>();
        for (int i = 0; i < estimatedBins; i++)
        {
            binLoad.Add(0); // All bins start empty
        }

        int currentBin = 0;

        // Step 3: Distribute weights using round-robin on randomized indices
        foreach (int weightIndex in indices)
        {
            double itemWeight = data[weightIndex];
            bool placed = false;

            // Try to place the weight in an existing bin using round-robin
            for (int j = 0; j < binLoad.Count; j++)
            {
                int targetBin = (currentBin + j) % binLoad.Count;

                if (binLoad[targetBin] + itemWeight <= binCapacity)
                {
                    binLoad[targetBin] += itemWeight;
                    res.Add(targetBin);
                    placed = true;
                    currentBin = (targetBin + 1) % binLoad.Count; // Update round-robin pointer
                    break;
                }
            }

            // If no suitable bin exists, create a new one
            if (!placed)
            {
                binLoad.Add(itemWeight);
                res.Add(binLoad.Count - 1); // Assign the item to the new bin
                currentBin = 0; // Reset round-robin pointer to include the new bin
            }
        }

        return res;
    }


    public void printSolution()
    {
        int numBinsUsed = binWeights.Count;
        int numOverflowedBins = 0;
        double totalUnusedSpace = 0.0;

        // Calculate overflowed bins and total unused space
        foreach (var binWeight in binWeights)
        {
            if (binWeight > binCapacity)
            {
                numOverflowedBins++;
            }
            else
            {
                totalUnusedSpace += (binCapacity - binWeight);
            }
        }

        Console.WriteLine("Solution:");
        for (int i = 0; i < solSize; i++)
        {
            Console.WriteLine($"Weight {data[i]} -> Bin {solution[i]}");
        }
        /*
                Console.WriteLine("Bin Weights:");
                foreach (var bw in binWeights)
                {
                    Console.WriteLine(bw);
                }
        */
        //Console.WriteLine($"Fitness: {fitness}");
        Console.WriteLine($"Number of Bins Used: {numBinsUsed}");
        Console.WriteLine($"Number of Overflowed Bins: {numOverflowedBins}");
        Console.WriteLine($"Total Unused Space: {totalUnusedSpace}");
    }

}
