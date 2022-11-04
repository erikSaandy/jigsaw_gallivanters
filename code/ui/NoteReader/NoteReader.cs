using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

[UseTemplate]
public partial class NoteReader : Panel
{
	public Image Graphic { get; set; } = null;
	public Label Text { get; set; } = null;

	public Texture NoteTexture { get; set; } = null;

	public NoteReader()
	{
		StyleSheet.Load( "/UI/NoteReader/NoteReader.scss" );

		NoteTexture = Texture.Load( FileSystem.Mounted, "/ui/note/note.png" );

	}

	public override void Tick()
	{
		base.Tick();

		Graphic.Texture = NoteTexture;
		Text.Text = ExplorerGame.Game.CurrentNoteText;

		if ( ExplorerGame.Game.IsNoteOpen() )
		{
			SetClass( "open", true );
			SetClass( "close", false );
		}
		else
		{
			SetClass( "open", false );
			SetClass( "close", true );
		}

	}

}
