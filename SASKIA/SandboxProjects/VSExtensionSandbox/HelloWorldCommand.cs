using System;
using System.ComponentModel.Design;
using System.Globalization;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace VSExtensionSandbox
{
    internal sealed class HelloWorldCommand
    {
        public const int CommandId = 0x0100;
        public static readonly Guid CommandSet = new Guid("d6e7f52b-98d1-4cba-990d-f5ef90e8a240");
        private readonly Package package;
        
        private HelloWorldCommand(Package package)
        {
            this.package = package ?? throw new ArgumentNullException("package");

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }
        
        public static HelloWorldCommand Instance
        {
            get;
            private set;
        }
        
        private IServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }
        
        public static void Initialize(Package package)
        {
            Instance = new HelloWorldCommand(package);
        }
        
        private void MenuItemCallback(object sender, EventArgs e)
        {
            ShowMessageBox("SASKIA VSExtension", "This is the SASKIA Hello World Visual Studio Plugin Extension!!");
        }

        private void ShowMessageBox(string title, string message)
        {
            VsShellUtilities.ShowMessageBox(ServiceProvider,
               message,
               title,
               OLEMSGICON.OLEMSGICON_INFO,
               OLEMSGBUTTON.OLEMSGBUTTON_OK,
               OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }
    }
}
