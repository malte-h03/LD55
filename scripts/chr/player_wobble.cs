using Godot;
using System;

public partial class player_wobble : Sprite2D
{
	[Export] CharacterBody2D root;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	double time = 0;
	public override void _Process(double delta)
	{
		Rotation = (float) Mathf.Sin(time) * 0.3f;

		Position = new Vector2(0.0f , (float) Mathf.Abs(Mathf.Sin(time))) * 2.0f;
		time += delta * root.Velocity.Length() * 0.1f;
	}
}
