using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.IO;
using System.Threading.Tasks;

public partial class Game
{



	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		using (Prediction.Off())
		{

		}

	}


}
