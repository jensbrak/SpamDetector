using System;
using Piranha;
using Piranha.Models;
using Piranha.Services;

namespace Zon3.SpamDetector
{
    public class SpamDetectorConfig : IDisposable
    {
        private readonly IParamService _service;

        public static readonly string ENABLED = "Enabled";
        public static readonly string IS_TEST = "IsTest";
        public static readonly string SPAM_API_URL = "SpamApiUrl";
        public static readonly string SITE_URL = "SiteUrl";
        public static readonly string SITE_LANGUAGE = "SiteLanguage";
        public static readonly string SITE_ENCODING = "SiteEncoding";
        public static readonly string USER_ROLE = "UserRole";

        public SpamDetectorConfig(IParamService paramService)
        {
            _service = paramService;
        }

        public SpamDetectorConfig(IApi api)
        {
            _service = api.Params;
        }

        public bool Enabled
        {
            get
            {
                var param = _service.GetByKeyAsync(ENABLED).GetAwaiter().GetResult();
                return param != null && Convert.ToBoolean(param.Value);
            }
            set
            {
                var param = _service.GetByKeyAsync(ENABLED).GetAwaiter().GetResult() ?? new Param
                {
                    Key = ENABLED
                };

                param.Value = value.ToString();
                _service.SaveAsync(param).GetAwaiter().GetResult();
            }
        }

        public string SiteUrl
        {
            get
            {
                var param = _service.GetByKeyAsync(SITE_URL).GetAwaiter().GetResult();
                return param?.Value;
            }
            set
            {
                var param = _service.GetByKeyAsync(SITE_URL).GetAwaiter().GetResult() ?? new Param
                {
                    Key = SITE_URL
                };

                // Ensure trailing slash
                if (!string.IsNullOrWhiteSpace(value) && !value.EndsWith("/"))
                    value += "/";

                param.Value = value;
                _service.SaveAsync(param).GetAwaiter().GetResult();
            }
        }

        public string SiteLanguage
        {
            get
            {
                var param = _service.GetByKeyAsync(SITE_LANGUAGE).GetAwaiter().GetResult();
                return param?.Value;
            }
            set
            {
                var param = _service.GetByKeyAsync(SITE_LANGUAGE).GetAwaiter().GetResult() ?? new Param
                {
                    Key = SITE_LANGUAGE
                };

                param.Value = value;
                _service.SaveAsync(param).GetAwaiter().GetResult();
            }
        }

        public string SiteEncoding
        {
            get
            {
                var param = _service.GetByKeyAsync(SITE_ENCODING).GetAwaiter().GetResult();
                return param?.Value;
            }
            set
            {
                var param = _service.GetByKeyAsync(SITE_ENCODING).GetAwaiter().GetResult() ?? new Param
                {
                    Key = SITE_ENCODING
                };

                param.Value = value;
                _service.SaveAsync(param).GetAwaiter().GetResult();
            }
        }

        public string UserRole
        {
            get
            {
                var param = _service.GetByKeyAsync(USER_ROLE).GetAwaiter().GetResult();
                return param?.Value;
            }
            set
            {
                var param = _service.GetByKeyAsync(USER_ROLE).GetAwaiter().GetResult() ?? new Param
                {
                    Key = USER_ROLE
                };

                param.Value = value;
                _service.SaveAsync(param).GetAwaiter().GetResult();
            }
        }

        public string SpamApiUrl
        {
            get
            {
                var param = _service.GetByKeyAsync(SPAM_API_URL).GetAwaiter().GetResult();
                return param?.Value;
            }
            set
            {
                var param = _service.GetByKeyAsync(SPAM_API_URL).GetAwaiter().GetResult() ?? new Param
                {
                    Key = SPAM_API_URL
                };

                // Ensure trailing slash
                if (!string.IsNullOrWhiteSpace(value) && !value.EndsWith("/"))
                    value += "/";

                param.Value = value;
                _service.SaveAsync(param).GetAwaiter().GetResult();
            }
        }

        public bool IsTest
        {
            get
            {
                var param = _service.GetByKeyAsync(IS_TEST).GetAwaiter().GetResult();
                return param != null && Convert.ToBoolean(param.Value);
            }
            set
            {
                var param = _service.GetByKeyAsync(IS_TEST).GetAwaiter().GetResult() ?? new Param
                {
                    Key = IS_TEST
                };

                param.Value = value.ToString();
                _service.SaveAsync(param).GetAwaiter().GetResult();
            }
        }

        public void Dispose()
        {
        }
    }
}
