using Godot;
using System;

public partial class agent_health : ProgressBar
{
	[Export] agent Agent;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GD.Print(Agent.GetHealth());
		GlobalPosition = Agent.GlobalPosition;
		Value = Agent.GetHealth().X / Agent.GetHealth().Y;
	}
}
