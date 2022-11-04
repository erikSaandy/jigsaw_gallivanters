using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

public partial class ExplorerGame : Sandbox.Game
{

	public bool IsNoteOpen() => NoteIsOpen;
	public bool OpenNote() => NoteIsOpen = true;
	public bool CloseNote() => NoteIsOpen = false;

	[Net, Local]
	private bool NoteIsOpen { get; set; } = false;

	[Net, Local]
	public string CurrentNoteText { get; set; } = "yo";

	public void NoteHelperInit()
	{

	}

}
