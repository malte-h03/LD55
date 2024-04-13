using System.Threading.Tasks;
using Godot;
using Godot.Collections;

public partial class wave_manager : Node
{
	[Export] int startGrabbers = 5;
	[Export] float waveMultiplier = 1.5f;
	[Export] int startPortals;

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
		grabbersInWave = startGrabbers;
		spawnLocations = new();
	}

	public void startWave()
	{
		Label waveText = GetTree().GetFirstNodeInGroup("HUD").GetChild<Label>(0);
		scoreText = GetTree().GetFirstNodeInGroup("HUD").GetChild<Label>(1);

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

		int counter = 0;
		foreach(enemy_spawner spawner in allSpawners)
		{
			spawner.SpawnEnemy(spawnLocations[counter], 1.0f);
			counter++;
		}
		
	}

	public void EnemyDie(Node2D body, int getScore)
	{
		grabbersLeft--;
		score += getScore;
		if (grabbersLeft == 0)
		{
			EndWave(currentWave);
		}
		scoreText.Text = "Score " + score;
	}

	public async void EndWave(int wave)
	{
		grabbersInWave = (int) Mathf.Floor(grabbersInWave * waveMultiplier);
		currentWave = wave + 1;

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