using Godot;
using System;

public partial class Swipe : Node2D
{
	[Export] Timer slashTimer;
	[Export] Area2D damageZone;
	[Export] CpuParticles2D particle;
	[Export] GpuParticles2D particle2;
	[Export] AudioStreamPlayer swoosh;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 mousePos = GetGlobalMousePosition();
		Vector2 directionVector =( GlobalPosition - mousePos ).Normalized();
		GlobalRotation = MathF.Atan2(directionVector.Y, directionVector.X) + Mathf.Pi;
	}

	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("SLASH") && slashTimer.TimeLeft == 0)
		{
			swoosh.Play(0);
			particle.Emitting = true;
			particle2.Emitting = true;

			var allbodies = damageZone.GetOverlappingBodies();
			GD.Print("Slashing");
			GD.Print("Colliding bodies ", allbodies.Count);
			foreach(Node2D body in allbodies)
			{
				if (body.IsInGroup("Enemy"))
				{
					agent enemyNode = (agent) body; //.TakeDamage(50);

					enemyNode.TakeDamage(50);
				}
			}

			slashTimer.Start(slashTimer.WaitTime);
		}
	}

	public void _on_timer_timeout()
	{
		GD.Print("slash ready");
	}
}
