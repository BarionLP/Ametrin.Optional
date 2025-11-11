## 0.3.0
- improved Option joining (#37)
  - `Join` API for `Option<T>`, `Result<T>` and `Result<T, E>` up to 4 elements (after that it starts nesting)
  - `Result/ErrorState.CombineErrors` API
  - `Map` for `(Result<T>, Result<T>)` and `(Result<T, E>, Result<T, E>)`
  - `Map` and `Consume` extensions for tuples overloads with an argument parameter
  - experimental `Consume` for `(Result<T, E>, Result<T, E>)`
- improved LINQ integration (#32)
  - `WhereSuccess`
  - `WhereError`
  - `Branch` for sequences of `Result<T>` and `Result<T, E>`
    - splits the sequence into two lists, one for values and one for errors (only enumerates once)
  - `ValuesOrFirstError` for sequences of `Result<T>` and `Result<T, E>`
  - `ValuesOrError` for sequences of `Option<T>`
  - rename `IEnumerable<T>.FirstOrError` to `TryFirst`
- `Or` overloads with an argument
- `Option.ToResult` and `ErrorState.ToResult` overloads with an argument
- `TryIndexOf` for `ReadOnlySpan<T>`
- `SpanParsableGenerator` improvements
  - improved exception message
  - fixed nesting in static classes
  - fixed `AmOptional008` not triggering
- fixed missing default values in generated async extension methods 
- warning for empty `Consume` calls (`AmOptional009`)
- improvements to Wrong conditional return type (AmOptional003) analyzer
- (TUnit) `IsSuccess(condition)` for `Option<T>` `Result<T>` and `Result<T, E>`
- (TUnit) `IsError(condition)` for `Result<T>`, `Result<T, E>`, `ErrorState` and `ErrorState<E>`
- updated TUnit to 1.0.48

## 0.2.9
- minor `SpanParsableGenerator` fixes
  - add analyzer rule (`AmOptional008`)
- (net10) add `TryParse` extension to all `IParsable` and `ISpanParsable`

## 0.2.8
- add `SpanParsableGenerator`
  - add `GenerateISpanParsableAttribute` to any type implementing `IOptionSpanParsable` to generate the `ISpanParsable` implementation
- updated TUnit to 0.90.45

## 0.2.7
- add `As<T>` operation for save up-casting (#33)
- add a basic analyzer to detect misusage
- made implicit `Exception`/`TError` to `Result`/`ErrorState` conversions not nullable
  - if you really need nullable exceptions `Result/ErrorState.Error` still accepts `Exception?`
  - for `TError` you need to provide a default error  
- updated TUnit to 0.73.4

## 0.2.6
- add `Consume/Match/Require/Reject` overloads with an argument (#26)
- add `ErrorState.Try` (#30)
- add `Result.Try` overload with an argument (#26, #30)
- removed `ToOption` and `ToResult` extensions on arbitary objects
  - use `Option.Of` or `Result.Of`
- fix nullability of error action in a `ConsumeAsync` overload
- updated TUnit to 0.57.24

## 0.2.5
- added `Or(Throw)Async` (#24)
- added `OrThrow` with custom message (#23)
- added `Option<T>.ToResultAsync`
- added `Result<T(, E)>.To(Option|ErrorState)Async`
- added `ErrorState(<E>).ToResult(Async)`
- added `DisposeAsync` extension methods
- made `Result<TValue>` to `Result<TValue, Exception>` explicit to prevent accidential conversions
- marked all custom exceptions as `System.Serializable` (#22)
- deprecated `ToOption` and `ToResult` on arbitary objects (they are more annoying than helpful)
- removed obsolete `(Try)Map(Async)` overloads that took an error map
  - replaced by `MapError` (#19)
- introduced a generator for some async extension methods. please report any breaking changes you encounter with async!  
- updated TUnit to 0.25.21

## 0.2.4
- added `(Try)Map(Error)` overloads with one argument (#21)
- added `RequireExists` for `Result<FileSystemInfo, Error>`
- added `RequireExists` overload with custom error for `Result<FileSystemInfo>`
- added `ZipArchive.TryGetEntry` returning an option
- updated TUnit to 0.24.0

## 0.2.3
- added `.MapError()` (#19) 
  - (replaces `.Map(map, errorMap)` overloads)
- added `Match(Async)` API (#18)
  - (replaces `Map` for `Option`, `ErrorState<T>` and `ErrorState`)
- added custom exceptions for `.OrThrow()`
- removed obsolete `Where(Not)`. Use `Require`/`Reject` instead.
- updated TUnit to 0.19.148

## 0.2.2
- moved nullable extensions to `Ametrin.Optional.Numerics`
- updated TUnit to 0.18.9
- removed obsolete `(Try)Select` use `(Try)Map` instead