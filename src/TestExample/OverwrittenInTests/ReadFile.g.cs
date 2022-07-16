
using System.CommandLine;
using System.CommandLine.Invocation;
using IncrementalGeneratorSamples.Runtime;

#nullable enable

namespace TestExample;

public partial class ReadFile
{
    internal class CommandHandler : CommandHandler<CommandHandler>
    {
        Option<System.IO.FileInfo?> fileOption = new Option<System.IO.FileInfo?>("--file", "The file to read and display on the console.");

        public CommandHandler()
            : base("read-file", "")
        {
            SystemCommandLineCommand.AddOption(fileOption);
        }

        /// <summary>
        /// The handler invoked by System.CommandLine. This will not be public when generated is more sophisticated.
        /// </summary>
        /// <param name="invocationContext">The System.CommandLine Invocation context used to retrieve values.</param>
        public override int Invoke(InvocationContext invocationContext)
        {
            var commandResult = invocationContext.ParseResult.CommandResult;
            var command = new ReadFile(GetValueForSymbol(fileOption, commandResult));
            return command.Execute();
        }

        /// <summary>
        /// The handler invoked by System.CommandLine. This will not be public when generated is more sophisticated.
        /// </summary>
        /// <param name="invocationContext">The System.CommandLine Invocation context used to retrieve values.</param>
        public override Task<int> InvokeAsync(InvocationContext invocationContext)
        {
            // Since this method is not implemented in the user source, we do not implement it here.
            throw new NotImplementedException();
        }
    }
}
