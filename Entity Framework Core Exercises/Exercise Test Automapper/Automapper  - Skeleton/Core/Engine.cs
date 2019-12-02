using Core.Contracts;
using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Core
{
    public class Engine : IEngine
    {

        private readonly IServiceProvider provider;
        public Engine(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public void Run()
        {
            while (true)
            {
                var inputArgs = Console.ReadLine()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

                if(inputArgs[0] == "Exit")
                {
                    break;
                }

                var commandInterpreter = this.provider.GetService<ICommandInterpreter>();
                var result = commandInterpreter.Read(inputArgs);

                Console.WriteLine(result);
            }
    
        }

    }
}
