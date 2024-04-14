using Godot;
using System;
using System.Threading.Tasks;

public partial class game_start : Node2D
{
	private wave_manager WaveManager;
	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		await Task.Delay(2000);
		WaveManager = GetTree().Root.GetNode<wave_manager>("WaveManager");
		WaveManager.startWave();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
