using Sandbox;
using SandboxEditor;

/// <summary>
/// This entity defines the spawn point of the player in first person shooter gamemodes.
/// </summary>

[Model]
[SupportsSolid]
[Library( "explorer_note" ), HammerEntity]
[Title( "Note" ), Category( "Note" ), Icon( "place" )]
[EditorModel( "models/parchment_01/parchment_01.vmdl" )]
public class ExplorerNote : ModelEntity
{

	[Property( Title = "Text" )]
	public string Text { get; set; } = null;

	public bool HasBeenOpened { get; set; } = false;

	public override void Spawn()
	{

		base.Spawn();

		SetModel( "models/parchment_01/parchment_01.vmdl" );
		SetupPhysicsFromModel( PhysicsMotionType.Static );
		
		EnableAllCollisions = true;
		EnableTouch = true;
		EnableTraceAndQueries = true;

		Tags.Add( "solid" );

		Transmit = TransmitType.Always;
	}

	public override void ClientSpawn()
	{
		base.ClientSpawn();

		SetModel( "models/parchment_01/parchment_01.vmdl" );
	}

}
