using System;
using System.Linq;
using Godot;

public partial class agent : RigidBody2D
{
	// Called when the node enters the scene tree for the first time.
	[Export] private float health = 100.0f;
	[Export] private float movementSpeed = 100.0f;
	[Export] private float grabPower = 0.2f;
	[Export] private float maxVelocity = 20.0f;

	[Export] private Area2D buddyArea; // = 20.0f;

	public bool isGrabbing = false;
	bool letGo = false;

	float grabDistance = 0.0f;
	Vector2 movementVector = default;
	player playerRef;

	wave_manager WaveManager;

	Line2D hand;
	public override void _Ready()
	{
		playerRef = (player) GetTree().GetFirstNodeInGroup("Player");

		hand = new();
		AddChild(hand);

		WaveManager = GetTree().Root.GetNode<wave_manager>("WaveManager");
	}

	public override void _Process(double delta)
	{
		if (isGrabbing)
		{
			
			if (playerRef.isDashing)
			{
				hand.ClearPoints();//.SetPointPosition(1, Position);
				playerRef.dragForce = Vector2.Zero;
				isGrabbing = false;
			}
			else
			{
				grabbingLoop();
			}
		}
		else
		{
			if (!playerRef.isDashing)
			{
				LinearVelocity += (playerRef.GlobalPosition - GlobalPosition).Normalized() * movementSpeed * 0.1f;
			}
		}
		var bodiesInArea = buddyArea.GetOverlappingBodies();

		Vector2 awayForce = new();
		float total = 0;
		foreach( Node2D body in bodiesInArea)
		{
			if (body.IsInGroup("Enemy"))
			{
				awayForce += (body.GlobalPosition - GlobalPosition).Normalized();
				total++;
			}
		}
		awayForce /= total;
		// LinearVelocity = awayForce.Normalized() * 0.1f;


		if(LinearVelocity.Length() > maxVelocity)
		{
			LinearVelocity = LinearVelocity.Normalized() * maxVelocity * 0.98f;
		}

		if((playerRef.GlobalPosition - GlobalPosition).Length() > grabDistance * 3.0f)
		{
			isGrabbing = false;
		}
	}

	public void TakeDamage(int amount)
	{
		GD.Print("Oouth!!");
		health -= amount;

		if (health == 0)
		{
			Kill();
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
			// playerRef = (player) body;
			isGrabbing = true;

			grabbingBehavior();
		}
	}

	private void grabbingBehavior()
	{
		if (hand.Points.Length < 2)
		{
			hand.AddPoint(Vector2.Zero, 0);
			hand.AddPoint(Vector2.Zero, 1);
			hand.Width = 5;
		}
		
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
		var allAgents = GetTree().GetNodesInGroup("Enemy");
		float totalGrabs = 0;

		foreach (agent enemy in allAgents)
		{
			if (enemy.isGrabbing)
			{
				totalGrabs++;
			}
		}

		float newDistance = playerRef.GlobalPosition.DistanceTo(GlobalPosition);
		if (playerRef.GlobalPosition.DistanceTo(GlobalPosition) > grabDistance)
		{
			Vector2 compensation = (playerRef.GlobalPosition - GlobalPosition).Normalized() * (newDistance - grabDistance);
			playerRef.dragForce = -compensation * grabPower * totalGrabs;
		}

		hand.SetPointPosition(1, (playerRef.GlobalPosition - GlobalPosition).Rotated(-GlobalRotation));

		LinearVelocity = -movementVector * MathF.Min(grabPower * totalGrabs * 100, movementSpeed);
		// constrain agent
		if (playerRef.GlobalPosition.DistanceTo(GlobalPosition) > grabDistance)
		{
			Vector2 compensation = (playerRef.GlobalPosition - GlobalPosition).Normalized() * (newDistance - grabDistance);
			// playerRef.Velocity -= compensation * grabPower * 10.0f;

			LinearVelocity += compensation;
		}
		
	}

}
