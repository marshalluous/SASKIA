using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace SASKIA
{
    internal sealed class AnalyzeProjectCommand
    {
        public const int CommandId = 0x0100;
        public static readonly Guid CommandSet = new Guid("ae54e031-e1ae-4468-855c-85cf80861f4d");
        private readonly Package package;
        
        private AnalyzeProjectCommand(Package package)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            if (!(ServiceProvider.GetService(typeof(IMenuCommandService)) is OleMenuCommandService commandService)) return;
            var menuCommandId = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(MenuItemCallback, menuCommandId);
            commandService.AddCommand(menuItem);
        }
        
        public static AnalyzeProjectCommand Instance { get; private set; }
        
        public static void Initialize(Package package)
        {
            Instance = new AnalyzeProjectCommand(package);
        }

        private void X(Document document)
        {
            string assemblyName = Path.GetRandomFileName();
            MetadataReference[] references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
            };

            var programText = File.ReadAllText(document.FilePath);

            var syntaxTree = CSharpSyntaxTree.ParseText(programText);

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            //var sox = sol.Projects;
            var ms = new MemoryStream();
            EmitResult er = compilation.Emit(ms);
        }

        private void MenuItemCallback(object sender, EventArgs e)
        {
            var workspace = MSBuildWorkspace.Create();

            var sol = workspace.OpenSolutionAsync(@"C:\Users\mjeosx\source\repos\BuettiPraesi\BuettiPraesi.sln")
                .Result;
            
            foreach (var p in sol.Projects)
            {
                foreach (var d in p.Documents)
                {
                    if (d.FilePath.EndsWith(".cs"))
                    {
                        X(d);
                    }
                }
            }

            /*
            if (workspace.CanOpenDocuments)
            {
                int x = 4;
            }
            else
            {
                VsShellUtilities.ShowMessageBox(
                    ServiceProvider,
                    "Please open document",
                    "SASKIA",
                    OLEMSGICON.OLEMSGICON_INFO,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
            }

            
            var message = string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", GetType().FullName);
            const string title = "SASKIA Analyze Project";

            VsShellUtilities.ShowMessageBox(
                ServiceProvider,
                message,
                title,
                OLEMSGICON.OLEMSGICON_INFO,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
            */
        }

        private IServiceProvider ServiceProvider => package;
    }
}