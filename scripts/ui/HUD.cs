using Godot;
using System;

public partial class HUD : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	[Export] ProgressBar waveBar;
	[Export] Label countDown;

	int grabbersInWave;

	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void WaveStarted(int grabbersTotal)
	{
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(countDown, "text", "3", 0);
		tween.Parallel().TweenProperty(countDown, "modulate", new Color(1, 1, 1, 1), 0);
		tween.TweenProperty(countDown, "text", "2", 1);
		tween.TweenProperty(countDown, "text", "1", 1);
		tween.TweenProperty(countDown, "text", "GO!", 0);
		tween.TweenProperty(countDown, "modulate", new Color(1, 1, 1, 0), 0.5);
		grabbersInWave = grabbersTotal;
		waveBar.Value = 1.0f;
	}

	public void EnemyDie(int grabbersLeft, int score)
	{
		waveBar.Value = grabbersLeft / (float) grabbersInWave;
		Tween tween = GetTree().CreateTween();
		RandomNumberGenerator rng = new();
		float scale = 10.0f;
		Vector2 prev = waveBar.GlobalPosition;

		for (int i = 0; i < 5; i++)
			tween.TweenProperty(waveBar, "global_position", waveBar.GlobalPosition + new Vector2(rng.RandfRange(-scale, scale), rng.RandfRange(-scale, scale)), 0.02);

		tween.TweenProperty(waveBar, "global_position", prev, 0.02);
	}
}
