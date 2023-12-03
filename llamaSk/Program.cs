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
            Console.WriteLine("アトリと会話する場合は 1 を、SemanticKernelのデモは 2 を入力");
            var input = Console.ReadLine();
            if (int.TryParse(input, out var inputnum))
            {
                switch (inputnum)
                {
                    case 1:
                        {
                            var path = @"/workspaces/llamasharp_SK_Demo/models/xwin-lm-13b-v0.2.Q3_K_S.gguf";
                            if (validateModelPath(path))
                                await talkatri.RunAsync(path);
                            break;
                        }
                    case 2:
                        {
                            var path = @"/workspaces/llamasharp_SK_Demo/models/xwincoder-13b.Q3_K_S.gguf";
                            if (validateModelPath(path))
                                await sk.RunAsync(path);
                            break;
                        }
                }
            }
            else
                Console.WriteLine($"「{input}」は無効な引数です");
        }

        private static bool validateModelPath(string path)
        {
            if (File.Exists(path))
                return true;
            else
            {
                Console.WriteLine($"{path}は存在しません。downloadModels.shは実行しましたか？");
                return false;
            }
        }

    }
}

