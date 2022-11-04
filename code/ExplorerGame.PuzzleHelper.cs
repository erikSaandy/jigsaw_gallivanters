using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

public partial class ExplorerGame : Sandbox.Game
{

	public Model PieceModel { get; set; } = null;

	public readonly float PieceSize = 64;

	public readonly float PieceThickness = 8;

	public Material PieceMaterial { get; set; } = null;

	public void PuzzleHelperInit()
	{

		Vector3 pieceDimensions = new Vector3( PieceSize, PieceSize, PieceThickness );

		VertexBuffer vb = new VertexBuffer();
		vb.Init( true );
		vb.Clear();

		vb.AddCube( Vector2.Zero, pieceDimensions, Rotation.Identity, Color.Blue );

		PieceMaterial = Material.Load( "materials/0_dirt.vmat" );
		Mesh m = new Mesh( PieceMaterial );
		//m.SetBounds( -Vector2.One * 32, Vector2.One * 32 );
		m.CreateBuffers( vb, true );

		// // //

		Log.Error( m.VertexCount );

		// // //
		PieceModel = new ModelBuilder()
		.AddMesh( m )
		.AddCollisionBox( PieceSize / 2 )
		.WithMass( 50 )
		.WithSurface( "wood" )
		.Create();

		Log.Error( "generated model." );
	}

}
