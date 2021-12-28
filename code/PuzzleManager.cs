using System.Collections;
using System.Collections.Generic;
using Sandbox;
using System.Linq;

partial class PuzzleManager : Entity
{

	[Net, Predicted] public Material ImageMat { get; private set; }
	[Net, Predicted] public int PuzzleWidth { get; set; }
	[Net, Predicted] public int PuzzleHeight { get; set; }
	[Net, Predicted] public List<PuzzlePiece> Pieces { get; private set; }

    public PuzzleManager(int w, int h, string m_default, string texture) {

		Log.Info( "Puzzle Manager Instantiation start." );
		PuzzlePiece[] a = new PuzzlePiece[w * h];
		Pieces = a.ToList();

		PuzzleWidth = w;
        PuzzleHeight = h;
		Log.Info( "Puzzle Manager Instantiation mid." );
		Material m = Material.Load( m_default );
		ImageMat = m;
		Log.Info( m.Name );
		Log.Info( "Puzzle Manager Instantiation end." );
	}

    public void AddPiece(PuzzlePiece piece, int x, int y) {
		Log.Info( "Add piece" );
		Log.Info( Pieces.Count );
		Pieces[(y * PuzzleWidth) + x] = piece;
		//Pieces[(y * puzzleWidth) + x] = piece;
		//Pieces.Add( piece );
	}

    public PuzzlePiece GetPiece(int x, int y) {
        return Pieces[(y * PuzzleWidth) + x];
    }

}
