﻿using Newtonsoft.Json;
using UnitTests.Fixtures;

namespace UnitTests.Harnesses
{
    public struct Person
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public struct Payload
    {
        public string Token { get; set; }
        public string Json { get; set; }
    }

    public class ClientHarness<TContext> : AbstractHarness<TContext> where TContext : AbstractClientFixture
    {
        protected override async Task Act(params object[] parameters)
        {
            
        }

        protected Payload Payload { get; set; }

        public ClientHarness(TContext fixture) : base(fixture)
        {
            Payload = new Payload
            {
                Token = Guid.NewGuid().ToString(),
                Json = JsonConvert.SerializeObject(new Payload
                {
                    Token = Guid.NewGuid().ToString(),
                    Json = JsonConvert.SerializeObject(new Person { Id = Guid.NewGuid().ToString(), Name = "Person" })
                })
            };
        }
    }
}
