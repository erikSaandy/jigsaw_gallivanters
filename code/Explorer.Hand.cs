using Sandbox;
using System;

partial class Explorer
{

	[Net, Local]
	public Prop PropCurrent { get; set; } = null;

	[Net, Local]
	public bool HoldingProp { get; set; } = false;

	[Net, Local]
	public float DistanceToGrabPoint { get; set; } = 0;

	//Offset of grab from the center of the object.
	[Net, Local]
	public Vector3 GrabOffset { get; set; } = 0;

	private Vector3 InitialPropUp { get; set; } = 0;

	private Vector3 WantedPosition { get; set; } = 0;

	private Vector3 NewGrabOffset { get; set; } = 0;


	/// <summary>
	/// This should be called somewhere in your player's tick to allow them to use entities
	/// </summary>
	protected void TickPlayerHand()
	{

		if ( PropCurrent == null ) return;

		// Turn prediction off
		using ( Prediction.Off() )
		{

			if ( Input.Pressed( InputButton.PrimaryAttack ) )
			{
				HoldingProp = true;
				InitialPropUp = PropCurrent.Rotation.Up;
			}

			if ( !HoldingProp ) return;

			if ( !Input.Down( InputButton.PrimaryAttack ) )
			{
				HoldingProp = false;
				PropCurrent.PhysicsBody.GravityScale = 1f;
				PropCurrent = null;

				return;
			}

		}

		if ( IsClient ) return;

		// DO PHYSICS
		if ( HoldingProp )
		{
			WantedPosition = EyePosition + (EyeRotation.Forward * DistanceToGrabPoint) - GrabOffset;
			Vector3 damping = PropCurrent.Velocity * 0.07f;

			if ( true || PropCurrent.Tags.Has( "light" ) )
			{
				PropCurrent.PhysicsBody.GravityScale = 0f;
				Vector3 dst = WantedPosition - PropCurrent.Position;

				PropCurrent.Velocity += Saandy.Math2d.Lerp( PropCurrent.Position, WantedPosition, 1 ) - PropCurrent.Position - damping;
			}

			else if ( PropCurrent.Tags.Has( "medium" ) )
			{
				PropCurrent.PhysicsBody.GravityScale = 0.25f;
				Vector3 dst = WantedPosition - PropCurrent.Position;

				PropCurrent.Velocity += Saandy.Math2d.Lerp( PropCurrent.Position, WantedPosition, (dst.Length / DistanceToGrabPoint) ) - PropCurrent.Position - damping;
			}

			else
			{
				PropCurrent.PhysicsBody.GravityScale = 1f;
				Vector3 dst = WantedPosition - PropCurrent.Position;

				PropCurrent.Velocity += Saandy.Math2d.Lerp( PropCurrent.Position, WantedPosition, (dst.Length / DistanceToGrabPoint) ) - PropCurrent.Position - damping;

			}

		}

		//// NEW PHYSICS
		//if ( HoldingProp )
		//{


		//	WantedPosition = EyePosition + (EyeRotation.Forward * DistanceToGrabPoint) + GrabOffset;

		//	Vector3 force = Vector3.Zero;

		//	Vector3 RotatedHitPoint = Vector3.Zero;

		//	////Rotation of grab with initial prop rotation
		//	//float GrabRotation = Saandy.Math2d.Angle3D( InitialPropUp, GrabOffset.Normal, Vector3.Cross( InitialPropUp, GrabOffset.Normal ) );

		//	////Rotation of grab with initial prop rotation

		//	Vector3 AxisOfRotation = Vector3.Cross( InitialPropUp, PropCurrent.Rotation.Up );
		//	float PropRotationOffset = Saandy.Math2d.Angle3D( InitialPropUp, PropCurrent.Rotation.Up, AxisOfRotation );
		//	Log.Error( PropRotationOffset );
		//	NewGrabOffset = Saandy.Math2d.RotateVector3D( GrabOffset, AxisOfRotation, MathX.DegreeToRadian( PropRotationOffset ) );

		//	DebugOverlay.Line( PropCurrent.Position, PropCurrent.Position + (NewGrabOffset), Color.Blue );


		//	WantedPosition = EyePosition + (EyeRotation.Forward * DistanceToGrabPoint);

		//	if ( true || PropCurrent.Tags.Has( "medium" ) )
		//	{
		//		PropCurrent.PhysicsBody.GravityScale = 0.5f;
		//		Vector3 RotatedGrabPoint = PropCurrent.Position + NewGrabOffset;
		//		force = WantedPosition - RotatedGrabPoint;
		//		DebugOverlay.Line( PropCurrent.Position + NewGrabOffset, RotatedGrabPoint + Vector3.Up * 32, Color.Red );

		//		DebugOverlay.Line( RotatedGrabPoint, WantedPosition, Color.Magenta );

		//		PropCurrent.PhysicsBody.ApplyForceAt( RotatedGrabPoint, force * 4000 );
		//	}

		//}
	}
}

