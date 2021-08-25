using System.Collections;
using System.Collections.Generic;
using Sandbox;

partial class PuzzlePiece : Sandbox.ModelEntity
{

    [Net] public PuzzleManager Manager { get; private set; }

    public PuzzlePiece root;
    public override PuzzlePiece Root => GetRoot();
    public PuzzlePiece GetRoot() {
        if (root == null) { root = this; }
        return root;
    }

	public PuzzlePiece(PuzzleManager manager)
	{
		this.Manager = manager;
	}

    public int x, y;
    private static float connectionDistance = 0.2f;

    public bool connectedLeft, connectedRight, connectedTop, connectedBottom;

    public void CheckForConnections( bool connectPhysically = true ) { // on pickup

        if (GetRoot().FindNeighbor( connectPhysically )) { return; }

		int childCount = root.Children.Count;
		for ( int i = 0; i < childCount; i++ ) {       
            if ( (root.Children[i] as PuzzlePiece).FindNeighbor( connectPhysically )) {
                return;
            }
        }

    }

    private bool FindNeighbor( bool connectPhysically = true ) {

        float dst = 0;
        PuzzlePiece other;

        if (!connectedLeft && GetPieceInDirection(-1, 0, out other)) { // left

            DebugOverlay.Line(Position - this.Right(), other.Position, Color.Red);
            dst = (Position - this.Right() - other.Position).Length;

            if (dst < connectionDistance) {
                if (connectPhysically) { ConnectToPiece(-1, 0); return true; }
                else if (root != other.root) { return false; }

                connectedLeft = true;
                Manager.GetPiece(x - 1, y).connectedRight = true;

            }
        }

        if (!connectedTop && GetPieceInDirection(0, 1, out other)) { // top

			DebugOverlay.Line( Position + this.Up(), other.Position, Color.Green );
            dst = (Position + this.Up() - other.Position).Length;

            if (dst < connectionDistance) {
                if (connectPhysically) { ConnectToPiece(0, 1); return true; }
                else if (root != other.root) { return false; }

                connectedTop = true;
                Manager.GetPiece(x, y + 1).connectedBottom = true;
            }
        }

        if (!connectedRight && GetPieceInDirection(1, 0, out other)) { // right

			DebugOverlay.Line( Position + this.Right(), other.Position, Color.Blue );
            dst = (Position + this.Right() - other.Position ).Length;

            if (dst < connectionDistance) {

                if (connectPhysically) { ConnectToPiece(1, 0); return true; }
                else if (root != other.root) { return false; }

                connectedRight = true;
                Manager.GetPiece(x + 1, y).connectedLeft = true;
            }
        }

        if (!connectedBottom && GetPieceInDirection(0, -1, out other)) { // bottom

			DebugOverlay.Line( Position - this.Up(), other.Position, Color.Yellow );
			dst = (Position - this.Up() - other.Position).Length;
            if (dst < connectionDistance) {

                if (connectPhysically) { ConnectToPiece(0, -1); return true; }
                else if(root != other.root) { return false; }

                connectedBottom = true;
                Manager.GetPiece(x, y - 1).connectedTop = true;

            }
        }

        return false;

    }

    private bool GetPieceInDirection(int dirX, int dirY, out PuzzlePiece piece) {
        if( dirX != 0 &&
            x + dirX > 0 && 
            x + dirX < Manager.puzzleWidth) {
            piece = Manager.GetPiece(x + dirX, y);
            return true;
        }

        if( dirY != 0 &&
            y + dirY > 0 &&
            y + dirY < Manager.puzzleHeight) {
            piece = Manager.GetPiece(x, y + dirY);
            return true;
        }

        piece = null;
        return false;
    }

    private void ConnectToPiece(int dirX, int dirY) {

		//(root.Parent as Player) TODO
        //GameObject.FindObjectOfType<Player>().DropPickup();

        PuzzlePiece newRoot = Manager.GetPiece(x + dirX, y + dirY).root;

		root.PhysicsEnabled = false;
        root.Parent = newRoot;
		root.LocalRotation = Rotation.Identity;
        root.LocalPosition = new Vector3(root.x - newRoot.x, root.y - newRoot.y);

        foreach (Entity child in root.Children) {
            child.Parent = newRoot;
            (child as PuzzlePiece).root = newRoot;
        }

        root.root = newRoot;

        CheckForConnections(false);

    }

}
