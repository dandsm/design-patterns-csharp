using System.Diagnostics;

namespace SingleResponsabilityProject;
class Journal
{
    private readonly List<string> entries = new List<string>();
    private static int count = 0;

    public int AddEntry(string text)
    {
        entries.Add($"{++count}: {text}");
        return count; //momento
    }

    public void RemoveEntry(int index)
    {
        entries.RemoveAt(index);
    }

    public override string ToString()
    {
        return string.Join(Environment.NewLine, entries);
    }

    // BAD Practice
    // Adding to much responsability
    // public void Save(string filename)
    // {
    //     File.WriteAllText(filename, this.ToString());
    // }

    // public static Journal Load(string filename)
    // {

    // }
}

// Best practice, add new class
class Persistance
{
    public void SaveToFile(Journal journal, string filename, bool overwrite = false)
    {
        if (overwrite || !File.Exists(filename))
            File.WriteAllText(filename, journal.ToString());
    }
}


class App
{
    static void Main(string[] args)
    {
        var j = new Journal();
        j.AddEntry("I cried today");
        j.AddEntry("I ate a bug");
        Console.WriteLine(j);

        var p = new Persistance();
        var filename = "/tmp/journal.txt";
        p.SaveToFile(j, filename, true);
        Process.Start($"open {filename}");

    }
}
