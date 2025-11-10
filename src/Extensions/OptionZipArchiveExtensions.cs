using System.IO.Compression;

namespace Ametrin.Optional;

public static class OptionZipArchiveExtensions
{
    public static Option<ZipArchiveEntry> TryGetEntry(this ZipArchive archive, string entryName)
        => archive.GetEntry(entryName);
}
