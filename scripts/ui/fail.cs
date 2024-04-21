using Godot;
using System;

public partial class fail : CanvasLayer
{

	[Export] Label summonText;
	[Export] ColorRect blackscreen;

	[Export] Button start;
	[Export] Button menu;

	[Export] Label scoreText;

	wave_manager WaveManager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		WaveManager = GetTree().Root.GetNode<wave_manager>("WaveManager");
		scoreText.Text = "Final Score: " + WaveManager.score;
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(summonText, "visible_ratio", 0, 1);
		tween.TweenProperty(summonText, "visible_ratio", 0.49, 1);
		tween.TweenProperty(summonText, "visible_ratio", 0.49, 1);
		tween.TweenProperty(summonText, "visible_ratio", 1.0, 1);
		tween.TweenProperty(summonText, "visible_ratio", 1.0, 0.5);
		tween.TweenProperty(blackscreen, "color", new Color(0, 0, 0, 0), 3);
		tween.Parallel().TweenProperty(start, "modulate", new Color(1, 1, 1, 1), 3);
		tween.Parallel().TweenProperty(menu, "modulate", new Color(1, 1, 1, 1), 3);
		tween.Parallel().TweenProperty(scoreText, "self_modulate", new Color(1, 1, 1, 1), 3);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
