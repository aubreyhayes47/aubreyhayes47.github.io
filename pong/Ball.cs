using Godot;
using System;

public class Ball : KinematicBody2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	public int speed = 225;
	public Vector2 ScreenSize;
	public Vector2 Velocity = Vector2.Zero;
	public float direction;
	public bool directionChange = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Make sure node stops when paused
		PauseMode = Node.PauseModeEnum.Stop;
		
		ScreenSize = GetViewportRect().Size;
	}

	public void start(){
		Position = new Vector2(
			x: 400,
			y: 312
		);
		
		Random rnd = new Random();
		float[] horizontalNumbers = new float[] {-1, 1};
		float[] verticalNumbers = new float[] {(float)0.8, (float)0.8};
		Velocity.x = horizontalNumbers[rnd.Next(0,1)];
		Velocity.y = verticalNumbers[rnd.Next(0,1)];

		var direction = (float)GD.RandRange(-7*Mathf.Pi/4, -3*Mathf.Pi/4);
		Velocity = Velocity.Rotated(direction);

	}

//  Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		//Make the ball move and bounce
		var collision = MoveAndCollide(Velocity * speed * delta);
		if (collision != null){
			Random rand = new Random();
			var soundPicker = rand.Next(0,4);
			if (soundPicker == 3){
				GetNode<AudioStreamPlayer2D>("d5").Play();
			}
			else if (soundPicker == 2){
				GetNode<AudioStreamPlayer2D>("e3").Play();
			}
			else if (soundPicker == 1){
				GetNode<AudioStreamPlayer2D>("e4").Play();
			}
			else {
				GetNode<AudioStreamPlayer2D>("f4").Play();
			}
			Velocity = Velocity.Bounce(collision.Normal);
		}

	}
}
