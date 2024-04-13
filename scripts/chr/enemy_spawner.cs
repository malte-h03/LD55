using Godot;
using System.Threading.Tasks;

public partial class enemy_spawner : Node2D
{

    [Export] float radius = 10.0f;
    [Export] PackedScene enemyRef;
	public override void _Ready()
	{	
        // SpawnEnemy(5, 1.0f);
	}

    public async void SpawnEnemy(int amount, float delay)
    {
        Node root = GetTree().Root;
        for (int i = 0; i < amount; i++)
        {
            await Task.Delay((int) (delay * 1000.0));
            RandomNumberGenerator rng = new();
            Node2D grabberNode = enemyRef.Instantiate<Node2D>();
            grabberNode.GlobalPosition = GlobalPosition;
            grabberNode.GlobalPosition += new Vector2(rng.RandfRange(-1, 1), rng.RandfRange(-1, 1)).Normalized() * rng.RandfRange(0, radius);

            root.AddChild(grabberNode);
        }
    }

    public override void _Process(double delta)
	{

	}
}