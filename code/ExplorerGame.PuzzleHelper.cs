using Saandy;
using Sandbox;
using System.Collections.Generic;
using System.Linq;

public partial class ExplorerGame : Sandbox.Game
{

	// All piece models (CLIENT)
	public Model[] PieceModels { get; set; } = null;


	// All PuzzlePiece entities (NETWORKED)
	[Net] public PuzzlePiece[] PieceEntities { get; set; } = null; 

	public Material BacksideMaterial { get; private set; } = null;
	public Material PuzzleImageMaterial { get; private set; } = null;
	public Texture PuzzleImageTexture { get; private set; } = null;

	[Net, Predicted]
	public int PieceCountX { get; set; } = 0;

	[Net, Predicted]
	public int PieceCountY { get; set; } = 0;

	public void PuzzleHelperInit()
	{

		if ( IsClient )
		{
			SetNewPuzzleTexture( Texture.Load( FileSystem.Mounted, "textures/sbox.png" ) );
			GeneratePuzzle();

			if(PieceEntities != null)
			{
				LateSetupPiecesClient();
			}

		}

		if( IsServer )
		{
			Texture t = Texture.Load( FileSystem.Mounted, "textures/sbox.png" );
			GetDimensions( t, out int pc );
			Log.Error( "DIM: " + PieceCountX + "," + PieceCountY );
		}

	}

	public void SetNewPuzzleTexture( Texture t )
	{
		PuzzleImageTexture = t;
		PuzzleImageMaterial = Material.Load( "materials/jigsaw/default_image/jigsaw_default.vmat" );
		PuzzleImageMaterial.Set("Color", t );
		BacksideMaterial = Material.Load( "materials/jigsaw/jigsaw_back/jigsaw_back.vmat" );
	}

	/// <summary>
	/// Spawn PuzzlePiece entities. (Load models BEFORE this.)
	/// </summary>
	public void SpawnPieces()
	{
		int count = PieceCountX * PieceCountY;

		PieceEntities = new PuzzlePiece[count];

		for ( int i = 0; i < count; i++ )
		{
			// Get x and y of piece.
			Math2d.FlattenedArrayIndex( i, ExplorerGame.Game.PieceCountX, out int x, out int y );

			// Generate piece.
			var ent = new PuzzlePiece( x, y );
			PieceEntities[i] = ent;
		}

		SetupPiecesServer();
	}

	private void SetupPiecesServer()
	{
		int count = PieceCountX * PieceCountY;

		for(int i = 0; i < count; i++ )
		{
			PieceEntities[i].GenerateServer();
			SetupPieceClient( PieceEntities[i] );
		}

		PlacePieces();

	}

	[ClientRpc]
	private void SetupPieceClient( PuzzlePiece ent )
	{
		ent.GenerateClient();
	}

	/// <summary>
	/// Place pieces according to map spawn points.
	/// </summary>
	/// <param name="ent"></param>
	private void PlacePieces()
	{
		IEnumerable<Entity> Spawners = FindAllByName( "PieceSpawner" );

		foreach(PuzzlePiece p in PieceEntities )
		{
			int id = Rand.Int( 0, Spawners.Count<Entity>() - 1 );
			p.Position = Spawners.ElementAt<Entity>( id ).Position + (Vector3.Up * 16);
		}
	}

	/// <summary>
	/// Setup spawned in puzzle pieces to use client-loaded models.
	/// This is for Clients joining in the middle of the game.
	/// </summary>
	private void LateSetupPiecesClient()
	{
		foreach ( PuzzlePiece p in PieceEntities )
		{
			SetupPieceClient( p );
		}
	}

}
