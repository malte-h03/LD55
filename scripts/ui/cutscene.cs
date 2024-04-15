using Godot;
using System;
using System.Threading.Tasks;

public partial class cutscene : CanvasLayer
{

	[Export] ColorRect blackScreen;
	[Export(PropertyHint.File)] string gameScene;
	[Export] AudioStreamPlayer buttonAudio;
	scene_manager SceneManager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SceneManager = GetTree().Root.GetNode<scene_manager>("SceneManager");
		Tween tween = GetTree().CreateTween();

		tween.TweenProperty(blackScreen, "color", new Color(0, 0, 0, 0), 1);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public async void _on_button_pressed()
	{
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(blackScreen, "color", new Color(0, 0, 0, 1), 0);
		buttonAudio.Play(0);

		await Task.Delay(1000);
		SceneManager.GotoScene(gameScene);
	}
}
