using Godot;
using System;
using System.Numerics;

public partial class BallSpawn : Node2D
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	public int numberOfBalls;
	private int ballsToShoot;
	public Timer ballTimer;
	private Label numberLabel;
	private bool grabbed;
	public Godot.Vector2 jumpDirection{get;set;}
	public bool roundDone{get;set;}
	public bool endGame{get;set;}
	[Export]
	public Label gameLabel;
	public Timer shootTimer;
	private int minX = 20;
	private int maxX = 535;
	private int minY = 400;
	private int maxY = 500;
	int ballIndex = 1;
	public Timer timer{get;set;}
	public int spawnIndex{get;set;}

	public int Index { get; set; }
    public int ballsCollided { get; set; } = 0; // Initialize to 0
    public bool ballLineCollided { get; set; } = false;
    public Godot.Vector2 newStartingPosition { get; set; }


	private RandomNumberGenerator rand;
	public override void _Ready()
	{
		this.ballsToShoot = numberOfBalls;
		this.ballTimer = GetNode<Timer>("Ball_Timer");
		this.numberLabel = GetNode<Label>("Number_Label");
		this.shootTimer = GetNode<Timer>("Timer_Shoot");
		this.timer = GetNode<Timer>("Timer");
		numberLabel.Text = ballsToShoot.ToString();
		roundDone = false;
		endGame = false;
		rand = new RandomNumberGenerator();
		rand.Randomize();
		//this.timer.Start();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		/*if(endGame) {
			gameLabel.ZIndex = 2;
			//gameLabel.Text = "Game over";
			timer.Start();
			endGame = false;
		}*/

	}

	private void SetAngle() {
		GenerateNewDirection();
	}

	public void Restore() {
		ballsToShoot = numberOfBalls;
		grabbed = false;
		ballTimer.Stop();
		numberLabel.Text = ballsToShoot.ToString();
		roundDone = true;
		ballIndex = 1;
		GenerateNewDirection();
		//StartShooting();
	}

	public void SpawnBall() {
		if (!endGame)
		{
			if (ballsToShoot > 0)
			{
				if (ballsToShoot == 0)
				{
					this.Visible = false;
				}
				PackedScene newBallScene = GD.Load<PackedScene>("res://scenes/ball.tscn");
				Ball newBall = (Ball)newBallScene.Instantiate();
				newBall.Position = this.Position;
				GetTree().Root.AddChild(newBall);
				newBall.isFlying = true;
				//newBall.number = ballIndex;
				//ballIndex++;
				newBall.spawnIndex = this.spawnIndex;

				//GD.Print("DIRECTION" + jumpDirection);
				newBall.SetFlying(jumpDirection);

				ballsToShoot--;
				numberLabel.Text = ballsToShoot.ToString();
				ballTimer.Start();
			}
		}
	}

	private void GenerateNewDirection()
	{
		RandomNumberGenerator rand = new RandomNumberGenerator();
		rand.Randomize();
		float randomX = rand.RandfRange(minX, maxX);
		float randomY = rand.RandfRange(minY, maxY);
		Godot.Vector2 newVector = new Godot.Vector2(randomX, randomY);
		Godot.Vector2 newJumpDirection = (newVector - GlobalPosition).Normalized();
		//GD.Print("new direction is" + newJumpDirection);
		jumpDirection = newJumpDirection;
	}
	
	 public void StartShooting()
	{
		if (!endGame)
		{
			shootTimer.Start();
		}
	}


	private void _on_ball_timer_timeout()
	{
		if (!endGame)
		{
			SpawnBall();
		}
	}


	private void _on_timer_shoot_timeout()
	{
		if (ballsToShoot > 0 && !endGame)
		{
			SpawnBall();
		}
		else
		{
			shootTimer.Stop();
		}
	}

	private void _on_timer_timeout()
	{
		if (!endGame)
		{
			//gameLabel.ZIndex = 1;
			//gameLabel.Text = "";
			SetAngle();
			//GD.Print(jumpDirection);
			SpawnBall();
			//timer.Stop();
			//endGame = false;
		}
	}
}



