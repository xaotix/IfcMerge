using System;
using System.IO;
using System.Linq;
using File = System.IO.File;

namespace IfcMerge
{
    public static class OptionsValidation
    {
        public static bool Validate(this Options options)
        {
            if (options == null)
                return false;
            if (options.InputFolder.Length == 0)
            {
                return false;
            }
            if (options.OutputFile.Length == 0)
            {
                options.OutputFile = @$"{options.InputFolder}\ARQUIVO_FINAL_UNIDO{DateTime.Now.ToShortDateString().Replace("/","-")}.IFC";
            }
            //foreach (var file in options.InputFiles)
            //{
            //    var exist = File.Exists(file);
            //    if (!exist)
            //    {
            //        Console.WriteLine($"Input file '{file}' not found.");
            //        return false;
            //    }
            //}
            return true;
        }
    }
}
