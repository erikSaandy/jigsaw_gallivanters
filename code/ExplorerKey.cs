using Sandbox;
using SandboxEditor;

/// <summary>
/// This entity defines the spawn point of the player in first person shooter gamemodes.
/// </summary>

[Model]
[SupportsSolid]
[Library( "explorer_key" ), HammerEntity]
[Title( "Key" ), Category( "Key" ), Icon( "place" )]
[EditorModel( "models/key_01/key_01.vmdl" )]
public class ExplorerKey : ModelEntity
{

	[Property( Title = "Door To Open" )]
	public string DoorToOpen { get; set; } = null;

	public override void Spawn()
	{

		base.Spawn();

		SetModel( "models/key_01/key_01.vmdl" );
		SetupPhysicsFromModel( PhysicsMotionType.Dynamic );
		EnableAllCollisions = true;
		EnableTouch = true;
		EnableTraceAndQueries = true;

		Tags.Add( "solid" );

		Transmit = TransmitType.Always;
	}

	public override void ClientSpawn()
	{
		base.ClientSpawn();

		SetModel( "models/key_01/key_01.vmdl" );
	}

	public bool OnUse( Entity user )
	{
		return true;
	}

	public bool IsUsable( Entity user )
	{
		return true;
	}

}
