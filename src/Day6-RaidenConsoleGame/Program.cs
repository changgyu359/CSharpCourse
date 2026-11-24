using System;
using System.Text;
using RaidenConsoleGame;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;
Console.CursorVisible = false;

var inputManager = new InputManager();
var renderer = new ConsoleRenderer(60, 32);
var engine = new GameEngine(renderer.Width, renderer.Height, inputManager);
var gameLoop = new GameLoop(engine, renderer, inputManager);

gameLoop.Run();
