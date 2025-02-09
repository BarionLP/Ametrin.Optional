using Ametrin.Optional;

// Option<T> is T or nothing. Examples on how to work with it:
Console.Write("Input a number: ");
Option<string> inputA = Console.ReadLine(); // implicit conversion

var optionA = inputA.Parse<int>();        // apply an operation              // check a condition               // provide a default value
Console.WriteLine($"You entered {optionA.Map(value => value.ToString()).WhereNot(string.IsNullOrWhiteSpace).Or("an invalid number")}");


Console.Write("Input another number: ");
var inputB = Console.ReadLine().ToOption(); // explicit conversion

var optionB = inputB.Parse<int>();
Console.WriteLine($"You entered {optionB.Map(value => value.ToString()).WhereNotWhiteSpace().Or("an invalid number")}");

// operations on tuples (all options must have a value)
(optionA, optionB).Map((a, b) => a * b);

// catching exceptions
optionA.TryMap(a => 1 / a);


// Result<T> is T or an exception. Same operations like Option<T>
Result<int> result = optionB.ToResult();

// stores the catched exception
result.TryMap(b => 1 / b);

// Result<T, E> is T or E. Same operations like Option<T>
Result<int, string> result2 = optionB.ToResult("not a number");
// convert exception to E
result2.TryMap(b => 1 / b, e => e.Message);


// async operations
var text = await new FileInfo("hey.txt").WhereExists().MapAsync(f => File.ReadAllTextAsync(f.FullName)).Map(s => s.ToLower());
await text.ConsumeAsync(text => File.WriteAllTextAsync("hey2.txt", text));
