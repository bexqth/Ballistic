using Godot;
using System;
using System.Collections.Generic;

public partial class Block : StaticBody2D
{
	// Called when the node enters the scene tree for the first time.
	private ColorRect colorRect;
	private Label labelNumber;
	public int number{get;set;}
	private int space = 10;
	private int blockHeight = 70;
	private int step;
	private bool signalConnected;
	public List<Block> blocks{get;set;}
	private Line2D line;
	private Color originalColor;
	public int scoreAdd{get;set;}
	public Label scoreLabel{get;set;}

	[Export]
	public Ball ball;
	public override void _Ready()
	{
		colorRect = GetNode<ColorRect>("ColorRect");
		line = colorRect.GetNode<Line2D>("Line2D");
		labelNumber = colorRect.GetNode<Label>("Label");
		originalColor = colorRect.Color;
		number = 6;
		updateLabelNumber();
		signalConnected = false;
		this.step = this.space + this.blockHeight;
		//this.Connect(nameof(Ball.BallCollided), new Callable(this, nameof(onBallCollided)));
	}


	public override void _Process(double delta)
	{
		checkForScore();
	}
	public void addScoreFromBlock(int totalScore) {
		totalScore += scoreAdd;
	}

	public override void _PhysicsProcess(double delta)
	{
			
	}

	public void LightUp() {
		this.colorRect.Color = line.DefaultColor;
	}

	public void LightDown() {
		this.colorRect.Color = originalColor;
	}

	public void MoveDown() {
		var pos = Position;
		pos.Y = this.Position.Y + this.step;
		Position = pos;
	}

	public void checkForScore() {
		if(number <= 0) {
			blocks.Remove(this);
			this.QueueFree();
		}
	}


	public void updateLabelNumber() {
		this.labelNumber.Text = this.number.ToString();
	}

	public void reduceNumber() {
		this.number--;
	}

	/*public void onBallCollided(Ball ball) {
		this.reduceNumber();
		this.updateLabelNumber();
	}*/
	
	private void _on_area_2d_body_entered(Node2D body)
	{
		//var global = Global.Instance;
		this.reduceNumber();
		this.updateLabelNumber();
		//global.totalScore++;
		GD.Print(scoreLabel);
		int score = this.scoreLabel.Text.ToInt();
		GD.Print(score);
		score++;
		this.scoreLabel.Text = score.ToString();
		GD.Print("score +1");
		LightUp();
		
	}
	
	private void _on_area_2d_body_exited(Node2D body)
	{
		LightDown();
		
	}

}


