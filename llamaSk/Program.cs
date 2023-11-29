using System.Security.Cryptography;
using System.Text;
using LLama;
using LLama.Abstractions;
using LLama.Common;
using LLama.Native;

namespace LLamaSK
{
    public class Program
    {
        public static async Task Main()
        {
            var talkatri = new TalkAtri();
            var sk = new SK();
            // await talkatri.RunAsync();
            await sk.RunAsync();
        }
    }
}