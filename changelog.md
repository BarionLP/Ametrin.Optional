## 0.2.6
- add `ErrorState.Try` (#30)
- add `Consume` overloads with an argument (#26)
- add `Result.Try` overload with an argument (#26, #30)
- add `Require/Reject` overloads with an argument (#26)
- removed `ToOption` and `ToResult` extensions on arbitary objects
  - use `Option.Of` or `Result.Of`
- fix nullability of error action in a `ConsumeAsync` overload
- updated TUnit to 0.57.1

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
- moved nullable extensions to Ametrin.Optional.Numerics
- updated TUnit to 0.18.9
- removed obsolete `(Try)Select` use `(Try)Map` instead