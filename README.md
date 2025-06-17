# Ametrin.Optional

A modern, allocation-free library providing robust optional types for .NET. It offers a flexible and efficient way to handle nullable values, errors, and exceptions with a fluent, monadic API.

```bash
dotnet add package Ametrin.Optional
```

## Features

- 🚀 **Zero Allocation** - Designed for high-performance scenarios
- 🧩 **Monadic API** - Fluent interface for transformations and error handling
- 🔄 **Easy Integration** - Seamless integration with existing C# code
- 🎯 **Multiple Option Types** - Different types for various use cases
- 💪 **Async Support** - First-class support for async operations

## Core Types

### `Option<T>`
Represents a value of type `T` or nothing (error state).

```csharp
// Creating Options
Option<T> b = Option.Success(someT);    // explicit success creation (throws if someT is null)
Option<T> c = Option.Error<T>();        // explicit error creation
Option<T> a = Option.Of(someT);         // returns Option.Error<T>() if someT is null
Option<T> a = someT;                    // implicit conversion from T -> Option.Of(someT)
Option<T> d = default;                  // default results in an error state -> Option.Error<T>() 
```

### `Result<T>` and `Result<T, E>`
Like `Option<T>` but with error information.  
`Result<T>` stores an Exception as error  
`Result<T, E>` stores a custom error type

```csharp
Result<T> b = Result.Success(someT);            // explicit success creation (throws if someT is null)
Result<T> c = Result.Error<T>(new Exception()); // explicit error creation
Result<T> a = Result.Of(someT);                 // returns Result.Error<T>(new NullReferenceException()) if someT is null
Result<T> a = someT;                            // implicit conversion from T -> Result.Of(someT)
Result<T> a = new Exception();                  // implicit conversion from Exception -> Result.Error<T>(new Exception())
// same applies for Result<T, E> (with generic arguments)
```

### `ErrorState` and `ErrorState<E>`
Represents a success state or an error value  
`ErrorState` stores an Exception as error  
`ErrorState<E>` stores a custom error type
```csharp
ErrorState success = default;           // ErrorState.Success()
ErrorState error = new Exception();     // ErrorState.Success(new Exception())
// same applies for ErrorState<E> (with generic arguments)
```

### `Option`
Represents a success state or error state. Holds no value.
```csharp
Option success = true;          // Option.Success();
Option error = false;           // Option.Error();
```

## Core API
```csharp
// Common Operations
option.Map(value => value * 2);         // transform value if present
option.Require(x => x > 0);             // filter based on predicate
option.Reject(x => x > 0);              // inverse of Require
option.Or(defaultValue);                // provide default value
option.OrThrow();                       // throw if error
option.Match(                           // reduce to a value (equivalent to .Map(success).Or(error) but supports ref structs)
    success: value => value, 
    error: error => defaultValue
);
option.Consume(                         // handle success and error
    success: value => {}, 
    error: error => {}
);
```

## Advanced Features

### Tuple Operations

```csharp
// Combine multiple options
(optionA, optionB).Map((a, b) => a + b);
(optionA, optionB).Consume(
    success: (a, b) => Console.WriteLine($"{a} + {b} = {a + b}"),
    error: () => Console.WriteLine("One of the values was missing")
);
```

### Async Support

```csharp
// Async operations with full option support
var text = await new FileInfo("file.txt")
    .RequireExists()
    .MapAsync(f => File.ReadAllTextAsync(f.FullName))
    .MapAsync(s => s.ToLower());

await text.ConsumeAsync(text => File.WriteAllTextAsync("output.txt", text));
```

### `RefOption<T>`
Like `Option<T>` but can hold a ref struct as value

## Edge Cases

For edge cases or high-performance scenarios where delegates cannot be static, use `OptionsMarshall`:

```csharp
if(OptionsMarshall.IsSuccess(result))
{
    if(OptionsMarshall.TryGetValue(result, out var value))
    {
        // use value
    }
}

if(OptionsMarshall.TryGetError(result, out var error))
{
    // handle error
}
```

## Testing

For testing code using Ametrin.Optional types, use the `Ametrin.Optional.Testing.TUnit` extensions:

```csharp
await Assert.That(option).IsSuccess(expectedValue);
await Assert.That(option).IsError();
```

## Contributing

Contributions are welcome! Feel free to:
- Create issues for bugs or feature requests
- Submit pull requests (discuss design first)
- Add extensions for more testing frameworks

## Performance

The library is designed with performance in mind and has minimal to no overhead. Example benchmark for parsing a DateTime:

```
| Method            | Mean      | Error    | StdDev   | Allocated |
|------------------ |----------:|---------:|---------:|----------:|
| Default_Success   | 291.30 ns | 1.093 ns | 1.022 ns |         - |
| Option_Success    | 310.56 ns | 1.914 ns | 1.697 ns |         - |
| RefOption_Success | 300.98 ns | 1.454 ns | 1.360 ns |         - |
| Default_Error     |  72.12 ns | 0.289 ns | 0.226 ns |         - | // using TryParse
| Option_Error      |  67.75 ns | 0.324 ns | 0.287 ns |         - |
| RefOption_Error   |  72.75 ns | 0.145 ns | 0.136 ns |         - |
```
