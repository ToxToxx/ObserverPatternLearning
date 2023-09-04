using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserverPatternLearning
{
    class Program
    {
        static void Main(string[] args)
        {
            ResourceAuction resourceAuction = new ResourceAuction();
            Bank bank = new Bank("Kings Bank", resourceAuction);
            Player player = new Player("Palladin123", resourceAuction);
            // auction immitation
            resourceAuction.StartAuction();
           
            player.StopTrade();
           
            resourceAuction.StartAuction();

            Console.Read();
        }
    }

    //observer interface
    interface IObserver
    {
        void Update(Object ob);
    }

    //interface of observable object
    interface IObservable
    {
        void RegisterObserver(IObserver o);
        void RemoveObserver(IObserver o);
        void NotifyObservers();
    }
    //concrete observable
    class ResourceAuction : IObservable
    {
        AuctionInfo resourceMarketInfo; 

        List<IObserver> observers;
        public ResourceAuction()
        {
            observers = new List<IObserver>();
            resourceMarketInfo = new AuctionInfo();
        }
        public void RegisterObserver(IObserver observer)
        {
            observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach (IObserver observer in observers)
            {
                observer.Update(resourceMarketInfo);
            }
        }

        public void StartAuction()
        {
            Console.WriteLine("The auction starts");
            Random rnd = new Random();
            resourceMarketInfo.GoldCoins = rnd.Next(5, 40);
            resourceMarketInfo.SilverOre = rnd.Next(30, 99);
            NotifyObservers();
        }
    }

    class AuctionInfo
    {
        public int GoldCoins { get; set; }
        public int SilverOre { get; set; }
    }

    //concrete observer
    class Player : IObserver
    {
        public string Name { get; set; }
        IObservable _observable;
        public Player(string name, IObservable observable)
        {
            this.Name = name;
            _observable = observable;
            _observable.RegisterObserver(this);
        }
        public void Update(object observable)
        {
            AuctionInfo auctionInfo = (AuctionInfo)observable;

            if (auctionInfo.SilverOre > 5)
            {
                Console.WriteLine($"Player: {this.Name}  sells silver ore for {auctionInfo.GoldCoins} gold coins ");
                Console.WriteLine($"Player: {this.Name}  sells gold  for {auctionInfo.SilverOre} silver ore");
            }              
            else
                Console.WriteLine($"Player: {this.Name}  sells gold  for {auctionInfo.SilverOre} silver ore");
        }
        public void StopTrade()
        {
            _observable.RemoveObserver(this);
            _observable = null;
            Console.WriteLine($"{this.Name} left auction");
        }
    }

    class Bank : IObserver
    {
        private float _bankPercentage = 0.1f;
        public string Name { get; set; }
        IObservable _observable;
        public Bank(string name, IObservable observable)
        {
            this.Name = name;
            _observable = observable;
            _observable.RegisterObserver(this);
        }
        public void Update(object observable)
        {
            AuctionInfo auctionInfo = (AuctionInfo)observable;


            if (auctionInfo.SilverOre > 10)
            {
                Console.WriteLine($"Bank: {this.Name}  sells silver ore for {auctionInfo.GoldCoins + (auctionInfo.GoldCoins * this._bankPercentage)} gold coins ");
                Console.WriteLine($"Bank: {this.Name}  sells gold  for {auctionInfo.SilverOre + (auctionInfo.GoldCoins * this._bankPercentage)} silver ore");
            }                
            else
                Console.WriteLine($"Bank: {this.Name}  sells gold  for {auctionInfo.SilverOre + (auctionInfo.GoldCoins * this._bankPercentage)} silver ore");
        }
    }
}
