﻿using Jackal.Core.Domain;

namespace JackalWebHost2.Models;

public class DrawTeam(Team team)
{
    public readonly int Id = team.Id;
    
    public readonly string Name = team.Name;
    
    public bool IsHuman => team.Name == "WebHumanPlayer";
    
    public DrawPosition Ship = new(team.ShipPosition);
}