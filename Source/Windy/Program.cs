using StructureMap;
using System;
using Windy.DependencyInversion;
using Windy.Domain.Contracts.Managers;

namespace Windy
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new Container(new RuntimeRegistry());

            container.GetInstance<IWindPowerManager>().Start();

            Console.WriteLine($"Data transmitted and stored {DateTime.Now.ToString("dd MMM yyyy HH:mm")}");
        }
    }
}



