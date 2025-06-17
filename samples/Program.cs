using Ametrin.Optional;
using Ametrin.Optional.Nullable;

BasicOptionsExample();
ResultTypesExample();
await AsyncOperationsExample();
TupleOperationsExample();
NullableIntegrationExample();

/// <summary>
/// Demonstrates basic Option<T> usage with parsing and validation
/// </summary>
static void BasicOptionsExample()
{
    Console.WriteLine("=== Basic Options Example ===\n");
    
    // Option<T> represents a value or nothing (error state)
    Console.Write("Input a number: ");
    Option<string> inputA = Console.ReadLine(); // Implicit conversion from string

    // Chain operations with fluent API
    var optionA = inputA
        .Parse<int>()                          // Try parse as integer
        .Map(value => value * 2)               // Transform value if present
        .Require(x => x > 0)                   // Ensure positive number
        .Map(x => $"Result: {x}");             // Convert to string

    Console.WriteLine($"Processed input: {optionA.Or("Invalid input")}");
}

/// <summary>
/// Shows how to use Result types for error handling
/// </summary>
static void ResultTypesExample()
{
    Console.WriteLine("\n=== Result Types Example ===\n");

    // Result<T> stores exceptions
    Result<double> divisionResult = Result.Success(10d)
        .TryMap(x => 100.0 / x); // Catches exceptions

    divisionResult.Consume(
        success: value => Console.WriteLine($"Division result: {value}"),
        error: ex => Console.WriteLine($"Division failed: {ex.Message}")
    );

    // Result<T, E> allows custom error types
    Result<int, string> parseResult = "abc"
        .ToOption()
        .Parse<int>()
        .ToResult("Not a valid number");       // Custom error message

    var message = parseResult.Match(
        success: value => $"Parsed: {value}",
        error: err => $"Error: {err}"
    );
    Console.WriteLine(message);
}

/// <summary>
/// Demonstrates async operations with options
/// </summary>
static async Task AsyncOperationsExample()
{
    Console.WriteLine("\n=== Async Operations Example ===\n");

    // Chain async operations while maintaining option context
    var fileResult = await new FileInfo("sample.txt")
        .RequireExists()                       // Check if file exists
        .MapAsync(async f =>                   // Async operations
        {
            var text = await File.ReadAllTextAsync(f.FullName);
            return text.ToUpperInvariant();
        });

    await fileResult.ConsumeAsync(
        async text => await File.WriteAllTextAsync("output.txt", text),
        () => Console.WriteLine("File processing failed")
    );
}

/// <summary>
/// Shows how to work with multiple options using tuple operations
/// </summary>
static void TupleOperationsExample()
{
    Console.WriteLine("\n=== Tuple Operations Example ===\n");

    Option<int> first = Option.Success(10);
    Option<int> second = Option.Success(20);

    // Combine multiple options
    var combined = (first, second).Map((a, b) => a + b);
    
    // Handle both success and error cases
    (first, second).Consume(
        success: (a, b) => Console.WriteLine($"Sum: {a + b}"),
        error: () => Console.WriteLine("One or both values were missing")
    );

    // Chain multiple operations
    var result = (first, second)
        .Map((a, b) => a * b)                 // Multiply values
        .Require(x => x != 0)                 // Ensure non-zero
        .Map(x => Math.Sqrt(x));              // Calculate square root

    Console.WriteLine($"Final result: {result.Or(-1)}");
}

/// <summary>
/// Demonstrates integration with nullable types
/// </summary>
static void NullableIntegrationExample()
{
    Console.WriteLine("\n=== Nullable Integration Example ===\n");

    Option<int> numberOption = Option.Success(42);

    // Convert to nullable
    int? nullable = numberOption.OrNull();

    // Fluent API with nullable types
    var result = nullable
        .Map(x => x * 2)                      // Transform
        .Require(x => x > 0)                  // Validate
        ?.ToString()                          // Use null conditional
        ?? "No value";                        // Provide default

    Console.WriteLine($"Nullable result: {result}");
}
