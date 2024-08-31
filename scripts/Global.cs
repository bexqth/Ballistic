using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Global : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	public Block firstBlock;
	[Export]
	public Node2D node;

	[Export]
	public Node2D spawnNode;
	private Vector2 spawnPosition;
	private Vector2 nodePosition;
	private Vector2 firstBlockPosition;
	private int space = 10;
	private int blockHeight = 70;
	private int step;
	private List<Block> blocks;
	private Label scoreLabel;
	private ColorRect scoreColorRect;
	public int totalScore{get;set;}
	[Export]
	public BallSpawn ballSpawn;
	private int level;
	private Timer moveBlockTimer;
	private bool blockStop;
	private bool shouldMoveBlocks = true;
	private Timer waitBlockTimer;
	//public static Global Instance { get; private set; }
	public override void _Ready()
	{
		this.scoreColorRect = GetNode<ColorRect>("ColorRect");
		this.scoreLabel = this.scoreColorRect.GetNode<Label>("Label");
		this.firstBlockPosition = firstBlock.Position;
		this.nodePosition = node.Position;
		this.spawnPosition = spawnNode.Position;

		this.blocks = new List<Block>();
		this.blocks.Add(firstBlock);
		firstBlock.blocks = blocks;
		firstBlock.scoreLabel = this.scoreLabel;
		//firstBlock.scoreAdd = totalScore;
		this.step = this.space + this.blockHeight;
		this.totalScore = 0;
		this.level = 1;
		this.scoreLabel.Text = this.totalScore.ToString();
		//Instance = this;
		//GD.Print(nodePosition);
		this.moveBlockTimer = GetNode<Timer>("Timer");
		this.waitBlockTimer = GetNode<Timer>("Timer_Wait");
		this.blockStop = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//GD.Print();
		if(!ballSpawn.endGame) {
			if(ballSpawn.roundDone == true) {
				//shouldMoveBlocks = true;
				SpawnBlocks();
				//MoveBlocksDown();
				waitBlockTimer.Start();
				ballSpawn.roundDone = false;
			}
		} else {
			for(int i = 0; i < this.blocks.Count; i++) {
				blocks[i].colorRect.Color = new Color("#474743");
				blocks[i].line.DefaultColor = new Color("#474743");
			}
		}
	}

	public void SpawnBlocks() {
		
		//int numberOfNewBlocks = 7;
		//int[] positions = new int[7] {1,2,3,4,5,6,7}; //MAX IS 7
		this.level++;
		GD.Print(level);
		var rnd = new Random();
		int numberOfNewBlocks = rnd.Next(1,5);
		var positions = Enumerable.Range(1,7).OrderBy(x => rnd.Next()).Take(numberOfNewBlocks).ToList();
		for(int i = 0; i < numberOfNewBlocks; i++) {
			PackedScene newBlockScene = GD.Load<PackedScene>("res://scenes/block.tscn");
			Block newBlock = (Block)newBlockScene.Instantiate();
			//Vector2 newBlockPosition = new Vector2(nodePosition.X  + positions[i]*this.step - this.step, nodePosition.Y);
			Vector2 newBlockPosition = new Vector2(spawnPosition.X  + positions[i]*this.step - this.step, spawnPosition.Y);

			newBlock.Position = newBlockPosition;
			newBlock.blocks = blocks;
			newBlock.scoreLabel = this.scoreLabel;

			blocks.Add(newBlock);
			GetTree().Root.AddChild(newBlock);

			newBlock.setNumber(level);
		}
	}

	public void MoveBlocksDown() {
		if (shouldMoveBlocks) {
		for (int i = 0; i < blocks.Count; i++) {
			blocks[i].MoveDownABit();
		}
		MoveSpawnNodeDownABit();
		moveBlockTimer.Start();
		} else {
			moveBlockTimer.Stop();
			spawnNode.Position = spawnPosition;
		}
	}

	public void MoveSpawnNodeDownABit() {
		var bit = 5;
		var pos = spawnNode.Position;
		pos.Y += bit;
		spawnNode.Position = pos;

		if (spawnNode.Position.Y >= 185) {
			spawnNode.Position = spawnPosition;
			shouldMoveBlocks = false;
		}
		//GD.Print(spawnNode.Position.Y);
	}
	
	
	private void _on_timer_timeout()
	{
		MoveBlocksDown();
	}
	
	private void _on_timer_wait_timeout()
	{
		shouldMoveBlocks = true;
		MoveBlocksDown();
		waitBlockTimer.Stop();
	}
}




