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

		for ( int x = 0; x < ExplorerGame.Game.PieceCountX; x++ )
		{
			for ( int y = 0; y < ExplorerGame.Game.PieceCountY; y++ )
			{
				var ent = new PuzzlePiece(x, y);
				ent.GenerateServer();
				ent.Position = new Vector3( ent.X * ExplorerGame.PieceScale, ent.Y * ExplorerGame.PieceScale, 64 );
				ent.Rotation = Rotation.RotateAroundAxis( Vector3.Up, 180 );
				SpawnPieceClient(ent);

			}
		}

	}

	[ClientRpc]
	private void SpawnPieceClient( PuzzlePiece ent )
	{
		ent.GenerateClient();
	}


}
