using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class LEnergyHunter : Bot
{
    private int moveDirection = 1;

    // Target dengan energi terendah
    private int targetId = -1;
    private double targetEnergy = double.MaxValue;

    static void Main(string[] args)
    {
        new LEnergyHunter().Start();
    }

    LEnergyHunter() : base(BotInfo.FromFile("LEnergyHunter.json"))
    {
    }

    public override void Run()
    {
        BodyColor = Color.DarkRed;
        RadarColor = Color.Yellow;
        GunColor = Color.Black;
        BulletColor = Color.Orange;

        AdjustGunForBodyTurn = true;
        AdjustRadarForGunTurn = false;
        AdjustRadarForBodyTurn = false;

        // Radar muter terus
        SetTurnGunRight(double.PositiveInfinity);

        while (IsRunning)
        {
            AvoidWall();

            // Gerakan zig-zag
            SetTurnRight(10 * moveDirection);
            SetForward(200);

            Go();
        }
    }

    private void AvoidWall()
    {
        double margin = 100;

        if (X < margin || X > ArenaWidth - margin ||
            Y < margin || Y > ArenaHeight - margin)
        {
            moveDirection *= -1;

            SetTurnRight(90);
            SetForward(150);
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        // Pilih musuh dengan energi paling rendah
        if (targetId == -1 || e.Energy < targetEnergy)
        {
            targetId = e.ScannedBotId;
            targetEnergy = e.Energy;
        }

        // Fokus hanya ke target terpilih
        if (e.ScannedBotId == targetId)
        {
            targetEnergy = e.Energy;

            // Arahkan body ke musuh
            double enemyDirection = DirectionTo(e.X, e.Y);
            double turn = enemyDirection - Direction;

            // Orbit target
            SetTurnRight(turn + 45);
            SetForward(120 * moveDirection);

            // Power tembakan berdasarkan jarak
            double distance = DistanceTo(e.X, e.Y);

            if (GunHeat == 0)
            {
                if (distance < 150)
                    Fire(3);
                else if (distance < 300)
                    Fire(2);
                else
                    Fire(1);
            }
        }

        Rescan();
    }

    public override void OnBotDeath(BotDeathEvent e)
    {
        // Reset target jika target mati
        if (e.VictimId == targetId)
        {
            targetId = -1;
            targetEnergy = double.MaxValue;
        }
    }

    public override void OnHitBot(HitBotEvent e)
    {
        if (GunHeat == 0)
        {
            Fire(3);
        }

        moveDirection *= -1;

        Back(100);
        SetTurnRight(45);
    }

    public override void OnHitWall(HitWallEvent e)
    {
        moveDirection *= -1;

        Back(150);
        SetTurnRight(90);

        Go();
    }
}
