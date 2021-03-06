using System;
using Eto.Forms;
using Eto.Drawing;
using System.IO;
using Pablo.Interface.Dialogs;
using Eto.IO;
using Eto;

namespace Pablo.Interface.Actions
{
	public class RenameFile : Command, IDisposable
	{
		Main main;
		
		public const string ActionID = "renameFile";
		
		public RenameFile (Main main)
		{
			this.main = main;
			base.ID = ActionID;
			this.MenuText = "&Rename";
			this.ToolTip = "Rename the selected file";
			this.Enabled = main.FileList.SelectedFile != null || main.FileList.SelectedDirectory != null;
			this.Shortcut = PabloCommand.CommonModifier | Keys.R;
			main.FileList.SelectedIndexChanged += fileList_Changed;
		}

		public void Dispose()
		{
			main.FileList.SelectedIndexChanged -= fileList_Changed;
		}
		
		void fileList_Changed(object sender, EventArgs e)
		{
			this.Enabled = main.FileList.SelectedFile != null || main.FileList.SelectedDirectory != null;
		}

		protected override void OnExecuted(EventArgs e)
		{
			base.OnExecuted(e);
			var file = main.FileList.SelectedFile;
			if (file != null && File.Exists(file.FullName)) {
				var dialog = new RenameDialog(file.FullName);
				var result = dialog.ShowModal(main);
				if (result == DialogResult.Ok)
				{
					File.Move(file.FullName, dialog.NewName);
					main.FileList.UpdateDirectory();
				}
			}

			var dir = main.FileList.SelectedDirectory;
			if (dir != null) {
				var dialog = new RenameDialog(dir.FullName);
				var result = dialog.ShowModal(null);
				if (result == DialogResult.Ok)
				{
					if (Directory.Exists(dir.FullName)) Directory.Move(dir.FullName, dialog.NewName);
					else if (File.Exists(dir.FullName)) File.Move(dir.FullName, dialog.NewName);
					main.FileList.UpdateDirectory();
				}
			}
		} 
	}
}

