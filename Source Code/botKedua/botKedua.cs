using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class botKedua : Bot
{   
    private double enemyX = 1;
    private double enemyY = 1;
    private double targetSpeed = 0;
    private double targetDirection = 0;

    private bool trackingTarget = false;
    private int trackScanTicks = 0;

    private int moveDirection = 1;

    static void Main(string[] args)
    {
        new botKedua().Start();
    }

    botKedua() : base(BotInfo.FromFile("botKedua.json")) { }

    public override void Run()
    {
        BodyColor = Color.RebeccaPurple;
        TurretColor = Color.BlanchedAlmond;
        RadarColor = Color.IndianRed;

        // Mengizinkan radar dan meriam bergerak bebas tidak terikat putaran badan bot
        AdjustRadarForGunTurn = true;
        AdjustGunForBodyTurn = true;

        while (IsRunning)
        {
            if (!trackingTarget) {

                    SetTurnRadarLeft(360);
                    SetForward(100 * moveDirection);

                } else {
                    
                    double absoluteDirection = DirectionTo(enemyX, enemyY);
                    double gunBearing = NormalizeRelativeAngle(absoluteDirection - GunDirection);
                    SetTurnGunLeft(gunBearing);

                    double radarBearing = NormalizeRelativeAngle(absoluteDirection - RadarDirection);
                    double microSweep = (trackScanTicks % 2 == 0) ? 6 : -6;
                    SetTurnRadarLeft (radarBearing + microSweep);

                    double jarak = DistanceTo(enemyX, enemyY);

                    double bearingFromBot = NormalizeRelativeAngle(BearingTo(enemyX, enemyY) - Direction);
                    if (jarak <= 90) {
                        SetTurnRight (bearingFromBot);
                        SetForward (500 * moveDirection);
                    } else {
                        SetTurnRight(bearingFromBot + 90);
                        SetForward(150 * moveDirection);
                    }
                

                    if (jarak <= 110) {
                        SetFire(2.3);

                        } else {
                        SetFire(1.6);
                        }

                }

                    trackScanTicks++;
                if (trackScanTicks >= 20) {

                    trackingTarget = false;
                    trackScanTicks = 0;
                }

                Go();
            }
        }
    

    public override void OnScannedBot(ScannedBotEvent e)
    {
        enemyX = e.X;           
        enemyY = e.Y;           
        targetSpeed = e.Speed;
        targetDirection = e.Direction;
        trackingTarget = true;   
        trackScanTicks = 0;
    }

    public override void OnHitBot(HitBotEvent e)
    {
        SetTurnRight(50);
    }

    public override void OnHitWall(HitWallEvent e)
    {
        moveDirection *= -1;
    }
    

}
