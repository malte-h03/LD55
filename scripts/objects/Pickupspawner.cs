using Godot;
using Godot.Collections;
using System;

public partial class Pickupspawner : Node2D
{

	[Export] Timer pickupTimer;
	[Export] PackedScene pickup;
	[Export] Array<Marker2D> points;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_timer_timeout()
	{
		GD.Print("Pickup spawned");
		Node2D pickupNode = pickup.Instantiate<Node2D>();
		pickupNode.GlobalPosition = points.PickRandom().GlobalPosition;
		GetTree().Root.AddChild(pickupNode);
	}
}
