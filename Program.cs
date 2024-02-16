using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace roslyingscriptingdemo
{
    public class GlobalData(IReadOnlyDictionary<string, object> data)
    {
        public IReadOnlyDictionary<string, object> Data { get; } = data;
    }

    public static class Program
    {
        private const string Code = @"return new {
    Name = Data.Value(""Name""),
    Price = Data.Value(""Price""),
    Code = Data.Value(""ProductCode""),
    Description = Data.Value(""ProductDesc"")
};

static object Value(this IReadOnlyDictionary<string, object> dictionary, string key)
{
    return (dictionary.ContainsKey(key)) ? dictionary[key] : null;
}";
        public static async Task Main()
        {
            // todo: Code would come from some external source.
            var data = await GenerateAsync(Code, new GlobalData(new Dictionary<string, object>
                {
                    { "Name", "2L Milk" },
                    { "Price", 3.75m },
                    { "ProductCode", "PAULS-FC-2L" },
                    { "ProductDesc", "Pauls 2 Litre Bottle Full-Cream Milk" }
                }));
            Console.WriteLine(JsonUtils.Stringify(data));
        }

        private static async Task<object> GenerateAsync(string codeAsString, GlobalData data)
        {
            var options = ScriptOptions.Default
                .AddReferences("System", "System.Collections.Generic")
                .AddImports("System", "System.Collections.Generic");

            var script = CSharpScript.Create(codeAsString, options: options, globalsType: typeof(GlobalData));

            var state = await script.RunAsync(data);

            return (state.Exception == null) ? state.ReturnValue : throw new Exception("Runtime code had issues.", state.Exception);
        }
    }
}
