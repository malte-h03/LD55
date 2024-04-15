using Godot;

public partial class player : CharacterBody2D
{
	[Export] public float movementSpeed = 300.0f;
	[Export] public float dashLength = 1.0f;
	[Export] public double dashTime = 1.0;
	[Export] public Timer dashTimer;

	[Export] AudioStreamPlayer walkSound;

	Timer slashTimer;

	private wave_manager WaveManager;

	public bool isDashing = false;
	public bool isSlashing = false;

	Vector2 direction;

	public Vector2 dragForce;
	public float dragForceMultiplier = 1.0f;


	public bool dashReady = false;

	public override void _Ready()
	{
		walkSound.Playing = true;
		WaveManager = GetTree().Root.GetNode<wave_manager>("WaveManager");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		direction = Input.GetVector("LEFT", "RIGHT", "UP", "DOWN");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * movementSpeed;
			velocity.Y = direction.Y * movementSpeed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, movementSpeed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, movementSpeed);
		}

		Velocity = velocity + dragForce * dragForceMultiplier;
		MoveAndSlide();

		if (Velocity.Length() > 5)
		{
			walkSound.StreamPaused = false;
		}
		else
		{
			walkSound.StreamPaused = true;
		}
		// if(Input.IsActionJustPressed("ui_up"))
		// {
		// 	WaveManager.startWave();
		// }
	}

	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("DASH") && dashReady)
		{
			GD.Print("Dash ready");
			dashReady = false;
			isDashing = true;

			Tween tween = GetTree().CreateTween();
			tween.SetEase(Tween.EaseType.Out);
			tween.TweenProperty(this, "global_position", GlobalPosition + direction.Normalized() * dashLength, dashTime).SetTrans(Tween.TransitionType.Cubic);
			tween.TweenProperty(this, "isDashing", false, 0);
			dashTimer.Start();
		}
	}

	public void _on_dash_timer_timeout()
	{
		
		dashReady = true;
	}
}
