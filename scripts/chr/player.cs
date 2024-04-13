using Godot;

public partial class player : CharacterBody2D
{
	[Export] public float movementSpeed = 300.0f;

	private wave_manager WaveManager;

    public override void _Ready()
    {
        WaveManager = GetTree().Root.GetNode<wave_manager>("WaveManager");
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("LEFT", "RIGHT", "UP", "DOWN");
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

		Velocity = velocity;
		MoveAndSlide();

		if(Input.IsActionJustPressed("ui_up"))
		{
			WaveManager.startWave();
		}
	}
}
