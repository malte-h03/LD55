using Godot;
using System;

public partial class Dash : Node2D
{
	player playerRef;
	
	[Export] Timer dashTimer;
	[Export] CpuParticles2D particle;
	[Export] Node2D particle2;
	
	GpuParticles2D particleGPU;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		playerRef = (player) GetTree().GetFirstNodeInGroup("Player");
		particleGPU = (GpuParticles2D) particle2;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 directionVector = playerRef.Velocity.Normalized();
		GlobalRotation = MathF.Atan2(directionVector.Y, directionVector.X) + Mathf.Pi;
	}

	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("DASH"))
		{
			particle.Emitting = true;
			particleGPU.Emitting = true;
			GD.Print("Dashing");

			dashTimer.Start(dashTimer.WaitTime);
		}
	}
	
	public void _on_dash_timer_timeout()
	{
		GD.Print("dash ready");
	}
}

