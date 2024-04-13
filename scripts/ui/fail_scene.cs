using Godot;

public partial class fail_scene : VBoxContainer
{
	// Called when the node enters the scene tree for the first time.
	[Export] PackedScene startScene;
	[Export] PackedScene menuScene;

	scene_manager SceneManager;

	public override void _Ready()
	{
		SceneManager = GetTree().Root.GetNode<scene_manager>("SceneManager");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_try_again_pressed()
	{
		SceneManager.GotoScene(startScene);
	}

	public void _on_menu_pressed()
	{
		SceneManager.GotoScene(menuScene);
	}
}
