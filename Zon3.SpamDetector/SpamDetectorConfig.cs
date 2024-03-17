using Piranha;
using Piranha.Models;
using Piranha.Services;
using System;

namespace Zon3.SpamDetector
{
    public class SpamDetectorConfig : IDisposable
    {
        private readonly IParamService _service;

        public static readonly string KeyPrefix = "SpamDetector_";
        public static readonly string KeyEnabled = $"{KeyPrefix}Enabled";
        public static readonly string KeyIsTest = $"{KeyPrefix}IsTest";
        public static readonly string KeySpamApiUrl = $"{KeyPrefix}SpamApiUrl";
        public static readonly string KeySiteUrl = $"{KeyPrefix}SiteUrl";
        public static readonly string KeySiteLanguage = $"{KeyPrefix}SiteLanguage";
        public static readonly string KeySiteEncoding = $"{KeyPrefix}SiteEncoding";
        public static readonly string KeyUserRole = $"{KeyPrefix}UserRole";

        private static readonly bool DefaultValueEnabled = false;
        private static readonly bool DefaultValueIsTest = true;
        private static readonly string DefaultValueSpamApiUrl = "";
        private static readonly string DefaultValueSiteUrl = "";
        private static readonly string DefaultValueSiteLanguage = "en-US";
        private static readonly string DefaultValueSiteEncoding = "UTF8";
        private static readonly string DefaultValueUserRole = "guest";

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
                var param = _service.GetByKeyAsync(KeyEnabled).GetAwaiter().GetResult();
                return param == null ? DefaultValueEnabled : Convert.ToBoolean(param.Value);
            }
            set
            {
                var param = _service.GetByKeyAsync(KeyEnabled).GetAwaiter().GetResult() ?? new Param
                {
                    Key = KeyEnabled
                };

                param.Value = value.ToString();
                _service.SaveAsync(param).GetAwaiter().GetResult();
            }
        }

        public bool IsTest
        {
            get
            {
                var param = _service.GetByKeyAsync(KeyIsTest).GetAwaiter().GetResult();
                return param == null ? DefaultValueIsTest : Convert.ToBoolean(param.Value);
            }
            set
            {
                var param = _service.GetByKeyAsync(KeyIsTest).GetAwaiter().GetResult() ?? new Param
                {
                    Key = KeyIsTest
                };

                param.Value = value.ToString();
                _service.SaveAsync(param).GetAwaiter().GetResult();
            }
        }

        public string SpamApiUrl
        {
            get
            {
                var param = _service.GetByKeyAsync(KeySpamApiUrl).GetAwaiter().GetResult();
                return param == null ? DefaultValueSpamApiUrl : param.Value;
            }
            set
            {
                var param = _service.GetByKeyAsync(KeySpamApiUrl).GetAwaiter().GetResult() ?? new Param
                {
                    Key = KeySpamApiUrl
                };

                param.Value = value;
                _service.SaveAsync(param).GetAwaiter().GetResult();
            }
        }

        public string SiteUrl
        {
            get
            {
                var param = _service.GetByKeyAsync(KeySiteUrl).GetAwaiter().GetResult();
                return param == null ? DefaultValueSiteUrl : param.Value;
            }
            set
            {
                var param = _service.GetByKeyAsync(KeySiteUrl).GetAwaiter().GetResult() ?? new Param
                {
                    Key = KeySiteUrl
                };

                param.Value = value;
                _service.SaveAsync(param).GetAwaiter().GetResult();
            }
        }

        public string SiteLanguage
        {
            get
            {
                var param = _service.GetByKeyAsync(KeySiteLanguage).GetAwaiter().GetResult();
                return param == null ? DefaultValueSiteLanguage : param.Value;
            }
            set
            {
                var param = _service.GetByKeyAsync(KeySiteLanguage).GetAwaiter().GetResult() ?? new Param
                {
                    Key = KeySiteLanguage
                };

                param.Value = value;
                _service.SaveAsync(param).GetAwaiter().GetResult();
            }
        }

        public string SiteEncoding
        {
            get
            {
                var param = _service.GetByKeyAsync(KeySiteEncoding).GetAwaiter().GetResult();
                return param == null ? DefaultValueSiteEncoding : param.Value;
            }
            set
            {
                var param = _service.GetByKeyAsync(KeySiteEncoding).GetAwaiter().GetResult() ?? new Param
                {
                    Key = KeySiteEncoding
                };

                param.Value = value;
                _service.SaveAsync(param).GetAwaiter().GetResult();
            }
        }

        public string UserRole
        {
            get
            {
                var param = _service.GetByKeyAsync(KeyUserRole).GetAwaiter().GetResult();
                return param == null ? DefaultValueUserRole: param.Value;
            }
            set
            {
                var param = _service.GetByKeyAsync(KeyUserRole).GetAwaiter().GetResult() ?? new Param
                {
                    Key = KeyUserRole
                };

                param.Value = value;
                _service.SaveAsync(param).GetAwaiter().GetResult();
            }
        }

        public void Dispose()
        {
        }
    }
}
