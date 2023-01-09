class Program
{
    private readonly string _connectionString = "Server=localhost,1433;Database=OrderDb;Uid=sa;Pwd=fanto237(!);";
    private string word;
    public int MyProperty { get; set; }
    public string Word { get => word; set => word = value; }

    public int File(string path)
    {
        if (path is null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        _ = File(_connectionString);
        return 12;
    }

    public Program()
    {
        MyProperty = 0;
        word = "this is my last";
    }

    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var m = new Myclass();
        var guid = Guid.NewGuid();

        Console.WriteLine(Myclass.JeRechnen(12, 0));
        int monAge = 23;
        Console.WriteLine($"{monAge}  + {guid}");
        Console.WriteLine(@"je ne sais pas si tu sais que ca ne marche pas !");
    }
}

class Myclass
{

    /// <summary>
    /// This method is used to calculate the addition of 2 numbers
    /// </summary>
    /// <param name="a">first number to be additionated</param>
    /// <param name="b">first number to be additionated</param>
    /// <returns></returns>
    public static int JeRechnen(int a, int b)
    {
        if (a == b) return 0;
        return a + b;
    }

}