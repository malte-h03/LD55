using Godot;

public partial class scene_manager : Node
{
	private Node currentSceneNode;
	public override void _Ready()
	{		
		Viewport sceneRoot = GetTree().Root;
		currentSceneNode = sceneRoot.GetChild(sceneRoot.GetChildCount() - 1);
	}
	public void GotoScene(string scenePath)
	{
		PackedScene newScene = GD.Load<PackedScene>(scenePath);
		CallDeferred("DeferredGotoScene", newScene);
	}
	private void DeferredGotoScene(PackedScene newScene)
	{
		currentSceneNode.Free();

		currentSceneNode = newScene.Instantiate();
		GetTree().Root.AddChild(currentSceneNode);
		GetTree().CurrentScene = currentSceneNode;
		GetTree().CallGroup("SceneStateSubscriber", "OnSceneChanged");
	}

	public Node GetCurrentSceneRoot()
	{
		return currentSceneNode;
	}
}
