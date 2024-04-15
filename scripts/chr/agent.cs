using System;
using System.Linq;
using System.Threading.Tasks;
using Godot;

public partial class agent : RigidBody2D
{
	// Called when the node enters the scene tree for the first time.
	[Export] private float totalHealth = 100.0f;
	private float health = 100.0f;
	[Export] private float movementSpeed = 100.0f;
	[Export] private float grabPower = 0.2f;
	[Export] private float maxVelocity = 20.0f;

	[Export] private Area2D buddyArea; // = 20.0f;

	[Export] private AudioStreamPlayer enemyDies;

	[Export] private Sprite2D handHand;
	[Export] private Texture2D handOpen;
	[Export] private Texture2D handGrabbed;
	[Export] private Color armColor;

	[Export] AudioStreamPlayer grabSound;

	public bool isGrabbing = false;
	bool letGo = false;

	float grabDistance = 0.0f;
	Vector2 movementVector = default;
	player playerRef;

	wave_manager WaveManager;

	Line2D hand;
	RandomNumberGenerator rng;
	Vector2 noiseOffset;

	public Vector2 GetHealth()
	{
		return new Vector2(health, totalHealth);
	}

	public override void _Ready()
	{
		health = totalHealth;
		playerRef = (player) GetTree().GetFirstNodeInGroup("Player");

		hand = new();
		hand.DefaultColor = armColor;
		AddChild(hand);
		rng = new();

		noiseOffset = new Vector2(rng.RandfRange(-2, 2), rng.RandfRange(-4, 4));
		WaveManager = GetTree().Root.GetNode<wave_manager>("WaveManager");
	}

	public override void _Process(double delta)
	{
		if (isGrabbing)
		{
			
			if (playerRef.isDashing || (playerRef.GlobalPosition - GlobalPosition).Length() > grabDistance * 3.0f)
			{
				hand.ClearPoints();//.SetPointPosition(1, Position);
				playerRef.dragForce = Vector2.Zero;
				isGrabbing = false;
				handHand.Position = new Vector2(7, -7);
				handHand.Texture = handOpen;
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

		handHand.GlobalRotation = 0;

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

	public async void Kill()
	{
		enemyDies.Play(0);
		WaveManager.EnemyDie(this, 1000);
		
		await Task.Delay(1000);

		GetParent().QueueFree();
	}

	

    public void _on_area_2d_body_entered(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			// playerRef = (player) body;
			if (!isGrabbing)
			{
				grabSound.Play(0);
			}
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

		
		hand.SetPointPosition(1, (playerRef.GlobalPosition - GlobalPosition + noiseOffset).Rotated(-GlobalRotation));

		handHand.Position = hand.GetPointPosition(1);
		handHand.Texture = handGrabbed;

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
