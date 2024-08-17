using System;
using System.Collections.Generic;
using System.Linq;

class Actor
{
    public string Name { get; set; }
    public DateTime Birthdate { get; set; }
}

abstract class ArtObject
{
    public string Author { get; set; }
    public string Name { get; set; }
    public int Year { get; set; }
}

class Film : ArtObject
{
    public int Length { get; set; }
    public IEnumerable<Actor> Actors { get; set; }
}

class Book : ArtObject
{
    public int Pages { get; set; }
}

class Program
{
    static void Main()
    {
        var data = new List<object>() {
            "Hello",
            new Book() { Author = "Terry Pratchett", Name = "Guards! Guards!", Pages = 810 },
            new List<int>() {4, 6, 8, 2},
            new string[] {"Hello inside array"},
            new Film() { Author = "Martin Scorsese", Name= "The Departed", Actors = new List<Actor>() {
                new Actor() { Name = "Jack Nickolson", Birthdate = new DateTime(1937, 4, 22)},
                new Actor() { Name = "Leonardo DiCaprio", Birthdate = new DateTime(1974, 11, 11)},
                new Actor() { Name = "Matt Damon", Birthdate = new DateTime(1970, 8, 10)}
            }},
            new Film() { Author = "Gus Van Sant", Name = "Good Will Hunting", Actors = new List<Actor>() {
                new Actor() { Name = "Matt Damon", Birthdate = new DateTime(1970, 8, 10)},
                new Actor() { Name = "Robin Williams", Birthdate = new DateTime(1951, 8, 11)},
            }},
            new Book() { Author = "Stephen King", Name="Finders Keepers", Pages = 200},
            "Leonardo DiCaprio"
        };

    
        data.Where(x => !(x is ArtObject)).ToList().ForEach(x => Console.WriteLine(x));

        data.OfType<Film>().SelectMany(f => f.Actors.Select(a => a.Name)).ToList().ForEach(Console.WriteLine);

        Console.WriteLine(data.OfType<Film>().SelectMany(f => f.Actors).Count(a => a.Birthdate.Month == 8));

        
        data.OfType<Film>().SelectMany(f => f.Actors).OrderBy(a => a.Birthdate).Take(2).ToList().ForEach(a => Console.WriteLine(a.Name));

       
        data.OfType<Book>().GroupBy(b => b.Author).ToList().ForEach(g => Console.WriteLine($"{g.Key}: {g.Count()}"));

        Console.WriteLine(data.OfType<Book>().GroupBy(b => b.Author).Count() + " books, " + data.OfType<Film>().GroupBy(f => f.Author).Count() + " films");

       
        Console.WriteLine(data.OfType<Film>().SelectMany(f => f.Actors.Select(a => a.Name)).SelectMany(name => name.Replace(" ", "").ToLower().Distinct()).Distinct().Count());

       
        data.OfType<Book>().OrderBy(b => b.Author).ThenBy(b => b.Pages).ToList().ForEach(b => Console.WriteLine(b.Name));

        data.OfType<Film>().SelectMany(f => f.Actors.Select(a => new { Actor = a.Name, Film = f.Name })).GroupBy(x => x.Actor).ToList().ForEach(g => Console.WriteLine($"{g.Key}: {string.Join(", ", g.Select(f => f.Film))}"));

        Console.WriteLine(data.OfType<Book>().Sum(b => b.Pages) + data.OfType<IEnumerable<int>>().SelectMany(i => i).Sum());

        
        var dict = data.OfType<Book>().GroupBy(b => b.Author).ToDictionary(g => g.Key, g => g.Select(b => b.Name).ToList());

        
        data.OfType<Film>().Where(f => f.Actors.Any(a => a.Name == "Matt Damon") && f.Actors.All(a => !data.OfType<string>().Contains(a.Name))).ToList().ForEach(f => Console.WriteLine(f.Name));
    }
}
