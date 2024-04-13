using Godot;
using System;

public partial class player_camera : Node2D
{
	player playerRef;
	float smoothingFactor = 0.9f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		playerRef = (player) GetTree().GetFirstNodeInGroup("Player");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GlobalPosition = GlobalPosition * smoothingFactor + playerRef.GlobalPosition * (1 - smoothingFactor);
	}
}
