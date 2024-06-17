﻿using RedditAuthorizationFilter.Services;
using UnitTests.Fixtures;

namespace UnitTests.Harnesses
{
    public class OktaServiceHarness : AbstractHarness<OktaServiceFixture>
    {
        protected override async Task<object?> Act(params object[] parameters)
        {
            TokenService service = (TokenService)parameters[0];

            return await service.GetToken();
        }

        public OktaServiceHarness(OktaServiceFixture filter) : base(filter)
        {

        }
    }
}
