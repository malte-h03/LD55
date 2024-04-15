using Godot;
using System;

public partial class Dash : Node2D
{
	player playerRef;
	
	[Export] Timer dashTimer;
	[Export] Timer ParticleResetTimer;
	[Export] AudioStreamPlayer dashSound;

	[Export] CpuParticles2D particle;
	[Export] Node2D particle2;
	[Export] Node2D particle3;
	
	GpuParticles2D particleGPU;
	GpuParticles2D particleGPU2;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		playerRef = (player) GetTree().GetFirstNodeInGroup("Player");
		particleGPU = (GpuParticles2D) particle2;
		particleGPU2 = (GpuParticles2D) particle3;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 directionVector = playerRef.Velocity.Normalized();
		GlobalRotation = MathF.Atan2(directionVector.Y, directionVector.X) + Mathf.Pi;
		
		//queue_free()
	}

	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("DASH") && playerRef.dashReady == true)
		{
			dashSound.Play(0);
			particle.Emitting = true;
			particleGPU.Emitting = true;
			particleGPU2.Emitting = true;
			//await get_tree().create_timer(1.0).timeout;
			//particleGPU.Emitting = false;
			//particleGPU2.Emitting = false;
			
			GD.Print("Dashing");
			dashTimer.Start(dashTimer.WaitTime);
			ParticleResetTimer.Start(ParticleResetTimer.WaitTime);
		}
	}
	
	public void _on_dash_timer_timeout()
	{
		GD.Print("dash ready");
	}
	
	private void _on_particle_reset_timer_timeout()
	{
		GD.Print("particles disabled");
		particleGPU.Restart();
		particleGPU2.Restart();
		// Replace with function body.
	}
	
}




