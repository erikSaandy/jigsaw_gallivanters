using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// This is your game class. This is an entity that is created serverside when
/// the game starts, and is replicated to the client. 
/// 
/// You can use this to create things like HUDs and declare which player class
/// to use for spawned players.
/// </summary>
public partial class ExplorerGame : Sandbox.Game
{

	public static ExplorerGame Game => Current as ExplorerGame;

	public ExplorerHud Hud;

	public ExplorerGame()
	{
		if ( IsClient )
		{
			// Create the HUD
			Hud = new ExplorerHud();
			Hud.HudInit();

			PuzzleHelperInit();
		}
	
		if( IsServer )
		{
			DoorHelperInit();
			NoteHelperInit();

		}

	}

	[Event.Hotload]
	public void HotLoad()
	{
		if ( !IsClient ) return;
		Hud?.Delete();
		Hud = new ExplorerHud();
	}

	/// <summary>
	/// A client has joined the server. Make them a pawn to play with
	/// </summary>
	public override void ClientJoined( Client client )
	{
		base.ClientJoined( client );

		// Create a pawn for this client to play with
		var player = new Explorer( client );
		player.Respawn();
		
		client.Pawn = player;

		//// Get all of the spawnpoints
		//var spawnpoints = Entity.All.OfType<SpawnPoint>();

		//// chose a random one
		//var randomSpawnPoint = spawnpoints.OrderBy( x => Guid.NewGuid() ).FirstOrDefault();

		//// if it exists, place the pawn there
		//if ( randomSpawnPoint != null )
		//{
		//	var tx = randomSpawnPoint.Transform;
		//	tx.Position = tx.Position + Vector3.Up * 50.0f; // raise it up
		//	player.Transform = tx;
		//}
	}
}
