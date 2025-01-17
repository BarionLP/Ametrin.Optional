# Ametrin.Optional

A simple and fast .NET library containing various allocation free option types written by Barion.
This is a rewrite of the Optional namespace in [Ametrin.Utils](https://github.com/BarionLP/Ametrin.Utils).</br>

```
dotnet add package Ametrin.Optional
```

# Types
## `Option<T>`
T or Error
```csharp
Option<T> a = someT; // equivalent to Option.Of(someT) - will produce Error if someT is null
Option<T> b = default; // equivalent to Option.Error<T>() 
//careful that default is actually referencing Option<T> and not T. Especially in conditional assignments.
Option<T> a = Option.Success(someT); //requires a nonnull value
```
### `Option` 
Success or Error
```csharp
Option success = true; // equivalent to Option.Success()
Option error = false; //equivalent to Option.Error()
```
## `Result<T, E>`
T or E
```csharp
Result<T, E> success = someT; // equivalent to Result.Success<T, E>(someT)  
Result<T, E> error = someE; // equivalent to Option.Error<T, E>(someE) 
```
### `Result<T>`
T or Exception
## `ErrorState<T>`
Success or T
### `ErrorState`
Success or Exception

## General API
All option types in this library have a monadic, linq-like api to interact with them. 
### Examples
```csharp
option.Select(value => value.ToString()).WhereNot(string.IsNullOrWhiteSpace).Or("John Doe");
option.Where(a => a > 5).Select(a => a * 5).Consume(a => process(a), () => reportFailure());
(optionA, optionB).Select((a, b) => a * b);
(optionA, optionB).Consume((a, b) => ...);
```
There is an alternative most `Try...` methods that return an option. If not, I'm happy to accept pull requests.</br>
If you are missing something feel free to create an issue or PR (talk to me before so i agree on the design)
### Edge Cases
If you run into edge cases that are not covered by the API let me know so we can find a solution</br>
In the mean time you can use the `OptionsMarshall` to get direct access to the underlying data. This can also be used in high performance scenarious where the delegates cannot be static
```csharp
if(OptionsMarshall.IsSuccess(result))
{
    ...
}

if(OptionsMarshall.TryGetError(result, out var error))
{
    ...
}

if(OptionsMarshall.TryGetValue(result, out var value))
{
    ...
}
```

### Unit tests
If you need to unit test an option you can use `Ametrin.Optional.Testing.TUnit` to to simplify the testing experience with TUnit. Feel free to contribute similar extensions for your testing framework

## Performance
Ametrin.Optional promises minimal to none performance impact on your application.
### Example Benchmarks
[Parsing a DateTime](/benchy/Examples/ParsingDateTimeBenchmarks.cs)
```
| Method            | Mean      | Error    | StdDev   | Allocated |
|------------------ |----------:|---------:|---------:|----------:|
| Default_Success   | 291.30 ns | 1.093 ns | 1.022 ns |         - |
| Option_Success    | 310.56 ns | 1.914 ns | 1.697 ns |         - |
| RefOption_Success | 300.98 ns | 1.454 ns | 1.360 ns |         - |

| Default_Error     |  72.12 ns | 0.289 ns | 0.226 ns |         - | // using TryParse (with Exceptions this would be way higher)
| Option_Error      |  67.75 ns | 0.324 ns | 0.287 ns |         - |
| RefOption_Error   |  72.75 ns | 0.145 ns | 0.136 ns |         - |
```