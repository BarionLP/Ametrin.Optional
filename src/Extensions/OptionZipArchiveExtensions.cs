using System.IO.Compression;

namespace Ametrin.Optional;

public static class OptionZipArchiveExtensions
{
    extension(ZipArchive archive)
    {
        public Option<ZipArchiveEntry> TryGetEntry(string entryName)
        => archive.GetEntry(entryName);
    }
}
