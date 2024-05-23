using CommandLine;
using System.Collections.Generic;
using System.Linq;

namespace IfcMerge
{
    public partial class Options
    {
        [Option('i', "InputFile", Required = true, HelpText = "Pasta de origem onde estão os arquivos IFC.")]
        public string InputFolder { get; set; } = "";

        [Option('o', "OutputFile", Required = false, HelpText = "Nome do arquivo de destino. Não deve estar na mesma pasta.")]
        public string OutputFile { get; set; } = "";

        [Option("RetainOwner", HelpText = "retains OwnerHistory ifnormation from the original file, where possible.")]
        public bool RetainOwner { get; set; } = false;

        [Option("RetainSpatial", HelpText = "retains space hierarchy objects from the original files, even if they share the same name of others in the merged set.")]
        public bool RetainSpatial { get; set; } = false;

    }

}
