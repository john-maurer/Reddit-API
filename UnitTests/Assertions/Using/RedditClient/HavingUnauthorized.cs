﻿using RedditClient.Verbs;
using System.Net;
using UnitTests.Fixtures;
using UnitTests.Harnesses;

namespace UnitTests.Assertions.Using.RedditClient
{
    public class HavingUnauthorized : ClientHarness<RedditClientFixture>
    {
        public HavingUnauthorized(RedditClientFixture fixture) : base(fixture)
        {

        }

        [Fact]
        public async Task ShouldGetWithoutException() =>
            Assert.Null(await Record.ExceptionAsync(() =>
                _fixture.UnauthorizedContext.SendRequestAsync<Get>("https://test/index.html", Payload)
            ));

        [Fact]
        public async Task ShouldGetSuccessfully()
        {
            var response = await _fixture.UnauthorizedContext.SendRequestAsync<Get>("https://test/index.html", Payload);

            Assert.False(response.IsSuccessStatusCode);
            response.StatusCode = HttpStatusCode.Unauthorized;
        }

        [Fact]
        public async Task ShouldPutWithoutException() =>
            Assert.Null(await Record.ExceptionAsync(() =>
                _fixture.UnauthorizedContext.SendRequestAsync<Put>("https://test/index.html", Payload)
            ));

        [Fact]
        public async Task ShouldPutSuccessfully()
        {
            var response = await _fixture.UnauthorizedContext.SendRequestAsync<Put>("https://test/index.html", Payload);

            Assert.False(response.IsSuccessStatusCode);
            response.StatusCode = HttpStatusCode.Unauthorized;
        }

        [Fact]
        public async Task ShouldPostWithoutException() =>
            Assert.Null(await Record.ExceptionAsync(() =>
                _fixture.UnauthorizedContext.SendRequestAsync<Post>("https://test/index.html", Payload)
            ));

        [Fact]
        public async Task ShouldPostSuccessfully()
        {
            var response = await _fixture.UnauthorizedContext.SendRequestAsync<Post>("https://test/index.html", Payload);

            Assert.False(response.IsSuccessStatusCode);
            response.StatusCode = HttpStatusCode.Unauthorized;
        }

        [Fact]
        public async Task ShouldDeleteWithoutException() =>
            Assert.Null(await Record.ExceptionAsync(() =>
                _fixture.UnauthorizedContext.SendRequestAsync<Delete>("https://test/index.html", Payload)
            ));

        [Fact]
        public async Task ShouldDeleteSuccessfully()
        {
            var response = await _fixture.UnauthorizedContext.SendRequestAsync<Delete>("https://test/index.html", Payload);

            Assert.False(response.IsSuccessStatusCode);
            response.StatusCode = HttpStatusCode.Unauthorized;
        }
    }
}
