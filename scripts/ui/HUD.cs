using Godot;
using System;

public partial class HUD : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	[Export] ProgressBar waveBar;

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
