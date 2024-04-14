using Godot;
using System;

public partial class music_mixer : AudioStreamPlayer
{
	[Export] AudioStreamPlayer looptrack;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_finished()
	{
		looptrack.Play(0);
	}
}
