using System.Threading.Tasks;
using Godot;

public partial class main_menu : Control
{

	scene_manager sceneManager;
	[Export] BaseButton startButton;
	[Export] BaseButton optionsButton;
	[Export] BaseButton ExitButton;

	[Export] ColorRect blackScreen;
	[Export] AudioStreamPlayer startGammeSound;

	[Export] PackedScene startScene;
	public override void _Ready()
	{
		sceneManager = GetTree().Root.GetNode<scene_manager>("SceneManager");
	}

	public async void _on_start_game_pressed()
	{

		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(blackScreen, "color", new Color(0, 0, 0, 1), 1);
		startGammeSound.Play(0);
		
		await Task.Delay(1000);
		sceneManager.GotoScene(startScene);
	}

	public void _on_exit_pressed()
	{
		GetTree().Quit();
	}
}
