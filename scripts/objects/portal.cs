using Godot;
using System;

public partial class portal : Node2D
{
	// Called when the node enters the scene tree for the first time.
	scene_manager sceneManager;
	[Export] float suckingPower;
	[Export] PackedScene failScene;

	CharacterBody2D playerRef;
	bool isSucking = false;
	public override void _Ready()
	{
		sceneManager = GetTree().Root.GetNode<scene_manager>("SceneManager");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (isSucking)
		{
			GD.Print("Sucking");
			float distance = playerRef.GlobalPosition.DistanceTo(GlobalPosition);
			playerRef.GlobalPosition -= (playerRef.GlobalPosition - GlobalPosition).Normalized() * suckingPower; // * Mathf.Max(250.0f - distance, 0.0f);
		}
	}

	public void _on_area_2d_body_entered(Node body)
	{
		if (body.IsInGroup("Player"))
		{
			sceneManager.GotoScene(failScene);
		}
	}

	public void _on_circle_area_body_entered(Node body)
	{
		isSucking = true;
		playerRef = (CharacterBody2D) body;
	}

	public void _on_circle_body_exited(Node body)
	{
		isSucking = false;
	}
}
