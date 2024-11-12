# Ametrin.Optional

A simple C#/.NET library containing various allocation free option types written by Barion at Ametrin Studios.
This is a rewrite of the Optional namespace in [Ametrin.Utils](https://github.com/BarionLP/Ametrin.Utils).
Warning: This is my 3rd take on options and I assume it will not be my last (Especially once .NET gets type unions). I try to avoid breaking changes. If I have to break something I'll create a legacy branch.

# Types
## Option\<T>
T or None
```csharp
Option<T> a = someT; // equivalent to Option.Of(someT)
Option<T> b = default; // equivalent to Option.Error<T>() 
//careful that default is actually referencing Option<T> and not T. Especially in conditional assignments.
Option<T> a = Option.Success(someT); //requires a nonnull value
```
### Option 
Success or Error
```csharp
Option success = true; // equivalent to Option.Success()
Option error = false; //equivalent to Option.Error()
```
## Result\<T, E>
T or E
```csharp
Result<T, E> success = someT; // equivalent to Option.Success<T>()  
Result<T, E> error = someE; // equivalent to Option.Error<T>() 
```
### Result\<T>
T or Exception
## ErrorState\<T>
Success or T
### ErrorState
Success or Exception

## General API
All option types in this library have a monadic, linq-like api to interact with them. 
### Examples:
```csharp
option.Select(value => value.ToString()).WhereNot(string.IsNullOfWhiteSpace).Or("John Doe");
option.Where(a => a > 5).Select(a => a * 5).Consume(a => process(a), () => reportFailure())
(optionA, optionB).Select((a, b) => a * b);
(optionA, optionB).Consume((a, b) => ...);
```
There is a `...OrNone` alternative most `Try...` methods. If not, I'm happy to accept pull request.
Not all functions might exist for every option type. Feel free to PR them.