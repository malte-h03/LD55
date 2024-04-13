using Godot;

public partial class main_menu : Control
{

	scene_manager sceneManager;
	[Export] BaseButton startButton;
	[Export] BaseButton optionsButton;
	[Export] BaseButton ExitButton;

	[Export] PackedScene startScene;
	public override void _Ready()
	{
		sceneManager = GetTree().Root.GetNode<scene_manager>("SceneManager");
	}

	public void _on_start_game_pressed()
	{
		sceneManager.GotoScene(startScene);
	}

	public void _on_exit_pressed()
	{
		GetTree().Quit();
	}
}
