using Godot;
using System;

public class PlayerTwo : KinematicBody2D
{
	// Declare member variables here. Examples:
	public int speed = 250;
	public Vector2 ScreenSize;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Make sure node stops when paused
		PauseMode = Node.PauseModeEnum.Stop;
		ScreenSize = GetViewportRect().Size;
	}

	public void start(Vector2 pos){
		Position = pos;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		var velocity = Vector2.Zero;
		if (Input.IsActionPressed("player_two_down"))
		{
			velocity.y += (float)1.4;
		}

		if (Input.IsActionPressed("player_two_up"))
		{
			velocity.y -= (float)1.4;
		}

		Position += velocity * delta * speed;
		Position = new Vector2(
			x: Mathf.Clamp(Position.x, 0, ScreenSize.x),
			y: Mathf.Clamp(Position.y, 0, ScreenSize.y - 48)
		);
	}
}

