using Sandbox;
using System;
using System.Linq;

namespace Sandbox;

partial class PuzzlePiece : Prop
{
	[Net]
	public int x { get; set; } = 0;
	[Net]
	public int y { get; set; } = 0;

	[Net]
	private bool Generated { get; set; } = false;

	/// <summary>
	/// Called when the entity is first created 
	/// </summary>
	public override void Spawn()
	{
		base.Spawn();
		Log.Error( "spawned" );
	}

	public PuzzlePiece() : base() { }

	public PuzzlePiece(Vector3 pos ) : base()
	{
		this.Position = pos;
	}

	[Event.Tick]
	void Tick()
	{

	}

	public void GenerateClient()
	{
		//this.SetModel( "models/sbox_props/watermelon/watermelon.vmdl" );

		this.Model = ExplorerGame.Game.PieceModel;
		Log.Error( "model: " + ExplorerGame.Game.PieceModel.ToString() );
		//SetupPhysicsFromModel( PhysicsMotionType.Dynamic );
		Vector3 dimensions = new Vector3( ExplorerGame.Game.PieceSize / 2, ExplorerGame.Game.PieceSize / 2, ExplorerGame.Game.PieceThickness / 2 );
		SetupPhysicsFromOBB( PhysicsMotionType.Dynamic, -dimensions, dimensions );
		PhysicsEnabled = true;
		UsePhysicsCollision = true;
	}

	public void GenerateServer()
	{
		//this.SetModel( "models/sbox_props/watermelon/watermelon.vmdl" );
		Log.Error( "server" );
		Vector3 dimensions = new Vector3( ExplorerGame.Game.PieceSize / 2, ExplorerGame.Game.PieceSize / 2, ExplorerGame.Game.PieceThickness / 2 );
		SetupPhysicsFromOBB( PhysicsMotionType.Dynamic, -dimensions, dimensions );
		PhysicsEnabled = true;
		UsePhysicsCollision = true;
	}


}
