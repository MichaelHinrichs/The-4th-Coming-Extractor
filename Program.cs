//Written for The 4th Coming. https://store.steampowered.com/app/523750/
using System.IO;
using System.IO.Compression;
namespace The_4th_Coming_Extractor
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = args[0].Remove(args[0].Length - 7);
            BinaryReader bdi = new(File.OpenRead(path + "bdi.dat"));

            int subfileCount = bdi.ReadInt32();
            int bdisize = bdi.ReadInt32();
            MemoryStream bdims = new();
            bdi.ReadInt16();
            Directory.CreateDirectory(path);
            using (var ds = new DeflateStream(new MemoryStream(bdi.ReadBytes(bdisize - 2)), CompressionMode.Decompress))
                ds.CopyTo(bdims);
            bdi = new(bdims);
            bdi.BaseStream.Position = 0;
            BinaryReader bdd = new(File.OpenRead(path + "bdd.dat"));
            Directory.CreateDirectory(path);

            for (int i = 0; i < subfileCount; i++)
            {
                bdd.BaseStream.Position = bdi.ReadInt32();
                int bddsize = bdd.ReadInt32();
                MemoryStream bddms = new();
                bdd.ReadInt16();
                using (var ds = new DeflateStream(new MemoryStream(bdd.ReadBytes(bddsize - 2)), CompressionMode.Decompress))
                    ds.CopyTo(File.Create(path + "//" + i));
            }
        }
    }
}
