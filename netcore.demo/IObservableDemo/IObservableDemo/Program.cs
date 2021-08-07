using IObservableDemo.subs;
using System;

namespace IObservableDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //LocationTracker provider = new();
            //LocationReporter reporter1 = new("FixedGPS");

            //reporter1.Subscribe(provider);

            //LocationReporter reporter2 = new("mobileGPS");
            //reporter2.Subscribe(provider);

            //provider.TrackLocation(new Location(47.6456, -122.1312));
            //reporter1.Unsubscribe();
            //provider.TrackLocation(new Location(47.6677, -122.1199));
            //provider.TrackLocation(null);
            //provider.EndTransmission();


            BaggageHandler provider = new BaggageHandler();
            ArrivalsMonitor observer1 = new ArrivalsMonitor("BaggageClaimMonitor1");
            ArrivalsMonitor observer2 = new ArrivalsMonitor("SecurityExit");

            provider.BaggageStatus(712, "Detroit", 3);
            observer1.Subscribe(provider);
            provider.BaggageStatus(712, "Kalamazoo", 3);
            provider.BaggageStatus(400, "New York-Kennedy", 1);
            provider.BaggageStatus(712, "Detroit", 3);
            observer2.Subscribe(provider);
            provider.BaggageStatus(511, "San Francisco", 2);
            provider.BaggageStatus(712);
            observer2.Unsubscribe();
            provider.BaggageStatus(400);
            provider.LastBaggageClaimed();

            Console.Read();
        }
    }
}
