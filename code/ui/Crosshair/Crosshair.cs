using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

[UseTemplate]
public partial class Crosshair : Panel
{

	public Crosshair()
	{
		StyleSheet.Load( "/UI/Crosshair/Crosshair.scss" );
	}


	public override void Tick()
	{
		base.Tick();

	}

}
