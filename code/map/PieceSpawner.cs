using Sandbox;
using SandboxEditor;

/// <summary>
/// This entity defines the spawn point of the player in first person shooter gamemodes.
/// </summary>

[Model]
[SupportsSolid]
[Library( "piece_spawner" ), HammerEntity]
[Title( "Piece Spawner" ), Category( "Jigsaw" ), Icon( "place" )]
[EditorModel( "models/jigsaw_spawn/jigsaw_spawn.vmdl" )]
public class PieceSpawner : Entity
{

	//[Property( Title = "Door To Open" )]
	//public string DoorToOpen { get; set; } = null;

	public override void Spawn()
	{

		Name = "PieceSpawner";
		base.Spawn();

	}

}
