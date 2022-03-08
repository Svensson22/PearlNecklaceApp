using System;
using System.Collections.Generic;
using System.Linq;

namespace PearlNecklaceApp
{

    public enum Color
    {
        Black,
        White,
        Pink
    }

    public enum Shape
    {
        Round,
        Teardrop
    }

    public enum Source
    {
        Freshwater,
        Saltwater
    }

    public class Pearl
    {
        private int diameter;

        public int Diameter
        {
            get { return diameter; }
            set
            {
                // Apply some basic size constraints to the setter
                if (value < 5)
                {
                    diameter = 5;
                }

                if (value > 25)
                {
                    diameter = 25;
                }

                diameter = value;
            }
        }

        public Color Color { get; set; }
        public Shape Shape { get; set; }
        public Source Source { get; set; }

        // This should not need a setter as it relies on diameter and fixed costs
        public int Price
        {
            get
            {
                // 50kr per mm in diameter for Freshwater sourced pearls
                if (Source == Source.Freshwater)
                {
                    return Diameter * 50;
                }

                // 100kr per mm in diameter for Saltwater sourced pearls
                else
                {
                    return Diameter * 100;
                }
            }
        }

        public override string ToString()
        {
            return $"{Color} {Shape} pearl sourced from {Source} is {Diameter}mm in diameter and costs {Price}kr";
        }
    }

    public class Necklace
    {
        public List<Pearl> PearlList = new List<Pearl>();
        private int price;
        public int Price
        {
            // Calculate total value on request
            get
            {
                // Clear between requests
                price = 0;

                foreach (Pearl p in PearlList)
                {
                    price += p.Price;
                }

                return price;
            }
        }

        // Automatic generation of 35 pearls by the constructor
        public Necklace()
        {
            for (int i = 0; i < 35; i++)
            {
                var pearl = RandomPearl();
                PearlList.Add(pearl);
            }

            // Adding the test pearl just to see if it works
            //Pearl findMe = new Pearl
            //{
            //    Shape = Shape.Round,
            //    Color = Color.White,
            //    Diameter = 15,
            //    Source = Source.Freshwater
            //};

            //PearlList.Add(findMe);
        }

        // Count totals of both shapes and print it out
        public void GetShapes()
        {
            int teardrop = PearlList.Where(p => p.Shape == Shape.Teardrop).Count();
            int round = PearlList.Where(p => p.Shape == Shape.Round).Count();

            Console.WriteLine($"This necklace has {teardrop} teardrops and {round} round pearls.");
        }

        // Print details for each individual pearl
        public void GetDetails()
        {
            foreach (Pearl p in PearlList)
            {
                Console.WriteLine(p);
            }
        }

        // Get the total cost of all pearls and print it out
        public void GetCost()
        {
            Console.WriteLine($"Total cost of the necklace is {Price}kr.");
        }

        // Generate a randomized pearl with size, shape, color and source
        public static Pearl RandomPearl()
        {
            var random = new Random();

            // Fetch the enumerators
            Array shapes = Enum.GetValues(typeof(Shape));
            Array colors = Enum.GetValues(typeof(Color));
            Array sources = Enum.GetValues(typeof(Source));

            // Pick random values for shape, source and color
            Shape randShape = (Shape)shapes.GetValue(random.Next(shapes.Length));
            Color randColor = (Color)colors.GetValue(random.Next(colors.Length));
            Source randSource = (Source)sources.GetValue(random.Next(sources.Length));

            // Randomize the diameter of the pearl
            int randSize = random.Next(5, 25);

            Pearl pearl = new Pearl
            {
                Diameter = randSize,
                Color = randColor,
                Shape = randShape,
                Source = randSource
            };

            // Briefly for testing
            //Console.WriteLine($"Generated {pearl}.");

            return pearl;
        }

        public void FindPearl(Pearl findMe)
        {
            Console.WriteLine($"Trying to find a match for {findMe}.");

            var result = PearlList.Find(
                p => p.Shape == findMe.Shape &&
                p.Color == findMe.Color &&
                p.Diameter == findMe.Diameter &&
                p.Source == findMe.Source);

            if (result != null)
            {
                var index = PearlList.IndexOf(result);
                Console.WriteLine($"Match found at #{index}: {result}.");
            }

            else
            {
                Console.WriteLine("No matching pearl found.");
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Generating a test necklace!");

            var necklace = new Necklace();

            Console.WriteLine("Shape count:");
            necklace.GetShapes();

            Console.WriteLine("More details:");
            necklace.GetDetails();

            var sortedPearls = necklace.PearlList
                .OrderBy(d => d.Diameter)
                .ThenBy(c => c.Color)
                .ThenBy(s => s.Shape)
                .ToList();

            Console.WriteLine("Sorted pearls:");

            foreach (Pearl p in sortedPearls)
            {
                Console.WriteLine(p);
            }

            necklace.GetCost();

            // Test pearl to try and find
            Pearl findMe = new Pearl
            {
                Shape = Shape.Round,
                Color = Color.White,
                Diameter = 15,
                Source = Source.Freshwater
            };

            // Broke it out into a method for readability
            necklace.FindPearl(findMe);
        }
    }
}
