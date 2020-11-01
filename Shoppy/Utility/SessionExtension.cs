using Microsoft.AspNetCore.Http;
//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shoppy.Utility
{
    public static class  SessionExtension
    {
        public static void Set<T>( this ISession session,string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        } 
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            
            if(value == null)
            {
                return default;
            }
            else
            {
              return  JsonSerializer.Deserialize<T>(value);
            }

           //return  value ==null? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
