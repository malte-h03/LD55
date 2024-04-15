using Godot;
using System;
using System.Threading.Tasks;

public partial class pickup : Node2D
{

	[Export] float movementBuff = 1.2f;
	[Export] float dashTimeBuff = 0.8f;
	[Export] float strengthBuff = 0.8f;

	[Export] Label BuffText;
	[Export] Sprite2D BuffIcon;
	[Export] CollisionShape2D BuffArea;
	[Export] Light2D BuffLight;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public async void _on_area_2d_body_entered(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			RandomNumberGenerator rng = new();

			int index = rng.RandiRange(0, 2);
			player Player = (player) body;
			switch (index)
			{
				case 0:
					Player.movementSpeed *= movementBuff;
					BuffText.Text = "Move UP";
					GD.Print("Movement buffed");
					break;

				case 1:
					Player.dashTimer.WaitTime *= dashTimeBuff;
					BuffText.Text = "Stamina UP";
					GD.Print("dash time buffed");
					break;

				case 2:
					Player.dragForceMultiplier *= strengthBuff;
					BuffText.Text = "Strength UP";
					GD.Print("strength buffed");
					break;

				default:
					break;
			}

		BuffArea.Disabled = true;
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(BuffText, "modulate", new Color(1, 1, 1, 1), 0);
		tween.TweenProperty(BuffText, "modulate", new Color(1, 1, 1, 0), 1);
		tween.Parallel().TweenProperty(BuffText, "global_position", BuffText.GlobalPosition + Vector2.Up * 20, 1).SetTrans(Tween.TransitionType.Elastic);
		tween.Parallel().TweenProperty(BuffIcon, "modulate", new Color(1, 1, 1, 0), 1);
		tween.Parallel().TweenProperty(BuffLight, "energy", 0, 1);
		// tween.TweenCallback(Callable.From(BuffText.QueueFree));
		await Task.Delay(1000);
		QueueFree();
		}
	}
}
