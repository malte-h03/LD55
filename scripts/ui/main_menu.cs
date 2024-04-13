using Godot;
using System;

public partial class main_menu : Control
{
	// Called when the node enters the scene tree for the first time.

	scene_manager sceneManager;
	[Export] BaseButton startButton;
	[Export] BaseButton optionsButton;
	[Export] BaseButton ExitButton;

	[Export] PackedScene startScene;
	public override void _Ready()
	{
		sceneManager = GetTree().Root.GetNode<scene_manager>("SceneManager");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_start_game_pressed()
	{
		sceneManager.GotoScene(startScene);
	}
}
