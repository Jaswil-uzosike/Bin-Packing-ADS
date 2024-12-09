public class ReadWriteFile
{
    public static void writeFile<T>(List<T> data, string filename)
    {
        using (StreamWriter sw = new StreamWriter(filename))
        {
            for(int i=0; i<data.Count; i++)
            {
                sw.Write(data[i]);
                if(i<data.Count-1)
                    sw.Write(",");
            }
        }
    }

    public static List<Double> readData(string filename)
    {
        List<Double> res = new List<double>();

        string [] x = System.IO.File.ReadAllLines(filename);
        var raw = x[0].Split(',');
        // Console.WriteLine(raw.Length);
        foreach(var item in raw)
        {
            res.Add(Convert.ToDouble(item));
        }
        return res;
    }

    public static void printList<T>(List<T> list)
     {
            
        foreach (T i in list)
        {
            Console.WriteLine(i);
        }
    }

    public static void writeResults<T>(T [,] results, string filename)
    {
        using (StreamWriter sw = new StreamWriter(filename))
        {
            sw.Write("iter,current_fitness,new_fitness");
            sw.WriteLine();
            for (int i=0; i<results.GetLength(0); i++)
            {
                for (int j=0; j<results.GetLength(1); j++)
                {
                    // Console.WriteLine(results[i,j]);
                    sw.Write(results[i,j]);
                    if (j<results.GetLength(1)-1)
                        sw.Write(",");
                }
                sw.Write("\n");
            }
        }
    }

    public static void writeSolutions(List<List<int>> solutions, string filename)
    {
        using(StreamWriter sw = new StreamWriter(filename))
        {
            sw.Write("iter,solution");
            sw.WriteLine();
            for(int i=0; i<solutions.Count; i++)
            {
                sw.Write(i+",");
                for(int j=0; j<solutions[i].Count; j++)
                {
                    sw.Write(solutions[i][j]);
                    sw.Write(" ");
                }
                sw.WriteLine();
            }
        }
    }

}