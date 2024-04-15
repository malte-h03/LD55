using Godot;
using System;

public partial class EnemyDiesScript : Node2D
{
	
	private CpuParticles2D emitter;
	private GpuParticles2D emitter2;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		emitter = GetNode<CpuParticles2D>("EnemyDiesParticle");
		emitter.Emitting = true;
		
		emitter2 = GetNode<GpuParticles2D>("EnemyDiesParticle/GPUParticles2D");
		emitter2.Emitting = true;
		
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
