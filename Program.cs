using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Antlr4.Runtime.Misc;
using EjemploQueryLanguage;



IParseTree parse(string input)
{
    ImmediateErrorListener errListener = ImmediateErrorListener.Instance;
    var inputStream = CharStreams.fromString(input);
    var lexer = new queryLexer(inputStream);
    var tokenStream = new CommonTokenStream(lexer);
    var parser = new queryParser(tokenStream);
    parser.RemoveErrorListeners();
    parser.AddErrorListener(errListener);
    var tree = parser.query();
    return tree;
}

Console.WriteLine("Escribe un query en el lenguaje de Amauris. \nEjemplo: tabla1(alias1)&tabla2(alias2){alias1.campoJoin1=alias2.campoJoin2}[alias1.campo1=3,alias2.campo2=\"algo\"].nombre,apellido");

bool todoBien = false;
IParseTree? tree = null;
string input;
ConstructorQuery constructorQuery = new ConstructorQuery();
do {

    do
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        input = Console.ReadLine() ?? "";
        Console.ResetColor();
        try
        {
            tree = parse(input);
            todoBien = true;
        }
        catch (ParseCanceledException e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.Message);
            Console.ResetColor();
            Console.WriteLine("Vuelva a escribir un query, y esta vez, hágalo bien...");
        }

    } while (!todoBien);
    
    if (todoBien)
    {
        string resultado = (string)constructorQuery.Visit(tree);
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(resultado);
        Console.ResetColor();
        Console.WriteLine("Bien. Ahora escribe otro:");
    }

} while (todoBien);
