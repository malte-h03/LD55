using Godot;

public partial class agent : RigidBody2D
{
	// Called when the node enters the scene tree for the first time.
	private float health = 100.0f;

	bool isGrabbing = false;
	float grabDistance = 0.0f;
	Vector2 movementVector = default;
	CharacterBody2D playerRef;

	Line2D hand;
	public override void _Ready()
	{
		hand = new();
		AddChild(hand);
		hand.AddPoint(Vector2.Zero, 0);
		hand.AddPoint(Vector2.Zero, 1);
	}

	public override void _Process(double delta)
	{
		if (isGrabbing)
		{
			float newDistance = playerRef.GlobalPosition.DistanceTo(GlobalPosition);
			if (playerRef.GlobalPosition.DistanceTo(GlobalPosition) > grabDistance)
			{
				Vector2 compensation = (playerRef.GlobalPosition - GlobalPosition).Normalized() * (newDistance - grabDistance);
				playerRef.GlobalPosition -= compensation;
			}

			hand.SetPointPosition(1, playerRef.GlobalPosition - GlobalPosition);

			GlobalPosition -= movementVector;
		}

	
	}

    public override void _Input(InputEvent @event)
    {
        if (Input.IsKeyPressed(Key.K))
		{
			Kill();
		}
    }

    public override void _PhysicsProcess(double delta)
    {
        
    }

	public void Kill()
	{
		QueueFree();
	}

    public void _on_area_2d_body_entered(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			playerRef = (CharacterBody2D) body;
			isGrabbing = true;
			grabDistance = GlobalPosition.DistanceTo(body.GlobalPosition);

			var allPortals = GetTree().GetNodesInGroup("Portal");

			float distance = 9999999999.0f;
			Node2D activePortal = default;
			foreach(Node2D portal in allPortals)
			{
				float portalDistance = portal.GlobalPosition.DistanceTo(GlobalPosition);
				if (portalDistance < distance)
				{
					distance = portalDistance;
					activePortal = portal;
				}
			}
			movementVector = (GlobalPosition - activePortal.GlobalPosition).Normalized();
		}
	}
}
