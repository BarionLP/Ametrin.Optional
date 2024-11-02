A simple C#/.NET library containing various option types written by Barion at Ametrin Studios.
This is a rewrite of the Optional namespace in [Ametrin.Utils](https://github.com/BarionLP/Ametrin.Utils).

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