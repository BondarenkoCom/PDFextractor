using System.IO.Compression;

namespace SpecFlowPdfReader.Helpers
{
    public class ZipExtractor
    {
        string extractPath = JsonHelper.GetValues().targetForResultExtractedJSON;
        public string ExtractZip(string zipResult)
        {
            using (ZipArchive archive = ZipFile.OpenRead(zipResult))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string entryPath = Path.Combine(extractPath, entry.FullName);
                    if (string.IsNullOrEmpty(entry.Name))
                    {
                        Directory.CreateDirectory(entryPath);
                    }
                    else
                    {
                        entry.ExtractToFile(entryPath,true);
                    }
                }
            }
            return null;
        }
    }
}
