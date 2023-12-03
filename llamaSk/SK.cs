using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using LLama;
using LLama.Abstractions;
using LLama.Common;
using LLama.Native;
using LLamaSharp.SemanticKernel.ChatCompletion;
using LLamaSharp.SemanticKernel.TextCompletion;
using LLamaSharp.SemanticKernel.TextEmbedding;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.Embeddings;
using Microsoft.SemanticKernel.AI.TextCompletion;
using Microsoft.SemanticKernel.Diagnostics;
using Microsoft.SemanticKernel.Planners;

namespace LLamaSK
{
    public class SK
    {
        public async Task RunAsync(string modelPath)
        {
            var t = NativeApi.llama_max_devices();
            // Load weights into memory
            var @params = new ModelParams(modelPath)
            {
                // MainGpu = 0,
                GpuLayerCount = 42,
                ContextSize = 2048,
                Encoding = System.Text.Encoding.UTF8
            };
            using var weights = LLamaWeights.LoadFromFile(@params);
            // var embedding = new LLamaEmbedder(weights, @params);

            var ex = new StatelessExecutor(weights, @params);
            var txtcmp = new LLamaSharpTextCompletion(ex);
            var builder = new KernelBuilder();
            builder.WithAIService<ITextCompletion>("local-llama", txtcmp, true);
            // builder.WithAIService<ITextEmbeddingGeneration>("local-llama-embed", new LLamaSharpEmbeddingGeneration(embedding), true);
            var kernel = builder.Build();
            kernel.ImportFunctions(new Plugins.WhetherPlugin.Whether(), "WhetherPlugin");
            kernel.ImportFunctions(new Plugins.RotateMotorPlugin.RotateMotor(), "RotateMotorPlugin");
            var seqpl = new SequentialPlanner(kernel);

            while (true)
            {
                Console.WriteLine("プロンプトを入力して下さい");
                var ask = Console.ReadLine();
                if (ask == "exit")
                    break;
                try
                {

                    var plan = await seqpl.CreatePlanAsync(ask);

                    Console.WriteLine("Plan:\n");
                    Console.WriteLine(JsonSerializer.Serialize(plan, new JsonSerializerOptions { WriteIndented = true }));

                    foreach (var s in plan.Steps)
                    {

                        var result = await kernel.RunAsync(s);

                        Console.WriteLine("Plan results:");
                        // foreach (var fr in result.FunctionResults)
                        // {
                        Console.WriteLine(result);
                        // }
                    }
                }
                catch (SKException skex)
                {
                    Console.WriteLine($"コケた:{skex.Message}");
                    await Prompt(ex, ConsoleColor.DarkCyan, ask, true, true);
                }

            }

        }

        private async Task<string> Prompt(ILLamaExecutor executor, ConsoleColor color, string prompt, bool showPrompt, bool showResponse)
        {
            var inferenceParams = new InferenceParams
            {
                Temperature = 0.3f,
                AntiPrompts = new List<string> { "アトリ:", "User:" },
                MaxTokens = 2048,
                Mirostat = MirostatType.Mirostat2,
                MirostatTau = 10,
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