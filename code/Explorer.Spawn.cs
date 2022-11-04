using Sandbox;
using Saandy;

partial class Explorer
{

	public VertexBuffer vb;

	[Net, Predicted]
	public PuzzlePiece Current { get; set; } = null;


	/// <summary>
	/// This should be called somewhere in your player's tick to allow them to use entities
	/// </summary>
	protected void TickPlayerSpawn()
	{


		//// This is serverside only
		if ( !Host.IsServer ) return;


		/// TODO: PREDICTION IN PUZZLEPIECE PROP. GENERATE MESH AND SHIT IN OUTSIDE LOOP (PUZZLEGENERATOR etc)

		// Turn prediction off
		using ( Prediction.Off() )
		{
			if ( Input.Pressed( InputButton.Flashlight ) )
			{
				SpawnServer();
			}
		}
	}

	private void SpawnServer()
	{

		var tr = Trace.Ray( EyePosition, EyePosition + (EyeRotation.Forward * 512) )
				.Ignore( this )
				.Size( 10 )
				.Run();


		if ( tr.Hit )
		{
			Vector3 spawnPos = tr.EndPosition + Vector3.Up * 32;
			DebugOverlay.Line( EyePosition, tr.EndPosition, 5 );
			DebugOverlay.Line( tr.EndPosition, spawnPos, 5 );

			var ent = new PuzzlePiece( spawnPos );
			ent.GenerateServer();
			SpawnClient( ent );

		}

	}

	[ClientRpc]
	private void SpawnClient( PuzzlePiece ent )
	{
		ent.GenerateClient();
	}


}
