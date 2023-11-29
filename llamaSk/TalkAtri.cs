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
        public async Task RunAsync()
        {
            // Console.Write("Please Runinput your model path: ");
            // var modelPath = Console.ReadLine();
            LLama.Native.NativeLibraryConfig.Instance.WithCuda(true);
            LLama.Native.NativeLibraryConfig.Instance.WithLibrary("/home/yukimakura/llamacppdocker/llama.cpp/buildforlib/libllama.so");
            var modelPath = @"/home/yukimakura/llamacppdocker/llama.cpp/models/xwincoder-13b.Q3_K_S.gguf";
            // var modelPath = @"/home/yukimakura/llamacppdocker/llama.cpp/models/xwin-lm-13b-v0.2.Q3_K_S.gguf";
            var t = NativeApi.llama_max_devices();
            // Load weights into memory
            var @params = new ModelParams(modelPath)
            {
                // MainGpu = 0,
                GpuLayerCount = 41,
                ContextSize = 1200,
                Encoding = System.Text.Encoding.UTF8
            };
            using var weights = LLamaWeights.LoadFromFile(@params);

            // Create 2 contexts sharing the same weights
            using var aliceCtx = weights.CreateContext(@params);
            var alice = new InteractiveExecutor(aliceCtx);
            using var bobCtx = weights.CreateContext(@params);
            var bob = new InteractiveExecutor(bobCtx);

            // Initial alice prompt
            var alicePrompt = "これは、Userとアトリが対話する記録です。あなたはアトリという名前で、気さくで、親切で、正直で、文章を書くのが得意なチャットボットです。アトリは、Userに褒められると絶対に「えっへん、高性能ですから！」と喋ります。アトリは発言の最後にUser:と絶対つけなければなりません。私はUserで人間です。\nアトリ: こんにちわ";
            var aliceResponse = await Prompt(alice, ConsoleColor.Green, alicePrompt, true, true);


            // Console.Write("User:");
            // swap back and forth from Alice to Bob
            while (true)
            {
                var input = Console.ReadLine()+"\nアトリ:";
                aliceResponse = await Prompt(alice, ConsoleColor.Green, input, true, true);

                // if (Console.KeyAvailable)
                //     break;
            }
        }

        private async Task<string> Prompt(ILLamaExecutor executor, ConsoleColor color, string prompt, bool showPrompt, bool showResponse)
        {
            var inferenceParams = new InferenceParams
            {
                Temperature = 0.01f,
                AntiPrompts = new List<string> { "アトリ:", "User:" },
                MaxTokens = 512,
                Mirostat = MirostatType.Mirostat2,
                MirostatTau = 10,
                // TopP = 
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