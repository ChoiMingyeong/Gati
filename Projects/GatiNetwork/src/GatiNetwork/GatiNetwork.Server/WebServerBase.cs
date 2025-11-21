using System.Net;

namespace GatiNetwork.Server
{
    public abstract class WebServerBase<TWebServerArgs> : ServerBase<TWebServerArgs>
        where TWebServerArgs : WebServerArgs
    {
        protected readonly HttpListener _httpListener = new();

        public override void Initialize(in TWebServerArgs args)
        {
            _httpListener.Prefixes.Add($"http://+:{args.Port}/");
            _initialize = true;
        }

        public override async Task Run()
        {
            if (false == _initialize)
            {
                throw new Exception("Not Initialized.");
            }

            _httpListener.Start();
        }
    }
}
