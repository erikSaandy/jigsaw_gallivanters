
using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.IO;
using System.Threading.Tasks;

/// <summary>
/// This is your game class. This is an entity that is created serverside when
/// the game starts, and is replicated to the client. 
/// 
/// You can use this to create things like HUDs and declare which player class
/// to use for spawned players.
/// </summary>
[Library( "sandbox", Title = "Sandbox" )]
public partial class Game : Sandbox.Game
{
	public Game()
	{


		if ( IsServer )
		{
			// Create the HUD
			_ = new MinimalHudEntity();
		}

		//if ( IsClient )
		//{
		//	if ( !PuzzleGenerator.PuzzleImage.IsLoaded )
		//	{
		//		PuzzleGenerator.LoadMaterialsOnClient();
		//	}
		//}

		using (Prediction.Off())
		{
			_ = new PuzzleGenerator();

			PuzzleGenerator.Instance.GeneratePuzzle();
		}

	}

	public override void ClientJoined( Client cl )
	{
		base.ClientJoined( cl );
		var player = new Player();
		player.Respawn();

		cl.Pawn = player;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	public override void DoPlayerNoclip( Client player )
	{
		if ( player.Pawn is Player basePlayer )
		{
			if ( basePlayer.DevController is NoclipController )
			{
				Log.Info( "Noclip Mode Off" );
				basePlayer.DevController = null;
			}
			else
			{
				Log.Info( "Noclip Mode On" );
				basePlayer.DevController = new NoclipController();
			}
		}
	}
}
