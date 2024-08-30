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
	//public static Global Instance { get; private set; }
	public override void _Ready()
	{
		this.scoreColorRect = GetNode<ColorRect>("ColorRect");
		this.scoreLabel = this.scoreColorRect.GetNode<Label>("Label");
		this.firstBlockPosition = firstBlock.Position;
		this.nodePosition = node.Position;
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
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//GD.Print();
		if(!ballSpawn.endGame) {
			if(ballSpawn.roundDone == true) {
				MoveBlocksDown();
				SpawnBlocks();
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
			Vector2 newBlockPosition = new Vector2(nodePosition.X  + positions[i]*this.step - this.step, nodePosition.Y);
			//GD.Print(newBlockPosition);
			newBlock.Position = newBlockPosition;
			newBlock.blocks = blocks;
			newBlock.scoreLabel = this.scoreLabel;

			blocks.Add(newBlock);
			GetTree().Root.AddChild(newBlock);

			newBlock.setNumber(level);
		}
	}

	public void MoveBlocksDown() {
		//GD.Print(blocks.Count);
		for(int i = 0; i < blocks.Count; i++) {
			blocks[i].MoveDown();
		}
	}
}
