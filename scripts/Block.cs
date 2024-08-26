using Godot;
using System;

public partial class Block : StaticBody2D
{
	// Called when the node enters the scene tree for the first time.
	private ColorRect colorRect;
	private Label labelNumber;
	private int number;
	private bool signalConnected;

	[Export]
	public Ball ball;
	public override void _Ready()
	{
		colorRect = GetNode<ColorRect>("ColorRect");
		labelNumber = colorRect.GetNode<Label>("Label");
		number = 1;
		updateLabelNumber();
		signalConnected = false;
		//this.Connect(nameof(Ball.BallCollided), new Callable(this, nameof(onBallCollided)));
	}


	public override void _PhysicsProcess(double delta)
	{


	}


	public void updateLabelNumber() {
		this.labelNumber.Text = this.number.ToString();
	}

	public void reduceNumber() {
		this.number--;
	}

	public void onBallCollided(Ball ball) {
		this.reduceNumber();
		this.updateLabelNumber();
	}
	
	
	private void _on_area_2d_body_entered(Node2D body)
	{
			this.reduceNumber();
			this.updateLabelNumber();
		
	}

}


