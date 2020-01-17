namespace SIS.HTTP.Sessions
{
    using System.Collections.Generic;
    using System.Text;
    using SIS.HTTP.Common;
    using SIS.HTTP.Sessions.Contracts;

    public class HttpSession : IHttpSession
    {
        private readonly Dictionary<string, object> sessionParameters;

        public HttpSession(string id)
        {
            this.Id = id;
            this.sessionParameters = new Dictionary<string, object>();
        }

        public string Id { get; }

        public object GetParameter(string parameterName)
        {
            CoreValidator.ThrowIfNullOrEmpty(parameterName, nameof(parameterName));

            // TODO: Validation for existing parameter (maybe throw exception)

            return this.sessionParameters[parameterName];
        }

        public bool ContainsParameter(string parameterName)
        {
            CoreValidator.ThrowIfNullOrEmpty(parameterName, nameof(parameterName));

            return this.sessionParameters.ContainsKey(parameterName);
        }

        public void AddParameter(string parameterName, object parameter)
        {
            CoreValidator.ThrowIfNullOrEmpty(parameterName, nameof(parameterName));
            CoreValidator.ThrowIfNull(parameter, nameof(parameter));

            this.sessionParameters[parameterName] = parameter;
        }

        public void ClearParameters()
        {
            this.sessionParameters.Clear();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var parameter in this.sessionParameters)
            {
                sb.AppendLine($"{parameter.Key} - {parameter.Value.ToString()}");
            }

            return sb.ToString();
        }
    }
}
