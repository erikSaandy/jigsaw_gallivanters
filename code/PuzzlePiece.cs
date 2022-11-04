using Sandbox;
using System;
using System.Linq;
using Saandy;

namespace Sandbox;

partial class PuzzlePiece : Prop
{
	[Net]
	public int Index { get; private set; } = 0;

	[Net]
	public int X { get; set; } = 0;
	[Net]
	public int Y { get; set; } = 0;

	/// <summary>
	/// Called when the entity is first created 
	/// </summary>
	public override void Spawn()
	{
		base.Spawn();
		Log.Error( "spawned" );
	}

	public PuzzlePiece() : base() { }

	public PuzzlePiece(int x, int y) : base()
	{
		this.X = x;
		this.Y = y;
		Index = Math2d.ArrayIndex( x, y, ExplorerGame.Game.PieceCountX, ExplorerGame.Game.PieceCountY );
	}

	[Event.Tick]
	void Tick()
	{

	}

	/// <summary>
	/// Generate model mesh and physics.
	/// </summary>
	public void GenerateServer()
	{
		//this.SetModel( "models/sbox_props/watermelon/watermelon.vmdl" );
		Vector3 dimensions = new Vector3( ExplorerGame.PieceScale / 2, ExplorerGame.PieceScale / 2, (ExplorerGame.PieceScale * ExplorerGame.PieceThickness) / 2 );
		SetupPhysicsFromOBB( PhysicsMotionType.Dynamic, -dimensions, dimensions );
		PhysicsEnabled = true;
		UsePhysicsCollision = true;

		GenerateClient();
	}


	[ClientRpc]
	public void GenerateClient()
	{
		//this.SetModel( "models/sbox_props/watermelon/watermelon.vmdl" );

		Log.Error( Index );
		this.Model = ExplorerGame.Game.PieceModels[Index];
		//Log.Error( "model: " + ExplorerGame.Game.PieceModel.ToString() );
		//SetupPhysicsFromModel( PhysicsMotionType.Dynamic );
		Vector3 dimensions = new Vector3( ExplorerGame.PieceScale / 2, ExplorerGame.PieceScale / 2,  (ExplorerGame.PieceScale * ExplorerGame.PieceThickness) / 2 );
		SetupPhysicsFromOBB( PhysicsMotionType.Dynamic, -dimensions, dimensions );
		PhysicsEnabled = true;
		UsePhysicsCollision = true;
	}

}
