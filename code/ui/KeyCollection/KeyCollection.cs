using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

[UseTemplate]
public partial class KeyCollection : Panel
{
	public Image Key0 { get; set; } = null;
	public Image Key1 { get; set; } = null;
	public Image Key2 { get; set; } = null;
	public Image Key3 { get; set; } = null;
	public Image Key4 { get; set; } = null;
	public Image Key5 { get; set; } = null;

	public Texture KeyIcon { get; set; } = null;

	public KeyCollection()
	{
		StyleSheet.Load( "/UI/KeyCollection/KeyCollection.scss" );

	}


	public override void Tick()
	{
		base.Tick();

		if(KeyIcon == null)
		{
			KeyIcon = Texture.Load( FileSystem.Mounted, "/ui/icons/icon_key.png" );
		}

		Key0.Texture = ExplorerGame.Game.KeyCount >= 1 ? KeyIcon : null;
		Key1.Texture = ExplorerGame.Game.KeyCount >= 2 ? KeyIcon : null;
		Key2.Texture = ExplorerGame.Game.KeyCount >= 3 ? KeyIcon : null;
		Key3.Texture = ExplorerGame.Game.KeyCount >= 4 ? KeyIcon : null;
		Key4.Texture = ExplorerGame.Game.KeyCount >= 5 ? KeyIcon : null;
		Key5.Texture = ExplorerGame.Game.KeyCount >= 5 ? KeyIcon : null;

	}

}
