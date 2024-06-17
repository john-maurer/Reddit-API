using RedditClient;

namespace UnitTests.Fixtures
{
    sealed public class RedditClientFixture : AbstractClientFixture
    {
        protected override void Arrange(params object[] parameters)
        {
            base.Arrange(parameters);

            OkContext = new RedItClient(OKClient);
            UnauthorizedContext = new RedItClient(UnauthorizedClient);
            ForbiddenContext = new RedItClient(ForbiddenClient);
            ProxyRequiredContext = new RedItClient(ProxyRequiredClient);
        }

        public RedditClientFixture() => Arrange();

        public RedItClient OkContext { get; private set; }
        public RedItClient UnauthorizedContext { get; private set; }
        public RedItClient ForbiddenContext { get; private set; }
        public RedItClient ProxyRequiredContext { get; private set; }
    }
}
