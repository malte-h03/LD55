using Godot;
using System;

public partial class player_wobble : Sprite2D
{
	[Export] CharacterBody2D root;

	[Export] Texture2D back;
	[Export] Texture2D front;
	[Export] Texture2D side;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	double time = 0;
	double realtime = 0;
	public override void _Process(double delta)
	{

		if (root.Velocity.Dot( Vector2.Up ) > 0.5f)
		{
			Texture = back;
		}
		else if (root.Velocity.Dot( Vector2.Down ) > 0.5f)
		{
			Texture = front;
		}
		else if (Mathf.Abs(root.Velocity.Dot( Vector2.Right )) > 0.5f)
		{
			Texture = side;
		}

		if (root.Velocity.Dot( Vector2.Left ) > 0.5f)
		{
			Scale = new Vector2(-1.0f, 1);
		}
		else
		{
			Scale = Vector2.One;
		}


		float isPlayerMoving = Mathf.Sign(root.Velocity.Length());
		Rotation = (float) Mathf.Sin(time) * 0.3f * isPlayerMoving;

		Position = new Vector2(0.0f, (float) Mathf.Abs(Mathf.Sin(time))) * 2.0f * isPlayerMoving;
		
		
		if(root.Velocity.Length() < 0.1f)
		{
			Frame = (int) Mathf.Floor((float) realtime * 4.0f) % (Hframes);
		}

		GD.Print(Frame);
		time += delta * root.Velocity.Length() * 0.1f;
		realtime += Mathf.Min(delta, 1.0);

		GD.Print(realtime);
	}
}
