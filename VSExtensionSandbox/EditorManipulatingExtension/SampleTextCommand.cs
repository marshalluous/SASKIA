using System;
using System.ComponentModel.Design;
using System.Globalization;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace EditorManipulatingExtension
{
    internal sealed class SampleTextCommand
    {
        private readonly Package package;
        public const int CommandId = 0x0100;
        public static readonly Guid CommandSet = new Guid("3991602d-7f29-4473-ad17-f57757ba7a28");

        private SampleTextCommand(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this.package = package;

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }
        
        public static SampleTextCommand Instance
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
            Instance = new SampleTextCommand(package);
        }
        
        private void MenuItemCallback(object sender, EventArgs e)
        {
            
        }
    }
}
