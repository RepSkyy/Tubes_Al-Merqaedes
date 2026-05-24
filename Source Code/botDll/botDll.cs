using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class botDll : Bot
{   
    
    private int moveDirection = 1;

    static void Main(string[] args)
    {
        new botDll().Start();
    }

    botDll() : base(BotInfo.FromFile("botDll.json")) { }

    public override void Run()
    {
        BodyColor = Color.DarkGreen;
        TurretColor = Color.HotPink;
        RadarColor = Color.IndianRed;

        while (IsRunning)
        {
            SetTurnRadarLeft(360);
            SetForward(100 * moveDirection);
            Go();
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {   
        double absoluteDirection = DirectionTo(e.X, e.Y);
                    double gunBearing = NormalizeRelativeAngle(absoluteDirection - GunDirection);
                    SetTurnGunLeft(gunBearing);
        SetFire(2.5);
    }

    public override void OnHitBot(HitBotEvent e)
    {
        SetTurnRight(70);
    }

    public override void OnHitWall(HitWallEvent e)
    {
        moveDirection *= -1;
    }

    /* Read the documentation for more events and methods */
}
