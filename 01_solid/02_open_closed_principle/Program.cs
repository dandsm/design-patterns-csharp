namespace OpenClosedPrinciple;

enum Color
{
    Red, Green, Blue
}

enum Size
{
    Small, Medium, Large, ExtraLarge
}

class Product
{
    public string? Name { get; set; }
    public Color Color { get; set; }
    public Size Size { get; set; }

    public Product() { }

    public Product(string? name, Color color, Size size)
    {
        if (name == null)
        {
            throw new ArgumentNullException(paramName: nameof(name));
        }
        Name = name;
        Color = color;
        Size = size;
    }

    public override string ToString()
    {
        return $"Name: {Name}, Color: {Color}, Size: {Size}";
    }

}

class ProductFilter
{
    public IEnumerable<Product> FilterBySize(IEnumerable<Product> products, Size size)
    {
        foreach (var product in products)
        {
            if (product.Size == size)
                yield return product;
        }
    }

    // Duplicate code
    public IEnumerable<Product> FilterByColor(IEnumerable<Product> products, Color color)
    {
        foreach (var product in products)
        {
            if (product.Color == color)
                yield return product;
        }
    }

    public IEnumerable<Product> FilterBySizeAndColor(IEnumerable<Product> products, Size size, Color color)
    {
        foreach (var product in products)
        {
            if (product.Size == size && product.Color == color)
                yield return product;
        }
    }
}


interface ISpecification<T>
{
    bool IsSatisfied(T t);
}

interface IFilter<T>
{
    IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> specification);
}

class ColorSpecification : ISpecification<Product>
{
    private Color color;

    public ColorSpecification(Color color)
    {
        this.color = color;
    }
    public bool IsSatisfied(Product product)
    {
        return product.Color == color;
    }
}

class SizeSpecification : ISpecification<Product>
{
    private Color color;
    private Size size;

    public SizeSpecification(Size size)
    {
        this.size = size;
    }
    public bool IsSatisfied(Product product)
    {
        return product.Size == size;
    }
}

class MultiSpecification : ISpecification<Product>
{
    private IEnumerable<ISpecification<Product>> specifications;

    public MultiSpecification(params ISpecification<Product>[] specifications)
    {
        this.specifications = specifications;
    }

    public MultiSpecification(IEnumerable<ISpecification<Product>> specifications)
    {
        this.specifications = specifications;
    }

    public bool IsSatisfied(Product product)
    {
        foreach (var spec in specifications)
        {
            if (!spec.IsSatisfied(product))
                return false;
        }

        return true;
    }
}

class BetterProductFiler : IFilter<Product>
{
    public IEnumerable<Product> Filter(IEnumerable<Product> products, ISpecification<Product> specification)
    {
        foreach (var product in products)
        {
            if (specification.IsSatisfied(product))
                yield return product;
        }
    }
}

class App
{
    static void Main(string[] args)
    {
        var apple = new Product("Apple", Color.Red, Size.Small);
        var tree = new Product("Tree", Color.Green, Size.Large);
        var house = new Product("House", Color.Blue, Size.Large);

        var products = new Product[] { apple, tree, house };

        var pf = new ProductFilter();
        Console.WriteLine("Green Products (bad): ");

        foreach (var product in pf.FilterByColor(products, Color.Green))
        {
            Console.WriteLine(product);
        }

        Console.WriteLine("Green Products (better): ");

        var bf = new BetterProductFiler();
        var greenSpec = new ColorSpecification(Color.Green);
        foreach (var product in bf.Filter(products, greenSpec))
        {
            Console.WriteLine(product);
        }

        var blueSpec = new ColorSpecification(Color.Blue);
        var largeSpec = new SizeSpecification(Size.Large);
        // Could be a List to add one by one
        // var specs = new ISpecification<Product>[] { blueSpec, largeSpec };
        // var multiSpec = new MultiSpecification(specs);
        var multiSpec = new MultiSpecification(blueSpec, largeSpec);
        Console.WriteLine("Blue Large Products (better): ");
        foreach (var product in bf.Filter(products, multiSpec))
        {
            Console.WriteLine(product);
        }

    }
}