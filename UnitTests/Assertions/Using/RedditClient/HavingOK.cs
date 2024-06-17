using UnitTests.Harnesses;
using UnitTests.Fixtures;
using RedditClient.Verbs;
using System.Net;

namespace UnitTests.Assertions.Using.RedditClient
{
    public class HavingOK : ClientHarness<RedditClientFixture>
    {
        public HavingOK(RedditClientFixture fixture) : base(fixture)
        {

        }

        [Fact]
        public async Task ShouldGetWithoutException() =>
            Assert.Null(await Record.ExceptionAsync(() =>
                _fixture.OkContext.SendRequestAsync<Get>("https://test/index.html", Payload)
            ));

        [Fact]
        public async Task ShouldGetSuccessfully()
        {
            var response = await _fixture.OkContext.SendRequestAsync<Get>("https://test/index.html", Payload);

            Assert.True(response.IsSuccessStatusCode);
            response.StatusCode = HttpStatusCode.OK;
        }

        [Fact]
        public async Task ShouldPutWithoutException() =>
            Assert.Null(await Record.ExceptionAsync(() =>
                _fixture.OkContext.SendRequestAsync<Put>("https://test/index.html", Payload)
            ));

        [Fact]
        public async Task ShouldPutSuccessfully()
        {
            var response = await _fixture.OkContext.SendRequestAsync<Put>("https://test/index.html", Payload);

            Assert.True(response.IsSuccessStatusCode);
            response.StatusCode = HttpStatusCode.OK;
        }

        [Fact]
        public async Task ShouldPostWithoutException() =>
            Assert.Null(await Record.ExceptionAsync(() =>
                _fixture.OkContext.SendRequestAsync<Post>("https://test/index.html", Payload)
            ));

        [Fact]
        public async Task ShouldPostSuccessfully()
        {
            var response = await _fixture.OkContext.SendRequestAsync<Post>("https://test/index.html", Payload);

            Assert.True(response.IsSuccessStatusCode);
            response.StatusCode = HttpStatusCode.OK;
        }

        [Fact]
        public async Task ShouldDeleteWithoutException() =>
            Assert.Null(await Record.ExceptionAsync(() =>
                _fixture.OkContext.SendRequestAsync<Delete>("https://test/index.html", Payload)
            ));

        [Fact]
        public async Task ShouldDeleteSuccessfully()
        {
            var response = await _fixture.OkContext.SendRequestAsync<Delete>("https://test/index.html", Payload);

            Assert.True(response.IsSuccessStatusCode);
            response.StatusCode = HttpStatusCode.OK;
        }
    }
}
