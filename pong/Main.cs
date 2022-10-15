using Godot;
using System;

public class Main : Node
{
	public int ScoreOne = 0;
	public int ScoreTwo = 0;
	public Boolean muted = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Allow this node to continue with game paused
		PauseMode = Node.PauseModeEnum.Process;

		GD.Randomize();
		var playerOne = GetNode<PlayerOne>("PlayerOne");
		var playerTwo = GetNode<PlayerTwo>("PlayerTwo");
		var ball = GetNode<Ball>("Ball");
		ball.Hide();
		playerOne.Hide();
		playerTwo.Hide();
		
		var winnerLabel = GetNode<Label>("WinnerLabel");
		winnerLabel.Text = "This Isn't Pong";
		winnerLabel.Show();
	}

	public void NewGame(){
		GD.Randomize();
		var ball = GetNode<Ball>("Ball");
		ball.start();
		if (ScoreOne > ScoreTwo){
			ball.direction = ball.direction * -1;
		}
		GetNode<Label>("WinnerLabel").Hide();
		ScoreOne = 0;
		ScoreTwo = 0;
		GetNode<Label>("ScoreOneLabel").Text = "0";
		GetNode<Label>("ScoreTwoLabel").Text = "0";

		var playerOne = GetNode<PlayerOne>("PlayerOne");
		var playerTwo = GetNode<PlayerTwo>("PlayerTwo");
		var startPositionOne = GetNode<Position2D>("StartPositionOne");
		var startPositionTwo = GetNode<Position2D>("StartPositionTwo");
		playerOne.start(startPositionOne.Position);
		playerTwo.start(startPositionTwo.Position);
		
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		var ball = GetNode<Ball>("Ball");
		var playerOne = GetNode<PlayerOne>("PlayerOne");
		var playerTwo = GetNode<PlayerTwo>("PlayerTwo");

	//Handle pausing
	if (Input.IsActionJustPressed("pause")) {
		if (GetTree().Paused) {
			var winnerLabel = GetNode<Label>("WinnerLabel");
			winnerLabel.Hide();
			GetTree().Paused = false;
		}
		else {
			GetTree().Paused = true;
			var winnerLabel = GetNode<Label>("WinnerLabel");
			winnerLabel.Text = "Paused";
			winnerLabel.Show();
		}
		
	}
	if (Input.IsActionJustPressed("mute")){
		var master_sound = AudioServer.GetBusIndex("Master");
		if (muted){
			AudioServer.SetBusMute(master_sound, true);
		}
		else{
			AudioServer.SetBusMute(master_sound, false);
		}
	}

	//Check for scoring
		if (ball.Position.x <= 0){
			if (ScoreTwo == 2){
				ScoreTwo = 3;
				GetNode<Label>("ScoreTwoLabel").Text = ScoreTwo.ToString();
				PlayerTwoWins();
			}
			else{
				ScoreTwo += 1;
				GetNode<Label>("ScoreTwoLabel").Text = ScoreTwo.ToString();
				ball.Hide();
				System.Threading.Thread.Sleep(500);
				ball.Show();
				ball.start();
			}
		}
		else if (ball.Position.x >= 800){
			if (ScoreOne == 2){
				ScoreOne = 3;
				GetNode<Label>("ScoreOneLabel").Text = ScoreTwo.ToString();
				PlayerOneWins();
			}
			else{
				ScoreOne += 1;
				GetNode<Label>("ScoreOneLabel").Text = ScoreOne.ToString();
				ball.Hide();
				System.Threading.Thread.Sleep(500);
				ball.Show();
				ball.start();
				ball.direction = ball.direction * -1;
			}
		}
	}
	private void OnButtonPressed()
	{
		var playerOne = GetNode<PlayerOne>("PlayerOne");
		var playerTwo = GetNode<PlayerTwo>("PlayerTwo");
		var ball = GetNode<Ball>("Ball");
		var button = GetNode<Button>("Button");
		playerOne.Show();
		playerTwo.Show();
		ball.Show();
		button.Hide();
		NewGame();
	}
	
	private void PlayerOneWins(){
		var winnerLabel = GetNode<Label>("WinnerLabel");
		var button = GetNode<Button>("Button");
		button.Text = "Play Again?";
		winnerLabel.Text = "Player One Wins!";
		GetNode<PlayerOne>("PlayerOne").Hide();
		GetNode<PlayerTwo>("PlayerTwo").Hide();
		winnerLabel.Show();
		button.Show();

		var ball = GetNode<Ball>("Ball");
		ball.Hide();
		ball.Velocity = Vector2.Zero;
		ball.Position = new Vector2(
			x: 400,
			y: 312
		);

	}
	
	private void PlayerTwoWins(){
		var winnerLabel = GetNode<Label>("WinnerLabel");
		var button = GetNode<Button>("Button");
		button.Text = "Play Again?";
		winnerLabel.Text = "Player Two Wins!";
		GetNode<PlayerOne>("PlayerOne").Hide();
		GetNode<PlayerTwo>("PlayerTwo").Hide();
		winnerLabel.Show();
		button.Show();

		var ball = GetNode<Ball>("Ball");
		ball.Hide();
		ball.Velocity = Vector2.Zero;
		ball.Position = new Vector2(
			x: 400,
			y: 312
		);
	}
	
}

