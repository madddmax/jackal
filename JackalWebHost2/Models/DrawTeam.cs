﻿using Jackal.Core.Domain;

namespace JackalWebHost2.Models;

public class DrawTeam(Team team)
{
    public int Id = team.Id;
    public string Name = team.Name;
    public bool IsHuman = team.Name == "WebHumanPlayer";
    public int Coins = team.Coins;
    public DrawPosition Ship = new(team.ShipPosition);
}