using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

[UseTemplate]
public partial class GrabbyHand : Panel
{
	public Image HandIcon { get; set; } = null;

	public static Texture HandTexture { get; set; } = null;

	public GrabbyHand()
	{
		StyleSheet.Load( "/UI/GrabbyHand/GrabbyHand.scss" );

		HandTexture = Texture.Load( FileSystem.Mounted, "/ui/icons/icon_grabber.png" );

	}


	public override void Tick()
	{
		base.Tick();

		if(HandTexture == null)
		{
			//GrabbyHand.HandTexture = Texture.Load( FileSystem.Mounted, "/ui/icons/icon_grabber.png" );
		}

		SetClass( "close", true );
		SetClass( "open", false );

		if ( Local.Pawn == null ) return;

		Explorer p = (Local.Pawn as Explorer);

		if ( p.PropCurrent != null && !p.HoldingProp && p != null )
		{
			SetClass( "close", false );
			SetClass( "open", true );
		}

		HandIcon.Texture = HandTexture;

	}

}
