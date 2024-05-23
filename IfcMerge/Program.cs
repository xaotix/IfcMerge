using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;

namespace IfcMerge
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var t = Parser.Default.ParseArguments<Options>(args)
                .WithParsed(Run)
                .WithNotParsed(HandleParseError);
            return t.Tag switch
            {
                ParserResultType.Parsed => 0,
                _ => 1,
            };
        }

        static string AsmblyVer
        {
            get
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
                string version = fvi.FileVersion;
                return version;
            }
        }

        static void Run(Options opts)
        {
            Console.WriteLine($"DLM.IFC.Merge V.{AsmblyVer}");
            var valid = opts.Validate();
            if (!valid)
                return;

            var builder = new IfcMerger(opts);
            foreach (var inFile in Directory.GetFiles(opts.InputFolder))
            {
                if (File.Exists(inFile))
                {
                    var f = new FileInfo(inFile);

                    if (f.Extension.ToLowerInvariant() == ".txt")
                    {
                        Console.WriteLine($"Mapeando arquivos da pasta: ${inFile}");
                        var allLines = File.ReadAllLines(inFile);
                        foreach (var line in allLines)
                        {
                            if (string.IsNullOrEmpty(line))
                                continue;
                            Console.WriteLine($"- Carregando arquivo: ${line}");
                            var fi = new FileInfo(line);
                            builder.MergeFile(fi);
                        }
                    }
                    else if (f.Extension.ToLowerInvariant() == ".ifc")
                    {
                        Console.WriteLine($"Carregando ifc: ${inFile}");
                        builder.MergeFile(f);
                    }
                    else
                    {
                        Console.WriteLine($"Arquivo inválido: ${inFile}");
                    }
                }
                else
                {
                    Console.Error.WriteLine($"Falha ao tentar abrir o arquivo {inFile}");
                }

            }
            Console.WriteLine($"{builder.processed} arquivos unidos. Criando IFC...");
            var file = builder.SaveIfcModel();
            Console.WriteLine($"Criando arquivo IFC... {file.FullName}");
        }

        static void HandleParseError(IEnumerable<Error> errs)
        {
            //handle errors etc
            foreach (var err in errs)
            {

                switch (err)
                {
                    case VersionRequestedError _:
                    case HelpRequestedError _:
                        break;

                    case MissingRequiredOptionError missing:
                        Console.WriteLine($"Faltando dados mínimos para rodar, input: --{missing.NameInfo.LongName}");
                        break;

                    case MissingValueOptionError missing:
                        Console.WriteLine($"Faltando dados mínimos para rodar, input: --{missing.NameInfo.LongName}");
                        break;


                    default:
                        Console.Error.WriteLine(err);

                        break;

                }
                if (err.StopsProcessing)
                {
                    break;
                }
            }
        }
    }
}
