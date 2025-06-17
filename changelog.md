## 0.2.5
- added `Or(Throw)Async` (#24)
- added `OrThrow` with custom message (#23)
- removed obsolete `(Try)Map(Async)` overloads that took an error map
  - replaced by `MapError` (#19)

## 0.2.4
- added `(Try)Map(Error)` overloads with one argument (#21)
- added `RequireExists` for `Result<FileSystemInfo, Error>`
- added `RequireExists` overload with custom error for `Result<FileSystemInfo>`
- added `ZipArchive.TryGetEntry` returning an option
- updated TUnit 0.24.0

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