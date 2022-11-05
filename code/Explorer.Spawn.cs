using Sandbox;
using Saandy;

partial class Explorer
{

	/// <summary>
	/// This should be called somewhere in your player's tick to allow them to use entities
	/// </summary>
	protected void TickPlayerSpawn()
	{


		//// This is serverside only
		if ( !Host.IsServer ) return;

		// Turn prediction off
		using ( Prediction.Off() )
		{
			if ( Input.Pressed( InputButton.Flashlight ) )
			{
				SpawnPiecesServer();
			}
		}
	}

	private void SpawnPiecesServer()
	{
		Log.Error( ExplorerGame.Game.PieceCountX + ", " + ExplorerGame.Game.PieceCountY );

		//TODO: add piecemodels to specific index in array instead of in order. (they're generated out of order.)

		int l = ExplorerGame.Game.PieceCountX * ExplorerGame.Game.PieceCountY;
		for ( int i = 0; i < l; i++ )
		{
			Math2d.FlattenedArrayIndex( i, ExplorerGame.Game.PieceCountX, out int x, out int y ); 
			var ent = new PuzzlePiece( x, y );
			ent.GenerateServer();

			float spacing = 2;
			ent.Position = new Vector3( ent.X * ExplorerGame.PieceScale + (spacing * ent.X), ent.Y * ExplorerGame.PieceScale + (spacing * ent.Y), 64 );
			SpawnPieceClient(ent);
		}

		//for ( int x = 0; x < ExplorerGame.Game.PieceCountX; x++ )
		//{
		//	for ( int y = 0; y < ExplorerGame.Game.PieceCountY; y++ )
		//	{
		//		var ent = new PuzzlePiece(x, y);
		//		ent.GenerateServer();
		//		ent.Position = new Vector3( ent.X * ExplorerGame.PieceScale + (8 * ent.X), ent.Y * ExplorerGame.PieceScale + (8 * ent.Y), 64 );
		//		ent.Rotation = Rotation.RotateAroundAxis( Vector3.Up, 270 );
		//		SpawnPieceClient(ent);

		//	}
		//}

	}

	[ClientRpc]
	private void SpawnPieceClient( PuzzlePiece ent )
	{
		ent.GenerateClient();
	}


}
