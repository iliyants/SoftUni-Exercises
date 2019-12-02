using Core.Commands.Contracts;
using Core.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Core
{
    public class CommandInterpreter : ICommandInterpreter
    {
        private const string Suffix = "Command";
        private readonly IServiceProvider provider;

        public CommandInterpreter(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public string Read(string[] inputArgs)
        {
            var command = inputArgs[0] + Suffix;

            var commandParams = inputArgs.Skip(1).ToArray();

            var type = Assembly.GetCallingAssembly()
                .GetTypes()
                .FirstOrDefault(x => x.Name == command);

            if(type == null)
            {
                throw new ArgumentNullException("Invalid command!");
            }

            var constructor = type.GetConstructors().FirstOrDefault();

            var constructoParams = constructor
                .GetParameters()
                .Select(x => x.ParameterType)
                .ToArray();

            var services = constructoParams
                .Select(this.provider.GetService)
                .ToArray();

            var invokeConstructor = (ICommand)constructor
               .Invoke(services);

            var result = invokeConstructor.Execute(commandParams);

            return result;
        }
    }
}
