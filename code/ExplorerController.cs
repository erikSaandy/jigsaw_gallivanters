using System.Collections.Generic;

namespace Sandbox
{
	public class ExplorerController : WalkController
	{

		public ExplorerController() : base() {
			SprintSpeed = 130;
			DefaultSpeed = 80;
			WalkSpeed = 50;
			//StepSize = 110;
		}

		public override void CheckJumpButton() { return; }

	}
}
