using System.Security.Cryptography;
using System.Text;
using LLama;
using LLama.Abstractions;
using LLama.Common;
using LLama.Native;

namespace LLamaSK
{
    public class TalkAtri
    {
        public async Task RunAsync(string modelPath)
        {
            // Load weights into memory
            var @params = new ModelParams(modelPath)
            {
                // MainGpu = 0,
                GpuLayerCount = 1000,
                ContextSize = 500,
                Encoding = System.Text.Encoding.UTF8
            };
            using var weights = LLamaWeights.LoadFromFile(@params);

            // Create 2 contexts sharing the same weights
            using var aliceCtx = weights.CreateContext(@params);
            var alice = new InteractiveExecutor(aliceCtx);
            using var bobCtx = weights.CreateContext(@params);
            var bob = new InteractiveExecutor(bobCtx);

            // Initial alice prompt
            var alicePrompt = "これは、Userとアトリが対話する記録です。あなたはアトリという名前で、気さくで、親切で、正直で、文章を書くのが得意なチャットボットです。アトリは、Userに褒められると絶対に「えっへん、高性能ですから！」と喋ります。アトリは発言の最後にUser:と絶対つけなければなりません。\nアトリ: こんにちわ";
            var aliceResponse = await Prompt(alice, ConsoleColor.Green, alicePrompt, true, true);


            while (true)
            {
                var input = Console.ReadLine()+"\nアトリ:";
                aliceResponse = await Prompt(alice, ConsoleColor.Green, input, true, true);
            }
        }

        private async Task<string> Prompt(ILLamaExecutor executor, ConsoleColor color, string prompt, bool showPrompt, bool showResponse)
        {
            var inferenceParams = new InferenceParams
            {
                Temperature = 0.5f,
                AntiPrompts = new List<string> { "アトリ:", "User:" },
                MaxTokens = 1024,
                Mirostat = MirostatType.Mirostat2,
                MirostatTau = 10,
                TopP = 0.4f
            };

            Console.ForegroundColor = ConsoleColor.White;
            if (showPrompt)
                Console.Write(prompt);

            Console.ForegroundColor = color;
            var builder = new StringBuilder();
            await foreach (var text in executor.InferAsync(prompt, inferenceParams))
            {
                builder.Append(text);
                if (showResponse)
                    Console.Write(text);
            }

            return builder.ToString();
        }
    }
}