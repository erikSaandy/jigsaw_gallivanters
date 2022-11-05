using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class ExplorerGame : Sandbox.Game
{

	//public Model PieceModel { get; set; } = null;

	public Model[] PieceModels { get; set; } = null;

	public Material BacksideMaterial { get; set; } = null;
	public Material PuzzleImageMaterial { get; set; } = null;
	public Texture PuzzleImageTexture { get; set; } = null;

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
		PuzzleImageMaterial.OverrideTexture("Color", t );
		BacksideMaterial = Material.Load( "materials/jigsaw/jigsaw_back/jigsaw_back.vmat" );
	}

}
