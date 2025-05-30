## 0.2.4
- added `RequireExists` for `Result<FileSystemInfo, Error>` and overload with custom error for `Result<FileSystemInfo>`
- added `RequireExists` overload with custom error for `Result<FileSystemInfo>`
- added `ZipArchive.TryGetEntry` returning an option
- updated TUnit 0.21.13

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