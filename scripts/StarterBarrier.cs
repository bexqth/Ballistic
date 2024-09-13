using Godot;
using System;

public partial class StarterBarrier : Area2D
{
	
	private bool ballLineCollided;
	private int ballsCollided;
	private Vector2 newStartingPosition;
	private int minX = 20;
	private int maxX = 535;
	private int minY = 400;
	private int maxY = 500;
	[Export]
	public Timer gameOverTimer;
	[Export]
	public BallSpawn ballSpawn;
	public override void _Ready()
	{
		ballsCollided = 0;
	}

	
	public override void _Process(double delta)
	{

	}
	
	
	private void _on_body_entered(Node2D body)
	{		
		if(body is Ball ball) {
			//GD.Print("ball colided");
			if(ballsCollided == 0) { // first went thro
				ballLineCollided = true;
				newStartingPosition = new Vector2(ball.Position.X, 755);
				ballSpawn.Position = newStartingPosition;

				ballsCollided++;
			} else {
				ballsCollided++;
			}
			if(ballsCollided == ballSpawn.numberOfBalls) {
				ballsCollided = 0;
				
				ballSpawn.Restore();
				
			}
			ballSpawn.Visible = true;
			//ball.QueueFree();

			//GD.Print("Collision detected with: ", body.Name);
			//ball.restore(ball.Position.X);
		}
		
	}

	private void _on_area_entered(Area2D area)
	{
		if (area.GetParent() is Block block) {
			//GD.Print("Block colided");
			ballSpawn.endGame = true;
			gameOverTimer.Start();
			ballSpawn.timer.Start();

		}
	}

}


