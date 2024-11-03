A simple C#/.NET library containing various allocation free option types written by Barion at Ametrin Studios.
This is a rewrite of the Optional namespace in [Ametrin.Utils](https://github.com/BarionLP/Ametrin.Utils).
Warning: This might is my 3rd take on options and I assume it will not be my last. Especially once .NET gets type unions. I try to avoid breaking changes. If I have to I'll make a legacy branch.

# Types
## Option\<T>
T or None
```csharp
Option<T> a = Option.Some(someT);
Option<T> a = someT;
Option<T> b = Option.None<T>();
Option<T> b = default; 
//careful that default is actually referencing Option<T> and not T. Especially in conditional assignments.
```
### Option 
Success or Fail
```csharp
Option success = Option.Success();
Option success = true;
Option fail = Option.Fail();
Option fail = false;
```
## Result\<T, E>
T or E
```csharp
Result<T, E> success = someT;
Result<T, E> fail = someE;
```
### Result\<T>
T or Exception
## ErrorState\<T>
T or Success
### ErrorState
Exception or Success

## General API
All option types in this library have a monadic, linq-like api to interact with them. 
### Examples:
```csharp
option.Select(value => value.ToString()).WhereNot(string.IsNullOfWhiteSpace).Or("John Doe");
option.Where(a => a > 5).Select(a => a * 5).Consume(a => process(a), () => reportFailure())
(optionA, optionB).Select((a, b) => a * b);
(optionA, optionB).Consume((a, b) => ...);
```
There is a `...OrNone` alternative most `Try...` methods. If not I'm happy to accept pull request.
Not all functions might exist for every option type. Feel free to PR them. 