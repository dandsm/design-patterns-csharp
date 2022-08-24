namespace LiskovSubstitutionPrinciple;

class Rectangle
{
    public virtual int Width { get; set; }
    public virtual int Height { get; set; }

    public Rectangle() { }

    public Rectangle(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public override string ToString()
    {
        return $"{nameof(Width)}: {Width}, {nameof(Height)}: {Height}";
    }
}

class Square : Rectangle
{
    public override int Width 
    { 
        set {base.Width = value; base.Height = value; }
    }
    public override int Height 
    { 
        set {base.Width = value; base.Height = value; }
    }

}

class Program
{

    static private int Area(Rectangle r) => r.Width * r.Height;
    static void Main(string[] args)
    {
        var rc = new Rectangle(2,4);
        Console.WriteLine($"{rc} has an area of {Area(rc)}");

        var sq = new Square();
        sq.Width = 4;
        Console.WriteLine($"{sq} has an area of {Area(sq)}");

    }
}