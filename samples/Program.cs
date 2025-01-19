using Ametrin.Optional;

// basic usecases
Console.Write("Input a number: ");
Option<string> inputA = Console.ReadLine();

var optionA = inputA.Parse<int>();
Console.WriteLine($"You entered {optionA.Select(value => value.ToString()).WhereNot(string.IsNullOrWhiteSpace).Or("an invalid number")}");


Console.Write("Input another number: ");
var inputB = Console.ReadLine().ToOption();

var optionB = inputB.Parse<int>();
Console.WriteLine($"You entered {optionB.Select(value => value.ToString()).WhereNotWhiteSpace().Or("an invalid number")}");


// same operations for pairs of options
var result = (optionA, optionB).Select((a, b) => a * b);

// async operations
var text = new FileInfo("hey.txt").WhereExists().SelectAsync(static f => File.ReadAllTextAsync(f.FullName));