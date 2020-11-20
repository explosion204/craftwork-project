using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CraftworkProject.Test.Mocks
{
    public class MockHttpSession : ISession
    {
        private readonly Dictionary<string, object> _sessionStorage = new Dictionary<string, object>();
        public object this[string name]
        {
            get => _sessionStorage[name];
            set => _sessionStorage[name] = value;
        }

        string ISession.Id => throw new NotImplementedException();

        bool ISession.IsAvailable => throw new NotImplementedException();

        IEnumerable<string> ISession.Keys => _sessionStorage.Keys;

        void ISession.Clear()
        {
            _sessionStorage.Clear();
        }
        
        Task ISession.CommitAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task ISession.LoadAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        void ISession.Remove(string key)
        {
            _sessionStorage.Remove(key);
        }

        void ISession.Set(string key, byte[] value)
        {
            _sessionStorage[key] = Encoding.ASCII.GetString(value);
        }
        

        bool ISession.TryGetValue(string key, out byte[] value)
        {
            if (_sessionStorage[key] != null)
            {
                value = Encoding.ASCII.GetBytes(_sessionStorage[key].ToString() ?? string.Empty);
                return true;
            }

            value = null;
            return false;
        }
    }
}