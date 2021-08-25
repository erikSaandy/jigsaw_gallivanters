using System.Collections;
using System.Collections.Generic;
using Sandbox;

partial class PuzzleManager : Sandbox.Entity
{

	[Net] public Material ImageMat { get; private set; }
	public int puzzleWidth, puzzleHeight;

	[Net] public List<PuzzlePiece> Pieces { get; private set; }

    public PuzzleManager(int w, int h, Material imageMat) {
		Pieces = new List<PuzzlePiece>( );
		puzzleWidth = w;
        puzzleHeight = h;
		this.ImageMat = imageMat;
    }

    public void AddPiece(PuzzlePiece piece, int x, int y) {
		//Pieces[(y * puzzleWidth) + x] = piece;
		Pieces.Add( piece );
    }

    public PuzzlePiece GetPiece(int x, int y) {
        return Pieces[(y * puzzleWidth) + x];
    }

}
