using Godot;

public partial class agent : RigidBody2D
{
	// Called when the node enters the scene tree for the first time.
	private float health = 100.0f;

	bool isGrabbing = false;
	bool letGo = false;

	float grabDistance = 0.0f;
	Vector2 movementVector = default;
	player playerRef;

	wave_manager WaveManager;

	Line2D hand;
	public override void _Ready()
	{
		hand = new();
		AddChild(hand);

		WaveManager = GetTree().Root.GetNode<wave_manager>("WaveManager");
	}

	public override void _Process(double delta)
	{
		if (isGrabbing)
		{
			grabbingLoop();
			// if (!playerRef.isDashing)
			// {
			// 	float newDistance = playerRef.GlobalPosition.DistanceTo(GlobalPosition);
			// 	if (playerRef.GlobalPosition.DistanceTo(GlobalPosition) > grabDistance)
			// 	{
			// 		Vector2 compensation = (playerRef.GlobalPosition - GlobalPosition).Normalized() * (newDistance - grabDistance);
			// 		playerRef.GlobalPosition -= compensation;
			// 	}

			// 	hand.SetPointPosition(1, playerRef.GlobalPosition - GlobalPosition);

			// 	GlobalPosition -= movementVector;
			// }
		}

		if (playerRef.isDashing && isGrabbing)
		{
			hand.ClearPoints();//.SetPointPosition(1, Position);
			isGrabbing = false;

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
		WaveManager.EnemyDie(this, 1000);
		QueueFree();
	}

    public void _on_area_2d_body_entered(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			playerRef = (player) body;
			isGrabbing = true;

			grabbingBehavior();
		}
	}

	private void grabbingBehavior()
	{
		hand.AddPoint(Vector2.Zero, 0);
		hand.AddPoint(Vector2.Zero, 1);
		
		grabDistance = GlobalPosition.DistanceTo(playerRef.GlobalPosition);
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

	private void grabbingLoop()
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
