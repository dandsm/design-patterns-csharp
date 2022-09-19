namespace FluentBuilderInheritance;

class Person
{
    public string Name = string.Empty;

    public string Position = string.Empty;

    public DateTime DateOfBirth;

    public class Builder : PersonBirthDateBuilder<Builder>
    {
        internal Builder() { }
    }

    public static Builder New => new Builder();

    public override string ToString()
    {
        return $"{nameof(Name)}: {Name}, {nameof(Position)}: {Position}";
    }
}

abstract class PersonBuilder
{
    protected Person person = new Person();

    public Person Build()
    {
        return person;
    }
}

class PersonInfoBuilder<T> : PersonBuilder
  where T : PersonInfoBuilder<T>
{
    public T Called(string name)
    {
        person.Name = name;
        return (T)this;
    }
}

class PersonJobBuilder<T> : PersonInfoBuilder<PersonJobBuilder<T>>
  where T : PersonJobBuilder<T>
{
    public T WorksAsA(string position)
    {
        person.Position = position;
        return (T)this;
    }
}

// here's another inheritance level
// note there's no PersonInfoBuilder<PersonJobBuilder<PersonBirthDateBuilder<SELF>>>!

class PersonBirthDateBuilder<T> : PersonJobBuilder<PersonBirthDateBuilder<T>>
  where T : PersonBirthDateBuilder<T>
{
    public T Born(DateTime dateOfBirth)
    {
        person.DateOfBirth = dateOfBirth;
        return (T)this;
    }
}

class Program
{
    class SomeBuilder : PersonBirthDateBuilder<SomeBuilder>
    {

    }

    public static void Main(string[] args)
    {
        var me = Person.New
          .Called("Dmitri")
          .WorksAsA("Quant")
          .Born(DateTime.UtcNow)
          .Build();
        Console.WriteLine(me);
    }
}