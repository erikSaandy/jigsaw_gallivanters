
using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.IO;
using System.Threading.Tasks;

//
// You don't need to put things in a namespace, but it doesn't hurt.
//
namespace MinimalExample
{

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

			if ( IsClient )
			{
				_ = new PuzzleGenerator();
				PuzzleGenerator.Instance.GeneratePuzzle();
			}

		}

		public override void ClientJoined( Client cl )
		{
			base.ClientJoined( cl );
			var player = new SandboxPlayer();
			player.Respawn();

			cl.Pawn = player;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		[ServerCmd( "spawn" )]
		public static void Spawn( string modelname )
		{
			var owner = ConsoleSystem.Caller?.Pawn;

			if ( ConsoleSystem.Caller == null )
				return;

			var tr = Trace.Ray( owner.EyePos, owner.EyePos + owner.EyeRot.Forward * 500 )
				.UseHitboxes()
				.Ignore( owner )
				.Run();

			var ent = new Prop();
			ent.Position = tr.EndPos;
			ent.Rotation = Rotation.From( new Angles( 0, owner.EyeRot.Angles().yaw, 0 ) ) * Rotation.FromAxis( Vector3.Up, 180 );
			ent.SetModel( modelname );
			ent.Position = tr.EndPos - Vector3.Up * ent.CollisionBounds.Mins.z;
		}

		[ServerCmd( "spawn_entity" )]
		public static void SpawnEntity( string entName )
		{
			var owner = ConsoleSystem.Caller.Pawn;

			if ( owner == null )
				return;

			var attribute = Library.GetAttribute( entName );

			if ( attribute == null || !attribute.Spawnable )
				return;

			var tr = Trace.Ray( owner.EyePos, owner.EyePos + owner.EyeRot.Forward * 200 )
				.UseHitboxes()
				.Ignore( owner )
				.Size( 2 )
				.Run();

			var ent = Library.Create<Entity>( entName );
			if ( ent is BaseCarriable && owner.Inventory != null )
			{
				if ( owner.Inventory.Add( ent, true ) )
					return;
			}

			ent.Position = tr.EndPos;
			ent.Rotation = Rotation.From( new Angles( 0, owner.EyeRot.Angles().yaw, 0 ) );

			//Log.Info( $"ent: {ent}" );
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
}
