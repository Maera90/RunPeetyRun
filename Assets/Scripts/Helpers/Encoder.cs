using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Helpers
{
    public class Encoder
    {
        public string encodeString(string plain)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(plain);
            string encoded = Convert.ToBase64String(bytes);
            string pass = "lTuJryvmP2UotAIcg6Wnrbu9pkvzyShJzCgRYB1DuLPLboNfkyszxLyMVUbiAjPZJWGvlxWD6Cf7QQbYcroA0KcMscSSjxIQjc9vyVWun1s1GrQGI26zA79T7RBee9Sm";

            string finalString = encoded+pass;
            return finalString;
        }
    }
}
