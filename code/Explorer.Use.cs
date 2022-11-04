using Sandbox;
using Saandy;

partial class Explorer
{
	public Entity LookingAt { get; set; } = null;

	public bool IsUseDisabled()
	{
		return ActiveChild is IUse use && use.IsUsable( this );
	}

	/// <summary>
	/// This should be called somewhere in your player's tick to allow them to use entities
	/// </summary>
	protected override void TickPlayerUse()
	{
		LookingAt = null;
		if ( FindCollectible( out TraceResult tr ) )
		{

			if ( !ExplorerGame.Game.IsNoteOpen() )
			{
				LookingAt = tr.Entity;
			}
		}
		else if( ExplorerGame.Game.IsNoteOpen() )
		{
			PlaySound( "note_close" );
			ExplorerGame.Game.CloseNote();
		}	

		// This is serverside only
		if ( !Host.IsServer ) return;


		// Turn prediction off
		using ( Prediction.Off() )
		{

			if ( HoldingProp ) return;

			PropCurrent = null;

			if ( Input.Pressed( InputButton.Use ) )
			{
				if(LookingAt != null)
				{
					if(LookingAt is ExplorerKey)
					{
						ExplorerGame.Game.KeyCount++;
						PlaySound( "pickup_key" );
						LookingAt.Delete();
						return;
					}
					else if ( LookingAt is ExplorerNote )
					{

						if ( (LookingAt as ExplorerNote).HasBeenOpened )
						{
							PlaySound( "note_open_simple" );
						}
						else
						{
							PlaySound( "note_open" );
						}

						(LookingAt as ExplorerNote).HasBeenOpened = true;
						ExplorerGame.Game.CurrentNoteText = (LookingAt as ExplorerNote).Text;
						ExplorerGame.Game.OpenNote();
						return;
					}

					return;
				}

				Using = FindUsable();

				if ( Using == null )
				{
					UseFail();
					return;
				}

				if(Using is DoorEntity)
				{
					DoorEntity door = Using as DoorEntity;

					if(door.Locked && ExplorerGame.Game.KeyCount > 0)
					{
						door.Unlock();
						ExplorerGame.Game.KeyCount--;
						return;
					}
				}
			}

			// Found physics object, movable by hand.
			if ( tr.Entity is Prop && (tr.Entity as Prop).PhysicsEnabled )
			{
				PropCurrent = tr.Entity as Prop;

				// Vector from player to hit point.
				DistanceToGrabPoint = EyePosition.Distance(tr.HitPosition);

				// Vector from entity center to hit
				GrabOffset = tr.HitPosition - tr.Entity.Position;

				//DebugOverlay.Line( EyePosition, EyePosition + (EyeRotation.Forward * DistanceToGrabPoint), 10 );
			}


			if ( !Input.Down( InputButton.Use ) )
			{
				StopUsing();
				return;
			}

			if ( !Using.IsValid() )
				return;

			// If we move too far away or something we should probably ClearUse()?

			//
			// If use returns true then we can keep using it
			//
			if ( Using is IUse use && use.OnUse( this ) )
				return;

			StopUsing();
		}
	}

	protected bool FindCollectible(out TraceResult tr)
	{

		tr = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * (85 * Scale) )
		.WithoutTags( "trigger" )
		.Radius( 2 )
		.Ignore( this )
		.Run();

		if(tr.Entity == null) { return false; }

		if (tr.Entity is ExplorerKey or ExplorerNote )
		{
			return true;
		}

		return false;
	}

	protected override Entity FindUsable()
	{
		if ( IsUseDisabled() )
			return null;

		// First try a direct 0 width line
		var tr = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * (85 * Scale) )
			.WithoutTags( "trigger" )
			.Ignore( this )
			.Run();

		// See if any of the parent entities are usable if we ain't.
		var ent = tr.Entity;
		while ( ent.IsValid() && !IsValidUseEntity( ent ) )
		{
			ent = ent.Parent;
		}


		// Nothing found, try a wider search
		if ( !IsValidUseEntity( ent ) )
		{
			tr = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * (85 * Scale) )
			.WithoutTags( "trigger" )
			.Radius( 2 )
			.Ignore( this )
			.Run();

			// See if any of the parent entities are usable if we ain't.
			ent = tr.Entity;
			while ( ent.IsValid() && !IsValidUseEntity( ent ) )
			{
				ent = ent.Parent;
			}
		}
		
		// Still no good? Bail.
		if ( !IsValidUseEntity( ent ) ) return null;

		return ent;
	}

	protected override void UseFail()
	{
		if ( IsUseDisabled() )
			return;

		base.UseFail();
	}


}
