using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

[UseTemplate]
public partial class CollectibleBox : Panel
{
	public Image Icon { get; set; }
	public Label Info { get; set; }

	public Texture InspectIcon { get; set; } = null;

	public Texture PickupIcon { get; set; } = null;


	public CollectibleBox()
	{
		StyleSheet.Load( "/UI/CollectibleBox/CollectibleBox.scss" );

		InspectIcon = Texture.Load( FileSystem.Mounted, "/ui/icons/icon_inspect.png" );
		PickupIcon = Texture.Load( FileSystem.Mounted, "/ui/icons/icon_pickup.png" );
	}


	public override void Tick()
	{
		base.Tick();

		Explorer p = Local.Pawn as Explorer;

		if (p.LookingAt != null)
		{
			//Log.Error( p.LookingAt.Name );

			if ( p.LookingAt is ExplorerNote )
			{
				Info.Text = p.LookingAt.Name;
				Icon.Texture = InspectIcon;
			}
			else if ( p.LookingAt is ExplorerKey )
			{
				//Icon. = p.LookingAt.Name;
				Info.Text = p.LookingAt.Name;
				Icon.Texture = PickupIcon;
			}

			Info.Text += " (E)";

			SetClass( "close", false );
			SetClass( "open", true );
		}
		else
		{
			SetClass( "close", true );
			SetClass( "open", false );
		}	



	}

}
