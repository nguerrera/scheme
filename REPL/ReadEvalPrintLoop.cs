using System;

namespace Scheme
{
    internal static class ReadEvalPrintLoop
    {
        private static void Main()
        {
            ConsoleTextReader input = new ConsoleTextReader();
            Reader reader = new Reader(input);
            while (true)
            {
                if (!input.BufferedTextAvailable)
                {
                    Console.WriteLine();
                    Console.Write("> ");
                }

                try
                {
                    Datum datum = reader.NextDatum();
                    Expression command = Parser.ParseTopLevelExpression(datum);
                    SchemeObject result = Interpreter.Evaluate(command, SchemeEnvironment.Default);
                    result.WriteLine(Console.Out);
                    input.DiscardTrailingAtmosphere();
                }
                catch (InvalidSyntaxException ex)
                {
                    input.DiscardBufferedText();
                    Console.WriteLine(ex.Message);
                }
                catch (InvalidCastException)
                {
                    input.DiscardBufferedText();
                    Console.WriteLine("Type Error");//so lame!
                }
            }
        }
    }
}
