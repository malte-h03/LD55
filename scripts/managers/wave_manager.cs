using System.Threading.Tasks;
using Godot;
using Godot.Collections;

public partial class wave_manager : Node
{
	[Export] int startGrabbers = 5;
	[Export] float waveMultiplier = 1.5f;
	[Export] int startPortals = 1;

	int totalPortals;// = startPortals;
	PackedScene portal;

	private int score = 0;

	float waveDelay = 1.0f;

	private int grabbersInWave;

	private int grabbersLeft;
	private int portalsLeft;
	private int currentWave = 0;

	private Array<Node> allSpawners;
	private Array<int> spawnLocations;
	Label scoreText;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		totalPortals = startPortals;
		portal = GD.Load<PackedScene>("res://Scenes/objects/portal.tscn");
		grabbersInWave = startGrabbers;
		spawnLocations = new();
	}

	public async void startWave()
	{
		GetTree().CallGroup("WaveSubscriber", "WaveStarted", grabbersInWave);

		Label waveText = GetTree().GetFirstNodeInGroup("HUD").GetNode<Label>("Container/wave");
		scoreText = GetTree().GetFirstNodeInGroup("HUD").GetNode<Label>("Container/score");

		waveText.Text = "Wave: " + (currentWave + 1);
		allSpawners = GetTree().GetNodesInGroup("EnemySpawner");
		spawnLocations.Resize(allSpawners.Count);
		spawnLocations.Fill(0);

		grabbersLeft = grabbersInWave;
		for (int i = 0; i < grabbersInWave; i++)
		{
			RandomNumberGenerator rng = new();
			int entry = rng.RandiRange(0, allSpawners.Count - 1);

			spawnLocations[entry] += 1;
			GD.Print("added 1 to ", entry, " total ", spawnLocations[entry]);
		}

		await Task.Delay(1500);

		int counter = 0;
		foreach(enemy_spawner spawner in allSpawners)
		{
			spawner.SpawnEnemy(spawnLocations[counter], 1.0f);
			if (counter < totalPortals)
			{
				var portalNode = portal.Instantiate();
				spawner.AddChild(portalNode);
			}
			counter++;
		}
	}


	public void EnemyDie(Node2D body, int getScore)
	{
		grabbersLeft--;
		score += getScore;
		
		var scene = GD.Load<PackedScene>("res://Scenes/Effects/EnemyDies.tscn");
		var EnemyDies = scene.Instantiate();
		AddChild(EnemyDies);
		((Node2D)EnemyDies).GlobalPosition = body.GlobalPosition;
		
		GetTree().CallGroup("WaveSubscriber", "EnemyDie", grabbersLeft, score);
		
		if (grabbersLeft == 0)
		{
			EndWave(currentWave);
		}
		scoreText.Text = "score " + score;
		Tween tween = GetTree().CreateTween();
		RandomNumberGenerator rng = new();
		float scale = 5.0f;
		Vector2 prev = scoreText.GlobalPosition;

		for (int i = 0; i < 5; i++)
			tween.TweenProperty(scoreText, "global_position", scoreText.GlobalPosition + new Vector2(rng.RandfRange(-scale, scale), rng.RandfRange(-scale, scale)), 0.02);

		tween.TweenProperty(scoreText, "global_position", prev, 0.02);
	}

	public async void EndWave(int wave)
	{
		grabbersInWave = (int) Mathf.Floor(grabbersInWave * waveMultiplier);
		currentWave = wave + 1;
		totalPortals++;
		GD.Print("End wave", wave, " start ", currentWave);
		await Task.Delay((int) (waveDelay * 1000));

		startWave();
	}

	public void CancelWave()
	{
		var allEnemies = GetTree().GetNodesInGroup("Enemy");

		foreach( RigidBody2D enemy in allEnemies )
		{
			enemy.QueueFree();
		}
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
