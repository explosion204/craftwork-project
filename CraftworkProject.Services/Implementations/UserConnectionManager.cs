using System;
using System.Collections.Generic;
using System.Linq;
using CraftworkProject.Services.Interfaces;

namespace CraftworkProject.Services.Implementations
{
    public class UserConnectionManager : IUserConnectionManager
    {
        private static Dictionary<Guid, List<string>> _userConnectionMap = 
            new Dictionary<Guid, List<string>>();

        private static string _userConnectionMapLocker = string.Empty;
        
        public void KeepUserConnection(Guid userId, string connectionId)
        {
            lock (_userConnectionMapLocker)
            {
                if (!_userConnectionMap.ContainsKey(userId))
                {
                    _userConnectionMap[userId] = new List<string>();
                }
                
                _userConnectionMap[userId].Add(connectionId);
            }
        }

        public void RemoveUserConnection(string connectionId)
        {
            lock (_userConnectionMapLocker)
            {
                foreach (var userId in _userConnectionMap.Keys
                    .Where(userId => _userConnectionMap.ContainsKey(userId))
                    .Where(userId => _userConnectionMap[userId].Contains(connectionId)))
                {
                    _userConnectionMap[userId].Remove(connectionId);
                    break;
                }
            }
        }

        public List<string> GetUserConnections(Guid userId)
        {
            var conn = new List<string>();

            lock (_userConnectionMapLocker)
            {
                conn = _userConnectionMap[userId];
            }

            return conn;
        }
    }
}